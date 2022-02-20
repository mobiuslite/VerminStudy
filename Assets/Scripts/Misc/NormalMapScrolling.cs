using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMapScrolling : MonoBehaviour
{
    Renderer render;

    [SerializeField]
    Vector2 scrollSpeed;
    Vector2 offset = Vector2.zero;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

    private void Update()
    {

        render.material.SetTextureOffset("_BumpMap", new Vector2(offset.x, offset.y));
        offset += scrollSpeed * Time.deltaTime;

        Debug.Log(offset);
    }
}
