using System.Collections;
using Cinemachine;
using JnA.Core.ScriptableObjects;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Kataru
{
    [System.Serializable]
    public class StringVCamDictionary : SerializableDictionaryBase<string, VCamData> { }

    public class KataruVCam : Handler
    {
        [SerializeField] VCamEvent cameraEvent;
        [SerializeField] StringVCamDictionary stringToVCam;

        [CommandHandler]
        void ChangeVCamData(string key)
        {
            cameraEvent.ChangeData(stringToVCam[key]);
        }

        [CommandHandler]
        void VCamScreenshake(double amplitude, double frequency, double duration)
        {
            cameraEvent.Screenshake((float)amplitude, (float)frequency);
            if (duration != -1)
                StartCoroutine(WaitStopScreenshake((float)duration));
        }

        IEnumerator WaitStopScreenshake(float duration)
        {
            yield return new WaitForSeconds(duration);
            VCamStopScreenshake();
        }

        [CommandHandler]
        void VCamStopScreenshake()
        {
            cameraEvent.StopScreenshake();
        }
    }
}