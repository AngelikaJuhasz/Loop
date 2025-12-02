using UnityEngine;
using UnityEngine.Events;

namespace Prototype
{
    public sealed class Interactable : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private PlayerData _requiredPlayer;
        [SerializeField] private EquipmentSO _requiredEquipment;
        
        [Header("References")]
        [SerializeField] private GameObject _canvas;
        
        [Header("Events")]
        public UnityEvent OnInteract;
        
        public GameObject PlayerGO => _cameraTransform?.parent.gameObject; // horrendous bubblegum
        
        private Transform _cameraTransform;
        private bool _interactable;
        
        private void OnEnable() => PlayerMotor.OnInteract += Interact;
        
        private void OnDisable() => PlayerMotor.OnInteract -= Interact;
        
        private void Update()
        {
            if (!_cameraTransform) return;
            
            transform.LookAt(_cameraTransform);
            transform.Rotate(0f, 180f, 0f);
        }
        
        // due to the physics collision matrix, we don't need a player check as this only works with Player layer
        private void OnTriggerEnter(Collider other)
        {
            if (!_cameraTransform) _cameraTransform = Camera.main?.transform; // TODO: fix this bubblegum
            
            _canvas.SetActive(true);
            _interactable = true;
        }
        
        // due to the physics collision matrix, we don't need a player check as this only works with Player layer
        private void OnTriggerExit(Collider other)
        {
            _canvas.SetActive(false);
            _interactable = false;
        }

        private void Interact()
        {
            // check if player is in range
            if (!_interactable) return;
            
            // check if player is the same as required
            if (_requiredPlayer && PlayerGO && _requiredPlayer.Id != PlayerGO.GetComponent<PlayerData>().Id)
            {
                Debug.LogWarning($"Incorrect player ID!\nExpected: {_requiredPlayer.Id}");
                return;
            }
            
            // check if the equipment are as required
            // TODO: make into a list of required equipment later
            if (_requiredEquipment && PlayerGO && !PlayerGO.GetComponent<PlayerData>().CheckEquipment(_requiredEquipment))
            {
                Debug.LogWarning($"Cannot interact without the following equipment:\n{_requiredEquipment.name}!");
                return;
            }
            
            OnInteract?.Invoke();
        }

        public void Victory()
        {
            Debug.Log("You win!");
        }
        
        public void Pointless()
        {
            Debug.Log("You successfully interacted with the white box and it did... nothing!");
        }
    }
}
