using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public sealed class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        
        private readonly Dictionary<string, HashSet<EquipmentSO>> _inventories = new();

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep inventory data across scenes
        }

        public void Grant(string characterId, EquipmentSO equipment) => GetEquipmentSet(characterId).Add(equipment);

        public bool Has(string characterId, EquipmentSO equipment) => equipment && GetEquipmentSet(characterId).Contains(equipment);
        
        public void ApplyTo(GameObject character)
        {
            if (!character) return;
            
            PlayerData playerData = character.GetComponent<PlayerData>();
            
            if (!playerData) return;

            HashSet<EquipmentSO> set = GetEquipmentSet(playerData.Id);

            foreach (EquipmentSO eq in set)
            {
                if (!playerData.CheckEquipment(eq)) playerData.AddEquipment(eq);
            }
        }

        private HashSet<EquipmentSO> GetEquipmentSet(string id)
        {
            if (_inventories.TryGetValue(id, out HashSet<EquipmentSO> set)) return set;
            
            set = new HashSet<EquipmentSO>();
            _inventories[id] = set;
            return set;
        }
    }
}
