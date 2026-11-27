#nullable disable warnings
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class Geyser : CombatSpell
{
    // CombatSpells.pas:1505
    public Geyser(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_geyserText);
        SpellId = 118;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = DamageTypes.Physical;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 1;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1522
    public override CombatAction CreateCombatAction(CombatObject? source, CombatObject target, bool chargeMana)
    {
        CombatSpellCombatAction action = new(null)
        {
            Holder = CreationHolder(),
            SpellId = SpellId
        };
        if (target.GetAbilityEnabled(1) || target.GetAbilityEnabled(59))
        {
            action.Generate(source, target, CombatInfo.Attack, damage: CombatInfo.Damage / 2, CombatInfo.DamageTypes);
        }
        else
        {
            action.Generate(source, target, CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes);
        }

        if (chargeMana && source is CombatUnit sourceUnit)
        {
            action.ManaCost = sourceUnit.Troop.CastingMana(this);
        }

        return action;
    }

    private static readonly string s_geyserText = "Geyser";
}
