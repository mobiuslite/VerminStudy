using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    float minIntensity, maxIntensity;
    [SerializeField]
    [Range(0, 0.5f)]
    float minRandomTime, maxRandomTime;

    Light lightToFlicker;

    float timeToChange;
    float elapsedTime;

    private void Start()
    {
        lightToFlicker = GetComponent<Light>();
        GetNewRandomTime();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= timeToChange)
        {
            GetNewRandomTime();
        }

        lightToFlicker.intensity = Mathf.Lerp(minIntensity, maxIntensity, elapsedTime / timeToChange);

    }

    void GetNewRandomTime()
    {
        timeToChange = Random.Range(minRandomTime, maxRandomTime);
        elapsedTime = 0.0f;
    }
}
