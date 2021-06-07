using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace JnA.UI.Settings
{
    [RequireComponent(typeof(UniversalAdditionalCameraData))]
    public class PostProcessListener : MonoBehaviour
    {
        [SerializeField] SettingsEvent settingsEvent;

        void Awake()
        {
            settingsEvent.OnSetPostProcess += SetPostProcess;
        }

        void SetPostProcess(bool on)
        {
            GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = on;
        }
    }
}