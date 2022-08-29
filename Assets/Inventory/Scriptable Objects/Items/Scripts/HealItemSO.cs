using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Heal Item", menuName = "Inventory System/Item/Heal Item")]
public class HealItemSO : ItemSO
{
    public int healthRecover = 20;
    public void Awake()
    {
        itemType = ItemType.LifeRecover;
        isStackable = true;
    }
}
