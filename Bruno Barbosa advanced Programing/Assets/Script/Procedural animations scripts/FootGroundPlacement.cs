using UnityEngine;
using UnityEngine.UIElements;

public class FootGroundPlacement : MonoBehaviour
{

    [SerializeField] private LayerMask groundLayer;

    [Header("Raycast")]
    [SerializeField] private float YOffset;
    [SerializeField] private Transform playerModel;
    [SerializeField] private float footspacing;

    [Header("Location")]
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 nextPosition;
    [SerializeField] private float stepDistance;

    [Header("Animation")]
    [SerializeField] private AnimationCurve walkHeightCurve;
    [SerializeField] private Vector3 targetAnimation;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        //set the position as the current position so feet i stuck in locaiton
        transform.position = currentPosition;

        //calculater a new location to have
        Vector3 calculatedFootPosition = playerModel.position + (playerModel.right * footspacing) + Vector3.up * YOffset;
        RaycastHit hit;
        moveFoot();
        if (Physics.Raycast(calculatedFootPosition, Vector3.down,out hit,Mathf.Infinity, groundLayer))
        {

            //set the new location for the closest
            nextPosition = hit.point;
            
            //if the closest location is far enough set it as the new location
            if (Vector3.Distance(currentPosition, nextPosition) >= stepDistance)
            {
                
                currentPosition = targetAnimation;
                //currentPosition = nextPosition;
                Debug.Log("new location found");
            }
            
        }
    }

    /// <summary>
    /// use either move towards or a timer
    /// 
    /// In this case it makes more sense to use a timer since it uses an animation curve
    /// </summary>
    private void moveFoot()
    {
        //D for distance and normalize into the distance
        float totalStepDistance = Vector3.Distance(currentPosition, nextPosition);

        //this will return the current position oof the feet, which in this case will be 0
        float currentFeetDistance = Vector3.Distance(transform.position, currentPosition);

        float d = Mathf.Clamp01(currentFeetDistance / totalStepDistance);

        float newFootHeight = walkHeightCurve.Evaluate(d);
        //Debug.Log(newFootHeight);
        targetAnimation = new Vector3 (transform.position.x,transform.position.y + newFootHeight, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        //raycast draw line not updating??
        Vector3 calculatedFootPosition = playerModel.position + (playerModel.right * footspacing) + Vector3.up * YOffset;
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(calculatedFootPosition,Vector3.down);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nextPosition, 5);
    }


}
