using System;
using UnityEngine;

namespace JnA.Vfx
{
    [CreateAssetMenu(fileName = "Light2DEvent", menuName = "ScriptableObjects/Vfx/Light2DEvent", order = 1)]
    public class Light2DEvent : ScriptableObject
    {
        public Action<float> RevertGlobalLight;
        public Action<Color, float> SetGlobalLight;
    }
}