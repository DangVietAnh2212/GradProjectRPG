using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : Interactable, ISerializationCallbackReceiver
{
    float startLifeTime;
    float lifeDuration = 60f;
    public ItemSO item;
    [HideInInspector]
    public bool isNew = true;
    [HideInInspector]
    public InventoryItem inventoryItem;

    void Start()
    {
        startLifeTime = Time.time;
    }
    public override void Interact()
    {
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
        if(Time.time - startLifeTime >= lifeDuration)
            Destroy(this.gameObject);
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
