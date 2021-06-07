using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
#endif

namespace Kataru
{
    /// <summary>
    /// An interactable that triggers a passage OnInteract.
    /// A speaker is any game object with a collider that can send its collider to dialogue objects.
    /// </summary>
    public class KataruInteractable : MonoBehaviour
    {
        [SerializeField] bool useOnce = false;
        [SerializeField] protected DialogueEvent dialogueEvent;

        #region PASSAGE
        [SerializeField] [Dropdown("PassagesList")] protected string passage = Passages.None;
        protected List<string> PassagesList() => Passages.All();
        #endregion

#if UNITY_EDITOR
        protected void OnEnable()
        {
            if (string.IsNullOrEmpty(passage) || passage == Passages.None)
            {
                Debug.LogError("ERROR: Passage of " + gameObject.name + " is empty");
            }
        }
#endif

        public void OnInteract()
        {
            dialogueEvent.RunPassage(passage);

            if (useOnce)
            {
                Destroy(this);
            }
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