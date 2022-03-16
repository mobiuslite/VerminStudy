using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    InventoryObject inventory;
    [SerializeField]
    InventoryObject equipment;
    [SerializeField]
    AudioSource pickUpAudio;

    private GameObject inventoryPanel; 
    private GameObject equipmentPanel; 
    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel = GameObject.Find("Inventory Screen");
        equipmentPanel = GameObject.Find("Equipment Screen");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Saved player inventory");
            inventory.Save();
            equipment.Save();

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loaded player inventory");
            inventory.Load();
            equipment.Load();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
            equipmentPanel.gameObject.SetActive(!equipmentPanel.gameObject.activeSelf);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            var item = other.GetComponent<GroundItem>();
            if(item)
            {
                Item _item = new Item(item.item);
                if(inventory.AddItem(_item, 1))
                {
                    Destroy(other.gameObject);
                    pickUpAudio.Play();
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.inventory.Clear();
        equipment.inventory.Clear();
    }
}
