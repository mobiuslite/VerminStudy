using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChild : MonoBehaviour
{
    [SerializeField]
    UIType type;
    public bool UIActive = false;

    public void Show()
    {
        this.gameObject.SetActive(true);
        UIActive = true;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        UIActive = false;
    }

    public UIType GetUIType()
    {
        return type;
    }
}
