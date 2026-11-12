// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public partial class PlayerStructureIncomeSource : IncomeSource
{
    public PlayerStructureIncomeSource(ECustomNode owner) : base(owner)
    { }

    public PlayerStructure Structure;

    // Structur.PAS:2486
    public override string Name() => Structure.GetName();

    // Structur.PAS:2491
    public override int Income() => Structure.GetIncome();

    // Structur.PAS:2496
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = Structure.HexX;
        Y = Structure.HexY;
        L = Structure.HexLevel;
    }

    // Structur.PAS:2481
    // (Structure)
    public void Setup(PlayerStructure structure)
    {
        Structure = structure;
    }

    // Engine.pas:524

}
