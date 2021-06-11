using System;
using UnityEngine;

namespace JnA.Interaction
{
    /// <summary>
    /// Closeup that plays given animator once shown
    /// </summary>
    public class CloseupAnimation : CloseupBase
    {
        [SerializeField] Animator animator;

        private void Awake()
        {
            animator.enabled = false;
        }

        override public void ShowCloseup()
        {
            base.ShowCloseup();
            animator.enabled = true;
        }

        // expected to be used as an animation event
        public void HideCloseup()
        {
            HideCloseup();
        }

        override internal void HideCloseup(Action onComplete = null)
        {
            base.HideCloseup(() =>
            {
                animator.enabled = false;
                if (onComplete != null) onComplete();
            });

        }
    }
}