// Translated from UnitTabC.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.UnitIGrd;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.UnitTabC;

// selection control, unit index grid, selection event
public partial class TUnitTabControl : TTabControl
{
    public TUnitSelectionControl FSelectionControl;
    public TUnitIndexGrid FUnitIndexGrid;
    public TOnSelectUnitEvent FOnSelectUnit;

    // UnitTabC.pas:42
    public override void Destroy()
    {
        /* TODO: Implement ComCtrls.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitTabC.pas:35

    public TUnitTabControl(TComponent owner) : base(owner)
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
    public void SetUnitIndexGrid(TUnitIndexGrid Value)
    {
        FUnitIndexGrid = Value;
        if (Value != null)
        {
            FUnitIndexGrid.OnSelectCell = UnitIndexGridSelectCell;
            FUnitIndexGrid.FSelectionControl = FSelectionControl;
        }
    }

    // UnitTabC.pas:64
    public void UnitIndexGridSelectCell(TObject Sender, int ACol, int ARow, ref bool CanSelect)
    {
        int Index;
        TUnitIndexList List;
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
    public TUnitIndexGrid UnitIndexGrid { get => FUnitIndexGrid; set => SetUnitIndexGrid(value); }

    [Published]
    public TOnSelectUnitEvent OnSelectUnit { get => FOnSelectUnit; set => FOnSelectUnit = value; }
}

public delegate void TOnSelectUnitEvent(TObject Sender, int UnitIndex);

public static partial class UnitTabCUnit
{
    // UnitTabC.pas:112
    public static void Register()
    {
        RegisterComponents("AoWEngine", [typeof(TUnitTabControl)]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
