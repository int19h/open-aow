using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// A data module of the game's interface. It reads the interface graphics, the fonts and the other
// modules through the context that created it. Its owner is null, as Application owned the
// Delphi modules.
public abstract class UIModule(UIContext ui) : DataModule(null)
{
    public UIContext UI { get; } = ui;
}
