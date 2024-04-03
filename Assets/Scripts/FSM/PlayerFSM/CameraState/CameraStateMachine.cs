using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine : StateMachine
{
    private static CameraStateMachine instance;
    public static CameraStateMachine Instance
    {
        get
        {
            if (instance == null)
            {
            }
            return instance;
        }
    }

    public Character character { get; }

    public CameraLockOnState cameraLockOnState { get; private set; }
    public CameraLockOffState cameraLockOffState { get; private set; }

    public CameraStateMachine(Character character)
    {
        this.character = character;

        cameraLockOnState = new CameraLockOnState(this);
        cameraLockOffState = new CameraLockOffState(this);
    }

    public static void Initialize(Character character)
    {
        if (instance == null)
        {
            instance = new CameraStateMachine(character);
        }
    }
}
