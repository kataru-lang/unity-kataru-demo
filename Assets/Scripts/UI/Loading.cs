using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using JnA.Utils;
using JnA.Core.ScriptableObjects;
using UnityEngine.UI;
using System;
using Kataru;

namespace JnA.UI
{
    public class Loading : Handler
    {
        [SerializeField] QuitEvent quitEvent;
        [SerializeField] LoadEvent loadEvent;

        [Header("UI")]
        [SerializeField] CanvasGroup group;
        [SerializeField] Image bg;
        [SerializeField] GameObject art;

        private void Start()
        {
            quitEvent.OnSaveAndQuit += OnSaveAndQuit;
            loadEvent.OnLoad += ShowLoading;
            loadEvent.OnDoneLoading += HideLoading;
        }

        private void HideLoading()
        {
            group.FadeOut(0.5f);
        }

        private void ShowLoading(bool showArt, Color color, Action onComplete)
        {
            bg.color = color;
            art.SetActive(showArt);
            group.FadeIn(0.3f).OnComplete(() => onComplete());
        }

        [Kataru.CommandHandler(autoNext: false)]
        private void FadeInScreen(string color, double duration, bool wait)
        {
            bg.color = Functions.StringToColor(color);
            art.SetActive(false);
            if (wait)
            {
                group.FadeIn((float)duration, onComplete: () => Runner.Next());
            }
            else
            {
                group.FadeIn((float)duration);
                Runner.Next();
            }
        }

        [Kataru.CommandHandler]
        private void FadeOutScreen(double duration, bool wait)
        {
            if (wait)
            {
                group.FadeOut((float)duration, onComplete: () => Runner.Next());
            }
            else
            {
                group.FadeOut((float)duration);
                Runner.Next();
            }
        }

        void OnSaveAndQuit(bool save)
        {
            if (save)
            {
                Runner.Save();
                ShowLoading(true, Color.black, Application.Quit);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}