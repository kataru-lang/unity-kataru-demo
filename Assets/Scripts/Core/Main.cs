using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using System;
using UnityEngine.UI;
using NaughtyAttributes;
using System.IO;
using Kataru;
using System.Linq;

namespace JnA.Core
{
    /// <summary>
    // assumed to be present for the entirety of the game
    // manages things that must be done across game:
    // - pause screen
    // - loading screen
    // - adding inputEvent, pauseEVent listeners
    /// </summary>
    public class Main : MainBase
    {
        [Header("Scene Management")]
        [SerializeField] SceneDB sceneDatabase;
        [SerializeField] LoadEvent loadEvent;
        [SerializeField] PlayerEvent playerEvent;

        [BoxGroup("Input")] [SerializeField] PlayerInput playerInput;

        public static int PLAYER_LAYER;

        int uiRequests;

        [Button]
        public static void DeleteAll()
        {
            string path = Application.persistentDataPath;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        [Button]
        void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();

        }

        protected override void Awake()
        {
            base.Awake();
            pauseEvent.EnablePause += EnablePauseListeners;

            PLAYER_LAYER = LayerMask.NameToLayer("Player");

            inputEvent.SwitchActionMap += TrySwitchActionMap;
            inputEvent.EnableActionMap += EnableActionMap;
            inputEvent.RevertToSceneMap += RevertToSceneMap;
            inputEvent.GetActionMap += () => playerInput.currentActionMap.name;


            sceneDatabase.Prepare();
            sceneDatabase.OnPrepLoad += OnPrepLoadScene;
            sceneDatabase.OnLoaded += OnLoadedScene;
        }

        private void TrySwitchActionMap(string map)
        {
            if (!TryRequestActionMap(map)) return;
            playerInput.SwitchCurrentActionMap(map);
        }

        private bool TryRequestActionMap(string map)
        {
            if (Kataru.Runner.isRunning)
            {
                Debug.Log($"Action map request {map} rejected: Kataru currently running");
                return false;
            }

            Debug.Log($"Try request action map {map} with uiRequests: {uiRequests} and with current action map: {playerInput.currentActionMap.name}");
            if (map == Constants.UI_MAP) ++uiRequests;
            else if (uiRequests > 0)
            {
                if (playerInput.currentActionMap.name == Constants.UI_MAP) --uiRequests;
                if (uiRequests > 0) return false;
            }
            return true;
        }

        private void EnableActionMap(string map, bool enable)
        {
            InputActionMap m = playerInput.actions.FindActionMap(map);
            if (enable)
                m.Enable();
            else
                m.Disable();
        }

        void Start()
        {
#if UNITY_EDITOR
            // check what the current loaded scene is
            bool foundScene = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                string n = SceneManager.GetSceneAt(i).name;
                if (n != Constants.CORE_SCENE)
                {
                    foundScene = true;
                    sceneDatabase.PrepareEditorScene(n);
                    if (n != Constants.START_SCENE)
                    {
                        Runner.Load();
                        Runner.RunPassage(Constants.INIT_PASSAGE);
                        EnablePauseListeners(n != BunnioConstants.BUNNIO_SCENE);
                    }
                    break;
                }
            }
            if (!foundScene)
            {
                sceneDatabase.LoadScene(Constants.START_SCENE);
            }
#else
            sceneDatabase.LoadScene(Constants.START_SCENE);
#endif
        }

        public static InputControl GetInputControl(InputAction action)
        {
            return action.activeControl ?? action.controls[0];
        }


        void EnablePauseListeners(bool enable)
        {
            if (enable)
            {
                AddPauseListeners();
            }
            else
            {
                RemovePauseListeners();
            }
        }

        void OnPrepLoadScene(SceneData scene, Action onComplete, bool viaMirror)
        {
            if (!viaMirror)
            {
                // switch scene map immediately if not travelling via mirror
                sceneDatabase.OnLoaded += SwitchSceneMapHandler;
            }
            if (viaMirror)
            {
                loadEvent.OnLoad.Invoke(false, Color.white, onComplete);
            }
            else if (scene.showLoad)
            {
                loadEvent.OnLoad.Invoke(scene.showLoadArt, scene.loadColor, onComplete);
            }
            else
            {
                onComplete();
            }
        }

        void OnLoadedScene(string oldScene, SceneData scene)
        {
            if (scene.showLoad)
                loadEvent.OnDoneLoading();
        }

        private void SwitchSceneMapHandler(string oldScene, SceneData scene)
        {
            sceneDatabase.OnLoaded -= SwitchSceneMapHandler;
            SwitchSceneMap(scene);
        }

        private void SwitchSceneMap(SceneData scene)
        {
            TrySwitchActionMap(scene.actionMap);
        }

        private void RevertToSceneMap()
        {
            SceneData sceneData = sceneDatabase.GetCurrentSceneData();
            // when reverting, we want to deduct from uiRequests no matter what, 
            // because reverting should only happen when we're done with the switch
            if (uiRequests > 0) --uiRequests;
            string mapName = sceneData.actionMap;
            if (mapName == Constants.UI_MAP || uiRequests == 0)
                playerInput.SwitchCurrentActionMap(mapName);
            // TrySwitchActionMap(sceneData.closeup ? Constants.UI_MAP : sceneData.actionMap);
        }

        public void TryShowPause()
        {
            if (!paused) return;
            DOTween.Kill(pauseMenu);
            pauseMenu.FadeIn(0.3f);
        }
    }
}