using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] public bool punching;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 recoilDirection;

    [SerializeField] private Transform enemy;
    [SerializeField] private hurtRecoilHandler recoilHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //punching = false;
        animator = GetComponentInParent<Animator>();
    }


    public void punch()
    {
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hit, rayDistance, playerMask))
        {
            enemy = hit.transform;

          
            //why subtract both directions?
            //since when you're punched the direction of where the limb recoils to is based on where the punch was dealt 
            // and the where it landed by subtracting both locations I find and in between direction
            recoilDirection = (transform.forward - hit.normal).normalized;

            Debug.Log("name:" +hit.collider.gameObject.name.ToString()  +  "," + hit.collider.gameObject.layer.ToString());
            
            recoilHandler = hit.collider.gameObject.GetComponent<hurtRecoilHandler>();
            recoilHandler.recoilDirection = recoilDirection;


            recoilHandler.punched = true;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.red;
        if(enemy != null) 
            Gizmos.DrawRay(enemy.transform.position, recoilDirection);
    }

}
