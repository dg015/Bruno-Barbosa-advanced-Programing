using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private float sens;

    [SerializeField] private Transform playerBody;

    float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, 0f,0f);   
        playerBody.Rotate(Vector3.up * mouseX);



    }
}
