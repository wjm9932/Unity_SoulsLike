using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestScrollList : MonoBehaviour
{

    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private VerticalLayoutGroup layOut;

    private Dictionary<string, QuestLogButton> idToButtonMap = new Dictionary<string, QuestLogButton>();

    public QuestLogButton CreateButtonIfNotExists(Quest quest, Action selectAction)
    {
        QuestLogButton questLogButton = null;
        // only create the button if we haven't seen this quest id before
        if (idToButtonMap.ContainsKey(quest.info.id) == false)
        {
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        }
        else
        {
            questLogButton = idToButtonMap[quest.info.id];
        }
        return questLogButton;
    }

    private QuestLogButton InstantiateQuestLogButton(Quest quest, Action selectAction)
    {
        QuestLogButton questLogButton = Instantiate(buttonPrefab, contentParent.transform).GetComponent<QuestLogButton>();
        questLogButton.Initialize(quest.info.displpayName, () => { selectAction(); });

        RectTransform contentRectTransform = contentParent.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentParent.GetComponent<RectTransform>().sizeDelta.x, contentRectTransform.sizeDelta.y + 30f + layOut.spacing);

        idToButtonMap.Add(quest.info.id, questLogButton);
        return questLogButton;
    }
}
