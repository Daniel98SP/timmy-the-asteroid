using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTile : MonoBehaviour
{
    public Material orangeMat;
    public Material blueMat;

    private GameObject connectedPortal;
    private float lastUse = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (Time.time - lastUse > 0.5f)
            {
                other.gameObject.GetComponent<controller>().PlayPortalSound();
                other.gameObject.transform.position = connectedPortal.transform.position + Vector3.up;
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, rb.velocity.z);
                lastUse = Time.time;
                connectedPortal.GetComponent<PortalTile>().UpdateLastUse();
            }            
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

    public void ConnectToPortal(GameObject orangePortalTile)
    {
        connectedPortal = orangePortalTile;
    }

    public void UpdateLastUse () 
    {
        lastUse = Time.time;
    }
}
