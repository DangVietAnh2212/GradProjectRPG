using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    //bool itemOnMouse = false;

    public float sizeOfSlot = 90f;

    public InventorySO inventory;

    protected Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateInventorySlots();
        /*AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });*/
    }

    public void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.inventoryItem.ID >= 0)
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemSO.uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount == 1 ? "" : slot.amount.ToString("n0");
        }
        else
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    public abstract void CreateInventorySlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var evenTrigger = new EventTrigger.Entry();
        evenTrigger.eventID = type;
        evenTrigger.callback.AddListener(action);
        trigger.triggers.Add(evenTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoverOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoverOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceWhenMouseIsOver = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        //Unity has bug
        //MouseData.interfaceWhenMouseIsOver = null;

    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].inventoryItem.ID >= 0)
        {
            tempItem = new GameObject();
            tempItem.transform.SetParent(transform.parent.transform.parent);//make the canvas the parent of the image
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemSO.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.interfaceWhenMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if(MouseData.slotHoverOver != null)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceWhenMouseIsOver.slotsOnInterface[MouseData.slotHoverOver];
            inventory.SwapItemInSlot(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnClick(GameObject obj)
    {
        if (MouseData.itemOnMouseDisplay == null)
        {
            MouseData.itemOnMouseDisplay = CreateTempItem(obj);
            if (MouseData.itemOnMouseDisplay != null)
            {
                MouseData.itemOnMouseRef = obj;
                MouseData.colorOfItemOnMouse = MouseData.itemOnMouseRef.GetComponentInChildren<Image>().color;
                MouseData.itemOnMouseRef.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.5f);
                MouseData.itemOnMouse = true;
                MouseData.tempSlot = slotsOnInterface[obj];
                return;
            }
        }
        else if (MouseData.itemOnMouseDisplay != null)
        {
            MouseData.itemOnMouseRef.GetComponentInChildren<Image>().color = MouseData.colorOfItemOnMouse;
            MouseData.itemOnMouse = false;
            Destroy(MouseData.itemOnMouseDisplay);
            if (MouseData.tempSlot != null)
            {
                inventory.SwapItemInSlot(slotsOnInterface[obj], MouseData.tempSlot);
                MouseData.tempSlot = null;
            }
        }
    }
    private void Update()
    {
        if (MouseData.itemOnMouse == true && MouseData.itemOnMouseDisplay)
        {
            MouseData.itemOnMouseDisplay.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void ClearItemOnMouse()
    {
        if(MouseData.itemOnMouse == true)
        {
            MouseData.itemOnMouseRef.GetComponentInChildren<Image>().color = MouseData.colorOfItemOnMouse;
            MouseData.itemOnMouse = false;
            Destroy(MouseData.itemOnMouseDisplay);
        }
    }
}

public static class MouseData
{
    public static UserInterface interfaceWhenMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoverOver;
    //this part is not doing anything right now

    public static GameObject itemOnMouseDisplay;
    public static Color colorOfItemOnMouse;
    public static GameObject itemOnMouseRef;
    public static InventorySlot tempSlot;
    public static bool itemOnMouse = false;
}

