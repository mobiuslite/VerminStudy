using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class OldMovement : MonoBehaviour
{
    [SerializeField]
    float movementAmount;
	[SerializeField]
	float smoothingAmount;

    Rigidbody rb;
    SpriteRenderer spriteRenderer;

    bool isFlipped = false;

    Vector3 velocity = Vector3.zero;

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

		Vector3 targetMovement = direction.normalized * movementAmount;

        float veloXSmoothing = 0.0f;
        float veloYSmoothing = 0.0f;

		velocity.x = Mathf.SmoothDamp(velocity.x, targetMovement.x, ref veloXSmoothing, smoothingAmount);
		velocity.z = Mathf.SmoothDamp(velocity.z, targetMovement.z, ref veloYSmoothing, smoothingAmount);

        rb.MovePosition(rb.position + velocity * Time.deltaTime);
        spriteRenderer.flipX = isFlipped;
    }

    public void TravelToSpot(Vector3 pos)
    {
        rb.MovePosition(pos);
    }
}