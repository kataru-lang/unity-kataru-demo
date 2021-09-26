using UnityEngine;
using System;
using JnA.Utils;
using JnA.Core.ScriptableObjects;
using NaughtyAttributes;

namespace Kataru
{
    /// <summary>
    /// Kataru Manager class.
    /// This class is in charge of initializing the Runner.
    /// Also includes examples of reacting to Runner's events.
    /// </summary>
    public class KataruManager : Manager
    {
        [Header("Events")]
        [SerializeField] InputEvent inputEvent;
        [SerializeField] SceneDB sceneDB;
        [SerializeField] DialogueEvent dialogueEvent;
        [SerializeField] AudioEvent audioEvent;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Runner.OnLine += dialogueEvent.RaiseLine;
            dialogueEvent.EndDialogue += EndDialogue;
            dialogueEvent.PickChoice += PickChoice;
            dialogueEvent.RunPassage += RunPassage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Runner.OnLine -= dialogueEvent.RaiseLine;
            dialogueEvent.EndDialogue -= EndDialogue;
            dialogueEvent.PickChoice -= PickChoice;
            dialogueEvent.RunPassage -= RunPassage;
        }

        void OnSceneLoadedNextLine(string old, SceneData newScene)
        {
            sceneDB.OnLoaded -= OnSceneLoadedNextLine;
            Runner.Next();
        }

        void PickChoice(string input) => Runner.Next(input);

        /// <summary>
        /// Starts a Kataru passage.
        /// Switches action map to UI and makes the runner goto passage node.
        /// </summary>
        /// <param name="passage"></param>
        void RunPassage(string passage)
        {
            Debug.Log($"RunPassage({passage})");
            if (Runner.isRunning)
            {
                Debug.LogWarning($"Attempted to start passage '{passage}' while passage '{Runner.GetPassage()}' was already running.");
                return;
            }

            inputEvent.SwitchActionMap(JnA.Utils.Constants.UI_MAP, false);
            Runner.RunPassage(passage.ToString());
        }

        /// <summary>
        /// Handles when Kataru has no more lines to provide.
        /// Reverts the input map to the scene's default and triggers a dialogue event.
        /// </summary>
        protected override void OnDialogueEnd()
        {
            dialogueEvent.RaiseEndDialogue();
        }

        void EndDialogue()
        {
            inputEvent.RevertToSceneMap();
        }

#if UNITY_EDITOR
        // Debug print handlers.
        protected override void OnChoices(Choices choices)
        {
            Debug.Log($"Choices: {{{String.Join(", ", choices.choices)}}}");
        }
#endif

#if UNITY_EDITOR
        // Debug print handlers.
        protected override void OnInvalidChoice()
        {
            Debug.LogError("Invalid choice.");
        }
#endif

        [CommandHandler]
        void LoadScene(string scene)
        {
            sceneDB.LoadScene(scene);
            sceneDB.OnLoaded += OnSceneLoadedNextLine;
        }

        [CommandHandler]
        void Wait(double duration)
        {
            StartCoroutine(Runner.DelayedNext((float)duration));
        }

        [Button]
        [CommandHandler]
        public void Save()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Debug.LogError("Save: Game needs to be running");
                return;
            }
#endif
            Runner.Save();
            Runner.Next();
        }

        #region MUSIC
        [CommandHandler]
        void TransitionToSnapshot(string snapshot, double seconds)
        {
            audioEvent.TransitionTo(snapshot, (float)seconds);
            Runner.Next();
        }
        #endregion

        #region MATH
        [CommandHandler]
        void Clamp(string key, double value, double increment, double min, double max)
        {
            double v = value + increment;
            if (v > max || v < min) return;
            Runner.SetState(key, v);
        }
        #endregion
    }
}