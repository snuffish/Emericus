using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovementPlayer : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    private Vector3 direction;
    [SerializeField] private float riseGravity;
    [SerializeField] private float fallGravity;

    [Header("movement")]
    [SerializeField] private float moveSpeed;
    private Vector2 deltaMovement;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;

    [Header("Ground Check")] 
    [SerializeField] private bool isGrounded;
    [SerializeField] private float playerHeaight;
    [SerializeField] private LayerMask whatIsGround;
    

    [Header("Jump")] 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airmultiplier;
    private bool canJump;
    
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }

    // Update is called once per frame
    void Update() {

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeaight * 0.5f + 0.1f, whatIsGround);

        if (isGrounded)
            rb.drag = groundDrag;
        else {
            rb.drag = airDrag;
            
            if(rb.velocity.y > 0)
                rb.AddForce(-transform.up * riseGravity * Time.deltaTime);
            else 
                rb.AddForce(-transform.up * fallGravity * Time.deltaTime);
                
                
            
        } 
            

        if (Input.GetButtonDown("Jump") && canJump && isGrounded) {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        /*else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
            rb.AddForce(-transform.up * riseGravity / 3);*/

        GetInput();
        LimitSpeed();
        MovePlayer();
    }

    void GetInput() {
        deltaMovement.x = Input.GetAxis("Horizontal");
        deltaMovement.y = Input.GetAxis("Vertical");
    }

    void MovePlayer() {
        //  Calculate direction
        direction = orientation.forward * deltaMovement.y + orientation.right * deltaMovement.x;

        //  on ground
        if(isGrounded)
            rb.AddForce(direction.normalized * Time.deltaTime * moveSpeed * 300f, ForceMode.Force);
        
        //  in air
        else if(!isGrounded)
            rb.AddForce(direction.normalized * Time.deltaTime * moveSpeed * 300f * airmultiplier, ForceMode.Force);
    }

    void LimitSpeed() {

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        //  Limit velocity
        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    void Jump() {
        //  Reset Y-velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump() {
        canJump = true;
    }
    

}
