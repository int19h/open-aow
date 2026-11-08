using Aow.Rtl;

namespace Aow.Units.AoWReg;

public partial class TAoWRegistry
{
    // A port setting the original does not have: the file dialogs' last sort order.
    public bool GetFileDialogSortByDate()
    {
        bool byDate = false;
        rwBool("OpenAoW", "File Dialog Sort By Date", ref byDate, TReadWriteMode.rwRead);
        return byDate;
    }

    public void SetFileDialogSortByDate(bool byDate) =>
        rwBool("OpenAoW", "File Dialog Sort By Date", ref byDate, TReadWriteMode.rwWrite);
}
