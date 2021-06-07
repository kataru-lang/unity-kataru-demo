using JnA.Core.ScriptableObjects;
using UnityEngine;

namespace JnA.Audio
{
    public class SfxManager : MonoBehaviour
    {
        [SerializeField]
        AudioSource audioSource;

        [Header("Events")]
        [SerializeField] SceneDB sceneDatabase;

        [SerializeField] AudioEvent audioEvent;

        private void OnEnable()
        {
            audioEvent.PlayOneShot += PlayOneShot;
        }

        void PlayOneShot(AudioClip audioClip) => audioSource.PlayOneShot(audioClip);
    }
}