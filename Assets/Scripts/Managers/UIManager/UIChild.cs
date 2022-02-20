using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChild : MonoBehaviour
{
    [SerializeField]
    UIType type;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public UIType GetUIType()
    {
        return type;
    }
}
