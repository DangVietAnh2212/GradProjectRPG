using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    Equipment
}

/// <summary>
/// Base class to manage inventory
/// </summary>
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public string savePath;
    public ItemDatabaseSO itemDatabase;
    public Inventory slotsContainer;
    public InterfaceType interfaceType;
    public InventorySlot[] GetSlots { get { return slotsContainer.itemSlots; } }

    public bool AddItem(InventoryItem inventoryItem, int amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(inventoryItem);
        if (!itemDatabase.itemSOs[inventoryItem.ID].isStackable || slot == null)
        {
            PutItemToEmptySlot(inventoryItem, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }

    public InventorySlot FindItemOnInventory(InventoryItem inventoryItem)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].inventoryItem.ID == inventoryItem.ID)
                return GetSlots[i];
        }
        return null;
    }
    public int EmptySlotCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].inventoryItem.ID <= -1)
                    count++;
            }
            return count;
        }
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
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].inventoryItem.ID == -1)
            {
                GetSlots[i].UpdateSlot(inventoryItem, amount);
                return GetSlots[i];
            }
        }
        //Do something if inventory is full
        return null;
    }

    public void SwapItemInSlot(InventorySlot slot1, InventorySlot slot2)
    {
        if (slot2.CanPlaceInSlot(slot1.ItemSO) && slot1.CanPlaceInSlot(slot2.ItemSO))
        {
            InventorySlot temp = new InventorySlot(slot2.inventoryItem, slot2.amount);
            Debug.Log("Inside Swap Item In Slot");
            slot2.UpdateSlot(slot1.inventoryItem, slot1.amount);
            slot1.UpdateSlot(temp.inventoryItem, temp.amount);
        }
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].inventoryItem == inventoryItem)
                GetSlots[i].UpdateSlot(null, 0);
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
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(
                    newSlotsContainer.itemSlots[i].inventoryItem,
                    newSlotsContainer.itemSlots[i].amount
                    );
            }
            stream.Close();
            /*Debug.Log("Loaded");
            Debug.Log("Inside Inventory: " + GetSlots[0].inventoryItem.name);*/
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        if(interfaceType == InterfaceType.Inventory)
            slotsContainer.Clear();
        if(interfaceType == InterfaceType.Equipment)
        {
            ItemType[] allowedSlots = new ItemType[12];
            for (int i = 0; i < allowedSlots.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        allowedSlots[i] = ItemType.Helmet;
                        break;
                    case 1:
                        allowedSlots[i] = ItemType.BodyArmour;
                        break;
                    case 2:
                        allowedSlots[i] = ItemType.Glove;
                        break;
                    case 3:
                        allowedSlots[i] = ItemType.SpellBook;
                        break;
                    case 4:
                        allowedSlots[i] = ItemType.Boots;
                        break;
                    case 5:
                        allowedSlots[i] = ItemType.LifeRecover;
                        break;
                    case 6:
                        allowedSlots[i] = ItemType.ManaRecover;
                        break;
                    case 7:
                        allowedSlots[i] = ItemType.Spell;
                        break;
                    case 8:
                        allowedSlots[i] = ItemType.Spell;
                        break;
                    case 9:
                        allowedSlots[i] = ItemType.Spell;
                        break;
                    case 10:
                        allowedSlots[i] = ItemType.Spell;
                        break;
                    case 11:
                        allowedSlots[i] = ItemType.Spell;
                        break;
                }
            }
            int allowedSlotCount = 0;
            for (int i = 0; i < slotsContainer.itemSlots.Length; i++)
            {
                GetSlots[i].allowedItems = new ItemType[1];
                GetSlots[i].allowedItems[0] = allowedSlots[allowedSlotCount++];
            }
            slotsContainer.Clear();
        }
    }
}

/// <summary>
/// Class that holds the item slots
/// </summary>
[System.Serializable]
public class Inventory 
{
    public static int inventorySlotNum = 6 * 6;
    public InventorySlot[] itemSlots = new InventorySlot[inventorySlotNum];
    public void Clear()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdate(InventorySlot slot);

/// <summary>
/// Class manage each slot in the inventory and how they change
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdate OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdate OnBeforeUpdate;
    public InventoryItem inventoryItem = new InventoryItem();
    public int amount;

    public InventorySlot()
    {
        UpdateSlot(new InventoryItem(), 0);
    }
    public InventorySlot(InventoryItem item, int amount)
    {
        UpdateSlot(item, amount);
    }

    public ItemSO ItemSO 
    { 
        get
        {
            if (inventoryItem.ID >= 0)
            {
                return parent.inventory.itemDatabase.itemSOs[inventoryItem.ID];
            }
            return null;
        }
    }

    /// <summary>
    /// this method update the item slot, replace the slot's ID with ID of item in the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void UpdateSlot(InventoryItem item, int amount)
    {
        if (OnBeforeUpdate != null) 
        {
            OnBeforeUpdate.Invoke(this);
        }
        this.inventoryItem = item;
        this.amount = amount;
        if (OnAfterUpdate != null) 
        {
            OnAfterUpdate.Invoke(this);
        } 
    }

    public void RemoveItem()
    {
        UpdateSlot(new InventoryItem(), 0);
    }
    public void AddAmount(int amount)
    {
        UpdateSlot(inventoryItem, this.amount += amount);
    }

    public bool CanPlaceInSlot(ItemSO itemSO)
    {
        if (allowedItems.Length <= 0 || itemSO == null || itemSO.inventoryItemData.ID < 0)
            return true;//Checking if itemSO can be placed on this slot or not
        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (itemSO.itemType == allowedItems[i])
                return true;
        }
        return false;
    }


}