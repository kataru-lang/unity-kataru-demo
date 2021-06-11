using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace JnA.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Interaction/Closeups/CloseupData", order = 1)]
    public class CloseupData : ScriptableObject
    {
        [Header("Visuals")]
        public VolumeProfile profile;

        [InfoBox("Allow say dialogues or not?", EInfoBoxType.Normal)]
        public bool allowSay = false;
    }
}