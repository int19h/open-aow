// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct UnitShowInfo
{
    public byte Direction;

    public Set<UnitOverlayModesElement> Overlays { get; set => field = Set<UnitOverlayModesElement>.FromMask((byte)value.Image); }

    public UnitAnimation Animation;
    public int Frame;
    public bool Looped;
}
