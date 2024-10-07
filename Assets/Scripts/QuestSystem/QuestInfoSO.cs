using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObject/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    public string id;
    public string displayName;
    public QuestState initialState;


    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string requirementsNotMetDialogue;
    [TextArea(3, 10)]
    public string doneDialogue;
    [TextArea(3, 10)]
    public string onStartDialogue;
    [TextArea(3, 10)]
    public string onFinishDialogue;

    [Header ("Requirements")]
    public QuestInfoSO[] questPrerequisites;

    [Header("Steps")]
    public QuestStepPrefabs[] questStepPrefabs;

    [Header("Target Items")] 
    public GameObjectInfo[] targetItem;

    [Header("Reward Items")]
    public GameObjectInfo[] rewards;

    [System.Serializable]
    public class QuestStepPrefabs
    {
        public GameObject[] stepPrefabs;  // Array of prefabs for each step
    }

    [System.Serializable]
    public class GameObjectInfo
    {
        public GameObject gameObject;
        public string itemName;      
        public int count;            
    }

    // ensure the id is always the name of script
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
