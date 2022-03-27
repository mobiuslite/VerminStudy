using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleMessenger))]
public class Enemy : MonoBehaviour
{
    BattleMessenger messenger;

    private void Awake()
    {
        if(messenger == null)
            messenger = GetComponent<BattleMessenger>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If is player, start combat
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Start combat");

            List<IBattleMessenger> enemies = new List<IBattleMessenger>();
            enemies.Add(messenger);
            BattleMediator.Instance.SetEnemies(enemies);

            BattleMediator.Instance.RequestAllies();
            BattleMediator.Instance.StartBattle();
        }
    }
}
