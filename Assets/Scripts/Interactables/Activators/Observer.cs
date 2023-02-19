using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : Activators
{

    [SerializeField] private float viewDistance;

    // Update is called once per frame
    void Update()
    {
        //  Check for player, Raycast toward the camera with viewDistance
        //  If the player is visible, activate? deactivate?
        //  If the player isnt visible, the other
        
        //  Future possibility, several view points. If the camera isnt seen, maybe the feet are?

    }
}
