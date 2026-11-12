// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// (spell ID, source ID, refcount)
public partial class CombatSpellCaster : EngineObject
{
    // Spells.PAS:793
    public CombatSpellCaster(ECustomNode owner) : base(owner)
    {
        CombatCasterRefCount = 1;
    }

    public int CombatCasterSpellId;
    public int CombatCasterSourceId;
    public int CombatCasterRefCount;

    // Spells.PAS:798
    public override int AddRef()
    {
        CombatCasterRefCount += 1;
        return CombatCasterRefCount;
    }

    // Spells.PAS:803
    public override int Release()
    {
        int count;
        int releasedCount;
        CombatCasterRefCount -= 1;
        count = CombatCasterRefCount;
        releasedCount = count;
        if (count == 0)
        {
            Dispose();
        }

        return releasedCount;
    }

    // intf_P4_02.inc:1012

}
