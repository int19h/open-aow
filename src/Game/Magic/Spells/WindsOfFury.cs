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
        FSpellName = AbilityRules.TranslateRStr(WindsOfFuryRStr);
        FSpellID = 126;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:1904
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        CombatSpellCombatAction Action;
        Action = new CombatSpellCombatAction(null);
        Action.FSpellID = FSpellID;
        if (Target.GetAbilityEnabled(1) || Target.GetAbilityEnabled(59))
        {
            Action.Generate(Source, Target, (sbyte)(FCombatInfo.Attack + 2), FCombatInfo.Damage, FCombatInfo.DamageTypes);
        }
        else
        {
            Action.Generate(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
        }

        if (ChargeMana)
        {
            Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
        }

        return Action;
    }

    private static readonly string WindsOfFuryRStr = "Winds Of Fury";
}
