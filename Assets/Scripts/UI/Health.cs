using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    GameObject healthBar;
    Renderer healthRenderer;

    private void Start()
    {
        healthBar = transform.GetChild(0).gameObject;
        healthRenderer = healthBar.GetComponent<MeshRenderer>();
    }

    public void ShowHealth()
    {
        healthRenderer.enabled = true;
    }

    public void HideHealth()
    {
        healthRenderer.enabled = false;
    }

    public void SetHealthScale(float scale)
    {
        gameObject.transform.localScale = new Vector3(scale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }
}
