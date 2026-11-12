namespace Aow.Game.UI.Dialogs;

// The original lists files by name each time the dialog fills its list, whichever sort button the
// player pressed last. The port remembers the last button in the settings and applies it on every
// fill, so the open and save dialogs keep the player's order between uses and between sessions.
public partial class FileDialogWindow
{
    private void RememberSortByDate(bool byDate) => UI.Engine.Registry.SetFileDialogSortByDate(byDate);

    // The sort buttons sit on the open and save pages. Mode 2 is the map picker, which has no
    // sort buttons. Restoring date order there rewrites the map name the dialog already filled in.
    private void RestoreSortOrder()
    {
        if (Mode != 0 && Mode != 1) return;
        if (UI.Engine.Registry.GetFileDialogSortByDate())
        {
            SortByDate();
        }
    }
}
