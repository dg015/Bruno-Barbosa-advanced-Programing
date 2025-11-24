using UnityEngine;

public class hurtRecoilHandler : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;

    //[SerializeField] private CONSTRAINTHERE constraint;

    [SerializeField] private float force;

    [SerializeField] private LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //layermask.valuye converts it to nan interget value
    private void OnTriggerEnter(Collider other)
    {
        //make sure only to detect when getting hit by the player
        if (other.gameObject.layer == 8)
        {
            Debug.Log("player");

        }
    }

    //check for the colisions
    private void OnCollisionEnter(Collision collision)
    {



    }
}
