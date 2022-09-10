using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingObstacle : MonoBehaviour
{
    private bool landed = false;
    public AudioClip hit, fall;
    public float fallingSoundDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fallSoundEffect(fallingSoundDelay));
    }

    IEnumerator fallSoundEffect(float n)
    {
        yield return new WaitForSeconds(n);
        AudioSource.PlayClipAtPoint(fall, transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !landed)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Die();
        }
        AudioSource.PlayClipAtPoint(hit, transform.position);
    }

    public void MarkAsLanded() 
    {
        landed = true;
    }
}
