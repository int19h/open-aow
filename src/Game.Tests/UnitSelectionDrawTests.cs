
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UnitSelectionDrawTests
{
    // AoWUnitSelDlg.pas 146 reads the unit slot's position through an unchecked TAOWImage cast, and
    // the slots are check boxes. A checked cast here once broke every frame of the healing target
    // dialog with "Exception during Draw".
    [Fact]
    public void UnitSlotsDrawThroughTheirOwnComponentType()
    {
        GameEngine previousEngine = GameTables.GameEngine;
        GameTables.GameEngine = EngineFixture.Engine;
        var dialog = new UnitSelectionDialog(null!);
        try
        {
            // The layout (UnitSelectionDialog.Layout.cs) makes each slot a check box with this handler.
            dialog.UnitPanel = new Panel(dialog);
            dialog.Unit1CheckBox = new CheckBox(dialog);
            dialog.Unit1CheckBox.OnPreDraw += (sender, surface) => dialog.U1PreDraw(sender, surface, 0);
            dialog.Event = new HealingUnitSelector();
            LockedSurface lockedSurface = default;
            // The check box raises the base event, as its PreDraw does.
            EventFields.Handler<AoWOnDrawEvent<UIComponent>>(dialog.Unit1CheckBox, declaring: typeof(UIComponent), name: nameof(UIComponent.OnPreDraw))!(dialog.Unit1CheckBox, lockedSurface);
        }
        finally
        {
            dialog.Event = null!;
            dialog.Dispose();
            GameTables.GameEngine = previousEngine;
        }
    }
}
