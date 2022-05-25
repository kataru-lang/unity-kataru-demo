using System;
using DG.Tweening;
using JnA.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JnA.Core
{
    /// <summary>
    /// Class with CanvasGroup that automatically selects given button when faded in
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupSelect : MonoBehaviour
    {
        public Selectable autoSelect;
        [HideInInspector] public CanvasGroup group;

        [InfoBox("Set CanvasGroup properties to false and zero on Awake()", EInfoBoxType.Normal)]
        [SerializeField] bool offOnAwake = false;
        [SerializeField] bool deselectOnOut = false;
        [SerializeField] UnityEvent OnOpen, OnClose;

        virtual protected void Awake()
        {
            group = GetComponent<CanvasGroup>();
            if (offOnAwake)
            {
                group.TurnOff();
            }
            else
            {
                group.TurnOn();
                AutoSelect();
            }
        }

        public void AutoSelect()
        {
            if (autoSelect)
                EventSystem.current.SetSelectedGameObject(autoSelect.gameObject);
        }

        // button listeners
        public void FadeInListener() => FadeIn();

        public void FadeInListener(float duration) => FadeIn(duration);

        public void FadeOutListener() => FadeOut();

        // utils
        public Tween FadeIn(float duration = 0.3f, float delay = 0, bool ignoreTimeScale = true)
        {
            AutoSelect();
            return group.FadeIn(duration, delay: delay, ignoreTimeScale: ignoreTimeScale, onComplete: () => OnOpen?.Invoke());
        }

        public Tween FadeOut(float duration = 0.3f, Action onComplete = null)
        {
            if (deselectOnOut) EventSystem.current.SetSelectedGameObject(null);
            return group.FadeOut(duration, onComplete: () =>
            {
                OnClose?.Invoke();
                if (onComplete != null) onComplete();
            });
        }
    }
}