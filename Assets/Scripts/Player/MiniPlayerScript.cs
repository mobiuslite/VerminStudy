using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlayerScript : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float movementSpeed = 0.2f;

    bool canMove = false;
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

            rb.MovePosition(rb.position + new Vector3(input.x, input.y, 0.0f) * movementSpeed * Time.deltaTime);
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
}
