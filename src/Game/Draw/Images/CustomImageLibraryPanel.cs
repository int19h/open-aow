// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// { Splits Text into lines no wider than MaxWidth pixels in Font, at spaces and line breaks, and
//   adds them to Strings. }
// a panel that shows one image of an image library with BltLibraryImage. FScaleX, FScaleY
// (percent), FBltMode, FOnUpdated (called after every Paint), FImageLibrary, FImageIndex
public partial class CustomImageLibraryPanel : CustomPanel
{
    internal int FScaleX;
    internal int FScaleY;
    internal BltMode FBltMode;
    internal NotifyEvent<object> FOnUpdated;
    internal CustomImageLibrary FImageLibrary;
    internal int FImageIndex;

    // ImageLib.pas:6513
    internal void SetBltMode(BltMode Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6519
    internal void SetScaleX(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6525
    internal void SetScaleY(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6531
    internal void SetImageIndex(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6537
    internal void SetImageLibrary(CustomImageLibrary Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6543
    protected internal override void Paint()
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    protected internal CustomImageLibrary ImageLibrary { get => FImageLibrary; set => SetImageLibrary(value); }
    protected internal int ImageIndex { get => FImageIndex; set => SetImageIndex(value); }
    protected internal BltMode BltMode { get => FBltMode; set => SetBltMode(value); }
    protected internal NotifyEvent<object> OnUpdated { get => FOnUpdated; set => FOnUpdated = value; }
    protected internal int ScaleX { get => FScaleX; set => SetScaleX(value); }
    protected internal int ScaleY { get => FScaleY; set => SetScaleY(value); }

    // ImageLib.pas:6504

    public CustomImageLibraryPanel(Component owner) : base(owner)
    {
        /* TODO: Implement ExtCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }
}
