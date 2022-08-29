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
        player.GetComponent<PlayerInventoryManager>().inventory.AddItem(new InventoryItem(item), 1);
        Destroy(this.gameObject);
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
    }
}
