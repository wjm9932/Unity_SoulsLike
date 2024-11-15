using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEvent : MonoBehaviour
{
    public event Action<string> onCollect;
    public event Action<string> onUse;
    public event Action<string> onVisit;
    public event Action<EntityType> OnKill;
    public event Action<string> onAttack;
    public event Action<float> onSprint;
    public event Action onDodge;
    public event Action onCameraLockOn;
    public event Action onChangeTarget;

    public event Action onUnlock;

    public void UpdateItemCount(string item)
    {
        if (onCollect != null)
        {
            onCollect(item);
        }
    }

    public void UseItem(string item)
    {
        if(onUse != null)
        {
            onUse(item);
        }
    }
    public void KillEnemy(EntityType type)
    {
        if(OnKill != null)
        {
            OnKill(type);
        }
    }

    public void Visit(string place)
    {
        if(onVisit != null)
        {
            onVisit(place);
        }
    }

    public void Attack(string combo)
    {
        if(onAttack != null)
        {
            onAttack(combo);
        }
    }
    public void Sprint(float sprintTime)
    {
        if(onSprint != null)
        {
            onSprint(sprintTime);
        }
    }

    public void Dodge()
    {
        if(onDodge != null)
        {
            onDodge();
        }
    }

    public void CameraLockOn()
    {
        if(onCameraLockOn != null)
        {
            onCameraLockOn();
        }
    }

    public void ChangeTarget()
    {
        if(onChangeTarget != null)
        {
            onChangeTarget();
        }
    }

    public void Unlock()
    {
        if(onUnlock != null)
        {
            onUnlock();
        }
    }
}
