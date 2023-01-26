using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    [SerializeField] private Vector2 mouseSens;
    [SerializeField] private Transform orientation;
    private Vector2 rotation;

    
    // Start is called before the first frame update
    void Start() {
        
        //  Lock the cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        
        //  Get the mouse input
        Vector2 mouseInput;
        mouseInput.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens.x;
        mouseInput.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens.y;

        rotation.y += mouseInput.x;
        
        rotation.x -= mouseInput.y;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        
        
        //  Rotate the camera and player
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
}
