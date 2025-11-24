using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class hurtRecoilHandler : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;

    //[SerializeField] private CONSTRAINTHERE constraint;

    [SerializeField] private float force;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private TwoBoneIKConstraint constraint;
    [SerializeField] private GameObject animationTarget;

    [SerializeField] private AnimationCurve recoilWeightCurve;
    [SerializeField] private float recoilDuration;


    [SerializeField] private float maxRecoilTimer = 1;

    [SerializeField] private float currentTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }



    public void applyRecoil(Vector3 recoilDirection)
    {
        if(currentTimer < maxRecoilTimer)
        {
            currentTimer--;
        }
        if(currentTimer == 0)
        {
            currentTimer = maxRecoilTimer;
        }
            constraint.weight = recoilWeightCurve.Evaluate(currentTimer);
        animationTarget.transform.position = transform.position + (recoilDirection * force);
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
