using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using JnA.Utils;

namespace Kataru
{
    /// <summary>
    /// Enable and disable components via Kataru commands.
    /// </summary>
    public class KataruEnable : Handler
    {
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string reference = Characters.None;
        protected List<string> NamespaceList() => Namespaces.All();
        protected List<string> CharacterList() => Characters.AllInNamespace(kataruNamespace);
        protected override string Name
        {
            get => reference.ToString();
        }

#if UNITY_EDITOR
        protected override void OnEnable()
        {
            base.OnEnable();
            if (string.IsNullOrEmpty(reference) || reference == Characters.None)
            {
                Debug.LogError("ERROR: Character of " + gameObject.name + " is empty");
            }
        }
#endif


        [Kataru.CommandHandler(character: true)]
        protected virtual void EnableCollider(bool enabled)
        {
            GetComponent<Collider2D>().enabled = enabled;
        }

        [Kataru.CommandHandler(character: true, autoNext: false)]
        protected virtual void FadeSprite(double from, double to, double duration, double delay, bool wait)
        {
            float d = (float)duration;
            SpriteRenderer spr = GetComponent<SpriteRenderer>();
            if (from != -1)
                spr.color = Functions.ChangeColorA(spr.color, (float)from);
            spr.DOFade((float)to, d).SetDelay((float)delay);
            if (!wait) Runner.Next();
            else Runner.DelayedNext(d);
        }

        [Kataru.CommandHandler(character: true)]
        protected virtual void EnableSpeaker(bool enabled)
        {
            GetComponent<KataruSpeaker>().enabled = enabled;
        }

        // if there's multiple of the same speaker, call this on interact
        public void EnableSpeakerThenInteract(KataruSpeaker speaker)
        {
            StartCoroutine(RunEnableThenInteract(speaker));
        }

        IEnumerator RunEnableThenInteract(KataruSpeaker speaker)
        {
            speaker.enabled = true;
            yield return null;
            speaker.GetComponent<KataruInteractable>().OnInteract();
        }


    }
}