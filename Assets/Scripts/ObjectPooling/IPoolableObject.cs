using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolableObject
{
    public IObjectPool<GameObject> pool { get; }
    public void SetPool(IObjectPool<GameObject> pool);
    public void Initialize(Vector3 position, Quaternion rotation, Transform parent = null);
}
