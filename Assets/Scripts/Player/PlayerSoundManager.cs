using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AreaType
{
    OUTSIDE,
    DUNGEON,
    BOSS
}

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Player FootStep Sound")]
    [SerializeField] private List<AudioClip> tileFootStepClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> groundFootStepClips = new List<AudioClip>();
    private List<AudioClip> currentFootStepClips = new List<AudioClip>();
    private AudioSource playerFootStepSoundSource;
    public AreaType areaType { get; private set; }


    private void Awake()
    {
        playerFootStepSoundSource = GetComponent<AudioSource>();
        currentFootStepClips = groundFootStepClips;
    }

    public void ChangeFootStepSound(AreaType type)
    {
        areaType = type;

        switch (type)
        {
            case AreaType.BOSS:
            case AreaType.DUNGEON:
                currentFootStepClips = tileFootStepClips;
                break;
            case AreaType.OUTSIDE:
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

    public void Intialize(AreaType type)
    {
        areaType = type;
        ChangeFootStepSound(areaType);
        SoundManager.Instance.ChangeBackGroundMusic(areaType);
    }
}
