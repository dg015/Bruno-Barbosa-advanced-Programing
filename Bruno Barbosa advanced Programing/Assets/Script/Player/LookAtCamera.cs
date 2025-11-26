using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeReference] private Camera targetCamera;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position);
    }
}
