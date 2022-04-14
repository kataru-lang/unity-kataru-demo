using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using JnA.Core.Interaction;
#if UNITY_EDITOR
using System.Linq;
#endif

namespace Kataru
{
    /// <summary>
    /// An interactable that triggers a passage OnInteract.
    /// </summary>
    public class KataruInteractable : MonoBehaviour
    {
        [SerializeField] bool useOnce = false;
        [SerializeField] protected DialogueEvent dialogueEvent;

        #region PASSAGE
        [SerializeField][Dropdown("NamespaceList")] protected string kataruNamespace = Namespaces.Global;
        [SerializeField][Dropdown("PassagesList")] protected string passage = Passages.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] PassagesList() => Passages.InNamespace(kataruNamespace);
        #endregion

        /// <summary>
        /// If there's an Interactable component on this same gameobject;
        /// there must be a collider2d. In which case we should disable the collider
        /// when the kataru passage starts, then reenable it when kataru passage ends,
        /// so that we dont have to walk out then back in to be able to re-interact
        /// </summary>
        Collider2D collider2d;

#if UNITY_EDITOR
        protected void OnEnable()
        {
            if (string.IsNullOrEmpty(passage) || passage == Passages.None)
            {
                Debug.LogError("ERROR: Passage of " + gameObject.name + " is empty");
            }
        }
#endif

        private void Awake()
        {
            if (GetComponent<Interactable>() != null)
            {
                collider2d = GetComponent<Collider2D>();
                if (collider2d == null)
                {
                    Debug.LogError($"Expected a collider on gameObject '{gameObject.name}'");
                }
            }
        }

        public void OnInteract()
        {
            if (collider2d != null)
            {
                collider2d.enabled = false;
                dialogueEvent.EndDialogue += EnableCollider;
            }

            if (useOnce)
            {
                Destroy(this);
            }

            dialogueEvent.RunPassage(passage);
        }

        void EnableCollider()
        {
            collider2d.enabled = true;
            dialogueEvent.EndDialogue -= EnableCollider;
        }

        // in cases like:
        // - we're in a closeup and we want to prematurely exit 
        public void ForceExitInteraction()
        {
            Runner.Exit();
        }

        public void SetPassage(string passage) => this.passage = passage;
    }
}