using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    bool hasInventoryInitiated = false;
    public GameObject inventoryPanel;
    public InventorySO inventory;
    public InventorySO equipmentInventory;
    public AttributeManager[] attributeTypes;

    private Transform equippedHelmet;
    private Transform equippedBodyArmor;
    private Transform equippedGloves;
    private Transform equippedBoots;
    private Transform equippedSpellBook;


    private BoneCombiner boneCombiner;

    private void Start()
    {
        boneCombiner = new BoneCombiner(gameObject);
        for (int i = 0; i < attributeTypes.Length; i++)
        {
            attributeTypes[i].SetParent(this);
        }

        for (int i = 0; i < equipmentInventory.GetSlots.Length; i++)
        {
            equipmentInventory.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipmentInventory.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
    }

    public void OnRemoveItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.ItemSO == null)
        {
            //print("no item found:" + inventorySlot);
            return;
        }

        switch (inventorySlot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                //Debug.Log(string.Concat("Remove ", inventorySlot.ItemSO, "  on", inventorySlot.parent.inventory.interfaceType, ", Allowed Items: ", string.Join(", ", inventorySlot.allowedItems)));
                for (int i = 0; i < inventorySlot.inventoryItem.buffs.Length; i++)
                {
                    for (int j = 0; j < attributeTypes.Length; j++)
                    {
                        if (attributeTypes[j].attributeType == inventorySlot.inventoryItem.buffs[i].attributeType)
                            attributeTypes[j].value.RemoveModifier(inventorySlot.inventoryItem.buffs[i]);
                    }
                }

                if (inventorySlot.ItemSO.displayCharEquipment != null)
                {
                    switch (inventorySlot.allowedItems[0])
                    {
                        case ItemType.Helmet:
                            Destroy(equippedHelmet.gameObject);
                            break;
                        case ItemType.BodyArmour:
                            Destroy(equippedBodyArmor.gameObject);
                            break;
                        case ItemType.Glove:
                            Destroy(equippedGloves.gameObject);
                            break;
                        case ItemType.Boots:
                            Destroy(equippedBoots.gameObject);
                            break;
                        case ItemType.SpellBook:
                            Destroy(equippedSpellBook.gameObject);
                            break;
                    }
                }
                break;
            default:
                break;
        }

    }

    public void OnAddItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.ItemSO == null)
        {
            //print("no item found:" + inventorySlot);
            return;
        }

        switch (inventorySlot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                //Debug.Log(string.Concat("Placed ", inventorySlot.ItemSO, "on ", inventorySlot.parent.inventory.interfaceType, ", Allowed Items: ", string.Join(", ", inventorySlot.allowedItems)));

                for (int i = 0; i < inventorySlot.inventoryItem.buffs.Length; i++)
                {
                    for (int j = 0; j < attributeTypes.Length; j++)
                    {
                        if (attributeTypes[j].attributeType == inventorySlot.inventoryItem.buffs[i].attributeType)
                            attributeTypes[j].value.AddModifier(inventorySlot.inventoryItem.buffs[i]);
                    }
                }

                if(inventorySlot.ItemSO.displayCharEquipment != null)
                {
                    switch (inventorySlot.allowedItems[0])
                    {
                        case ItemType.Helmet:
                            equippedHelmet = boneCombiner.AddLimb(inventorySlot.ItemSO.displayCharEquipment);
                            break;
                        case ItemType.BodyArmour:
                            equippedBodyArmor = boneCombiner.AddLimb(inventorySlot.ItemSO.displayCharEquipment);
                            break;
                        case ItemType.Glove:
                            equippedGloves = boneCombiner.AddLimb(inventorySlot.ItemSO.displayCharEquipment);
                            break;
                        case ItemType.Boots:
                            equippedBoots = boneCombiner.AddLimb(inventorySlot.ItemSO.displayCharEquipment);
                            break;
                        case ItemType.SpellBook:
                            equippedSpellBook = boneCombiner.AddLimb(inventorySlot.ItemSO.displayCharEquipment);
                            break;
                    }
                }

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

    private void LateUpdate()
    {
        if (!hasInventoryInitiated)
        {
            inventory.Load();
            equipmentInventory.Load();
            inventoryPanel.SetActive(false);
            hasInventoryInitiated = true;
        }
    }

    public void AttributeModifierNotice(AttributeManager attribute)
    {
        //Debug.Log(string.Concat(attribute.attributeType, " was updated! The value is now:  ", attribute.value.ModifiedValue));
        
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipmentInventory.Clear();
    }
}
