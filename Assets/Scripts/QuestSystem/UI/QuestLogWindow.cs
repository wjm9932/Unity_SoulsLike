using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLogWindow : MonoBehaviour
{

    [SerializeField] 
    private GameObject contentParent;
    [SerializeField]
    private QuestScrollList questScrollList;
    [SerializeField]
    private TextMeshProUGUI displayNameText;
    [SerializeField]
    private TextMeshProUGUI progressText;

    private QuestLogButton lastPressedButton;

    private void OnEnable()
    {
        QuestManager.Instance.onChangeQuestState += QuestStateChange;
        QuestManager.Instance.onUpdateQuestProgress += UpdateQuestInfo;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onChangeQuestState -= QuestStateChange;
        QuestManager.Instance.onUpdateQuestProgress -= UpdateQuestInfo;
    }

    private void QuestStateChange(Quest quest)
    {
        QuestLogButton questLogButton = questScrollList.CreateButtonIfNotExists(quest, UpdateQuestInfo, LastClickedButton);
        questLogButton.SetState(quest.state);

        displayNameText.text = quest.info.displayName;
        progressText.text = quest.GetFullStatusText();
        lastPressedButton = questLogButton;
    }

    private void LastClickedButton(QuestLogButton button)
    {
        lastPressedButton = button;
    }

    private void UpdateQuestInfo(Quest quest)
    {
        if (lastPressedButton.quest.info.id == quest.info.id)
        {
            displayNameText.text = quest.info.displayName;
            progressText.text = quest.GetFullStatusText();
        }
    }
}
