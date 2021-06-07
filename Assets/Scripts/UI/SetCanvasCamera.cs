using JnA.Utils;
using UnityEngine;

namespace JnA.UI
{
    [RequireComponent(typeof(Canvas))]
    public class SetCanvasCamera : MonoBehaviour
    {
        [SerializeField] Cameras type = Cameras.UI;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Functions.GetCamera(type);
        }
    }
}