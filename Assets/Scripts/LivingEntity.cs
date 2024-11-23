using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    CHARACTER,
    WARRIOR,
    ARCHER,
    TANK,
    BOSS
}

public abstract class LivingEntity : MonoBehaviour
{
    public Animator animator { get; private set; }
    [SerializeField] protected Transform damageTextPosition;

    [Header("Entitiy")]
    [SerializeField] private EntityType _entityType;
    public EntityType entityType { get { return _entityType; } protected set { _entityType = value; } }

    [SerializeField] private float _maxHealth = 100f;
    public virtual float maxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
        }
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

    public bool canAttack { get; set; }
    public float damage { get; protected set; }
    public float buffArmorPercent { get; set; }
    public bool isDead { get; protected set; }
    public bool canBeDamaged { get; set; }
    public event Action onDeath;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
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

    public virtual bool ApplyDamage(LivingEntity damager)
    {
        if (this.canBeDamaged == true)
        {
            canBeDamaged = false;

            float damageToApply = Mathf.RoundToInt(damager.damage * (1f - buffArmorPercent));
            health -= damageToApply;

            if (health <= 0)
            {
                Die();
                if(damager.GetComponent<Character>() != null)
                {
                    damager.GetComponent<Character>().KillLivingEntity(this.entityType);
                }
            }

            TextManager.Instance.PlayDamageText(damageTextPosition.position, damageTextPosition, damageToApply);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetCanAttack(int flag)
    {
        if (animator.IsInTransition(0) == true)
        {
            return;
        }

        if (flag == 1)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public virtual void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public virtual void Die()
    {
        if(onDeath != null)
        {
            onDeath();
        }
        isDead = true;
    }
}
