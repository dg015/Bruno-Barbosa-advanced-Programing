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

    [SerializeField] private float maxRecoilTimer = 1;

    [SerializeField] private float currentTimer;

    [SerializeField] public Vector3 recoilDirection;

    [SerializeField] public bool punched;

    [SerializeField] private Vector3 originalPosition;

    [SerializeField] private Vector3 recoilLocation;

    [SerializeField] private bool firstRun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();

        originalPosition = transform.position;
        firstRun = true;
    }

    private void Update()
    {
        if (punched)
        {
            applyRecoil(recoilDirection);
        }
    }

    private void applyRecoil(Vector3 recoilDirection)
    {
        if(firstRun)
        {
            recoilLocation = transform.position + (recoilDirection * force);
            firstRun = false;
        }

        if(currentTimer < maxRecoilTimer)
        {
            currentTimer += Time.deltaTime;
        }
        else if (currentTimer >= maxRecoilTimer)
        {
            currentTimer = 0;
            punched = false;
            firstRun = true;
            return;
        }
        Debug.Log("hurtin");

        //get the base LOCAL position of where the head is usualy at X ONLY
        //I get the position where it should be
        //Then I lerp!

        float t= Mathf.Clamp01(currentTimer/maxRecoilTimer);

        animationTarget.transform.position = Vector3.Lerp(originalPosition, recoilLocation, currentTimer);

        /*
        if (Vector3.Distance(transform.position, recoilLocation)<0.2f)
        {
            Debug.Log("going back");
            Vector3 position = new Vector3(originalPosition.x, transform.position.y,originalPosition.z);
            animationTarget.transform.position = Vector3.Lerp(transform.position, position, t);
        }
        */
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
}
