using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public State state { get; set; }
    
    public enum State
    {
        Idle,
        Dodging,
        Attacking
    }
    
    public void ResetPlayerState()
    {
        state = State.Idle;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
