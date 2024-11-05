using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private struct EnemySoundEffect
    { 
    }


    public enum SoundEffectType
    {
        DODGE,
        DODGE_LANDING,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        WARRIOR_ENEMY_ATTACK,
        DRINK,
        PICKUP,
        PLAYER_HIT,
        ENEMY_HIT,
        ENEMY_DIE,
    }
    [System.Serializable]
    private struct SoundEffectInfo
    {
        public SoundEffectType effectType;
        public AudioClip[] audioClips;
    }

    [SerializeField]
    private SoundEffectInfo[] effectInfos;

    private Dictionary<SoundEffectType, List<AudioClip>> audioClipsContainer = new Dictionary<SoundEffectType, List<AudioClip>>();
 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        for(int i = 0; i < effectInfos.Length; i++)
        {
            List<AudioClip> clips = new List<AudioClip>(effectInfos[i].audioClips.Length);

            for(int j = 0; j < effectInfos[i].audioClips.Length; j++)
            {
                clips.Add(effectInfos[i].audioClips[j]);
            }
            
            audioClipsContainer.Add(effectInfos[i].effectType, clips);
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

    public void Play2DSoundEffect(SoundEffectType type, float volume = 0.2f)
    {
        var audioSource = ObjectPoolManager.Instance.GetPoolableObject(ObjectType.SOUND);
        audioSource.GetComponent<AudioSource>().volume = volume;
        audioSource.GetComponent<AudioSource>().spatialBlend = 0f;

        if (audioClipsContainer[type].Count == 1)
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(audioClipsContainer[type][0]);
        }
        else
        {
            int index = Random.Range(0, audioClipsContainer[type].Count);
            audioSource.GetComponent<AudioSource>().PlayOneShot(audioClipsContainer[type][index]);
        }
    }

    public void Play3DSoundEffect(SoundEffectType type, float volume, Vector3 position, Quaternion rotation, Transform parent)
    {
        var audioSource = ObjectPoolManager.Instance.GetPoolableObject(ObjectType.SOUND);
        audioSource.GetComponent<AudioSource>().volume = volume;
        audioSource.GetComponent<AudioSource>().spatialBlend = 1f;

        if (audioClipsContainer[type].Count == 1)
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(audioClipsContainer[type][0]);
        }
        else
        {
            int index = Random.Range(0, audioClipsContainer[type].Count);
            audioSource.GetComponent<AudioSource>().PlayOneShot(audioClipsContainer[type][index]);
        }

        audioSource.GetComponent<IPoolableObject>().Initialize(position, rotation, parent);
    }
}
