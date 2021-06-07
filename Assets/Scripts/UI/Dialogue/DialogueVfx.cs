using Kataru;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace JnA.UI.Dialogue
{
    [System.Serializable]
    public struct DialogueStyle
    {
        public Color bubbleColor;
        public Color textColor;
    }

    [System.Serializable]
    public class StyleDictionary : SerializableDictionaryBase<string, DialogueStyle> { }

    /// <summary>
    /// Change color of dialogue depending on Kataru character
    /// </summary>
    public class DialogueVfx : MonoBehaviour
    {
        [SerializeField] protected StyleDictionary styleDict;

        [Space(20)]
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Image[] bubbleImages;

        string currCharacter = string.Empty;

        public void TrySetStyle(string character)
        {
            if (character != currCharacter)
            {
                if (styleDict.TryGetValue(character, out DialogueStyle style))
                    SetColor(style.textColor, style.bubbleColor);
                currCharacter = character;
            }
        }

        private void SetColor(Color textColor, Color bubbleColor)
        {
            text.color = textColor;
            foreach (var image in bubbleImages)
            {
                image.color = bubbleColor;
            }
        }
    }
}