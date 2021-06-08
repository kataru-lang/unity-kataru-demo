using UnityEngine;

namespace JnA.Platformer
{
    [CreateAssetMenu(fileName = "ControllerDataBase", menuName = "ScriptableObjects/Platformer/ControllerDataBase", order = 1)]
    public class ControllerDataBase : ScriptableObject
    {
        [Header("Walk Params")]
        public float walkVelocity = 10;

        [Header("Run Params")]
        public float runVelocity = 20;
    }
}