using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    private enum AttackType
    {
        Normal,
        Horizontal,
        Both
    }

    public static MinigameManager Instance { get; private set; }

    Camera minigameCamera;

    List<GameObject> projectiles = new List<GameObject>();
    GameObject playerObject = null;

    [SerializeField]
    List<GameObject> projectileSpritePrefabs = new List<GameObject>();

    [SerializeField]
    GameObject PlayerSpritePrefab;

    enum MinigameState { inProgress, idle }

    private MinigameState currentState;
    AttackType attackType;

    float spawnTime = 0.6f;
    float elapsedSpawnTime = 0.0f;
    float actualSpawnTime;

    float gameTime = 6.0f;
    float elapsedGameTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        if(playerObject == null)
        {
            playerObject = Instantiate(PlayerSpritePrefab, transform.position, Quaternion.identity);
            playerObject.SetActive(false);
        }

        currentState = MinigameState.idle;

        minigameCamera = GameObject.FindGameObjectWithTag("MinigameCamera").GetComponent<Camera>();
        minigameCamera.enabled = false;
    }
        
    public void StartMinigame()
    {
        playerObject.SetActive(true);
        playerObject.GetComponent<MiniPlayerScript>().AllowMovement(true);
        minigameCamera.enabled = true;

        InputManager.Instance.AllowMoving(false);

        currentState = MinigameState.inProgress;

        actualSpawnTime = spawnTime * Random.Range(0.4f, 1.2f);

        int randomType = Random.Range(0, 11);

        if(randomType <= 3)
        {
            attackType = AttackType.Normal;
        }
        else if(randomType <= 7)
        {
            attackType = AttackType.Horizontal;
        }
        else
        {
            attackType = AttackType.Both;
        }
    }

    void EndMinigame()
    {       
        playerObject.GetComponent<MiniPlayerScript>().AllowMovement(false);
        minigameCamera.enabled = false;

        InputManager.Instance.AllowMoving(true);

        currentState = MinigameState.idle;

        BattleMessage msg = new BattleMessage("enemy_done_attacking");
        BattleMediator.Instance.ReceiveMessage(msg);

        playerObject.SetActive(false);

        foreach(GameObject obj in projectiles)
        {
            Destroy(obj);
        }

        projectiles.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MinigameState.idle:
                break;
            case MinigameState.inProgress:

                elapsedGameTime += Time.deltaTime;
                elapsedSpawnTime += Time.deltaTime;

                if(elapsedSpawnTime >= actualSpawnTime)
                {
                    if (attackType == AttackType.Normal)
                    {
                        SimpleProj();
                    }
                    else if (attackType == AttackType.Horizontal)
                    {
                        HorizontalProj();
                    }
                    else
                    {
                        if(Random.Range(0, 2) == 1)
                        {
                            HorizontalProj();
                        }
                        else
                        {
                            SimpleProj();
                        }
                    }

                    elapsedSpawnTime = 0.0f;
                }

                if(elapsedGameTime >= gameTime)
                {
                    elapsedGameTime = 0.0f;
                    EndMinigame();
                }

                break;
        }
    }

    public void HorizontalProj()
    {
        bool startLeft = Random.Range(0, 2) == 1;

        Vector3 startPos = new(transform.position.x + (startLeft ? 3.2f : -3.2f), playerObject.transform.position.y + Random.Range(-2f, 2f), transform.position.z);

        GameObject proj = Instantiate(projectileSpritePrefabs[0], startPos, Quaternion.identity);

        ProjectileScript projScript = proj.GetComponent<ProjectileScript>();
        projScript.SetParent(this);
        projScript.SetDirection(startLeft ? Vector3.left : Vector3.right);
        projScript.SetSpeed(4f);

        projectiles.Add(proj);
    }

    public void SimpleProj()
    {
        Vector3 startPos = new(playerObject.transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + 4.0f, transform.position.z);

        GameObject proj = Instantiate(projectileSpritePrefabs[0], startPos, Quaternion.identity);
        proj.GetComponent<ProjectileScript>().SetParent(this);
        projectiles.Add(proj);
    }

    public void RemoveProjectile(GameObject proj)
    {
        projectiles.Remove(proj);
    }
}
