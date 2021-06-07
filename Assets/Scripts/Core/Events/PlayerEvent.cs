using System;
using JnA.Core.Interaction;
using UnityEngine;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerEvent", menuName = "ScriptableObjects/Platformer/PlayerEvent", order = 1)]
    public class PlayerEvent : ScriptableObject
    {
        public Action<Interactable> SetInteract;

        public Action<Interactable> TryExitInteract;
    }
}