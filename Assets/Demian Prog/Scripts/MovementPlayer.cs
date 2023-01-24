using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class MovementPlayer : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    private Vector3 deltaMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaMovement.x = Input.GetAxis("Horizontal");
        deltaMovement.y = 0;
        deltaMovement.z = Input.GetAxis("Vertical");
        
        Debug.Log(deltaMovement.x);
        Debug.Log(deltaMovement.z);
        
        MovePlayer(deltaMovement * moveSpeed * Time.deltaTime);
    }

    void MovePlayer(Vector3 deltaMovement)
    {
        rb.MovePosition(transform.position + deltaMovement);
    }
}
