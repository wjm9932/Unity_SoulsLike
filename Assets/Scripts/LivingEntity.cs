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

    public bool RecoverHP(int amount)
    {
        if(health >= startingHealth)
        {
            return false;
        }
        else
        {
            health = Mathf.Min(health + amount, startingHealth);
            return true;
        }
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
