// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class AbilityActivation : EngineObject
{
    // Ability.PAS:920
    public AbilityActivation(ECustomNode owner) : base(owner)
    {
        _references = 1;
    }

    private int _references;

    public int UnitId { get; set; }
    public int AbilityId { get; set; }

    // Ability.PAS:926
    public override int AddRef()
    {
        _references += 1;
        return _references;
    }

    // Ability.PAS:932
    public override int Release()
    {
        int count;
        _references -= 1;
        count = _references;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // intf_P1_06.inc:17

}
