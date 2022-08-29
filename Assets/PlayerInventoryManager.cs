using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public InventorySO inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Saving");
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            Debug.Log("Loading");
            inventory.Load();
        }

    }

    private void OnApplicationQuit()
    {
        inventory.slotsContainer.itemSlots = new InventorySlot[Inventory.inventorySlotNum];
    }
}
