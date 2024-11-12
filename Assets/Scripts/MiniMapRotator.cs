using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRotator : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private Camera minimapCamera;
    private bool previousDungeonState = false;

    void Start()
    {
        previousDungeonState = mainCharacter.GetComponent<Character>().isInDungeon;
        UpdateCullingMask(previousDungeonState);
    }

    void Update()
    {
        bool currentDungeonState = mainCharacter.GetComponent<Character>().isInDungeon;

        if (currentDungeonState != previousDungeonState)
        {
            UpdateCullingMask(currentDungeonState);
            previousDungeonState = currentDungeonState;
        }
    }

    private void LateUpdate()
    {
        Vector3 newPosition = mainCharacter.transform.position;
        newPosition.y = this.transform.position.y;

        this.transform.position = newPosition;

        this.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
    }

    private void UpdateCullingMask(bool isInDungeon)
    {
        if (isInDungeon)
        {
            minimapCamera.cullingMask = (1 << 10) | (1 << 12);
        }
        else
        {
            minimapCamera.cullingMask = 1 << 11 | (1 << 12);
        }
    }
}
