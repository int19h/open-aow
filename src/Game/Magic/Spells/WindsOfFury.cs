#nullable disable
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
    public WindsOfFury(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_windsOfFuryText);
        SpellId = 126;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1904
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        CombatSpellCombatAction action = new(null)
        {
            Holder = CreationHolder(),
            SpellId = SpellId
        };
        if (target.GetAbilityEnabled(1) || target.GetAbilityEnabled(59))
        {
            action.Generate(source, target, (sbyte)(CombatInfo.Attack + 2), CombatInfo.Damage, CombatInfo.DamageTypes);
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

    private static readonly string s_windsOfFuryText = "Winds Of Fury";
}
