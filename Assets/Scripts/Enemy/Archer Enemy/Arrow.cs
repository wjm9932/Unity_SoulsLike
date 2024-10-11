using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    public LivingEntity parent;
    private bool isGotShot;
    [SerializeField]
    private float speed = 20f;
    private Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        isGotShot = false;
        rb.AddForce(this.transform.forward * speed, ForceMode.Impulse);
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(isGotShot == false)
        {
            if (other.CompareTag("Enemy") == false)
            {
                Character player = other.GetComponent<Character>();
                if (player != null)
                {
                    if(player.canBeDamaged == true)
                    {
                        isGotShot = true;
                        rb.isKinematic = true;
                        transform.position = other.ClosestPoint(transform.position);
                        transform.SetParent(player.arrowParent);
                        GetComponent<Collider>().enabled = false;
                    }
                }
                else
                {
                    isGotShot = true;
                    rb.isKinematic = true;
                    transform.position = other.ClosestPoint(transform.position);
                    transform.SetParent(other.transform);
                    GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
