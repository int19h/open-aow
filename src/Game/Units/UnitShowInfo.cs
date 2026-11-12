// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct UnitShowInfo
{
    [FieldOffset(0)]
    public byte Direction;
    [FieldOffset(1)]
    public byte OverlaysImage;

    public Set<UnitOverlayModesElement> Overlays { get => Set<UnitOverlayModesElement>.FromMask(OverlaysImage); set => OverlaysImage = (byte)value.Image; }

    [FieldOffset(2)]
    public UnitAnimation Animation;
    [FieldOffset(4)]
    public int Frame;
    [FieldOffset(8)]
    public bool Looped;
}
