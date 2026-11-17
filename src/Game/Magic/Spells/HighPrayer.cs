#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class HighPrayer : CombatSpell
{
    // CombatSpells.pas:996
    public HighPrayer(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_highPrayerText);
        SpellId = 102;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
        spellCombatInfo.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1013
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        TacticalBattleHighPrayerCombatAction action;
        action = new TacticalBattleHighPrayerCombatAction(null) { Holder = CreationHolder() };
        action.Succeeded = true;
        action.SourceId = source.Id;
        action.TargetId = target.Id;
        action.SpellId = SpellId;
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        return action;
    }

    // CombatSpells.pas:1104
    public override void TacticalBattleGetDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        int healing;
        int value;
        healing = target.GetHits() - target.GetHitPoints();
        if (healing > 5)
        {
            healing = 5;
        }

        value = healing * 100;
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

    // CombatSpells.pas:1026
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        CombatSide combatSide;
        CombatObject target;
        int value;
        FastBattleSpellCommand command;
        value = 0;
        CombatSideIndex side = commands.Source.GetSide();
        combatSide = commands.Combat.Sides[(int)side];
        for (int i = combatSide.GetCount() - 1; i >= 0; i--)
        {
            target = combatSide.GetObjects(i);
            if (target.CombatValue != 0 && target is CombatUnit)
            {
                value += AbilityRules.DamageValueToDamageEvaluation((target.GetHits() - target.GetHitPoints()) * 100, 0, commands.Source, target);
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

    // CombatSpells.pas:1054
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        FastBattleHighPrayerCombatAction action;
        action = new FastBattleHighPrayerCombatAction(null) { Holder = CreationHolder() };
        action.Generate(source);
        if (source is CombatUnit sourceUnit)
        {
            action.ManaCost = sourceUnit.Troop.CastingMana(this);
        }

        source.Combat.ExecuteCombatAction(action);
        _ = action.Release();
        return true;
    }

    // CombatSpells.pas:1067
    public override void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        int value;
        int healing;
        CombatSide combatSide;
        CombatObject ally;
        value = 0;
        CombatSideIndex side = source.GetSide();
        combatSide = source.Combat.Sides[(int)side];
        for (int i = combatSide.GetCount() - 1; i >= 0; i--)
        {
            ally = combatSide.GetObjects(i);
            if (ally.CombatValue != 0 && ally is CombatUnit)
            {
                healing = ally.GetHits() - ally.GetHitPoints();
                if (healing > 5)
                {
                    healing = 5;
                }

                value += healing * 100;
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

    private static readonly string s_highPrayerText = "High Prayer";
}
