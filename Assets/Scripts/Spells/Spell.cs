using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum SpellType
{
    Fire,
    Ice,
    Lightning
}

[System.Serializable]
public class Spell
{
    public SpellType spellType;
    public float minRollBaseDamage;
    public float maxRollBaseDamage;
    [SerializeField]
    float baseDamageValue;
    [HideInInspector]
    public float minBaseDamage;
    [HideInInspector]
    public float maxBaseDamage;
    public int manaCost;
    public float speed;
    public float lifeDuration;
    public float AOE;

    public Spell()
    {
        minRollBaseDamage = 0;
        maxRollBaseDamage = 0;
        baseDamageValue = 0;
        minBaseDamage = 0;
        maxBaseDamage = 0;
    }
    public Spell(ItemSO itemSO)
    {
        speed = itemSO.spell.speed;
        spellType = itemSO.spell.spellType;
        minRollBaseDamage = itemSO.spell.minRollBaseDamage;
        maxRollBaseDamage = itemSO.spell.maxRollBaseDamage;
        manaCost = itemSO.spell.manaCost;
        lifeDuration = itemSO.spell.lifeDuration;
        AOE = itemSO.spell.AOE;
        baseDamageValue = RollBaseDamage();
        switch (spellType)
        {
            case SpellType.Fire:
                minBaseDamage = baseDamageValue;
                maxBaseDamage = baseDamageValue;
                break;
            case SpellType.Ice:
                minBaseDamage = baseDamageValue;
                maxBaseDamage = baseDamageValue * 2f;
                break;
            case SpellType.Lightning:
                minBaseDamage = baseDamageValue;
                maxBaseDamage = baseDamageValue * 10f;
                break;
            default:
                break;
        }
    }

    float RollBaseDamage()
    {
        return baseDamageValue = Random.Range(minRollBaseDamage, maxRollBaseDamage);
    }
}
