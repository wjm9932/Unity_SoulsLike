using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour, IPoolableObject
{
    [HideInInspector]
    public LivingEntity parent;

    [SerializeField]
    private float speed = 20f;


    public IObjectPool<GameObject> pool { get; private set; }

    public Rigidbody rb { get; private set; }
    public TrailRenderer arrowEffect { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        arrowEffect = GetComponent<TrailRenderer>();
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        this.parent = parent.gameObject.GetComponent<LivingEntity>();
        this.enabled = true;
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;

        rb.isKinematic = false;
        arrowEffect.enabled = true;
        GetComponent<Collider>().enabled = true;

        rb.AddForce(this.transform.forward * speed, ForceMode.Impulse);
        StartCoroutine(Release(3f));
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") == true)
        {
            rb.isKinematic = true;
            arrowEffect.enabled = false;
            GetComponent<Collider>().enabled = false;
            transform.SetParent(other.transform);
            this.enabled = false;
        }
    }

    private IEnumerator Release(float delay)
    {
        yield return new WaitForSeconds(delay);
        Release();
    }

    public void Release()
    {
        transform.SetParent(null);
        pool.Release(this.gameObject);
    }
}
