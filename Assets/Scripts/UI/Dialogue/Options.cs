using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Kataru;

namespace JnA.UI.Dialogue
{
    public class Options : Dialogue
    {
        [Header("Options-specific")]
        [SerializeField] Button[] optionButtons;
        [SerializeField] Slider timer;
        private const float DELAY_SECONDS = 0.15f;

        protected override void OnEnable()
        {
            base.OnEnable();
            dialogueEvent.ShowOptions += ShowOptions;
            dialogueEvent.SayLine += OnSayLine;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dialogueEvent.ShowOptions -= ShowOptions;
            dialogueEvent.SayLine -= OnSayLine;
        }

        /// <summary>
        /// Hide OptionsDialogue when SayDialogue appears.
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="collider2D"></param>
        private void OnSayLine(Kataru.Dialogue dialogue, Transform target) => Hide();

        /// <summary>
        /// Creates the option buttons and handles timer.
        /// </summary>
        /// <param name="choices"></param>
        /// <param name="collider2D"></param>
        protected virtual void ShowOptions(Kataru.Choices choices, Transform target)
        {
            Show();
            DeactivateAllOptions();
            for (int i = 0; i < choices.choices.Count; ++i)
            {
                SetOption(i, choices.choices[i]);
            }

            StartCoroutine(HighlightFirstChoice());

            bool isTimed = choices.timeout > 0;
            timer?.gameObject.SetActive(isTimed);
            if (isTimed)
            {
                StartCoroutine(DoRunTimer((float)choices.timeout));
            }
        }

        private IEnumerator HighlightFirstChoice()
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForSeconds(0.3f);
            // highlight first choice
            optionButtons[0].Select();
        }

        /// <summary>
        /// Sets up a dialogue option button.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="choice"></param>
        public void SetOption(int i, string choice)
        {
            Debug.Log($"Setting up option {choice}");
            if (i >= optionButtons.Length)
            {
                Debug.LogError("There are more options to present than there are options: " + choice);
                return;
            }

            Button button = optionButtons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OptionOnClick(choice, button));

            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = choice;
            button.gameObject.SetActive(true);
        }

        /// <summary>
        /// Callback for clicking on a dialogue option button.
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="clicked"></param>
        private void OptionOnClick(string choice, Button clicked)
        {
            // Don't allow early keyboard clicks in case we're still in the middle of showing options
            AnimatorStateInfo a = animator.GetCurrentAnimatorStateInfo(0);
            if (a.length * a.normalizedTime < DELAY_SECONDS)
            {
                return;
            }

            foreach (var button in optionButtons)
            {
                if (button == clicked) continue;
                button.gameObject.SetActive(false);
            }
            dialogueEvent.PickChoice(choice);
            Hide();
        }

        IEnumerator DoRunTimer(float seconds)
        {
            timer.maxValue = seconds;
            timer.value = 0;

            var frame = new WaitForEndOfFrame();

            while (timer.value < seconds)
            {
                timer.value += UnityEngine.Time.deltaTime;
                // if selected option before timer ran out
                if (!IsActive())
                {
                    yield break; // exit coroutine prematurely
                }
                yield return frame;
            }

            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
            if (currentSelected != null && currentSelected.transform.IsChildOf(transform))
            {
                ClickButton(currentSelected);
            }
            else
            {
                ClickButton(optionButtons[0].gameObject);
            }
        }

        private void ClickButton(GameObject click)
        {
            BaseEventData data = new BaseEventData(EventSystem.current);
            ExecuteEvents.Execute(click, data, ExecuteEvents.submitHandler);
        }

        public void DeactivateAllOptions()
        {
            foreach (var button in optionButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        protected override bool ShouldHide(LineTag tag) => tag != LineTag.Choices;
    }
}