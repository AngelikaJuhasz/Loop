using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype
{
    public sealed class InputManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputManager _playerInputManager;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private List<GameObject> _playerPrefabs;

        private int _prefabIndex;
        
        private void Awake()
        {
            // TODO: move these elsewhere later, they are not part of managing inputs
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            _playerInputManager.playerPrefab = _playerPrefabs[_prefabIndex]; // TODO: set this to match selected order later
            _prefabIndex = (_prefabIndex + 1) % _playerPrefabs.Count; // ensure looping
        }
        
        private void Start()
        {
            if (PlayerInputManager.instance.playerCount != 0) return;
        
            PlayerInputManager.instance.JoinPlayer();
        }
        
        private void OnEnable()
        {
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
            _playerInputManager.onPlayerLeft += OnPlayerLeft;
        }
    
        private void OnDisable()
        {
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
            _playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            GameObject go = playerInput.gameObject;
            
            if (_spawnPoint) go.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            
            IInputReader reader = go.GetComponents<MonoBehaviour>().OfType<IInputReader>().FirstOrDefault()!;
            IPlayerMotor motor = go.GetComponents<MonoBehaviour>().OfType<IPlayerMotor>().FirstOrDefault()!;
        
            reader.Attach(playerInput);
            reader.Enable();
        
            reader.Move += motor.Move;
            reader.Look += motor.Look;
            reader.Jump += motor.Jump;
            reader.Interact += motor.Interact;
            
            // timer subscribe
            Timer timer = go.GetComponent<Timer>();

            if (!timer) return;
            
            timer.ResetAndStart(); // reset state of timer
            timer.Elapsed += OnTimerElapsed; // subscribe to timer
            
            // repopulate inventory
            InventoryManager.Instance?.ApplyTo(go);
        }
        
        private void OnPlayerLeft(PlayerInput playerInput)
        {
            IInputReader reader = playerInput.GetComponents<MonoBehaviour>().OfType<IInputReader>().FirstOrDefault();
            reader?.Dispose();
            
            // timer cleanup
            Timer timer = playerInput.GetComponent<Timer>();
            if (timer) timer.Elapsed -= OnTimerElapsed;
        }

        private void OnTimerElapsed(Timer timer)
        {
            PlayerInput playerInput = timer.GetComponent<PlayerInput>();
            
            if (!playerInput) return;

            timer.Elapsed -= OnTimerElapsed; // for safety
            playerInput.actions.Disable(); // for double safety
            
            Destroy(playerInput.gameObject); // destroy current character

            // set index to next character in sequence and loop is at max index
            _playerInputManager.playerPrefab = _playerPrefabs[_prefabIndex];
            _prefabIndex = (_prefabIndex + 1) % _playerPrefabs.Count;

            // spawn next pawn
            PlayerInputManager.instance.JoinPlayer();
        }
    }
}
