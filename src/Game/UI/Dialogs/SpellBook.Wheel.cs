namespace Aow.Game.UI.Dialogs;

public partial class SpellBook
{
    public override void Loaded()
    {
        base.Loaded();
        if (Book != null)
        {
            Book.PageBack = PreviousButton;
            Book.PageForward = NextButton;
        }
    }
}
