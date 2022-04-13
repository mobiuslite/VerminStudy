using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{

    float timer = 0.0f;

    List<GameObject> projectiles = new List<GameObject>();
    GameObject playerObject = null;

    [SerializeField]
    List<GameObject> projectileSpritePrefabs = new List<GameObject>();

    [SerializeField]
    GameObject PlayerSpritePrefab;

    enum minigameState { start, end, inProgress, idle }

    private minigameState currentState;

    // Start is called before the first frame update
    void Awake()
    {
        if(playerObject == null)
        {
            playerObject = Instantiate(PlayerSpritePrefab);
        }

        currentState = minigameState.idle;
        startMinigame();
    }
        
    public void startMinigame()
    {
        timer = 10.0f;
        currentState = minigameState.inProgress;
        for (int i = 0; i < 6; i++) {
            projectiles.Add(Instantiate(projectileSpritePrefabs[0]));
            projectiles[i].transform.position = new Vector3(0.0f, 10.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case minigameState.idle:
                break;
            case minigameState.inProgress:
                timer -= Time.deltaTime;
                for(int timeGate = 0; timeGate < 6; timeGate++)
                {
                    if(timer < (10 - (timeGate * 10.0f / 6.0f)))
                    {
                        if (!projectiles[timeGate].GetComponent<Rigidbody2D>().IsAwake())
                        {
                            projectiles[timeGate].GetComponent<Rigidbody2D>().WakeUp();
                            projectiles[timeGate].GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0.2f);

                        }
                    }
                }
                break;
            case minigameState.end:
                break;
        }
    }
}
