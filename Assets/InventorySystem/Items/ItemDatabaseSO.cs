using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for all item exist in the game. It will give each item their own ID
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Item/Database")]
public class ItemDatabaseSO : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemSO[] itemSOs;
    //public Dictionary<int, ItemSO> GetItem = new Dictionary<int, ItemSO>();
    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if(itemSOs[i].inventoryItemData.ID != i)
                itemSOs[i].inventoryItemData.ID = i;
        }
    }
    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
        //GetItem = new Dictionary<int, ItemSO>();
    }

    //Need ISerializationCallbackReceiver because this is a Scriptable Obj i create in the Editor so 
    //to fill this with Data, i need to implement this Interface (there is no Start or Update for
    //Scriptable Object to initialize the Databse value, if i initialize this elsewhere, it doesn't
    //make sense because i want this ready everytime i create a Database Scriptable Obj in Menu Asset
}
