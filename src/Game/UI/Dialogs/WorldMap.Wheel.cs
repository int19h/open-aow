namespace Aow.Game.UI.Dialogs;

public partial class WorldMap
{
    public override void Loaded()
    {
        base.Loaded();
        if (Book != null)
        {
            Book.PageBack = BackButton;
            Book.PageForward = NextButton;
        }
    }
}
