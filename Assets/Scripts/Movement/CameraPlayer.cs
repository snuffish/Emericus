using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    [SerializeField] Vector2 mouseSens;
    [SerializeField] Transform orientation;
    [SerializeField] GameObject optionsPanel;
    Vector2 rotation;
    Vector2 currentSensitivty;
    Vector2 currentRotationSensitivty;
    bool settingsIsActive;


    // Start is called before the first frame update
    void Start()
    {
        currentSensitivty = Vector2.one;
        //  Lock the cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (!Input.GetButton("RightClick") && !settingsIsActive)
        {
            //  Get the mouse input
            Vector2 mouseInput;
            mouseInput.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime *(mouseSens.x* currentSensitivty.x);
            mouseInput.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (mouseSens.y*currentSensitivty.y);

            rotation.y += mouseInput.x;

            rotation.x -= mouseInput.y;
            rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);


            //  Rotate the camera and player
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            orientation.rotation = Quaternion.Euler(0, rotation.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) OpenSettings();
    }

    void OpenSettings()
    {
        switch (settingsIsActive)
        {
            case true:
                optionsPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case false:
                optionsPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                break;
        }
        settingsIsActive = !settingsIsActive;
    }
    public void SetSensitivty(float input)
    {
        currentSensitivty = input*mouseSens;
    }
    public void SetRotationSensitivty(float input)
    {
       // currentSensitivty = input * mouseSens;
    }
}
