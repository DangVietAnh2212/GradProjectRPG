using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    //public MouseItem mouseItem = new MouseItem();

    public InventorySO inventory;
    public InventorySO equipmentInventory;
    public Attributes[] attributes;

    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipmentInventory.GetSlots.Length; i++)
        {
            equipmentInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipmentInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot inventorySlot)
    {
        if (inventorySlot.ItemSO == null)
            return;

        switch (inventorySlot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                Debug.Log(string.Concat("Remove ", inventorySlot.ItemSO, "  on", inventorySlot.parent.inventory.interfaceType, ", Allowed Items: ", string.Join(", ", inventorySlot.allowedItems)));
                for (int i = 0; i < inventorySlot.inventoryItem.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].attributeType == inventorySlot.inventoryItem.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(inventorySlot.inventoryItem.buffs[i]);
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }

    }

    public void OnAfterSlotUpdate(InventorySlot inventorySlot)
    {
        if (inventorySlot.ItemSO == null)
            return;
        switch (inventorySlot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                Debug.Log(string.Concat("Placed ", inventorySlot.ItemSO, "on ", inventorySlot.parent.inventory.interfaceType, ", Allowed Items: ", string.Join(", ", inventorySlot.allowedItems)));

                for (int i = 0; i < inventorySlot.inventoryItem.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].attributeType == inventorySlot.inventoryItem.buffs[i].attribute)
                            attributes[j].value.AddModifier(inventorySlot.inventoryItem.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
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

    public void AttributeModifierNotice(Attributes attribute)
    {
        Debug.Log(string.Concat(attribute.attributeType, " was updated! The value is now:  ", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipmentInventory.Clear();
    }
}
