using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{

    [SerializeField] private Interactable connectedTo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact() {
        connectedTo.Interact();
    }
}