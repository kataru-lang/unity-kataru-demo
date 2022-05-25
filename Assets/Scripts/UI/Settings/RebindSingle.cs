using JnA.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace JnA.UI.Settings
{
    public class RebindSingle : MonoBehaviour
    {
        [SerializeField] RebindEvent rebindEvent;
        [SerializeField] RebindData rebindData;
        [SerializeField] protected InputActionReference[] actions;
        [SerializeField] TextMeshProUGUI bindingText;
        [SerializeField] Image bindingIcon;

        private RebindingOperation rebind;

        virtual protected int GetBindingIndex(InputActionReference action)
        {
            return action.action.GetBindingIndexForControl(Main.GetInputControl(action.action));
        }

        public void StartRebinding()
        {
            rebindEvent.OnShowWaitForInput(true);

            // cannot rebind an action that is enabled
            bool initiallyEnabled = actions[0].action.enabled;
            if (initiallyEnabled)
            {
                actions[0].action.Disable();
            }

            rebind = actions[0].action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete(initiallyEnabled));

            rebind = FormatRebind(actions[0], rebind);

            rebind.Start();
        }

        virtual protected RebindingOperation FormatRebind(InputActionReference action, RebindingOperation op) => op;

        private void RebindComplete(bool initiallyEnabled)
        {
            InputBinding? nullableBind = rebind.action.GetBindingForControl(rebind.selectedControl);
            if (nullableBind != null)
            {
                var bind = (InputBinding)nullableBind;
                ApplyOverrideBinding(bind.overridePath);
#if UNITY_EDITOR
                Debug.Log($"Set key at {Rebind.REBIND_KEY + actions[0].name}");
#endif
                PlayerPrefs.SetString(Rebind.REBIND_KEY + actions[0].name, bind.overridePath);
            }
            rebind.Dispose();
            rebind = null;

            rebindEvent.OnShowWaitForInput(false);

            if (initiallyEnabled)
            {
                actions[0].action.Enable();
            }
        }

        public void ApplyOverrideBindingFromSave()
        {
            string overridePath = PlayerPrefs.GetString(Rebind.REBIND_KEY + actions[0].name, null);
            if (string.IsNullOrEmpty(overridePath)) return;
            ApplyOverrideBinding(overridePath);
        }

        protected void ApplyOverrideBinding(string overridePath)
        {
            for (int i = 1; i < actions.Length; i++)
            {
                InputActionReference action = actions[i];
                int bindingIndex = GetBindingIndex(action);
#if UNITY_EDITOR
                Debug.Log($"Binding {action.action.name} to index {bindingIndex} with bind path {overridePath}");
#endif
                action.action.ApplyBindingOverride(bindingIndex, overridePath);
            }
            UpdateUI(overridePath);
        }

        private void UpdateUI(string overridePath)
        {
            string currentBindingInput = GetBindingString(overridePath);

            Sprite currentDisplayIcon = rebindData.GetDeviceBindingIcon(currentBindingInput);

            if (currentDisplayIcon)
            {
                bindingText.gameObject.SetActive(false);
                bindingIcon.gameObject.SetActive(true);
                bindingIcon.sprite = currentDisplayIcon;
            }
            else if (currentDisplayIcon == null)
            {
                bindingText.gameObject.SetActive(true);
                bindingIcon.gameObject.SetActive(false);
                bindingText.text = currentBindingInput;
            }
        }

        virtual protected string GetBindingString(string overridePath)
        {
            return InputControlPath.ToHumanReadableString(overridePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}