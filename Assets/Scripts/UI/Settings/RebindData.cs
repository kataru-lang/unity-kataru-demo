using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace JnA.UI.Settings
{
    [System.Serializable]
    public class InputIconDictionary : SerializableDictionaryBase<string, Sprite> { }

    [CreateAssetMenu(fileName = "RebindData", menuName = "ScriptableObjects/UI/Settings/RebindData", order = 1)]
    public class RebindData : ScriptableObject
    {
        [SerializeField] InputIconDictionary icons;

        public Sprite GetDeviceBindingIcon(string binding)
        {
            Sprite displaySpriteIcon = null;
            icons.TryGetValue(binding, out displaySpriteIcon);
            return displaySpriteIcon;
        }
    }
}