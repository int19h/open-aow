namespace Aow.Game;

public partial class GameSettings
{
    // A port setting the original does not have: the file dialogs' last sort order.
    internal bool GetFileDialogSortByDate()
    {
        bool byDate = false;
        Boolean("OpenAoW", "File Dialog Sort By Date", ref byDate, ReadWriteMode.Read);
        return byDate;
    }

    internal void SetFileDialogSortByDate(bool byDate) =>
        Boolean("OpenAoW", "File Dialog Sort By Date", ref byDate, ReadWriteMode.Write);
}
