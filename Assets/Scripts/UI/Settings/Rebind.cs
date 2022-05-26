using UnityEngine;
using JnA.Utils;
using UnityEngine.InputSystem;

namespace JnA.UI.Settings
{
    public class Rebind : MonoBehaviour
    {
        [SerializeField] RebindEvent rebindEvent;
        [SerializeField] CanvasGroup waitForInput;

        public const string REBIND_KEY = "controls_";

        private void Awake()
        {
            rebindEvent.OnShowWaitForInput = ShowWaitForInput;
        }

        private void Start()
        {
            waitForInput.TurnOff();
        }

        public void ShowWaitForInput(bool show)
        {
            if (show)
                waitForInput.FadeIn(0.1f);
            else
                waitForInput.FadeOut(0.1f);
        }
    }
}