using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private Queue<GameObject> notificationTextQueue = new Queue<GameObject>();
    private const int notificationTextMaxCount = 4;

    [SerializeField] private GameObject notificationPanel;


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
        var text = ObjectPoolManager.Instance.GetPoolableObject(ObjectPoolManager.ObjectType.DAMAGE_TEXT);
        text.GetComponent<IPoolableObject>().Initialize(pos, Quaternion.identity, parent);
        text.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public void PlayNotificationText(string text, Color color = default)
    {
        if (color == default)
        {
            color = Color.black;
        }

        CheckNotificationTextQueueCount();

        var textObject = ObjectPoolManager.Instance.GetPoolableObject(ObjectPoolManager.ObjectType.NOTIFICATION_TEXT);
        textObject.GetComponent<IPoolableObject>().Initialize(Vector3.zero, Quaternion.identity, notificationPanel.transform);
        textObject.GetComponent<TextMeshProUGUI>().text = text;
        textObject.GetComponent<TextMeshProUGUI>().color = color;
        textObject.gameObject.GetComponent<DestroyTextInTime>().OnDestroy += () => notificationTextQueue.Dequeue();

        notificationTextQueue.Enqueue(textObject.gameObject);
    }


    private void CheckNotificationTextQueueCount()
    {
        if (notificationTextQueue.Count >= notificationTextMaxCount)
        {
            var textToRemove = notificationTextQueue.Dequeue();
            textToRemove.GetComponent<IPoolableObject>().Release();
        }
    }
}
