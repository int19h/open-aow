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
    public HighPrayer(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(HighPrayerRStr);
        FSpellID = 102;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
        spellCombatInfo.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);
    }

    // CombatSpells.pas:1013
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        TacticalCombatHighPrayerCA Action;
        Action = new TacticalCombatHighPrayerCA(null);
        Action.FSucceeded = true;
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        Action.FSpellID = FSpellID;
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        return Action;
    }

    // CombatSpells.pas:1104
    public override void tcGetDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        int Healing;
        int Value;
        Healing = Target.GetHits() - Target.GetHitPoints();
        if (Healing > 5)
        {
            Healing = 5;
        }

        Value = Healing * 100;
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

    // CombatSpells.pas:1026
    public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
    {
        CombatSide Side;
        CombatObject Target;
        int Value;
        SpellCC Command;
        Value = 0;
        var side = Commands.FSource.GetSide();
        Side = Commands.FCombat.FSides[(int)side];
        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            Target = Side.GetObjects(I);
            if (Target.FCombatValue != 0 && Target is CombatUnit)
            {
                Value += AbilityRules.DVtoDEV((Target.GetHits() - Target.GetHitPoints()) * 100, 0, Commands.FSource, Target);
            }
        }

        if (Value != 0)
        {
            Command = new SpellCC(52);
            Command.FTarget = null;
            Command.FMoveCost = 1;
            Command.FSpellID = FSpellID;
            Command.FExpectedDV = Value;
            Commands.Add(Command);
        }
    }

    // CombatSpells.pas:1054
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        FastCombatHighPrayerCA Action;
        Action = new FastCombatHighPrayerCA(null);
        Action.Generate(Source);
        Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
        Source.FCombat.ExecuteCombatAction(Action);
        Action.Release();
        return true;
    }

    // CombatSpells.pas:1067
    public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
    {
        int Value;
        int Healing;
        CombatSide Side;
        CombatObject Ally;
        Value = 0;
        var side = Source.GetSide();
        Side = Source.FCombat.FSides[(int)side];
        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            Ally = Side.GetObjects(I);
            if (Ally.FCombatValue != 0 && Ally is CombatUnit)
            {
                Healing = Ally.GetHits() - Ally.GetHitPoints();
                if (Healing > 5)
                {
                    Healing = 5;
                }

                Value += Healing * 100;
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

    private static readonly string HighPrayerRStr = "High Prayer";
}
