using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plainTile : MonoBehaviour
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
        if (other.gameObject.tag == "fallingObstacle")
        {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                other.gameObject.GetComponent<fallingObstacle>().MarkAsLanded();
        }
    }
}
