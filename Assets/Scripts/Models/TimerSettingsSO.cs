using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu(menuName = Paths.ScriptableObjects.TimerSettings.MenuName, fileName = Paths.ScriptableObjects.TimerSettings.FileName)]
    public sealed class SwapTimerSettingsSO : ScriptableObject
    {
        public float durationSeconds = 10f;
        public bool autoRestart = true; // restart timer when it hits 0
        public bool useUnscaledTime = false; // if timer should move when paused
    }
}
