using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FootStepSoundType
{
    GROUND,
    TILE,
}

[RequireComponent(typeof(BoxCollider))]

public class FootStepTrigger : MonoBehaviour
{
    [SerializeField] private FootStepSoundType type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            other.GetComponent<Character>().ChangeFootStepSound(type);
        }
    }
}
