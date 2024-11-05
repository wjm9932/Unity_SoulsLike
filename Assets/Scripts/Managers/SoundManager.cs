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
        PICKUP,
        DRINK,
        ALERT,
        DODGE_LANDING,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        PLAYER_HIT,
        WARRIOR_ENEMY_ATTACK,
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
    private int maxPickUpSoundEffect = 3;
    private int currentPlayingPickupEffect = 0;

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

    public void Update()
    {
    }

    public void Play2DSoundEffect(SoundEffectType type, float volume = 0.2f)
    {
        if (type == SoundEffectType.PICKUP && currentPlayingPickupEffect >= maxPickUpSoundEffect)
        {
            return;
        }

        var audioSource = ObjectPoolManager.Instance.GetPoolableObject(ObjectType.SOUND);
        var soundObjectPool = audioSource.GetComponent<SoundObjectPool>();
        var audioComponent = audioSource.GetComponent<AudioSource>();

        if (type == SoundEffectType.PICKUP)
        {
            currentPlayingPickupEffect++;
            soundObjectPool.removeFromIndex += () => { --currentPlayingPickupEffect; };
        }
        
        audioComponent.volume = volume;
        audioComponent.spatialBlend = 0f;

        if (audioClipsContainer[type].Count == 1)
        {
            audioComponent.PlayOneShot(audioClipsContainer[type][0]);
        }
        else
        {
            int index = Random.Range(0, audioClipsContainer[type].Count);
            audioComponent.PlayOneShot(audioClipsContainer[type][index]);
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
