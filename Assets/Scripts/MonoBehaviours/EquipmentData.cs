using UnityEngine;

namespace Prototype
{
    // Should probably just inherit Interactable or conform to an interface with it or something
    public sealed class EquipmentData : MonoBehaviour
    {
        [SerializeField] private EquipmentSO _equipmentSO;
        [SerializeField] private Interactable _interactable;

        private void OnEnable() => _interactable.OnInteract.AddListener(GrantEquipment);

        private void OnDisable() => _interactable.OnInteract.RemoveListener(GrantEquipment);

        private void GrantEquipment()
        {
            PlayerData playerData = _interactable.PlayerGO?.GetComponent<PlayerData>();

            if (!playerData) return;
            
            if (!playerData.CheckEquipment(_equipmentSO)) playerData.AddEquipment(_equipmentSO);
            
            InventoryManager.Instance?.Grant(playerData?.Id, _equipmentSO);
            Destroy(_interactable.gameObject);
        }
    }
}
