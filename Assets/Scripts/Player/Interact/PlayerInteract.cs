using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerLook))]
public class PlayerInteract : MonoBehaviour
{


    private PlayerLook playerLook;

    [Header("Drag")]
    [SerializeField] float startAngularDrag = 0.05f;
    [SerializeField] float pickUpAngularDrag = 5f;
    [SerializeField] float startDrag = 1f;
    [SerializeField] float pickUpDrag = 8f;

    [Header("Pick up")]
    [SerializeField, Tooltip("Transform that pickup objects try to be close to")] Transform pickupParent = null;
    public GameObject currentlyPickedUpObject;
    Rigidbody pickupRB;
    PhysicsObject physicsObject;

    [Header("Settings")]
    [SerializeField, Tooltip("Check if you want smooth Interperlation")] bool useSmoothInterperlation = true;
    [SerializeField] bool useGravity = false;
    [SerializeField, Tooltip("A More aggresive kinda Force")] bool useImpulseForce = false;
    [SerializeField, Tooltip("dampning using distanceThreshold")] bool useDampning = true;

    [Header("Hold Item")]
    [SerializeField, Tooltip("Min Step Speed when moving picked up Object")] float minSpeed = 0;
    [SerializeField, Tooltip("Max Step Speed")] float maxSpeed = 300f;
    [SerializeField, Tooltip("ScrollSpeed")] float scrollSpeed = 5f;
    [SerializeField, Tooltip("Max Distance object is allowed to move in one step")] float maxDistance = 10f;
    [SerializeField, Tooltip("Distance Item is held from the player")] float holdItemDistance = 4;
    [SerializeField, Tooltip("Distance when item should be dropped")] float dropHeldItemDistance = 8;
    [SerializeField, Tooltip("Min Allowed Scroll distance")] float minScrollDistance = 2;
    [SerializeField, Tooltip("Max Allowed Hold Distance")] float maxScrollDistance = 6;
    [SerializeField, Tooltip("Distance threshold for when the object should stop moving")] float distanceThreshold = 0.2f;
    [SerializeField, Tooltip("Rotation Speed")] Vector2 rotationSens;
    Vector2 rotation;
    float currentSpeed = 0f;
    float currentDist = 0f;
    float currentScroll = 0f;

    bool hasPlayedDropSound = false;

    void Start()
    {
        playerLook = GetComponent<PlayerLook>();
    }

    void Update()
    {
        CheckForConnection();
    }
    void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            pickupParent.position = playerLook.Cam.transform.position + playerLook.Cam.transform.forward * holdItemDistance;
            Vector3 direction = pickupParent.position - pickupRB.position;

            if (!useSmoothInterperlation) currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, currentDist);
            else currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist);
            currentSpeed *= 10;
            currentSpeed *= Time.fixedDeltaTime;
            if (useDampning)
            {
                float dampingFactor = Mathf.Lerp(0, 1, currentDist / distanceThreshold);
                currentSpeed *= dampingFactor;
            }

            //pickupRB.velocity = direction.normalized * currentSpeed;
            if (useImpulseForce) pickupRB.AddForce(direction.normalized * currentSpeed, ForceMode.Impulse);
            else pickupRB.AddForce(direction.normalized * currentSpeed, ForceMode.Force);

            //else
            //{
            //    if (currentDist > 0.1f)
            //    {
            //        currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist);
            //        currentSpeed *= 10;
            //        currentSpeed *= Time.fixedDeltaTime;

            //        pickupRB.velocity = direction.normalized * currentSpeed;
            //        //if (pickupRB.velocity.magnitude > 0.2f)
            //        //{
            //        //    pickupRB.AddForce(-direction.normalized * (pickupRB.velocity.magnitude*0.8f), ForceMode.Force);
            //        //}
            //    }
            //    else
            //    {
            //        Vector3.Lerp(pickupRB.transform.position, pickupParent.position, 0.1f);
            //    }
            //}
            //Rotation
            //lookRot = Quaternion.Slerp(cam.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            //lookRot = Quaternion.LookRotation(cam.transform.position - pickupRB.position);
            //pickupRB.MoveRotation(lookRot);
            //Vector3 torqueDirection = (Quaternion.Slerp(pickupRB.rotation, lookRot, rotationSpeed * Time.deltaTime) * Vector3.forward).normalized;
            //Vector3 torque = torqueDirection * pickupRB.mass * rotationSpeed * Time.deltaTime;
            //pickupRB.AddTorque(torque, ForceMode.Force);

            if (Input.GetButton("RightClick"))
            {

                //  Get the mouse input
                Vector2 mouseInput;
                mouseInput.x = Input.GetAxisRaw("Mouse X") * rotationSens.x;
                mouseInput.y = Input.GetAxisRaw("Mouse Y") * rotationSens.y;

                rotation.y -= mouseInput.x;
                rotation.x -= mouseInput.y;

                //  Rotate the camera and player
                pickupRB.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            }
            currentScroll = Input.GetAxis("Mouse ScrollWheel");
            holdItemDistance = Mathf.Clamp(holdItemDistance + currentScroll * scrollSpeed, minScrollDistance, maxScrollDistance);
        }
    }

    void CheckForConnection()
    {
        if (currentlyPickedUpObject == null) BreakConnection();
        else if (Vector3.Distance(transform.position, currentlyPickedUpObject.transform.position) > dropHeldItemDistance) BreakConnection();
    }
    public void PickUpObject()
    {
        hasPlayedDropSound = false;
        physicsObject = playerLook.LookObject.GetComponentInChildren<PhysicsObject>();
        currentlyPickedUpObject = playerLook.LookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        if (useGravity) pickupRB.useGravity = true;
        else pickupRB.useGravity = false;
        /*pickupRB.constraints = RigidbodyConstraints.FreezeRotation;*/
        pickupRB.angularDrag = pickUpAngularDrag;
        pickupRB.drag = pickUpDrag;
        holdItemDistance = Vector3.Distance(playerLook.Cam.transform.position, pickupRB.transform.position);
        physicsObject.playerInteract = this;
        StartCoroutine(physicsObject.PickUp());
        rotation = pickupRB.rotation.eulerAngles;
    }
    public void BreakConnection()
    {
        if (pickupRB != null)
        {
            if (!hasPlayedDropSound)
            {
                pickupRB.gameObject.GetComponent<PhysicsObject>().PlayDropSound();
                hasPlayedDropSound = true;
            }
            pickupRB.useGravity = true;
            if (!pickupRB.GetComponent<PhysicsObject>().keepRestraints)
            {
                pickupRB.constraints = RigidbodyConstraints.None;
            }
            pickupRB.angularDrag = startAngularDrag;
            pickupRB.drag = startDrag;

            currentlyPickedUpObject = null;
            physicsObject.pickedUp = false;
            currentDist = 0;
        }
    }

}
