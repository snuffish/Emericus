using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Player.Actions {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerJump : MonoBehaviour {
        [SerializeField, Tooltip("The RigidBody Jump Force")]
        private float jumpForce = 18f;
        [SerializeField, Tooltip("Max distance to the Ground RayCast")]
        private float groundCheckDistance = 2.1f;
        
        private Rigidbody _rb;
        private bool _isGrounded;
        private RaycastHit _groundRaycastHit;

        private void Awake() {
            TryGetComponent<Rigidbody>(out _rb);
        }

        private void Update() {
            var canJump = _isGrounded && Input.GetKeyDown(KeyCode.Space);
            if (canJump)
                Jump();

            CheckIfGrounded();
        }

        private void Jump() {
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        
        private void CheckIfGrounded() {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, out _groundRaycastHit, groundCheckDistance);
        }

        private void OnDrawGizmos() {
            if (!_groundRaycastHit.collider) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, -_groundRaycastHit.transform.up + (Vector3.down * groundCheckDistance));
        }
    }
}
