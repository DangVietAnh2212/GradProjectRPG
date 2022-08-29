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
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public string savePath;
    public ItemDatabaseSO itemDatabase;
    public Inventory slotsContainer;


    public void AddItem(InventoryItem inventoryItem, int amount)
    {
        for (int i = 0; i < slotsContainer.itemSlots.Length; i++)
        {
            if (slotsContainer.itemSlots[i].ID == inventoryItem.ID && inventoryItem.isStackable)
            {
                slotsContainer.itemSlots[i].AddAmount(amount);
                return;
            }
        }
        SetFirstEmptySlot(inventoryItem, amount);
    }

    public InventorySlot SetFirstEmptySlot(InventoryItem inventoryItem, int amount)
    {
        for (int i = 0; i < slotsContainer.itemSlots.Length; i++)
        {
            if (slotsContainer.itemSlots[i].ID == -1)
            {
                slotsContainer.itemSlots[i].UpdateSlot(inventoryItem.ID, inventoryItem, amount);
                return slotsContainer.itemSlots[i];
            }
        }
        //Do something if inventory is full
        return null;
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
    public static int inventorySlotNum = 24;
    public InventorySlot[] itemSlots = new InventorySlot[inventorySlotNum];
}
[System.Serializable]
public class InventorySlot
{
    public int ID = -1;//so the inventory won't load item having ID = 0 
    public InventoryItem inventoryItem;
    public int amount;

    public InventorySlot()
    {
        this.ID = -1;
        this.inventoryItem = null;
        this.amount = 0;
    }
    public InventorySlot(int ID, InventoryItem item, int amount)
    {
        this.ID = ID;
        this.inventoryItem = item;
        this.amount = amount;
    }
    public void UpdateSlot(int ID, InventoryItem item, int amount)
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