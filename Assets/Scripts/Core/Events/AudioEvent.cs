using System;
using UnityEngine;
using UnityEngine.Audio;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;

[System.Serializable]
public struct MusicMetadata
{
    public AudioClip audio;

    public bool isPlaying;
}
// generic event to change the music
namespace JnA.Core.ScriptableObjects
{

    [CreateAssetMenu(fileName = "MusicEvent", menuName = "ScriptableObjects/Main/MusicEvent", order = 1)]
    public class AudioEvent : ScriptableObject
    {

        [System.Serializable]
        public class SnapshotDictionary : SerializableDictionaryBase<string, AudioMixerSnapshot> { }

        /// <summary>
        /// Maps names to snapshots.
        /// </summary>
        [SerializeField] SnapshotDictionary snapshots = new SnapshotDictionary();

        /// <summary>
        /// Transition to a given mixer snapshot.
        /// </summary>
        /// <param name="snapshotName"></param>
        /// <param name="timeToReach"></param>
        public void TransitionTo(string snapshotName, float timeToReach)
        {
            Debug.Log("Transition to snapshot in audio event...");
            AudioMixerSnapshot snapshot;
            if (snapshots.TryGetValue(snapshotName, out snapshot))
            {
                snapshot.TransitionTo(timeToReach);
            }
            else
            {
                throw new KeyNotFoundException($"No such mixer snapshot named '{snapshotName}'.");
            }
        }

        /// <summary>
        /// Event to tell SFX audio source to play a oneshot
        /// </summary>
        public Action<AudioClip> PlayOneShot;

        /// <summary>
        /// Event to tell audio source to change song
        /// Takes multiple tracks to support song stems.
        /// </summary>
        public Action<AudioClip[]> ChangeMusic;

        /// <summary>
        /// Event to inform that music audio source is done changing song
        /// </summary>
        public event Action<AudioClip[]> OnChangedMusic;

        public Action<float> ScrubMusic;

        public Action PlayMusic;
        public Action PauseMusic;
        public Func<float> GetMusicPlayback;
        public Func<MusicMetadata> GetCurrentMusicData;

        public Func<AudioSource> GetMusicSource;

        public void RaiseChangedMusic(AudioClip[] clips)
        {
            OnChangedMusic?.Invoke(clips);
        }
    }
}