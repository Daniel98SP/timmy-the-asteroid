using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followSphere : MonoBehaviour
{

    public Transform sphere;
    public Vector3 offset;
    public float smooth;
    private Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = sphere.forward;
    }

    // Update is called once per frame
    void Update()
    {

        
    }


    private void FixedUpdate()
    {
        transform.position = new Vector3(sphere.position.x - offset.x, sphere.position.y - offset.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, sphere.position - offset, ref vel, smooth);
    }
}
