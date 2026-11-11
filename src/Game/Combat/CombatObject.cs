// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FCombat. FID. FTargets. FTargetedBy. outgoing CV/wall CV. incoming CV/wall CV. target counts.
// attacker counts. target priority/cache flag. strength/cache flag. FCombatPlayer. FPosition.
// FStatus. FControlMode
public partial class CombatObject : EngineObject
{

    // Combat.pas:1922
    public CombatObject(ECustomNode owner) : base(owner)
    {
        FTargets = new QuadItemList<CombatObject>();
        FTargetedBy = new QuadItemList<CombatObject>();
        FCombatPlayer = -1;
    }

    public Battle FCombat;
    public byte FID;
    public QuadItemList<CombatObject> FTargets;
    public QuadItemList<CombatObject> FTargetedBy;
    public int FCombatValue;
    public int FWallCombatValue;
    public int FIncomingCombatValue;
    public int FIncomingWallCombatValue;
    public int FTargetCount;
    public int FWallTargetCount;
    public int FAttackerCount;
    public int FWallAttackerCount;
    public int FTargetPriority;
    public bool FTargetPriorityInvalid;
    public int FTargetStrength;
    public bool FTargetStrengthInvalid;
    public sbyte FCombatPlayer;
    public byte FPosition;
    public Set<CombatObjectStatusElement> FStatus;
    public Set<CombatObjectControlModeElement> FControlMode;

    // Combat.pas:1930
    public override void Dispose()
    {
        if (FStatus.Contains(CombatObjectStatusElement.Activated))
        {
            Deactivate();
        }

        FTargets?.Dispose();
        FTargetedBy?.Dispose();
        base.Dispose();
    }

    // Combat.pas:5620
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FPosition, 10);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FCombatPlayer), 12);
        RwSet(ref FStatus, 1, 13, Stream.rwData, 0);
        Stream.rwData(Bytes.Of(ref FID, 1), 1, 14);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            FStatus = FStatus - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated);
        }
    }

    // Combat.pas:5524
    public virtual void GetTargetCVDV(CombatObject Target, ref int CV, ref int DV)
    {
        CV = 0;
        DV = 0;
    }

    // Combat.pas:5527
    public virtual int GetTargetWallCV(CombatObject Target) => 0;

    // Combat.pas:1938
    public virtual void TargetsChanged()
    {
        FTargetPriorityInvalid = true;
        FTargetStrengthInvalid = true;
    }

    // Combat.pas:5530
    public virtual int GetTargetPriority()
    {
        int I;
        int CV;
        CombatObject Target;
        int Count;
        if (FTargetPriorityInvalid)
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

            FTargetPriority = (Count + 2) * FTargetPriority;
            I = FTargetedBy.Count;
            // { Original bug: the normalized priority is stored in FTargetStrength. }
            if (I != 0)
            {
                FTargetStrength = FTargetPriority / I;
            }
            else
            {
                FTargetStrength = 0;
            }

            FTargetPriorityInvalid = false;
        }

        return FTargetPriority;
    }

    // Combat.pas:5559
    public virtual int GetTargetStrength()
    {
        if (FTargetStrengthInvalid)
        {
            FTargetStrength = 0;
            for (int I = FTargets.Count - 1; I >= 0; I--)
            {
                FTargetStrength += FTargets.List[I].D;
            }

            if (FTargetStrength != 0)
            {
                FTargetStrength = FTargetStrength / FTargets.Count;
            }

            FTargetStrengthInvalid = false;
            return FTargetStrength;
        }

        return FTargetStrength;
    }

    // Combat.pas:5633
    public virtual bool GetEnabled() => (Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & FStatus) == Set<CombatObjectStatusElement>.Of();

    // Combat.pas:5706
    public virtual bool GetLocked() => false;

    // Combat.pas:5709
    public virtual void SetCombatPlayer(byte Player)
    {
        bool ChangedSide;
        Battle Combat;
        Battle NewCombat;
        if (FStatus.Contains(CombatObjectStatusElement.Activated))
        {
            bool condition = Player != 255;
            if (condition)
            {
                var players = FCombat.FCombatData.FPlayers.GetPlayers((sbyte)Player);
                var side = GetSide();
                condition = (CombatSideIndex)players.FSideIndex != side;
            }

            ChangedSide = condition;
            if (FCombatPlayer != -1)
            {
                Combat = FCombat;
                var players = Combat.FCombatData.FPlayers.GetPlayers(FCombatPlayer);
                Combat.FSides[(int)(CombatSideIndex)players.FSideIndex].RemoveChild(this);
            }

            FCombatPlayer = (sbyte)Player;
            if (FCombatPlayer != -1)
            {
                NewCombat = FCombat;
                var players = NewCombat.FCombatData.FPlayers.GetPlayers(FCombatPlayer);
                NewCombat.FSides[(int)(CombatSideIndex)players.FSideIndex].AddChild(this);
            }

            if (ChangedSide)
            {
                ResetTargets();
                SetupTargets();
                FCombat.ObjectSideChanged(this);
            }
        }
        else
        {
            FCombatPlayer = (sbyte)Player;
        }
    }

    // Combat.pas:5737
    public virtual sbyte GetAttack() => 0;

    // Combat.pas:5740
    public virtual sbyte GetDefense() => 0;

    // Combat.pas:5743
    public virtual sbyte GetResistance() => 0;

    // Combat.pas:5746
    public virtual sbyte GetDamage() => 0;

    // Combat.pas:5749
    public virtual Set<DamageType> GetImmunityTypes() => Set<DamageType>.Of();

    // Combat.pas:5752
    public virtual Set<DamageType> GetProtectionTypes() => Set<DamageType>.Of();

    // Combat.pas:5758
    public virtual sbyte GetHits() => 0;

    // Combat.pas:5764
    public virtual sbyte GetHitPoints() => 0;

    // Combat.pas:5761
    public virtual void SetHitPoints(byte Value)
    {
    }

    // Combat.pas:5779
    public virtual Alignment GetAlignment() => Alignment.None;

    // Combat.pas:5755
    public virtual int GetMoves() => 0;

    // Combat.pas:5767
    public virtual int GetExperience() => 0;

    // Combat.pas:5770
    public virtual void SetExperience(int Value)
    {
    }

    // Combat.pas:5773
    public virtual byte GetLevel() => 0;

    // Combat.pas:5617
    public virtual int GetVisibilityRange() => 0;

    // Combat.pas:5785
    public virtual bool GetAbilityEnabled(int AbilityID) => false;

    // Combat.pas:5788
    public virtual void SetAbilityEnabled(int AbilityID, bool Enabled)
    {
    }

    // Combat.pas:5791
    public virtual int GetAbilityLevel(int AbilityID) => 0;

    // Combat.pas:5794
    public virtual int GetAbilityCount() => 0;

    // Combat.pas:5782
    public virtual AbilityOwner GetAbilityOwner() => null;

    // Combat.pas:5672
    public virtual void Activate(Battle ACombat)
    {
        byte Player;
        Battle Combat;
        FCombat = ACombat;
        FStatus = FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated);
        Player = (byte)FCombatPlayer;
        if (Player != 255)
        {
            Combat = FCombat;
            // { This eliminated copy preserves the original saved registers. }
            var players = Combat.FCombatData.FPlayers.GetPlayers((sbyte)Player);
            Combat.FSides[(int)(CombatSideIndex)players.FSideIndex].AddChild(this);
        }
    }

    // Combat.pas:5685
    public virtual void Deactivate()
    {
        byte Player;
        Battle Combat;
        FStatus = FStatus - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated);
        Player = (byte)FCombatPlayer;
        if (Player != 255)
        {
            Combat = FCombat;
            // { This eliminated copy preserves the original saved registers. }
            var players = Combat.FCombatData.FPlayers.GetPlayers((sbyte)Player);
            Combat.FSides[(int)(CombatSideIndex)players.FSideIndex].RemoveChild(this);
        }
    }

    // Combat.pas:5813
    public virtual void ObjectDestroyed(CombatObject Obj)
    {
        if (Obj.GetSide() != GetSide())
        {
            RemoveTarget(Obj);
        }
    }

    // Combat.pas:5592
    public virtual void ObjectSideChanged(CombatObject Obj)
    {
        int CV = default;
        int DV = default;
        int WallCV;
        if (Obj.GetSide() != GetSide())
        {
            GetTargetCVDV(Obj, ref CV, ref DV);
            if (FCombat.FCombatData.FWallType != 0)
            {
                WallCV = GetTargetWallCV(Obj);
            }
            else
            {
                WallCV = 0;
            }

            AddTarget(Obj, CV, WallCV, DV);
        }
        else
        {
            RemoveTarget(Obj);
        }
    }

    // Combat.pas:5697
    public virtual void Initialize(Battle ACombat)
    {
        FCombat = ACombat;
    }

    // Combat.pas:5700
    public virtual void Finalize(FinalizeMode Mode)
    {
        Dispose();
    }

    // Combat.pas:5614
    public virtual int fcRoundDistance() => 0;

    // Combat.pas:5605
    public virtual bool fcExecute(int MoveCost) => false;

    // Combat.pas:5608
    public virtual bool fcBehindWall() => false;

    // Combat.pas:5611
    public virtual bool fcWallInBetween(byte Target) => false;

    // Combat.pas:5818
    public virtual bool SupportCombatMode(Set<CombatMode> Modes) => false;

    // Combat.pas:5630
    public virtual Set<WallCombatFeaturesElement> WallCombatFeatures() => Set<WallCombatFeaturesElement>.Of();

    // Combat.pas:5703
    public virtual void PlaySFX(byte Sound)
    {
    }

    // Combat.pas:5654
    public virtual void NewTurn(byte CombatPlayer)
    {
        if (CombatPlayer == (byte)FCombatPlayer)
        {
            FStatus = FStatus - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.TurnDone);
        }
    }

    // Combat.pas:5657
    public virtual void NewRound()
    {
    }

    // Combat.pas:5660
    public virtual void CombatDone()
    {
    }

    // Combat.pas:5990
    public virtual void Show(int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
    }

    // Combat.pas:5993
    public virtual int ShowAttackAnimation(int Frame, int Direction, int X, int Y, in LockedSurface Surface) => 0;

    // Combat.pas:5996
    public virtual int ShowHitAnimation(int Frame, int Direction, int X, int Y, in LockedSurface Surface) => 0;

    // Combat.pas:5868
    public virtual int DoDamage(CombatObject Target, int Damage)
    {
        int Result;
        if (Damage != 0 && Target.GetEnabled())
        {
            Result = Target.ExecuteDamage(Damage);
            if (Target.FStatus.Contains(CombatObjectStatusElement.Destroyed) && !Target.GetAbilityEnabled(129) && !Target.GetAbilityEnabled(128))
            {
                Damage = GetExperience();
                Damage += Target.GetLevel();
                SetExperience(Damage);
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:5821
    public virtual DamageType GetOptimalDamageType(Set<DamageType> Types)
    {
        Set<DamageType> Available;
        Set<DamageType> Protection;
        DamageType Result = DamageType.None;
        Available = Types - GetImmunityTypes();
        if (Available != Set<DamageType>.Of())
        {
            Protection = GetProtectionTypes();
            if (Protection == Set<DamageType>.Of() || Available - Protection == Set<DamageType>.Of())
            {
                while (!Available.Contains(Result))
                {
                    Result = (DamageType)((int)Result - 1);
                }
            }
            else
            {
                while (!Available.Contains(Result) || Protection.Contains(Result))
                {
                    Result = (DamageType)((int)Result - 1);
                }
            }
        }

        return Result;
    }

    // Combat.pas:5836
    public virtual int ExecuteDamageRole(int AAttack, int Damage, Set<DamageType> DamageTypes)
    {
        Set<DamageType> Types;
        int Attack;
        int Result = 0;
        Types = DamageTypes - GetImmunityTypes();
        if (Types != Set<DamageType>.Of())
        {
            Attack = AAttack - GetDefense();
            Result = AbilityRules.ExecuteDamageRole(Damage, Attack);
            if (Types - GetProtectionTypes() == Set<DamageType>.Of())
            {
                Result = (Result + 1) / 2;
            }
        }

        return Result;
    }

    // Combat.pas:5849
    public virtual int ExecuteDamageRoleEx(int AAttack, int Damage, Set<DamageType> DamageTypes, byte DamageMode)
    {
        Set<DamageType> Types;
        int Attack;
        int Result = 0;
        Types = DamageTypes - GetImmunityTypes();
        if (Types != Set<DamageType>.Of())
        {
            if (DamageMode == 1)
            {
                Attack = AAttack - GetResistance();
            }
            else
            {
                Attack = AAttack - GetDefense();
            }

            Result = AbilityRules.ExecuteDamageRole(Damage, Attack);
            if (Types - GetProtectionTypes() == Set<DamageType>.Of())
            {
                Result = (Result + 1) / 2;
            }
        }

        return Result;
    }

    // Combat.pas:5865
    public virtual Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> DamageTypes) => Set<DamageType>.Of();

    // Combat.pas:5895
    public virtual void Surrender()
    {
        SetHitPoints(0);
        FStatus = FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        ResetTargets();
        FCombat.ObjectDestroyed(this);
    }

    // Combat.pas:5881
    public virtual void DestroyObject()
    {
        SetHitPoints(0);
        FStatus = FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        for (int I = GetAbilityCount() - 1; I >= 0; I--)
        {
            if (GetAbilityEnabled(I))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(I).CombatObjectDestroyed(this);
            }
        }

        if (FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            ResetTargets();
            FCombat.ObjectDestroyed(this);
        }
    }

    // Combat.pas:5903
    public virtual int ExecuteDamage(int Damage)
    {
        int Result;
        int Hits;
        Assert(Damage >= 0 && Damage < 50, "Invalid Damage Value (TCombatUnit.ExecuteDamage)", "Combat.pas", 5982);
        Hits = GetHitPoints();
        if (Hits < Damage)
        {
            Result = Hits;
        }
        else
        {
            Result = Damage;
        }

        SetHitPoints((byte)(Hits - Result));
        if (Damage != 0)
        {
            FStatus = FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Damaged);
        }

        if (Result != 0 && GetHitPoints() == 0)
        {
            DestroyObject();
        }

        return Result;
    }

    // Combat.pas:5862
    public virtual Set<DamageType> ExecuteDamageEffects(Set<DamageType> DamageTypes) => Set<DamageType>.Of();

    // Combat.pas:5663
    public virtual void TurnDone()
    {
        CombatObjectTurnDoneCombatAction A;
        A = new CombatObjectTurnDoneCombatAction(null);
        A.F_CombatObjectTurnDoneCA_0C = FID;
        FCombat.ExecuteCombatAction(A);
        A.Release();
    }

    // Combat.pas:5776
    public virtual string IDStr() => "TCombatObject";

    // Combat.pas:1941
    public void AddTarget(CombatObject Target, int CV, int WallCV, int DV)
    {
        Assert(FTargets.IndexOfItemA(Target) == -1, "target already exists use update target", "Combat.pas", 5473);
        FCombatValue += CV;
        FWallCombatValue += WallCV;
        FTargets.AddABCD(Target, CV, WallCV, DV);
        Target.FTargetedBy.AddABCD(this, CV, WallCV, DV);
        Target.FIncomingCombatValue += CV;
        Target.FIncomingWallCombatValue += WallCV;
        if (CV > 0)
        {
            FTargetCount += 1;
            Target.FAttackerCount += 1;
        }

        if (WallCV > 0)
        {
            FWallTargetCount += 1;
            Target.FWallAttackerCount += 1;
        }

        TargetsChanged();
        Target.TargetsChanged();
    }

    // Combat.pas:5488
    public void DeleteTarget(int Index)
    {
        CombatObject Target;
        int CV;
        int DV;
        if (Index != -1)
        {
            Target = FTargets.List[Index].A;
            CV = FTargets.List[Index].B;
            DV = FTargets.List[Index].C;
            FCombatValue -= CV;
            FWallCombatValue -= DV;
            FTargets.Delete(Index);
            if (CV > 0)
            {
                FTargetCount -= 1;
            }

            if (DV > 0)
            {
                FWallTargetCount -= 1;
            }

            TargetsChanged();
            Index = Target.FTargetedBy.IndexOfItemA(this);
            if (Index != -1)
            {
                Target.FIncomingCombatValue -= Target.FTargetedBy.List[Index].B;
                // { Original bug: FIncomingWallCombatValue is not decremented. }
                Target.FTargetedBy.Delete(Index);
                if (CV > 0)
                {
                    Target.FAttackerCount -= 1;
                }

                if (DV > 0)
                {
                    Target.FWallAttackerCount -= 1;
                }

                Target.TargetsChanged();
            }
        }
    }

    // Combat.pas:5515
    public void RemoveTarget(CombatObject Target)
    {
        DeleteTarget(FTargets.IndexOfItemA(Target));
    }

    // Combat.pas:5518
    public void ResetTargets()
    {
        for (int I = FTargets.Count - 1; I >= 0; I--)
        {
            DeleteTarget(I);
        }
    }

    // Combat.pas:5575
    public void SetupTargets()
    {
        int CV = default;
        int DV = default;
        int WallCV;
        CombatObject Target;
        CombatSide Side;
        var opponentSide = GetOpponentSide();
        Side = FCombat.FSides[(int)opponentSide];
        for (int I = Side.Count - 1; I >= 0; I--)
        {
            Target = Side.GetObjects(I);
            if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                GetTargetCVDV(Target, ref CV, ref DV);
                if (FCombat.FCombatData.FWallType != 0)
                {
                    WallCV = GetTargetWallCV(Target);
                }
                else
                {
                    WallCV = 0;
                }

                AddTarget(Target, CV, WallCV, DV);
            }
        }
    }

    // Combat.pas:5636
    public CombatSideIndex GetSide()
    {
        CombatSideIndex Result;
        sbyte Player;
        Player = FCombatPlayer;
        if (Player != -1)
        {
            Result = (CombatSideIndex)FCombat.FCombatData.FPlayers.GetPlayers(Player).FSideIndex;
        }
        else
        {
            Result = CombatSideIndex.Neutral;
        }

        return Result;
    }

    // Combat.pas:5645
    public CombatSideIndex GetOpponentSide()
    {
        CombatSideIndex Result;
        sbyte Player;
        Player = FCombatPlayer;
        if (Player != -1)
        {
            Result = (CombatSideIndex)(FCombat.FCombatData.FPlayers.GetPlayers(Player).FSideIndex ^ 1);
        }
        else
        {
            Result = CombatSideIndex.Neutral;
        }

        return Result;
    }

    // Combat.pas:5797
    public sbyte GetPlayer()
    {
        sbyte Result;
        byte Player;
        Player = (byte)FCombatPlayer;
        if (Player != 255)
        {
            Result = FCombat.FCombatData.FPlayers.GetPlayers((sbyte)Player).FPlayer;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Combat.pas:5805
    public sbyte GetPlayerIR()
    {
        sbyte Result;
        byte Player;
        Player = (byte)FCombatPlayer;
        if (Player != 255)
        {
            Result = (sbyte)FCombat.FCombatData.FPlayers.GetPlayers((sbyte)Player).FIndependentRelation;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    internal static Set<DamageType> MeleeDamageTypes(CombatObject Source)
    {
        Set<DamageType> Result;
        if (Source is CombatUnit)
        {
            Result = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(((CombatUnit)Source).FUnit);
        }
        else
        {
            Result = Set<DamageType>.Of();
        }

        return Result;
    }

    internal static int StrikeDV(CombatObject Source, CombatObject Target)
    {
        int Result;
        sbyte Attack;
        sbyte Damage;
        Set<DamageType> Types;
        if (Source.GetAbilityEnabled(21))
        {
            Attack = Source.GetAttack();
            Damage = Source.GetDamage();
            Types = MeleeDamageTypes(Source);
            bool condition = Source.GetAbilityEnabled(146) || Source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = Target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                Attack = (sbyte)(Attack + 2);
                Damage = (sbyte)(Damage + 2);
            }

            bool test = Source.GetAbilityEnabled(147) || Source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = Target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                Attack = (sbyte)(Attack + 2);
                Damage = (sbyte)(Damage + 2);
            }

            if (Source.GetAbilityEnabled(112) && Target.GetAbilityEnabled(63))
            {
                Attack = (sbyte)(Attack + 3);
                Damage = (sbyte)(Damage + 3);
            }

            Result = AbilityRules.StatisticsToLimitedDV(Attack, Damage, Types, Target.GetHitPoints(), Target.GetDefense(), Target.GetResistance(), Target.GetImmunityTypes(), Target.GetProtectionTypes());
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static StrikeCombatAction CreateStrikeCA(CombatObject Source, CombatObject Target, bool Defensive)
    {
        StrikeCombatAction Result;
        sbyte Attack;
        Set<DamageType> Types;
        sbyte Damage;
        if (Source.GetAbilityEnabled(21))
        {
            Attack = Source.GetAttack();
            Damage = Source.GetDamage();
            Types = MeleeDamageTypes(Source);
            bool condition = Source.GetAbilityEnabled(146) || Source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = Target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                Attack = (sbyte)(Attack + 2);
                Damage = (sbyte)(Damage + 2);
            }

            bool test = Source.GetAbilityEnabled(147) || Source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = Target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                Attack = (sbyte)(Attack + 2);
                Damage = (sbyte)(Damage + 2);
            }

            if (Source.GetAbilityEnabled(112) && Target.GetAbilityEnabled(63))
            {
                Attack = (sbyte)(Attack + 3);
                Damage = (sbyte)(Damage + 3);
            }

            Result = new StrikeCombatAction(null);
            Result.Generate(Source, Target, Attack, Damage, Types);
            Result.SetDefensive(Defensive);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // intf_P3_01.inc:157

    // intf_P3_01.inc:157

}
