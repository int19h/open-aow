
namespace Aow.Game;

public partial class AoWRegistry
{
    // A port setting the original does not have: the file dialogs' last sort order.
    internal bool GetFileDialogSortByDate()
    {
        bool byDate = false;
        rwBool("OpenAoW", "File Dialog Sort By Date", ref byDate, ReadWriteMode.Read);
        return byDate;
    }

    internal void SetFileDialogSortByDate(bool byDate) =>
        rwBool("OpenAoW", "File Dialog Sort By Date", ref byDate, ReadWriteMode.Write);
}
