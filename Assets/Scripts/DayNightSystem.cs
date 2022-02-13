using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{

    public static DayNightSystem Instance { get; private set; }

    [SerializeField]
    Light sunLight;
    [SerializeField]
    Light moonLight;

    [SerializeField]
    float dayTime = 30.0f;
    [SerializeField]
    float dawnAngle = -23.0f;
    [SerializeField]
    float duskAngle = 200.0f;

    float curTime = 0.0f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Removes the daynight script from whatever this object is.
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if(curTime > dayTime)
        {
            curTime = 0.0f;
        }

        transform.rotation = Quaternion.Euler(Mathf.Lerp(dawnAngle, duskAngle, curTime / dayTime), 0.0f, 0.0f);
    }

    public float GetCurrentTime()
    {
        return curTime;
    }
}
