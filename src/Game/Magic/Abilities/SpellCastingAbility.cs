// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class SpellCastingAbility : MultiLevelAbility
{
    // AoWAb.PAS:4439
    public SpellCastingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 52;
        FAbilityName = AbilityRules.TranslateRStr(SpellCastingRStr);
        FAbilityType = AbilityType.Enhancement;
        FMaxLevel = 5;
        FLevelCosts[1] = 20;
        FLevelCosts[2] = 20;
        FLevelCosts[3] = 20;
        FLevelCosts[4] = 20;
        FLevelCosts[5] = 20;
    }

    // AoWAb.PAS:4463
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4588
    public override void fcPrefetchCombatCommands(FastBattleCommandList Commands)
    {
        FastBattle Combat;
        CombatObject Source;
        string Error = "";
        PlayerMagicControl Magic;
        Spell Spell;
        Combat = Commands.FCombat;
        Source = Commands.FSource;
        if (Source is CombatUnit)
        {
            var players = Combat.FCombatData.FPlayers.GetPlayers(Source.FCombatPlayer);
            if (AoWHSMap.Current.FPlayers.GetPlayers(players.FPlayer).FPlayerType != PlayerType.Human || ((Hero)((CombatUnit)Source).FUnit).FCastingSpellID == 0)
            {
                var side = Source.GetSide();
                var opponentSide = Source.GetOpponentSide();
                if (Combat.FSides[(int)side].FStrength < Combat.FSides[(int)opponentSide].FStrength * 2 || Source.GetHitPoints() < 10 || Combat.FCombatData.FWallType != 0 && !Combat.FCombatData.FWallBreached && CanCastBreachWallSpell(((CombatUnit)Source).FUnit))
                {
                    var player = Commands.FSource.GetPlayer();
                    Magic = AoWHSMap.Current.FPlayers.GetPlayers(player).FMagicControl;
                    if (!Magic.GetCheatEnabled())
                    {
                        for (int I = Magic.FSpells.Count - 1; I >= 0; I--)
                        {
                            var item = Magic.FSpells[I];
                            Spell = AoWHSSet.Current.FSpellControl.GetSpell(item);
                            if (Spell != null && Spell.FSpellControlTypes.Contains(SpellControlTypeElement.Combat))
                            {
                                if (((CombatUnit)Commands.FSource).FUnit is Hero)
                                {
                                    if (((Hero)((CombatUnit)Commands.FSource).FUnit).CanCastSpellInstantly(Spell.FSpellID, Spell.FBaseManaCost, ref Error))
                                    {
                                        Spell.fcPrefetchCombatCommands(Commands);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = 0, last = AoWHSSet.Current.FSpellControl.GetCount() - 1; I <= last; I++)
                        {
                            Spell = AoWHSSet.Current.FSpellControl.GetSpell(I);
                            if (Spell != null && Spell.FSpellControlTypes.Contains(SpellControlTypeElement.Combat))
                            {
                                Spell.fcPrefetchCombatCommands(Commands);
                            }
                        }
                    }
                }
            }
        }
    }

    // AoWAb.PAS:4484
    public override bool fcExecuteCombatCommand(CombatObject Source, FastBattleCommand Command) => AoWHSSet.Current.FSpellControl.GetSpell(((FastBattleSpellCommand)Command).FSpellID).fcExecuteCombatCommand(Source, Command);

    // AoWAb.PAS:4479
    public override bool CanActivate(AbilityOwner Owner, ref string Reason)
    {
        bool Result;
        if (Owner is Hero)
        {
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:4465
    public override bool Expand(AbilityOwner owner)
    {
        bool Result = base.Expand(owner);
        if (Result)
        {
            if (owner is Troop && AoWHSMap.Current != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(((Troop)owner).FPlayer).FMagicControl.Update();
            }
        }

        return Result;
    }

    // AoWAb.PAS:4472
    public override bool Remove(AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result)
        {
            if (owner is Troop && AoWHSMap.Current != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(((Troop)owner).FPlayer).FMagicControl.Update();
            }
        }

        return Result;
    }

    // AoWAb.PAS:4538
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        Spell Spell;
        Troop Source;
        PlayerMagicControl Magic;
        AbilityDamageValues Values = default;
        DamageValues = default;
        if (Attacker is CombatUnit)
        {
            var player = Attacker.GetPlayer();
            Magic = AoWHSMap.Current.FPlayers.GetPlayers(player).FMagicControl;
            Source = ((CombatUnit)Attacker).FUnit;
            for (int I = Magic.FSpells.Count - 1; I >= 0; I--)
            {
                var item = Magic.FSpells[I];
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(item);
                if (Spell != null && Spell.FSpellControlTypes.Contains(SpellControlTypeElement.Combat) && Source.GetCastingPointsMax() >= Spell.FBaseManaCost)
                {
                    Spell.fcGetDamageValueEx(Attacker, Defender, WallSituation, ref Values);
                    if (DamageValues.SourceDamage < Values.SourceDamage)
                    {
                        DamageValues = Values;
                    }
                }
            }
        }
    }

    // AoWAb.PAS:4563
    public override void tcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, ref AbilityDamageValues DamageValues)
    {
        Spell Spell;
        Troop Source;
        PlayerMagicControl Magic;
        AbilityDamageValues Values = default;
        DamageValues = default;
        if (Attacker is CombatUnit)
        {
            var player = Attacker.GetPlayer();
            Magic = AoWHSMap.Current.FPlayers.GetPlayers(player).FMagicControl;
            Source = ((CombatUnit)Attacker).FUnit;
            for (int I = Magic.FSpells.Count - 1; I >= 0; I--)
            {
                var item = Magic.FSpells[I];
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(item);
                if (Spell != null && Spell.FSpellControlTypes.Contains(SpellControlTypeElement.Combat) && Source.GetCastingPoints() >= Spell.FBaseManaCost)
                {
                    Spell.fcGetDamageValueEx(Attacker, Defender, WallSituation.None, ref Values);
                    if (DamageValues.SourceDamage < Values.SourceDamage)
                    {
                        DamageValues = Values;
                    }
                }
            }
        }
    }

    // AoWAb.PAS:4452
    public override string GetLevelName(int Level)
    {
        string Result = "";
        switch (Level)
        {
            case 0:
                Result = AbilityRules.TranslateRStr(SpellCastingRStr);
                break;
            case 1:
                Result = AbilityRules.TranslateRStr(SpellCastingRStr) + " I";
                break;
            case 2:
                Result = AbilityRules.TranslateRStr(SpellCastingRStr) + " II";
                break;
            case 3:
                Result = AbilityRules.TranslateRStr(SpellCastingRStr) + " III";
                break;
            case 4:
                Result = AbilityRules.TranslateRStr(SpellCastingRStr) + " IV";
                break;
            case 5:
                Result = AbilityRules.TranslateRStr(SpellCastingRStr) + " V";
                break;
        }

        return Result;
    }

    // AoWAb.PAS:4488
    public bool CanCastBreachWallSpell(Troop AUnit)
    {
        bool Result;
        int I;
        Spell Spell;
        SpellCombatInfo Info = default;
        string Error = "";
        PlayerMagicControl Magic;
        if (AUnit is Hero)
        {
            Magic = AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).FMagicControl;
            Result = Magic.GetCheatEnabled();
            I = Magic.FSpells.Count;
            while (!Result && I != 0)
            {
                I -= 1;
                var item = Magic.FSpells[I];
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(item);
                if (Spell != null && Spell.FSpellType == SpellType.Combat)
                {
                    Spell.GetCombatInfo(AUnit, ref Info);
                    bool condition = Info.DamageTypes.Contains(DamageType.Wall);
                    if (condition)
                    {
                        var fSpellsItem = Magic.FSpells[I];
                        condition = ((Hero)AUnit).CanCastCombatSpell(fSpellsItem, ref Error);
                    }

                    Result = condition;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:4515
    public bool AICanCastCombatSpell(CombatUnit Source)
    {
        Hero Hero;
        int SourceStrength;
        int TargetStrength;
        float Ratio;
        Hero = (Hero)Source.FUnit;
        bool Result = AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).FPlayerType != PlayerType.Human || Hero.FCastingSpellID == 0;
        if (Result)
        {
            var side = Source.GetSide();
            SourceStrength = Source.FCombat.FSides[(int)side].FStrength;
            if (SourceStrength <= 0)
            {
                SourceStrength = 0;
            }

            var opponentSide = Source.GetOpponentSide();
            TargetStrength = Source.FCombat.FSides[(int)opponentSide].FStrength;
            if (TargetStrength <= 0)
            {
                TargetStrength = 0;
            }

            // { Original divides by zero when the opponent strength is zero. }
            Ratio = (float)((double)SourceStrength / TargetStrength);
            if (((double)Ratio >= 1.5 || Hero.FCastingSpellID != 0) && (double)Ratio >= 1 && (double)((double)Source.GetHitPoints() * Ratio) >= 7)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    private static readonly string SpellCastingRStr = "Spell Casting";
}
