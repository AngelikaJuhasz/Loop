using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype
{
    public sealed class InputReader : MonoBehaviour, IInputReader
    {
        [Header("References")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerActionsSO _playerActionsSO;
        
        [Header("Settings")]
        [SerializeField] private float _mouseLookSensitivity = 0.1f;
        [SerializeField] private float _gamepadLookSpeed = 180f;
        
        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action Jump;
        public event Action Interact;
        
        public PlayerInput PlayerInput => _playerInput;
        
        private InputAction _move, _look, _jump, _interact;
        private Vector2 _stickLookRate;
        private bool _attached, _disposed;
        
        private void Update()
        {
            if (_stickLookRate == Vector2.zero) return;
            
            Look?.Invoke(_stickLookRate * Time.deltaTime);
        }
        
        private void OnDisable()
        {
            Dispose();
        }
        
        public void Attach(PlayerInput playerInput)
        {
            if (_attached) Detach();
        
            _playerInput = playerInput;

            InputActionAsset actions = _playerInput.actions;
            _move = actions.FindAction(_playerActionsSO.GetReference(InputActionOptions.Move).action.id);
            _look = actions.FindAction(_playerActionsSO.GetReference(InputActionOptions.Look).action.id);
            _jump = actions.FindAction(_playerActionsSO.GetReference(InputActionOptions.Jump).action.id);
            _interact = actions.FindAction(_playerActionsSO.GetReference(InputActionOptions.Interact).action.id);

            _move.performed += OnMovePerformed;
            _move.canceled  += OnMoveCanceled;
            _look.performed += OnLookPerformed;
            _look.canceled  += OnLookCanceled;
            _jump.performed += OnJumpPerformed;
            _interact.performed += OnInteractPerformed;
        
            Enable();
            
            _attached = true;
            _disposed = false;
        }
        
        public void Detach()
        {
            if (!_attached) return;

            Disable();

            _move.performed -= OnMovePerformed;
            _move.canceled  -= OnMoveCanceled;
            _look.performed -= OnLookPerformed;
            _look.canceled  -= OnLookCanceled;
            _jump.performed -= OnJumpPerformed;
            _interact.performed -= OnInteractPerformed;
            
            _move = null;
            _look = null;
            _jump = null;
            _interact = null;
            _playerInput = null;
            _stickLookRate = Vector2.zero;
            
            _attached = false;
        }
        
        public void Enable()
        {
            _move?.Enable();
            _look?.Enable();
            _jump?.Enable();
            _interact?.Enable();
        }
        
        public void Disable()
        {
            _move?.Disable();
            _look?.Disable();
            _jump?.Disable();
            _interact?.Disable();
        }
    
        public void Dispose()
        {
            if (_disposed) return;
        
            Detach();
            
            _disposed = true;
        }
        
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            Move?.Invoke(value.normalized);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context) => Move?.Invoke(Vector2.zero);

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();

            switch (context.control?.device)
            {
                case Mouse:
                    Look?.Invoke(new Vector2(value.x * _mouseLookSensitivity, -value.y * _mouseLookSensitivity));
                    break;
                case Gamepad:
                    _stickLookRate = new Vector2(value.x * _gamepadLookSpeed, -value.y * _gamepadLookSpeed);
                    break;
                default:
                    Debug.Log($"Unknown controller device: {context.control?.device}");
                    break;
            }
        }

        private void OnLookCanceled(InputAction.CallbackContext context) => _stickLookRate = Vector2.zero;

        private void OnJumpPerformed(InputAction.CallbackContext context) => Jump?.Invoke();
        
        private void OnInteractPerformed(InputAction.CallbackContext context) => Interact?.Invoke();
    }
}
