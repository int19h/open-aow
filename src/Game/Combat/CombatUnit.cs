// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FUnit. FParty. FDisabled. FWallCombatFeatures. FUnitGFXResourceIndex. Inherited: FID.
// FPosition. FStatus. FControlMode
public partial class CombatUnit : CombatObject
{
    // Combat.pas:1084
    public new const int ClassID = 131745;

    // Combat.pas:1071
    // { CombatDID is declared with the shared engine globals. }
    public CombatUnit(ECustomNode owner) : base(owner)
    {
        FControlMode = Set<CombatObjectControlModeElement>.Of(CombatObjectControlModeElement.ConquerObject);
        FUnitGFXResourceIndex = -1;
    }

    public AbstractUnit FUnit;
    public CombatParty FParty;
    public bool FDisabled;
    public Set<WallCombatFeaturesElement> FWallCombatFeatures;
    public int FUnitGFXResourceIndex;

    // Combat.pas:1078
    public override void Dispose()
    {
        SetUnit(null);
        base.Dispose();
    }

    // Combat.pas:1382
    public override void ReadWrite(EStorageStream Stream)
    {
        AbstractUnit U = default;
        base.ReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Translate)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Stream.rwCreateEObject(ref U, null, 20);
            SetUnit(U);
            if (U != null)
            {
                U.Release();
            }
        }
        else
        {
            Stream.rwCreateEObject(ref FUnit, null, 20);
        }

        Stream.rwBool(ref FDisabled, 21);
    }

    // Combat.pas:1408
    public override void GetTargetCVDV(CombatObject Target, ref int CV, ref int DV)
    {
        AbstractUnit U;
        AbilityTypeList Abilities;
        AbilityDamageValues Values = default;
        int V;
        Ability Ability;
        if (!FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            U = FUnit;
            CV = 0;
            DV = 0;
            Abilities = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[(int)AbilityType.Combat];
            for (int I = Abilities.Count - 1; I >= 0; I--)
            {
                Ability = Abilities.GetAbility(I);
                if (U.GetAbilityEnabled(Ability.FAbilityID))
                {
                    Ability.tcGetDamageValueEx(this, Target, ref Values);
                    if (Values.SourceDamage > 0)
                    {
                        V = Values.SourceDamage * Values.SourceDamage / (Values.TargetDamage + Values.SourceDamage);
                        if (V <= 0)
                        {
                            V = 1;
                        }

                        if (V > CV)
                        {
                            CV = V;
                        }

                        if (Values.SourceDamage > DV)
                        {
                            DV = Values.SourceDamage;
                        }
                    }
                }
            }

            if (U.GetAbilityEnabled(52))
            {
                Ability = AoWHSSet.Current.FAbilityControl.GetAbility(52);
                Ability.tcGetDamageValueEx(this, Target, ref Values);
                if (Values.SourceDamage > 0)
                {
                    V = Values.SourceDamage * Values.SourceDamage / (Values.TargetDamage + Values.SourceDamage);
                    if (V <= 0)
                    {
                        V = 1;
                    }

                    if (V > CV)
                    {
                        CV = V;
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

    // Combat.pas:1450
    public override int GetTargetWallCV(CombatObject Target)
    {
        int Result;
        if (Target is CombatUnit && !Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            if (FCombat.FCombatData.FPlayers.GetPlayers(FCombatPlayer).FSideIndex == 1)
            {
                Result = FUnit.GetCombatValue(((CombatUnit)Target).FUnit, 1);
            }
            else
            {
                Result = FUnit.GetCombatValue(((CombatUnit)Target).FUnit, 2);
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:1554
    public override int GetTargetPriority()
    {
        if (FTargetPriorityInvalid)
        {
            base.GetTargetPriority();
            var commandingTargetPriority = CommandingTargetPriority();
            FTargetPriority += commandingTargetPriority;
            return FTargetPriority;
        }

        return FTargetPriority;
    }

    // Combat.pas:1566
    public override int GetTargetStrength()
    {
        if (FTargetStrengthInvalid)
        {
            base.GetTargetStrength();
            var commandingTargetStrength = CommandingTargetStrength();
            FTargetStrength += commandingTargetStrength;
            return FTargetStrength;
        }

        return FTargetStrength;
    }

    // Combat.pas:1136
    public override bool GetEnabled() => FUnit != null && (Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & FStatus) == Set<CombatObjectStatusElement>.Of() && !FDisabled;

    // Combat.pas:1142
    public override bool GetLocked() => FUnit.GetAbilityEnabled(95) || FUnit.GetAbilityEnabled(97) || FUnit.GetAbilityEnabled(92) || FUnit.GetAbilityEnabled(94) || FUnit.GetAbilityEnabled(34);

    // Combat.pas:1459
    public override sbyte GetAttack() => FUnit.GetAttack();

    // Combat.pas:1462
    public override sbyte GetDefense() => FUnit.GetDefense();

    // Combat.pas:1465
    public override sbyte GetResistance() => FUnit.GetResistance();

    // Combat.pas:1468
    public override sbyte GetDamage() => FUnit.GetDamage();

    // Combat.pas:1471
    public override Set<DamageType> GetImmunityTypes() => FUnit.GetImmunityTypes() + FCombat.FCombatData.F_CombatData_22;

    // Combat.pas:1474
    public override Set<DamageType> GetProtectionTypes() => FUnit.GetProtectionTypes();

    // Combat.pas:1282
    public override sbyte GetHits()
    {
        sbyte Result;
        // { Direct field test retains the receiver. }
        // { No cast temporary. }
        if (FUnit != null)
        {
            Result = FUnit.GetHits();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:1289
    public override sbyte GetHitPoints()
    {
        sbyte Result;
        // { Direct field test retains the receiver. }
        // { No cast temporary. }
        if (FUnit != null)
        {
            Result = FUnit.GetHitPoints();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:1296
    public override void SetHitPoints(byte Value)
    {
        // { Direct nil guard. }
        // { No cast temporary. }
        if (FUnit != null)
        {
            FUnit.SetHitPoints((sbyte)Value);
        }
    }

    // Combat.pas:1260
    public override Alignment GetAlignment() => FUnit.GetAlignment();

    // Combat.pas:1257
    public override int GetMoves() => FUnit.GetMoves();

    // Combat.pas:1477
    public override int GetExperience() => FUnit.GetExperience();

    // Combat.pas:1480
    public override void SetExperience(int Value)
    {
        FUnit.SetExperience(Value);
    }

    // Combat.pas:1483
    public override byte GetLevel() => (byte)FUnit.GetUnitLevel();

    // Combat.pas:1095
    public override int GetVisibilityRange()
    {
        int Result;
        int Level;
        AbstractUnit U;
        U = FUnit;
        if (U != null)
        {
            Level = U.GetAbilityLevel(64);
            if (Level != 0)
            {
                Result = Level * 2 + 4;
            }
            else
            {
                Result = 4;
            }

            if (FCombat.FExplorationSite == 1 && !FUnit.GetAbilityEnabled(39))
            {
                Result = (Result + 1) / 2;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:1266
    public override bool GetAbilityEnabled(int AbilityID)
    {
        bool Result;
        if (FUnit != null)
        {
            Result = FUnit.GetAbilityEnabled(AbilityID);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Combat.pas:1271
    public override void SetAbilityEnabled(int AbilityID, bool Enabled)
    {
        FUnit.SetAbSet(AbilityID, (byte)(Enabled ? 1 : 0));
    }

    // Combat.pas:1274
    public override int GetAbilityLevel(int AbilityID) => FUnit.GetAbilityLevel(AbilityID);

    // Combat.pas:1277
    public override int GetAbilityCount()
    {
        int Result;
        if (FUnit != null)
        {
            Result = FUnit.GetAbilityCount();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:1263
    public override AbilityOwner GetAbilityOwner() => FUnit;

    // Combat.pas:1178
    public override void Activate(Battle ACombat)
    {
        base.Activate(ACombat);
        UpdateParty();
        UpdateWallCombatFeatures();
        UpdateUnitGFXResource();
    }

    // Combat.pas:1186
    public override void Deactivate()
    {
        base.Deactivate();
        UpdateParty();
        ReleaseUnitGFXResource();
    }

    // Combat.pas:1396
    public override void ObjectDestroyed(CombatObject Obj)
    {
        CombatUnit C;
        AbstractUnit U;
        base.ObjectDestroyed(Obj);
        if (FDisabled && Obj is CombatUnit)
        {
            C = (CombatUnit)Obj;
            U = C.FUnit;
            if (U != null && U.GetTransporter() && (int)(((uint)C.FPosition) >> 4) == (int)(((uint)FPosition) >> 4))
            {
                DestroyObject();
            }
        }
    }

    // Combat.pas:1193
    public override void Initialize(Battle ACombat)
    {
        base.Initialize(ACombat);
        if (FUnit is Leader)
        {
            ACombat.FCombatData.FInvolvedLeaders.Add((byte)FCombatPlayer);
        }
    }

    // Combat.pas:1200
    public override void Finalize(FinalizeMode Mode)
    {
        CombatParty Party;
        ArmyHS Placed;
        if (!FStatus.Contains(CombatObjectStatusElement.Destroyed) && !FUnit.GetAbilityEnabled(130) && !FUnit.GetAbilityEnabled(131))
        {
            if (Mode != FinalizeMode.All)
            {
                if ((FPosition & 15) <= 7)
                {
                    Party = FCombat.FCombatData.FParties.GetParty((byte)(int)(((uint)FPosition) >> 4));
                    if (Party != null && (sbyte)Party.FPlayerIndex == FCombatPlayer)
                    {
                        var player = GetPlayer();
                        if (AoWHSMap.Current.FPlayers.GetPlayers(player).FGameOverStatus == GameOverStatus.Lost)
                        {
                            Placed = FUnit.Place(Party.F_CombatParty_0C, Party.F_CombatParty_10, Party.F_CombatParty_14, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.NeglectEmptyPlayerLocation));
                        }
                        else
                        {
                            var player2 = GetPlayer();
                            Placed = FUnit.Place(Party.F_CombatParty_0C, Party.F_CombatParty_10, Party.F_CombatParty_14, player2, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.NeglectEmptyPlayerLocation));
                        }

                        if (Placed != null)
                        {
                            Dispose();
                        }
                    }
                }
            }
            else
            {
                if ((FPosition & 15) <= 7)
                {
                    Party = FCombat.FCombatData.FParties.GetParty((byte)(int)(((uint)FPosition) >> 4));
                }
                else
                {
                    Party = null;
                }

                if (Party == null || Party.F_CombatParty_0C != -1)
                {
                    var player = GetPlayer();
                    if (AoWHSMap.Current.FPlayers.GetPlayers(player).FGameOverStatus == GameOverStatus.Lost)
                    {
                        FUnit.Place(FCombat.FCombatData.FX, FCombat.FCombatData.FY, FCombat.FCombatData.FL, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }
                    else
                    {
                        var player2 = GetPlayer();
                        FUnit.Place(FCombat.FCombatData.FX, FCombat.FCombatData.FY, FCombat.FCombatData.FL, player2, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }

                    Dispose();
                }
                else
                {
                    if (Party != null && (sbyte)Party.FPlayerIndex != FCombatPlayer)
                    {
                        var player = GetPlayer();
                        FUnit.Place(FCombat.FCombatData.FX, FCombat.FCombatData.FY, FCombat.FCombatData.FL, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }
                }
            }
        }
        else
        {
            Dispose();
        }
    }

    // Combat.pas:1494
    public override bool SupportCombatMode(Set<CombatMode> Modes)
    {
        bool Result;
        AbstractUnit U;
        int I;
        U = FUnit;
        if (U != null && U.GetHitPoints() > 0)
        {
            Result = false;
            I = FUnit.GetAbilityCount();
            while (I != 0 && !Result)
            {
                I -= 1;
                Result = FUnit.GetAbilityEnabled(I) && Modes.Contains(AoWHSSet.Current.FAbilityControl.GetAbility(I).GetCombatMode(FUnit));
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Combat.pas:1151
    public override Set<WallCombatFeaturesElement> WallCombatFeatures() => FWallCombatFeatures;

    // Combat.pas:1247
    public override void PlaySFX(byte Sound)
    {
        switch (Sound)
        {
            case 0:
                var unitGFXResourceIndex = FUnit.GetUnitGFXResourceIndex();
                var unitGFXResource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex);
                unitGFXResource.FSFXLibraries[2].Play(0);
                break;
            case 1:
                var fUnitUnitGFXResourceIndex = FUnit.GetUnitGFXResourceIndex();
                var fUnitGFXResourcesUnitGFXResource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(fUnitUnitGFXResourceIndex);
                fUnitGFXResourcesUnitGFXResource.FSFXLibraries[2].Play(1);
                break;
            case 2:
                var unitGFXResourceIndex2 = FUnit.GetUnitGFXResourceIndex();
                var unitGFXResource2 = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex2);
                unitGFXResource2.FSFXLibraries[3].Play(0);
                break;
            case 3:
                var unitGFXResourceIndex3 = FUnit.GetUnitGFXResourceIndex();
                var unitGFXResource3 = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex3);
                unitGFXResource3.FSFXLibraries[4].Play(0);
                break;
        }
    }

    // Combat.pas:1115
    public override void NewTurn(byte CombatPlayer)
    {
        AbstractUnit U;
        base.NewTurn(CombatPlayer);
        U = FUnit;
        if (U != null)
        {
            U.TriggerNewCombatTurn(this, (sbyte)CombatPlayer);
        }
    }

    // Combat.pas:1124
    public override void NewRound()
    {
        base.NewRound();
    }

    // Combat.pas:1127
    public override void CombatDone()
    {
        AbstractUnit U;
        base.CombatDone();
        U = FUnit;
        if (U != null)
        {
            U.TriggerCombatDone(this);
        }
    }

    // Combat.pas:1604
    public override void Show(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        AbstractUnit U;
        P2UnitShowInfo Info = default;
        U = FUnit;
        if (U != null)
        {
            Info.Direction = (byte)Direction;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank, UnitOverlayModesElement.Combat);
            Info.Animation = UnitAnimation.StandStill;
            if ((Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & FStatus) != Set<CombatObjectStatusElement>.Of())
            {
                Info.Frame = 0;
                Info.Looped = false;
            }
            else
            {
                Info.Frame = Frame;
                Info.Looped = true;
            }

            U.ShowEx(X, Y, Surface, Info);
            if (FStatus.Contains(CombatObjectStatusElement.Fled))
            {
                AoWHSSet.Current.FCombatImages[7].Show(X + 18, Y + 10, Surface);
            }
        }
    }

    // Combat.pas:1628
    public override int ShowAttackAnimation(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        int Result;
        P2UnitShowInfo Info = default;
        // { Direct field receiver. }
        if (FUnit != null)
        {
            Info.Direction = (byte)Direction;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank, UnitOverlayModesElement.Combat);
            Info.Animation = UnitAnimation.Attack;
            Info.Frame = Frame;
            Info.Looped = false;
            Result = FUnit.ShowEx(X, Y, Surface, Info);
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Combat.pas:1644
    public override int ShowHitAnimation(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        int Result;
        P2UnitShowInfo Info = default;
        // { Direct field receiver. }
        if (FUnit != null)
        {
            Info.Direction = (byte)Direction;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank, UnitOverlayModesElement.Combat);
            Info.Animation = UnitAnimation.Hit;
            Info.Frame = Frame;
            Info.Looped = false;
            Result = FUnit.ShowEx(X, Y, Surface, Info);
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Combat.pas:1159
    public override Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> DamageTypes) => FUnit.ExecuteDamageEffectsRole(DamageTypes);

    // Combat.pas:1544
    public override void Surrender()
    {
        if (GetHitPoints() != 0)
        {
            SetHitPoints(0);
        }

        FStatus = FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        ResetTargets();
        if (FUnit is Leader)
        {
            FCombat.FCombatData.FDefeatedLeaders.Add((byte)FCombatPlayer);
        }

        FCombat.ObjectDestroyed(this);
        if (FUnit != null)
        {
            KillUnit();
        }
    }

    // Combat.pas:1525
    public override void DestroyObject()
    {
        AbstractUnit U;
        if (GetHitPoints() != 0)
        {
            SetHitPoints(0);
        }

        FStatus = FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        U = FUnit;
        if (U != null)
        {
            for (int I = U.GetAbilityCount() - 1; I >= 0; I--)
            {
                if (FUnit.GetAbilityEnabled(I))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(I).CombatObjectDestroyed(this);
                }
            }
        }

        if (FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            ResetTargets();
            if (FUnit is Leader)
            {
                FCombat.FCombatData.FDefeatedLeaders.Add((byte)FCombatPlayer);
            }

            FCombat.ObjectDestroyed(this);
            if (FUnit != null)
            {
                KillUnit();
            }
        }
    }

    // Combat.pas:1154
    public override Set<DamageType> ExecuteDamageEffects(Set<DamageType> DamageTypes) => FUnit.ExecuteCombatDamageEffects(FCombat, DamageTypes);

    // Combat.pas:1486
    public override string IDStr()
    {
        string Result;
        if (FCombat.FCombatData.FPlayers.GetPlayers(FCombatPlayer).FSideIndex == 0)
        {
            Result = "A";
        }
        else
        {
            Result = "D";
        }

        if (FUnit != null)
        {
            var name = FUnit.GetName();
            Result = Result + "(" + IntToStr(FPosition) + ")=" + name;
        }
        else
        {
            Result = Result + "(" + IntToStr(FPosition) + ")";
        }

        return Result;
    }

    // Combat.pas:1358
    public virtual void SetUnit(AbstractUnit AUnit)
    {
        AbstractUnit U;
        CombatParty Party;
        U = FUnit;
        if (U != AUnit)
        {
            if (U != null)
            {
                Party = FParty;
                if (Party != null && FUnit.Owner == Party.FArmy)
                {
                    FUnit.SetOwner(null);
                }

                FUnit.Release();
            }

            FUnit = AUnit;
            U = FUnit;
            if (U != null)
            {
                U.AddRef();
                Party = FParty;
                if (Party != null && !FStatus.Contains(CombatObjectStatusElement.Destroyed))
                {
                    Party.FArmy.AddUnit(FUnit);
                }
            }
        }

        if (FStatus.Contains(CombatObjectStatusElement.Activated))
        {
            UpdateUnitGFXResource();
        }
    }

    // Combat.pas:1522
    public virtual void KillUnit()
    {
        FUnit.Killed();
    }

    // Combat.pas:1087
    public void Setup(byte ID, byte Side, AbstractUnit AUnit, byte Position, byte CombatPlayer)
    {
        FID = ID;
        FPosition = Position;
        SetCombatPlayer(CombatPlayer);
        SetUnit(AUnit);
    }

    // Combat.pas:1164
    public void UpdateWallCombatFeatures()
    {
        AbstractUnit U;
        U = FUnit;
        if (U != null && !FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            FWallCombatFeatures = U.GetWallCombatFeatures();
            if (FUnit.GetAbilityEnabled(52) && ((SpellCastingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(52)).CanCastBreachWallSpell(FUnit))
            {
                FWallCombatFeatures = FWallCombatFeatures + Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching);
            }
        }
        else
        {
            FWallCombatFeatures = Set<WallCombatFeaturesElement>.Of();
        }
    }

    // Combat.pas:1303
    public void SetParty(CombatParty Party)
    {
        CombatParty P;
        if (Party != FParty)
        {
            if (FParty != null && FUnit != null)
            {
                FUnit.SetOwner(null);
            }

            FParty = Party;
            P = FParty;
            if (P != null && FUnit != null)
            {
                FUnit.SetOwner(P.FArmy);
                FParty.FArmy.UpdateFormation();
            }
        }
    }

    // Combat.pas:1319
    public void UpdateParty()
    {
        CombatParty Party;
        if (FStatus.Contains(CombatObjectStatusElement.Activated))
        {
            Party = FCombat.FCombatData.FParties.GetParty((byte)(int)(((uint)FPosition) >> 4));
            if ((byte)FCombatPlayer != Party.FPlayerIndex || FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                Party = null;
            }
        }
        else
        {
            Party = null;
        }

        SetParty(Party);
    }

    // Combat.pas:1331
    public void UpdateUnitGFXResource()
    {
        int Index;
        AbstractUnit U;
        U = FUnit;
        if (U != null)
        {
            Index = U.GetUnitGFXResourceIndex();
        }
        else
        {
            Index = -1;
        }

        if (Index != FUnitGFXResourceIndex)
        {
            ReleaseUnitGFXResource();
            if (Index != -1)
            {
                AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index).AddRef();
                FUnitGFXResourceIndex = Index;
            }
        }
    }

    // Combat.pas:1347
    public void ReleaseUnitGFXResource()
    {
        int Index;
        Index = FUnitGFXResourceIndex;
        if (Index != -1)
        {
            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index).Release();
            FUnitGFXResourceIndex = -1;
        }
    }

    // Combat.pas:1512
    public void SetStatus(Set<CombatObjectStatusElement> Status)
    {
        if (Status.Contains(CombatObjectStatusElement.Fled) && !FStatus.Contains(CombatObjectStatusElement.Fled))
        {
            FStatus = Status;
            FCombat.CheckTerminate();
        }
        else
        {
            FStatus = Status;
        }
    }

    // Combat.pas:1578
    public int CommandingTargetPriority()
    {
        CommandAbilityData Data;
        int Result = 0;
        for (int I = 0; I <= 2; I++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.TCombatUnit_CommandingTargetPriority__CommandAbilityIDs[I]))
            {
                Data = (CommandAbilityData)FUnit.GetAbilityData(CombatAbilityTables.TCombatUnit_CommandingTargetPriority__CommandAbilityIDs[I]);
                if (Data != null)
                {
                    Result += Data.CommandingTargetPriority(FCombat);
                }
            }
        }

        return Result;
    }

    // Combat.pas:1591
    public int CommandingTargetStrength()
    {
        CommandAbilityData Data;
        int Result = 0;
        for (int I = 0; I <= 2; I++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.TCombatUnit_CommandingTargetStrength__CommandAbilityIDs[I]))
            {
                Data = (CommandAbilityData)FUnit.GetAbilityData(CombatAbilityTables.TCombatUnit_CommandingTargetStrength__CommandAbilityIDs[I]);
                if (Data != null)
                {
                    Result += Data.CommandingTargetStrength(FCombat);
                }
            }
        }

        return Result;
    }

    // intf_P3_01.inc:244

}
