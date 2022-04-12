using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleMessenger
{
    Health healthBar;

    [SerializeField]
    ItemObject[] possibleDrops;

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

    public override void OnDeath()
    {
        if (possibleDrops.Length > 0)
        {
            int maxIndex = possibleDrops.Length;
            int itemIndex = Random.Range(0, maxIndex);

            ItemInstantiate.Instantiate(possibleDrops[itemIndex], transform.position);
        }
    }
}
