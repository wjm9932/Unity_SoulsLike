using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Player FootStep")]
    [SerializeField] private AudioSource playerFootStepSoundSource;
    [SerializeField] private AudioClip[] footStepClips;

    public enum SoundEffectType
    {
        DODGE,
        DODGE_LANDING,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        DRINK,
        PICKUP,
    }
    [System.Serializable]
    private struct SoundEffectInfo
    {
        public SoundEffectType effectType;
        public AudioClip audioClip;
    }

    [SerializeField]
    private SoundEffectInfo[] effectInfos;

    private Dictionary<SoundEffectType, AudioClip> audioClips = new Dictionary<SoundEffectType, AudioClip>();
 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        for(int i = 0; i < effectInfos.Length; i++)
        {
            audioClips.Add(effectInfos[i].effectType, effectInfos[i].audioClip);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayFootStepSound()
    {
        int index = Random.Range(0, footStepClips.Length);
        playerFootStepSoundSource.PlayOneShot(footStepClips[index]);
    }

    public void PlaySoundEffect(SoundEffectType type, float volume = 0.3f)
    {
        var audioSource = ObjectPoolManager.Instance.GetPoolableObject(ObjectType.SOUND);
        audioSource.GetComponent<AudioSource>().volume = volume;
        audioSource.GetComponent<AudioSource>().PlayOneShot(audioClips[type]);

    }
}
