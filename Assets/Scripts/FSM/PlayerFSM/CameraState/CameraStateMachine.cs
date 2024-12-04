using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine : StateMachine
{
    public Character owner { get; }

    public CameraLockOnState cameraLockOnState { get; private set; }
    public CameraLockOffState cameraLockOffState { get; private set; }

    public CameraStateMachine(Character character)
    {
        this.owner = character;

        cameraLockOnState = new CameraLockOnState(this);
        cameraLockOffState = new CameraLockOffState(this);
    }
}
