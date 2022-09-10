using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doubleJumpTile : MonoBehaviour
{
    public float seconds;

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
            controller playerController = other.gameObject.GetComponent<controller>();
            playerController.DoubleJumpTrigger(seconds);
            GetComponentInChildren<Animator>().SetBool("activate", true);
        }

        else if (other.gameObject.tag == "fallingObstacle")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<fallingObstacle>().MarkAsLanded();
        }
    }

   
}
