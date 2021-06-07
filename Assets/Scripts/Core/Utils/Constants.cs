using UnityEngine.Events;

namespace JnA.Utils
{
    public static class Constants
    {

        #region INPUT
        public const string PLAYER_MAP = "Player",
                            UI_MAP = "UI";

        public const string PAUSE_ACTION = "Pause",
                            INTERACT_ACTION = "Interact";
        #endregion

        #region PHYSICS
        public const float DELAY_COLLIDER_SEC = 0.2f; // sometimes, need to delay enabling a collider because OnTriggerEnter/OnCollisionEnter gets called before other lifecycle methods
        #endregion

        #region METADATA
        // if you ever were to change the name of this scene, make sure to update its name here
        public const string CORE_SCENE = "Core",
                            START_SCENE = "StartMenu";

        public const string UI_CAMERA_TAG = "UICamera";
        public const string PLAYER_TAG = "Player";
        #endregion

        #region ANIMATION
        public const int DEFAULT_TWEEN_ID = -1;
        #endregion
    }
}