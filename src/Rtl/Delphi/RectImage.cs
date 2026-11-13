using System.Runtime.InteropServices;

namespace Aow.Rtl;

/// <summary>The stored image of a <see cref="Rect"/>: four little-endian integers (Delphi <c>TRect</c>).</summary>
[StoredLayout, StoredRecord("Rect")]
[StructLayout(LayoutKind.Explicit, Size = 16)]
public readonly struct RectImage : IRecordImage<RectImage, Rect>
{
    [FieldOffset(0)] public readonly int Left;
    [FieldOffset(4)] public readonly int Top;
    [FieldOffset(8)] public readonly int Right;
    [FieldOffset(12)] public readonly int Bottom;

    private RectImage(in Rect value)
    {
        Left = value.Left;
        Top = value.Top;
        Right = value.Right;
        Bottom = value.Bottom;
    }

    public static RectImage From(Rect value) => new(value);

    public Rect ToValue() => new(left: Left, top: Top, right: Right, bottom: Bottom);
}
