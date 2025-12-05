using System;
using TMPro;
using UnityEngine;

namespace Prototype
{
    public sealed class Timer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SwapTimerSettingsSO settings;

        [Header("UI")]
        [SerializeField] private TMP_Text label;

        public event Action<Timer> Elapsed;

        private float _timeRemaining;
        private bool _isRunning;

        private void OnEnable()
        {
            PlayerMotor.OnSpecial += ElapseTimer;
            ResetAndStart();
        }
        
        private void OnDisable() => PlayerMotor.OnSpecial -= ElapseTimer;

        private void Update()
        {
            if (!_isRunning) return;

            _timeRemaining -= settings && settings.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            
            if (_timeRemaining <= 0f)
            {
                _timeRemaining = 0f;
                UpdateLabel();
                _isRunning = false;
                Elapsed?.Invoke(this);

                if (settings && settings.autoRestart) ResetAndStart();
                return;
            }

            UpdateLabel();
        }

        private void ElapseTimer()
        {
            ResetAndStart();
            Elapsed?.Invoke(this);
        }
        
        private void UpdateLabel()
        {
            if (!label) return;
            float display = settings ? Mathf.Clamp(_timeRemaining, 0f, settings.durationSeconds) : _timeRemaining;
            label.text = display.ToString("F2");
        }

        public void ResetAndStart()
        {
            _timeRemaining = settings ? settings.durationSeconds : 10f;
            _isRunning = true;
            
            UpdateLabel();
        }

        public void Stop() => _isRunning = false;
        
        public void Pause(bool pause) => _isRunning = !pause;
        
        public float TimeRemaining => _timeRemaining;
        
        public void SetLabel(TMP_Text text) => label = text;
        
        public void SetSettings(SwapTimerSettingsSO settingsSO) => settings = settingsSO;
    }
}
