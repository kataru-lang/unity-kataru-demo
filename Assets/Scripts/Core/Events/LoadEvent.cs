using System;
using UnityEngine;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LoadEvent", menuName = "ScriptableObjects/Main/LoadEvent", order = 1)]
    public class LoadEvent : ScriptableObject
    {
        public Action<bool, Color, Action> OnLoad;
        public Action OnDoneLoading;
    }
}