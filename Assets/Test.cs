using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //if (flatVel.magnitude > 3)
        //{
        //    Vector3 limitedVel = flatVel.normalized * 3;
        //    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        //}
        //rb.AddForce(transform.forward * 5f, ForceMode.Force);
        transform.Translate(new Vector3(0.005f, 0f, 0f));
    }
    private void FixedUpdate()
    {
        //transform.Translate(new Vector3(0.03f, 0f, 0f));
    }
}
