using UnityEngine;
using UnityEngine.Events;
using JnA.Utils;
using JnA.Core.ScriptableObjects;
using DG.Tweening;

namespace JnA.Core.Interaction
{

    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] bool useOnce = false;
        [SerializeField] IconType iconType;
        [SerializeField] AudioClip sfxClip;
        [SerializeField] Transform target;
        [SerializeField] PlayerEvent playerEvent;
        [SerializeField] AudioEvent audioEvent;
        [SerializeField] InteractDB interactDB;
        [SerializeField] UnityEvent OnInteract;

        int tween = Constants.DEFAULT_TWEEN_ID;

        public void Enable(bool enable)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, Main.PLAYER_LAYER, enable);
        }

        public void Interact()
        {
            if (Kataru.Runner.isRunning) return;

            if (audioEvent != null && sfxClip != null) audioEvent.PlayOneShot(sfxClip);
            OnInteract.Invoke();
            Hide();

            if (useOnce)
            {
                Destroy(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
            {
                playerEvent.SetInteract(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
            {
                playerEvent.TryExitInteract(this);
            }
        }

        public void Show()
        {
            if (Kataru.Runner.isRunning) return;
            interactDB.ShowInteractIcon(target, iconType);
        }

        public void Hide()
        {
            interactDB.HideInteractIcon();
        }

        private void OnDestroy()
        {
            DOTween.Kill(tween);
        }

#if UNITY_EDITOR
        // UnityEvent listener, for testing OnInteract
        public void DebugLog(string s)
        {
            Debug.Log(s);
        }
#endif
    }
}
