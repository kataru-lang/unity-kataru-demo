using System.Collections;
using System.Collections.Generic;
using JnA.Core.ScriptableObjects;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Kataru
{
    /// <summary>
    /// Handles scene data.
    /// </summary>
    class KataruScene : Handler
    {
        [SerializeField] protected DialogueEvent dialogueEvent;
        [SerializeField] protected AudioEvent audioEvent;

        #region PASSAGE
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("PassagesList")] string passage = Passages.None;
        protected List<string> NamespaceList() => Namespaces.All();
        protected List<string> PassagesList() => Passages.AllInNamespace(kataruNamespace);
        #endregion
        [System.Serializable]
        public class AudioClipDictionary : SerializableDictionaryBase<string, AudioClip> { }
        [SerializeField] AudioClipDictionary audioClips;

#if UNITY_EDITOR
        protected override void OnEnable()
        {
            base.OnEnable();
            if (string.IsNullOrEmpty(passage))
            {
                Debug.LogError("ERROR: Passage of " + gameObject.name + " is empty");
            }
        }
#endif

        /// <summary>
        /// Runs a given passage on start.
        /// </summary>
        IEnumerator Start()
        {
            if (passage == Passages.None) yield break;

            // wait for a few beats or else ui map might get requested before scene map gets set
            yield return new WaitForSeconds(0.1f);
            dialogueEvent?.RunPassage(passage);

            // Autosave
            Runner.Save();
        }

        #region MUSIC
        [CommandHandler(autoNext: false)]
        public void PlaySfx(string clip, bool wait)
        {
            if (audioClips.TryGetValue(clip, out AudioClip audioClip))
            {
                audioEvent.PlayOneShot(audioClip);
            }
            else
            {
                Debug.LogError($"No SFX clip named '{clip}'. Make sure this Kataru Scene has '{clip}' in its Audio Clips.");
            }
            if (wait)
            {
                StartCoroutine(Runner.DelayedNext(audioClip.length));
            }
            else
            {
                Runner.Next();
            }
        }

        [CommandHandler]
        public void PlayMusic(string clip)
        {
            if (audioClips.TryGetValue(clip, out AudioClip audioClip))
            {
                audioEvent.ChangeMusic(new[] { audioClip });
            }
            else
            {
                Debug.LogError($"No audio clip named '{clip}'. Make sure this Kataru Scene has '{clip}' in its Audio Clips.");
            }
        }
        #endregion
    }
}
