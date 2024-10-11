using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    private Vector3 shotDirection;
    private Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        shotDirection = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {

            rb.AddForce(shotDirection * speed, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.isKinematic = true;
        transform.position = other.ClosestPoint(transform.position);
        transform.SetParent(other.transform);
    }
}
