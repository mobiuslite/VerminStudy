using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float speed;
    [SerializeField]
    float aliveTime;

    float elapsedAliveTime = 0.0f;

    MinigameManager parent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.down * speed * Time.deltaTime);

        elapsedAliveTime += Time.deltaTime;
        if (elapsedAliveTime >= aliveTime)
        {
            parent.RemoveProjectile(gameObject);
            Destroy(gameObject);      
        }
    }

    public void SetParent(MinigameManager manager)
    {
        parent = manager;
    }
}
