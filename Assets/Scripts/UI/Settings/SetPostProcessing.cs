using UnityEngine.UI;
using UnityEngine;
using System;

namespace JnA.UI.Settings
{
    public class SetPostProcessing : MonoBehaviour, ISettingsSetter
    {
        const string KEY = "PostProcessing";
        const int ppDefault = 1;
        public Toggle toggle;

        [SerializeField] SettingsEvent settingsEvent;

        private void Awake()
        {
            Prepare();
        }

        private void Start()
        {
            Load();
        }

        public void Prepare()
        {
            // set listener
            toggle.onValueChanged.AddListener(PostProcessing);
        }

        public void Load()
        {
            float pp = PlayerPrefs.GetInt(KEY, ppDefault);
            toggle.isOn = pp == 1;
        }

        public void Sync()
        {
            float pp = PlayerPrefs.GetInt(KEY, ppDefault);
            toggle.SetIsOnWithoutNotify(pp == 1);
        }

        private void PostProcessing(bool on)
        {
            PlayerPrefs.SetInt(KEY, on ? 1 : 0);
            settingsEvent.RaisePostProcess(on);
        }
    }
}