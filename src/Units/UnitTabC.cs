// Translated from UnitTabC.pas.
using Aow.Game.Ui.Grids;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

// selection control, unit index grid, selection event
public partial class UnitTabControl : TabControl
{
    public UnitSelectionControl FSelectionControl;
    public UnitIndexGrid FUnitIndexGrid;
    public OnSelectUnitEvent FOnSelectUnit;

    // UnitTabC.pas:42
    public override void Dispose()
    {
        /* TODO: Implement ComCtrls.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitTabC.pas:35

    public UnitTabControl(Component owner) : base(owner)
    {
        /* TODO: Implement ComCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitTabC.pas:48
    protected internal override void Change()
    {
        /* TODO: Implement ComCtrls.Change in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitTabC.pas:54
    public void SetUnitIndexGrid(UnitIndexGrid Value)
    {
        FUnitIndexGrid = Value;
        if (Value != null)
        {
            FUnitIndexGrid.OnSelectCell = UnitIndexGridSelectCell;
            FUnitIndexGrid.FSelectionControl = FSelectionControl;
        }
    }

    // UnitTabC.pas:64
    public void UnitIndexGridSelectCell(object Sender, int ACol, int ARow, ref bool CanSelect)
    {
        int Index;
        UnitIndexList List;
        Index = FUnitIndexGrid.ImageIndex(ACol, ARow);
        FUnitIndexGrid.SelectionEnabled = true;
        List = FUnitIndexGrid.FUnitIndexList;
        if (List != null)
        {
            var list = List[Index];
            FSelectionControl.SetSelectedUnitIndex(list);
        }

        if (FOnSelectUnit != null)
        {
            FOnSelectUnit(this, FSelectionControl.FSelectedUnitIndex);
        }
    }

    // UnitTabC.pas:76
    public void UpdateUnitIndexGrid()
    {
        /* TODO: Implement ComCtrls.TabIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitTabC.pas:105
    public void UpdateSelection()
    {
        /* TODO: Implement ComCtrls.Tabs in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public UnitIndexGrid UnitIndexGrid { get => FUnitIndexGrid; set => SetUnitIndexGrid(value); }

    [Published]
    public OnSelectUnitEvent OnSelectUnit { get => FOnSelectUnit; set => FOnSelectUnit = value; }

    public delegate void OnSelectUnitEvent(object Sender, int UnitIndex);
}
