// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// a grid of the images of an image library, FImageLibrary. The edit methods work on the
// selected cells, through the clipboard format of TCustomImageLibrary, when EditMode allows
// them
public partial class CustomImageLibraryGrid : CustomImageDrawGrid
{
    public CustomImageLibraryGrid(Component owner) : base(owner)
    { }

    internal CustomImageLibrary FImageLibrary;

    // ImageLib.pas:6934
    internal void SetImageLibrary(CustomImageLibrary Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6942
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    protected internal CustomImageLibrary ImageLibrary { get => FImageLibrary; set => SetImageLibrary(value); }

    // ImageLib.pas:6818
    public CustomImageLibrary CreateSelectedIL()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6834
    public override void InsCell()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6843
    public override void DelCell()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6852
    public override void CutCell()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6877
    public override void CopyCell()
    {
        CustomImageLibrary L;
        if (FEditMode.Contains(ILGridEditModeElement.Copy) && FImageLibrary != null)
        {
            L = CreateSelectedIL();
            try
            {
                L.CopyToClipboard();
            }
            finally
            {
                L?.Dispose();
            }
        }
    }

    // ImageLib.pas:6892
    public override void DeleteCell()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6907
    public override void PasteCell()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }
}
