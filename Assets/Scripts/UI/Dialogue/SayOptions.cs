using JnA.Core.ScriptableObjects;
using JnA.Utils;
using UnityEngine;

namespace JnA.UI.Dialogue
{
    public class SayOptions : Options
    {
        [Header("Say-specific")]
        [SerializeField] Core.ScriptableObjects.VCamEvent vCamEvent;

        protected override void Awake()
        {
            base.Awake();
            vCamEvent.OnDataChanged += OnVCamDataChanged;
        }

        private void OnDestroy()
        {
            vCamEvent.OnDataChanged -= OnVCamDataChanged;
        }

        protected override void ShowOptions(Kataru.Choices choices, Transform target)
        {
            base.ShowOptions(choices, target);
            transform.position = target.position;
        }
    }
}