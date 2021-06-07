using System;
using JnA.Core;
using UnityEngine;

namespace JnA.UI.Settings
{
    [CreateAssetMenu(fileName = "SettingsEvent", menuName = "ScriptableObjects/UI/Settings/SettingsEvent", order = 1)]
    public class SettingsEvent : ScriptableObject
    {
        public Action<Action> OnOpenSettings;
        public Action<CanvasGroupSelect, Action> OnDeleteSave;

        public event Action<bool> OnSetPostProcess;
        public event Action<float> OnSetTextSpeed;
        public Func<float> GetTextSpeed;

        public void RaisePostProcess(bool b)
        {
            OnSetPostProcess?.Invoke(b);
        }

        public void RaiseTextSpeed(float speed)
        {
            OnSetTextSpeed?.Invoke(speed);
        }
    }
}