using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InputEvent", menuName = "ScriptableObjects/Main/InputEvent", order = 1)]
    public class InputEvent : ScriptableObject
    {
        public Action<string, bool> SwitchActionMap;
        public Action<string, bool> EnableActionMap;
        public Action RevertToSceneMap;
        public Func<string> GetActionMap;
        public InputActionAsset input;
    }
}