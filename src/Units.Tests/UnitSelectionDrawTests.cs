using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;

namespace Aow.Units.Tests;

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
        var previousEngine = Game.Magic.Abilities.GameTables.AoWEngine;
        Game.Magic.Abilities.GameTables.AoWEngine = EngineFixture.Engine;
        var dialog = new UnitSelectionDlg(null!);
        try
        {
            // The layout (UnitSelectionDlg.Layout.cs) makes each slot a check box with this handler.
            dialog.UnitPnl = new AOWPanel(dialog);
            dialog.U1 = new AOWCheckBox(dialog) { OnPreDraw = (sender, surface) => dialog.U1PreDraw(sender, surface, 0) };
            dialog.FEvent = new HealingUnitSelector();
            SurfaceDesc sd = default;
            dialog.U1.OnPreDraw(dialog.U1, sd);
        }
        finally
        {
            dialog.FEvent = null!;
            dialog.Dispose();
            Game.Magic.Abilities.GameTables.AoWEngine = previousEngine;
        }
    }
}
