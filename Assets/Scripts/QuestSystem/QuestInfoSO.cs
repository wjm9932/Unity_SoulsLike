using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObject/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [SerializeField]
    public string id;
    public string displpayName;
    public QuestState initialState;

    [Header ("Requirements")]
    public QuestInfoSO[] questPrerequisites;

    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Target Items")]
    public GameObject targetItem;
    public int targetItemCount;

    [Header("Reward Items")]
    public GameObject rewardItem;
    public int rewardItemCount;

    // ensure the id is always the name of script
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
