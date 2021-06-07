using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace JnA.UI.Settings
{
    public class SetVolume : MonoBehaviour, ISettingsSetter
    {
        const string SFX_KEY = "SfxVol", MUSIC_KEY = "MusicVol";
        public float sfxDefault = 1, musicDefault = 0.92f;
        public AudioMixer mixer;
        public Slider soundSlider, musicSlider;

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
            // add value listener
            soundSlider.onValueChanged.AddListener(SfxVol);
            musicSlider.onValueChanged.AddListener(MusicVol);
        }

        public void Load()
        {
            Set(true);
        }

        public void Sync()
        {
            Set(false);
        }

        void Set(bool notify)
        {
            float sfx = PlayerPrefs.GetFloat(SFX_KEY, -1), music = PlayerPrefs.GetFloat(MUSIC_KEY, -1);
            if (sfx == -1)
            {
                sfx = 1;
            }
            if (music == -1)
            {
                music = 0.92f;
            }
            // have to manually call the methods because they don't get triggered here for some reason
            soundSlider.value = sfx;
            if (notify)
                SfxVol(sfx);
            musicSlider.value = music;
            if (notify)
                MusicVol(music);
        }

        float Log(float s)
        {
            return Mathf.Log(s) * 20;
        }

        private void SfxVol(float s)
        {
            PlayerPrefs.SetFloat(SFX_KEY, s);
            mixer.SetFloat(SFX_KEY, Log(s));

        }

        private void MusicVol(float s)
        {
            PlayerPrefs.SetFloat(MUSIC_KEY, s);
            mixer.SetFloat(MUSIC_KEY, Log(s));
        }
    }
}