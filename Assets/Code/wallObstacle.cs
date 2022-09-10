using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallObstacle : MonoBehaviour
{
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
            GameObject.Find("GameManager").GetComponent<GameManager>().Die();
        }

        else if (other.gameObject.tag == "fallingObstacle")
        {
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody plainTileRigidBody = gameObject.AddComponent<Rigidbody>();
                other.gameObject.GetComponent<fallingObstacle>().MarkAsLanded();
            }
        }
    }
}
