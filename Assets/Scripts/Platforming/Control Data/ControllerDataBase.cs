using UnityEngine;

namespace JnA.Platformer
{
    [CreateAssetMenu(fileName = "ControllerDataBase", menuName = "ScriptableObjects/Platformer/ControllerDataBase", order = 1)]
    public class ControllerDataBase : ScriptableObject
    {
        [Header("Walk Params")]
        public float walkForce = 40;
        public float maxWalkVelocity = 6;

        [Header("Run Params")]
        public float runForce = 60;
        public float maxRunVelocity = 10;
    }
}