using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogWindow : MonoBehaviour
{

    [SerializeField]
    private QuestScrollList questScrollList;

    private void OnEnable()
    {
        QuestManager.Instance.onChangeQuestState += QuestStateChange;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onChangeQuestState -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        // add the button to the scrolling list if not already added
        QuestLogButton questLogButton = questScrollList.CreateButtonIfNotExists(quest, () => {
            Debug.Log(quest.info.id);
        });

        questLogButton.SetState(quest.state);
    }
}
