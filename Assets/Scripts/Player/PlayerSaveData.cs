using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public float currentHealth;
    public float maxHealth;
    public float currentStamina;
    public bool isInDungeon;
    public Vector3 checkPoint;

    public PlayerSaveData(Vector3 playerPosition, Quaternion playerRotation, float currentHealth, float maxHealth, float currentStamina, bool isInDungeon, Vector3 checkPoint)
    {
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.currentStamina = currentStamina;
        this.isInDungeon = isInDungeon;
        this.checkPoint = checkPoint;
    }
}
