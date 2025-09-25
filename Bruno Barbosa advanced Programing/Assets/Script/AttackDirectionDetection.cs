using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class AttackDirectionDetection : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Transform UIIconsGroup;
    [SerializeField] private Image[] images;
    [SerializeField] private Image CurrentIconDirection;
    [SerializeField] private GameObject UIElementsObject;
    [SerializeField] private Camera cam;

    [Header("Direction identification")]
    [SerializeField] private Vector2 MousePosition;    
    [SerializeField] private Vector2 directionRaw;
    [SerializeField] private float directionAngle;
    [SerializeField] private float MouseMinimumMovement;

    [Header("attack destinguition")]
    [SerializeField] private float AttackTimer;
    [SerializeField] private float HeavyAttackTimerLimit;
    [SerializeField] private bool isCombat = false;

    [Header("Enemy detection")]
    [SerializeField] private bool enemiesNear;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private float maxdistance;
    [SerializeField] private GameObject closestEnemy;

    static RaycastHit[] hit = new RaycastHit[128];

    [Header("Animation")]
    [SerializeField] private Animator animator;


    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UIElementsObject.SetActive(false);
    }
    void Update()
    {
        searchForEnemies();
        startCombat();
        if (isCombat && enemiesNear)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(closestEnemy.transform.position);
            UIIconsGroup.position = screenPos;

            
            UIElementsObject.SetActive(true);
            MousePosition = Input.mousePositionDelta;
            StartCoroutine(TestAttack());
            GetMouseLocation();

        }
        else if(!isCombat)
        {
            
            UIElementsObject.SetActive(false);
        }


    }

    private void startCombat()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isCombat == false)
        {
            isCombat = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isCombat == true)
        {
            isCombat = false;
        }
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
            
        }
    }

    private void searchForEnemies()
    {
        int hits = Physics.SphereCastNonAlloc(transform.position, radius, transform.forward, hit, maxdistance, EnemyLayer);
        if(hits > 0)
        {
            float ClosestDistance = Mathf.Infinity; 
            enemiesNear = true;
            
            for (int i = 0; i < hits; i++)
            {
               float distance = Vector3.Distance(hit[i].collider.transform.position, transform.position);
                if (ClosestDistance > distance)
                {
                    ClosestDistance = distance;
                    closestEnemy = hit[i].collider.gameObject;
                }
            }
        }
        else
        {
            enemiesNear = false;
        }
    }

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
                animator.SetTrigger("LeftAttack");//for now using the same left attack for everything
                Debug.Log("heavyAttack");
                AttackTimer = 0;
                yield return null;
            }
            else
            {
                animator.SetTrigger("LeftAttack");//for now using the same left attack for everything
                Debug.Log("simpleAttack");
                AttackTimer = 0;
                yield return null;
            }
        }
        
    }


    private void ProceduralAnimationHandler()
    {

    }

    private void OnDrawGizmos()
    {
        // Draw the starting sphere (at player position)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Draw the ending sphere at the max distance along forward
        Vector3 endPosition = transform.position + transform.forward * maxdistance;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, radius);

        // Draw a line connecting the start and end spheres to visualize sweep
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, endPosition);

        
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, maxdistance, EnemyLayer))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.point);
            Gizmos.DrawWireSphere(hit.point, radius * 0.5f); // visualize hit point
        }
    }


    private void AssignAngleToAttack()
    {
        //remvove the hard coded angles
        if (directionAngle > 60 && directionAngle <= 120)
        {
            CurrentIconDirection = images[0];
            //Debug.Log("up");
        }
        else if (directionAngle > -30 && directionAngle <= 30)
        {
            CurrentIconDirection = images[1];
            //Debug.Log("right");
        }
        else if (directionAngle > 150 || directionAngle <= -150)
        {
            CurrentIconDirection = images[2];
            //Debug.Log("left");
        }
        else if (directionAngle > -90 && directionAngle <= -30)
        {
            CurrentIconDirection = images[3];
            //Debug.Log("down right");
        }
        else if (directionAngle > -120 && directionAngle <= -90)
        {
            CurrentIconDirection = images[4];
            //Debug.Log("down left");
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
