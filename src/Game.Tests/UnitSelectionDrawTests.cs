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
        UnitSelectionDialog dialog = new(null!);
        try
        {
            // The layout (UnitSelectionDialog.Layout.cs) makes each slot a check box with this handler.
            dialog.UnitPanel = new Panel(dialog);
            dialog.Unit1CheckBox = new CheckBox(dialog);
            dialog.Unit1CheckBox.OnPreDraw += (sender, surface) => dialog.U1PreDraw(sender, surface, 0);
            dialog.Event = new HealingUnitSelector(EngineFixture.Engine);
            // The check box raises the base event, as its PreDraw does.
            EventFields.Handler<AoWOnDrawEvent<UIComponent>>(dialog.Unit1CheckBox, declaring: typeof(UIComponent), name: nameof(UIComponent.OnPreDraw))!(surfDesc: default, sender: dialog.Unit1CheckBox);
        }
        finally
        {
            dialog.Event = null!;
            dialog.Dispose();
        }
    }
}
