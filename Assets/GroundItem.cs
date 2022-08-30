using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : Interactible, ISerializationCallbackReceiver
{
    public ItemSO item;
    public override void Interact()
    {
        print("Pick up " + this);
        var playerInventory = player.GetComponent<PlayerInventoryManager>().inventory;
        if(playerInventory.AddItem(new InventoryItem(item), 1))
        {
            Destroy(this.gameObject);
        }
                                                 
       
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
