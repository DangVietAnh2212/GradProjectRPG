using JetBrains.Annotations;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public string savePath;
    public ItemDatabaseSO itemDatabase;
    public Inventory slotsContainer;

    public void AddItem(InventoryItem inventoryItem, int amount)
    {
        for(int i = 0; i < slotsContainer.itemSlots.Count; i++)
        {
            if (slotsContainer.itemSlots[i].inventoryItem.ID == inventoryItem.ID && inventoryItem.isStackable)
            {
                slotsContainer.itemSlots[i].AddAmount(amount);
                return;
            }
        }
        slotsContainer.itemSlots.Add(new InventorySlot(inventoryItem.ID, inventoryItem, amount));
    }


    [ContextMenu("Save")]
    public void Save()
    {
        /*string saveData = JsonUtility.ToJson(this, true);
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, saveData);
        file.Close();*/

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, slotsContainer);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            /*FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();   */
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            slotsContainer = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        slotsContainer = new Inventory();
    }
}

[System.Serializable]
public class Inventory 
{
    public List<InventorySlot> itemSlots = new List<InventorySlot>();
}
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public InventoryItem inventoryItem;
    public int amount;
    public InventorySlot(int ID, InventoryItem item, int amount)
    {
        this.ID = ID;
        this.inventoryItem = item;
        this.amount = amount;
    }

    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
}