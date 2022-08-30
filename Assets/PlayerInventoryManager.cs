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
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            inventory.Load();
        }

    }

    private void OnApplicationQuit()
    {
        inventory.slotsContainer.itemSlots = new InventorySlot[Inventory.inventorySlotNum];
    }
}
