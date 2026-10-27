using System.Runtime.InteropServices;
using Aow.Rtl;

namespace Aow.Graphics;

[StructLayout(LayoutKind.Explicit, Size = 60)]
public struct TSurfaceDesc
{
    [FieldOffset(0)] public int Width;
    [FieldOffset(4)] public int Height;
    [FieldOffset(8)] public int BitCount;
    [FieldOffset(12)] public int Pitch;
    [FieldOffset(16)] public TRect ClipRect;
    [FieldOffset(32)] private int data;
    [FieldOffset(36)] private int palette;
    [FieldOffset(40)] public int PixelFormatID;
    [FieldOffset(44)] public int RMask;
    [FieldOffset(48)] public int GMask;
    [FieldOffset(52)] public int BMask;
    [FieldOffset(56)] public int AMask;
    public object? Data { readonly get => BorrowedReferences.Load(data); set => data = BorrowedReferences.Store(value); }
    public object? Palette { readonly get => BorrowedReferences.Load(palette); set => palette = BorrowedReferences.Store(value); }
}
