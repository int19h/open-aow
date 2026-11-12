// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class AbilityActivation : EngineObject
{
    // Ability.PAS:920
    public AbilityActivation(ECustomNode owner) : base(owner)
    {
        References = 1;
    }

    public int References;
    public int UnitId;
    public int AbilityId;

    // Ability.PAS:926
    public override int AddRef()
    {
        References += 1;
        return References;
    }

    // Ability.PAS:932
    public override int Release()
    {
        int count;
        References -= 1;
        count = References;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // intf_P1_06.inc:17

}
