using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerQuestEvent : MonoBehaviour
{
    public event Action<string> onCollect;
    public event Action<string> onUse;
    public event Action<string> onVisit;
    public event Action<EntityType> OnKill;


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
}
