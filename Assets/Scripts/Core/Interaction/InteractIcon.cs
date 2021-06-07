using JnA.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace JnA.Core.Interaction
{
    public enum IconType
    {
        Look,
        Talk,
    }
    public class InteractIcon : MonoBehaviour
    {
        [Expandable] [SerializeField] InteractDB interactDB;
        [SerializeField] SpriteRenderer icon;

        int tweenId;

        private void Awake()
        {
            interactDB.HideInteractIcon += Hide;
            interactDB.ShowInteractIcon += Show;

            Hide();
        }

        private void OnDestroy()
        {
            interactDB.HideInteractIcon -= Hide;
            interactDB.ShowInteractIcon -= Show;
        }

        const float ANIMATION_DURATION = 0.25f;

        internal void Hide()
        {
            DOTween.Kill(tweenId);
            tweenId = DOTween.Sequence()
             .Append(icon.DOFade(0, ANIMATION_DURATION))
             .Join(icon.transform.DOScale(Vector3.zero, ANIMATION_DURATION).SetEase(Ease.InBack))
             .OnComplete(() => icon.enabled = false).intId;
        }

        internal void Show(Transform target, IconType iconType)
        {

            icon.transform.position = target.position;
            SetIcon(iconType);
            icon.color = new Color(1, 1, 1, 0);
            icon.transform.localScale = Vector3.zero;
            icon.enabled = true;
            DOTween.Kill(tweenId);
            tweenId = DOTween.Sequence()
           .Append(icon.DOFade(1, ANIMATION_DURATION))
           .Join(icon.transform.DOScale(Vector3.one, ANIMATION_DURATION).SetEase(Ease.OutBack)).intId;
        }

        void SetIcon(IconType type)
        {
            switch (type)
            {
                case IconType.Look:
                    icon.sprite = interactDB.look;
                    break;
                case IconType.Talk:
                    icon.sprite = interactDB.talk;
                    break;
                default:
                    break;
            }
        }
    }
}