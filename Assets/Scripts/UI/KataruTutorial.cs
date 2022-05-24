using Kataru;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using JnA.Utils;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using JnA.Core.ScriptableObjects;
using System.Collections;

namespace JnA.UI
{
    [System.Serializable]
    public class Tutorial
    {
        public Sprite guide;
        public InputActionReference action;

        public bool completed;
    }

    [System.Serializable]
    public class TutorialDictionary : SerializableDictionaryBase<string, Tutorial> { }

    [System.Serializable]
    class TutorialSaveData
    {
        public Dictionary<string, bool> completed = new Dictionary<string, bool>();

        public TutorialSaveData(TutorialDictionary tutorials)
        {
            if (tutorials == null) return;
            foreach (string k in tutorials.Keys)
            {
                completed[k] = tutorials[k].completed;
            }
        }
    }

    /// <summary>
    /// Controls in-game tutorials
    /// Assumption: belongs on a root level Canvas whose children is only relevant for tutorials
    /// So that when we destroy, it doesn't destroy other important stuff
    /// </summary>
    public class KataruTutorial : Handler
    {
        private const string SAVE_PATH = "tutorial.json";
        [SerializeField] TutorialDictionary tutorials;
        [SerializeField] InputEvent inputEvent;
        [SerializeField] Image image;

        InputAction currentAction;

        bool readyToDestroy = true;

        private void Start()
        {
            Load();
            TryDestroy();
        }

        private string GetPath() => Path.Combine(Application.persistentDataPath, SAVE_PATH);

        void Save()
        {
            TutorialSaveData saveData = new TutorialSaveData(tutorials);
            System.IO.File.WriteAllText(GetPath(), JsonConvert.SerializeObject(saveData));
        }

        void Load()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                TutorialSaveData data = JsonConvert.DeserializeObject<TutorialSaveData>(System.IO.File.ReadAllText(path));
                foreach (string k in tutorials.Keys)
                {
                    tutorials[k].completed = data.completed[k];
                }
            }
        }

        [CommandHandler(autoNext: false)]
        void ShowTutorial(string key, bool wait)
        {
            if (tutorials.TryGetValue(key, out Tutorial tutorial))
            {
                if (!tutorial.completed)
                {
                    image.sprite = tutorial.guide;
                    image.SetNativeSize();
                    DOTween.Kill(image);
                    image.DOFade(1, 0.3f);
                    currentAction = inputEvent.input.FindActionMap(JnA.Utils.Constants.PLAYER_MAP, true).FindAction(key, true);
                    if (wait)
                    {
                        inputEvent.RevertToSceneMap();
                        currentAction.performed += RunnerNextListener;
                    }
                    else Runner.Next();
                    currentAction.performed += EndTutorial;
                }
            }
            else Runner.Next();
        }

        void RunnerNextListener(InputAction.CallbackContext ctx)
        {
            currentAction.performed -= RunnerNextListener;
            StartCoroutine(DelayNext());
        }

        private IEnumerator DelayNext()
        {
            readyToDestroy = false;
            yield return new WaitForSeconds(1f);
            inputEvent.SwitchActionMap(JnA.Utils.Constants.UI_MAP, true);
            Runner.Next();
            readyToDestroy = true;
        }

        private void EndTutorial(InputAction.CallbackContext ctx)
        {
            currentAction.performed -= EndTutorial;
            image.DOFade(0, 0.6f);
            tutorials[currentAction.name].completed = true;
            Save();
            TryDestroy();
        }

        void TryDestroy()
        {
            foreach (Tutorial t in tutorials.Values)
            {
                if (!t.completed)
                {
                    return;
                }
            }
            StartCoroutine(RunTryDestroy());
        }

        IEnumerator RunTryDestroy()
        {
            yield return new WaitUntil(() => readyToDestroy);
            Destroy(gameObject);
        }
    }
}