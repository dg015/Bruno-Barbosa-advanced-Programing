using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class AttackDirectionDetection : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Image[] images;
    [SerializeField] private Image CurrentIconDirection;

    [Header("Direction identification")]
    [SerializeField] private Vector2 MousePosition;    
    [SerializeField] private Vector2 directionRaw;
    [SerializeField] private float directionAngle;
    [SerializeField] private float MouseMinimumMovement;

    [Header("attack destinguition")]
    [SerializeField] private float AttackTimer;
    [SerializeField] private float HeavyAttackTimerLimit;

    // Update is called once per frame
    void Update()
    {
        MousePosition = Input.mousePositionDelta;
        StartCoroutine(TestAttack());
        


        GetMouseLocation();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    /// <summary>
    /// -------------------------Process to follow----------------------------
    ///first get the new mouse location     -> done
    ///set the center to 0.5( easier for math I think)   --> done
    ///later every frame set the past position as the last location     --> done
    ///if the mouse has moved get location between past location and new location --> done
    ///compare location to angles to get the named direction ( up, left, right, down left, down right)
    ///Paint the equivalent UI element red and set the previous one as white
    ///
    ///todo
    ///use Input.getAxis instead of using the mouse position
    ///
    ///--------------------------End of idea process --------------------------
    ///
    ///------------Angles to direction ------------
    ///Up     60 -> 120
    ///Right   -30 -> 30
    ///Left      150 -> -150
    ///Down right   -60 -> -30
    ///Down left    -120 - 60
    /// --------------------------- attacking---------------------
    /// For attacking 
    /// When just clicking will do the attack and no chance of changing input
    /// If held will charge the attack and and will allow to change the input right before attack
    ///------------Angles to direction ----------
    /// </summary>

    private void GetMouseLocation()
    {
        //check if the mouse hasnt moved AND if the mouse has moved enough to trigger the effect
        if (MousePosition.magnitude > MouseMinimumMovement)
        {
            //get the direction's vector
            directionRaw = MousePosition.normalized;
            //transform it into angles
            directionAngle = Mathf.Atan2(directionRaw.y, directionRaw.x) * Mathf.Rad2Deg;
            AssignAngleToAttack();
            //Debug.Log("mouse Moved");
        }
    }

    /// <summary>
    /// Ideas for testing the simple vs heavy attack
    /// First one
    /// The moment it identifies the button is being held down start a timer
    ///     if the button is released before the expected heavy attack timer then do an simple attack
    ///     else if the button is held past the heavy attack timer trigger a heavy attack
    ///     
    /// Idea: Make this into a coroutine so it first takes the timer and later asigns how much time they spent with the button to its attack
    /// </summary>
    /// 


    private IEnumerator TestAttack()
    {
        //as long as the mosue is being held increment timer
        if (Input.GetMouseButton(0))
        {
            AttackTimer += Time.deltaTime;
        }
        //check if the player has let go of the button to stop timer and check how long they spent holding the button
        if (Input.GetMouseButtonUp(0))
        {
            if (AttackTimer >= HeavyAttackTimerLimit)
            {
                Debug.Log("heavyAttack");
                AttackTimer = 0;
                yield return null;
            }
            else
            {
                Debug.Log("simpleAttack");
                AttackTimer = 0;
                yield return null;
            }
        }
        
    }


    /*
    private void TestAttack()
    {
        //start increment timer
        AttackTimer += Time.deltaTime;
        if( AttackTimer >= HeavyAttackTimerLimit )
        {
            Debug.Log("heavyAttack");
            AttackTimer = 0;
        }
        
        else if( AttackTimer <= HeavyAttackTimerLimit ) 
        {

            Debug.Log("simpleAttack");
            AttackTimer = 0;
        }
        
       
    }
    */


    private void AssignAngleToAttack()
    {
        //remvove the hard coded angles
        if (directionAngle > 60 && directionAngle <= 120)
        {
            CurrentIconDirection = images[1];
            Debug.Log("up");
        }
        else if (directionAngle > -30 && directionAngle <= 30)
        {
            CurrentIconDirection = images[2];
            Debug.Log("right");
        }
        else if (directionAngle > 150 || directionAngle <= -150)
        {
            CurrentIconDirection = images[3];
            Debug.Log("left");
        }
        else if (directionAngle > -90 && directionAngle <= -30)
        {
            CurrentIconDirection = images[4];
            Debug.Log("down right");
        }
        else if (directionAngle > -120 && directionAngle <= -90)
        {
            CurrentIconDirection = images[5];
            Debug.Log("down left");
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
       for(int i = 0; i < images.Length; i++)
        {
            if (images[i] == CurrentIconDirection )
            {
                images[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                images[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
