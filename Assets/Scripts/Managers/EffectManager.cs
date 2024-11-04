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

    public ParticleSystem fleshHitEffectPrefab;

    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent, EffectType effectType)
    {
        var effect = ObjectPoolManager.Instance.GetPoolableObject(ObjectPoolManager.ObjectType.EFFECT).GetComponent<IPoolableObject>();
        effect.Initialize(pos, Quaternion.LookRotation(normal), parent);
    }
}