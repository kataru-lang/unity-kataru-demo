using UnityEngine;
using Cinemachine;
using JnA.Core.ScriptableObjects;
using System.Collections.Generic;
using NaughtyAttributes;

namespace JnA.Core
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class MainVCam : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        CinemachineFramingTransposer transposer;
        CinemachineConfiner confiner;
        [SerializeField] Core.ScriptableObjects.VCamEvent vCamEvent;
        [SerializeField] SceneDB sceneDatabase;
        [SerializeField] NoiseSettings noiseProfile;
        Stack<VCamData> pastData = new Stack<VCamData>();
        Stack<Transform> pastFollows = new Stack<Transform>();

        CinemachinePixelPerfect pixelPerfect;

        private void OnValidate()
        {
            virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        }

        private void Awake()
        {
            vCamEvent.ChangeData += ChangeData;
            vCamEvent.RevertData += RevertData;
            vCamEvent.Follow += Follow;
            vCamEvent.StopFollow += StopFollow;
            vCamEvent.Screenshake += Screenshake;
            vCamEvent.StopScreenshake += StopScreenshake;
            vCamEvent.ChangeConfiner += ChangeConfiner;
            vCamEvent.AddPixelPerfect += AddPixelPerfect;
            vCamEvent.RemovePixelPerfect += RemovePixelPerfect;
            sceneDatabase.OnLoaded += (old, scene) =>
            {
                SetSceneVCam(scene);
            };

            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        }

        void SetSceneVCam(SceneData data)
        {
            pastData.Clear();
            ChangeData(data.cameraData);
        }

        #region VCAM DATA -----------------------
        void ChangeData(VCamData data)
        {
            if (data == null) return;

            SetData(data);
            pastData.Push(data);
        }

        void RevertData()
        {
            if (pastData.Count >= 2)
                pastData.Pop();
            SetData(pastData.Peek());
        }

        private void SetData(VCamData data)
        {
            transposer.m_CameraDistance = data.distance;
            transposer.m_ScreenY = data.screenY;
            vCamEvent.RaiseDataChanged(data);
        }
        #endregion

        #region FOLLOW -----------------------
        public void Follow(Transform transform)
        {
            virtualCamera.Follow = transform;
            pastFollows.Push(transform);
        }

        void StopFollow()
        {
            if (pastFollows.Count >= 2)
                pastFollows.Pop();
            virtualCamera.Follow = pastFollows.Peek();
        }
        #endregion

        #region SCREENSHAKE -----------------------
#if UNITY_EDITOR
        [Button]
        void Screenshake() => Screenshake(0.2f, 0.4f);
#endif

        void Screenshake(float amplitude, float frequency)
        {
            CinemachineBasicMultiChannelPerlin multiChannelPerlin = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            multiChannelPerlin.m_NoiseProfile = noiseProfile;
        }

        [Button]
        void StopScreenshake()
        {
            virtualCamera.DestroyCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        #endregion
        #region PIXELPERFECT -----------------------
        void AddPixelPerfect() => pixelPerfect = virtualCamera.gameObject.AddComponent<CinemachinePixelPerfect>();

        void RemovePixelPerfect() { if (pixelPerfect != null) Destroy(pixelPerfect); }
        #endregion
        void ChangeConfiner(PolygonCollider2D collider2D)
        {
            confiner.m_BoundingShape2D = collider2D;
            confiner.InvalidatePathCache();
        }
    }
}