using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype
{
    [CreateAssetMenu(fileName = Paths.ScriptableObjects.PlayerActions.FileName, menuName = Paths.ScriptableObjects.PlayerActions.MenuName)]
    public sealed class PlayerActionsSO : ScriptableObject
    {
        public List<PlayerAction> playerActions;

        public InputActionReference GetReference(InputActionOptions options)
        {
            return playerActions.First(entry => entry.inputActionOptions == options).inputAction;
        }
        
        [Serializable]
        public sealed class PlayerAction
        {
            public InputActionOptions inputActionOptions;
            public InputActionReference inputAction;
        }
    }
}
