using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
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

    float spawnTime = 0.6f;
    float elapsedSpawnTime = 0.0f;

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

                if(elapsedSpawnTime >= spawnTime)
                {
                    Vector3 startPos = new Vector3(playerObject.transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + 4.0f, transform.position.z);

                    GameObject proj = Instantiate(projectileSpritePrefabs[0], startPos, Quaternion.identity);
                    proj.GetComponent<ProjectileScript>().SetParent(this);
                    projectiles.Add(proj);

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

    public void RemoveProjectile(GameObject proj)
    {
        projectiles.Remove(proj);
    }
}
