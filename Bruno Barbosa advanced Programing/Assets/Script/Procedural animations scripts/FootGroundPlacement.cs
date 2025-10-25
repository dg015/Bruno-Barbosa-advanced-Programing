using UnityEngine;

public class FootGroundPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    /// <summary>
    /// BUG 
    /// the feet location is only calculated when the body goes up, when going down it stops calculating
    /// Fix: Make the raycast higher?
    /// FIXED
    /// new bug
    /// Now the feet is being calculated exaclty where the head is, needs to do the spacing
    /// FIXED
    /// </summary>
    void Update()
    {
        Vector3 calculatedFootPosition = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        RaycastHit hit;
        if(Physics.Raycast(calculatedFootPosition, Vector3.down,out hit,Mathf.Infinity, groundLayer))
        {
            transform.position = hit.point;
            Debug.Log(hit.ToString());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,Vector3.down);
    }


}
