// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Geyser : CombatSpell
{
    // CombatSpells.pas:1505
    public Geyser(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_geyserText);
        SpellId = 118;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 1;
    }

    // CombatSpells.pas:1522
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        CombatSpellCombatAction action;
        action = new CombatSpellCombatAction(null);
        action.SpellId = SpellId;
        if (target.GetAbilityEnabled(1) || target.GetAbilityEnabled(59))
        {
            action.Generate(source, target, CombatInfo.Attack, (sbyte)(CombatInfo.Damage / 2), CombatInfo.DamageTypes);
        }
        else
        {
            action.Generate(source, target, CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes);
        }

        if (chargeMana)
        {
            action.ManaCost = ((Hero)((CombatUnit)source).Troop).CastingMana(this);
        }

        return action;
    }

    private static readonly string s_geyserText = "Geyser";
}
