using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Hash;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

internal static class LibraryDrawing
{
    internal static void BltLibraryImage(Canvas Canvas, LibraryImage Image, global::Aow.Rtl.Rect Rect, BltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color, bool Selected, ref global::Aow.Rtl.Rect DrawRect)
    {
        /* TODO: Implement Windows.SetStretchBltMode in the platform or editor service. */
        throw new NotImplementedException();
    }

    internal static void Register()
    {
        /* TODO: Implement DsgnIntf.RegisterComponentEditor in the platform or editor service. */
        throw new NotImplementedException();
    }

    internal static void WriteStringToStream(string S, Stream Stream)
    {
        int L;
        L = S.Length;
        Stream.Write(Bytes.Of(ref L, 4));
        Stream.Write(Cp1252.Encode(S).AsSpan().Slice(0, L));
    }

    internal static void ReadStringFromStream(ref string S, Stream Stream)
    {
        int L = default;
        Stream.ReadBytes(Bytes.Of(ref L, 4));
        if (L < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative string length");
        byte[] buffer = new byte[L];
        Cp1252.Encode(S)[..Math.Min(S.Length, L)].CopyTo(buffer);
        // A failed read leaves the resized value in the destination.
        S = Cp1252.Decode(buffer);
        Stream.ReadBytes(buffer);
        S = Cp1252.Decode(buffer);
    }

    internal static void DrawFocusFrame(int DC, global::Aow.Rtl.Rect R, bool Focused, int Color)
    {
        /* TODO: Implement Windows.CreateSolidBrush in the platform or editor service. */
        throw new NotImplementedException();
    }

}
