using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelManager : MonoBehaviour
{
    public static FastTravelManager Instance { get; private set; }

    public List<FastTravelSpot> FastTravelSpots { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        FastTravelSpots = new List<FastTravelSpot>();

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddFastTravelSpot(FastTravelSpot spot)
    {
        foreach(FastTravelSpot s in FastTravelSpots)
        {
            if (s == spot)
                return;
        }

        FastTravelSpots.Add(spot);
    }

    public void TravelToSpot(string spotName)
    {
        FastTravelSpot spotToTravelTo = FastTravelSpots.Find((spot) => spot.GetSpotName() == spotName);

        GameObject.FindGameObjectWithTag("Player").GetComponent<OldMovement>().TravelToSpot(spotToTravelTo.GetSpawnPoint());
    }
}
