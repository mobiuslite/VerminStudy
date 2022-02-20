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

        direction.x = InputManager.Instance.GetHorizontal();
        direction.z = InputManager.Instance.GetVertical();

        isFlipped = direction.x < 0.0f;

        rb.MovePosition(rb.position + direction.normalized * movementAmount * Time.deltaTime);
        spriteRenderer.flipX = isFlipped;
    }

    public void TravelToSpot(Vector3 pos)
    {
        rb.MovePosition(pos);
    }
}