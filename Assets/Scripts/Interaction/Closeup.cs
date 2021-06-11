using JnA.Core.ScriptableObjects;
using Kataru;
using NaughtyAttributes;
using UnityEngine;

namespace JnA.Interaction
{
    public class Closeup : CloseupBase
    {
        [Header("Optional")]
        [InfoBox("If not null, kataruInteract.OnInteract() will get called when this closeup gets shown", EInfoBoxType.Normal)]
        [SerializeField] protected KataruInteractable kataruInteract;

        override public void ShowCloseup()
        {
            base.ShowCloseup();

            // this must be called after everything else in case OnShowCloseup raised important listeners
            if (kataruInteract != null) { kataruInteract.OnInteract(); }
        }

        override protected void Cleanup()
        {
            base.Cleanup();
            kataruInteract.ForceExitInteraction();
        }
    }
}