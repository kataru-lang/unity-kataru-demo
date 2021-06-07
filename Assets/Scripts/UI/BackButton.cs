using System.Collections;
using System.Collections.Generic;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JnA.UI
{
    /// <summary>
    /// if there's a back button present, allow triggering it by pressing Cancel
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class BackButton : MonoBehaviour
    {
        [SerializeField] InputEvent inputEvent;
        InputAction uiPause;
        Button button;

        private void Awake()
        {
            uiPause = inputEvent.input.FindActionMap(Constants.UI_MAP, true).FindAction(Constants.PAUSE_ACTION, true);
            button = GetComponent<Button>();
        }

        public void Enable()
        {
            uiPause.performed += GoBack;
        }

        public void Disable()
        {
            uiPause.performed -= GoBack;
        }

        public void GoBack(InputAction.CallbackContext ctx)
        {
            button.onClick.Invoke();
        }
    }
}