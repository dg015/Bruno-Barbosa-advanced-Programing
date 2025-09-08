using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDirectionDetection : MonoBehaviour
{
    [SerializeField] private Image[] image;
    [SerializeField] private Vector2 oldMousePosition;
    [SerializeField] private Vector2 MousePosition;
    [SerializeField] private Vector2 directionRaw;
    [SerializeField] private float directionAngle;
    [SerializeField] private float MouseMinimumMovement;

    // Start is called before the first frame update
    void Start()
    {
       
        oldMousePosition = MousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        
        getMouseLocation();
    }

    private void getMouseLocation()
    {
        // -------------------------Process to follow----------------------------
        //first get the new mouse location     -> done
        //set the center to 0.5( easier for math I think)   --> done
        //later every frame set the past position as the last location     
        //if the mouse has moved get location between past location and new location
        //compare location to angles to get the named direction ( up, left, right, down left, down right)
        //Paint the equivalent UI element red and set the previous one as white
        //--------------------------End of idea process --------------------------

        //check if the mouse hasnt moved AND if the mouse has moved enough to trigger the effect
        if (MousePosition != oldMousePosition &&  (MousePosition - oldMousePosition).magnitude > MouseMinimumMovement)
        {
            //get the direction's vector
            directionRaw = (MousePosition - oldMousePosition).normalized;
            //transform it into angles
            directionAngle = Mathf.Atan2(directionRaw.y, directionRaw.x) * Mathf.Rad2Deg;
            
            Debug.Log("mouse Moved");
        }
        else
        {
            Debug.Log("mouse stayed");
        }
        oldMousePosition = MousePosition;
    }

    private void updateUI()
    {

    }

}
