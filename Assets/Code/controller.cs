using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using TMPro;



public class controller : MonoBehaviour
{
    public float forwardSpeed;
    public float lateralSpeed;
    public int pointsToWin;
    public Transform landingHelper;
    public float landingDistance;
    public LayerMask lm;
    public AudioClip landSound;
    public AudioClip pointSound;
    public AudioClip portalSound;
    public AudioClip invertSound;
    public AudioClip dJumpSound;
    public AudioSource aS;
    private TextMeshProUGUI currentScoreText;


    private Rigidbody rb;
    private MapGeneration mapGeneration;
    private GameManager gameManager;
    private Animator playerAnimation;
    private string currentSceneName;
    private bool onGround = true;
    private bool impulseFlag = false;
    private Vector3 impulseForce;
    private int controlsInvertedAux = 1;
    private bool inMovement = true;
    private int doubleJump = 0;
    private float jumpTime = 0.0f;
    private int currentPoints = 0;
    private bool jumping = false;
    private float currentZVelocity = 0;
    private bool ZObstacle = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        mapGeneration = GetComponentInChildren<MapGeneration>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponentInChildren<Animator>();
        playerAnimation.enabled = true;
        currentSceneName = gameManager.CurrentSceneName();
        currentScoreText = GameObject.Find("currentScore").GetComponent<TextMeshProUGUI>();
        currentScoreText.text = "0";
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
            AddPoint();


        if (inMovement)
        {
            JumpAndImpulseManagement();

            GroundDetectionManagement();            

            rb.velocity = onGround
            ? new Vector3(forwardSpeed, rb.velocity.y, Input.GetAxis("Horizontal") * controlsInvertedAux * lateralSpeed * -1)
            : new Vector3(rb.velocity.x, rb.velocity.y, Input.GetAxis("Horizontal") * controlsInvertedAux * lateralSpeed * -1);
        }
        else
        {
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private void FixedUpdate()
    {
        ZMovementManagement();

        if (currentPoints >= pointsToWin)
        {
            currentPoints = 0;
            gameManager.NextScene();
        }
            
        

        float currentH = mapGeneration.GetCurrentHeight();
        if (currentH - 5 > transform.position.y) {
            if(!gameManager.getGodMode())
                gameManager.Die();
            else
            {
                transform.position = new Vector3(transform.position.x, currentH + 5f, 3f);
                rb.velocity = new Vector3(forwardSpeed, 0f, rb.velocity.z);

            }
        }
    }

    public void PlayPortalSound()
    {
        AudioSource.PlayClipAtPoint(portalSound, transform.position);


    }

    private void JumpAndImpulseManagement() {
        if (impulseFlag)
        {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
            rb.AddForce(impulseForce);
            onGround = false;
            impulseFlag = false;
          
        }
        else if (onGround || doubleJump == 1)
        {
            if (Input.GetKey(KeyCode.Space) && jumpTime < 0.2f)
            {
                jumping = true;

                if (!onGround)
                {
                    doubleJump = 2;
                }
                if (Input.GetKeyDown(KeyCode.Space) && jumpTime == 0.0f)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + 250 * Time.deltaTime, rb.velocity.z);
                    jumpTime += Time.deltaTime;
                    aS.pitch = aS.pitch + Random.Range(-0.1f, 0.1f);
                    aS.Play();
                }
                else
                {
                    if (jumpTime >= 0.1f)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + 25 * Time.deltaTime, rb.velocity.z);
                    }

                    jumpTime += Time.deltaTime;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space) || jumpTime == 0.2f)
            {
                jumpTime = 0.0f;
                onGround = false;
                jumping = false;

            }
        }
    }

    private void GroundDetectionManagement() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, landingDistance, lm))
        {
            if (!onGround) {
                playerAnimation.speed = 1;
                AudioSource.PlayClipAtPoint(landSound, transform.position);
            }

            onGround = true;

            if (doubleJump == 2)
            {
                doubleJump = 1;
            }
        }
        else
        {
            if (!jumping) onGround = false;

            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 4 * Time.deltaTime, rb.velocity.z);
            }
            if (rb.velocity.x < 1)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                playerAnimation.speed = 0.5f;
            }
        }
    }

    private void ZMovementManagement()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z + currentZVelocity);

        if (currentSceneName == "Earth" || currentSceneName == "Planet")
        {
            if (ZObstacle) 
            {
                currentZVelocity += 1.2f / (gameObject.transform.position.z - (- 2));
            }            

            if (currentZVelocity >= 0.1f)
            {
                currentZVelocity -= 0.1f;
            }
            else
            {
                currentZVelocity = 0;
            }
        }
        else if (currentSceneName == "Space")
        {
            if (ZObstacle)
            {
                currentZVelocity -= 1.2f / (gameObject.transform.position.z - (-2));
            }

            if (currentZVelocity <= -0.2f)
            {
                currentZVelocity += 0.2f;
            }
            else
            {
                currentZVelocity = 0;
            }
        }
    }

    
    public void InvertControlsTrigger(float n)
    {
        if (controlsInvertedAux == 1)
            StartCoroutine(InvertControlsFor(n));
    }


    public void DoubleJumpTrigger(float n)
    {
        if (doubleJump == 0)
            StartCoroutine(ActivateDoubleJumpFor(n));
    }

    public IEnumerator InvertControlsFor(float n)
    {
        InvertControls();
        gameManager.Efecto((int)n);
        yield return new WaitForSeconds(n);
        InvertControls();

    }

    IEnumerator ActivateDoubleJumpFor(float seconds)
    {
        ActivateDoubleJump();
        gameManager.EfectoJ((int)seconds);
        yield return new WaitForSeconds(seconds);
        DeactivateDoubleJump();

    }

    public void InvertControls()
    {
        Debug.Log("INVERT");

        if (controlsInvertedAux == 1)
        {
            AudioSource.PlayClipAtPoint(invertSound, transform.position);


            controlsInvertedAux = -1;
        }

        else controlsInvertedAux = 1;
    }

    public void ActivateDoubleJump()
    {
        doubleJump = 1;
        AudioSource.PlayClipAtPoint(dJumpSound, transform.position);


    }

    public void DeactivateDoubleJump()
    {
        doubleJump = 0;
    }

    public void ActivateImpulse(Vector3 impulse) {
        impulseFlag = true;
        impulseForce = impulse;
    }

    public void StopMovement() 
    {
        if (!gameManager.getGodMode())
        {
            inMovement = false;
            playerAnimation.enabled = false;
        }
    }

    public void StartMovement()
    {
        inMovement = true;
        playerAnimation.enabled = true;
    }

    public void AddPoint()
    {
        currentPoints += 1;
        currentScoreText.text = currentPoints.ToString();
        AudioSource.PlayClipAtPoint(pointSound, transform.position);

    }

    public void ZVelocityEnter()
    {
        ZObstacle = true;
    }

    public void ZVelocityExit()
    {
        ZObstacle = false;
    }

}

