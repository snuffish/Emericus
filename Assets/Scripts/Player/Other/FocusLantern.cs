using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLantern : MonoBehaviour
{
    [Header("LightSource")]
    [SerializeField] private Light spotLight;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minIntesity = 50f;
    [SerializeField] private float maxIntesity = 10000f;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float currentTime;
    private float targetAngle;
    private float targetIntesity;
    private float startAngle;

    [Header("Other")] 
    [SerializeField] private float raycastDistance = 30;
    private Ray distanceCheck;
    private float distanceToHit;
    private bool isFocused = false;
    [SerializeField] private Camera Cam;

    void Start() {
        spotLight.spotAngle = maxAngle;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        //  Inputs
        if (Input.GetButtonDown("FocusLantern")) {
            targetAngle = minAngle;
            startAngle = spotLight.spotAngle;
            spotLight.intensity = minIntesity;
            targetIntesity = minIntesity;
            currentTime = 0;
            isFocused = true;
        }
        
        else if (Input.GetButtonUp("FocusLantern")) {
            targetAngle = maxAngle;
            startAngle = spotLight.spotAngle;
            spotLight.intensity = minIntesity - 10;
            targetIntesity = minIntesity - 10;
            currentTime = 0;
            isFocused = false;
        }
        
        //  SpotAngle
        spotLight.spotAngle = Mathf.Lerp(startAngle, targetAngle, currentTime);
        currentTime += Time.deltaTime * transitionSpeed / 10;

        
        //  SpotIntesity
        if (isFocused) {
            distanceCheck = new Ray(Cam.transform.position, Cam.transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(distanceCheck, out hitInfo, raycastDistance)) {
                distanceToHit = Vector3.Distance(hitInfo.point, Cam.transform.position);
            }
            
            Debug.DrawRay(distanceCheck.origin, distanceCheck.direction * raycastDistance, Color.green);

            distanceToHit -= 1f;
            targetIntesity = distanceToHit * maxIntesity / minIntesity;
            
            spotLight.intensity += (targetIntesity - spotLight.intensity) * Time.deltaTime * transitionSpeed / 2;
            Mathf.Clamp(spotLight.intensity, minIntesity, maxIntesity);
        }

    }
}
