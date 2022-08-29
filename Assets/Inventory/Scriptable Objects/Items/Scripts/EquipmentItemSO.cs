using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory System/Item/Equipment")]
public class EquipmentItemSO : ItemSO
{
    public int statValue;
    public void Awake()
    {
        itemType = ItemType.Equipment;
    }
}
