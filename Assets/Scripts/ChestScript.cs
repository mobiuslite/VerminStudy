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
                ItemObject curItem = heldItems[i];

                GameObject basePrefab = GameObject.FindGameObjectWithTag("BaseItemPrefab");
                basePrefab.name = curItem.data.Name;
                basePrefab.GetComponent<GroundItem>().item = curItem;

                Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

                basePrefab.GetComponentInChildren<SpriteRenderer>().sprite = curItem.uiSprite;

                GameObject newItem = Instantiate(basePrefab, playerPos + new Vector3(0.0f, 0.0f, -1.4f), Quaternion.identity);

                newItem.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2.0f, 2.0f), 2.5f, -0.5f);
                newItem.tag = "Item";
            }

            wasOpened = true;
        }
        
    }
}
