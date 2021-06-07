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
        public Action<SceneData, Action> OnPrepLoad;
        public event Action<string, SceneData> OnLoaded;
        public Action OnUnloaded;

        private string currentScene;
        [SerializeField, DrawKeyAsProperty] SceneDictionary sceneDictionary;
        const bool DEFAULT_SAVE_CURRENT_SCENE = false;

        public void Prepare()
        {
            currentScene = null;
        }

        //Load scene
        public void LoadScene(string sceneKey)
        {
            if (currentScene == sceneKey)
                return;

#if UNITY_EDITOR
            if (!sceneDictionary.ContainsKey(new SceneString(sceneKey)))
            {
                Debug.LogError($"Scene {sceneKey} is not in sceneDB. Please add it to sceneDB.");
            }
#endif

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
            });
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
            OnPrepLoad?.Invoke(scene, () => OnLoaded?.Invoke(null, scene));
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