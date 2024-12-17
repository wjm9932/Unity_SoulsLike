using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class AnimationEventHandler : MonoBehaviour
{
    public Action onAnimationTransition;
    public Action onAnimationComplete;
    // Start is called before the first frame update
    private void OnAnimationEnterEvent()
    {
    }
    private void OnAnimationTransitionEvent()
    {
        onAnimationTransition?.Invoke();
    }
    private void OnAnimationExitEvent()
    {
        onAnimationComplete?.Invoke();
    }
}
