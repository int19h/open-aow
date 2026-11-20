// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// spell ID, mana cost charged after execution
public partial class DispelMagicSpellCombatAction(CustomNode owner) : DispelMagicCombatAction(owner)
{
    // UnitSpells.pas:975
    public new const int ClassId = 131793;

    public int SpellId { get; set; }
    public int ManaCost { get; set; }

    // UnitSpells.pas:980
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SpellId = persister.Int32("SpellId", value: SpellId);
        ManaCost = persister.Int32("ManaCost", value: ManaCost);
    }

    // UnitSpells.pas:987
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            if (GamePack.Current.SpellControl.GetSpell(SpellId) is not null)
            {
                CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
                GamePack.Current.SpellControl.GetSpell(SpellId).CastingDone(source.Troop, ManaCost);
            }
        }
    }

}
