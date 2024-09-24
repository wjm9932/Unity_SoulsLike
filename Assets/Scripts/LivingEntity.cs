using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    private bool _canBeDamaged;
    public virtual bool canBeDamaged
    {
        get { return _canBeDamaged; }
        set { _canBeDamaged = value; }
    }

    private float _health;
    public virtual float health 
    {
        get { return _health; }
        protected set
        {
            _health = value;
        }
    }
    public bool canAttack { get; private set; }
    public float damage { get; private set; }
    public event Action onDeath;

    [SerializeField]
    Transform damageTextPosition;

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
            canBeDamaged = false;
            
            if(health <= 0)
            {
                if(onDeath != null)
                {
                    onDeath();
                }
            }

            TextManager.Instance.PlayDamageText(damageTextPosition.position, damageTextPosition, damage);
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
