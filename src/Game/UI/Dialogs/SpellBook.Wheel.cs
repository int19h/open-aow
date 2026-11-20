namespace Aow.Game.UI.Dialogs;

public sealed partial class SpellBook
{
    public override void Loaded()
    {
        base.Loaded();
        if (Book is not null)
        {
            Book.PageBack = PreviousButton;
            Book.PageForward = NextButton;
        }
    }
}
