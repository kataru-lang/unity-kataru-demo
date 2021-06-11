using System.Collections.Generic;
using JnA.Core.ScriptableObjects;
using JnA.Interaction;
using NaughtyAttributes;
using UnityEngine;

namespace Kataru
{
    /// <summary>
    /// Used to open or close closeups via Kataru commands
    /// </summary>
    public class KataruCloseup : Handler
    {
        [Header("Closeup")]
        [SerializeField] CloseupBase closeup;

        [Header("Kataru")]
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string reference = Characters.None;
        protected List<string> NamespaceList() => Namespaces.All();
        protected List<string> CharacterList() => Characters.AllInNamespace(kataruNamespace);

        protected override string Name
        {
            get => reference;
        }

        [Kataru.CommandHandler(character: true, autoNext: false)]
        void ShowCloseup(bool wait)
        {
            closeup.ShowCloseup();
            Debug.Log($"show closeup for {reference}; wait: {wait}");
            if (!wait)
                Runner.Next();
            else
                closeup.closeupEvent.OnHideCloseup += TryNext;
        }

        void TryNext(string goName)
        {
            if (goName == closeup.gameObject.name)
            {
                Runner.Next();
            }
        }

        /// <summary>
        /// Force end a closeup
        /// </summary>
        [Kataru.CommandHandler(character: true)]
        void HideCloseup()
        {
            closeup.HideCloseup();
        }
    }
}