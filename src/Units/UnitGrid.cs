// Translated from UnitGrid.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.ImageLib;
using Aow.Units.AoWE;

namespace Aow.Units.UnitGrid;

// FSelectionControl, FUnits, FEditEnabled, FShowUnitLevel, FMemoryStream
public partial class TUnitGrid : TCustomImageDrawGrid
{
    public TUnitSelectionControl FSelectionControl;
    public TUnitList FUnits;
    public bool FEditEnabled;
    public bool FShowUnitLevel;
    public TMemoryStream FMemoryStream;
    // UnitGrid.pas:60
    public override void Destroy()
    {
        /* TODO: Implement Grids.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:50
    // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
    public TUnitGrid(TComponent owner) : base(owner)
    {

        FMemoryStream = new TMemoryStream();
        RowCount = 1;
        ColCount = 8;
        DefaultColWidth = 48;
        DefaultRowHeight = 48;
    }

    // UnitGrid.pas:98
    protected internal override void DrawCell(int ACol, int ARow, TRect ARect, TSet<TGridDrawState> AState)
    {
        /* TODO: Implement Graphics.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:67
    public void SetShowUnitLevel(bool Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:73
    public void SetUnits(TUnitList Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:81
    public TAbstractUnit GetUnit(int ACol, int ARow)
    {
        TAbstractUnit Result;
        int Index;
        Index = ImageIndex(ACol, ARow);
        // { Original leaves negative indices unchecked in both getters. }
        if (FUnits != null && Index < FUnits.Count)
        {
            Result = FUnits.GetUnit(Index);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // UnitGrid.pas:90
    public TAbstractUnit GetSelectedUnit()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:133
    public void AddUnit()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:169
    public void DeleteUnit()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:184
    public void EditUnitDone(TEditResult EditResult, TEObject Obj)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:196
    protected internal override void DblClick()
    {
        /* TODO: Implement Forms.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public bool ShowUnitLevel { get => FShowUnitLevel; set => SetShowUnitLevel(value); }

    [Published]
    public bool EditEnabled { get => FEditEnabled; set => FEditEnabled = value; }
}

public static partial class UnitGrid
{
    // UnitGrid.pas:207
    public static void Register()
    {
        RegisterComponents("AoWEngine", [typeof(TUnitGrid)]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
