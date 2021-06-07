using UnityEngine;
using DG.Tweening;
using System;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "VfxEvent", menuName = "ScriptableObjects/Vfx/VfxEvent", order = 1)]
    public class VfxEvent : ScriptableObject
    {
        public Action<float, Ease> FadeInUIBloom; // duration, ease

        public Action<float, float, Ease> LerpLUTContribution; // target, duration, ease

        public Action<float, float, float, Ease> LerpVignette; // targetIntensity, targetSmoothness, duration, ease
    }
}