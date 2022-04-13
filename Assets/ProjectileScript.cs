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

    float actualSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        actualSpeed = speed * Random.Range(0.7f, 1.3f);
    }

    private void Update()
    {
        //transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 10.0f * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.down * actualSpeed * Time.deltaTime);
        elapsedAliveTime += Time.deltaTime;
        if (elapsedAliveTime >= aliveTime)
        {
            parent.RemoveProjectile(gameObject);
            Destroy(gameObject);      
        }

        rb.MoveRotation(Quaternion.Euler(0.0f, 0.0f, rb.rotation.eulerAngles.z - 360.0f * Time.deltaTime));
    }

    public void SetParent(MinigameManager manager)
    {
        parent = manager;
    }
}
