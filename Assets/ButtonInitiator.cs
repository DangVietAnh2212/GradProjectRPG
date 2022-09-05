using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInitiator : MonoBehaviour
{
    public InventorySO savedInventory;
    private void Start()
    {
        savedInventory.Load();
    }
}
