using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damagePerTick;
    private Health hurtObject;
    private bool shouldItHurt = false;
    
    void Update(){
        if(shouldItHurt)
            hurtObject.Hurt(damagePerTick);
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.GetComponent<Health>() != null) {
            hurtObject = col.gameObject.GetComponent<Health>();
            shouldItHurt = true;
        }
    }

    void OnTriggerExit(Collider col) {

        if (col.gameObject.GetComponent<Health>() != null) {
            if (hurtObject != null) {
                shouldItHurt = false;
            }
        }

        
    }
}