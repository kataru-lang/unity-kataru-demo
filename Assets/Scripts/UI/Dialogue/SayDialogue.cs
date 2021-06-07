using UnityEngine;
using System.Collections;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using UnityEngine.UI;
using UnityEngine.Events;

namespace JnA.UI.Dialogue
{
    public class SayDialogue : TextDialogue
    {
        [Header("Say-specific")]
        [SerializeField] Image bubble;
        [SerializeField] Image bubbleTail;
        [Space(20)]
        [SerializeField] Transform target;

        [Space(20)]
        [SerializeField] Core.ScriptableObjects.VCamEvent vCamEvent;

        #region PRIVATE
        string lastCharacter = string.Empty;

        Camera _camera;
        Camera Camera
        {
            get
            {
                if (_camera == null) _camera = transform.root.GetComponent<Canvas>().worldCamera;
                return _camera;
            }
        }

        const float BUBBLE_TAIL_ROTATION_FACTOR = 0.3f,
                    TAIL_Y_EPSILON = 5;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            vCamEvent.OnDataChanged += OnVCamDataChanged;
        }

        private void OnDestroy()
        {
            vCamEvent.OnDataChanged -= OnVCamDataChanged;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            dialogueEvent.SayLine += SayLine;
            dialogueEvent.ShowOptions += OnShowOptions;
            dialogueEvent.ThinkLine += ThinkLine;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dialogueEvent.SayLine -= SayLine;
            dialogueEvent.ShowOptions -= OnShowOptions;
            dialogueEvent.ThinkLine -= ThinkLine;
        }

        /// <summary>
        /// Say a given line.
        /// Automatically triggers fade animation if speaker has changed.
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="position"></param>
        private void SayLine(Kataru.Dialogue dialogue, Transform target)
        {
            StartCoroutine(RunSayLine(dialogue, target));
        }

        private IEnumerator RunSayLine(Kataru.Dialogue dialogue, Transform target)
        {
            yield return TrySwitchCharacter(dialogue);
            Show();
            this.target = target;
            ShowLine(dialogue.text);
        }

        IEnumerator TrySwitchCharacter(Kataru.Dialogue dialogue)
        {
            if (lastCharacter != dialogue.name)
            {
                // if it's a different character speaking
                if (!string.IsNullOrEmpty(lastCharacter))
                {
                    Hide();
                    yield return new WaitForSeconds(0.3f);
                }
                lastCharacter = dialogue.name;
            }
        }

        private void ThinkLine(Kataru.Dialogue dialogue) => Hide();

        protected override void Hide()
        {
            base.Hide();
            lastCharacter = string.Empty;
        }

        /// <summary>
        /// Make the bubble follow the top of the given target collider while remaining visible.
        /// </summary>
        void Update()
        {
            if (target != null)
            {
                KeepBubbleInScreen();
            }
        }

        void KeepBubbleInScreen()
        {
            Vector3 targetPos = Camera.WorldToScreenPoint(target.position);
            Vector3[] corners = new Vector3[4];
            bubble.rectTransform.GetWorldCorners(corners);
            Vector3 btmLeft = Camera.WorldToScreenPoint(corners[0]), topRight = Camera.WorldToScreenPoint(corners[2]);
            float width = topRight.x - btmLeft.x;
            float height = topRight.y - btmLeft.y;
            Vector2 pivot = bubble.rectTransform.pivot;
            targetPos.x = Mathf.Clamp(targetPos.x,
                // clamp to left of the screen, accounting for pivot
                width * pivot.x,
                // clamp to right of screen
                Screen.width - width * (1 - pivot.x));
            targetPos.y = Mathf.Clamp(targetPos.y,
                // clamp to bottom of screen, accounting for pivot
                height * pivot.y,
                // clamp to top of screen
                Screen.height - height * (1 - pivot.y));
            transform.position = Camera.ScreenToWorldPoint(targetPos);

            AdjustBubbleTail();
        }

        /// <summary>
        /// Rotate and position bubble's tail so that it points to off screen character
        /// Note that speed of Z rotation is determined by BUBBLE_TAIL_ROTATION_FACTOR; theres prob a better way
        /// Also, assumes margin around bubble sprite is uniform
        /// </summary>
        void AdjustBubbleTail()
        {
            Vector3 targetRot = Vector3.zero;
            RectTransform rt = bubbleTail.rectTransform;
            // if target is above by TAIL_Y_EPSILON, flip tail vertically
            if (target.position.y - transform.position.y > TAIL_Y_EPSILON)
            {
                targetRot.x = 180;
                // try to change anchor to top
                if (rt.anchorMin.y != 1)
                {
                    rt.anchorMin = new Vector2(rt.anchorMin.x, 1);
                    rt.anchorMax = new Vector2(rt.anchorMax.x, 1);
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -Mathf.Abs(rt.anchoredPosition.y));
                }
            }
            else
            {
                // try to change anchor to bottom
                if (rt.anchorMin.y != 0)
                {
                    rt.anchorMin = new Vector2(rt.anchorMin.x, 0);
                    rt.anchorMax = new Vector2(rt.anchorMax.x, 0);
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, Mathf.Abs(rt.anchoredPosition.y));
                }
            }
            targetRot.z = Mathf.Lerp(-45, 45, (target.position.x - transform.position.x) * BUBBLE_TAIL_ROTATION_FACTOR + 0.5f);
            rt.localRotation = Quaternion.Euler(targetRot);
        }
    }
}