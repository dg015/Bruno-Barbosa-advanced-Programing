using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDirectionDetection : MonoBehaviour
{
    [SerializeField] private Image[] image;
    [SerializeField] private Vector2 oldMousePosition;
    [SerializeField] private Vector2 MousePosition; 

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


        // set the starting mouse location to 0 (center)
        Vector2 startingMouseLocation = new Vector2 (0,0);
        //get the mouse lcoation
        Vector2 mouseLocation = Input.mousePosition;
        // get the center to 0.5 for easier math

        if (MousePosition != oldMousePosition)
        {
            oldMousePosition = MousePosition;
            Debug.Log("mouse Moved");
        }
        else
        {
            Debug.Log("mouse stayed");
            oldMousePosition = MousePosition;
        }
        
    }

    private void updateUI()
    {

    }

}
