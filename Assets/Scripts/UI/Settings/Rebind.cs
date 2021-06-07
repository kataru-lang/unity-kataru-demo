using UnityEngine;
using JnA.Utils;
using UnityEngine.InputSystem;

namespace JnA.UI.Settings
{
    public class Rebind : MonoBehaviour
    {
        [SerializeField] RebindEvent rebindEvent;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] CanvasGroup waitForInput;

        private const string REBIND_KEY = "controls";

        private void Awake()
        {
            rebindEvent.OnShowWaitForInput = ShowWaitForInput;

            Load();
        }

        private void Load()
        {
            string rebinds = PlayerPrefs.GetString(REBIND_KEY, string.Empty);

            if (string.IsNullOrEmpty(rebinds)) { return; }

            // in the case of Bunnio
            if (playerInput == null)
            {
                playerInput = FindObjectOfType<PlayerInput>();
            }

            playerInput.actions.LoadFromJson(rebinds);
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

        public void Save()
        {
            string rebinds = playerInput.actions.ToJson();

            PlayerPrefs.SetString(REBIND_KEY, rebinds);
        }
    }
}