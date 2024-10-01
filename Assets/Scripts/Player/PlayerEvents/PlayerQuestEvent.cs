using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerQuestEvent : MonoBehaviour
{
    public event Action onCollect;

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

    public void UpdateItemCount()
    {
        if(onCollect != null)
        {
            onCollect();
        }
    }
}
