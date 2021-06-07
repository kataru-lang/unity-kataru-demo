using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JnA.Core;
using System;
using JnA.UI.Settings;
using Kataru;

namespace JnA.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ConfirmationModal : CanvasGroupSelect
    {
        [SerializeField]
        TextMeshProUGUI text;

        [SerializeField]
        Button yesButton;

        [SerializeField]
        Button noButton;

        [SerializeField] SettingsEvent settingsEvent;

        [SerializeField] QuitEvent quitEvent;

        private System.Action onConfirm, onDeny;

        private void OnValidate()
        {
            text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            yesButton = transform.Find("Yes").GetComponent<Button>();
            noButton = transform.Find("No").GetComponent<Button>();
        }

        override protected void Awake()
        {
            base.Awake();
            settingsEvent.OnDeleteSave += ViewConfirmDelete;
        }

        public void ViewConfirmDelete(CanvasGroupSelect canvasGroup, Action onDelete)
        {
            Action close =
            () =>
            {
                HideModal();
                canvasGroup.FadeIn();
            };
            Init(
                "Delete all saved data?", () =>
            {
                onDelete();
                close();
            }, close);
        }

        void HideModal()
        {
            FadeOut(0.3f);
        }

        public void Init(string message, System.Action onConfirm, System.Action onDeny)
        {
            text.text = message;
            this.onConfirm = onConfirm;
            this.onDeny = onDeny;
            FadeIn(0.3f);
        }

        public void OnConfirm()
        {
            onConfirm?.Invoke();
        }

        public void OnReject()
        {
            onDeny?.Invoke();
        }
    }
}