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
        [SerializeField] TextType textType = TextType.All;
        [InfoBox("Dictionary of character names to voice blips.", EInfoBoxType.Normal)]
        [SerializeField] protected VoiceBlipsDictionary voiceBlipsDict;
        [SerializeField] protected AudioEvent audioEvent;
        [SerializeField] protected DialogueEvent dialogueEvent;
        protected float nextBlipTime;

        string currCharacter = string.Empty;

        protected void OnEnable()
        {
            if (textType == TextType.All || textType == TextType.Say)
                dialogueEvent.SayLine += SetCharacterSay;
            if (textType == TextType.All || textType == TextType.Think)
                dialogueEvent.ThinkLine += SetCharacter;
        }

        protected void OnDisable()
        {
            dialogueEvent.SayLine -= SetCharacterSay;
            dialogueEvent.ThinkLine -= SetCharacter;
        }

        void SetCharacter(Kataru.Dialogue dialogue) => currCharacter = dialogue.name;

        void SetCharacterSay(Kataru.Dialogue dialogue, Transform target) => SetCharacter(dialogue);

        // listener to textAnimatorPlayer OnCharacterVisible
        public void OnChar(char c)
        {
            AudioClips voiceBlips;
            if (!char.IsLetterOrDigit(c) || !voiceBlipsDict.TryGetValue(currCharacter, out voiceBlips)) return;
            if (nextBlipTime < Time.realtimeSinceStartup)
            {
                AudioClip random = voiceBlips.clips[Random.Range(0, voiceBlips.clips.Length)];
                audioEvent.PlayOneShot(random);
                nextBlipTime = Time.realtimeSinceStartup + random.length;
            }
        }
    }
}