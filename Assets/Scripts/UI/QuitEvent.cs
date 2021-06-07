using System;
using UnityEngine;

namespace JnA.UI
{
    [CreateAssetMenu(fileName = "QuitEvent", menuName = "ScriptableObjects/UI/QuitEvent", order = 1)]
    public class QuitEvent : ScriptableObject
    {
        public event Action<bool> OnSaveAndQuit;

        public void RaiseEvent(bool save)
        {
            OnSaveAndQuit?.Invoke(save);
        }
    }
}