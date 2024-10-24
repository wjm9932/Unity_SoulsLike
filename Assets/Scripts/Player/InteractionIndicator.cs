using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private Canvas canvas;


    private void Update()
    {
        if(interactionIndicator.activeSelf == true)
        {
            if (this.GetComponent<Character>().playerMovementStateMachine.currentState is InteractState == true ||
                this.GetComponent<Character>().uiStateMachine.currentState is OpenState == true)
            {
                interactionIndicator.SetActive(false);
            }
        }
    }

    public void Show()
    {
        if(this.GetComponent<Character>().uiStateMachine.currentState is OpenState == false)
        {
            interactionIndicator.SetActive(true);
        }
    }
    public void Hide()
    {
        interactionIndicator.SetActive(false);
    }
}
