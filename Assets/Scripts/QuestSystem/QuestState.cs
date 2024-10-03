using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    REQUIREMENTS_NOT_MET,
    CAN_START,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED
}

public enum QuestStepState
{
    IN_PROGRESS,
    FINISHED
}