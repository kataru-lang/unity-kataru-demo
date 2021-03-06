using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Kataru
{
    /// <summary>
    /// Script for handling commands that deal with UI
    /// Feel free to modify
    /// </summary>
    public class KataruUI : Handler
    {
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string reference = Characters.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);
        protected override string Name
        {
            get => reference.ToString();
        }

        [Kataru.CommandHandler(character: true)]
        protected virtual void ClickButton()
        {
            GetComponent<Button>().onClick?.Invoke();
        }
    }
}