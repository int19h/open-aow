using Aow.Engine.Nodes;

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
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine previousEngine = GameTables.AoWEngine;
        GameTables.AoWEngine = EngineFixture.Engine;
        var dialog = new UnitSelectionDialog(null!);
        try
        {
            // The layout (UnitSelectionDialog.Layout.cs) makes each slot a check box with this handler.
            dialog.UnitPnl = new Panel(dialog);
            dialog.U1 = new CheckBox(dialog) { OnPreDraw = (sender, surface) => dialog.U1PreDraw(sender, surface, 0) };
            dialog.FEvent = new HealingUnitSelector();
            LockedSurface sd = default;
            dialog.U1.OnPreDraw(dialog.U1, sd);
        }
        finally
        {
            dialog.FEvent = null!;
            dialog.Dispose();
            GameTables.AoWEngine = previousEngine;
        }
    }
}
