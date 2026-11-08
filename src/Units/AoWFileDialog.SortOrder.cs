using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AoWFileDialog;

// The original lists files by name each time the dialog fills its list, whichever sort button the
// player pressed last. The port remembers the last button in the settings and applies it on every
// fill, so the open and save dialogs keep the player's order between uses and between sessions.
public partial class TFileDlgWindow
{
    private void RememberSortByDate(bool byDate) => AoWEngine.FRegistry.SetFileDialogSortByDate(byDate);

    private void RestoreSortOrder()
    {
        if (AoWEngine.FRegistry.GetFileDialogSortByDate())
        {
            SortByDate();
        }
    }
}
