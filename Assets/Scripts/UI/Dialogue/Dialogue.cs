using UnityEngine;
using UnityEngine.InputSystem;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using Kataru;

// public enum ActionMapType
// {
//     Player,
//     UI
// }

namespace JnA.UI.Dialogue
{
    /// <summary>
    /// Defines dialogue UI.
    /// Handles all related UI logic like animations.
    /// Extended by DialogueOptionsGroup and TextDialogue.
    /// </summary>
    public class Dialogue : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] protected Animator animator;
        protected readonly int openHash = Animator.StringToHash("Open");

        [Header("Events")]
        [SerializeField] protected DialogueEvent dialogueEvent;
        [SerializeField] InputEvent inputEvent;
        // [SerializeField] ActionMapType actionMapType;
        protected InputAction interactAction;

        [SerializeField] protected AudioEvent audioEvent;

        [SerializeField] AudioClip openClip;
        [SerializeField] AudioClip closeClip;
        [SerializeField] AudioClip nextClip;

        protected bool showing;

        public void PlayCloseClip()
        {
            if (IsActive()) audioEvent.PlayOneShot(closeClip);
        }

        public void PlayOpenClip()
        {
            if (IsActive())
            {
                audioEvent.PlayOneShot(nextClip);
            }
            else
            {
                audioEvent.PlayOneShot(openClip);
            }
        }

        protected virtual void Awake()
        {
            // Listen for input
            // switch (actionMapType)
            // {
            //     case ActionMapType.Player:
            //         InputActionMap playerMap = inputEvent.input.FindActionMap(Constants.PLAYER_MAP, true);
            //         interactAction = playerMap.FindAction(Constants.INTERACT_ACTION, true);
            //         break;
            //     case ActionMapType.UI:
            //     default:
            InputActionMap uiMap = inputEvent.input.FindActionMap(Constants.UI_MAP, true);
            interactAction = uiMap.FindAction(Constants.INTERACT_ACTION, true);
            //         break;
            // }
        }

        protected virtual void OnEnable()
        {
            dialogueEvent.EndDialogue += Hide;
            dialogueEvent.Line += TryHide;
        }

        protected virtual void OnDisable()
        {
            dialogueEvent.EndDialogue -= Hide;
            dialogueEvent.Line -= TryHide;
        }

        protected void OnVCamDataChanged(VCamData data)
        {
            float distance = data.distance;
            RectTransform t = (RectTransform)transform;
            float xSign = t.localScale.x < 0 ? -1 : 1;
            float scale = Mathf.Lerp(0.01f, 0.022f, (distance - 8) / (20 - 8));
            t.localScale = new Vector3(scale * xSign, scale, 1);
        }

        protected bool IsActive() => animator.GetBool(openHash);

        protected virtual void Show()
        {
            if (showing) return;
            showing = true;
            // Ensure gameobject is on and we set dialogueUI field
            gameObject.SetActive(true);
            animator.SetBool(openHash, true);
        }

        protected void TryHide(LineTag tag)
        {
            if (ShouldHide(tag)) Hide();
        }

        protected virtual bool ShouldHide(LineTag tag) => false;

        protected virtual void Hide()
        {
            if (!showing) return;
            showing = false;
            animator.SetBool(openHash, false);
        }
    }
}