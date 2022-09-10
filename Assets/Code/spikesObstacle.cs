using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikesObstacle : MonoBehaviour
{
    Animator spikesAnimation;
    public AudioClip spikeSound;
    

    // Start is called before the first frame update
    void Start()
    {
        spikesAnimation = GetComponentInChildren<Animator>();
        spikesAnimation.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        controller playerController = other.gameObject.GetComponent<controller>();
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(showSpikesAnimation(playerController));
        }

        else if (other.gameObject.tag == "fallingObstacle")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<fallingObstacle>().MarkAsLanded();
        }
    }

    IEnumerator showSpikesAnimation(controller playerController)
    {
        AudioSource.PlayClipAtPoint(spikeSound, transform.position);
        playerController.StopMovement();
        spikesAnimation.enabled = true;
        yield return new WaitForSeconds(1);
        GameObject.Find("GameManager").GetComponent<GameManager>().Die();
    }
}
