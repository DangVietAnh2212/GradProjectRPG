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

public enum Attribute
{
    Strength,
    Dexterity,
    Intelligence,
    Life,
    Mana,
    Attack,
    Defence
}
/// <summary>
/// Base class for all items in the game
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite uiDisplay;
    public GameObject displayCharEquipment;
    public ItemType itemType;
    [TextArea(15, 20)]
    public string description;
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
    public string name;
    public int ID = -1;
    public ItemBuff[] buffs;
    public bool isStackable;

    public InventoryItem()
    {
        name = "";
        ID = -1;
    }
    public InventoryItem(ItemSO itemSO)
    {
        name = itemSO.name;
        ID = itemSO.inventoryItemData.ID;
        buffs = new ItemBuff[itemSO.inventoryItemData.buffs.Length];
        isStackable = itemSO.isStackable;
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemSO.inventoryItemData.buffs[i].min, itemSO.inventoryItemData.buffs[i].max);
            buffs[i].attribute = itemSO.inventoryItemData.buffs[i].attribute;
        }
    }
}

/// <summary>
/// Class for buffs in item
/// </summary>
[System.Serializable]
public class ItemBuff : IModifier
{
    public Attribute attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}