using UnityEngine.Events;

namespace JnA.Utils
{
    [System.Serializable]
    public class UnityIntEvent : UnityEvent<int>
    {
    }

    public static class BunnioConstants
    {
        public const string BUNNIO_MAP = "Bunnio";

        public const string BUNNIO_SCENE = "Bunnio";

        public const string PLANT_LAYER = "Plant";
    }

    public static class Constants
    {

        #region INPUT
        public const string PLAYER_MAP = "Player",
                            UI_MAP = "UI",
                            DANCEOFF_MAP = "DanceOff";

        public const string PAUSE_ACTION = "Pause",
                            POINT_ACTION = "Point",
                            CLICK_ACTION = "Click",
                            INTERACT_ACTION = "Interact";
        #endregion

        #region PHYSICS
        public const float DELAY_COLLIDER_SEC = 0.2f;
        public const string HITBOX_LAYER = "Hitbox";
        #endregion

        #region METADATA
        // if you ever were to change the name of this scene, make sure to update its name here
        public const string CORE_SCENE = "Core",
                            START_SCENE = "StartMenu";

        public const string UI_CAMERA_TAG = "UICamera";
        public const string PLAYER_TAG = "Player";
        #endregion

        #region STORY
        public const string PLAYER_CHARACTER = "Lily";
        public const string INIT_PASSAGE = "Load"; // called upon loading the game

        #endregion

        #region ANIMATION
        public const string MIRROR_ENTER_TRIGGER = "mirrorenter";

        public const int IDLE_FRONT = 0,
            IDLE_BACK = 1,
            IDLE_SIDE = 2;

        public const int DEFAULT_TWEEN_ID = -1;

        #endregion
    }
}