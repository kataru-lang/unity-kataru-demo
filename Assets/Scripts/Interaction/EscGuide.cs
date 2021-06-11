using JnA.Core.ScriptableObjects;
using Kataru;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using JnA.Utils;
using UnityEngine.EventSystems;

namespace JnA.Interaction
{
    /// <summary>
    /// Show esc button after some period of time passes after Kataru passage ends
    /// For people who don't realize that you press esc to escape a closeup
    /// </summary>
    public class EscGuide : MonoBehaviour
    {
        [InfoBox("Make sure to add a GraphicRaycaster at the root Canvas", EInfoBoxType.Normal)]
        [SerializeField] Closeup closeup;

        [Header("Events")]
        [SerializeField] CloseupEvent closeupEvent;
        [SerializeField] DialogueEvent dialogueEvent;

        Button btn;

        float currTime = 0;

        const float timeBeforeShown = 2.5f;

        private void Start()
        {
            closeupEvent.OnShowCloseup += OnShowCloseup;
            btn = GetComponent<Button>();
            btn.onClick.AddListener(ClickBtn);
            StopRunning();
        }

        void ClickBtn()
        {
            closeup.HideCloseupListener();
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void OnDestroy()
        {
            closeupEvent.OnShowCloseup -= OnShowCloseup;
            closeupEvent.OnHideCloseup -= OnHideCloseup;
            dialogueEvent.EndDialogue -= StartRunning;
        }

        void OnShowCloseup(string goName)
        {
            btn.targetGraphic.enabled = false;
            if (goName == closeup.gameObject.name)
            {
                dialogueEvent.EndDialogue += StartRunning;
            }
        }

        // if we esc before update has finished
        void OnHideCloseup(string goName)
        {
            if (goName == closeup.gameObject.name)
            {
                StopRunning();
                closeupEvent.OnHideCloseup -= OnHideCloseup;
            }
        }

        void StartRunning()
        {
            Debug.Log("EscGuide.StartRunning");
            dialogueEvent.EndDialogue -= StartRunning;
            // if we've force exited this closeup, return
            if (!closeup.IsShowing()) return;
            currTime = 0;
            enabled = true;
            closeupEvent.OnHideCloseup += OnHideCloseup;
        }

        void StopRunning()
        {
            Debug.Log("EscGuide.StopRunning");
            enabled = false;
        }

        // update loop - if we don't escape before timeBeforeShown passes, then show esc guide
        private void Update()
        {
            currTime += Time.deltaTime;
            if (currTime >= timeBeforeShown)
            {
                StopRunning();
                btn.targetGraphic.color = Functions.ChangeColorA(btn.targetGraphic.color, 0);
                btn.targetGraphic.FadeIn(0.75f);
                btn.targetGraphic.enabled = true;
            }
        }
    }
}