using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    LifeRecover,
    ManaRecover, 
    Equipment,
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
public abstract class ItemSO : ScriptableObject
{
    public int ID;
    public Sprite uiDisplay;
    public ItemType itemType;
    [TextArea(15, 20)]
    public string description;
    public ItemBuff[] buffs;
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
    public int ID;
    public ItemBuff[] buffs;
    public bool isStackable;

    public InventoryItem(ItemSO itemSO)
    {
        name = itemSO.name;
        ID = itemSO.ID;
        buffs = new ItemBuff[itemSO.buffs.Length];
        isStackable = itemSO.isStackable;
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemSO.buffs[i].min, itemSO.buffs[i].max);
            buffs[i].attribute = itemSO.buffs[i].attribute;
        }
    }
}

/// <summary>
/// Class for buffs in item
/// </summary>
[System.Serializable]
public class ItemBuff
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

    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}