using UnityEngine;
using JnA.Core;
using JnA.Utils;

namespace Kataru
{
    /// <summary>
    /// Triggers interact on trigger enter.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class KataruTrigger : KataruInteractable
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
                base.OnInteract();
        }
    }
}
