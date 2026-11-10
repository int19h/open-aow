
namespace Aow.Game.Ui.Dialogs;

// The original lists files by name each time the dialog fills its list, whichever sort button the
// player pressed last. The port remembers the last button in the settings and applies it on every
// fill, so the open and save dialogs keep the player's order between uses and between sessions.
public partial class FileDlgWindow
{
    private void RememberSortByDate(bool byDate) => Magic.Abilities.GameTables.AoWEngine.FRegistry.SetFileDialogSortByDate(byDate);

    // The sort buttons sit on the open and save pages. Mode 2 is the map picker, which has no
    // sort buttons. Restoring date order there rewrites the map name the dialog already filled in.
    private void RestoreSortOrder()
    {
        if (FMode != 0 && FMode != 1) return;
        if (Magic.Abilities.GameTables.AoWEngine.FRegistry.GetFileDialogSortByDate())
        {
            SortByDate();
        }
    }
}
