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
        FSpellName = AbilityRules.TranslateRStr(GeyserRStr);
        FSpellID = 118;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 1;
    }

    // CombatSpells.pas:1522
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        CombatSpellCA Action;
        Action = new CombatSpellCA(null);
        Action.FSpellID = FSpellID;
        if (Target.GetAbilityEnabled(1) || Target.GetAbilityEnabled(59))
        {
            Action.Generate(Source, Target, FCombatInfo.Attack, (sbyte)(FCombatInfo.Damage / 2), FCombatInfo.DamageTypes);
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

    private static readonly string GeyserRStr = "Geyser";
}
