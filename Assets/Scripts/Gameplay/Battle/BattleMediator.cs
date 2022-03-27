using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMessage
{
    public BattleMessage(string type)
    {
        this.type = type;
        data = new Dictionary<string, float>();
    }

    public string type;
    public Dictionary<string, float> data;
}

public enum BattleState
{
    Start,
    End,
    PlayerTurn,
    EnemyTurn,
}

public interface IBattleMessenger
{
    public void ReceiveMessage(BattleMessage message);
}

public class BattleMediator : MonoBehaviour, IBattleMessenger
{
    public static BattleMediator Instance { private set; get; }

    public BattleState state;

    List<IBattleMessenger> allies;
    List<IBattleMessenger> enemies;

    Player player;

    bool alliesSet;
    bool enemiesSet;

    bool inBattle;

    [SerializeField]
    float waitTime = 2.0f;
    float elaspedWaitTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        alliesSet = false;
        enemiesSet = false;

        inBattle = false;

        elaspedWaitTime = 0.0f;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (inBattle == true)
        {
            switch (state)
            {
                case BattleState.Start:
                    break;
                case BattleState.End:
                    break;
                case BattleState.PlayerTurn:
                    break;
                case BattleState.EnemyTurn:
                    break;
            }
        }
    }

    public void SetAllies(List<IBattleMessenger> allies)
    {
        alliesSet = true;

        this.allies = allies;
    }

    public void SetEnemies(List<IBattleMessenger> enemies)
    {
        enemiesSet = true;

        this.enemies = enemies;
    }

    public void StartBattle()
    {
        if(!alliesSet || !enemiesSet)
        {
            Debug.Log("You have not set enemies or allies!");
        }
        else if (!inBattle)
        {
            inBattle = true;
            state = BattleState.Start;

            UIManager.Instance.ShowUI(UIType.Battle);
        }
    }

    public void EndBattle()
    {
        alliesSet = false;
        enemiesSet = false;

        inBattle = false;

        UIManager.Instance.HideUI();
    }

    public void RequestAllies()
    {
        player.StartBattle();
    }

    public void ReceiveMessage(BattleMessage message)
    {
        switch (message.type)
        {
            case "enemy_take_damage":

                BattleMessage msg = new BattleMessage("take_damage");
                msg.data.Add("damage", message.data["damage"]);

                this.enemies[(int)message.data["enemy_index"]].ReceiveMessage(msg);
                break;
        }
    }
}
