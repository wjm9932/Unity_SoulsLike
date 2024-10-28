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
    public string requirementsNotMetDialogue = "You have to complete other quest before start this quest!";
    [TextArea(3, 10)]
    public string doneDialogue = "Thank you for your help!";
    [TextArea(3, 10)]
    public string onStartDialogue;
    [TextArea(3, 10)]
    public string onFinishDialogue;
    [TextArea(3, 10)]
    public string onFailFinishDialogue = "Not enough space!";

    [Header ("Requirements")]
    public QuestInfoSO[] questPrerequisites;

    [Header("Steps")]
    public QuestStepPrefabs[] questStepPrefabs;

    [Header("Target Items")] 
    public ItemInfo[] targetItem;

    [Header("Reward Items")]
    public ItemInfo[] rewards;

    [System.Serializable]
    public class QuestStepPrefabs
    {
        public GameObject[] stepPrefabs;  // Array of prefabs for each step
    }

    [System.Serializable]
    public class ItemInfo
    {
        [SerializeField]
        private GameObject _itemPrefab;
        public UX.UX_Item itemPrefab
        {
            get
            {
                if (_itemPrefab.GetComponent<UX.UX_Item>() == null)
                {
                    Debug.LogError(_itemPrefab.name + "does not have UX.UX_Item component!");
                    return null;
                }
                else
                {
                    return _itemPrefab.GetComponent<UX.UX_Item>();
                }

            }
        }
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
