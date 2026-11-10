// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public partial class PlayerStructurePowerSource : PowerSource
{
    public PlayerStructurePowerSource(ECustomNode owner) : base(owner)
    { }

    public PlayerStructure FStructure;

    // Structur.PAS:2509
    public override string Name() => FStructure.GetName();

    // Structur.PAS:2514
    public override int Power() => FStructure.GetPower();

    // Structur.PAS:2519
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = FStructure.Xhx;
        Y = FStructure.Yhx;
        L = FStructure.Lhx;
    }

    // Structur.PAS:2526
    public override void Select()
    {
        AoWHSMap.Current.Select(FStructure, true);
    }

    // Structur.PAS:2504
    // (FStructure)
    public void Setup(PlayerStructure Structure)
    {
        FStructure = Structure;
    }

    // Engine.pas:524

}
