using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private Animator openAnimator;
    [SerializeField]
    ItemObject heldItem;
    [SerializeField]
    GameObject prefab;

    private bool wasOpened = false;
    public void Start()
    {
        if(openAnimator != null && openAnimator.isActiveAndEnabled)
        {
            openAnimator.Play("Idle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!wasOpened)
        {
            openAnimator = gameObject.GetComponent<Animator>();

            openAnimator.SetTrigger("OpenCollisionTrigger");


            Debug.Log("Dropped item");


            prefab.GetComponent<GroundItem>().item = heldItem;


            prefab.GetComponentInChildren<SpriteRenderer>().sprite = heldItem.uiSprite;

            GameObject newItem = Instantiate(prefab, gameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f), Quaternion.identity);

            newItem.GetComponent<Rigidbody>().velocity = new Vector3(2.0f, 2.0f, 0.0f);
            newItem.tag = "Item";
            wasOpened = true;
        }
        
    }
}
