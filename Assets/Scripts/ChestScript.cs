using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestScript : MonoBehaviour
{
    private Animator openAnimator;
    [SerializeField]
    ItemObject[] heldItems;

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

            for (int i = 0; i < heldItems.Length; i++)
            {
                ItemInstantiate.Instantiate(heldItems[i], transform.position);
            }

            wasOpened = true;
        }
        
    }
}
