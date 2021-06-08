using DG.Tweening;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JnA.Core
{
    /// <summary>
    /// There are many "Main" classes throughout this project;
    /// Main classes serve as controllers for an overall game or minigame.
    /// Examples: Main, BunnioMain, DanceOffMain
    /// 
    /// They all share similar features such as controlling pause menus,
    /// hence this base class.
    /// </summary>
    public class MainBase : MonoBehaviour
    {
        [BoxGroup("Input")] [SerializeField] protected InputEvent inputEvent;
        [BoxGroup("Pause")] [SerializeField] protected CanvasGroupSelect pauseMenu;
        [BoxGroup("Pause")] [SerializeField] protected PauseEvent pauseEvent;
        protected InputAction playerPause, uiPause;
        protected bool paused = false;


        // keep track of whether we added pause listeners or not so that we don't add twice
        bool listeningForPause = false;

        protected virtual void Awake()
        {
            playerPause = inputEvent.input.FindActionMap(Constants.PLAYER_MAP, true).FindAction(Constants.PAUSE_ACTION, true);
            uiPause = inputEvent.input.FindActionMap(Constants.UI_MAP, true).FindAction(Constants.PAUSE_ACTION, true);
        }

        protected virtual void OnDestroy()
        {
            DOTween.KillAll();
        }

        public virtual void AddPauseListeners()
        {
            if (listeningForPause) return;
            listeningForPause = true;
            playerPause.performed += TogglePause;
            uiPause.performed += TogglePause;
        }

        protected virtual void RemovePauseListeners()
        {
            if (!listeningForPause) return;
            listeningForPause = false;
            if (playerPause != null)
            {
                playerPause.performed -= TogglePause;
                uiPause.performed -= TogglePause;
            }
        }

        protected void TogglePause(InputAction.CallbackContext ctx)
        {
            TogglePause();
        }

        public virtual void TogglePause()
        {
            paused = !paused;
            if (paused)
            {
                inputEvent.SwitchActionMap(Constants.UI_MAP, true);
                ShowPause();
                Time.timeScale = 0;
            }
            else
            {
                inputEvent.RevertToSceneMap();
                HidePause();
                Time.timeScale = 1;
            }
            pauseEvent.RaiseOnPause(paused);
        }

        protected virtual void ShowPause()
        {
            DOTween.Kill(pauseMenu);
            pauseMenu.FadeIn(0.3f);
        }

        protected virtual void HidePause()
        {
            DOTween.Kill(pauseMenu);
            pauseMenu.FadeOut(0.3f);
        }
    }
}