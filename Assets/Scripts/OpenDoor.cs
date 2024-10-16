using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private QuestInfoSO targetQuest;

    private void OnEnable()
    {
        QuestManager.Instance.onFinishQuest += OpenTheDoor;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onFinishQuest -= OpenTheDoor;
    }

    private void OpenTheDoor(Quest quest)
    {
        if (targetQuest.id == quest.info.id)
        {
            this.gameObject.transform.rotation = Quaternion.identity;
            Destroy(this);
        }
    }
}
