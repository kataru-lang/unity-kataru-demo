using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Scenes/SceneData", order = 1)]
    public class SceneData : ScriptableObject
    {
        [Header("Loading")]
        [InfoBox("Show loading screen when loading this scene?", EInfoBoxType.Normal)]
        public bool showLoad;
        [ShowIf("showLoad")] public Color loadColor = Color.black;
        [ShowIf("showLoad")] public bool showLoadArt;


        [Space(10)]
        public string actionMap;


        [Space(10)]

        [Header("Audio")]
        public AudioClip[] clips;
        public AudioMixerSnapshot snapshot;


        [Space(10)]

        [Header("Vfx")]
        public VolumeProfile profile;


        [Space(10)]

        [Header("Camera")]
        public VCamData cameraData;
        public Color cameraBgColor = Color.white;
    }
}