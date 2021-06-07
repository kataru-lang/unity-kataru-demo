using System;
using UnityEngine;

namespace JnA.Core.Interaction
{
    [CreateAssetMenu(fileName = "InteractEvent", menuName = "ScriptableObjects/Platformer/InteractEvent", order = 1)]
    public class InteractDB : ScriptableObject
    {
        public Action HideInteractIcon;
        public Action<Transform, IconType> ShowInteractIcon;
        public Sprite look, talk;
    }
}