using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;

    public InventorySO inventory;

    //public GameObject slotPrefab;

    public float xStart;
    public float yStart;

    public float xSpaceBetweenSlot;
    public float ySpaceBetweenSlot;

    public int columnNum;

    Dictionary<InventorySlot, GameObject> displayDictionary = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.slotsContainer.itemSlots.Count; i++)
        {
            ArrangeSlotByIndex(i);
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.slotsContainer.itemSlots.Count; i++)
        {
            if (displayDictionary.ContainsKey(inventory.slotsContainer.itemSlots[i]))
            {
                displayDictionary[inventory.slotsContainer.itemSlots[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.slotsContainer.itemSlots[i].amount.ToString("n0");
            }
            else
                ArrangeSlotByIndex(i);
        }
    }

    public void ArrangeSlotByIndex(int index)
    {
        InventorySlot slot = inventory.slotsContainer.itemSlots[index];
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.itemDatabase.GetItem[slot.inventoryItem.ID].uiDisplay;
        obj.GetComponent<RectTransform>().localPosition = GetSlotPositionByIndex(index);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        displayDictionary.Add(slot, obj);
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
