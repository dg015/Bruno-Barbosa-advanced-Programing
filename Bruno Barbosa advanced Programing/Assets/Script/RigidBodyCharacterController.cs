using System;
using Unity.VisualScripting;
using UnityEngine;

public class RigidBodyCharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;

    [Header("Speed")]
    //decelerating using drag

    [SerializeField] protected float deceleration;
    [SerializeField] protected float decelerationStrengh;
    [SerializeField] private float dragTime = 1f;

    //acelerating
    [SerializeField] private float acceleration;
    [SerializeField] private float currentMaxSpeed;
    [SerializeField] private float defaultMaxSpeed;
    [SerializeField] private float boostedMaxSpeed;
    //[SerializeField] private AnimationCurve maxSpeedCurve;
    //[SerializeField] private float speedBoostDuration;
    //[SerializeField] private float speedBoostStrengh;

    
    //speed boost while starting walking

     private float currentSpeed;


    [Header("Inputs")]
    float horizontalInput;
    float verticalInput;

    [Header("Camera")]
    [SerializeField] private Transform camera;


    [Header("Grounded")]
    [SerializeField] private LayerMask ground;
    bool grounded;

    [Header("Dodge")]
    Vector3 MoveDirection;
    //dodging
    private float dodgeDurationTimer;
    [SerializeField] private bool dodging;
    private float originalYValue;
    [SerializeField] private AnimationCurve dodgeCurve;
    [SerializeField] private float dodgeDuration;
    //changing the scale while doding

    [SerializeField] private float defaultColliderHeight;
    [SerializeField] private float dodgeForce;
    int keyPresses;
    float DodgeRunTime = 0;
    [SerializeField] private KeyCode lastPressedKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMaxSpeed = defaultMaxSpeed;
        rb = GetComponent<Rigidbody>();
        playerCollider = rb.GetComponent<CapsuleCollider>();    
        rb.freezeRotation = true;
        originalYValue = transform.localPosition.y;
        defaultColliderHeight = playerCollider.height;
    }

    private void getInput()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

    }

    private void MultipleTapInput(KeyCode key, float maxTimer, int requiredKeyPresses)
    {
      
        if(Input.GetKeyDown(key))
        {
            lastPressedKey = key;
            keyPresses++;
        }
        if(keyPresses >0)
        {
            DodgeRunTime += Time.deltaTime;
        }
        if(keyPresses >= requiredKeyPresses)
        {
            //do action
            //for now i call the action here but later make it so I can just pass it as a method
            dodging = true;
            dodgeDurationTimer = 0f;
            Debug.Log("action Started");
            keyPresses = 0;
            DodgeRunTime = 0;
        }
        if(DodgeRunTime >= maxTimer)
        {
            keyPresses = 0;
            DodgeRunTime = 0;
            Debug.Log("time ran out");
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
        getCameraDireciton();
        getInput();
        isGroundedCheck(grounded, 15f);
        stopGravity(grounded);

        MultipleTapInput(KeyCode.D, 1f, 2);
        MultipleTapInput(KeyCode.A, 1f, 2);
        if(Input.GetKeyDown(KeyCode.R))
        {
            dodging = true; 
        }
    }

    private void FixedUpdate()
    {
        movePlayer();

        if (dodging == true)
        {
            crouching();
        }
        else if (dodging == false)
            playerCollider.height = defaultColliderHeight;

    }

    void getCameraDireciton()
    {
        Vector3 camFoward = camera.forward;
        Vector3 camRight = camera.right;

        camFoward.y = 0;
        camRight.y = 0;

        Vector3 fowardRelative = verticalInput * camFoward;
        Vector3 rightRelative = horizontalInput * camRight;

        MoveDirection = fowardRelative + rightRelative;
    }

    private void isGroundedCheck(bool isGrounded, float rayCastDistance)
    {
        if (Physics.Raycast(transform.position, Vector3.down, rayCastDistance, ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }


    private void stopGravity(bool isGrounded)
    {
        if (grounded)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }

    }
    private void crouching()
    {
        currentMaxSpeed = boostedMaxSpeed;
        //start a timer for the duration of the dodge
        dodgeDurationTimer += Time.deltaTime;
        float t = Mathf.Clamp01(dodgeDurationTimer / dodgeDuration);

        //lower the Y value using the curve
        float newY = dodgeCurve.Evaluate(t);
        transform.position = new Vector3(transform.position.x, originalYValue - newY, transform.position.z);
       
        //reduce player height to not cause clipping with the ground
        playerCollider.height = defaultColliderHeight - newY;


        //applying force
        /*
        if(lastPressedKey == KeyCode.A)
        {
            rb.AddForce(- transform.right,ForceMode.Impulse);
        }
        if (lastPressedKey == KeyCode.D)
        {
            rb.AddForce(transform.right, ForceMode.Impulse);
        }
        */
        //when t is bigger then 1 that means the animation is over and the dodge is over
        if (t >= 1f)
        {
            Vector3 afterDodgeSpeed = new Vector3 (0,rb.linearVelocity.y,0);
            rb.linearVelocity = afterDodgeSpeed;
            currentMaxSpeed = defaultMaxSpeed;

            dodging = false;
        }
    }



    /// <summary>
    /// notes for later
    /// 1- decelearation should change the rigidbodies velocity and not the current speed
    /// 2- Watch the video again :https://www.youtube.com/watch?v=qdskE8PJy6Q 3 minute mark and undestand the math
    /// 
    /// </summary>
    private void movePlayer()
    {
        //checking if theres any input from the player
        if(MoveDirection.magnitude > 0.01f)
        {
            //float maxSpeedBoosted = maxSpeedCurve.Evaluate(speedBoostDuration) * speedBoostStrengh;
            
           // maxSpeed = maxSpeed + maxSpeedBoosted;
            rb.linearDamping = 0;
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, currentMaxSpeed);
            rb.AddForce(MoveDirection * currentSpeed, ForceMode.Force);
            
        }
        else
        {
            currentSpeed -=deceleration* Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);

            float newDrag = Mathf.Lerp(decelerationStrengh, 0, dragTime * Time.deltaTime);

            rb.linearDamping = newDrag;
            
        }
    }
}
