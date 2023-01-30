using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float reachDistance;
    [SerializeField] LayerMask layerMask;

    [SerializeField] Transform pickupParent = null;
    [HideInInspector] public GameObject currentlyPickedUpObject;
    [HideInInspector] Rigidbody pickupRB;
    [HideInInspector] PhysicsObject physicsObject;
    [HideInInspector] public GameObject lookObject;

    Quaternion lookRot;
    public float rotationSpeed = 100f;


    [SerializeField] float minSpeed = 0;
    [SerializeField] float maxSpeed = 300f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float holdItemDistance;
    float currentSpeed = 0f;
    float currentDist = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * reachDistance);

        if (Input.GetButtonDown("Interact"))
        {

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, reachDistance, layerMask))
            {
                lookObject = hitInfo.collider.transform.root.gameObject;
                if (hitInfo.collider.GetComponent<PhysicsObject>() != null || currentlyPickedUpObject != null)
                {
                    if (currentlyPickedUpObject == null && lookObject != null) PickUpObject();

                }
                else BreakConnection();
                if (hitInfo.collider.GetComponent<Interactable>() != null) hitInfo.collider.GetComponent<Interactable>().Interact();
                /*Debug.Log(hitInfo.collider.GetComponent<Interactable>().promptMessage);*/
            }
            else lookObject = null;
        }
    }
    void FixedUpdate()
    {
        if (currentlyPickedUpObject != null) {
            
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
