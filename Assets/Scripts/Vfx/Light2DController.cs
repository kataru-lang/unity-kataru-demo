using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace JnA.Vfx
{
    [RequireComponent(typeof(Light2D))]
    public class Light2DController : MonoBehaviour
    {
        [SerializeField] Light2DEvent light2DEvent;
        [SerializeField] new Light2D light;

        Color origColor;
        int tweenId;

        int tweenIdPulse;

        private void OnValidate()
        {
            light = GetComponent<Light2D>();
        }

        private void OnEnable()
        {
            light2DEvent.RevertGlobalLight += RevertGlobalLight;
            light2DEvent.SetGlobalLight += SetGlobalLight;
            origColor = light.color;
        }

        private void OnDisable()
        {
            light2DEvent.RevertGlobalLight -= RevertGlobalLight;
            light2DEvent.SetGlobalLight -= SetGlobalLight;
            DOTween.Kill(tweenIdPulse);
        }

        void SetGlobalLight(Color targetColor, float duration)
        {
            DOTween.Kill(tweenId);
            tweenId = DOTween.To(() => light.color, (x) => light.color = x, targetColor, duration).intId;
        }

        void RevertGlobalLight(float duration)
        {
            SetGlobalLight(origColor, duration);
        }

        void Pulse()
        {
            // tweenIdPulse = DOTween.To(() => light.color, (c) => light.color = c, Color.red, 0.3f).id; // yoyo
        }
    }
}