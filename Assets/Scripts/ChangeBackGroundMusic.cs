using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackGroundMusic : MonoBehaviour
{
    [SerializeField] private Character character;
    private AudioSource audioSource;
    private bool previousIsInDungeonState;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        previousIsInDungeonState = character.isInDungeon;
        UpdateAudioSettings();
    }

    void Update()
    {
        if (character.isInDungeon != previousIsInDungeonState)
        {
            previousIsInDungeonState = character.isInDungeon;
            UpdateAudioSettings();
        }
    }

    private void UpdateAudioSettings()
    {
        if (character.isInDungeon == true)
        {
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
        }
        else
        {
            audioSource.pitch = 2.5f;
            audioSource.volume = 0.5f;
        }
    }
}
