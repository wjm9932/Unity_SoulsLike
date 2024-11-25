using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    [Header("NavMesh")]
    public LayerMask whatIsTarget;
    public Transform eyeTransform;

    [Header("Drop Item")]
    [SerializeField] protected GameObject[] dropItem;

    [Header("Health Bar")]
    [SerializeField] public EnemyHealthBar hpBar;

    [Header("Lock On Indicator")]
    public Image lockOnIndicator;

    [Header("Minimap Icon")]
    [SerializeField] private GameObject minimapIcon;


    public float viewDistance { get; protected set; }
    public float fieldOfView { get; protected set; }
    public float trackingStopDistance { get; protected set; }
    public float trackingSpeed { get; protected set; }
    public NavMeshAgent navMesh { get; private set; }
    public GameObject target { get; set; }

    [Header("Foot Step Sound")]
    [SerializeField] private AudioClip[] footStepClips;
    private AudioSource audioSource;

    public GameObject attackSound { get; set; }

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        navMesh = GetComponent<NavMeshAgent>();
        canBeDamaged = true;
        isDead = false;
    }
    public void SetNavMeshArea(int mask)
    {
        navMesh.areaMask = 1 << mask;
    }
    protected virtual void OnEnemyTriggerStay(GameObject target, Collider collider)
    {
        var hitPoint = collider.ClosestPoint(transform.position);
        Vector3 hitNormal = (hitPoint - transform.position).normalized;
        EffectManager.Instance.PlayEffect(hitPoint, hitNormal, transform, ObjectPoolManager.ObjectType.EFFECT);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            LivingEntity player = other.transform.root.GetComponent<LivingEntity>();

            if (player != null && player.canAttack == true)
            {
                if (ApplyDamage(player) == true)
                {
                    OnEnemyTriggerStay(player.gameObject, other);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slash"))
        {
            LivingEntity player = other.GetComponent<Slash>().owner.GetComponent<LivingEntity>();
            if (player != null)
            {
                if (ApplyDamage(player) == true)
                {
                    OnEnemyTriggerStay(player.gameObject, other);
                }
            }
        }
    }
    public override void Die()
    {
        base.Die();

        navMesh.enabled = false;
        minimapIcon.SetActive(false);
        hpBar.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        DropItem();
        Destroy(this.gameObject, 3f);
    }
    protected void DropItem()
    {
        for (int i = 0; i < dropItem.Length; i++)
        {
            UX_Item item = dropItem[i].GetComponent<UX_Item>();

            if (IsDrop(item.data.dropChance) == true)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 0.5f; 
                Vector3 dropPosition = this.gameObject.transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

                Instantiate(dropItem[i], dropPosition, Quaternion.identity);
            }
        }
    }
    private void PlayFootStepSound()
    {
        int index = Random.Range(0, footStepClips.Length);
        audioSource.PlayOneShot(footStepClips[index]);
    }
    private bool IsDrop(float chances)
    {
        return Random.Range(0f, 1f) <= chances;
    }
}
