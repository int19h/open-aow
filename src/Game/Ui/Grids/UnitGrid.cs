// Translated from UnitGrid.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Ui.Grids;

// FSelectionControl, FUnits, FEditEnabled, FShowUnitLevel, FMemoryStream
public partial class UnitGrid : CustomImageDrawGrid
{
    public UnitSelectionControl FSelectionControl;
    public UnitList FUnits;
    public bool FEditEnabled;
    public bool FShowUnitLevel;
    public MemoryStream FMemoryStream;

    // UnitGrid.pas:60
    public override void Dispose()
    {
        /* TODO: Implement Grids.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:50
    // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
    public UnitGrid(Component owner) : base(owner)
    {

        FMemoryStream = new MemoryStream();
        RowCount = 1;
        ColCount = 8;
        DefaultColWidth = 48;
        DefaultRowHeight = 48;
    }

    // UnitGrid.pas:98
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
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
    public void SetUnits(UnitList Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitGrid.pas:81
    public AbstractUnit GetUnit(int ACol, int ARow)
    {
        AbstractUnit Result;
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
    public AbstractUnit GetSelectedUnit()
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
    public void EditUnitDone(EditResult EditResult, EObject Obj)
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
