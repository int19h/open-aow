// Translated from DisplayC.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Draw;

[StructLayout(LayoutKind.Explicit, Size = 1028)]
public struct Record_TWindows08DC_CreateHPalette_LogPal_2256_3
{
    [FieldOffset(0)]
    public ushort palVersion;
    [FieldOffset(2)]
    public ushort palNumEntries;
    [FieldOffset(4)]
    public FixedArray_Aow_Rtl_TPaletteEntry_256 palPalEntry;
}
