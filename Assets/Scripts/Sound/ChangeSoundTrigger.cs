using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChangeSoundTrigger : MonoBehaviour
{
    [SerializeField] private SoundManager.BackGroundMusic type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            other.GetComponent<Character>().ChangeSoundEffect(type);
            SoundManager.Instance.ChangeBackGroundMusic(type);
        }
    }
}
