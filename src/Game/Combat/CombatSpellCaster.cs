// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// (spell ID, source ID, refcount)
public sealed class CombatSpellCaster(CustomNode owner) : EngineObject(owner)
{
    public int CombatCasterSpellId { get; init; }
    public int CombatCasterSourceId { get; init; }

    private int _combatCasterRefCount = 1;

    // Spells.PAS:798
    public override int AddRef()
    {
        _combatCasterRefCount += 1;
        return _combatCasterRefCount;
    }

    // Spells.PAS:803
    public override int Release()
    {
        _combatCasterRefCount -= 1;
        int count = _combatCasterRefCount;
        int releasedCount = count;
        if (count == 0)
        {
            Dispose();
        }

        return releasedCount;
    }
}
