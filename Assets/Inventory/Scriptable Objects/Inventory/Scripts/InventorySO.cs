using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// Base class to manage inventory
/// </summary>
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
            if (slotsContainer.itemSlots[i].slotID == inventoryItem.ID && inventoryItem.isStackable)
            {
                slotsContainer.itemSlots[i].AddAmount(amount);
                return;
            }
        }
        PutItemToEmptySlot(inventoryItem, amount);
    }
    /// <summary>
    /// Add the new item to a empty inventory slot, return the slot in the inventory of that item if find one,
    /// return null if can't find an empty slot.
    /// </summary>
    /// <param name="inventoryItem"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public InventorySlot PutItemToEmptySlot(InventoryItem inventoryItem, int amount)
    {
        for (int i = 0; i < slotsContainer.itemSlots.Length; i++)
        {
            if (slotsContainer.itemSlots[i].slotID == -1)
            {
                slotsContainer.itemSlots[i].UpdateSlot(inventoryItem.ID, inventoryItem, amount);
                return slotsContainer.itemSlots[i];
            }
        }
        //Do something if inventory is full
        return null;
    }

    public void SwapItemInSlot(InventorySlot slot1, InventorySlot slot2)
    {
        InventorySlot temp = new InventorySlot(slot2.slotID, slot2.inventoryItem, slot2.amount);
        slot2.UpdateSlot(slot1.slotID, slot1.inventoryItem, slot1.amount);
        slot1.UpdateSlot(temp.slotID, temp.inventoryItem, temp.amount);
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        for (int i = 0; i < slotsContainer.itemSlots.Length; i++)
        {
            if (slotsContainer.itemSlots[i].inventoryItem == inventoryItem)
                slotsContainer.itemSlots[i].UpdateSlot(-1, null, 0);
        }
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
        Debug.Log("Saved");
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
            Inventory newSlotsContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < slotsContainer.itemSlots.Length; i++)
            {
                slotsContainer.itemSlots[i].UpdateSlot(
                    newSlotsContainer.itemSlots[i].slotID,
                    newSlotsContainer.itemSlots[i].inventoryItem,
                    newSlotsContainer.itemSlots[i].amount
                    );
            }
            stream.Close();
            Debug.Log("Loaded");
            Debug.Log("Inside Inventory: " + slotsContainer.itemSlots[0].inventoryItem.name);
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        slotsContainer = new Inventory();
    }
}

/// <summary>
/// Class that holds the item slots
/// </summary>
[System.Serializable]
public class Inventory 
{
    public static int inventorySlotNum = 24;
    public InventorySlot[] itemSlots = new InventorySlot[inventorySlotNum];
}

/// <summary>
/// Class manage each slot in the inventory and how they change
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public int slotID = -1;//so the inventory won't load item having ID = 0 
    public InventoryItem inventoryItem;
    public int amount;

    public InventorySlot()
    {
        this.slotID = -1;
        this.inventoryItem = null;
        this.amount = 0;
    }
    public InventorySlot(int ID, InventoryItem item, int amount)
    {
        this.slotID = ID;
        this.inventoryItem = item;
        this.amount = amount;
    }

    /// <summary>
    /// this method update the item slot, replace the slot's ID with ID of item in the inventory
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void UpdateSlot(int ID, InventoryItem item, int amount)
    {
        this.slotID = ID;
        this.inventoryItem = item;
        this.amount = amount;
    }
    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
}