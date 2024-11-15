using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Locker : MonoBehaviour
{
    [SerializeField] private GameObject minimapIcon;
    [SerializeField] private GameObject key;

    private Action unlockAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Unlock(Character player)
    {
        if(player.inventory.GetTargetItemCountFromInventory(key.GetComponent<UX.UX_Item>()) >= 1)
        {
            player.playerEvents.onUnlock -= unlockAction;
            player.GetComponent<InteractionIndicator>().Hide();
            this.transform.rotation *= Quaternion.Euler(0, 90, 0);
            minimapIcon.SetActive(false);
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            unlockAction = () => { Unlock(other.GetComponent<Character>()); };
            other.GetComponent<Character>().playerEvents.onUnlock += unlockAction;
            other.GetComponent<Character>().GetComponent<InteractionIndicator>().Show();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            other.GetComponent<Character>().GetComponent<InteractionIndicator>().Hide();
            other.GetComponent<Character>().playerEvents.onUnlock -= unlockAction;
        }
    }
}
