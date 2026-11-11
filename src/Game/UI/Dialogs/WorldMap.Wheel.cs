namespace Aow.Game.UI.Dialogs;

public partial class WorldMap
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
