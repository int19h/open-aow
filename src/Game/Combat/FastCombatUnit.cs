// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// round distance, state
public partial class FastCombatUnit : CombatUnit
{
    // AIGroup.pas:6778
    public new const int ClassID = 131791;

    // AIGroup.pas:6773
    public FastCombatUnit(ECustomNode owner) : base(owner)
    {
    }

    public int FRoundDistance;
    public Set<FastCombatUnitStateElement> FState;

    // AIGroup.pas:6797
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FState, 1, 40, Stream.rwData, 0);
    }

    // AIGroup.pas:6862
    public override void GetTargetCVDV(CombatObject Target, ref int CV, ref int DV)
    {
        AbstractUnit AUnit;
        AbilityTypeList Abilities;
        AbilityDamageValues Values = default;
        int Value;
        Ability Ability;
        if (!FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            AUnit = FUnit;
            CV = 0;
            DV = 0;
            Abilities = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[(int)AbilityType.Combat];
            for (int I = Abilities.Count - 1; I >= 0; I--)
            {
                Ability = Abilities.GetAbility(I);
                if (AUnit.GetAbilityEnabled(Ability.FAbilityID))
                {
                    Ability.fcGetDamageValueEx(this, Target, WallSituation.None, ref Values);
                    if (Values.SourceDamage > 0)
                    {
                        Value = Values.SourceDamage * Values.SourceDamage / (Values.TargetDamage + Values.SourceDamage);
                        if (Value <= 0)
                        {
                            Value = 1;
                        }

                        if (Value > CV)
                        {
                            CV = Value;
                        }

                        if (Values.SourceDamage > DV)
                        {
                            DV = Values.SourceDamage;
                        }
                    }
                }
            }

            if (AUnit.GetAbilityEnabled(52))
            {
                Ability = AoWHSSet.Current.FAbilityControl.GetAbility(52);
                Ability.tcGetDamageValueEx(this, Target, ref Values);
                if (Values.SourceDamage > 0)
                {
                    Value = Values.SourceDamage * Values.SourceDamage / (Values.TargetDamage + Values.SourceDamage);
                    if (Value <= 0)
                    {
                        Value = 1;
                    }

                    if (Value > CV)
                    {
                        CV = Value;
                    }

                    if (Values.SourceDamage > DV)
                    {
                        DV = Values.SourceDamage;
                    }
                }
            }
        }
        else
        {
            CV = 0;
            DV = 0;
        }
    }

    // AIGroup.pas:6913
    public override int GetTargetWallCV(CombatObject Target)
    {
        int Best;
        WallSituation Wall;
        AbstractUnit AUnit;
        AbilityTypeList Abilities;
        int Value;
        Ability Ability;
        AbilityDamageValues Values = default;
        AUnit = FUnit;
        Best = 0;
        if (FCombat.FCombatData.FPlayers.GetPlayers(FCombatPlayer).FSideIndex == 1)
        {
            Wall = WallSituation.Source;
        }
        else
        {
            Wall = WallSituation.Target;
        }

        Abilities = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[(int)AbilityType.Combat];
        for (int I = Abilities.Count - 1; I >= 0; I--)
        {
            Ability = Abilities.GetAbility(I);
            if (AUnit.GetAbilityEnabled(Ability.FAbilityID))
            {
                Ability.fcGetDamageValueEx(this, Target, Wall, ref Values);
                if (Values.SourceDamage > 0)
                {
                    Value = Values.SourceDamage * Values.SourceDamage / (Values.TargetDamage + Values.SourceDamage);
                    if (Value <= 0)
                    {
                        Value = 1;
                    }

                    if (Value > Best)
                    {
                        Best = Value;
                    }
                }
            }
        }

        return Best;
    }

    // AIGroup.pas:6946
    public override int GetTargetPriority()
    {
        int I;
        int CV;
        CombatObject Target;
        int Count;
        if (FTargetPriorityInvalid)
        {
            try
            {
                if (FCombat.FCombatData.FWallType == 0 || FCombat.FCombatData.FWallBreached)
                {
                    FTargetPriority = 0;
                    Count = 0;
                    I = FTargetedBy.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if (FTargetedBy.List[I].B == 0)
                        {
                            Count += 1;
                        }
                    }

                    I = FTargets.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        CV = FTargets.List[I].B;
                        if (CV != 0)
                        {
                            Target = FTargets.List[I].A;
                            CV = Round((double)(CV * CV) / Target.FIncomingCombatValue);
                            FTargetPriority += CV;
                        }
                    }

                    FTargetPriority = FTargetPriority * (Count + 2);
                    I = FTargetedBy.Count;
                    // { The original stores normalized priority in the target-strength field. }
                    if (I != 0)
                    {
                        FTargetStrength = FTargetPriority / I;
                    }
                    else
                    {
                        FTargetStrength = 0;
                    }
                }
                else
                {
                    FTargetPriority = 0;
                    Count = 0;
                    I = FTargetedBy.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if (FTargetedBy.List[I].C == 0)
                        {
                            Count += 1;
                        }
                    }

                    I = FTargets.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        CV = FTargets.List[I].C;
                        if (CV != 0)
                        {
                            Target = FTargets.List[I].A;
                            CV = Round((double)(CV * CV) / Target.FIncomingCombatValue);
                            FTargetPriority += CV;
                        }
                    }

                    FTargetPriority = FTargetPriority * (Count + 2);
                    I = FTargetedBy.Count;
                    if (I != 0)
                    {
                        FTargetStrength = FTargetPriority / I;
                    }
                    else
                    {
                        FTargetStrength = 0;
                    }
                }

                FTargetPriorityInvalid = false;
                var commandingTargetPriority = CommandingTargetPriority();
                FTargetPriority += commandingTargetPriority * 2;
            }
            catch
            {
            }
        }

        return FTargetPriority;
    }

    // AIGroup.pas:6850
    public override void SetCombatPlayer(byte Player)
    {
        base.SetCombatPlayer(Player);
        UpdateWallSiegeObjectRegistration();
    }

    // AIGroup.pas:7011
    public override void Activate(Battle ACombat)
    {
        base.Activate(ACombat);
        if (ACombat is FastBattle)
        {
            UpdateWallSiegeObjectRegistration();
        }
    }

    // AIGroup.pas:7017
    public override void Deactivate()
    {
        base.Deactivate();
        if (FCombat is FastBattle)
        {
            UpdateWallSiegeObjectRegistration();
        }
    }

    // AIGroup.pas:7002
    public override void Initialize(Battle ACombat)
    {
        AbstractUnit AUnit;
        base.Initialize(ACombat);
        AUnit = FUnit;
        if (AUnit is Leader || AUnit is Hero)
        {
            FRoundDistance = 8;
        }
        else
        {
            FRoundDistance = 4;
        }
    }

    // AIGroup.pas:6856
    public override int fcRoundDistance()
    {
        int Result = FRoundDistance;
        if (FState.Contains(FastCombatUnitStateElement.Vulnerable))
        {
            Result -= 1;
        }

        return Result;
    }

    // AIGroup.pas:7048
    public override bool fcExecute(int MoveCost)
    {
        FastBattle Combat;
        FastCombatCommandList Commands;
        AbstractUnit AUnit;
        FixedArray_Aow_Units_AoWE_TAbility_1 Ability = default;
        FleeCA Action;
        bool Executed;
        Combat = (FastBattle)FCombat;
        var side = GetSide();
        Commands = new FastCombatCommandList(this, Combat, Combat.FSides[(int)side]);
        AUnit = FUnit;
        for (int I = AUnit.GetAbilityCount() - 1; I >= 0; I--)
        {
            if (AUnit.GetAbilityEnabled(I))
            {
                var ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
                Ability[0] = ability;
                Ability[0].fcPrefetchCombatCommands(Commands);
            }
        }

        if (Commands.GetCount() == 0)
        {
            if (fcBehindWall())
            {
                if (((FastBattle)FCombat).AttackerCanCrushWall() || (double)((FastBattle)FCombat).GetAttackerBalisticCV() > (double)((FastBattle)FCombat).GetDefenderBalisticCV() * 1.5)
                {
                    FState = FState + Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.OutsideWall);
                    AUnit = FUnit;
                    for (int I = AUnit.GetAbilityCount() - 1; I >= 0; I--)
                    {
                        if (AUnit.GetAbilityEnabled(I))
                        {
                            var ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
                            Ability[0] = ability;
                            Ability[0].fcPrefetchCombatCommands(Commands);
                        }
                    }

                    FState = FState - Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.OutsideWall);
                }
            }
        }

        if (GetSide() == CombatSideIndex.Defender && FAttackerCount == 0 && !FCombat.FCombatData.GetSideLeaderInvolved(1))
        {
            for (int I = Commands.GetCount() - 1; I >= 0; I--)
            {
                var commands = Commands.GetCommands(I);
                var hitPoints = GetHitPoints();
                if (commands.FTargetDV >= hitPoints * 75)
                {
                    Commands.Delete(I);
                }
            }
        }

        if (Commands.GetCount() != 0)
        {
            Commands.Sort();
            if (Commands.GetCommands(0).FMoveCost <= MoveCost)
            {
                var commands = Commands.GetCommands(0);
                var commandsCommands = Commands.GetCommands(0);
                Executed = AoWHSSet.Current.FAbilityControl.GetAbility(commandsCommands.FAbilityID).fcExecuteCombatCommand(this, commands);
                if (Commands.GetCommands(0).FAbilityID == 52)
                {
                    UpdateWallSiegeObjectRegistration();
                }

                TurnDone();
            }
            else
            {
                Executed = false;
            }
        }
        else
        {
            if (MoveCost == 2)
            {
                if (FCombatValue == 0 && FIncomingCombatValue != 0 && GetSide() == CombatSideIndex.Attacker && !GetLocked())
                {
                    Action = new FleeCA(null);
                    Action.FObjectID = FID;
                    FCombat.ExecuteCombatAction(Action);
                    Action.Release();
                    Executed = true;
                }
                else
                {
                    Executed = false;
                }
            }
            else
            {
                Executed = false;
            }
        }

        Commands?.Dispose();
        return Executed;
    }

    // AIGroup.pas:7035
    public override bool fcBehindWall()
    {
        bool Result;
        byte Position;
        // { Inner failure shares the validated-position return. }
        if (GetSide() == CombatSideIndex.Defender && !FState.Contains(FastCombatUnitStateElement.OutsideWall))
        {
            Position = FPosition;
            if ((Position & 15) > 7 || (int)(((uint)Position) >> 4) > 7 || FCombat.FCombatData.FParties.GetParty((byte)(int)(((uint)Position) >> 4)).FWall == 0 || FCombat.FCombatData.FWall.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        return false;
    }

    // AIGroup.pas:7029
    public override bool fcWallInBetween(byte Target) => !FState.Contains(FastCombatUnitStateElement.OutsideWall) && FCombat.GetWallInBetween(FPosition, Target);

    // AIGroup.pas:6830
    public override void Show(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        if (FState.Contains(FastCombatUnitStateElement.OutsideWall))
        {
            AoWHSSet.Current.FCombatImages[21].Show(X + 10, Y, Surface);
        }

        base.Show(Frame, Direction, X, Y, Surface);
    }

    // AIGroup.pas:6836
    public override int ShowAttackAnimation(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        if (Direction != 6 && FState.Contains(FastCombatUnitStateElement.OutsideWall))
        {
            AoWHSSet.Current.FCombatImages[21].Show(X + 10, Y, Surface);
        }

        return base.ShowAttackAnimation(Frame, Direction, X, Y, Surface);
    }

    // AIGroup.pas:6843
    public override int ShowHitAnimation(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        if (FState.Contains(FastCombatUnitStateElement.OutsideWall))
        {
            AoWHSSet.Current.FCombatImages[21].Show(X + 10, Y, Surface);
        }

        // { The original uses the attack animation for this hit-animation override. }
        return base.ShowAttackAnimation(Frame, Direction, X, Y, Surface);
    }

    // AIGroup.pas:7023
    public override void DestroyObject()
    {
        base.DestroyObject();
        if (FCombat is FastBattle)
        {
            UpdateWallSiegeObjectRegistration();
        }
    }

    // AIGroup.pas:6792
    public override void SetUnit(AbstractUnit AUnit)
    {
        base.SetUnit(AUnit);
    }

    // AIGroup.pas:6783
    public override void KillUnit()
    {
        AbstractUnit AUnit;
        AUnit = FUnit;
        if (AUnit is Hero && FCombat.FStatus.Contains(CombatStatusElement.Executing))
        {
            var combatData = FCombat.FCombatData;
            ((Hero)AUnit).DropItemsOnMap(AoWHSMap.Current, (sbyte)combatData.FX, (sbyte)combatData.FY, (sbyte)combatData.FL);
        }

        base.KillUnit();
    }

    // AIGroup.pas:6803
    public bool CanCastWallSiegeSpell() => FUnit.GetAbilityEnabled(52) && ((SpellCastingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(52)).CanCastBreachWallSpell(FUnit);

    // AIGroup.pas:6809
    public void UpdateWallSiegeObjectRegistration()
    {
        bool Registered;
        Registered = FStatus.Contains(CombatObjectStatusElement.Activated) && GetSide() == CombatSideIndex.Attacker && !FStatus.Contains(CombatObjectStatusElement.Destroyed) && ((FWallCombatFeatures & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of() || CanCastWallSiegeSpell());
        if (Registered)
        {
            if (!FState.Contains(FastCombatUnitStateElement.WallSiegeObjectRegistered))
            {
                ((FastBattle)FCombat).FWallSiegeObjectCount += 1;
                FState = FState + Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.WallSiegeObjectRegistered);
            }
        }
        else
        {
            if (FState.Contains(FastCombatUnitStateElement.WallSiegeObjectRegistered))
            {
                ((FastBattle)FCombat).FWallSiegeObjectCount -= 1;
                FState = FState - Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.WallSiegeObjectRegistered);
            }
        }
    }

    // intf_P5_07.inc:1031

}
