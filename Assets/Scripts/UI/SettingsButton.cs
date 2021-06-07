using JnA.Core;
using JnA.UI.Settings;
using JnA.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace JnA.UI
{
    [RequireComponent(typeof(Button))]
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] SettingsEvent settingsEvent;
        [SerializeField] CanvasGroupSelect fadeOnRaise;

        public void Settings()
        {
            fadeOnRaise.FadeOut();
            settingsEvent.OnOpenSettings(() =>
            {
                fadeOnRaise.FadeIn();
            });
        }
    }
}