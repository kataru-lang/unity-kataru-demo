using UnityEngine;
using JnA.Utils;
using System.Collections;
using NaughtyAttributes;

namespace JnA.Vfx
{
    /// <summary>
    /// Change vCamera params on player trigger or enter
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class GlobalLightTrigger : MonoBehaviour
    {
        [InfoBox("Note: There must exist some light with a Light2DController", EInfoBoxType.Normal)]
        // use only once - on enter
        [SerializeField] bool useOnce = false;
        [SerializeField] Color targetColor;
        [SerializeField] float duration = 3f;
        [SerializeField] Light2DEvent light2DEvent;

        // turn off collider initially; 
        // turn back on in start so that OnTriggerEnter2D won't get called before SceneDB.OnLoaded
        private void OnValidate()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(Constants.DELAY_COLLIDER_SEC);
            GetComponent<Collider2D>().enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
            {
                light2DEvent.SetGlobalLight(targetColor, duration);
                if (useOnce) Destroy(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
            {
                light2DEvent.RevertGlobalLight(duration);
            }
        }
    }
}