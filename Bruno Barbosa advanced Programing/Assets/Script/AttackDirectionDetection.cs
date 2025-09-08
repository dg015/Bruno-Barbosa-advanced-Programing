using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDirectionDetection : MonoBehaviour
{
    [SerializeField] private Image[] image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getMouseLocation();
    }

    private void getMouseLocation()
    {
        //first get the new mouse location
        //set the center to 0.5( easier for math I think)
        //later every frame set the past position as the last location
        //if the mouse has moved get location between past location and new location
        //compare location to angles to get the named direction ( up, left, right, down left, down right)
        //Paint the equivalent UI element red and set the previous one as white

        Vector2 startingMouseLocation = new Vector2 (0,0);
        Vector2 mouseLocation = Input.mousePosition;
        Vector2 normalizedPosition = new Vector2(mouseLocation.x/Screen.width, mouseLocation.y/Screen.height);


        Debug.Log(normalizedPosition);
    }

    private void updateUI()
    {

    }

}
