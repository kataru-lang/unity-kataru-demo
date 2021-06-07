using System;
using JnA.Core;
using JnA.UI;
using JnA.Utils;
using NaughtyAttributes;
using UnityEngine;

namespace JnA.UI.Settings
{
    public class Settings : CanvasGroupSelect
    {
        [SerializeField] SettingsEvent settings;

        Action onClose;

        override protected void Awake()
        {
            base.Awake();
            group.TurnOff();
            ListenOpenSettings();
        }

        private void OnDestroy()
        {
            StopListenOpenSettings();
        }

        public void StopListenOpenSettings() => settings.OnOpenSettings -= OnOpenSettings;

        public void ListenOpenSettings() => settings.OnOpenSettings += OnOpenSettings;

        public void SyncSettings()
        {
            ISettingsSetter[] setters = gameObject.GetComponentsInChildren<ISettingsSetter>();
            foreach (var setter in setters)
            {
                setter.Sync();
            }
        }

        void OnOpenSettings(Action onCancel)
        {
            onClose = onCancel;
            FadeIn(0.2f);
        }

        public void OnCloseSettings()
        {
            onClose?.Invoke();
            FadeOut(0.2f);
        }
    }
}