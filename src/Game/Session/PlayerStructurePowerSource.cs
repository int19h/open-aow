#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public class PlayerStructurePowerSource(CustomNode? owner) : PowerSource(owner)
{
    private PlayerStructure _structure;

    // Structur.PAS:2509
    public override string Name() => _structure.GetName();

    // Structur.PAS:2514
    public override int Power() => _structure.GetPower();

    // Structur.PAS:2519
    public override MapCoordinate GetLocation() => new(
        _structure.HexX,
        _structure.HexY,
        _structure.HexLevel);

    // Structur.PAS:2526
    public override void Select() => StrategicMap.Current.Select(_structure, center: true);

    // Structur.PAS:2504
    // (Structure)
    public void Setup(PlayerStructure structure) => _structure = structure;
}
