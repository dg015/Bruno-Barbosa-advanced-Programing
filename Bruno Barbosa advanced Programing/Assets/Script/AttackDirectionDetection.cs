using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttackDirectionDetection : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private Vector2 oldMousePosition;
    [SerializeField] private Vector2 MousePosition;
    [SerializeField] private Vector2 directionRaw;
    [SerializeField] private float directionAngle;
    [SerializeField] private float MouseMinimumMovement;
    [SerializeField] private Image CurrentIconDirection;

    // Known issues
    // Right now its kinda finicky
    // You can run out of screen to attack 


    void Start()
    {

        oldMousePosition = MousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        
        getMouseLocation();
        Cursor.visible = false;
    }



    private void getMouseLocation()
    {
        // -------------------------Process to follow----------------------------
        //first get the new mouse location     -> done
        //set the center to 0.5( easier for math I think)   --> done
        //later every frame set the past position as the last location     --> done
        //if the mouse has moved get location between past location and new location --> done
        //compare location to angles to get the named direction ( up, left, right, down left, down right)
        //Paint the equivalent UI element red and set the previous one as white

        //todo
        //use Input.getAxis instead of using the mouse position

        //--------------------------End of idea process --------------------------

        //------------Angles to direction ------------
        //Up     60 -> 120
        //Right   -30 -> 30
        //Left      150 -> -150
        //Down right   -60 -> -30
        //Down left    -120 - 60
        //------------Angles to direction ------------


        //check if the mouse hasnt moved AND if the mouse has moved enough to trigger the effect
        if (MousePosition != oldMousePosition &&  (MousePosition - oldMousePosition).magnitude > MouseMinimumMovement)
        {
            //get the direction's vector
            directionRaw = (MousePosition - oldMousePosition).normalized;
            //transform it into angles
            directionAngle = Mathf.Atan2(directionRaw.y, directionRaw.x) * Mathf.Rad2Deg;
            AssignAngleToAttack();
            //Debug.Log("mouse Moved");
        }
        else
        {
            //Debug.Log("mouse stayed");
        }
        oldMousePosition = MousePosition;
    }

    private void AssignAngleToAttack()
    {
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
        updateUI();
    }

    private void updateUI()
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
