using UnityEngine;

namespace Prototype
{
    // should probably definitely be a ScriptableObject powerup
    // TODO: make "interact" and/or "special" go over every power and equipment
    public sealed class CarryPower : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _carryPoint;
        
        [SerializeField] private LayerMask _carryableMask;

        [SerializeField] private float _throwBoost = 3f;
        
        private GameObject _carriedObject;
        private bool _carrying;

        private Vector3 _lastCarryPos;
        private Vector3 _carryPointVel;
        
        private void OnEnable() => PlayerMotor.OnInteract += ToggleCarryState;
        
        private void OnDisable() => PlayerMotor.OnInteract -= ToggleCarryState;
        
        private void FixedUpdate()
        {
            if (!_carryPoint) return;
            Vector3 pos = _carryPoint.position;
            _carryPointVel = (pos - _lastCarryPos) / Time.fixedDeltaTime;
            _lastCarryPos = pos;
        }
        
        private void OnDestroy()
        {
            Drop();
        }

        public void ToggleCarryState()
        {
            if (_carrying)
            {
                Drop();
                return;
            }
            
            TryPickup();
        }
        
        private void Drop()
        {
            if (!_carrying) return;

            Carriable carriable = _carriedObject.GetComponent<Carriable>();
            
            if (carriable)
            {
                Vector3 releaseVel = _carryPointVel + _camera.transform.forward * _throwBoost;
                carriable.Drop(releaseVel);
            }
            
            _carriedObject.transform.SetParent(null, worldPositionStays: true);
            
            _carriedObject = null;
            _carrying = false;
        }
        
        private void TryPickup()
        {
            if (_carrying) return;
            
            Ray ray = new(_camera.transform.position, _camera.transform.forward);
            
            if (!Physics.Raycast(ray, out RaycastHit hit, 2f, _carryableMask)) return;
            
            Carriable carriable = hit.collider.GetComponentInParent<Carriable>();

            if (!carriable) return;
            
            carriable.transform.SetParent(_carryPoint, worldPositionStays: true);
            
            carriable.transform.localPosition = Vector3.zero;
            carriable.transform.localRotation = Quaternion.identity;
            
            _lastCarryPos = _carryPoint.position;
            _carryPointVel = Vector3.zero;
            
            carriable.PickUp();
            
            _carriedObject = carriable.gameObject;
            _carrying = true;
        }
    }
}
