using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Prototype
{
    public sealed class PlayerData : MonoBehaviour
    {
        [SerializeField] private string id;
        public string Id => id;
        [SerializeField] private List<EquipmentSO> _equipment;
        
        [SerializeField] private TMP_Text _errorText;

        public void AddEquipment(EquipmentSO equipment) => _equipment.Add(equipment);

        public void RemoveEquipment(EquipmentSO equipment) => _equipment.Remove(equipment);
        
        public bool CheckEquipment(EquipmentSO equipment) => _equipment.Contains(equipment);
        
        // ChatGPT bubblegum to get the demo done in time:
        Coroutine _showCo;

        public void Show(string message, float seconds)
        {
            if (_showCo != null) StopCoroutine(_showCo);
            _showCo = StartCoroutine(ShowCo(message, seconds));
        }

        IEnumerator ShowCo(string msg, float t)
        {
            if (_errorText) _errorText.text = msg;
            if (_errorText) _errorText.gameObject.SetActive(true);
            yield return new WaitForSeconds(t);
            if (_errorText) _errorText.gameObject.SetActive(false);
            _showCo = null;
        }
    }
}
