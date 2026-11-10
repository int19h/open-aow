// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// a grid of Count image cells. FScaleX, FScaleY, FBltMode, FEditMode, FOnCellUpdated (draws a
// cell at run time), FOnSelectCell, FFocusColor, FGrowDirection, FSelectionEnabled,
// FMinRowCount, FMinColCount, FMaxRowCount, FMaxColCount (0 for no limit), FShowGridIndex,
// FCount. Ctrl+X/C/V/Del call the edit methods, Ins and Del without a modifier InsCell and
// DelCell. they do nothing here
public partial class CustomImageDrawGrid : CustomGrid
{
    internal int FScaleX;
    internal int FScaleY;
    internal BltMode FBltMode;
    internal Set<ILGridEditModeElement> FEditMode;
    internal DrawCellEvent<object> FOnCellUpdated;
    internal SelectCellEvent<object> FOnSelectCell;
    internal int FFocusColor;
    internal ILGrowDirection FGrowDirection;
    internal bool FSelectionEnabled;
    internal int FMinRowCount;
    internal int FMinColCount;
    internal int FMaxRowCount;
    internal int FMaxColCount;
    internal bool FShowGridIndex;
    internal int FCount;

    // ImageLib.pas:6705
    internal void SetBltMode(BltMode Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6711
    internal void SetSelectionEnabled(bool Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6717
    internal void SetFocusColor(int Value)
    {
        FFocusColor = Value;
    }

    // ImageLib.pas:6722
    internal void SetScaleX(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6728
    internal void SetScaleY(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6734
    internal void SetShowGridIndex(bool Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6740
    internal int GetColCount()
    {
        /* TODO: Implement Grids.ColCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6745
    internal new void SetColCount(int Value)
    {
        /* TODO: Implement Grids.ColCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6752
    internal void SetMinRowCount(int Value)
    {
        FMinRowCount = Value;
        _ = RowCount;
    }

    // ImageLib.pas:6758
    internal void SetMinColCount(int Value)
    {
        FMinColCount = Value;
        _ = ColCount;
    }

    // ImageLib.pas:6764
    internal void SetMaxRowCount(int Value)
    {
        FMaxRowCount = Value;
        _ = RowCount;
    }

    // ImageLib.pas:6770
    internal void SetMaxColCount(int Value)
    {
        FMaxColCount = Value;
        _ = ColCount;
    }

    // ImageLib.pas:6776
    internal int GetRowCount()
    {
        /* TODO: Implement Grids.RowCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6781
    internal new void SetRowCount(int Value)
    {
        /* TODO: Implement Grids.RowCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6788
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Graphics.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6613
    protected internal override bool SelectCell(int ACol, int ARow)
    {
        bool Result;
        Result = true;
        if (FOnSelectCell != null)
        {
            FOnSelectCell(this, ACol, ARow, ref Result);
        }

        FSelectionEnabled = true;
        return Result;
    }

    // ImageLib.pas:6620
    protected internal override void KeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        /* TODO: Implement Grids.KeyDown in the platform or editor service. */
        throw new NotImplementedException();
    }

    protected internal bool SelectionEnabled { get => FSelectionEnabled; set => SetSelectionEnabled(value); }
    protected internal int FocusColor { get => FFocusColor; set => SetFocusColor(value); }
    protected internal BltMode BltMode { get => FBltMode; set => SetBltMode(value); }
    protected internal DrawCellEvent<object> OnCellUpdated { get => FOnCellUpdated; set => FOnCellUpdated = value; }
    protected internal SelectCellEvent<object> OnSelectCell { get => FOnSelectCell; set => FOnSelectCell = value; }
    protected internal int ScaleX { get => FScaleX; set => SetScaleX(value); }
    protected internal int ScaleY { get => FScaleY; set => SetScaleY(value); }
    protected internal new int ColCount { get => GetColCount(); set => SetColCount(value); }
    protected internal new int RowCount { get => GetRowCount(); set => SetRowCount(value); }
    protected internal Set<ILGridEditModeElement> EditMode { get => FEditMode; set => FEditMode = value; }
    protected internal ILGrowDirection GrowDirection { get => FGrowDirection; set => FGrowDirection = value; }
    protected internal int MinRowCount { get => FMinRowCount; set => SetMinRowCount(value); }
    protected internal int MinColCount { get => FMinColCount; set => SetMinColCount(value); }
    protected internal int MaxRowCount { get => FMaxRowCount; set => SetMaxRowCount(value); }
    protected internal int MaxColCount { get => FMaxColCount; set => SetMaxColCount(value); }
    protected internal bool ShowGridIndex { get => FShowGridIndex; set => SetShowGridIndex(value); }

    // ImageLib.pas:6594

    public CustomImageDrawGrid(Component owner) : base(owner)
    {
        /* TODO: Implement Grids.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6661
    public virtual void SetCount(int Value)
    {
        FCount = Value;
        UpdateDimensions();
    }

    // ImageLib.pas:6668
    public virtual void UpdateDimensions()
    {
        switch (FGrowDirection)
        {
            case ILGrowDirection.Horizontal:
                var rowCount = RowCount;
                var rowCount2 = RowCount;
                ColCount = (rowCount + FCount) / rowCount2;
                break;
            case ILGrowDirection.Vertical:
                var colCount = ColCount;
                var colCount2 = ColCount;
                RowCount = (colCount + FCount) / colCount2;
                break;
        }
    }

    // ImageLib.pas:6637
    public virtual void CutCell()
    {
    }

    // ImageLib.pas:6641
    public virtual void CopyCell()
    {
    }

    // ImageLib.pas:6645
    public virtual void DeleteCell()
    {
    }

    // ImageLib.pas:6649
    public virtual void PasteCell()
    {
    }

    // ImageLib.pas:6653
    public virtual void InsCell()
    {
    }

    // ImageLib.pas:6657
    public virtual void DelCell()
    {
    }

    // ImageLib.pas:6676
    public virtual int ImageIndex(int ACol, int ARow) => ColCount * ARow + ACol;

    // ImageLib.pas:6681
    public virtual void IndexToCell(int Index, ref int ACol, ref int ARow)
    {
        ACol = Index % ColCount;
        ARow = Index / ColCount;
    }

    // ImageLib.pas:6687
    public void SelectIndex(int Index)
    {
        /* TODO: Implement Grids.Left in the platform or editor service. */
        throw new NotImplementedException();
    }

    public int Count { get => FCount; set => SetCount(value); }

    public enum ILGrowDirection : byte
    {
        None = 0, Horizontal = 1, Vertical = 2
    }
}
