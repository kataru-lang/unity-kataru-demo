using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Kataru
{
    /// <summary>
    /// An interactable that can also say dialogue.
    /// </summary>
    public class KataruSpeaker : Handler
    {
        [SerializeField] protected DialogueEvent dialogueEvent;
        [SerializeField] Transform target;
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string character = Characters.None;
        protected List<string> NamespaceList() => Namespaces.All();
        protected List<string> CharacterList() => Characters.AllInNamespace(kataruNamespace);

        /// Pass character name to the handler attributes.
        /// </summary>
        /// <value></value>
        protected override string Name
        {
            get => character.ToString();
        }

#if UNITY_EDITOR

        protected override void OnEnable()
        {
            base.OnEnable();
            if (string.IsNullOrEmpty(character) || character == Characters.None)
            {
                Debug.LogError("ERROR: Character of " + gameObject.name + " is empty");
            }
        }
#endif

        [CharacterHandler]
        void OnDialogue(Dialogue dialogue)
        {
            dialogueEvent.RaiseSayLine(dialogue, target);
        }
    }
}