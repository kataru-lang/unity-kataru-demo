using System;
using UnityEngine;

namespace Kataru
{
    [CreateAssetMenu(fileName = "DialogueEvent", menuName = "ScriptableObjects/Kataru/DialogueEvent", order = 1)]
    public class DialogueEvent : ScriptableObject
    {

        /// <summary>
        /// Event to go to a specific passage.
        /// KataruSpeaker|KataruTrigger -> KataruManager.
        /// </summary>
        public Action<string> RunPassage;

        /// <summary>
        /// Event that's raised every line.
        /// Runner -> Dialogue
        /// </summary>
        public event Action<LineTag> Line;

        /// <summary>
        /// Think a line.
        /// KataruSpeaker -> ThinkDialogue.
        /// </summary>
        public event Action<Dialogue> ThinkLine;

        /// <summary>
        /// Say a line.
        /// KataruSpeaker -> SayDialogue.
        /// </summary>
        public event Action<Dialogue, Transform> SayLine;

        /// <summary>
        /// Show options.
        /// KataruManager -> Options.
        /// </summary>
        public event Action<Choices, Transform> ShowOptions;

        /// <summary>
        /// Get the next line by choosing via input.
        /// OptionsDialogue|SayDialogue -> KataruManager.
        /// </summary>
        public Action<string> PickChoice;

        /// <summary>
        /// Ends the dialogue sequence.
        /// </summary>
        public event Action EndDialogue;

        public void RaiseEndDialogue() => EndDialogue?.Invoke();
        public void RaiseLine(LineTag tag) => Line?.Invoke(tag);
        public void RaiseThinkLine(Dialogue dialogue) => ThinkLine?.Invoke(dialogue);
        public void RaiseSayLine(Dialogue dialogue, Transform transform) => SayLine?.Invoke(dialogue, transform);
        public void RaiseShowOptions(Choices choices, Transform transform) => ShowOptions?.Invoke(choices, transform);
    }
}
