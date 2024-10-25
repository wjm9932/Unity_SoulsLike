using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackQuestStep : QuestStep
{
    [SerializeField] private string attackNumber;
    private void Awake()
    {
        status = "Objective: " + attackNumber +" Attack!";
    }
    // Start is called before the first frame update
    void Start()
    {
        status = attackNumber + " Combo Attack";
        questOwner.playerEvents.onAttack += Attack;
    }

    private void Attack(string combo)
    {
        if(combo == attackNumber)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
            questOwner.playerEvents.onAttack -= Attack;
        }
    }
    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.playerEvents.onAttack -= Attack;
        }
    }
}
