using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField, Tooltip("Insert Player Cam")] Camera cam;
    [SerializeField, Tooltip("Distance you can reach objects")] float reachDistance;
    [SerializeField, Tooltip("Which Layer Interactable Objects lay in")] LayerMask layerMask;

    [Header("Rotation")]
    Quaternion lookRot;
    public float rotationSpeed = 100f;

    [Header("Selection")]
    public GameObject lookObject;
    [SerializeField, Tooltip("Color of Object when selected")] Color selectColor;
    [SerializeField, Tooltip("Insert Select Crosshair Sprite")] Image selectedCrosshair;

    [Header("Pick up")]
    [SerializeField, Tooltip("Transform that pickup objects try to be close to")] Transform pickupParent = null;
    public GameObject currentlyPickedUpObject;
    Rigidbody pickupRB;
    PhysicsObject physicsObject;


    [Header("Hold Item")]
    [SerializeField, Tooltip("Min Step Speed when moving picked up Object")] float minSpeed = 0;
    [SerializeField,Tooltip("Max Step Speed")] float maxSpeed = 300f;
    [SerializeField,Tooltip("Max Distance object is allowed to move in one step")] float maxDistance = 10f;
    [SerializeField,Tooltip("Distance Item is held from the player")] float holdItemDistance;
    [SerializeField,Tooltip("Distance Allowed before being dropped")] float maxHoldItemDistance;
    float currentSpeed = 0f;
    float currentDist = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckForConnection();
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.red);
        if (lookObject != null)
        {
            Renderer selectionRenderer = lookObject.GetComponent<Renderer>();
            selectionRenderer.material.color = Color.white;
            selectedCrosshair.enabled = false;
            lookObject = null;
        }


        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, reachDistance, layerMask))
        {
            lookObject = hitInfo.collider.gameObject;
            if (Input.GetButtonDown("Interact"))
            {
                if (hitInfo.collider.GetComponent<PhysicsObject>() != null || currentlyPickedUpObject != null)
                {
                    if (currentlyPickedUpObject == null && lookObject != null) PickUpObject();
                }
                else BreakConnection();
                if (hitInfo.collider.GetComponent<Interactable>() != null) hitInfo.collider.GetComponent<Interactable>().Interact();
                /*Debug.Log(hitInfo.collider.GetComponent<Interactable>().promptMessage);*/
            }
            Renderer selectionRenderer = lookObject.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material.color = selectColor;
                selectedCrosshair.enabled = true;
            }
        }
        else
        {
            lookObject = null;
            if (Input.GetButtonDown("Interact") && currentlyPickedUpObject != null)
            {
                BreakConnection();
            }
        }
    }
    void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= 10;
            currentSpeed *= Time.fixedDeltaTime;
            pickupParent.position = cam.transform.position + cam.transform.forward * holdItemDistance;

            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;
            //Rotation
            lookRot = Quaternion.LookRotation(cam.transform.position - pickupRB.position);
            lookRot = Quaternion.Slerp(cam.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(lookRot);
        }
    }

    void CheckForConnection()
    {
        if (currentlyPickedUpObject == null) BreakConnection();
        else if (Vector3.Distance(transform.position, currentlyPickedUpObject.transform.position) > maxHoldItemDistance) BreakConnection();
    }
    public void PickUpObject()
    {
        physicsObject = lookObject.GetComponentInChildren<PhysicsObject>();
        currentlyPickedUpObject = lookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteract = this;
        StartCoroutine(physicsObject.PickUp());
    }
    public void BreakConnection()
    {
        if (pickupRB != null)
        {
            pickupRB.constraints = RigidbodyConstraints.None;
            currentlyPickedUpObject = null;
            physicsObject.pickedUp = false;
            currentDist = 0;
        }
    }

}
