using UnityEngine.UI;
using UnityEngine;
using System;

namespace JnA.UI.Settings
{
    public class SetTextSpeed : MonoBehaviour, ISettingsSetter
    {
        const string KEY = "TextSpeed";
        public const float TEXT_TIMESCALE_DEFAULT = 1;
        public Slider slider;

        [SerializeField] SettingsEvent settingsEvent;

        private void Awake()
        {
            Prepare();
            settingsEvent.GetTextSpeed += GetTextSpeed;
        }

        private void Start()
        {
            Load();
        }

        public void Prepare()
        {
            // set listener
            slider.onValueChanged.AddListener(textSpeed);
        }

        public void Load()
        {
            float speed = PlayerPrefs.GetFloat(KEY, TEXT_TIMESCALE_DEFAULT);
            // set value
            slider.value = speed;
        }

        public void Sync()
        {
            float speed = PlayerPrefs.GetFloat(KEY, TEXT_TIMESCALE_DEFAULT);
            // set value
            slider.SetValueWithoutNotify(speed);
        }

        private void textSpeed(float value)
        {
            PlayerPrefs.SetFloat(KEY, value);
            settingsEvent.RaiseTextSpeed(value);
        }

        private float GetTextSpeed() => slider.value;
    }
}