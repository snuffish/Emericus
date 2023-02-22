using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClamp : MonoBehaviour
{
    [SerializeField] float maxClampDistance = 0.2f;
    [SerializeField] Camera mainCamera;

    void Update()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Quaternion cameraRotation = mainCamera.transform.rotation;

        RaycastHit hit;
        if(Physics.Raycast(cameraPosition,transform.position - cameraPosition, out hit, maxClampDistance))
        {
            Vector3 closestPoint = hit.collider.ClosestPointOnBounds(transform.position);

            transform.position = closestPoint;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * cameraRotation;

        }
    }
}
