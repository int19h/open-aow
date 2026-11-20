#nullable disable
// Translated from ImageLib.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Draw.Images;

// an opaque 8 bit picture (ClassID 1). FPitch (of the rows: a multiple of 4 below 32 pixels, of
// 8 from 32 on), FBuffer (the rows from its first byte. The original's FBits, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture08(CustomImageLibrary owner) : Image08(owner), ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 1;
    static string ILibraryImageClass.Name => "Picture (08bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Picture08(owner);

    private int _pitch;
    private byte[] _buffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(_buffer, 0, _pitch);

    // ImageLib.pas:7305
    public override void SetXshift(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        base.SetXshift(value);
        SetWidth(Width);
    }

    // ImageLib.pas:7312
    public override void SetYshift(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        base.SetYshift(value);
        SetHeight(Height);
    }

    // ImageLib.pas:7319
    public override void SetWidth(int value)
    {
        if (value < DataWidth + Xshift)
        {
            value = DataWidth + Xshift;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:7325
    public override void SetHeight(int value)
    {
        if (value < DataHeight + Yshift)
        {
            value = DataHeight + Yshift;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:7331
    public override bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId) => (pixelFormatId is PixelFormat.None or PixelFormat.Pal8 or PixelFormat.Rgb555 or PixelFormat.Rgb565) && showMode == ShowMode.Opaque;

    // ImageLib.pas:7337
    public void AllocateDiscardableImageData(int width, int height)
    {
        _pitch = width < 32 ? (width + 3) & ~3 : (width + 7) & ~7;

        // The original allocates 7 bytes more than the rows and aligns its pixel pointer to 8 for
        // MMX. The alignment has no effect on pixels, so the rows start at the first byte and the
        // 7 bytes stay after the last row (graphics.md 6.8).
        int size = (_pitch * height) + 7;
        _buffer = MemorySurfaces.Reallocate(_buffer, size);
    }

    // ImageLib.pas:7350
    public override void ReleaseDiscardableImageData()
    {
        if (DataLoaded)
        {
            _buffer = null;
            DataLoaded = false;
        }
    }

    // ImageLib.pas:7359
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        AllocateDiscardableImageData(DataWidth, DataHeight);
        for (int y = 0, last = DataHeight - 1; y <= last; y++)
        {
            stream.ReadExactly(_buffer.AsSpan(y * _pitch, DataWidth));
        }
    }

    // ImageLib.pas:7369
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        for (int y = 0, last = DataHeight - 1; y <= last; y++)
        {
            stream.Write(_buffer.AsSpan(y * _pitch, DataWidth));
        }
    }

    // ImageLib.pas:7384
    public override bool SupportGrabPixelFormat(PixelFormat pixelFormatId) => pixelFormatId == PixelFormat.Pal8;

    // ImageLib.pas:7389
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        try
        {
            Clear();
            DataLoaded = true;
            DataWidth = R.Right - R.Left;
            DataHeight = R.Bottom - R.Top;
            SetWidthWithoutOverrides(DataWidth);
            SetHeightWithoutOverrides(DataHeight);
            FileName = ExtractFileName(grabInfo.FileName);
            FileIndex = grabInfo.FileIndex;
            DataSize = DataWidth * DataHeight;
            AllocateDiscardableImageData(Width, Height);
            if (lockedSurface.PixelFormatId == PixelFormat.Pal8)
            {
                for (int y = 0, last = Height - 1; y <= last; y++)
                {
                    lockedSurface.View().Bytes(((R.Top + y) * lockedSurface.Pitch) + R.Left, DataWidth).CopyTo(_buffer.AsSpan(_pitch * y, DataWidth));
                }

                CreateLibraryImagePalette(lockedSurface.Palette);
                if (Owner is not null)
                {
                    RegisterPalette();
                }
            }
            else
            {
                return false;
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            return false;
        }

        return true;
    }

    public override void ShowOpaque(int x, int y, in LockedSurface lockedSurface)
    {
        int destinationX = x + Xshift, destinationY = y + Yshift;
        int right = destinationX + DataWidth, bottom = destinationY + DataHeight;
        int width = DataWidth, height = DataHeight, sourceX = 0, sourceY = 0;
        // Preserve the source offsets and byte destination offset of ImageLib.pas:7424.
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sourceX = lockedSurface.ClipRectangle.Left - x;
            destinationX = lockedSurface.ClipRectangle.Left;
            width -= sourceX;
        }
        if (right > lockedSurface.ClipRectangle.Right)
        {
            width -= right - lockedSurface.ClipRectangle.Right;
        }

        if (width <= 0)
        {
            return;
        }

        if (destinationY < lockedSurface.ClipRectangle.Top)
        {
            sourceY = lockedSurface.ClipRectangle.Top - y;
            destinationY = lockedSurface.ClipRectangle.Top;
            height -= sourceY;
        }
        if (bottom > lockedSurface.ClipRectangle.Bottom)
        {
            height -= bottom - lockedSurface.ClipRectangle.Bottom;
        }

        if (height <= 0)
        {
            return;
        }

        bool loadedHere = !DataLoaded;
        if (loadedHere)
        {
            LoadDiscardableImageData();
        }

        SurfaceView source = View.At(sourceX + (sourceY * _pitch));
        SurfaceView destination = lockedSurface.View(destinationX + (destinationY * lockedSurface.Pitch));
        if (lockedSurface.PixelFormatId == PixelFormat.Pal8)
        {
            BlitRects.BlitRectangle(source, destination, width, height);
        }
        else
        {
            if (lockedSurface.PixelFormatId != RemapPaletteValue.PixelFormatId &&
                RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            if (lockedSurface.PixelFormatId == PixelFormat.Rgb555)
            {
                PixelRemap.RemapSurface08To555(source, destination, width, height, RemapPaletteValue);
            }
            else if (lockedSurface.PixelFormatId == PixelFormat.Rgb565)
            {
                PixelRemap.RemapSurface08To565(source, destination, width, height, RemapPaletteValue);
            }
        }
        if (loadedHere && LoadMode == LoadMode.LoadFreeOnDemand)
        {
            ReleaseDiscardableImageData();
        }
    }
}
