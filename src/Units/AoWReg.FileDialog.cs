
namespace Aow.Game.Engine;

public partial class TAoWRegistry
{
    // A port setting the original does not have: the file dialogs' last sort order.
    internal bool GetFileDialogSortByDate()
    {
        bool byDate = false;
        rwBool("OpenAoW", "File Dialog Sort By Date", ref byDate, TReadWriteMode.rwRead);
        return byDate;
    }

    internal void SetFileDialogSortByDate(bool byDate) =>
        rwBool("OpenAoW", "File Dialog Sort By Date", ref byDate, TReadWriteMode.rwWrite);
}
