using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Activators
{
    public override void Interact() 
    {
        if (!isLocked)
        {
            InvertState();
        }
    }
}