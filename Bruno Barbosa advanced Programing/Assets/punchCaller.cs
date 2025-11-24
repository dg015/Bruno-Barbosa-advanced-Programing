using UnityEngine;

public class punchCaller : MonoBehaviour
    
{
    [SerializeField] private PunchHandler rightFist;
    [SerializeField] private PunchHandler leftFist;


    public void leftPunchEvent()
    {
        leftFist.punch();
    }

    public void rightPunchEvent()
    {
        rightFist.punch();
    }
}
