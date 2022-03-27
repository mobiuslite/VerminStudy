using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleStats))]
public class BattleMessenger : MonoBehaviour, IBattleMessenger
{
    public BattleStats stats;

    private void Awake()
    {
        stats = GetComponent<BattleStats>();
    }

    public void ReceiveMessage(BattleMessage message)
    {
        switch (message.type)
        {
            case "take_damage":

                float damage = message.data["damage"];
                stats.TakeDamage(damage);
                Debug.Log(gameObject.name + $" took {damage} damage!");

                if (stats.GetHealth() <= 0.0f)
                {
                    Debug.Log("Battle over");
                    BattleMediator.Instance.EndBattle();
                    Destroy(gameObject);
                }

                break;
        }
    }
}
