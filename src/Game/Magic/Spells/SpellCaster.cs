#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// (spell ID, source ID, refcount)
public class SpellCaster(CustomNode owner) : EngineObject(owner)
{
    public int CasterSpellId { get; set; }
    public int CasterSourceId { get; set; }

    private int _casterRefCount = 1;

    // Spells.PAS:753
    public override int AddRef()
    {
        _casterRefCount += 1;
        return _casterRefCount;
    }

    // Spells.PAS:758
    public override int Release()
    {
        _casterRefCount -= 1;
        int count = _casterRefCount;
        int releasedCount = count;
        if (count == 0)
        {
            Dispose();
        }

        return releasedCount;
    }

    // Spells.PAS:789
    public virtual bool Cast() => true;

    // intf_P4_02.inc:998
    public Troop Source => StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);

    public virtual bool CanCast(ref string reason) => StrategicMap.Current.UnitControl.FindUnit(CasterSourceId) is not null;
}
