using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Kataru
{
    public class KataruUI : Handler
    {
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string reference;
        protected List<string> NamespaceList() => Namespaces.All();
        protected List<string> CharacterList() => Characters.AllInNamespace(kataruNamespace);
        protected override string Name
        {
            get => reference.ToString();
        }

        [Kataru.CommandHandler(local: true)]
        protected virtual void ClickButton()
        {
            GetComponent<Button>().onClick?.Invoke();
        }
    }
}