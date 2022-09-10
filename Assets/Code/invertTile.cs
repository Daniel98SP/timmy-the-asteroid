using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invertTile : MonoBehaviour
{
    public float seconds;

    private GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           controller playerController =  other.gameObject.GetComponent<controller>();
            playerController.InvertControlsTrigger(seconds);
            Debug.Log("TRIGGER");
        }

        else if (other.gameObject.tag == "fallingObstacle")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<fallingObstacle>().MarkAsLanded();
        }
    }

   

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
