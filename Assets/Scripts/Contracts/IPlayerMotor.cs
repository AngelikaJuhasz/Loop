using UnityEngine;

namespace Prototype
{
    public interface IPlayerMotor
    {
        // For getting the root GameObject of Player
        public GameObject Player { get; }
        
        // Methods invoked by InputReader
        public void Move(Vector2 axes);
        public void Look(Vector2 deltaDegrees);
        public void Jump();
        public void Interact();
        public void Special();
    }
}
