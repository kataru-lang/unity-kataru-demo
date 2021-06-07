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
        [SerializeField] [Dropdown("NamespaceList")] protected string kataruNamespace;
        [SerializeField] [Dropdown("PassagesList")] protected string passage;
        protected List<string> NamespaceList() => Namespaces.All();
        protected List<string> PassagesList() => Passages.AllInNamespace(kataruNamespace);
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(kataruNamespace) && !string.IsNullOrEmpty(passage))
            {
                kataruNamespace = NamespaceList().FirstOrDefault((s) => passage.StartsWith(s));
            }
        }

        protected void OnEnable()
        {
            if (string.IsNullOrEmpty(passage) || passage == Passages.None)
            {
                Debug.LogError("ERROR: Passage of " + gameObject.name + " is empty");
            }
            if (string.IsNullOrEmpty(kataruNamespace))
            {
                Debug.LogError("ERROR: Namespace of " + gameObject.name + " is empty");
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