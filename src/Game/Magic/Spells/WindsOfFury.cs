// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public partial class WindsOfFury : CombatSpell
{
    // CombatSpells.pas:1889
    public WindsOfFury(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_windsOfFuryText);
        SpellId = 126;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:1904
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        CombatSpellCombatAction action;
        action = new CombatSpellCombatAction(null);
        action.SpellId = SpellId;
        if (target.GetAbilityEnabled(1) || target.GetAbilityEnabled(59))
        {
            action.Generate(source, target, (sbyte)(CombatInfo.Attack + 2), CombatInfo.Damage, CombatInfo.DamageTypes);
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

    private static readonly string s_windsOfFuryText = "Winds Of Fury";
}
