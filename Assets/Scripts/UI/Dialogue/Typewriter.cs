using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using JnA.UI.Settings;

namespace JnA.UI.Dialogue
{
    public class Typewriter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] UnityEvent<char> OnChar;
        [SerializeField] protected SettingsEvent settingsEvent;

        [SerializeField] float secondsPerChar = 0.05f;

        Tween typeTween;
        float timeScale = 1;
        const float SKIP_SPEED_ADD = 1.5f;

        protected void OnEnable()
        {
            settingsEvent.OnSetTextSpeed += SetSpeed;
        }

        protected void OnDisable()
        {
            settingsEvent.OnSetTextSpeed -= SetSpeed;
        }


        // listener to textdialogue's OnShowLine event
        public void ShowText(string line)
        {
            ResetSpeed();
            text.text = line;
            text.ForceMeshUpdate();
            text.maxVisibleCharacters = 0;
            float duration = text.textInfo.characterCount * secondsPerChar;
            Debug.Log($"Line: '{line}' CharacterCount: '{text.textInfo.characterCount}' duration '{duration}'");
            typeTween = DOTween.To(() => text.maxVisibleCharacters, (x) =>
            {
                Debug.Log("hi update");
                text.maxVisibleCharacters = x;
                OnChar?.Invoke(text.text[x]);
            }, text.textInfo.characterCount, duration);
        }

        void SetSpeed(float speed)
        {
            timeScale = speed;
        }

        public void ResetSpeed() => SetSpeed(settingsEvent.GetTextSpeed());

        public void FastForwardSpeed()
        {
            if (typeTween != null)
                typeTween.timeScale = settingsEvent.GetTextSpeed() + SKIP_SPEED_ADD;
        }

        public bool IsTyping() => typeTween != null && typeTween.IsPlaying();

        public void HideText()
        {
            text.color = Color.clear;
        }

        public bool IsHidden() => text.color.a == 0;
    }
}