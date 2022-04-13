using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryLoad : MonoBehaviour
{
    public static DontDestoryLoad Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }     
    }
}
