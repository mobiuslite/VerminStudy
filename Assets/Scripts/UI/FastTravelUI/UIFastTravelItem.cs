using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIFastTravelItem : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void SetText(string textString)
    {
        text.text = textString;
    }

    public void OnClick()
    {
        if(text.text != "Exit Menu")
            FastTravelManager.Instance.TravelToSpot(text.text);

        UIManager.Instance.HideUI();
    }

}
