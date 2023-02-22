using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    [SerializeField] Vector2 mouseSens;
    [SerializeField] Transform orientation;
    Vector2 rotation;


    // Start is called before the first frame update
    void Start()
    {
        //  Lock the cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetButton("Rotate"))
        {
            //  Get the mouse input
            Vector2 mouseInput;
            mouseInput.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens.x;
            mouseInput.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens.y;

            rotation.y += mouseInput.x;
            rotation.x += mouseInput.y;
            rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

            //  Rotate the camera and player
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            orientation.rotation = Quaternion.Euler(0, rotation.y, 0);
        }

        
    }
}