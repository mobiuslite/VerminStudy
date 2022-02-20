using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NonSpatialSoundFalloff: MonoBehaviour
{
    float minDistance;
    float maxDistance;

    Transform player;
    AudioSource audioSource;

    float maxVolume;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        audioSource = GetComponent<AudioSource>();

        maxVolume = audioSource.volume;
        minDistance = audioSource.minDistance;
        maxDistance = audioSource.maxDistance;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < minDistance)
        {
            audioSource.volume = maxVolume;
            audioSource.enabled = true;
        }
        else if (distance > maxDistance)
        {
            audioSource.volume = 0;
            audioSource.enabled = false;
            audioSource.Stop();
        }
        else
        {
            float t = (distance - minDistance) / (maxDistance - minDistance);
            audioSource.volume = Mathf.Lerp(maxVolume, 0.0f, t);
            audioSource.enabled = true;
        }

    }

}
