using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu(fileName = Paths.SOs.PartyOrder.FileName, menuName = Paths.SOs.PartyOrder.MenuName)]
    public class PartyOrderSO : ScriptableObject
    {
        [SerializeField] private List<GameObject> _characters = new();
        public List<GameObject> Characters => _characters;
        
        public void SetOrder(List<GameObject> order)
        {
            _characters.Clear();
            _characters.AddRange(order);
        }
    }
}
