using UnityEngine;

namespace Prototype
{
    public sealed class Carriable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        
        public void PickUp()
        {
            GetComponent<Collider>().isTrigger = true;
            
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }

        public void Drop(Vector3 velocity)
        {
            GetComponent<Collider>().isTrigger = false;
            
            _rb.isKinematic = false;
            _rb.useGravity = true;
            
            _rb.linearVelocity = velocity;
        }
    }
}
