using UnityEngine;

public class punchCaller : MonoBehaviour
    
{
    [SerializeField] private PunchHandler rightFist;
    [SerializeField] private PunchHandler leftFist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void leftPunchEvent()
    {
        leftFist.punch();
    }

    public void rightPunchEvent()
    {
        rightFist.punch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
