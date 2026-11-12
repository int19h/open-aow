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
        _casterRefCount = 1;
    }

    public int CasterSpellId { get; set; }
    public int CasterSourceId { get; set; }

    private int _casterRefCount;

    // Spells.PAS:753
    public override int AddRef()
    {
        _casterRefCount += 1;
        return _casterRefCount;
    }

    // Spells.PAS:758
    public override int Release()
    {
        int count;
        int releasedCount;
        _casterRefCount -= 1;
        count = _casterRefCount;
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
