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
    [SerializeField] private Vector3 oldPosition;
    [SerializeField] private float stepDistance;

    [Header("Animation")]
    [SerializeField] private float lerp;
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepSpeed;
    [SerializeField] private float distanceModifier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentPosition = transform.position;
    }

    void Update()
    {
        //set the position as the current position so feet i stuck in locaiton
        transform.position = currentPosition;

        Ray ray = new Ray(playerModel.position + (playerModel.right * footspacing), Vector3.down);
        
        if (Physics.Raycast(ray, out RaycastHit hit,10,groundLayer))
        {
            //if the closest location is far enough set it as the new location
            if (Vector3.Distance(nextPosition, hit.point) >= stepDistance)
            {

                lerp = 0;

                nextPosition = hit.point;
                //currentPosition = nextPosition;
                Debug.Log("new location found");
            }
            
        }
        if(lerp <1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, nextPosition,lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            oldPosition = nextPosition;
        }

        
    }

    

    /// <summary>
    /// use either move towards or a timer
    /// 
    /// In this case it makes more sense to use a timer since it uses an animation curve
    /// </summary>
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Ray ray = new Ray(playerModel.position + (playerModel.right * footspacing), Vector3.down + new Vector3(0,15));
        Gizmos.DrawRay(ray);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nextPosition, 0.5f);
    }


}
