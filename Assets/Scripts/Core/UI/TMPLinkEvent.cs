using System;
using UnityEngine;

namespace JnA.UI
{
    [CreateAssetMenu(fileName = "TMPLinkEvent", menuName = "ScriptableObjects/UI/TMPLinkEvent", order = 1)]
    public class TMPLinkEvent : ScriptableObject
    {
        public Action<string> OnLinkClicked;
    }
}