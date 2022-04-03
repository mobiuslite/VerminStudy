using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BattleMessenger
{
    [SerializeField]
    InventoryObject inventory;
    [SerializeField]
    InventoryObject equipment;
    [SerializeField]
    AudioClip pickUpSound;

    AudioSource playerAudio;

    Health healthBar;

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<Health>();
        healthBar.HideHealth();
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
        allies.Add(this);
        BattleMediator.Instance.SetAllies(allies);

        healthBar.ShowHealth();
    }

    public void DamageEnemy()
    {
        BattleMessage msg = new BattleMessage("enemy_take_damage");
        msg.data.Add("enemy_index", 0);
        msg.data.Add("damage", this.stats.GetDamageAmount());


        BattleMediator.Instance.ReceiveMessage(msg);
    }

    private void OnApplicationQuit()
    {
        inventory.inventory.Clear();
        equipment.inventory.Clear();
    }

    public override void ReceiveMessage(BattleMessage message)
    {
        switch (message.type)
        {
            case "take_damage":
                {

                    float damage = message.data["damage"];
                    stats.TakeDamage(damage);
                    Debug.Log(gameObject.name + $" took {damage} damage!");

                    if (stats.GetHealth() <= 0.0f)
                    {
                        Debug.Log("Game over");
                        BattleMediator.Instance.EndBattle();
                        //Destroy(gameObject);
                    }

                    break;
                }
            case "end_battle":
                {
                    healthBar.HideHealth();
                    break;
                }
        }

        healthBar.SetHealthScale(stats.GetHealth() / stats.GetMaxHealth());
    }
}
