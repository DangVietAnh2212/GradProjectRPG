using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Mana Heal", menuName = "Inventory System/Item/Mana Item")]
public class ManaHealItemSO : ItemSO
{
    public int manaRecover = 30;

    private void Awake()
    {
        itemType = ItemType.ManaRecover;
        isStackable = true;
    }
}
