namespace Aow.Game;

public sealed partial class GameSettings
{
    // A port setting the original does not have: the file dialogs' last sort order.
    internal bool GetFileDialogSortByDate()
    {
        bool byDate = false;
        Boolean("OpenAoW", name: "File Dialog Sort By Date", value: ref byDate, mode: ReadWriteMode.Read);
        return byDate;
    }

    internal void SetFileDialogSortByDate(bool byDate)
        => Boolean("OpenAoW", name: "File Dialog Sort By Date", value: ref byDate, mode: ReadWriteMode.Write);
}
