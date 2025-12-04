using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype
{
    public interface IInputReader : IDisposable
    {
        // Read actions from PlayerInput component
        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action Jump;
        public event Action Interact;
        public event Action Special;
        
        // Field for getting the PlayerInput component
        public PlayerInput PlayerInput { get; }
        
        // For InputManager to be able to attach to a PlayerInput
        public void Attach(PlayerInput playerInput);
        public void Detach();
        
        // For enabling/disabling InputReader
        public void Enable();
        public void Disable();
    }
}
