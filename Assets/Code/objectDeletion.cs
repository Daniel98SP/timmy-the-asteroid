using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectDeletion : MonoBehaviour
{
    MapGeneration mapGeneration;

    // Start is called before the first frame update
    void Start()
    {
        mapGeneration = GameObject.Find("Sphere").GetComponent<MapGeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < mapGeneration.SafeObjectDeletionCoordinate()) 
        {
            Destroy(gameObject);
        }
    }
}
