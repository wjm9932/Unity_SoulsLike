using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public bool canBeDamaged;
    public float health { get; protected set; }
    public bool canAttack { get; private set; }
    public float damage { get; private set; }
    public event Action onDeath;

    [SerializeField]
    private float _maxHealth = 100f;

    public float maxHealth
    {
        get { return _maxHealth; }
    }

    private void Start()
    {
        canAttack = false;
    }

    public bool RecoverHP(float amount)
    {
        if(health >= maxHealth)
        {
            return false;
        }
        else
        {
            health = Mathf.Min(health + amount, maxHealth);
            return true;
        }
    }

    public bool IsMaxHp()
    {
        return health >= _maxHealth;
    }

    public bool ApplyDamage(float damage)
    {
        if (this.canBeDamaged == true)
        {
            health -= damage;
            
            if(health <= 0)
            {
                if(onDeath != null)
                {
                    onDeath();
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetCanAttack(int flag)
    {
        if (flag == 1)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
