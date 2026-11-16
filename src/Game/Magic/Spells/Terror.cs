#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Terror : CombatSpell
{
    // CombatSpells.pas:1759
    public Terror(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_terrorText);
        SpellId = 105;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1775
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        TacticalBattleTerrorCombatAction action;
        action = new TacticalBattleTerrorCombatAction(null) { Holder = CreationHolder() };
        action.Generate(source, target);
        if (chargeMana && source is CombatUnit sourceUnit)
        {
            action.ManaCost = sourceUnit.Troop.CastingMana(this);
        }

        return action;
    }

    // CombatSpells.pas:1862
    public override void TacticalBattleGetDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        int value = !target.GetAbilityEnabled(108) && target.CombatValue != 0 && target is CombatUnit && !target.GetImmunityTypes().Contains(DamageType.Death)
            ? Round((double)HitChance.HitRoleProbability(6 - target.GetResistance()) * 100)
            : 0;
        if (value != 0)
        {
            values.SourceDamage = value;
            values.SourceCost = value / 100;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            values.SourceDamage = 0;
            values.SourceCost = 0;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of();
        }
    }

    // CombatSpells.pas:1786
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        CombatSide side;
        CombatObject target;
        int value;
        FastBattleSpellCommand command;
        value = 0;
        CombatSideIndex opponentSide = commands.Source.GetOpponentSide();
        side = commands.Combat.Sides[(int)opponentSide];
        for (int i = side.GetCount() - 1; i >= 0; i--)
        {
            target = side.GetObjects(i);
            if (!target.GetAbilityEnabled(108) && target.CombatValue != 0 && target is CombatUnit && !target.GetImmunityTypes().Contains(DamageType.Death))
            {
                value += target.GetTargetPriority();
            }
        }

        if (value != 0)
        {
            command = new FastBattleSpellCommand(52, creator: this);
            command.Target = null;
            command.MoveCost = 1;
            command.SpellId = SpellId;
            command.ExpectedDamageValue = value;
            commands.Add(command);
        }
    }

    // CombatSpells.pas:1815
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        FastBattleTerrorCombatAction action;
        action = new FastBattleTerrorCombatAction(null) { Holder = CreationHolder() };
        action.Generate(source);
        if (source is CombatUnit sourceUnit)
        {
            action.ManaCost = sourceUnit.Troop.CastingMana(this);
        }

        source.Combat.ExecuteCombatAction(action);
        action.Release();
        return true;
    }

    // CombatSpells.pas:1828
    public override void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        int value;
        CombatSide side;
        CombatObject opponent;
        value = 0;
        CombatSideIndex opponentSide = source.GetOpponentSide();
        side = source.Combat.Sides[(int)opponentSide];
        for (int i = side.GetCount() - 1; i >= 0; i--)
        {
            opponent = side.GetObjects(i);
            if (!opponent.GetAbilityEnabled(108) && opponent.CombatValue != 0 && opponent is CombatUnit && !opponent.GetImmunityTypes().Contains(DamageType.Death))
            {
                value += Round((double)HitChance.HitRoleProbability(6 - opponent.GetResistance()) * 100);
            }
        }

        if (value != 0)
        {
            values.SourceDamage = value;
            values.SourceCost = value / 100;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            values.SourceDamage = 0;
            values.SourceCost = 0;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of();
        }
    }

    private static readonly string s_terrorText = "Terror";
}
