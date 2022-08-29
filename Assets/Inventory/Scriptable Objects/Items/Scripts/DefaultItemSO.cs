using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName ="Inventory System/Item/Default")]
public class DefaultItemSO : ItemSO
{
    public void Awake()
    {
        itemType = ItemType.Default;
        isStackable = true;
    }
}
