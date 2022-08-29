using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject slotPrefab;

    public InventorySO inventory;

    //public GameObject slotPrefab;

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
        foreach(KeyValuePair<GameObject, InventorySlot> slot in displayDictionary)
        {
            if(slot.Value.ID >= 0)
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

            displayDictionary.Add(obj, inventory.slotsContainer.itemSlots[i]);
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
