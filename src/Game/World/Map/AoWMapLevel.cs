// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

// inherited field table and coordinate offsets, reserved
public partial class AoWMapLevel : MapLevel
{
    public AoWMapLevel(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131091;

    // AoWHSMap.PAS:231
    public override EClass MapFieldClass() => EClass.Of<AoWMapField>();

}
