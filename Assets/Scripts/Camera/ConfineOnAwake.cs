using JnA.Core;
using JnA.Core.ScriptableObjects;
using UnityEngine;


namespace JnA.Cam
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class ConfineOnAwake : MonoBehaviour
    {
        [SerializeField] VCamEvent vCamEvent;

        private void Awake()
        {
            vCamEvent.ChangeConfiner(GetComponent<PolygonCollider2D>());
        }
    }
}