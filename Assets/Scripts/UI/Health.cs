using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    GameObject healthBar;

    private void Awake()
    {
        healthBar = transform.GetChild(0).gameObject;
    }

    public void ShowHealth()
    {
        healthBar.SetActive(true);
    }

    public void HideHealth()
    {
        healthBar.SetActive(false);
    }

    public void SetHealthScale(float scale)
    {
        gameObject.transform.localScale = new Vector3(scale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }
}
