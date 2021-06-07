using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using JnA.Utils;

namespace JnA.UI
{
    /// <summary>
    /// Raise event for link tags
    /// Change color of links depending on hover state
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLink : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] TMPLinkEvent linkEvent;
        private TextMeshProUGUI text;
        private Camera main;

        readonly Color32 linkColor = new Color32(0, 0, 238, 255),
           clickedLinkColor = new Color32(100, 0, 238, 255);

        private void Start()
        {
            main = Functions.GetCamera(Cameras.Main);
            text = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, main);
            if (linkIndex != -1)
            {
                // was a link clicked?
                TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
                string id = linkInfo.GetLinkID();
                if (!string.IsNullOrEmpty(id))
                {
                    SetLinkToColor(linkIndex, clickedLinkColor);
                    linkEvent.OnLinkClicked?.Invoke(id);
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        List<Color32[]> SetLinkToColor(int linkIndex, Color32 color)
        {
            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];

            var oldVertColors = new List<Color32[]>(); // store the old character colors

            for (int i = 0; i < linkInfo.linkTextLength; i++)
            { // for each character in the link string
                int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
                var charInfo = text.textInfo.characterInfo[characterIndex];
                int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
                int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

                Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32; // the colors for this character
                oldVertColors.Add(vertexColors.ToArray());

                if (charInfo.isVisible)
                {
                    vertexColors[vertexIndex + 0] = color;
                    vertexColors[vertexIndex + 1] = color;
                    vertexColors[vertexIndex + 2] = color;
                    vertexColors[vertexIndex + 3] = color;
                }
            }

            // Update Geometry
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            return oldVertColors;
        }
    }
}