using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementsNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;


    [Header("MiniMap Icons")]
    [SerializeField] private GameObject requirementsNotMetToStartMinimapIcon;
    [SerializeField] private GameObject canStartMinimapIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishMinimapIcon;
    [SerializeField] private GameObject canFinishMinimapIcon;

    public void SetState(QuestState newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        requirementsNotMetToStartIcon.SetActive(false);
        requirementsNotMetToStartMinimapIcon.SetActive(false);

        canStartIcon.SetActive(false);
        canStartMinimapIcon.SetActive(false);

        requirementsNotMetToFinishIcon.SetActive(false);
        requirementsNotMetToFinishMinimapIcon.SetActive(false);

        canFinishIcon.SetActive(false);
        canFinishMinimapIcon.SetActive(false);

        // set the appropriate one to active based on the new state
        switch (newState)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                if (startPoint) { 
                    requirementsNotMetToStartIcon.SetActive(true);
                    requirementsNotMetToStartMinimapIcon.SetActive(true);
                }
                break;
            case QuestState.CAN_START:
                if (startPoint) { 
                    canStartIcon.SetActive(true);
                    canStartMinimapIcon.SetActive (true);
                }
                break;
            case QuestState.IN_PROGRESS:
                if (finishPoint) { 
                    requirementsNotMetToFinishIcon.SetActive(true);
                    requirementsNotMetToFinishMinimapIcon.SetActive(true);
                }
                break;
            case QuestState.CAN_FINISH:
                if (finishPoint) { 
                    canFinishIcon.SetActive(true);
                    canFinishMinimapIcon.SetActive (true); 
                }
                break;
            case QuestState.FINISHED:
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}
