using JnA.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace JnA.UI
{
    public class SplashScreen : MonoBehaviour
    {
        /// <summary>
        /// How long should the splash screen be there?
        /// </summary>
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] UnityEvent OnDone;

        private void Awake()
        {
            canvasGroup.alpha = 1;
            canvasGroup.FadeOut(1f, 2.5f, 0, () =>
            {
                OnDone?.Invoke();
                Destroy(gameObject);
            }, ease: DG.Tweening.Ease.OutQuint);
        }
    }
}