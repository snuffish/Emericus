using System;
using UnityEngine;

namespace Player.Actions {
    public class PlayerMove : MonoBehaviour {
        [SerializeField] private float moveSpeed;
        private Rigidbody _rb;
        private float _horizontal;
        private float _vertical;

        private void Awake() {
            TryGetComponent<Rigidbody>(out _rb);
        }

        private void Update() {
            GetKeyboardInput();
            MovePlayer();
        }

        private void GetKeyboardInput() {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }

        private void MovePlayer() {
            var direction = transform.forward * _vertical + transform.right * _horizontal;
            _rb.AddForce(direction.normalized * (Time.deltaTime * moveSpeed * 300f), ForceMode.Force);
        }
    }
}