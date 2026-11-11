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
    public Terror(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(TerrorRStr);
        FSpellID = 105;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
    }

    // CombatSpells.pas:1775
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        TacticalBattleTerrorCombatAction Action;
        Action = new TacticalBattleTerrorCombatAction(null);
        Action.Generate(Source, Target);
        if (ChargeMana)
        {
            Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
        }

        return Action;
    }

    // CombatSpells.pas:1862
    public override void tcGetDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        int Value;
        if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is CombatUnit && !Target.GetImmunityTypes().Contains(DamageType.Death))
        {
            Value = Round((double)HitChance.HitRoleProbability(6 - Target.GetResistance()) * 100);
        }
        else
        {
            Value = 0;
        }

        if (Value != 0)
        {
            Values.SourceDamage = Value;
            Values.SourceCost = Value / 100;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            Values.SourceDamage = 0;
            Values.SourceCost = 0;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of();
        }
    }

    // CombatSpells.pas:1786
    public override void fcPrefetchCombatCommands(FastBattleCommandList Commands)
    {
        CombatSide Side;
        CombatObject Target;
        int Value;
        FastBattleSpellCommand Command;
        Value = 0;
        var opponentSide = Commands.FSource.GetOpponentSide();
        Side = Commands.FCombat.FSides[(int)opponentSide];
        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            Target = Side.GetObjects(I);
            if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is CombatUnit && !Target.GetImmunityTypes().Contains(DamageType.Death))
            {
                Value += Target.GetTargetPriority();
            }
        }

        if (Value != 0)
        {
            Command = new FastBattleSpellCommand(52);
            Command.FTarget = null;
            Command.FMoveCost = 1;
            Command.FSpellID = FSpellID;
            Command.FExpectedDV = Value;
            Commands.Add(Command);
        }
    }

    // CombatSpells.pas:1815
    public override bool fcExecuteCombatCommand(CombatObject Source, FastBattleCommand Command)
    {
        FastBattleTerrorCombatAction Action;
        Action = new FastBattleTerrorCombatAction(null);
        Action.Generate(Source);
        Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
        Source.FCombat.ExecuteCombatAction(Action);
        Action.Release();
        return true;
    }

    // CombatSpells.pas:1828
    public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
    {
        int Value;
        CombatSide Side;
        CombatObject Opponent;
        Value = 0;
        var opponentSide = Source.GetOpponentSide();
        Side = Source.FCombat.FSides[(int)opponentSide];
        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            Opponent = Side.GetObjects(I);
            if (!Opponent.GetAbilityEnabled(108) && Opponent.FCombatValue != 0 && Opponent is CombatUnit && !Opponent.GetImmunityTypes().Contains(DamageType.Death))
            {
                Value += Round((double)HitChance.HitRoleProbability(6 - Opponent.GetResistance()) * 100);
            }
        }

        if (Value != 0)
        {
            Values.SourceDamage = Value;
            Values.SourceCost = Value / 100;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            Values.SourceDamage = 0;
            Values.SourceCost = 0;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of();
        }
    }

    private static readonly string TerrorRStr = "Terror";
}
