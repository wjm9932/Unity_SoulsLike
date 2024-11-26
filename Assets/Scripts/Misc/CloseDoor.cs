using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public void Close()
    {
        StartCoroutine(RotateOverTime(Quaternion.Euler(0, -90, 0), 1.5f));
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
    }
}
