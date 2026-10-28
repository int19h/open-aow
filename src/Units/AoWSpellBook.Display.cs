namespace Aow.Units.AoWSpellBook;

public partial class TSpellBook
{
    // AoWSpellBook.pas:385. The host surfaces reside in system memory.
    public void ShowBook()
    {
        AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }
}
