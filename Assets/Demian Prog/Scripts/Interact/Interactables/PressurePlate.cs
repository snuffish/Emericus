using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable
{
    [SerializeField] private Interactable connectedTo;
    [SerializeField] private LayerMask pressureLayers;


    void OnTriggerEnter(Collider col) {
        if (connectedTo.GetComponent<Door>() != null) {
            connectedTo.GetComponent<Door>().SetDoorOpen(true);
        }
    }
    
    void OnTriggerExit(Collider col) {
        if (connectedTo.GetComponent<Door>() != null) {
            connectedTo.GetComponent<Door>().SetDoorOpen(false);
        }
    }
}