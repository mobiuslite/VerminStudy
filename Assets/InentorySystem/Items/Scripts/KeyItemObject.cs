using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New KeyItem Object", menuName = "Inventory System/Items/KeyItem")]

public class KeyItemObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Key;
    }
}
