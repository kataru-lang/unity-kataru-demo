using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using JnA.Utils;

namespace Kataru
{
    /// <summary>
    /// A folder that can be enabled or disasbled by Kataru commands.
    /// </summary>
    class KataruFolder : Handler
    {

        [SerializeField] [Dropdown("CharacterList")] string reference;
        protected List<string> CharacterList() => Characters.All();

        /// <summary>
        /// Pass character name to the handler attributes.
        /// </summary>
        /// <value></value>
        protected override string Name
        {
            get => reference;
        }

#if UNITY_EDITOR
        // methods to easily trigger SetChildrenActive functionality in inspector
        [Button]
        void SetChildrenActive() => transform.SetChildrenActive(true);
        [Button]
        void SetChildrenInactive() => transform.SetChildrenActive(false);
#endif

        [CommandHandler(local: true)]
        void SetChildrenActive(bool isActive)
        {
            transform.SetChildrenActive(isActive);
        }

        [CommandHandler(local: true)]
        void SetChildActive(double index, bool isActive)
        {
            transform.GetChild((int)index).gameObject.SetActive(isActive);
        }
    }
}