using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitCampFireQuestStep : QuestStep
{
    [SerializeField] GameObject targetPlace;

    private void Awake()
    {
        status = "Objective: Visit the camp fire";
    }

    // Start is called before the first frame update
    void Start()
    {
        status = "Visit the camp fire";
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerQuestEvent>().onVisit += VisitCampFire;
        }
    }
    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerQuestEvent>().onVisit -= VisitCampFire;
        }
    }
    private void VisitCampFire(string place)
    {
        if(targetPlace.CompareTag(place) == true)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }
}
