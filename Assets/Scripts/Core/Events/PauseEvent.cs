using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PauseEvent", menuName = "ScriptableObjects/Main/PauseEvent", order = 1)]
    public class PauseEvent : ScriptableObject
    {
        public Action<bool> EnablePause;

        public event Action<bool> OnPause;

        public void RaiseOnPause(bool paused)
        {
            OnPause?.Invoke(paused);
        }
    }
}