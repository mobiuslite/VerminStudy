using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    bool inBattle;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<Health>();
        healthBar.HideHealth();
        inBattle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.GetInventoryDown())
        {         
            UIManager.Instance.ToggleUI(UIType.Inventory);
            if (!inBattle)
            {
                bool? inventoryIsActive = UIManager.Instance.CheckUIActive(UIType.Inventory);

                if (inventoryIsActive == true)
                {
                    healthBar.ShowHealth();
                }
                else if (inventoryIsActive == false)
                {
                    healthBar.HideHealth();
                }
            }
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

        if (Input.GetMouseButtonDown(1))
        {
            GameObject obj = MouseData.tempItemBeingDragged;
            if(obj != null)
            {
                ItemObject item = MouseData.item;
                if(item.type == ItemType.Consumable)
                {
                    PotionObject potion = (PotionObject)item;
                    if (MouseData.amount > 0)
                    {
                        if (potion != null && stats.GetHealth() < stats.GetMaxHealth())
                        {
                            stats.Heal(potion.healthRestoreValue);
                            healthBar.SetHealthScale(stats.GetHealth() / stats.GetMaxHealth());
                            MouseData.amount -= 1;
                        }
                    }
                }
            }
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
        inBattle = true;
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
        base.ReceiveMessage(message);
        switch (message.type)
        {
            case "end_battle":
                {
                    healthBar.HideHealth();
                    inBattle = false;
                    break;
                }
        }

        healthBar.SetHealthScale(stats.GetHealth() / stats.GetMaxHealth());
    }

    public override void OnDeath()
    {
        UIManager.Instance.ShowUI(UIType.GameOver);
    }
}
