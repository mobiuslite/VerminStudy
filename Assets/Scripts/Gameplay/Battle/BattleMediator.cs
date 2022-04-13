using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BattleMessage
{
    public BattleMessage(string type)
    {
        this.type = type;
        data = new Dictionary<string, float>();
    }

    public string type;
    public Dictionary<string, float> data;

    public IBattleMessenger who = null;
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

    private MinigameManager minigameManager;

    List<IBattleMessenger> allies;
    List<IBattleMessenger> enemies;

    Player player;

    bool alliesSet;
    bool enemiesSet;

    bool inBattle;

    [SerializeField]
    float waitTime = 2.0f;
    float elaspedWaitTime;

    [SerializeField]
    AudioMixer masterMixer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        minigameManager = GameObject.FindGameObjectWithTag("minigameManager").GetComponent<MinigameManager>();

        alliesSet = false;
        enemiesSet = false;

        inBattle = false;

        elaspedWaitTime = 0.0f;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if(elaspedWaitTime < waitTime)
            elaspedWaitTime += Time.deltaTime;
        else if (inBattle && elaspedWaitTime >= waitTime)
        {
            switch (state)
            {
                case BattleState.Start:
                    state = BattleState.PlayerTurn;
                    break;
                case BattleState.End:
                    break;
                case BattleState.PlayerTurn:
                    break;
                case BattleState.EnemyTurn:
                    {
                        //foreach(IBattleMessenger messenger in this.enemies)
                        //{
                        //    BattleMessage msg = new BattleMessage("request_action");
                        //    msg.data.Add("num_party", (float)this.allies.Count);

                        //    messenger.ReceiveMessage(msg);
                        //}

                        this.state = BattleState.PlayerTurn;
                    }
                    break;
            }         
        }
    }

    public void SetAllies(List<IBattleMessenger> allies)
    {
        if (!this.inBattle)
        {
            alliesSet = true;

            this.allies = allies;
        }
    }

    public void SetEnemies(List<IBattleMessenger> enemies)
    {
        if (!this.inBattle)
        {
            enemiesSet = true;

            this.enemies = enemies;
        }
    }

    public void StartBattle()
    {
        if(!alliesSet || !enemiesSet)
        {
            Debug.Log("You have not set enemies or allies!");
        }
        //Start battle!!
        else if (!inBattle)
        {
            inBattle = true;
            state = BattleState.Start;

            UIManager.Instance.ShowUI(UIType.Battle);

            masterMixer.SetFloat("BattleVolume", 0.0f);
            masterMixer.SetFloat("EnvVolume", -80.0f);
        }
    }

    public void EndBattle()
    {
        alliesSet = false;
        enemiesSet = false;

        inBattle = false;

        UIManager.Instance.HideUI();

        foreach (IBattleMessenger messenger in this.enemies)
        {
            BattleMessage msg = new BattleMessage("end_battle");
            messenger.ReceiveMessage(msg);
        }

        foreach (IBattleMessenger messenger in this.allies)
        {
            BattleMessage msg = new BattleMessage("end_battle");
            messenger.ReceiveMessage(msg);
        }

        this.state = BattleState.End;

        masterMixer.SetFloat("BattleVolume", -80.0f);
        masterMixer.SetFloat("EnvVolume", 0.0f);
    }

    public void RequestAllies()
    {
        if (!this.inBattle)
        {
            player.StartBattle();
        }
    }

    public void ReceiveMessage(BattleMessage message)
    {
        if (this.inBattle)
        {
            switch (message.type)
            {
                case "enemy_take_damage":
                    {
                        if (this.state == BattleState.PlayerTurn)
                        {
                            BattleMessage msg = new BattleMessage("take_damage");
                            msg.data.Add("damage", message.data["damage"]);

                            this.enemies[(int)message.data["enemy_index"]].ReceiveMessage(msg);

                            this.state = BattleState.EnemyTurn;
                            elaspedWaitTime = 0.0f;
                        }
                        break;
                    }
                case "allies_take_damage":
                    {
                        if (this.state == BattleState.EnemyTurn)
                        {
                            BattleMessage msg = new BattleMessage("take_damage");
                            msg.data.Add("damage", message.data["damage"]);

                            this.allies[(int)message.data["party_index"]].ReceiveMessage(msg);

                            elaspedWaitTime = 0.0f;
                        }
                        break;
                    }
                case "dead":
                    {
                        this.allies.Remove(message.who);
                        this.enemies.Remove(message.who);

                        if (allies.Count == 0 || enemies.Count == 0)
                        {
                            this.EndBattle();
                        }
                        break;
                    }
                default:
                    Debug.LogError("Wrong message type sent");
                    break;
            }
        }
    }
}
