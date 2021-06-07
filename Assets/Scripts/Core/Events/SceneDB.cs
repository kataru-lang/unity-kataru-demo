using UnityEngine;
using UnityEngine.SceneManagement;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
namespace JnA.Core.ScriptableObjects
{
    [System.Serializable]
    public class SceneDictionary : SerializableDictionaryBase<SceneString, SceneData>
    {
    }

    [CreateAssetMenu(fileName = "SceneDB", menuName = "ScriptableObjects/Scenes/SceneDB", order = 1)]
    public class SceneDB : ScriptableObject
    {
        public Action<SceneData, Action, bool> OnPrepLoad;
        public event Action<string, SceneData> OnLoaded;
        public Action OnUnloaded;

        public Action<Action> OnSceneMirrorLoad;

        private string currentScene;

        [SerializeField, DrawKeyAsProperty] SceneDictionary sceneDictionary;

        const bool DEFAULT_SAVE_CURRENT_SCENE = false;

        public void Prepare()
        {
            currentScene = null;
        }

        // Load mirror scene - play player's stepping into mirror animation and post processing wave effect
        public void LoadSceneMirror(string sceneKey)
        {
            OnSceneMirrorLoad?.Invoke(() => LoadScene(sceneKey, true));
        }

        //Load scene
        public void LoadScene(string sceneKey, bool viaMirror = false)
        {
            if (currentScene == sceneKey)
                return;

            SceneData scene = sceneDictionary[new SceneString(sceneKey)];
            OnPrepLoad.Invoke(scene, () =>
            {
                string oldScene = currentScene;
                if (!string.IsNullOrEmpty(oldScene))
                    SceneManager.UnloadSceneAsync(oldScene).completed += a => OnUnloaded?.Invoke();
                currentScene = sceneKey;
                Debug.Log($"Load scene '{sceneKey}'");
                SceneManager.LoadSceneAsync(sceneKey, LoadSceneMode.Additive).completed += a =>
          {
              Debug.Log($"Loaded scene '{sceneKey}'");
              OnLoaded?.Invoke(oldScene, scene);
          };
            }, viaMirror);
        }

        public void ReloadScene(string sceneKey)
        {
            if (currentScene != sceneKey)
                return;

            SceneManager.UnloadSceneAsync(sceneKey).completed += a => { currentScene = string.Empty; LoadScene(sceneKey); };
        }

        // For testing. Assumes given scene is already loaded.
#if UNITY_EDITOR
        public void PrepareEditorScene(string sceneKey)
        {
            SceneData scene;
            if (!sceneDictionary.TryGetValue(new SceneString(sceneKey), out scene))
            {
                throw new KeyNotFoundException($"No SceneData found for scene named '{sceneKey}'. Add it to SceneDB.");
            }
            currentScene = sceneKey;
            OnPrepLoad?.Invoke(scene, () => OnLoaded?.Invoke(null, scene), false);
        }
#endif

        public SceneData GetCurrentSceneData() => sceneDictionary[new SceneString(currentScene)];
    }
}

namespace JnA.Core
{
    [System.Serializable]
    public class SceneString
    {
        [Scene] public string value;

        public SceneString(string s) { value = s; }

        //Override the Equals function to include any logic that we need to make sure that the keys are the same. This is what will be used when working with the dictionary
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SceneString)) return false;
            return ((SceneString)obj).value == value;
        }

        //Finally override the GetHashCode and include any kind of HashCode check
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(value)) return 0;
            return value.GetHashCode();
        }
    }
}