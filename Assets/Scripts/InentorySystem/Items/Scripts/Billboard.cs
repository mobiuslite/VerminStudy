using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    Camera mCamera;
    private void LateUpdate()
    {
        if(mCamera)
        {
            transform.forward = mCamera.transform.forward;
        }
        else
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
