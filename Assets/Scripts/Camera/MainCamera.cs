using UnityEngine;
using Cinemachine;
using JnA.Core.ScriptableObjects;


namespace JnA.Cam
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] new UnityEngine.Camera camera;
        [SerializeField] SceneDB sceneDatabase;

        private void OnValidate()
        {
            camera = gameObject.GetComponent<UnityEngine.Camera>();
        }

        private void Awake()
        {
            sceneDatabase.OnLoaded += (oldScene, scene) =>
            {
                SetCamera(scene);
            };
        }

        void SetCamera(SceneData data)
        {
            camera.backgroundColor = data.cameraBgColor;
        }
    }
}