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
        [SerializeField] protected DialogueEvent dialogueEvent;

        [Space(20)]
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Image[] bubbleImages;

        string currCharacter = string.Empty;

        protected void OnEnable()
        {
            dialogueEvent.SayLine += TrySetStyle;
        }

        protected void OnDisable()
        {
            dialogueEvent.SayLine -= TrySetStyle;
        }

        void TrySetStyle(Kataru.Dialogue dialogue, Transform target)
        {
            if (dialogue.name != currCharacter)
            {
                if (styleDict.TryGetValue(dialogue.name, out DialogueStyle style))
                    SetColor(style.textColor, style.bubbleColor);
                currCharacter = dialogue.name;
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