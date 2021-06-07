using UnityEngine.UI;
using UnityEngine;

namespace JnA.UI
{
    [RequireComponent(typeof(Button))]
    public class QuitButton : MonoBehaviour
    {
        [SerializeField] QuitEvent quitEvent;

        [SerializeField] bool saveOnQuit;

        public void Quit()
        {
            quitEvent.RaiseEvent(saveOnQuit);
        }

    }
}