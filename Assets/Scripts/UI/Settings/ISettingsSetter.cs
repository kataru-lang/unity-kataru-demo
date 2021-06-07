namespace JnA.UI.Settings
{
    public interface ISettingsSetter
    {
        /// <summary>
        /// Prepare listeners
        /// </summary>
        void Prepare();

        /// <summary>
        /// Load saved values, if any
        /// </summary>
        void Load();

        /// <summary>
        /// Sync ui without raising listeners
        /// </summary>
        void Sync();
    }
}