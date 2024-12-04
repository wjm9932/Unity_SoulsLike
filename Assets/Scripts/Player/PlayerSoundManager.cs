using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Player FootStep Sound")]
    [SerializeField] private List<AudioClip> tileFootStepClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> groundFootStepClips = new List<AudioClip>();
    private List<AudioClip> currentFootStepClips = new List<AudioClip>();
    private AudioSource playerFootStepSoundSource;
    public SoundManager.BackGroundMusic musicType { get; private set; }


    private void Awake()
    {
        playerFootStepSoundSource = GetComponent<AudioSource>();
        currentFootStepClips = groundFootStepClips;
    }


    // Start is called before the first frame update
    public void ChangeSoundEffect(SoundManager.BackGroundMusic type)
    {
        musicType = type;

        switch (type)
        {
            case SoundManager.BackGroundMusic.BOSS:
            case SoundManager.BackGroundMusic.DUNGEON:
                currentFootStepClips = tileFootStepClips;
                break;
            case SoundManager.BackGroundMusic.OUTSIDE:
                currentFootStepClips = groundFootStepClips;
                break;
        }
    }

    private void PlayFootStepSound(AnimationEvent ev)
    {
        if (ev.animatorClipInfo.weight >= 0.5f)
        {
            int index = Random.Range(0, currentFootStepClips.Count);
            playerFootStepSoundSource.PlayOneShot(currentFootStepClips[index]);
        }
    }

    public void Intialize(SoundManager.BackGroundMusic type)
    {
        musicType = type;
        ChangeSoundEffect(musicType);
        SoundManager.Instance.ChangeBackGroundMusic(musicType);
    }
}
