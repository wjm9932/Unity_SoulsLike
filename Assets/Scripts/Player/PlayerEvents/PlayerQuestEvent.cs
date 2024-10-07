using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerQuestEvent : MonoBehaviour
{
    public event Action<string> onCollect;
    public event Action<string> onUse;
    public event Action OnKill;

    // Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
    public void KillEnemy()
    {
        if(OnKill != null)
        {
            OnKill();
        }
    }
}
