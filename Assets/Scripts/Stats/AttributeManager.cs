using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AttributeManager
{
    [System.NonSerialized]
    public PlayerInventoryManager parent;
    public AttributeType attributeType;
    public ModifiableInt value;

    public void SetParent(PlayerInventoryManager player)
    {
        parent = player;
        value = new ModifiableInt();
    }

    public void AttributeModified()
    {
        parent.AttributeModifierNotice(this);
    }
}
