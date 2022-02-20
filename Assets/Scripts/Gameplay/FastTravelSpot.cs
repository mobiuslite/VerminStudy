using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class FastTravelSpot : MonoBehaviour
{
    [SerializeField]
    string spotName;

    [Space(10)]
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    Transform spawnPoint;

    BoxCollider trigger;

    private void Start()
    {
        trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;

        text.enabled = false;

        //Register this spot with the manager
        FastTravelManager.Instance.AddFastTravelSpot(this);
    }

    private void Update()
    {
        //If the player is next to a spot and interacts with it.
        if(text.enabled == true && InputManager.Instance.GetInteract())
        {
            UIManager.Instance.ShowUI(UIType.FastTravel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        text.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        text.enabled = false;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }

    public string GetSpotName()
    {
        return spotName;
    }

}
