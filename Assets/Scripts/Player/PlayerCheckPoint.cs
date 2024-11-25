using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    public Vector3 checkPointPosition { get; private set;}

    public void SetCheckPoint(Vector3 checkPointPosition)
    {
        this.checkPointPosition = checkPointPosition;
    }
}
