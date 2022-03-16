using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    [SerializeField]
    int xSpacing;
    [SerializeField]
    int ySpacing;
    [SerializeField]
    int numOfColumns;

    [SerializeField]
    int xStartPosition;
    [SerializeField]
    int yStartPosition;

    public GameObject inventoryPrefab;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.inventory.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            Addevent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            Addevent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            Addevent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            Addevent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            Addevent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotsOnInterface.Add(obj, inventory.inventory.Items[i]);
        }
    }


    private Vector3 GetPosition(int index)
    {
        return new Vector3(xStartPosition + (xSpacing * (index % numOfColumns)), yStartPosition + (-ySpacing * (index / numOfColumns)), 0f);
    }
}
