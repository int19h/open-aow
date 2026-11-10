namespace Aow.Game.Ui.Dialogs;

public partial class TWorldMap
{
    public override void Loaded()
    {
        base.Loaded();
        if (BookWin != null)
        {
            BookWin.PageBack = Back;
            BookWin.PageForward = Next;
        }
    }
}
