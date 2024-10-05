using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDialogue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questTitleText;
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private void OnEnable()
    {
        QuestManager.Instance.onUpdateQuestDialogue += SetDialogueText;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onUpdateQuestDialogue -= SetDialogueText;
    }

    private void SetDialogueText(string questName, string text)
    {
        questTitleText.text = questName;
        dialogueText.text = text;
    }
}
