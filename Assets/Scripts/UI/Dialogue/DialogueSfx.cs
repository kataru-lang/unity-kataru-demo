using JnA.Core.ScriptableObjects;
using Kataru;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace JnA.UI.Dialogue
{
    [System.Serializable]
    public class AudioClips
    {
        public AudioClip[] clips;
    }

    [System.Serializable]
    public class VoiceBlipsDictionary : SerializableDictionaryBase<string, AudioClips> { }

    /// <summary>
    /// Add OnCharacter as a listener to an event that occurs everytime a character is typed
    /// </summary>
    public class DialogueSfx : MonoBehaviour
    {
        [InfoBox("Dictionary of character names to voice blips.", EInfoBoxType.Normal)]
        [SerializeField] protected VoiceBlipsDictionary voiceBlipsDict;
        [InfoBox("If character key not found in voiceBlipsDict, then can resort to defaultBlips.", EInfoBoxType.Normal)]
        [SerializeField] AudioClip[] defaultBlips;
        [SerializeField] protected AudioEvent audioEvent;
        [SerializeField] protected DialogueEvent dialogueEvent;
        [Range(0, 0.5f)] [SerializeField] float reducedTimeBetweenBlips = 0.1f;
        protected float nextBlipTime;

        string currCharacter = string.Empty;

        public void TrySetSfx(string character)
        {
            if (character != currCharacter)
            {
                currCharacter = character;
            }
        }

        // listener to textAnimatorPlayer OnCharacterVisible
        public void OnChar(char c)
        {
            AudioClip[] voiceBlips;

            if (voiceBlipsDict.TryGetValue(currCharacter, out AudioClips blipsContainer)) voiceBlips = blipsContainer.clips;
            else voiceBlips = defaultBlips;

            if (!char.IsLetterOrDigit(c) || voiceBlips == null) return;

            if (nextBlipTime < Time.realtimeSinceStartup)
            {
                AudioClip random = voiceBlips[Random.Range(0, voiceBlips.Length)];
                audioEvent.PlayOneShot(random);
                nextBlipTime = Time.realtimeSinceStartup + random.length - reducedTimeBetweenBlips;
            }
        }
    }
}