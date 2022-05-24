using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace JnA.UI.Settings
{
    public class SetQuality : MonoBehaviour, ISettingsSetter
    {
        const string RES_KEY = "Resolution", Q_KEY = "Quality";

        public TextMeshProUGUI resolution;
        public Button decreaseR, increaseR;

        public TextMeshProUGUI quality;

        public Button decreaseQ, increaseQ;
        Resolution[] resolutions;
        string[] qualities;

        private int currentResolution = 0,
            currentQuality = 0;


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
            qualities = QualitySettings.names;
            decreaseR.onClick.AddListener(DecreaseRes);
            increaseR.onClick.AddListener(IncreaseRes);
            increaseQ.onClick.AddListener(IncreaseQuality);
            decreaseQ.onClick.AddListener(DecreaseQuality);
        }

        public void Load()
        {
            resolutions = Screen.resolutions;
            currentQuality = PlayerPrefs.GetInt(Q_KEY, QualitySettings.GetQualityLevel());
            currentResolution = PlayerPrefs.GetInt(RES_KEY, Array.FindIndex(resolutions,
             r => r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height));

            ApplyQuality();
            ApplyRes();
        }

        public void Sync()
        {
            Load();
        }

        public void IncreaseQuality()
        {
            currentQuality++;
            ApplyQuality();
        }


        public void DecreaseQuality()
        {
            if (currentQuality == 0) return;
            currentQuality--;
            ApplyQuality();
        }

        private void ApplyQuality()
        {
            SetQButtons();
            quality.text = qualities[currentQuality];
            PlayerPrefs.SetInt(Q_KEY, currentQuality);
            QualitySettings.SetQualityLevel(currentQuality);
        }

        private void SetQButtons()
        {
            increaseQ.interactable = currentQuality < qualities.Length - 1;
            decreaseQ.interactable = currentQuality != 0;
        }

        public void IncreaseRes()
        {
            if (currentResolution >= resolutions.Length - 1) return;
            currentResolution++;
            ApplyRes();
        }

        public void DecreaseRes()
        {
            if (currentResolution <= 0) return;
            currentResolution--;
            ApplyRes();
        }

        private void ApplyRes()
        {
            SetResButtons();
            int clampedCurrent = Mathf.Clamp(currentResolution, 0, resolutions.Length - 1);
            resolution.text = ResToString(resolutions[clampedCurrent]);
            PlayerPrefs.SetInt(RES_KEY, clampedCurrent);
            Screen.SetResolution(resolutions[clampedCurrent].width, resolutions[clampedCurrent].height, true);
        }


        private void SetResButtons()
        {
            increaseR.interactable = currentResolution < resolutions.Length - 1;
            decreaseR.interactable = currentResolution != 0;
        }

        string ResToString(Resolution res)
        {
            return res.width + " x " + res.height;
        }
    }
}