using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float movementSpeed, jumpHeight;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Transform groundDetector;

    [SerializeField]
    SpriteRenderer sprite;

    //Testing a spin when turning
    [SerializeField]
    bool useYFlip;
    public Animator flipAmimation;

    private bool isOnGround;
    private bool movingBackwards;
    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        input.Normalize();

        rb.velocity = new Vector3(input.x * movementSpeed, rb.velocity.y, input.y * movementSpeed);

        RaycastHit rch;
        if (Physics.Raycast(groundDetector.position, Vector3.down, out rch, .3f, groundLayer))
        {
            isOnGround = true;
            Debug.Log("ON GROUND");
        }
        else
        {
            isOnGround = false;
            Debug.Log("OFF GROUND");
        }

        //Jumping
        if(Input.GetButton("Jump") && isOnGround)
        {
            rb.velocity += new Vector3(0f, jumpHeight, 0f);
        }

        //Flip sprite left and right
        if(!sprite.flipX && input.x < 0)
        {
            sprite.flipX = true;
            if (useYFlip)
            {
                flipAmimation.SetTrigger("FlipTrigger");
            }
        }
        else if(sprite.flipX && input.x > 0)
        {
            sprite.flipX = false;
            if (useYFlip)
            {
                flipAmimation.SetTrigger("FlipTrigger");
            }
        }

        //Flip sprite when going backwards
        if (!movingBackwards && input.y > 0)
        {
            movingBackwards = true;
        }
        else if (movingBackwards && input.y < 0)
        {
            movingBackwards = false;
        }


    }
}
