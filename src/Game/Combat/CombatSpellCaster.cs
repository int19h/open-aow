// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// (spell ID, source ID, refcount)
public partial class CombatSpellCaster : EngineObject
{
    public CombatSpellCaster(CustomNode owner) : base(owner) => _combatCasterRefCount = 1;

    public int CombatCasterSpellId { get; init; }
    public int CombatCasterSourceId { get; init; }

    private int _combatCasterRefCount;

    // Spells.PAS:798
    public override int AddRef()
    {
        _combatCasterRefCount += 1;
        return _combatCasterRefCount;
    }

    // Spells.PAS:803
    public override int Release()
    {
        int count;
        int releasedCount;
        _combatCasterRefCount -= 1;
        count = _combatCasterRefCount;
        releasedCount = count;
        if (count == 0)
        {
            Dispose();
        }

        return releasedCount;
    }

    // intf_P4_02.inc:1012

}
