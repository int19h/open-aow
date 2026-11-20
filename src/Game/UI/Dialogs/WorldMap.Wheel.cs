namespace Aow.Game.UI.Dialogs;

public sealed partial class WorldMap
{
    public override void Loaded()
    {
        base.Loaded();
        if (Book is not null)
        {
            Book.PageBack = BackButton;
            Book.PageForward = NextButton;
        }
    }
}
