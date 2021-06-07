using System;
using JnA.Core.ScriptableObjects;
using UnityEngine;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraEvent", menuName = "ScriptableObjects/Camera/CameraEvent", order = 1)]
    public class VCamEvent : ScriptableObject
    {
        public Action<VCamData> ChangeData;

        public Action RevertData;

        public Action<float, float> Screenshake;

        public Action StopScreenshake;

        public Action<Transform> Follow;

        public Action RevertFollow;

        public Action AddPixelPerfect;

        public Action RemovePixelPerfect;

        public event Action<VCamData> OnDataChanged;

        public Action<PolygonCollider2D> ChangeConfiner;

        public void RaiseDataChanged(VCamData d)
        {
            OnDataChanged?.Invoke(d);
        }
    }
}