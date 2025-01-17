using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossRoom : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject targetDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            //boss.GetComponent<Enemy>().target = other.gameObject;
            boss.GetComponent<BehaviorTreeBuilder>().blackboard.SetData<GameObject>("target", other.gameObject);
            boss.GetComponent<Enemy>().hpBar.gameObject.SetActive(true);
            targetDoor.GetComponent<CloseDoor>().Close();
            Destroy(this.gameObject);
        }
    }
}
