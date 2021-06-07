using JnA.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace JnA.Vfx
{
    public class MainVfx : MonoBehaviour
    {
        [SerializeField] Volume main, ui;

        [Header("Events")]
        [SerializeField] SceneDB sceneDatabase;

        [SerializeField] VfxEvent vfxEvent;

        private void Awake()
        {
            sceneDatabase.OnLoaded += (old, scene) =>
            {
                ChangeVolume(scene.profile);
            };
            vfxEvent.FadeInUIBloom += FadeInUIBloom;
            vfxEvent.LerpLUTContribution += LerpLUTContribution;
            vfxEvent.LerpVignette += LerpVignette;
        }

        internal void ChangeVolume(VolumeProfile profile)
        {
            if (profile == null) return;
            main.profile = profile;
        }

        void FadeInUIBloom(float duration, Ease ease)
        {
            Bloom bloom;
            if (ui.profile.TryGet(out bloom))
            {
                float origVal = bloom.intensity.GetValue<float>();
                bloom.intensity.Override(0);
                DOTween.To(() => bloom.intensity.GetValue<float>(), x => bloom.intensity.Override(x), origVal, duration)
                .SetEase(ease);
            }
        }

        public void LerpLUTContribution(float target, float duration, Ease ease)
        {
            ColorLookup lookup;
            if (main.profile.TryGet(out lookup))
            {
                DOTween.To(() => lookup.contribution.GetValue<float>(), x => lookup.contribution.Override(x), target, duration)
                .SetEase(ease);
            }
        }

        void LerpVignette(float targetIntensity, float targetSmoothness, float duration, Ease ease)
        {
            Vignette vignette;
            if (main.profile.TryGet(out vignette))
            {
                DOTween.Sequence()
                    .Append(DOTween.To(() => vignette.intensity.GetValue<float>(), x => vignette.intensity.Override(x), targetIntensity, duration)
                        .SetEase(ease))
                    .Join(DOTween.To(() => vignette.smoothness.GetValue<float>(), x => vignette.smoothness.Override(x), targetSmoothness, duration)
                        .SetEase(ease));
            }
        }
    }
}