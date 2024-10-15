using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float time;
    private Camera mainCamera;

    private void Awake()
    {
      
        time = 2f;
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - Camera.main.transform.position;
        direction.y = 0; 

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
