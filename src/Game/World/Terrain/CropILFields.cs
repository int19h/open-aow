// Translated from Crops.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Terrain;

[StructLayout(LayoutKind.Explicit, Size = 25)]
public struct CropILFields
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public int ImageIndex;
    [FieldOffset(24)]
    public sbyte TerrainType;
}
