using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static EffectManager;
using static UnityEditor.PlayerSettings;

public class TextManager : MonoBehaviour
{
    private static TextManager _instance;
    public static TextManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<TextManager>();
            return _instance;
        }
    }

    private Queue<GameObject> notificationTextStack = new Queue<GameObject>();
    private const int notificationTextMaxCount = 4;

    [SerializeField]
    private TextMeshPro damageText;

    [SerializeField]
    private TextMeshProUGUI hpIsFullText;
    [SerializeField]
    private TextMeshProUGUI inventoryIsFullText;

    [SerializeField]
    private GameObject notificationPanel;

    public enum DisplayText
    {
        HP_IS_FULL,
        INVENTORY_IS_FUll,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDamageText(Vector3 pos, Transform parent, float damage)
    {
        var text = Instantiate(damageText, pos, Quaternion.identity, parent);
        text.text = damage.ToString();
    }
    public void PlayNotificationText(DisplayText textType)
    {
        var displayText = GetDisplayText(textType);

        if(displayText == null)
        {
            Debug.LogError("Notification text is null");
            return;
        }

        if (notificationTextStack.Count >= notificationTextMaxCount)
        {
            Destroy(notificationTextStack.Peek().gameObject);
            notificationTextStack.Dequeue();
        }

        var text = Instantiate(displayText, notificationPanel.transform).gameObject;
        text.GetComponent<DestroyTextInTime>().OnDestroy += RemoveFromStack;

        notificationTextStack.Enqueue(text);
    }

    private void RemoveFromStack()
    {
        notificationTextStack.Dequeue();
    }

    TextMeshProUGUI GetDisplayText(DisplayText textType)
    {

        switch (textType)
        {
            case DisplayText.HP_IS_FULL:
                return hpIsFullText;
            case DisplayText.INVENTORY_IS_FUll:
                return inventoryIsFullText;
            default:
                return null;
        }
    }
}
