using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]

public class EquipmentObject : ItemObject
{
    [SerializeField]
    int attackBonus;

    [SerializeField]
    int defenceBonus;

    public void Awake()
    {
        type = ItemType.Armor;
    }
}
