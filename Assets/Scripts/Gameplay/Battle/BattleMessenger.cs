using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleStats))]
public abstract class BattleMessenger : MonoBehaviour, IBattleMessenger
{
    public BattleStats stats;

    private void Start()
    {
        stats = GetComponent<BattleStats>();
    }

    public virtual void ReceiveMessage(BattleMessage message)
    {
        switch (message.type)
        {
            case "take_damage":

                float damage = message.data["damage"];
                stats.TakeDamage(damage);
                Debug.Log(gameObject.name + $" took {damage} damage!");

                if (stats.GetHealth() <= 0.0f)
                {
                    Debug.Log($"{gameObject.name} is dead");

                    BattleMessage msg = new BattleMessage("dead");
                    msg.who = this;
                    
                    BattleMediator.Instance.ReceiveMessage(msg);
                    //BattleMediator.Instance.EndBattle();

                    OnDeath();
                    //Destroy(gameObject);
                }

                break;
        }
    }

    public virtual void OnDeath()
    {
    }
}
