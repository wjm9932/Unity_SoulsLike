using TMPro;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    public enum EffectType
    {
        Flesh,
        Wall
    }

    public void PlayEffect(Vector3 pos, Vector3 normal, Transform parent, ObjectPoolManager.ObjectType effectType)
    {
        var effect = ObjectPoolManager.Instance.GetPoolableObject(effectType).GetComponent<IPoolableObject>();
        effect.Initialize(pos, Quaternion.LookRotation(normal), parent);
    }
}