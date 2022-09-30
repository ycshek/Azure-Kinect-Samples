using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsChecking : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider bc;
    public float thrust = 500f;
    public Vector3 directionMin = new Vector3(-2, 0, 1);
    public Vector3 directionMax = new Vector3(2, 2, 1);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Trigger " + other.name);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision " + collision.gameObject.name);

        if (!collision.gameObject.name.Contains("Hand"))
            return;

        // turn on trigger make rigibody can pass through the hidden wall 
        bc.isTrigger = true;
        
        // get random direction for the force
        Vector3 direction = new Vector3(Random.Range(directionMin.x, directionMax.x), Random.Range(directionMin.y, directionMax.y), Random.Range(directionMin.z, directionMax.z));
        // add force to the rigibody
        rb.AddForce( direction * thrust);
    }
}
