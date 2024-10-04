using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class QuestLogButton : MonoBehaviour
{
    public Button button { get; private set; }
    private TextMeshProUGUI buttonText;
    private event Action onSelectAction;

    // because we're instantiating the button and it may be disabled when we
    // instantiate it, we need to manually initialize anything here.
    public void Initialize(string displayName, Action selectAction)
    {
        this.button = this.GetComponent<Button>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();

        this.buttonText.text = displayName;
        this.onSelectAction += selectAction;
    }

    public void OnClicked()
    {
        Debug.Log(this.gameObject.name);
        if(onSelectAction != null)
        {
            onSelectAction();
        }
    }

    public void SetState(QuestState state)
    {
        switch (state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                buttonText.color = Color.red;
                break;
            case QuestState.CAN_START:
                buttonText.color = Color.blue;
                break;
            case QuestState.IN_PROGRESS:
                buttonText.color = Color.yellow;
                break;
            case QuestState.CAN_FINISH:
                buttonText.color = Color.green;
                break;
            case QuestState.FINISHED:
                buttonText.color = Color.black;
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement: " + state);
                break;
        }
    }
}
