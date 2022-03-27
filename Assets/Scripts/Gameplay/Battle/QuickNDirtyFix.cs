using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickNDirtyFix : MonoBehaviour
{
    [SerializeField]
    Button button;

    private void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        Player player = playerGO.GetComponent<Player>();

        button.onClick.AddListener(player.DamageEnemy);
    }
}
