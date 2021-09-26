using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Kataru
{
    /// <summary>
    /// Handle animation through Kataru
    /// </summary>
    public class KataruAnimator : Handler
    {
        [SerializeField] protected Animator animator;
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string reference = Characters.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);
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
        protected virtual void SetAnimatorInt(string param, double val)
        {
            animator.SetInteger(param, (int)val);
        }

        [Kataru.CommandHandler(character: true)]
        protected virtual void SetAnimatorBool(string param, bool val)
        {
            animator.SetBool(param, val);
        }

        [Kataru.CommandHandler(character: true)]
        protected virtual void PlayAnimationState(string state, double layer)
        {
            animator.Play(state, (int)layer);
        }

        [Kataru.CommandHandler(character: true, autoNext: false)]
        protected virtual void SetAnimatorTrigger(string trigger, bool wait)
        {
            animator.SetTrigger(trigger);
            if (!wait)
            {
                Runner.Next();
            }
            else
            {
                StartCoroutine(WaitAnimationNext());
            }
        }

        IEnumerator WaitAnimationNext()
        {
            // wait a frame to ensure transition to next state
            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length
            + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Runner.Next();
        }
    }
}