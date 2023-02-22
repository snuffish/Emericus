using System.Collections;
using System.Collections.Generic;
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
    
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.GetComponent<Health>() != null) {
            hurtObject = col.gameObject.GetComponent<Health>();
            shouldItHurt = true;
        }
    }

    void OnCollisionExit(Collision col) {
        if (hurtObject != null) {
            shouldItHurt = false;
        }
    }
}