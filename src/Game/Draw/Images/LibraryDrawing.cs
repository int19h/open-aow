
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

internal static class LibraryDrawing
{
    internal static void BlitLibraryImage(Canvas canvas, LibraryImage image, Rect rectangle, BlitMode mode, int scaleX, int scaleY, int bkColor, int color, bool selected, ref readonly Rect drawRectangle) =>
        /* TODO: Implement Windows.SetStretchBltMode in the platform or editor service. */
        throw new NotImplementedException();

    internal static void Register() =>
        /* TODO: Implement DsgnIntf.RegisterComponentEditor in the platform or editor service. */
        throw new NotImplementedException();

    internal static void WriteStringToStream(string S, Stream stream)
    {
        int l = S.Length;
        stream.Write(Bytes.Of(ref l, 4));
        stream.Write(Cp1252.Encode(S).AsSpan()[..l]);
    }

    internal static void ReadStringFromStream(ref string S, Stream stream)
    {
        int l = default;
        stream.ReadExactly(Bytes.Of(ref l, 4));
        if (l < 0)
        {
            throw new ArgumentOutOfRangeException(paramName: null, "Negative string length");
        }

        byte[] buffer = new byte[l];
        Cp1252.Encode(S)[..Math.Min(S.Length, l)].CopyTo(buffer);
        // A failed read leaves the resized value in the destination.
        S = Cp1252.Decode(buffer);
        stream.ReadExactly(buffer);
        S = Cp1252.Decode(buffer);
    }

    internal static void DrawFocusFrame(int displayController, Rect R, bool focused, int color) =>
        /* TODO: Implement Windows.CreateSolidBrush in the platform or editor service. */
        throw new NotImplementedException();

}
