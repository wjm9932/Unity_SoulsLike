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
    private event Action<Quest> onSelectAction;
    private event Action<QuestLogButton> onClick;
    public Quest quest { get; private set; }

    public void Initialize(string displayName, Action<Quest> selectAction, Quest quest, Action<QuestLogButton> onClick)
    {
        this.button = this.GetComponent<Button>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();

        this.quest = quest;
        this.onClick = onClick;
        this.buttonText.text = displayName;
        this.onSelectAction = selectAction;
    }

    public void OnClicked()
    {
        if(onClick != null)
        {
            onClick(this);
        }
        if(onSelectAction != null)
        {
            onSelectAction(quest);
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