using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    
    public string promptMessage;
    
    
    
    

    public virtual void Interact() {
        //  Basklass för barnen att ärva från
    }
}
