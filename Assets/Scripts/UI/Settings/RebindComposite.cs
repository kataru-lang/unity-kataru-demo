using JnA.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace JnA.UI.Settings
{
    public class RebindComposite : RebindSingle
    {
        [SerializeField] string partBinding;


        override protected string GetBindingString(InputActionReference a)
        {
            int controlBindingIndex = GetBindingIndex(a);
            string currentBindingInput = InputControlPath.ToHumanReadableString(a.action.bindings[controlBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            return currentBindingInput;
            // return action.action.GetBindingDisplayString();
        }

        override protected RebindingOperation FormatRebind(InputActionReference a, RebindingOperation op)
        {
            var bindingIndex = GetBindingIndex(a);
            return op.WithTargetBinding(bindingIndex).WithExpectedControlType("Button");
            // if going to adopt this to other devices, only expect "Button"if current device is keyboard; see https://forum.unity.com/threads/performinteractiverebindings-for-composite.779879/
        }

        override protected int GetBindingIndex(InputActionReference a)
        {
            int i = a.action.bindings.IndexOf((c) => compare(c, Main.GetInputControl(a.action)));
            return i;
        }

        bool compare(InputBinding x, InputControl control)
        {
            return x.groups.Contains(control.device.displayName) && x.isPartOfComposite && x.name == partBinding;
        }
    }
}