// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class AbilityActivation : EngineObject
{
    // Ability.PAS:920
    public AbilityActivation(ECustomNode owner) : base(owner)
    {
        FReferences = 1;
    }

    public int FReferences;
    public int FUnitID;
    public int FAbilityID;

    // Ability.PAS:926
    public override int AddRef()
    {
        FReferences += 1;
        return FReferences;
    }

    // Ability.PAS:932
    public override int Release()
    {
        int Count;
        FReferences -= 1;
        Count = FReferences;
        int Result = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return Result;
    }

    // intf_P1_06.inc:17

}
