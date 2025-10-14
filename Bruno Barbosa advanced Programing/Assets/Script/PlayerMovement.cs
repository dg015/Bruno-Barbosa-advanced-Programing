using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float dodgeTimer;
    [SerializeField] private float dodgeTimerMax;

    [SerializeField] private float speed = 12;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime); 

    }

    private void FixedUpdate()
    {

    }





    /// <summary>
    /// check if the key was pressed
    /// if the key was pressed run timer for x seconds
    /// if the timer runs out set the timer to 0 and reset
    /// if they press the key again in that time period dodge happens
    /// </summary>
    private void dodge()
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
