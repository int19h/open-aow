// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public partial class PlayerStructurePowerSource : PowerSource
{
    public PlayerStructurePowerSource(ECustomNode owner) : base(owner)
    { }

    public PlayerStructure Structure;

    // Structur.PAS:2509
    public override string Name() => Structure.GetName();

    // Structur.PAS:2514
    public override int Power() => Structure.GetPower();

    // Structur.PAS:2519
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = Structure.HexX;
        Y = Structure.HexY;
        L = Structure.HexLevel;
    }

    // Structur.PAS:2526
    public override void Select()
    {
        AoWHSMap.Current.Select(Structure, true);
    }

    // Structur.PAS:2504
    // (Structure)
    public void Setup(PlayerStructure structure)
    {
        Structure = structure;
    }

    // Engine.pas:524

}
