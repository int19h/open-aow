namespace Aow.Game.UI.Dialogs;

public partial class SpellBook
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
