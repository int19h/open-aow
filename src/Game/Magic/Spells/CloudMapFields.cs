// Translated from SmokeCloud.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells;

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct CloudMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public byte SeatedPlayer;
}
