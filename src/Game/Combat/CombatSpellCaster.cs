// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// (spell ID, source ID, refcount)
public partial class CombatSpellCaster : EngineObject
{
    // Spells.PAS:793
    public CombatSpellCaster(ECustomNode owner) : base(owner)
    {
        FCombatCasterRefCount = 1;
    }

    public int FCombatCasterSpellID;
    public int FCombatCasterSourceID;
    public int FCombatCasterRefCount;

    // Spells.PAS:798
    public override int AddRef()
    {
        FCombatCasterRefCount += 1;
        return FCombatCasterRefCount;
    }

    // Spells.PAS:803
    public override int Release()
    {
        int Count;
        int ReleasedCount;
        FCombatCasterRefCount -= 1;
        Count = FCombatCasterRefCount;
        ReleasedCount = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return ReleasedCount;
    }

    // intf_P4_02.inc:1012

}
