// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// (spell ID, source ID, refcount)
public partial class SpellCaster : EngineObject
{

    // Spells.PAS:748
    public SpellCaster(ECustomNode owner) : base(owner)
    {
        FCasterRefCount = 1;
    }

    public int FCasterSpellID;
    public int FCasterSourceID;
    public int FCasterRefCount;

    // Spells.PAS:753
    public override int AddRef()
    {
        FCasterRefCount += 1;
        return FCasterRefCount;
    }

    // Spells.PAS:758
    public override int Release()
    {
        int Count;
        int ReleasedCount;
        FCasterRefCount -= 1;
        Count = FCasterRefCount;
        ReleasedCount = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return ReleasedCount;
    }

    // Spells.PAS:789
    public virtual bool Cast() => true;

    // intf_P4_02.inc:998
    public Troop GetSource() => AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
    public virtual bool CanCast(ref string reason) => AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID) != null;
}
