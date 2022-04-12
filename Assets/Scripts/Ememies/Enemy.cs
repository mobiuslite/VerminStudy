using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleMessenger
{
    Health healthBar;
    [SerializeField]
    ItemObject[] heldItems;


    public override void ReceiveMessage(BattleMessage message)
    {
        base.ReceiveMessage(message);

        switch (message.type)
        {
            case "request_action":
                {
                    BattleMessage msg = new BattleMessage("allies_take_damage");
                    msg.data.Add("damage", stats.GetDamageAmount());
                    msg.data.Add("party_index", Random.Range(0, (int)message.data["num_party"]));

                    BattleMediator.Instance.ReceiveMessage(msg);

                    break;
                }
            case "end_battle":
                {
                    healthBar.HideHealth();
                    Debug.Log("BP");
                    break;
                }
        }

        healthBar.SetHealthScale(stats.GetHealth() / stats.GetMaxHealth());
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<Health>();
        healthBar.HideHealth();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If is player, start combat
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Start combat");

            List<IBattleMessenger> enemies = new List<IBattleMessenger>();
            enemies.Add(this);
            BattleMediator.Instance.SetEnemies(enemies);

            BattleMediator.Instance.RequestAllies();
            BattleMediator.Instance.StartBattle();

            healthBar.ShowHealth();
        }
    }

    public void DropItems()
    {
        Debug.Log("Dropped item");

        for (int i = 0; i < heldItems.Length; i++)
        {
            ItemObject curItem = heldItems[i];

            GameObject basePrefab = GameObject.FindGameObjectWithTag("BaseItemPrefab");
            basePrefab.name = curItem.data.Name;
            basePrefab.GetComponent<GroundItem>().item = curItem;

            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

            basePrefab.GetComponentInChildren<SpriteRenderer>().sprite = curItem.uiSprite;

            GameObject newItem = Instantiate(basePrefab, playerPos + new Vector3(0.0f, 0.0f, -1.4f), Quaternion.identity);

            newItem.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2.0f, 2.0f), 2.5f, -0.5f);
            newItem.tag = "Item";
        }
    }
}
