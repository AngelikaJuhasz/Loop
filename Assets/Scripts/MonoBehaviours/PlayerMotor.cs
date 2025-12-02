using System;
using UnityEngine;

namespace Prototype
{
    public sealed class PlayerMotor : MonoBehaviour, IPlayerMotor
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Camera _cam;
        
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private uint _maxJumps = 1;
        
        [Header("Miscellaneous")]
        [SerializeField] private LayerMask _jumpResetLayer;
        
        // TODO: should be separated into its own whole; interaction is not part of player movement
        public static event Action OnInteract;
        public GameObject Player => gameObject;
        
        private Vector2 _moveAxes;
        private float _yaw, _pitch;
        private bool _jumpQueued;
        private uint _remainingJumps;
        
        private void Awake()
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;

            _yaw = transform.eulerAngles.y;
            float angle = _cam.transform.localEulerAngles.x;
            if (angle > 180f) angle -= 360f;
            _pitch = Mathf.Clamp(angle, -90f, 90f);
        
            ResetJumps();
        }
        
        private void Update() => _cam.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        
        private void FixedUpdate()
        {
            _rb.angularVelocity = Vector3.zero;
            _rb.MoveRotation(Quaternion.Euler(0f, _yaw, 0f));

            Vector3 translation = transform.right * _moveAxes.x + transform.forward * _moveAxes.y;
            _rb.linearVelocity = new(translation.normalized.x * _moveSpeed, _rb.linearVelocity.y, translation.normalized.z * _moveSpeed);
        
            if (!_jumpQueued || _remainingJumps <= 0) return;
        
            _jumpQueued = false;
            _remainingJumps--;
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if ((_jumpResetLayer.value & (1 << other.gameObject.layer)) == 0) return;
        
            ResetJumps();
        }
        
        private void ResetJumps()
        {
            _remainingJumps = _maxJumps;
        }

        public void Move(Vector2 axes)
        {
            _moveAxes = axes.normalized;
        }
        
        public void Look(Vector2 deltaDegrees)
        {
            _yaw += deltaDegrees.x;
            _pitch = Mathf.Clamp(_pitch + deltaDegrees.y, -90f, 90f);
        }

        public void Jump()
        {
            if (_remainingJumps <= 0) return;
            
            _jumpQueued = true;
        }
        
        // TODO: should get separated into an interaction system script
        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }
}
