using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damagePerTick;
    
    void OnCollisionStay(Collision col) {
        if (col.gameObject.GetComponent<Health>() != null) {
            col.gameObject.GetComponent<Health>().Hurt(damagePerTick);
        }
    }
}