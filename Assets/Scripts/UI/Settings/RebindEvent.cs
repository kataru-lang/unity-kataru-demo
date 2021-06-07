using System;
using JnA.Core;
using UnityEngine;

namespace JnA.UI.Settings
{
    [CreateAssetMenu(fileName = "RebindEvent", menuName = "ScriptableObjects/UI/Settings/RebindEvent", order = 1)]
    public class RebindEvent : ScriptableObject
    {
        public Action<bool> OnShowWaitForInput;
    }
}