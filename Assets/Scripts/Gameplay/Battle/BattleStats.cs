using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStats : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 100.0f;
    [SerializeField]
    float health = 100.0f;

    [SerializeField]
    float damage = 25.0f;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public float GetDamageAmount()
    {
        return damage;
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
