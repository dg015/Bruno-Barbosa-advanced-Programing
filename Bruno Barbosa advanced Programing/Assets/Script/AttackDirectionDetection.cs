using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
    private Vector2 MousePosition;    
    private Vector2 directionRaw;
    private float directionAngle;
    [SerializeField] private float MouseMinimumMovement;
    [SerializeField] private string currentAttackAngle;

    [Header("attack destinguition")]
    [SerializeField] private float AttackTimer;
    [SerializeField] private float HeavyAttackTimerLimit;
    private bool isCombat = false;

    [Header("Enemy detection")]
    private bool enemiesNear;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private float maxdistance;
    [SerializeField] private GameObject closestEnemy;

    static RaycastHit[] hit = new RaycastHit[128];

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationCurve attackWeightCurve;
    [SerializeField] TwoBoneIKConstraint LeftArmAnimation;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private GameObject animationTarget;
    [SerializeField] private Transform animationTargetRestingPosition;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UIElementsObject.SetActive(false);
    }
    void Update()
    {
        //searchForEnemies();
        startCombat();
        checkForCloseEnemies(transform.position, sphereCastRadius, EnemyLayer);
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
    ///------------Angles to direction ------------
    ///Up     60 -> 120
    ///Right   -30 -> 30
    ///Left      150 -> -150
    ///Down right   -60 -> -30
    ///Down left    -120 - 60
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

    /*
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

    */


    private void checkForCloseEnemies(Vector3 center, float radius, LayerMask enemy)
    {
        //create sphere to check for coliders
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemy);

        //run through the array
        if (hitColliders.Length > 0)
        {
            float ClosestDistance = Mathf.Infinity;
            enemiesNear = true;

            for (int i = 0; i < hitColliders.Length; i++)
            {
                float distance = Vector3.Distance(hitColliders[i].transform.position, transform.position);

                if (ClosestDistance > distance)
                {
                    ClosestDistance = distance;
                    closestEnemy = hitColliders[i].gameObject;
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("LeftPunch"))
            {
                //if the animation is playing

                //animationTarget.transform.position = hitColliders[0].gameObject.transform.position;
                //animationTarget.transform.position = closestEnemy.transform.Find("Head").GetComponent<Transform>().position;
                AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                

                LeftArmAnimation.weight = attackWeightCurve.Evaluate(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            }
        }
        else
        {
            enemiesNear = false;
            animationTarget.transform.position = animationTargetRestingPosition.position;
        }
    }


    private void proceduralAnimationManager()
    {
        switch (currentAttackAngle)
        {
            case "up":
                //play correct animation for now Im using left punch
                animator.SetTrigger("LeftAttack");
                //set attack location
                animationTarget.transform.position = closestEnemy.transform.Find("Head").GetComponent<Transform>().position;
                break;
            case "right":
                //play correct animation for now Im using left punch
                animator.SetTrigger("LeftAttack");
                //set attack location
                animationTarget.transform.position = closestEnemy.transform.position;
                break;
            case "left":
                //play correct animation for now Im using left punch
                animator.SetTrigger("LeftAttack");
                //set attack location
                animationTarget.transform.position = closestEnemy.transform.position;
                break;
            case "down right":
                //play correct animation for now Im using left punch
                animator.SetTrigger("LeftAttack");
                //set attack location
                animationTarget.transform.position = closestEnemy.transform.Find("Gut Left").GetComponent<Transform>().position;
                break;
            case "down left":
                //play correct animation for now Im using left punch
                animator.SetTrigger("LeftAttack");
                //set attack location
                animationTarget.transform.position = closestEnemy.transform.Find("Gut Right").GetComponent<Transform>().position;
                break;
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
                proceduralAnimationManager();//for now using the same left attack for everything
                Debug.Log("heavyAttack");
                AttackTimer = 0;
                yield return null;
            }
            else
            {
                proceduralAnimationManager();//for now using the same left attack for everything
                Debug.Log("simpleAttack");
                AttackTimer = 0;
                yield return null;
               
            }
        }
        
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
            currentAttackAngle = "up";
            //Debug.Log("up");
        }
        else if (directionAngle > -30 && directionAngle <= 30)
        {
            CurrentIconDirection = images[1];
            currentAttackAngle = "right";
            //Debug.Log("right");
        }
        else if (directionAngle > 150 || directionAngle <= -150)
        {
            CurrentIconDirection = images[2];
            currentAttackAngle = "left";
            //Debug.Log("left");
        }
        else if (directionAngle > -90 && directionAngle <= -30)
        {
            CurrentIconDirection = images[3];
            currentAttackAngle = "down right";
            //Debug.Log("down right");
        }
        else if (directionAngle > -120 && directionAngle <= -90)
        {
            CurrentIconDirection = images[4];
            currentAttackAngle = "down left";
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
