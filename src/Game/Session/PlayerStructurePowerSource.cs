// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public partial class PlayerStructurePowerSource : PowerSource
{
    public PlayerStructurePowerSource(ECustomNode owner) : base(owner)
    { }

    private PlayerStructure _structure;

    // Structur.PAS:2509
    public override string Name() => _structure.GetName();

    // Structur.PAS:2514
    public override int Power() => _structure.GetPower();

    // Structur.PAS:2519
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = _structure.HexX;
        Y = _structure.HexY;
        L = _structure.HexLevel;
    }

    // Structur.PAS:2526
    public override void Select()
    {
        AoWHSMap.Current.Select(_structure, true);
    }

    // Structur.PAS:2504
    // (Structure)
    public void Setup(PlayerStructure structure)
    {
        _structure = structure;
    }

    // Engine.pas:524

}
