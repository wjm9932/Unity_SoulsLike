using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundObjectPool : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }
    public AudioSource audioSource { get; private set; }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(audioSource.isPlaying == false)
        {
            transform.parent = null;
            pool.Release(this.gameObject);
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
    }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }
}
