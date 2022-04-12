using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Hat,
    Armor,
    Weapon,
    Shield,
    Accessory,
    Key,
    Default
}

public enum Attributes
{
    Strength,
    Perception,
    Endurance,
    Charisma,
    Intelligence,
    Agility,
    Luck
}

public class ItemObject : ScriptableObject
{
    public Sprite uiSprite;
    public bool stackable;
    public ItemType type;

    [SerializeField]
    [TextArea(15, 20)]
    string description;

    public Item data = new Item();
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID = -1;
    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        ID = -1;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        ID = item.data.ID;
        this.buffs = new ItemBuff[item.data.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].mValue)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int mValue;
    public ItemBuff(int value)
    {
        mValue = value;
    }
}