// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// spell ID, mana cost charged after execution
public partial class DispelMagicSpellCombatAction : DispelMagicCombatAction
{
    // UnitSpells.pas:975
    public new const int ClassId = 131793;

    public DispelMagicSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    public int SpellId;
    public int ManaCost;

    // UnitSpells.pas:980
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref SpellId, 60);
        stream.rwInteger(ref ManaCost, 61);
    }

    // UnitSpells.pas:987
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        base.Execute(combat);
        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            if (GamePack.Current.SpellControl.GetSpell(SpellId) != null)
            {
                source = (CombatUnit)combat.CombatData.FindId(SourceId);
                GamePack.Current.SpellControl.GetSpell(SpellId).CastingDone(source.Troop, ManaCost);
            }
        }
    }

}
