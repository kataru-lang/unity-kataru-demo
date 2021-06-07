using JnA.Core;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using UnityEngine;
using NaughtyAttributes;
using JnA.UI.Settings;
using Kataru;
using UnityEngine.UI;
using System.Collections;

namespace JnA.UI
{
    public class StartMenu : MonoBehaviour
    {
        string TW_KEY = "TW";

        [Header("Start Game")]
        [SerializeField] DialogueEvent dialogueEvent;
        [Scene] [SerializeField] string startScene;
        [SerializeField] SceneDB sceneDB;
        [SerializeField] PauseEvent pauseEvent;
        [SerializeField] SettingsEvent settingsEvent;

        [Header("Other UI")]
        [SerializeField] CanvasGroupSelect aboutCanvas;
        [SerializeField] CanvasGroupSelect menuCanvas;
        [SerializeField] GameObject continueButton;

        private void Start()
        {
            // if theres not a save file, deactivate the continue button
            if (!Runner.SaveExists())
            {
                continueButton.SetActive(false);
            }
            menuCanvas.FadeIn(0.5f);
        }

        public void NewGame()
        {
            if (Runner.SaveExists())
            {
                menuCanvas.group.FadeOut(0.2f);
                settingsEvent.OnDeleteSave?.Invoke(menuCanvas, () =>
                {
                    StartCoroutine(RunDelete());
                });
            }
            else
            {
                TryLoadGame();
            }
        }

        private IEnumerator RunDelete()
        {
            Main.DeleteAll();
            yield return new WaitForSeconds(0.5f);
            TryLoadGame();
        }

        public void Continue()
        {
            TryLoadGame();
        }

        private void TryLoadGame()
        {
            Runner.Load();
            menuCanvas.group.FadeOut(0.2f);
            sceneDB.LoadScene(startScene);
            pauseEvent.EnablePause(true);
        }

        public void About()
        {
            aboutCanvas.FadeIn(0.2f);
            menuCanvas.FadeOut(0.2f);
        }

        public void QuitAbout()
        {
            aboutCanvas.FadeOut(0.2f);
            menuCanvas.FadeIn(0.2f);
        }
    }
}