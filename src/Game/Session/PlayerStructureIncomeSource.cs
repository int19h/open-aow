// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

public partial class PlayerStructureIncomeSource : IncomeSource
{
    public PlayerStructureIncomeSource(ECustomNode owner) : base(owner)
    { }

    private PlayerStructure _structure;

    // Structur.PAS:2486
    public override string Name() => _structure.GetName();

    // Structur.PAS:2491
    public override int Income() => _structure.GetIncome();

    // Structur.PAS:2496
    public override void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        X = _structure.HexX;
        Y = _structure.HexY;
        L = _structure.HexLevel;
    }

    // Structur.PAS:2481
    // (Structure)
    public void Setup(PlayerStructure structure)
    {
        _structure = structure;
    }

    // Engine.pas:524

}
