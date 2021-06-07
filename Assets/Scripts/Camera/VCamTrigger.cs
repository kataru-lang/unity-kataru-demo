using JnA.Core;
using JnA.Core.ScriptableObjects;
using UnityEngine;
using NaughtyAttributes;
using JnA.Utils;
using System.Collections;

namespace JnA.Cam
{
    /// <summary>
    /// Change vCamera params on player trigger or enter
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class VCamTrigger : MonoBehaviour
    {
        // use only once - on enter
        [SerializeField] bool useOnce = false;
        [SerializeField] VCamEvent vCamEvent;
        [Expandable] [SerializeField] VCamData data;


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
                vCamEvent.ChangeData.Invoke(data);
                if (useOnce) Destroy(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
            {
                vCamEvent.RevertData.Invoke();
            }
        }
    }
}