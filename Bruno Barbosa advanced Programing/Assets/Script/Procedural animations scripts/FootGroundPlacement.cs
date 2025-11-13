using UnityEngine;

public class FootGroundPlacement : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float YOffset;
    [SerializeField] private Transform playerModel;
    [SerializeField] private float footspacing;
    [SerializeField] private LayerMask groundLayer;


    [Header("Location")]
    [SerializeField] private float stepDistance;
    [SerializeField] private float velocityPredictionMultiplier = 0.3f;

    private Vector3 currentPosition, nextPosition, oldPosition;

    private Vector3 lastPosition;
    [SerializeField] private Vector3 velocity;


    [Header("Animation")]
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepSpeed;
    [SerializeField] private float distanceModifier;
    private float lerp;

    [SerializeField] private FootGroundPlacement other;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RigidBodyCharacterController characterController;

    public bool IsMoving => lerp < 1f;

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
    }
    private void moveLeg()
    {
        //set the position as the current position so feet i stuck in locaiton
        transform.position = currentPosition;

        Ray ray = new Ray(playerModel.position + (playerModel.right * footspacing) + Vector3.up * 2, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, groundLayer))
        {
            //if the closest location is far enough set it as the new location
            if (Vector3.Distance(nextPosition, hit.point) >= stepDistance && !other.IsMoving && lerp >= 1f)
            {
                Debug.Log(hit.collider);
                lerp = 0f;

                nextPosition = hit.point;
                nextPosition += velocity * velocityPredictionMultiplier;
                oldPosition = transform.position;
                Debug.Log("new location found");
            }

        }
        if (lerp < 1f)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, nextPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            //oldPosition = nextPosition;
        }

        
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
