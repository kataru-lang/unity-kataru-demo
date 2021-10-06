using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Kataru
{
    /// <summary>
    /// Supports listening to commands for enabling and disabling lights.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    class KataruLight : Handler
    {
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string character = Characters.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);

        protected override string Name
        {
            get => character;
        }

        Light2D light2d;


        private void Awake()
        {
            light2d = GetComponent<Light2D>();
        }

        [CommandHandler(character: true)]
        void SetLighting(double intensity)
        {
            light2d.intensity = (float)intensity;
        }

        [CommandHandler(character: true)]
        void LerpLighting(double intensity, double duration)
        {
            DOTween.To(() => light2d.intensity, (x) => light2d.intensity = x, (float)intensity, (float)duration);
        }
    }
}
