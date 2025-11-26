using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private float sens;

    [SerializeField] private Transform playerBody;

    [SerializeField] private Transform cameraHolder;
    float xRotation = 0f;
    float yRotation = 0f;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = cameraHolder.position;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, yRotation,0f);   
        playerBody.rotation = Quaternion.Euler(0,yRotation,0f);
        //playerBody.Rotate(Vector3.up * mouseX);



    }


}
