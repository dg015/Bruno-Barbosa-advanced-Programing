using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float dodgeTimer;
    [SerializeField] private float dodgeTimerMax;

    [SerializeField] private float speed = 12;

    [SerializeField] private float dodgeDuration;

    [SerializeField] private float dodgeDurationTimer;
    [SerializeField] private bool dodging;
    [SerializeField] private float originalYValue;
    [SerializeField] private AnimationCurve dodgeCurve;


    private float crouchHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalYValue = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float startingY = transform.position.y;
        crouchHeight = startingY - crouchHeight;

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);
        Checkdodge();
        //for testing
        if(Input.GetKeyDown(KeyCode.LeftShift) && !dodging)
        {
            dodgeDurationTimer = 0f;
            dodging = true;
        }
        if(dodging == true)
        {
            crouching();
        }
    }


    private void getMovementDirection(Transform player)
    {

    }

    private void crouching( )
    {
        Debug.Log("dodging");

        dodgeDurationTimer += Time.deltaTime;
        float t = Mathf.Clamp01(dodgeDurationTimer/dodgeDuration);

        float newY = dodgeCurve.Evaluate(t);

        transform.position = new Vector3 (transform.position.x, originalYValue - newY, transform.position.z);

        if (t >= 1f)
        {
            dodging = false;
        }
    }



 


    /// <summary>
    /// check if the key was pressed
    /// if the key was pressed run timer for x seconds
    /// if the timer runs out set the timer to 0 and reset
    /// if they press the key again in that time period dodge happens
    /// </summary>
    private void Checkdodge()
    {
        
        //checking for pressing the key
        if(Input.GetKeyDown(KeyCode.A))
        {
            //increase timer
            dodgeTimer += Time.deltaTime;
        }
        if(dodgeTimer > dodgeTimerMax)
        {

        }
    }


}
