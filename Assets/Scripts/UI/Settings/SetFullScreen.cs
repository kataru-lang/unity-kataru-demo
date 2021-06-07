using UnityEngine.UI;
using UnityEngine;
using System;

namespace JnA.UI.Settings
{
    public class SetFullScreen : MonoBehaviour, ISettingsSetter
    {
        const string KEY = "FullScreen";
        const int fsDefault = 1;
        public Toggle toggle;

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
            toggle.onValueChanged.AddListener(FullScreen);
        }

        public void Load()
        {
            int fs = PlayerPrefs.GetInt(KEY, fsDefault);
            Screen.fullScreen = fs == 1;
        }

        public void Sync()
        {
            int fs = PlayerPrefs.GetInt(KEY, fsDefault);
            toggle.SetIsOnWithoutNotify(fs == 1);
        }

        private void FullScreen(bool on)
        {
            PlayerPrefs.SetInt(KEY, on ? 1 : 0);
        }
    }
}