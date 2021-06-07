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

        [SerializeField] [Dropdown("CharacterList")] string reference;
        protected List<string> CharacterList() => Characters.All();
        protected override string Name
        {
            get => reference.ToString();
        }

        bool waitNext = false;

        private void OnValidate()
        {
            ps = GetComponent<ParticleSystem>();
        }

        [Kataru.CommandHandler(local: true, autoNext: false)]
        protected virtual void PlayParticles(bool wait)
        {
            ps.Play();
            if (!wait)
            {
                Runner.Next();
            }
            else
            {
                waitNext = true;
            }
        }
    }
}