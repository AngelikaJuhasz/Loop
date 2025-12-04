using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] private string _nextSceneName;
        [SerializeField] private PartyOrderSO _partyOrderSO;
        [SerializeField] private PartyOrderManager _partyOrderManager;
        
        public void OnConfirmParty()
        {
            List<GameObject> order = _partyOrderManager.GetCharactersInOrder();
            _partyOrderSO.SetOrder(order);

            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
