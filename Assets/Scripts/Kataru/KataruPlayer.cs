using System.Collections;
using System.Collections.Generic;
using JnA.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using JnA.Utils;
using JnA.Core.ScriptableObjects;

namespace Kataru
{
    /// <summary>
    /// Handles player dialogue.
    /// </summary>
    class KataruPlayer : Handler
    {
        [SerializeField] DialogueEvent dialogueEvent;
        [SerializeField] Transform dialogueTarget;
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string character = Characters.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);

        protected override string Name
        {
            get => character;
        }

        [CharacterHandler]
        void OnDialogue(Dialogue dialogue)
        {
            dialogueEvent.RaiseSayLine(dialogue, dialogueTarget);
        }


        [CharacterHandler(Characters.Think)]
        void OnThink(Dialogue dialogue)
        {
            dialogueEvent.RaiseThinkLine(dialogue);
        }

        protected override void OnChoices(Choices choices) => dialogueEvent.RaiseShowOptions(choices, dialogueTarget);
    }
}
