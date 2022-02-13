using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class OldMovement : MonoBehaviour
{
    [SerializeField]
    float movementAmount;

    Rigidbody rb;
    SpriteRenderer spriteRenderer;

    bool isFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
            isFlipped = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
            isFlipped = false;
        }

        rb.MovePosition(rb.position + direction.normalized * movementAmount * Time.deltaTime);
        spriteRenderer.flipX = isFlipped;
    }
}