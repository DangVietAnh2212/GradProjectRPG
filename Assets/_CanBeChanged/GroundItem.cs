using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : Interactible, ISerializationCallbackReceiver
{

    public ItemSO item;
    public bool isNew = true;
    public InventoryItem inventoryItem;
    public override void Interact()
    {
        print("Pick up " + this);
        var playerInventory = player.GetComponent<PlayerInventoryManager>().inventory;
        if(isNew == true)
        {
            if (playerInventory.AddItem(new InventoryItem(item), 1))
            {
                Destroy(this.gameObject);
            }
        }
        else if(isNew == false)
        {
            if (playerInventory.AddItem(inventoryItem, 1))
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    public void LateUpdate()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
    }
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }
}
