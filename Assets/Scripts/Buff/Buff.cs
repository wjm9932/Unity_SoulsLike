using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Buff : MonoBehaviour
{
    public enum BuffType
    {
        ATTACK,
        ARMOR,
        STAMINA,
    }

    public Action onDestroy;

    [SerializeField] private Image icon;
    [SerializeField] protected float duration;
    protected float elapsedTime;
    
    protected Character owner;

    public abstract void Initialize(float value);

    public void Update()
    {
        if (elapsedTime <= 0)
        {
            RemoveBuff();
        }
        else
        {
            elapsedTime -= Time.deltaTime;
        }

        icon.fillAmount = elapsedTime / duration;
    }

    public void RemoveBuff()
    {
        if(onDestroy != null)
        {
            onDestroy();
        }
        Destroy(this.gameObject);
    }

    public void ResetTime()
    {
        elapsedTime = duration;
    }

    public void SetOwner(Character owner)
    {
        this.owner = owner; 
    }

}
