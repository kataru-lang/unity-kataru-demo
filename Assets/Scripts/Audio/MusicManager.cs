using JnA.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace JnA.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource[] audioSources;

        [SerializeField]
        AudioMixer mixer;

        [Header("Events")]
        [SerializeField] SceneDB sceneDatabase;

        [SerializeField] AudioEvent audioEvent;
        [SerializeField] int numTracks;
        [SerializeField] float defaultTransitionTime = 0.5f;

        private void OnValidate()
        {
            audioSources = GetComponents<AudioSource>();
        }

        private void OnEnable()
        {
            sceneDatabase.OnLoaded += OnLoaded;
            audioEvent.ChangeMusic += ChangeMusic;
            audioEvent.ScrubMusic += ScrubMusic;
            audioEvent.PauseMusic += PauseMusic;
            audioEvent.PlayMusic += PlayMusic;
            audioEvent.GetMusicPlayback += GetPlayback;
            audioEvent.GetCurrentMusicData += GetMetadata;
            audioEvent.GetMusicSource += GetMusicSource;
        }

        private void OnDisable()
        {
            sceneDatabase.OnLoaded -= OnLoaded;
            audioEvent.ChangeMusic -= ChangeMusic;
            audioEvent.ScrubMusic -= ScrubMusic;
            audioEvent.PauseMusic -= PauseMusic;
            audioEvent.PlayMusic -= PlayMusic;
            audioEvent.GetMusicPlayback -= GetPlayback;
            audioEvent.GetCurrentMusicData -= GetMetadata;
            audioEvent.GetMusicSource -= GetMusicSource;
        }

        void OnLoaded(string oldScene, SceneData sceneData)
        {
            if (sceneData.clips != null && sceneData.clips.Length > 0)
            {
                ChangeMusic(sceneData.clips);
            }

            if (sceneData.snapshot != null)
            {
                sceneData.snapshot.TransitionTo(defaultTransitionTime);
            }
        }

        void ChangeMusic(AudioClip[] clips)
        {
            if (clips.Length > audioSources.Length)
            {
                throw new System.Exception($"Music setup error: tried to set {clips.Length} clips while there are only {audioSources.Length} tracks.");
            }

            // Stop all audio tracks
            numTracks = clips.Length;
            for (int i = 0; i < audioSources.Length; i++)
            {
                AudioSource audioSource = audioSources[i];
                if (i < numTracks && audioSource.clip == clips[i] && audioSource.volume != 0) continue; //if the clip is the same and you can hear it, don't stop it.
                audioSource.Stop();
                if (i < numTracks)
                {
                    // Update and start all provided clips each in its own track
                    audioSources[i].clip = clips[i];
                    audioSources[i].time = 0f;
                    audioSources[i].Play();
                }
            }
            audioEvent.RaiseChangedMusic(clips);
        }

        void ScrubMusic(float newPlayback)
        {
            for (int i = 0; i < numTracks; ++i)
            {
                audioSources[i].time = newPlayback;
            }
        }

        void PauseMusic()
        {
            for (int i = 0; i < numTracks; ++i)
            {
                audioSources[i].Pause();
            }
        }

        void PlayMusic()
        {
            for (int i = 0; i < numTracks; ++i)
            {
                audioSources[i].Play();
            }
        }

        float GetPlayback() => audioSources[0].time;

        MusicMetadata GetMetadata() => new MusicMetadata { audio = audioSources[0].clip, isPlaying = audioSources[0].isPlaying };

        AudioSource GetMusicSource() => audioSources[0];
    }
}