using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInteract))]
public class PlayerLook : MonoBehaviour
{
    private PlayerInteract playerInteract;
    
    [Header("Camera")]
    [SerializeField, Tooltip("Insert Player Cam")] public Camera Cam;
    [SerializeField, Tooltip("Distance you can reach objects")] float reachDistance;
    [SerializeField, Tooltip("Which Layer Interactable Objects lay in")] LayerMask interactLayers;
    
    [Header("Selection")]
    public GameObject LookObject;
    [SerializeField, Tooltip("Color of Object when selected")] Color defaultColor;
    [SerializeField, Tooltip("Color of Object when selected")] Color selectionColor;
    [SerializeField, Tooltip("Insert Select Crosshair Sprite")] Image selectedCrosshair;

    void Start() {
        playerInteract = GetComponent<PlayerInteract>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Cam.transform.position, Cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.red);
        if (LookObject != null)
        {
            Renderer selectionRenderer = LookObject.GetComponent<Renderer>();
            selectionRenderer.material.color = defaultColor;
            selectedCrosshair.enabled = false;
            LookObject = null;
        }
        
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, reachDistance, interactLayers))
        {
            LookObject = hitInfo.collider.gameObject;
            if (Input.GetButtonDown("Interact"))
            {
                if (hitInfo.collider.GetComponent<PhysicsObject>() != null || playerInteract.currentlyPickedUpObject != null)
                {
                    if (playerInteract.currentlyPickedUpObject == null && LookObject != null) playerInteract.PickUpObject();
                }
                else playerInteract.BreakConnection();
                if (hitInfo.collider.GetComponent<Interactable>() != null) hitInfo.collider.GetComponent<Interactable>().Interact();
            }
            Renderer selectionRenderer = LookObject.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material.color = selectionColor;
                selectedCrosshair.enabled = true;
            }
        }
        else
        {
            LookObject = null;
            if (Input.GetButtonDown("Interact") && playerInteract.currentlyPickedUpObject != null)
            {
                playerInteract.BreakConnection();
            }
        }
    }
}
