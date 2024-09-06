using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public bool canBeDamaged;
    public float health { get; protected set; }

    [SerializeField]
    protected float startingHealth = 100f;

    private void Start()
    {
        
    }

    public void RecoverHP(int amount)
    {
        health += amount;
    }

    public bool ApplyDamage(DamageMessage msg)
    {
        if(canBeDamaged == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
