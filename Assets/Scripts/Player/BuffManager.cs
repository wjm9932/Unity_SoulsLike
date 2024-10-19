using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private Transform buffIndicatorPanel;
    public enum BuffType
    {
        ATTACK,
        ARMOR,
    }

    [System.Serializable]
    private struct BuffInfo
    {
        public BuffType buffType;
        public GameObject buff;
    }
    [SerializeField] private BuffInfo[] buffs;

    private Dictionary<BuffType, GameObject> buffPrefabs = new Dictionary<BuffType, GameObject>();
    private Dictionary<BuffType, Buff> buffContainer = new Dictionary<BuffType, Buff>();

    private void Awake()
    {
        for(int i = 0; i < buffs.Length; i++)
        {
            buffPrefabs.Add(buffs[i].buffType, buffs[i].buff);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBuff(BuffType type)
    {
        if(buffContainer.ContainsKey(type) == true)
        {
            buffContainer[type].ResetTime();
        }
        else
        {
            Buff buff = Instantiate(buffPrefabs[type], buffIndicatorPanel).GetComponent<Buff>();
            buff.onDestroy += () => { RemoveFromBuffContainer(type); };
            buff.SetOwner(this.gameObject.GetComponent<LivingEntity>());
            buffContainer.Add(type, buff);  
        }
    }

    private void RemoveFromBuffContainer(BuffType type)
    {
        if (buffContainer.ContainsKey(type) == true)
        {
            buffContainer.Remove(type);
        }
    }
}
