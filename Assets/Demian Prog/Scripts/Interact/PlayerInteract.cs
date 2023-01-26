using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private float reachDistance;
    [SerializeField] private LayerMask layerMask;
    
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * reachDistance);

        if (Input.GetButtonDown("Interact")) {

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, reachDistance, layerMask)) {
                if(hitInfo.collider.GetComponent<Interactable>() != null)
                    /*Debug.Log(hitInfo.collider.GetComponent<Interactable>().promptMessage);*/
                    hitInfo.collider.GetComponent<Interactable>().Interact();
            }
        }
    }
}