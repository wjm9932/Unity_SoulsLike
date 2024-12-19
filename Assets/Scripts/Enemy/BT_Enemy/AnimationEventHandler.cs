using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class AnimationEventHandler : MonoBehaviour
{
    public Action onAnimationEnter;
    public Action onAnimationTransition;
    public Action onAnimationExit;
    
    public Action animationIK;

    // Start is called before the first frame update
    public void OnAnimationEnterEvent()
    {
        onAnimationEnter?.Invoke();
    }
    public void OnAnimationTransitionEvent()
    {
        onAnimationTransition?.Invoke();
    }
    public void OnAnimationExitEvent()
    {
        onAnimationExit?.Invoke();
    }

    private void OnAnimatorIK()
    {
        animationIK?.Invoke();
    }
}
