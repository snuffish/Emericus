
using System.Collections.Generic;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovementController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] public PlayerAudio playerAudio;
    [SerializeField] Animator animator;
    Vector3 direction;

    [Header("Movement")]
    [HideInInspector] public float currentMoveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float crouchSpeed;
    [SerializeField] float blendModifier = 1;

    [HideInInspector] public Vector2 deltaMovement;
    [SerializeField] float groundDrag;
    [SerializeField] float airDragMulyiplier;

    [Header("Ground Check")]
    [SerializeField] float groundCheckRayLength;
    [SerializeField] private float isGroundedCheckRadius;
    private Ray[] groundRays;
    [SerializeField] LayerMask jumpableLayers;
    [SerializeField] public bool isGrounded { get; private set; }


    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float fallForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airmultiplier;
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
        groundRays = new Ray[4];
        currentState = idleState;
        currentState.EnterState(this);
        if (playerAudio == null)
            playerAudio = GetComponent<PlayerAudio>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        // ResetJump();
    }

    // Update is called once per frame
    void Update()
    {

        currentState.UpdateState(this);
        animator.SetFloat("Blend", rb.velocity.magnitude * blendModifier);
        if (Mathf.Approximately(rb.velocity.magnitude, 0f)) animator.CrossFade("MangeIdle_ani", 0.2f);


        //  Airborne Check
        groundRays[0] = new Ray(transform.position + Vector3.forward * isGroundedCheckRadius, Vector3.down);
        groundRays[1] = new Ray(transform.position - Vector3.forward * isGroundedCheckRadius, Vector3.down);
        groundRays[2] = new Ray(transform.position + Vector3.right * isGroundedCheckRadius, Vector3.down);
        groundRays[3] = new Ray(transform.position - Vector3.right * isGroundedCheckRadius, Vector3.down);

        if (!isGrounded)
        {
            foreach (Ray groundRay in groundRays) {
                if (Physics.Raycast(groundRay, groundCheckRayLength, LayerMask.GetMask("Ground")))
                {
                    playerAudio.PlayLand(gameObject);
                }
            }
        }

        foreach (Ray groundRay in groundRays) {
            isGrounded = Physics.Raycast(groundRay, groundCheckRayLength, jumpableLayers);
            if (isGrounded) break;
        }
        
        foreach (Ray groundRay in groundRays) {
            Debug.DrawRay(groundRay.origin, groundRay.direction * groundCheckRayLength, Color.cyan);
        }

        
        //  Change Drag if airborne or not
        if (isGrounded)
            rb.drag = groundDrag;
        else
        {
            rb.drag = groundDrag * airDragMulyiplier;

            //  Added falling gravity for extra snappiness, good for feel
            if (rb.velocity.y < 0)
                rb.AddForce(-transform.up * fallForce * Time.deltaTime);
        }


        //  Check if player wannt to jump and if its possible
        // if (Input.GetButtonDown("Jump") && canJump && isGrounded)
        // {
            // canJump = false;

            // Jump();

            //  Resets the jump after a specific time, adjustable via the inspector
            // Invoke(nameof(ResetJump), jumpCooldown);
        // }

    }

    public void GetInput()
    {
        // Gets and stores the horizontal and vertical keyboard movements in a Vector2
        deltaMovement.x = Input.GetAxis("Horizontal");
        deltaMovement.y = Input.GetAxis("Vertical");
    }

    public void LimitSpeed()
    {
        //  Store the current speed for the player without considering fall speed and jump speed.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //  Checks if the player moves too fast
        if (flatVel.magnitude > currentMoveSpeed)
        {
            //  Limits the speed if its too fast
            Vector3 limitVel = flatVel.normalized * currentMoveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    public void MovePlayer() {
        
        
        //  Calculate direction
        direction = orientation.forward * deltaMovement.y + orientation.right * deltaMovement.x;

        //  On ground
        if (isGrounded)
            rb.AddForce(direction.normalized * Time.deltaTime * currentMoveSpeed * 300f, ForceMode.Force);

        //  In air
        else if (!isGrounded)
            rb.AddForce(direction.normalized * Time.deltaTime * currentMoveSpeed * 300f * airmultiplier, ForceMode.Force);
    }

    void Jump()
    {
        //  Play Jump Audio
        playerAudio.PlayJump(gameObject);

        //  Reset Y-velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //  Adds a force upward
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        //  Impressive code indeed
        canJump = true;
    }

    public void ChangeState(PlayerMovementBaseState state)
    {
        //  Changes the state and runs the enterstate for the new state.
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}