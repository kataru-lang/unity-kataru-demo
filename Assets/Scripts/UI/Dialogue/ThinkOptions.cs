
namespace JnA.UI.Dialogue
{
    /// <summary>
    /// If you want choices to be displayed in the think panel:
    /// - You can use this to differentiate when SayOptions or ThinkOptions are enabled
    /// - On ThinkDialogue.OnShowOptions, make sure to hide based on whether or not the shown options are in the think panel or not
    /// </summary>
    public class ThinkOptions : Options
    {
    }
}