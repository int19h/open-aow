// Translated from AoWE.pas.

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct UnitShowInfo
{
    public byte Direction { get; internal set; }

    public UnitOverlayModes Overlays { get; set; }

    public UnitAnimation Animation { get; internal set; }
    public int Frame { get; internal set; }
    public bool Looped { get; internal set; }
}
