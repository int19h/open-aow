using Aow.Units.AoWCheckBox;
using Aow.Units.AoWE;
using Aow.Units.AoWPanel;
using Aow.Units.AoWUnitSelDlg;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Game = Aow.Units.AoWE.AoWEUnit;

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
        if (EngineUnit.Engines is null) EngineUnit.Initialize();
        var previousEngine = Game.AoWEngine;
        Game.AoWEngine = EngineFixture.Engine;
        var dialog = new TUnitSelectionDlg(null!);
        try
        {
            // The layout (UnitSelectionDlg.Layout.cs) makes each slot a check box with this handler.
            dialog.UnitPnl = new TAOWPanel(dialog);
            dialog.U1 = new TAOWCheckBox(dialog) { OnPreDraw = (sender, surface) => dialog.U1PreDraw(sender, surface, 0) };
            dialog.FEvent = new THealingUnitSelector();
            TSurfaceDesc sd = default;
            dialog.U1.OnPreDraw(dialog.U1, sd);
        }
        finally
        {
            dialog.FEvent = null!;
            dialog.Destroy();
            Game.AoWEngine = previousEngine;
        }
    }
}
