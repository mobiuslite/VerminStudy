using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.inventory.Items.Length; i++)
        {
            var obj = slots[i];

            Addevent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            Addevent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            Addevent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            Addevent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            Addevent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotsOnInterface.Add(obj, inventory.inventory.Items[i]);
        }
    }
}
