using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform pos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            other.GetComponent<PlayerCheckPoint>().SetCheckPoint(pos.position);
        }
    }
    private void OnValidate()
    {
        var boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null && !boxCollider.isTrigger)
        {
            boxCollider.isTrigger = true;
        }
    }
}
