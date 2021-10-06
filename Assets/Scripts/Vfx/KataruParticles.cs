using System.Collections;
using System.Collections.Generic;
using Kataru;
using NaughtyAttributes;
using UnityEngine;

namespace JnA.Vfx
{
    [RequireComponent(typeof(ParticleSystem))]
    public class KataruParticles : Handler
    {
        [SerializeField] ParticleSystem ps;

        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string reference = Characters.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);
        protected override string Name
        {
            get => reference.ToString();
        }

        private void OnValidate()
        {
            ps = GetComponent<ParticleSystem>();
        }

        [Kataru.CommandHandler(character: true, autoNext: false)]
        protected virtual void PlayParticles(bool wait)
        {
            ps.Play();
            if (!wait)
            {
                Runner.Next();
            }
            else
            {
                Runner.DelayedNext(ps.main.duration);
            }
        }
    }
}