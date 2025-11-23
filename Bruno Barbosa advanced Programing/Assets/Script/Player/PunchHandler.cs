using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] public bool punching;
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //punching = false;
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

            
    }

    public void punch()
    {
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hit, rayDistance, playerMask))
        {
            Debug.Log(hit.transform.gameObject.name);
            Debug.Log("hit player");
        }
        
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}
