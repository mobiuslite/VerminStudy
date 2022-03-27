using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BattleMessenger))]
public class Player : MonoBehaviour
{
    [SerializeField]
    InventoryObject inventory;
    [SerializeField]
    InventoryObject equipment;
    [SerializeField]
    AudioClip pickUpSound;

    AudioSource playerAudio;

    BattleMessenger messenger;

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        messenger = GetComponent<BattleMessenger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.GetInventoryDown())
        {
            UIManager.Instance.ToggleUI(UIType.Inventory);
        }
        if (InputManager.Instance.GetEquipmentDown())
        {
            UIManager.Instance.ToggleUI(UIType.Equipment);
        }

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
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GroundItem item = other.GetComponent<GroundItem>();
            if (item)
            {
                Item _item = new Item(item.item);
                if (inventory.AddItem(_item, 1))
                {
                    Destroy(other.gameObject);
                    playerAudio.PlayOneShot(pickUpSound);
                }
            }
        }
    }
    public void StartBattle()
    {
        List<IBattleMessenger> allies = new List<IBattleMessenger>();
        allies.Add(messenger);
        BattleMediator.Instance.SetAllies(allies);
    }

    public void DamageEnemy()
    {
        BattleMessage msg = new BattleMessage("enemy_take_damage");
        msg.data.Add("enemy_index", 0);
        msg.data.Add("damage", messenger.stats.GetDamageAmount());


        BattleMediator.Instance.ReceiveMessage(msg);
    }

    private void OnApplicationQuit()
    {
        inventory.inventory.Clear();
        equipment.inventory.Clear();
    }
}
