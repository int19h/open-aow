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
        CasterRefCount = 1;
    }

    public int CasterSpellId;
    public int CasterSourceId;
    public int CasterRefCount;

    // Spells.PAS:753
    public override int AddRef()
    {
        CasterRefCount += 1;
        return CasterRefCount;
    }

    // Spells.PAS:758
    public override int Release()
    {
        int count;
        int releasedCount;
        CasterRefCount -= 1;
        count = CasterRefCount;
        releasedCount = count;
        if (count == 0)
        {
            Dispose();
        }

        return releasedCount;
    }

    // Spells.PAS:789
    public virtual bool Cast() => true;

    // intf_P4_02.inc:998
    public Troop GetSource() => AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
    public virtual bool CanCast(ref string reason) => AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId) != null;
}
