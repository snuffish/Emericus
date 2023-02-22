
using UnityEngine;

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
    [HideInInspector] public float currentMoveSpeed;
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
    private Ray groundRay;
    

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airmultiplier;
    private bool canJump;
    
    [Header("States")]
    private PlayerMovementBaseState currentState;

    [HideInInspector] public PlayerWalkState walkState = new PlayerWalkState();
    [HideInInspector] public PlayerRunState runState = new PlayerRunState();
    [HideInInspector] public PlayerCrouchState crouchState = new PlayerCrouchState();
    [HideInInspector] public PlayerIdleState idleState = new PlayerIdleState();
    
    [Header("Sound Parameters")]
    [HideInInspector] public float currentStepInterval;
    [SerializeField, Tooltip("Time between fotsteps")] public float stepIntervalWalk;
    [SerializeField, Tooltip("Time between fotsteps")] public float stepIntervalRun;
    [SerializeField, Tooltip("Time between fotsteps")] public float stepIntervalCrouch;
    [HideInInspector] public float currentTime;
    
    
    
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

        
        
        //  Airborne Check
        groundRay = new Ray(transform.position, Vector3.down);
        
        if (!isGrounded) {
            if (Physics.Raycast(groundRay, groundCheckRayLength, LayerMask.GetMask("Ground"))) {
                playerAudio.PlayLand(gameObject);
            }
        }
        
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
        //  Play Jump Audio
        playerAudio.PlayJump(gameObject);
        
        //  Reset Y-velocity
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
