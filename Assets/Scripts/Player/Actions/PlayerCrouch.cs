using UnityEngine;

namespace Player.Actions {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCrouch : MonoBehaviour {
        private Rigidbody _rb;

        private void Awake() {
            TryGetComponent<Rigidbody>(out _rb);
        }

        private void Update() {
            var holdButton = Input.GetKeyDown(KeyCode.LeftControl);
            var releaseButton = Input.GetKeyUp(KeyCode.LeftControl);

            if (holdButton)
                _rb.transform.position -= Vector3.up * .85f;

            if (releaseButton)
                _rb.transform.position += Vector3.up * .5f;
        }
    }
}