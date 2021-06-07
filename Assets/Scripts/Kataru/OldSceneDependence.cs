using JnA.Core;
using JnA.Core.ScriptableObjects;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Kataru
{
    [System.Serializable]
    public class ScenePassageDictionary : SerializableDictionaryBase<SceneString, string> { }

    // depending on old scene value in OnLoaded, change referenced KataruInteractable's passage
    public class OldSceneDependence : MonoBehaviour
    {
        [SerializeField] KataruInteractable interactable;
        [SerializeField] SceneDB sceneDB;
        [SerializeField] ScenePassageDictionary scenePassages;

        private void Awake()
        {
            sceneDB.OnLoaded += OnLoaded;
        }

        void OnLoaded(string oldScene, SceneData newData)
        {
            if (scenePassages.TryGetValue(new SceneString(oldScene), out string passage))
            {
                interactable.SetPassage(passage);
            }
        }
    }
}