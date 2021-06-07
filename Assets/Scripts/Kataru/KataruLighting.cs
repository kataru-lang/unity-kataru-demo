using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
    }
}
