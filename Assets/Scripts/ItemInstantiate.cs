using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstantiate : MonoBehaviour
{
    public static void Instantiate(ItemObject curItem, Vector3 pos)
    {
        GameObject basePrefab = GameObject.FindGameObjectWithTag("BaseItemPrefab");
        basePrefab.name = curItem.data.Name;
        basePrefab.GetComponent<GroundItem>().item = curItem;

        GameObject newItem = Instantiate(basePrefab, pos + new Vector3(0.0f, 0.0f, -1.4f), Quaternion.identity);
        newItem.GetComponentInChildren<SpriteRenderer>().sprite = curItem.uiSprite;
        newItem.name = curItem.name;

        newItem.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-5.0f, 5.0f), 2.5f, -2.5f);
        newItem.tag = "Item";
    }
}
