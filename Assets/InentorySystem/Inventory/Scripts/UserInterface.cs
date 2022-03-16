using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;

    protected Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    // Start is called before the first frame update
    void Start()
    {
        //Set reference to which interface each item came from
        for (int i = 0; i < inventory.inventory.Items.Length; i++)
        {
            inventory.inventory.Items[i].parent = this;
        }
        CreateSlots();
        Addevent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        Addevent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    //From old project
    //public void CreateDisplay()
    //{
    //    for (int i = 0; i < inventory.inventory.Items.Count; i++)
    //    {
    //        InventorySlot slot = inventory.inventory.Items[i];

    //        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
    //        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.dataBase.GetItem[slot.mItem.ID].uiSprite;
    //        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //        obj.GetComponentInChildren<TMP_Text>().text = slot.mAmount.ToString("n0");
    //        slotsOnInterface.Add(slot, obj);

    //    }
    //}
    public abstract void CreateSlots();


    //Old, for reference
    //public void UpdateDisplay()
    //{
    //    for (int i = 0; i < inventory.inventory.Items.Count; i++)
    //    {
    //        InventorySlot slot = inventory.inventory.Items[i];

    //        if (slotsOnInterface.ContainsKey(slot))
    //        {
    //            slotsOnInterface[slot].GetComponentInChildren<TMP_Text>().text = slot.mAmount.ToString("n0");
    //        }
    //        else
    //        {
    //            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
    //            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.dataBase.GetItem[slot.mItem.ID].uiSprite;
    //            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //            obj.GetComponentInChildren<TMP_Text>().text = slot.mAmount.ToString("n0");
    //            slotsOnInterface.Add(slot, obj);
    //        }
    //    }
    //}
    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            //There is an item
            if (slot.Value.mItem.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiSprite;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TMP_Text>().text = slot.Value.mAmount == 1 ? "" : slot.Value.mAmount.ToString("n0"); //Display qty if there is more than 1 
            }
            //There is not an item
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TMP_Text>().text = "";
            }
        }
    }

    //For easily adding events through code
    protected void Addevent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotMouseIsOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        //Clear out the object when not hovering over a slot
        MouseData.slotMouseIsOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.uiMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.uiMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        //Create visual representation of item we are moving
        GameObject mouseObject = new GameObject();
        RectTransform rectTransform = mouseObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);

        //Is there actually an item
        if (slotsOnInterface[obj].mItem.ID >= 0)
        {
            Image img = mouseObject.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiSprite;
            img.raycastTarget = false; //Let mouse ignore this so we can still register the drag

            MouseData.item = slotsOnInterface[obj].ItemObject;
        }

        MouseData.tempItemBeingDragged = mouseObject;
        
    }

    public void OnDragEnd(GameObject obj)
    {
        //Clear the image so it isnt just left on the screen
        ItemObject droppedItem = MouseData.item;

        Destroy(MouseData.tempItemBeingDragged);

        //Mouse is not over a ui so remove it
        if(MouseData.uiMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();

            Debug.Log("Dropped item");

            GameObject basePrefab = GameObject.FindGameObjectWithTag("BaseItemPrefab");
            basePrefab.GetComponent<GroundItem>().item = droppedItem;

            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            GameObject newItem = Instantiate(basePrefab, playerPos + new Vector3(0.0f, 0.0f, 2.0f), Quaternion.identity);

            newItem.GetComponent<Rigidbody>().velocity = new Vector3(2.0f, 2.0f, 0.0f);
            newItem.tag = "Item";

            return;
        }

        //Mouse is over a slot
        if(MouseData.slotMouseIsOver)
        {
            InventorySlot slotMouseIsOver = MouseData.uiMouseIsOver.slotsOnInterface[MouseData.slotMouseIsOver];
            inventory.SwapItems(slotsOnInterface[obj], slotMouseIsOver);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public static class MouseData
{
    public static GameObject tempItemBeingDragged;
    public static GameObject slotMouseIsOver;
    public static UserInterface uiMouseIsOver;

    public static ItemObject item;
}