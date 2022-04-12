using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    public Item mItem;
    public int mAmount;

    public ItemObject ItemObject
    {
        get
        {
            if(mItem.ID >= 0)
            {
                return parent.inventory.dataBase.Items[mItem.ID];
            }
            return null;
        }
    }

    public InventorySlot(Item item, int amount)
    {
        this.mItem = item;
        this.mAmount = amount;
    }
    public InventorySlot()
    {
        this.mItem = new Item();
        this.mAmount = 0;
    }

    public void UpdateSlot(Item item, int amount)
    {
        this.mItem = item;
        this.mAmount = amount;
    }

    public void AddAmount(int value)
    {
        mAmount += value;
    }

    public void RemoveItem()
    {
        mItem = new Item();
        mAmount = 0;
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if(AllowedItems.Length <= 0 || itemObject == null || itemObject.data.ID < 0)
        {
            return true;
        }
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObject.type == AllowedItems[i])
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[15];
    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject//, ISerializationCallbackReceiver
{
    [SerializeField]
    string filePath;
    public ItemDatabaseObject dataBase;
    public Inventory inventory;
    //If dataBase is private
//    private void OnEnable()
//    {
//#if UNITY_EDITOR
//        dataBase = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
//#else
//        dataBase = Resources.Load<ItemDatabaseObject>("Database");
//#endif
//    }

    public bool AddItem(Item item, int amount)
    {
        //Inventory is full
        if(EmptySlotCount <= 0)
        {
            return false;
        }
        InventorySlot slot = FindItemOnInventory(item);
        //If not stackable or no item was found
        if(!dataBase.Items[item.ID].stackable || slot == null)
        {
            SetEmptySlot(item, amount);
            return true;
        }
        //Found item and it is stackable
        slot.AddAmount(amount);
        return true;
    }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < inventory.Items.Length; i++)
            {
                if(inventory.Items[i].mItem.ID <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if(inventory.Items[i].mItem.ID == item.ID)
            {
                return inventory.Items[i];
            }
        }
        return null;
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        if (slot2.CanPlaceInSlot(slot1.ItemObject) && slot1.CanPlaceInSlot(slot2.ItemObject))
        {
            //Temp 3rd slot
            InventorySlot tempSlot = new InventorySlot(slot1.mItem, slot1.mAmount);

            slot1.UpdateSlot(slot2.mItem, slot2.mAmount);
            slot2.UpdateSlot(tempSlot.mItem, tempSlot.mAmount);
        }

    }

    public void DropItem(Item item)
    {
        //Find the item we are trying to remove
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if(inventory.Items[i].mItem == item)
            {
                inventory.Items[i].UpdateSlot(null, 0);
                return;
            }
        }
        //Shouldn't get here
    }

    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (inventory.Items[i].mItem.ID <= -1)
            {
                inventory.Items[i].UpdateSlot(item, amount);
                return inventory.Items[i];
            }
        }
        //TODO: inventory is full
        return null;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //Creates editable files
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter binaryFormatter = new BinaryFormatter();
        //FileStream fs = File.Create(string.Concat(Application.persistentDataPath, filePath));
        //binaryFormatter.Serialize(fs, saveData);
        //fs.Close();

        //Non editable files
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, filePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, inventory);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, filePath)))
        {
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            //FileStream fs = File.Open(string.Concat(Application.persistentDataPath, filePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fs).ToString(), this);
            //fs.Close();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, filePath), FileMode.Open, FileAccess.Read);
            Inventory newInventory = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < inventory.Items.Length; i++)
            {
                InventorySlot temp = newInventory.Items[i];
                inventory.Items[i].UpdateSlot(temp.mItem, temp.mAmount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        inventory.Clear();
    }

    //public void OnAfterDeserialize()
    //{
    //    for(int i = 0; i < inventory.Items.Count; i++)
    //    {
    //        inventory.Items[i].mItem = dataBase.GetItem[inventory.Items[i].mID];
    //    }
    //}

    //public void OnBeforeSerialize()
    //{
    //}
}
