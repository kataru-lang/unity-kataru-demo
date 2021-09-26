using System;
using System.Collections;
using System.Collections.Generic;
using JnA.Platformer;
using NaughtyAttributes;
using UnityEngine;

namespace Kataru
{
    [RequireComponent(typeof(ControllerBase))]
    public class KataruController : Handler
    {
        [SerializeField] protected ControllerBase controller;

        [SerializeField] Transform[] destinations;
        [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
        [SerializeField] [Dropdown("CharacterList")] string character = Characters.None;
        protected string[] NamespaceList() => Namespaces.All();
        protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);
        protected override string Name
        {
            get => character.ToString();
        }

        private void OnValidate()
        {
            controller = GetComponent<ControllerBase>();
        }

#if UNITY_EDITOR
        protected override void OnEnable()
        {
            base.OnEnable();
            if (string.IsNullOrEmpty(character) || character == Characters.None)
            {
                Debug.LogError("ERROR: Character of " + gameObject.name + " is empty");
            }
        }
#endif

        /// <summary>
        /// Walk in a direction for a set duration in seconds.
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="duration"></param>
        /// <param name="wait"></param>
        /// <param name="activeOnComplete"></param>
        [Kataru.CommandHandler(character: true, autoNext: false)]
        protected virtual void Walk(double xAxis, double duration, bool wait, bool activeOnComplete)
        {
            controller.StartMove(new Vector2((float)xAxis, 0));

            if (!wait)
                Runner.Next();
            else
                StartCoroutine(WalkForDuration(duration, activeOnComplete));

        }

        /// <summary>
        /// Walk to a named location.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="wait"></param>
        /// <param name="activeOnComplete"></param>
        [Kataru.CommandHandler(character: true, autoNext: false)]
        protected virtual void WalkTo(string name, double range, bool wait, bool activeOnComplete)
        {
            Transform target = GetDestination(name);

            if (TargetReached(target, range)) return;

            controller.StartMoveTo(target.transform.position);

            StartCoroutine(WalkUntilTargetReached(target, range, activeOnComplete, wait));
        }

        private Transform GetDestination(string name)
        {
            Transform target = Array.Find(destinations, (x) => x.name == name);
            if (target == null) throw new NullReferenceException($"No walk target named '{name}'");
            return target;
        }

        /// <summary>
        /// Walk until a duration in seconds is complete.
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="activeOnComplete"></param>
        /// <returns></returns>
        protected virtual IEnumerator WalkForDuration(double seconds, bool activeOnComplete)
        {
            yield return StartCoroutine(Runner.DelayedNext((float)seconds));
            EndMove(activeOnComplete);
        }

        bool TargetReached(Transform target, double range) =>
            Vector2.Distance(transform.position, target.position) < range;

        /// <summary>
        /// Coroutine to walk until target reached.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="activeOnComplete"></param>
        /// <returns></returns>
        protected virtual IEnumerator WalkUntilTargetReached(Transform target, double range, bool activeOnComplete, bool wait)
        {
            if (!wait)
            {
                Runner.Next();
                yield return new WaitUntil(() => TargetReached(target, range));
            }
            else
            {
                yield return Runner.DelayedNext(() => TargetReached(target, range));
            }
            EndMove(activeOnComplete);
        }

        void EndMove(bool activeOnComplete)
        {
            controller.EndMove();
            gameObject.SetActive(activeOnComplete);
        }
    }
}