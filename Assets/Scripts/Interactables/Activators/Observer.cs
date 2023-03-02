using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : Activators
{

    [SerializeField] float viewDistance;
    [SerializeField] Transform player;
    [SerializeField] Transform lookFromPoint;
    [SerializeField] List<Transform> targetLookPoints = new List<Transform>();
    [SerializeField] LayerMask targetLayers;
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            lookFromPoint.LookAt(player);
            bool hasSeenTarget = false;
            if (Vector3.Distance(player.position, lookFromPoint.position) < viewDistance)
            {
                foreach (Transform lookPoint in targetLookPoints)
                {
                    Ray ray = new Ray(lookFromPoint.position, (lookPoint.position - lookFromPoint.position).normalized);
                    RaycastHit hitInfo;
                    Debug.DrawRay(ray.origin, ray.direction * viewDistance, Color.red);
                    if (Physics.Raycast(ray, out hitInfo, viewDistance, targetLayers))
                    {
                        if (hitInfo.collider.tag == "Player") hasSeenTarget = true;
                    }
                }
            }
            ChangeState(hasSeenTarget);
        }
        else print("Assign player to observer");
    }
}
