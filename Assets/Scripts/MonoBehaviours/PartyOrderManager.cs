using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class PartyOrderManager : MonoBehaviour
    {
        [SerializeField] private GameObject characterRow;

        public List<GameObject> GetCharactersInOrder()
        {
            int count = characterRow.transform.childCount;
            List<GameObject> result = new(count);

            for (int i = 0; i < count; i++)
            {
                Transform child = characterRow.transform.GetChild(i);
                ReorderableItem item = child.GetComponent<ReorderableItem>();

                result.Add(item.Character);
            }

            return result;
        }
    }
}
