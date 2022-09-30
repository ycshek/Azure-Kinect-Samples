using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxHandler : MonoBehaviour
{
    public delegate void SoundBehaviour();
    public event SoundBehaviour TouchHandler;
    public event SoundBehaviour DestroyHandler;

    TextMeshPro tm;
    Rigidbody rb;
    BoxCollider bc;
    public float txtPadding = 0.8f;
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

    public void init(string txt, Color color)
    {
        // init text box scale by text length
        tm = GetComponentInChildren<TextMeshPro>();
        tm.text = txt;
        if (txt.Length <= 2)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            tm.transform.localScale = new Vector3(1, 1, 1);
            tm.rectTransform.sizeDelta = new Vector2(1 * txtPadding, 1 * txtPadding);
        }
        else if (txt.Length <= 4)
        {
            this.transform.localScale = new Vector3(2, 1, 1);
            tm.transform.localScale = new Vector3(0.5f, 1, 1);
            tm.rectTransform.sizeDelta = new Vector2(2 * txtPadding, 1 * txtPadding);
        }
        else if (txt.Length <= 6)
        {
            this.transform.localScale = new Vector3(3, 1, 1);
            tm.transform.localScale = new Vector3(0.33f, 1, 1);
            tm.rectTransform.sizeDelta = new Vector2(3 * txtPadding, 1 * txtPadding);
        }
        else if (txt.Length <= 8)
        {
            this.transform.localScale = new Vector3(4, 1, 1);
            tm.transform.localScale = new Vector3(0.25f, 1, 1);
            tm.rectTransform.sizeDelta = new Vector2(4 * txtPadding, 1 * txtPadding);
        }
        else
        {
            this.transform.localScale = new Vector3(5, 1, 1);
            tm.transform.localScale = new Vector3(0.2f, 1, 1);
            tm.rectTransform.sizeDelta = new Vector2(5 * txtPadding, 1 * txtPadding);
        }

        // set text box color
        this.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision " + collision.gameObject.name);
        // clear the textbox when it is already touch the floor
        if (collision.gameObject.name.Contains("Floor"))
            Destroy(this.gameObject);
        else if ((collision.gameObject.name.Contains("Wrist") || collision.gameObject.name.Contains("Thumb") || collision.gameObject.name.Contains("Hand") || collision.gameObject.name.Contains("Finger")) && !bc.isTrigger)
        {
            // turn on trigger make rigibody can pass through the hidden wall 
            bc.isTrigger = true;

            // get random direction for the force
            Vector3 direction = new Vector3(Random.Range(directionMin.x, directionMax.x), Random.Range(directionMin.y, directionMax.y), Random.Range(directionMin.z, directionMax.z));
            // add force to the rigibody
            rb.AddForce(direction * thrust);

            // send touch event to game manager
            if (TouchHandler != null)
                TouchHandler.Invoke();
        }
        else
            return;
    }

    private void OnTriggerEnter(Collider other)
    {        
        // clear the textbox when it is already touch the floor
        if (other.name.Contains("Floor"))
        {
            // send destroy event to game manager
            if (DestroyHandler != null)
                DestroyHandler.Invoke();

            Destroy(this.gameObject);
        }
            
        //else if (!other.name.Contains("TxtBox"))
            //Debug.Log("Trigger " + other.name);
    }
}
