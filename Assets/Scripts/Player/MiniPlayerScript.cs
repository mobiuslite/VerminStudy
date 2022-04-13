using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlayerScript : MonoBehaviour
{

    Vector2 input;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    float movementSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        input.Normalize();

        rb.velocity = new Vector2(input.x * movementSpeed, input.y * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
