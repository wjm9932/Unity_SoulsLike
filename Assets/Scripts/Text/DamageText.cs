using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageText : MonoBehaviour, IPoolableObject
{
    private float time;
    public IObjectPool<GameObject> pool { get; private set; }

    private void Awake()
    {
        time = 2f;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - Camera.main.transform.position;
        direction.y = 0; 

        transform.rotation = Quaternion.LookRotation(direction);
    }
    public void Initialize(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;
        this.transform.SetParent(parent); 
        
        Invoke("Release", time);
    }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        this.transform.SetParent(null);
        time = 2f;
        pool.Release(this.gameObject);
    }
}
