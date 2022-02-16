using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float smoothAmount;

    Vector3 desiredCameraPos;
    Vector3 smoothCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        desiredCameraPos = target.position + offset;
        smoothCameraPos = desiredCameraPos;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        desiredCameraPos = target.position + offset;
        smoothCameraPos = Vector3.Lerp(transform.position, desiredCameraPos, Time.deltaTime * smoothAmount);
        transform.position = smoothCameraPos;
    }
}
