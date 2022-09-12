using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    LifeRecover,
    ManaRecover, 
    Helmet,
    BodyArmour,
    Glove,
    Boots,
    SpellBook,
    Spell,
    Default
}


/// <summary>
/// Base class for all items in the game
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/Item")]
public class ItemSO : ScriptableObject
{
    public GameObject spellPrefab;
    public Sprite uiDisplay;
    public GameObject displayCharEquipment;
    public ItemType itemType;
    [TextArea(15, 20)]
    public string description;
    public Spell spell = new Spell();
    public InventoryItem inventoryItemData = new InventoryItem();
    public bool isStackable = false;
    public InventoryItem CreateItem()
    {
        InventoryItem newInventoryItem = new InventoryItem(this);
        return newInventoryItem;
    }
}
/// <summary>
/// Class to manager items in the inventory
/// </summary>
[System.Serializable]
public class InventoryItem
{
    //public bool isNew = true;
    public string name;
    public int ID = -1;
    public ItemBuff[] buffs;
    public bool isStackable;
    public Spell spell;

    public InventoryItem()
    {
        name = "";
        ID = -1;
    }
    public InventoryItem(ItemSO itemSO)
    {
        name = itemSO.name;
        ID = itemSO.inventoryItemData.ID;
        isStackable = itemSO.isStackable;

        spell = new Spell(itemSO);

        buffs = new ItemBuff[itemSO.inventoryItemData.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemSO.inventoryItemData.buffs[i].min, itemSO.inventoryItemData.buffs[i].max);
            buffs[i].attributeType = itemSO.inventoryItemData.buffs[i].attributeType;
        }
    }
}

