using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Locker : MonoBehaviour
{
    [SerializeField] private GameObject minimapIcon;
    [SerializeField] private GameObject key;

    private Action unlockAction;

    private void Unlock(Character player)
    {
        if (player.inventory.RemoveTargetItemFromInventory(key.GetComponent<UX.UX_Item>(), 1) == true)
        {
            player.playerEvents.onUnlock -= unlockAction;
            player.GetComponent<InteractionIndicator>().Hide();

            StartCoroutine(RotateOverTime(Quaternion.Euler(0, 90, 0), 1.5f));
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.DOOR_OPEN, 0.5f);
        }
        else
        {
            TextManager.Instance.PlayNotificationText("Key is not found");
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ALERT, 0.2f);
        }
    }

    private IEnumerator RotateOverTime(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float percentage = elapsedTime / duration;
            percentage = Mathf.Pow(percentage, 1f / 2.5f);

            transform.rotation = Quaternion.Slerp(initialRotation, initialRotation * targetRotation, percentage);
            yield return null;
        }

        transform.rotation = initialRotation * targetRotation;
        minimapIcon.SetActive(false);
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>() != null)
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
