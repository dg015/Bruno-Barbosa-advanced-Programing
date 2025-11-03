using Unity.VisualScripting;
using UnityEngine;

public class RigidBodyCharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Header("Speed")]
    //decelerating using drag

    [SerializeField] protected float deceleration;
    [SerializeField] protected float decelerationStrengh;
    [SerializeField] private float dragTime = 1f;

    //acelerating
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    //[SerializeField] private AnimationCurve maxSpeedCurve;
    //[SerializeField] private float speedBoostDuration;
    //[SerializeField] private float speedBoostStrengh;

    
    //speed boost while starting walking

    [SerializeField] private float currentSpeed;


    [Header("Inputs")]
    float horizontalInput;
    float verticalInput;

    [Header("Camera")]
    [SerializeField] private Transform camera;


    [Header("Grounded")]
    [SerializeField] private LayerMask ground;
    bool grounded;

    Vector3 MoveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void getInput()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

    }

    // Update is called once per frame
    void Update()
    {
        getCameraDireciton();
        getInput();
        isGroundedCheck(grounded, 15f);
        stopGravity(grounded);
       
    }
    private void FixedUpdate()
    {
        movePlayer();
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
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
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
