using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();

    public float sizeOfSlot = 90f;

    public GameObject slotPrefab;

    public InventorySO inventory;

    public float xStart;
    public float yStart;

    public float xSpaceBetweenSlot;
    public float ySpaceBetweenSlot;

    public int columnNum;

    Dictionary<GameObject, InventorySlot> displayDictionary = new Dictionary<GameObject, InventorySlot>();
    // Start is called before the first frame update
    void Start()
    {
        CreateInventorySlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlotsDisplay();
    }

    public void UpdateSlotsDisplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in displayDictionary)
        {
            if (slot.Value.slotID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.itemDatabase.GetItem[slot.Value.inventoryItem.ID].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void CreateInventorySlots()
    {
        displayDictionary = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.slotsContainer.itemSlots.Length; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetSlotPositionByIndex(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            displayDictionary.Add(obj, inventory.slotsContainer.itemSlots[i]);
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var evenTrigger = new EventTrigger.Entry();
        evenTrigger.eventID = type;
        evenTrigger.callback.AddListener(action);
        trigger.triggers.Add(evenTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (displayDictionary.ContainsKey(obj))
        {
            mouseItem.hoverSlot = displayDictionary[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverSlot = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rectTransform = mouseObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(sizeOfSlot, sizeOfSlot);
        mouseObject.transform.SetParent(transform.parent);
        if (displayDictionary[obj].slotID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.itemDatabase.GetItem[displayDictionary[obj].slotID].uiDisplay;
            img.raycastTarget = false;
        }

        mouseItem.obj = mouseObject;
        mouseItem.item = displayDictionary[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj != null)
        {
            inventory.SwapItemInSlot(displayDictionary[obj], displayDictionary[mouseItem.hoverObj]);
        }
        else
        {
            //if drag item out to the ground, destroy it (can be use to spawn it out)
            inventory.RemoveItem(displayDictionary[obj].inventoryItem);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    public Vector3 GetSlotPositionByIndex(int index)
    {
        return new Vector3
            (
            xStart + xSpaceBetweenSlot * (index % columnNum),
            yStart - ySpaceBetweenSlot * (index / columnNum),
            0f
            );
    }
}

public class MouseItem
{
    public GameObject obj;
    public GameObject hoverObj;
    public InventorySlot item;
    public InventorySlot hoverSlot;
}

