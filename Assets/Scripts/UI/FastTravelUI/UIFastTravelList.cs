using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFastTravelList : MonoBehaviour
{
    [SerializeField]
    GameObject listTemplate;

    bool loadedList = false;
    private void Update()
    {
        if(!loadedList && FastTravelManager.Instance != null)
        {
            LoadList();
            loadedList = true;
        }
    }

    void LoadList()
    {
        List<FastTravelSpot> spots = FastTravelManager.Instance.FastTravelSpots;

        foreach (FastTravelSpot spot in spots)
        {
            UIFastTravelItem newItem = Instantiate(listTemplate).GetComponent<UIFastTravelItem>();
            newItem.SetText(spot.GetSpotName());

            newItem.transform.SetParent(listTemplate.transform.parent, false);
        }
    }
}
