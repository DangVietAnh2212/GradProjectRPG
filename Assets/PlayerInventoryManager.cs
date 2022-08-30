using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    //public MouseItem mouseItem = new MouseItem();

    public InventorySO inventory;
    public InventorySO equipmentInventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipmentInventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            inventory.Load();
            equipmentInventory.Load();
        }

    }

    private void OnApplicationQuit()
    {
        inventory.slotsContainer.Clear();
        equipmentInventory.slotsContainer.Clear();
    }
}
