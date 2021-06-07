using UnityEngine;
using UnityEngine.InputSystem;
using JnA.UI.Settings;
using Kataru;

namespace JnA.UI.Dialogue
{
    public enum TextType
    {
        All,
        Think,
        Say
    }

    /// <summary>
    /// Base class for text displayed dialogue.
    /// Extended by SayDialogue and ThinkDialogue.
    /// </summary>
    public class TextDialogue : Dialogue
    {
        [Header("Text-specific")]
        // feel free to exchange this out for another typewriter package
        [SerializeField] protected Typewriter typewriter;


        /// <summary>
        /// Hide say dialogue when options appear.
        /// </summary>
        /// <param name="choices"></param>
        /// <param name="collider2D"></param>
        /// <returns></returns>
        protected virtual void OnShowOptions(Kataru.Choices choices, Transform target) => Hide();

        protected override void Show()
        {
            if (!showing)
                EnableDialogueListener();
            base.Show();
        }

        protected override void Hide()
        {
            if (showing)
                DisableDialogueListener();
            base.Hide();
        }

        protected void EnableDialogueListener()
        {
            interactAction.performed += TriggerNextLineListener;
        }

        protected void DisableDialogueListener()
        {
            typewriter.ResetSpeed();
            interactAction.performed -= TriggerNextLineListener;
        }

        void TriggerNextLineListener(InputAction.CallbackContext ctx) => TriggerNextLine();

        // Possible listener for an event trigger for an OnClick type event
        public void TriggerNextLine()
        {
            if (typewriter.IsTyping())
            {
                typewriter.FastForwardSpeed();
            }
            else
            {
                Kataru.Runner.Next();
                typewriter.ResetSpeed();
            }
        }

        protected override bool ShouldHide(LineTag tag) => tag != LineTag.Dialogue;


        protected void ShowLine(string line)
        {
            typewriter.ShowText(line);
        }
    }
}