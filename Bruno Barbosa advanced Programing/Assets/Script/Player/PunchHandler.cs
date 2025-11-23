using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask playerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        punching(rayDistance, playerMask);
    }

    private void punching(float raycatDistace,LayerMask layer)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Physics.Raycast(transform.position,transform.forward,out RaycastHit hit,raycatDistace,layer);
            {
                Debug.Log("hit player");
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}
