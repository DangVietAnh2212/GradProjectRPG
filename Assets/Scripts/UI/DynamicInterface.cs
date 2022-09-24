using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject slotPrefab;

    public float xStart;
    public float yStart;

    public float xSpaceBetweenSlot;
    public float ySpaceBetweenSlot;

    public int columnNum;

    public override void CreateInventorySlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetSlotPositionByIndex(i);
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }

    private Vector3 GetSlotPositionByIndex(int index)
    {
        return new Vector3
            (
            xStart + xSpaceBetweenSlot * (index % columnNum),
            yStart - ySpaceBetweenSlot * (index / columnNum),
            0f
            );
    }
}
