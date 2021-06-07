using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Kataru
{
    public class KataruUI : Handler
    {
        [SerializeField] [Dropdown("CharacterList")] string reference;
        protected List<string> CharacterList() => Characters.All();
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