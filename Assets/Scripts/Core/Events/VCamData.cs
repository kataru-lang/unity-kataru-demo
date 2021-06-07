using UnityEngine;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Camera/CameraData", order = 1)]
    public class VCamData : ScriptableObject
    {
        public float distance;
        public float screenY;
    }
}