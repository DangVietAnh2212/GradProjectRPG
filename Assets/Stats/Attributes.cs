using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Attributes 
{
    [System.NonSerialized]
    public PlayerInventoryManager parent;
    public Attribute attributeType;
    public ModifiableInt value;

    public void SetParent(PlayerInventoryManager player)
    {
        parent = player;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModifierNotice(this);
    }
}
