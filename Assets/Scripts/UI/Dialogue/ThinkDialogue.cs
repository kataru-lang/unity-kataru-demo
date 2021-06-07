using JnA.Core.ScriptableObjects;
using Kataru;
using UnityEngine;

namespace JnA.UI.Dialogue
{
    public class ThinkDialogue : TextDialogue
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            dialogueEvent.ThinkLine += ThinkLine;
            dialogueEvent.ShowOptions += OnShowOptions;
            dialogueEvent.SayLine += SayLine;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dialogueEvent.ThinkLine -= ThinkLine;
            dialogueEvent.ShowOptions -= OnShowOptions;
            dialogueEvent.SayLine -= SayLine;
        }

        /// <summary>
        /// Say a given line.
        /// Automatically triggers fade animation if speaker has changed.
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="position"></param>
        private void ThinkLine(Kataru.Dialogue dialogue)
        {
            // if first time showing think dialogue
            if (!IsActive()) Show();
            // else if showed options and now showing think dialogue
            else if (typewriter.IsHidden()) EnableDialogueListener();
            ShowLine(dialogue.text);
        }

        // we don't want to call Hide(), just base.Hide()
        private void SayLine(Kataru.Dialogue dialogue, Transform target) => base.Hide();

        /// <summary>
        /// Hide say dialogue when options appear.
        /// </summary>
        /// <param name="choices"></param>
        /// <param name="collider2D"></param>
        /// <returns></returns>
        protected override void OnShowOptions(Kataru.Choices choices, Transform target)
        {
            // DisableDialogueListener();
            // typewriter.HideText();
            base.Hide();
        }

        protected override bool ShouldHide(LineTag tag) => base.ShouldHide(tag) && tag != LineTag.Choices;
    }
}