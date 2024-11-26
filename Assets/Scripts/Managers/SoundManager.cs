using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource bgmAudioSource;

    public enum BackGroundMusic
    {
        OUTSIDE,
        DUNGEON,
        BOSS
    }

    public enum SoundEffectType
    {
        PICKUP,
        DRINK,
        INCREASE_MAX_HP,
        BUFF,
        ALERT,
        DODGE_LANDING,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        PLAYER_HIT,
        WARRIOR_ENEMY_ATTACK,
        ARCHER_ENEMY_ATTACK,
        SHOT_ARROW,
        ENEMY_HIT,
        ENEMY_DIE, 
        DOOR_OPEN,
        PLAYER_DIE,
        SLASH,
        JUMP_ATTACK,
        BOSS_JUMP,
        BOSS_FLIP,
        BOSS_DASH,
        BOSS_CHARGE_ATTACK,
        BOSS_SWORD_ATTACK,
        BOSS_STAB_ATTACK,
    }

    public enum UISoundEffectType
    {
        CLICK,
        DROP,
        QUEST_DIALOGUE,
        QUEST_COMPLETED,
    }

    [System.Serializable]
    private struct BGMInfo
    {
        public BackGroundMusic type;
        public AudioClip audioClips;
    }

    [System.Serializable]
    private struct SoundEffectInfo
    {
        public SoundEffectType effectType;
        public AudioClip[] audioClips;
    }

    [System.Serializable]
    private struct UISoundEffectInfo
    {
        public UISoundEffectType effectType;
        public AudioClip[] audioClips;
    }

    [SerializeField] private BGMInfo[] bgmList;
    [SerializeField] private SoundEffectInfo[] effectInfos;
    [SerializeField] private UISoundEffectInfo[] uiEffectInfos;

    private Dictionary<BackGroundMusic, AudioClip> bgmAudioClips = new Dictionary<BackGroundMusic, AudioClip>();
    private Dictionary<SoundEffectType, List<AudioClip>> inGameAudioClips = new Dictionary<SoundEffectType, List<AudioClip>>();
    private Dictionary<UISoundEffectType, List<AudioClip>> uiGameAudioClips = new Dictionary<UISoundEffectType, List<AudioClip>>();

    private int maxPickUpSoundEffect = 3;
    private int currentPlayingPickupEffect = 0;
    public AudioSource drinkAudioSource { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        for(int i = 0; i < bgmList.Length; i++)
        {
            bgmAudioClips.Add(bgmList[i].type, bgmList[i].audioClips);
        }

        for(int i = 0; i < effectInfos.Length; i++)
        {
            List<AudioClip> clips = new List<AudioClip>(effectInfos[i].audioClips.Length);

            for(int j = 0; j < effectInfos[i].audioClips.Length; j++)
            {
                clips.Add(effectInfos[i].audioClips[j]);
            }
            
            inGameAudioClips.Add(effectInfos[i].effectType, clips);
        }

        for (int i = 0; i < uiEffectInfos.Length; i++)
        {
            List<AudioClip> clips = new List<AudioClip>(uiEffectInfos[i].audioClips.Length);

            for (int j = 0; j < uiEffectInfos[i].audioClips.Length; j++)
            {
                clips.Add(uiEffectInfos[i].audioClips[j]);
            }

            uiGameAudioClips.Add(uiEffectInfos[i].effectType, clips);
        }
    }

    public void Update()
    {
    }
    public void Play2DSoundEffect(UISoundEffectType type, float volume = 0.2f)
    {
        var audioSource = ObjectPoolManager.Instance.GetPoolableObject(ObjectType.SOUND);
        var soundObjectPool = audioSource.GetComponent<SoundObjectPool>();
        var audioComponent = audioSource.GetComponent<AudioSource>();

        audioComponent.volume = volume;
        audioComponent.spatialBlend = 0f;

        audioComponent.PlayOneShot(uiGameAudioClips[type][0]);
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
            soundObjectPool.removeAction += () => { --currentPlayingPickupEffect; };
        }
        if(type == SoundEffectType.DRINK)
        {
            drinkAudioSource = audioComponent;
            soundObjectPool.removeAction += () => { drinkAudioSource = null; };
        }
        
        audioComponent.volume = volume;
        audioComponent.spatialBlend = 0f;

        if (inGameAudioClips[type].Count == 1)
        {
            audioComponent.PlayOneShot(inGameAudioClips[type][0]);
        }
        else
        {
            int index = Random.Range(0, inGameAudioClips[type].Count);
            audioComponent.PlayOneShot(inGameAudioClips[type][index]);
        }
    }

    public GameObject Play3DSoundEffect(SoundEffectType type, float volume, Vector3 position, Quaternion rotation, Transform parent)
    {
        var audioSource = ObjectPoolManager.Instance.GetPoolableObject(ObjectType.SOUND);
        audioSource.GetComponent<AudioSource>().volume = volume;
        audioSource.GetComponent<AudioSource>().spatialBlend = 1f;

        if (inGameAudioClips[type].Count == 1)
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(inGameAudioClips[type][0]);
        }
        else
        {
            int index = Random.Range(0, inGameAudioClips[type].Count);
            audioSource.GetComponent<AudioSource>().PlayOneShot(inGameAudioClips[type][index]);
        }

        audioSource.GetComponent<IPoolableObject>().Initialize(position, rotation, parent);
        return audioSource;
    }

    public void ChangeBackGroundMusic(BackGroundMusic type)
    {
        if(type  == BackGroundMusic.OUTSIDE)
        {
            bgmAudioSource.clip = bgmAudioClips[type];
            bgmAudioSource.pitch = 2.5f;
            bgmAudioSource.volume = 0.5f;
            
        }
        else if (type == BackGroundMusic.DUNGEON)
        {
            bgmAudioSource.clip = bgmAudioClips[type];
            bgmAudioSource.pitch = 1f;
            bgmAudioSource.volume = 0.4f;
        }
        else
        {
            bgmAudioSource.clip = bgmAudioClips[type];
            bgmAudioSource.pitch = 1f;
            bgmAudioSource.volume = 0.35f;
        }
        bgmAudioSource.Play();
    }
}
