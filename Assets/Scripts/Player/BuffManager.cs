using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private Transform buffIndicatorPanel;
    

    [System.Serializable]
    private struct BuffInfo
    {
        public Buff.BuffType buffType;
        public GameObject buff;
    }
    [SerializeField] private BuffInfo[] buffs;

    private Dictionary<Buff.BuffType, GameObject> buffPrefabs = new Dictionary<Buff.BuffType, GameObject>();
    private Dictionary<Buff.BuffType, Buff> buffContainer = new Dictionary<Buff.BuffType, Buff>();

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

    public void AddBuff(Buff.BuffType type, float value)
    {
        if(buffContainer.ContainsKey(type) == true)
        {
            buffContainer[type].Initialize(value);
        }
        else
        {
            Buff buff = Instantiate(buffPrefabs[type], buffIndicatorPanel).GetComponent<Buff>();
            buff.onDestroy += () => { RemoveFromBuffContainer(type); };
            buff.SetOwner(this.gameObject.GetComponent<Character>());
            buff.Initialize(value);

            buffContainer.Add(type, buff);  
        }
    }

    private void RemoveFromBuffContainer(Buff.BuffType type)
    {
        if (buffContainer.ContainsKey(type) == true)
        {
            buffContainer.Remove(type);
        }
    }
}
