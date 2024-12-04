using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChangeSoundTrigger : MonoBehaviour
{
    [SerializeField] private AreaType type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            other.GetComponent<PlayerSoundManager>().ChangeFootStepSound(type);
            SoundManager.Instance.ChangeBackGroundMusic(type);
        }
    }
}
