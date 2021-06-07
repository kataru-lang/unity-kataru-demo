using UnityEngine;
using JnA.Core;

namespace Kataru
{
    /// <summary>
    /// Trigger Kataru passage on collision enter
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class KataruCollider : KataruInteractable
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == Main.PLAYER_LAYER)
                base.OnInteract();
        }
    }
}