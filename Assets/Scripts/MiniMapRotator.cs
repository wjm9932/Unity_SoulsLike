using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRotator : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;

    private void LateUpdate()
    {
        Vector3 newPosition = mainCharacter.transform.position;
        newPosition.y = this.transform.position.y;

        this.transform.position = newPosition;

        this.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
    }
}
