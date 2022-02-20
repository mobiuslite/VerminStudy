using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{

    public static DayNightSystem Instance { get; private set; }

    [SerializeField]
    Light sunLight;
    [SerializeField]
    float sunLightIntensity = 3.0f;
    [SerializeField]
    Light moonLight;
    [SerializeField]
    float moonLightIntensity = 0.5f;

    [Space(10)]
    [SerializeField]
    float fullDayTime = 30.0f;
    [SerializeField]
    float dawnAngle = -23.0f, duskAngle = 200.0f;
    [SerializeField]
    [Range(0, 1.0f)]
    float dayNightRatio;
    [SerializeField]
    [Range(0.0f, 0.25f)]
    float transitionLerpTime;

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

        if(curTime > fullDayTime)
        {
            curTime = 0.0f;
        }

        float lerpTime = curTime / fullDayTime;
        transform.rotation = Quaternion.Euler(Mathf.Lerp(dawnAngle, duskAngle, lerpTime), 0.0f, 0.0f);

        //Sun Rising
        if(lerpTime <= transitionLerpTime)
        {
            float dawnLerpTime = lerpTime / transitionLerpTime;
            
            sunLight.intensity = Mathf.Lerp(0.0f, sunLightIntensity, dawnLerpTime);
            moonLight.intensity = Mathf.Lerp(moonLightIntensity, 0.0f, dawnLerpTime);

            sunLight.enabled = true;
            if (moonLight.intensity < 0.01f)
                moonLight.enabled = false;
        }

        //Sun falling
        else if(lerpTime >= dayNightRatio - transitionLerpTime && lerpTime < dayNightRatio)
        {
            float duskLerpTime = (lerpTime - (dayNightRatio - transitionLerpTime)) * (1.0f / transitionLerpTime);

            sunLight.intensity = Mathf.Lerp(sunLightIntensity, 0.0f, duskLerpTime);
            moonLight.intensity = Mathf.Lerp(0.0f, moonLightIntensity, duskLerpTime);
        
            if (sunLight.intensity < 0.95f)
            {
                moonLight.enabled = true;
                sunLight.enabled = false;
            }        
        }
    }

    public float GetCurrentTime()
    {
        return curTime;
    }

    public bool IsDay()
    {
        float lerpTime = curTime / fullDayTime;
        return lerpTime <= dayNightRatio;
    }
}
