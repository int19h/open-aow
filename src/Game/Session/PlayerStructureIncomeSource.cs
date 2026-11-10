// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public partial class PlayerStructureIncomeSource : IncomeSource
{
    public PlayerStructureIncomeSource(ECustomNode owner) : base(owner)
    { }

    public PlayerStructure FStructure;

    // Structur.PAS:2486
    public override string Name() => FStructure.GetName();

    // Structur.PAS:2491
    public override int Income() => FStructure.GetIncome();

    // Structur.PAS:2496
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = FStructure.Xhx;
        Y = FStructure.Yhx;
        L = FStructure.Lhx;
    }

    // Structur.PAS:2481
    // (FStructure)
    public void Setup(PlayerStructure Structure)
    {
        FStructure = Structure;
    }

    // Engine.pas:524

}
