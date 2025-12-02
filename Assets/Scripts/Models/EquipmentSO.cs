using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu(fileName = Paths.ScriptableObjects.Equipment.FileName, menuName = Paths.ScriptableObjects.Equipment.MenuName)]
    public class EquipmentSO : ScriptableObject
    {
        // TODO: add unique functionality and fields for visuals etc.
        [SerializeField] private string id;
        public string Id => id;
        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;
    }
}
