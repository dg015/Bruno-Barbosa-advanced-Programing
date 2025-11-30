using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FootGroundPlacement : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform playerModel;
    [SerializeField] private float footspacing;
    [SerializeField] private LayerMask groundLayer;


    [Header("Location")]
    [SerializeField] private float stepDistance;
    [SerializeField] private float velocityPredictionMultiplier = 0.3f;
    private Ray feetRay;
    private Vector3 currentPosition, nextPosition, oldPosition;

    private Vector3 lastPosition;
    private Vector3 velocity;


    [Header("Animation")]
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepSpeed;
    [SerializeField] private float distanceModifier;
    [SerializeField] private float lerp;

    [SerializeField] private FootGroundPlacement other;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RigidBodyCharacterController characterController;

    [Header("Hip Movement")]
    [SerializeField] private OverrideTransform hipBone;
    [SerializeField] private float hipAnimationModifier;

    public bool IsMoving => lerp < 1f;
    [Header("Reposition feet after rotating")]
    [SerializeField] private bool newRotation;
    [SerializeField] private float oldAngle;
    [SerializeField] private bool recalcualte;
    [SerializeField] private int angleToChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        characterController = GetComponentInParent<RigidBodyCharacterController>();
        currentPosition = transform.position;
        nextPosition = transform.position;
        lerp = 2f;

    }

    void Update()
    {
        moveLeg();
        playerSpeedModifier();
        hipMovement(hipAnimationModifier);
        postitioningRotationAdjustament(angleToChange, playerModel);
    }
    private void moveLeg()
    {
        //set the position as the current position so feet i stuck in locaiton
        transform.position = currentPosition;

        feetRay = new Ray(playerModel.position + (playerModel.right * footspacing) + Vector3.up * 2, Vector3.down);

        if (Physics.Raycast(feetRay, out RaycastHit hit, 10, groundLayer))
        {
            //if the closest location is far enough set it as the new location
            if (Vector3.Distance(nextPosition, hit.point) >= stepDistance && !other.IsMoving && lerp >= 1f || recalcualte)
            {
                
               
                lerp = 0f;

                nextPosition = hit.point;
                nextPosition += velocity * velocityPredictionMultiplier;
                oldPosition = transform.position;
                

                //calculatingNextLocaiton(hit);
            }
        }
        if (lerp <= 1f)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, nextPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * stepSpeed;
        }     
    }


    /// <summary>
    /// get transform and set the angle
    /// Check if the player has moved
    /// if not se the immediate angle as the old angle then
    /// Check if the angle has increase over X degrees
    /// 
    ///     if so recalculate the feet
    ///     if not nothing happens and se the current angle as old angle
    /// 
    /// if player has walked dont check
    /// 
    /// </summary>
    private void postitioningRotationAdjustament(float angleToChange, Transform obj)
    {
        //check if player is moving, 1.5 to take into account the drag to slow speed down
        //also check if the boolean for new rotation is false so that it doesnt trigger every frame
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (!newRotation)
            {
                newRotation = true;
                oldAngle = transform.rotation.eulerAngles.y;
            }
            //get the current angle of the game object
            float currentAngle = obj.rotation.eulerAngles.y;
            //get the absolute subtraction between the two angles to check later if the player has moved turned enough
            float finalAngle = Mathf.Abs(currentAngle - oldAngle);

            if (finalAngle >= angleToChange)
            {
                //set the old angle as the current one
                oldAngle = currentAngle;


                //call booleant to recalculate the feet position
                recalcualte = true;
            }
            else
            {
                //if has not moved enough just keep the variable false
                recalcualte = false;
            }
        }
        else
        {
            newRotation = false;
        }
    }
    

    private void hipMovement(float hipAnimationYValue)
    {

        //float newY = Mathf.Lerp(0, walkHipAnimationY, lerp);
        float newY = Mathf.Sin(lerp * Mathf.PI) * hipAnimationYValue;
        hipBone.data.position = new Vector3(0, -newY, 0);
    }

    private void FixedUpdate()
    {
        velocity = (rb.position - lastPosition) / Time.deltaTime;
        lastPosition = rb.position;
    }

    private void playerSpeedModifier()
    {
        //distanceModifier = velocity.magnitude / characterController.defaultMaxSpeed;
        distanceModifier = rb.linearVelocity.magnitude / characterController.defaultMaxSpeed;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Ray ray = new Ray(playerModel.position + (playerModel.right * footspacing), Vector3.down + new Vector3(0,15));
        Gizmos.DrawRay(ray);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nextPosition, 0.5f);
    }


}
