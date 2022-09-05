using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for buffs in item
/// </summary>
[System.Serializable]
public class ItemBuff : IModifier
{
    public AttributeType attributeType;
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

public enum AttributeType
{
    Strength,
    Dexterity,
    Intelligence,
    Life,
    Mana,
    Attack,
    Defence
}