using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectObjectPool : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }
    private ParticleSystem particle;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.isPlaying && !particle.IsAlive())
        {
            Release();
        }
    }
    public void Initialize(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        this.enabled = true;
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;

        if (parent != null)
        {
            this.transform.SetParent(parent);
        }
        particle.Play();
    }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }
    public void Release()
    {
        transform.parent = null;
        pool.Release(this.gameObject);
    }
}
