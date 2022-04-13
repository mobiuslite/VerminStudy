using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlayerScript : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float movementSpeed = 0.2f;

    bool canMove = false;

    MinigameManager parent;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 input;
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input.Normalize();

            if(input.x > 0.0f && rb.position.x > parent.transform.position.x + 3.2f)
            {
                //don't move
            }
            else if(input.x < 0.0f && rb.position.x < parent.transform.position.x - 3.2f)
            {
                //don't move
            }
            else if (input.y > 0.0f && rb.position.y > parent.transform.position.y + 3.2f)
            {

            }
            else if (input.y < 0.0f && rb.position.y < parent.transform.position.y - 3.2f)
            {

            }
            else
            {
                rb.MovePosition(rb.position + new Vector3(input.x, input.y, 0.0f) * movementSpeed * Time.deltaTime);
            }              
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BattleMessage msg = new BattleMessage("allies_take_damage");
        msg.data.Add("damage", 15);
        msg.data.Add("party_index", 0);

        BattleMediator.Instance.ReceiveMessage(msg);
    }

    public void AllowMovement(bool state)
    {
        canMove = state;
    }

    public void SetParent(MinigameManager manager)
    {
        parent = manager;
    }
}
