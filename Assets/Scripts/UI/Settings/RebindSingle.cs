using JnA.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace JnA.UI.Settings
{
    public class RebindSingle : MonoBehaviour, ISettingsSetter
    {
        [SerializeField] RebindEvent rebindEvent;
        [SerializeField] RebindData rebindData;
        [SerializeField] protected InputActionReference[] actions;
        [SerializeField] TextMeshProUGUI bindingText;
        [SerializeField] Image bindingIcon;

        private RebindingOperation rebind;


        private void Start()
        {
            Load();
        }

        public void Prepare() { }

        public void Load()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            string currentBindingInput = GetBindingString(actions[0]);

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

        virtual protected string GetBindingString(InputActionReference action)
        {
            int controlBindingIndex = action.action.GetBindingIndexForControl(Main.GetInputControl(action.action));
            string currentBindingInput = InputControlPath.ToHumanReadableString(action.action.bindings[controlBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            if (currentBindingInput == "Escape") return "Esc";
            return currentBindingInput;
            // return action.action.GetBindingDisplayString();
        }


        virtual protected int GetBindingIndex(InputActionReference action)
        {
            return action.action.GetBindingIndexForControl(Main.GetInputControl(action.action));
        }

        public void Sync()
        {
            Load();
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
                for (int i = 1; i < actions.Length; i++)
                {
                    InputActionReference action = actions[i];
                    int bindingIndex = GetBindingIndex(action);
                    Debug.Log($"Binding {action.action.name} to index {bindingIndex} with bind {bind.name}");
                    action.action.ApplyBindingOverride(bindingIndex, bind);
                }
            }
            rebind.Dispose();
            rebind = null;

            UpdateUI();

            rebindEvent.OnShowWaitForInput(false);

            if (initiallyEnabled)
            {
                actions[0].action.Enable();
            }
        }
    }
}