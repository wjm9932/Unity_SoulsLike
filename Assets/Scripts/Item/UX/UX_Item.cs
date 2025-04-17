using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UX_Item : MonoBehaviour
{
    public UX_ItemDataSO data { get { return _data; } }
    [SerializeField] private UX_ItemDataSO _data;

    [SerializeField] public int numOfItem = 1;
    [HideInInspector] public int triggerCount;

    void Awake()
    {
        triggerCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            if (triggerCount <= 0)
            {
                if (character.inventory.AddItem(this, numOfItem) == true)
                {
                    TextManager.Instance.PlayNotificationText("You've got " + data.itemName + " x" + numOfItem);
                    SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.PICKUP, 0.2f);
                    Destroy(this.gameObject);
                }
                else
                {
                    TextManager.Instance.PlayNotificationText("Inventory is full");
                }
            }
            else
            {
                --triggerCount;
            }
        }
    }
}


