using System;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JnA.Interaction
{
    public class CloseupBase : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] InputEvent inputEvent;
        [SerializeField] protected PauseEvent pauseEvent;
        [SerializeField] internal CloseupEvent closeupEvent;

        [Header("UI")]
        [SerializeField] protected CanvasGroup group;
        [HideInInspector] public InputAction backoutAction;

        // sometimes, don't want to let player exit with Esc
        [SerializeField] bool allowPlayerToExit = true;

        // set this to false if we dont want to fade out when calling HideCloseup for whatever reason
        [SerializeField] bool hideAtEnd = true;

        private void Awake()
        {
            backoutAction = inputEvent.input.FindActionMap(Constants.UI_MAP).FindAction("Back");
        }

        public bool IsShowing() => group.alpha > 0;

        [Button]
        virtual public void ShowCloseup()
        {
            group.FadeIn();
            inputEvent.SwitchActionMap(Constants.UI_MAP, false);
            if (allowPlayerToExit)
            {
                backoutAction.performed += HideCloseupListener;
                pauseEvent.EnablePause(false);
            }
            closeupEvent.RaiseShowCloseup(gameObject.name);
        }

        private void OnDestroy()
        {
            backoutAction.performed -= HideCloseupListener;
        }

        // for input actions
        public void HideCloseupListener(InputAction.CallbackContext ctx) => HideCloseup();

        // for buttons
        public void HideCloseupListener() => HideCloseup();

        /// <summary>
        /// Split into two different parts for CloseupScene - 
        /// Cleanup should be called after scene is unloaded
        /// </summary>
        /// <param name="onComplete"></param>
        [Button]
        virtual internal void HideCloseup(Action onComplete = null)
        {
            Hide(onComplete);
            Cleanup();
        }

        virtual protected void Hide(Action onComplete)
        {
            if (allowPlayerToExit)
            {
                backoutAction.performed -= HideCloseupListener;
                pauseEvent.EnablePause(true);
            }
            if (hideAtEnd)
                group.FadeOut(onComplete: onComplete);
            else onComplete();
        }

        virtual protected void Cleanup()
        {
            inputEvent.RevertToSceneMap();
            closeupEvent.RaiseHideCloseup(gameObject.name);
        }
    }
}