using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impulseTile : MonoBehaviour
{
    public Vector3 impulseForce;
    public AudioClip soundEffect;

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
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<controller>().ActivateImpulse(impulseForce);
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);

        }

        else if (other.gameObject.tag == "fallingObstacle")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<fallingObstacle>().MarkAsLanded();
        }
    }
}
