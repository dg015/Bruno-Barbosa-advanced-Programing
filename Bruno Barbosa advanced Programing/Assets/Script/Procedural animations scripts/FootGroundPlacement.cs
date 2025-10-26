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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPosition = transform.position;
    }
    /// <summary>
    /// first set the player feet fixed in current position 
    /// Check if the new player position is ground layer 
    /// Check if the player has moved enough and if so then set the new locaiton as the target
    /// 
    /// </summary>
    void Update()
    {
        //set the position as the current position so feet i stuck in locaiton
        transform.position = currentPosition;

        //calculater a new location to have
        Vector3 calculatedFootPosition = playerModel.position + (playerModel.right * footspacing) + Vector3.up * YOffset;
        RaycastHit hit;
        if(Physics.Raycast(calculatedFootPosition, Vector3.down,out hit,Mathf.Infinity, groundLayer))
        {
            //set the new location for the closest
            nextPosition = hit.point;

            //if the closest location is far enough set it as the new location
            if(Vector3.Distance(currentPosition, nextPosition) >= stepDistance)
            {
                currentPosition = nextPosition;
                Debug.Log("new location found");
            }
            
        }
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
