using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Player FootStep")]
    [SerializeField] private AudioSource playerFootStepSoundSource;
    [SerializeField] private AudioClip[] footStepClips;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
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
}
