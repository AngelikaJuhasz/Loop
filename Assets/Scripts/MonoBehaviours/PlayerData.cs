using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public sealed class PlayerData : MonoBehaviour
    {
        [SerializeField] private string id;
        public string Id => id;
        [SerializeField] private List<EquipmentSO> _equipment;

        public void AddEquipment(EquipmentSO equipment) => _equipment.Add(equipment);

        public void RemoveEquipment(EquipmentSO equipment) => _equipment.Remove(equipment);
        
        public bool CheckEquipment(EquipmentSO equipment) => _equipment.Contains(equipment);
    }
}
