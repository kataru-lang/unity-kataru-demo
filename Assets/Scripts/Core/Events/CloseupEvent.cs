using System;
using UnityEngine;
using UnityEngine.Events;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CloseupEvent", menuName = "ScriptableObjects/Interaction/Closeups/CloseupEvent", order = 1)]
    public class CloseupEvent : ScriptableObject
    {
        public event Action<string> OnShowCloseup;

        public event Action<string> OnHideCloseup;

        public void RaiseShowCloseup(string goName)
        {
            OnShowCloseup?.Invoke(goName);
        }

        public void RaiseHideCloseup(string goName)
        {
            OnHideCloseup?.Invoke(goName);
        }
    }
}