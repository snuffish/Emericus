using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovementController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] public PlayerAudio playerAudio;
    //public Animator animator;
    private Vector3 direction;

    [Header("Movement")]
    public float currentMoveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float crouchSpeed;
    
    [HideInInspector] public Vector2 deltaMovement;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDragMulyiplier;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRayLength;
    [SerializeField] private LayerMask jumpableLayers;
    [SerializeField] public bool isGrounded { get; private set; }
    

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airmultiplier;
    private bool canJump;
    
    [Header("States")]
    private PlayerMovementBaseState currentState;

    public PlayerWalkState walkState = new PlayerWalkState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerCrouchState crouchState = new PlayerCrouchState();
    public PlayerIdleState idleState = new PlayerIdleState();
    
    [Header("Sound Parameters")]
    [SerializeField, Tooltip("Time between fotsteps")] public float stepInterval;
    [SerializeField, Tooltip("Minimum velocity before considered moving")] public float minVelocityForSteps;
    public bool isWalking;
    public float currentTime;
    
    
    // Start is called before the first frame update
    void Start() {

        currentState = idleState;
        currentState.EnterState(this);
        if (playerAudio == null) 
            playerAudio = GetComponent<PlayerAudio>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }

    // Update is called once per frame
    void Update() {

        currentState.UpdateState(this);
        if (rb.velocity.magnitude >= minVelocityForSteps)
        {
            isWalking = true;
        }
        else isWalking = false;
        if (isWalking)
        {
            if (currentTime >= stepInterval)
            {
                //Only play Footsteps while grounded (Leon) Clueless
                if (isGrounded)
                {
                    playerAudio.PlayFootstep(gameObject);
                }
                currentTime = 0;
            }
            else currentTime += Time.deltaTime;
        }
        //  Airborne Check
        Ray groundRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction * groundCheckRayLength, Color.cyan);

        isGrounded = Physics.Raycast(groundRay, groundCheckRayLength, jumpableLayers);
        
        //  Change Drag if airborne or not
        if (isGrounded)
            rb.drag = groundDrag;
        else {
            rb.drag = groundDrag * airDragMulyiplier;
            
            //  Added falling gravity for extra snappiness, good for feel
            if(rb.velocity.y < 0)
                rb.AddForce(-transform.up * fallForce * Time.deltaTime);
        } 
            

        //  Check if player wannt to jump and if its possible
        if (Input.GetButtonDown("Jump") && canJump && isGrounded) {
            canJump = false;
            
            Jump();
            
            //  Resets the jump after a specific time, adjustable via the inspector
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }
    
    public void GetInput() {
        // Gets and stores the horizontal and vertical keyboard movements in a Vector2
        deltaMovement.x = Input.GetAxis("Horizontal");
        deltaMovement.y = Input.GetAxis("Vertical");
    }
    
    public void LimitSpeed() {
        //  Store the current speed for the player without considering fall speed and jump speed.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        //  Checks if the player moves too fast
        if (flatVel.magnitude > currentMoveSpeed) {
            //  Limits the speed if its too fast
            Vector3 limitVel = flatVel.normalized * currentMoveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    public void MovePlayer() {

        
        //  Calculate direction
        direction = orientation.forward * deltaMovement.y + orientation.right * deltaMovement.x;

        //  On ground
        if(isGrounded)
            rb.AddForce(direction.normalized * Time.deltaTime * currentMoveSpeed * 300f, ForceMode.Force);
        
        //  In air
        else if(!isGrounded)
            rb.AddForce(direction.normalized * Time.deltaTime * currentMoveSpeed * 300f * airmultiplier, ForceMode.Force);
    }

    void Jump() {
        //  Reset Y-velocity
        playerAudio.PlayJump(gameObject);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        
        
        //  Adds a force upward
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump() {
        //  Impressive code indeed
        canJump = true;
    }

    public void ChangeState(PlayerMovementBaseState state) {
        //  Changes the state and runs the enterstate for the new state.
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
    

}
