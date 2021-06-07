using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
namespace Kataru
{
    /// <summary>
    /// Supports listening to commands for enabling and disabling lights.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    class KataruLighting : Handler
    {
        [SerializeField] Light2D[] light2ds;

        [CommandHandler]
        void SetLighting(double intensity)
        {
            foreach (var light2d in light2ds)
            {
                light2d.intensity = (float)intensity;
            }
        }

        [CommandHandler]
        void LerpLighting(double intensity, double duration)
        {
            float intensityF = (float)intensity;
            float durationF = (float)duration;
            foreach (var light2d in light2ds)
            {
                DOTween.To(() => light2d.intensity, (x) => light2d.intensity = x, intensityF, durationF);
            }
        }
    }
}
