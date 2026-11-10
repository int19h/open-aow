namespace Aow.Game.Ui.Dialogs;

public partial class TSpellBook
{
    public override void Loaded()
    {
        base.Loaded();
        if (Dlg != null)
        {
            Dlg.PageBack = PrevBtn;
            Dlg.PageForward = NextBtn;
        }
    }
}
