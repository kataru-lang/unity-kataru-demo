using UnityEngine;
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace JnA.Utils
{
    //It is common to create a class to contain all of your
    //extension methods. This class must be static.
    public static class Extensions
    {

        #region TIME
        public static string TimeToString(this DateTime time) { return time.ToString("h:mmtt"); }

        public static string ShortDateTimeToString(this DateTime time) { return time.ToString("MMM d h:mmtt"); } // Apr 4 4:24PM

        public static string DayToString(this DateTime time) { return time.ToString("dddd"); }
        #endregion

        #region C#
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            System.Random rnd = new System.Random();
            return source.OrderBy((item) => rnd.Next());
        }
        #endregion

        #region MATH
        public static bool IsParsable<T>(this string value) where T : struct
        {
            return Enum.TryParse<T>(value, true, out _);
        }

        public static float Distance(float a, float b)
        {
            return Mathf.Abs(a - b);
        }
        #endregion

        #region TRANSFORM
        public static void SetChildrenActive(this Transform transform, bool isActive)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(isActive);
        }
        #endregion

        #region TRANSITIONS
        public static bool IsOn(this CanvasGroup group)
        {
            return group.alpha == 1 && group.interactable == true && group.blocksRaycasts == true;
        }


        public static void TurnOn(this CanvasGroup group)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public static void TurnOff(this CanvasGroup group)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        public static Tween FadeOut(this CanvasGroup group,
        float duration = 0.3f,
        float delay = 0,
        float target = 0,
        Action onComplete = null,
        bool ignoreTimeScale = true,
        Ease ease = Ease.OutCubic)
        {
            DOTween.Kill(group.GetInstanceID());
            return group.DOFade(target, duration)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                if (target == 0)
                    group.TurnOff();
                if (onComplete != null)
                    onComplete();
            }).SetId(group.GetInstanceID())
            .SetEase(ease)
            .SetUpdate(ignoreTimeScale) // ignore time scale
            .Play();
        }

        public static Tween FadeIn(
            this CanvasGroup group,
            float duration = 0.3f,
            float delay = 0,
            Action onComplete = null,
            bool ignoreTimeScale = true,
            Ease ease = Ease.OutCubic
            )
        {
            DOTween.Kill(group.GetInstanceID());
            return group.DOFade(1, duration).OnComplete(() =>
           {
               group.TurnOn();
               if (onComplete != null)
                   onComplete();
           }).SetId(group.GetInstanceID())
           .SetEase(ease)
            .SetDelay(delay)
           .SetUpdate(ignoreTimeScale) // ignore time scale
           .Play();
        }

        public static Tween FadeIn(
           this Graphic graphic,
           float duration = 0.3f,
           float delay = 0,
           Action onComplete = null,
           bool ignoreTimeScale = true,
           Ease ease = Ease.OutCubic
           )
        {
            DOTween.Kill(graphic.GetInstanceID());
            return graphic.DOFade(1, duration).OnComplete(() =>
           {
               if (onComplete != null)
                   onComplete();
           }).SetId(graphic.GetInstanceID())
           .SetEase(ease)
            .SetDelay(delay)
           .SetUpdate(ignoreTimeScale) // ignore time scale
           .Play();
        }

        public static void ScaleIn(this RectTransform r, Ease ease, Action onComplete = null)
        {
            r.localScale = Vector3.zero;
            r.DOScale(Vector3.one, 0.3f)
            .SetId(r.gameObject.GetInstanceID())
            .SetEase(ease)
            .SetUpdate(true) // ignore time scale
            .OnComplete(() => { if (onComplete != null) onComplete(); });
        }


        public static void ScaleOut(this RectTransform r, Ease ease, Action onComplete = null)
        {
            r.DOScale(Vector3.zero, 0.3f)
              .SetId(r.gameObject.GetInstanceID())
              .SetEase(ease)
              .SetUpdate(true) // ignore time scale
              .OnComplete(() => { if (onComplete != null) onComplete(); });
        }
        #endregion
    }
}