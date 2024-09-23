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

    [SerializeField]
    private TextMeshPro damageText;

    public ParticleSystem fleshHitEffectPrefab;

    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent, EffectType effectType)
    {
        var targetPrefab = fleshHitEffectPrefab;

        if (effectType == EffectType.Flesh)
        {
            targetPrefab = fleshHitEffectPrefab;
        }

        var effect = Instantiate(targetPrefab, pos, Quaternion.LookRotation(normal));

        if (parent != null)
        {
            effect.transform.SetParent(parent);
        }

        effect.Play();
    }

    public void PlayDamageText(Vector3 pos, Transform parent, float damage)
    {
        var text = Instantiate(damageText, pos, Quaternion.identity, parent);
        text.text = damage.ToString();
    }
}