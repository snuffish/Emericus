using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    [SerializeField] float originSens;
    [SerializeField] float currentSens;
    [SerializeField] Transform orientation;
    [SerializeField] private PlayerData playerData;
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
    void Update() {
        
        currentSens = originSens * playerData.mouseSensitivity;
        
        if (!Input.GetButton("Rotate"))
        {
            //  Get the mouse input
            Vector2 mouseInput;
            mouseInput.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * currentSens;
            mouseInput.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * currentSens;

            rotation.y += mouseInput.x;
            rotation.x += mouseInput.y;
            rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

            //  Rotate the camera and player
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            orientation.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
    }
}