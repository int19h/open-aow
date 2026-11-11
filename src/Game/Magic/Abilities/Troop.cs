// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// FRefCount. FUnitID. FGroupID. FGroup: TAIGroup. FUnitStatus
public partial class Troop : AbilityOwner
{
    // SetOwner observes this count before the base body, as in Unit.PAS:95.
    public int FRefCount = 1;
    public int FUnitID;
    public int FGroupID;
    public AIGroup FGroup;
    // group
    public sbyte FPlayer;
    public Set<UnitStatusElement> FUnitStatus;
    public sbyte FMoraleValue;
    public ByteArray10 FOffensiveStrength;
    public byte FMaxOffensiveStrength;
    public int FDefensiveStrength;
    public byte F_AbstractUnit_38;

    // Unit.PAS:108
    public override void Dispose()
    {
        if (FUnitStatus.Contains(UnitStatusElement.Activated))
        {
            Deactivate();
        }

        if (AoWHSMap.Current != null)
        {
            AoWHSMap.Current.UnitDestroyed(this);
        }

        base.Dispose();
    }

    // Unit.PAS:518
    public override void SetOwner(ECustomNode AOwner)
    {
        if (AOwner != FOwner)
        {
            AddRef();
            if (FOwner != null)
            {
                FOwner.RemoveChild(this);
            }

            FOwner = AOwner;
            if (FOwner != null)
            {
                if (FOwner.AddChild(this) == -1)
                {
                    FOwner = null;
                }
            }

            Release();
        }
    }

    // Unit.PAS:1762
    public override void ReadWrite(EStorageStream Stream)
    {
        EngineObject Legacy;
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGroupID, 4);
        Stream.rwIntegerEx(ref FUnitID, -1, 5);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMoraleValue), 6);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 47);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            try
            {
                Legacy = null;
                Stream.rwCreateEObject(ref Legacy, null, 48);
                if (Legacy != null)
                {
                    F_AbstractUnit_38 = (byte)((GroupControl)(object)Legacy).Behavior();
                }
                else
                {
                    F_AbstractUnit_38 = 0;
                }
            }
            catch
            {
            }
        }
        else
        {
            // { Original retains the obsolete object loaded for behavior conversion. }
            F_AbstractUnit_38 = 0;
        }
    }

    // Unit.PAS:95
    public Troop(ECustomNode owner) : base(owner)
    {
        FGroupID = 0;
        FUnitID = -1;
    }

    // Unit.PAS:127
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Unit.PAS:132
    public override int Release()
    {
        int Count;
        FRefCount -= 1;
        Count = FRefCount;
        int Result = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Unit.PAS:1494
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Payload.Int);
            return;
        }
        else
        {
            if (Msg.MsgID == 537001987)
            {
                NewTurnDone((sbyte)Msg.Payload.Int);
            }
            else
            {
                if (Msg.MsgID == 537001985)
                {
                    NewDay();
                }
                else
                {
                    if (Msg.MsgID == 537002240)
                    {
                        ValidateLifeMastery();
                    }
                    else
                    {
                        if (Msg.MsgID == 537002241)
                        {
                            ValidateDeathMastery();
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:590
    public override string GetOwnerName() => AbilityRules.TranslateRStr(GameText.P2InherentRStr);

    // Unit.PAS:171
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => TroopConstants.TAbstractUnit_GetAbilitySelectionTypes__SelectionTypes;

    // Unit.PAS:1411
    public override void Changed()
    {
        if (FUnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateMoraleValue();
            UpdateOffensiveStrength();
            UpdateDefensiveStrength();
            UpdateEnchanted();
            if (AoWHSMap.Current != null)
            {
                AoWHSMap.Current.UnitChanged(this);
            }
        }

        if (Owner != null && Owner is UnitList)
        {
            UnitListOwner.Changed();
        }
    }

    // Unit.PAS:791
    public virtual byte GetUpkeep()
    {
        if (!GetAbilityEnabled(155))
        {
            return (byte)((GetUnitLevel() + 1) * 2);
        }

        return 0;
    }

    // Unit.PAS:809
    public virtual sbyte GetUnitLevel() => 0;

    // Unit.PAS:200
    public virtual sbyte GetRace() => -1;

    // Unit.PAS:866
    public virtual sbyte GetInherentAttack() => 0;

    // Unit.PAS:874
    public virtual sbyte GetInherentDefense() => 0;

    // Unit.PAS:882
    public virtual sbyte GetInherentDamage() => 0;

    // Unit.PAS:890
    public virtual sbyte GetInherentResistance() => 0;

    // Unit.PAS:534
    public virtual bool GetInherentAbility(int AbilityID)
    {
        if (this.GetAbSet(AbilityID))
        {
            return AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetInherent(this);
        }

        return false;
    }

    // Unit.PAS:543
    public virtual int GetInherentAbilityLevel(int AbilityID)
    {
        if (this.GetAbSet(AbilityID))
        {
            return AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetInherentLevel(this);
        }

        return 0;
    }

    // Unit.PAS:870
    public virtual sbyte GetAttack() => 0;

    // Unit.PAS:878
    public virtual sbyte GetDefense() => 0;

    // Unit.PAS:886
    public virtual sbyte GetDamage() => 0;

    // Unit.PAS:894
    public virtual sbyte GetResistance() => 0;

    // Unit.PAS:898
    public virtual sbyte GetHits() => 0;

    // Unit.PAS:902
    public virtual sbyte GetMoves() => 0;

    // Unit.PAS:817
    public virtual sbyte GetMovePoints() => 0;

    // Unit.PAS:821
    public virtual void SetMovePoints(sbyte Value)
    {
    }

    // Unit.PAS:823
    public virtual sbyte GetHitPoints() => 0;

    // Unit.PAS:827
    public virtual void SetHitPoints(sbyte Value)
    {
        UpdateDefensiveStrength();
    }

    // Unit.PAS:973
    public virtual Set<MoveType> GetMoveTypes() => GetAbMoveTypesAll();

    // Unit.PAS:852
    public virtual Set<DamageType> GetImmunityTypes() => this.GetAbImmunityTypesAll() + Set<DamageType>.Of(DamageType.Wall);

    // Unit.PAS:858
    public virtual Set<DamageType> GetProtectionTypes() => this.GetAbProtectionTypesAll();

    // Unit.PAS:977
    public virtual LibraryImage GetFace() => null;

    // Unit.PAS:981
    public virtual string GetName() => "";

    // Unit.PAS:787
    public virtual Alignment GetAlignment() => Alignment.Neutral;

    // Unit.PAS:985
    public virtual LibraryImage GetPreviewImage() => null;

    // Unit.PAS:813
    public virtual byte GetTransportCapacity() => 0;

    // Unit.PAS:846
    public virtual bool GetTransporter() => GetTransportCapacity() != 0;

    // Unit.PAS:1205
    public virtual UnitGender GetGender() => UnitGender.Neutral;

    // Unit.PAS:119
    public virtual BloodType GetBloodType() => BloodType.Red;

    // Unit.PAS:862
    public virtual UnitType GetUnitType() => UnitType.Humanoid;

    // Unit.PAS:989
    public virtual int GetUnitGFXResourceIndex() => -1;

    // Unit.PAS:1480
    public virtual bool CanAddToList(UnitList AList) => true;

    // Unit.PAS:179
    public virtual Set<WallCombatFeaturesElement> GetWallCombatFeatures()
    {
        Set<WallCombatFeaturesElement> Result = Set<WallCombatFeaturesElement>.Of();
        if (GetAbilityEnabled(1) || GetAbilityEnabled(165) || GetAbilityEnabled(36) || GetAbilityEnabled(66))
        {
            Result = Result + Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);
        }

        if (GetAbilityEnabled(117) || GetAbilityEnabled(24) || GetAbilityEnabled(65) || GetAbilityEnabled(44))
        {
            Result = Result + Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);
        }

        return Result;
    }

    // Unit.PAS:123
    public virtual int GetCampaignTransferPoints() => GetObtainValue();

    // Unit.PAS:910
    public virtual sbyte GetCastingPointsMax() => 0;

    // Unit.PAS:914
    public virtual sbyte GetCastingPoints() => 0;

    // Unit.PAS:918
    public virtual void SetCastingPoints(sbyte Value)
    {
    }

    // Unit.PAS:906
    public virtual int GetPowerGeneration() => 0;

    // Unit.PAS:1484
    public virtual void NewDay()
    {
    }

    // Unit.PAS:1355
    public virtual void NewTurn(sbyte Player)
    {
        sbyte Heal;
        if (Player == FPlayer)
        {
            SetMovePoints(GetMoves());
            if (GetHits() != GetHitPoints() && !GetAbilityEnabled(93))
            {
                if (!GetAbilityEnabled(49))
                {
                    Heal = (sbyte)Round((double)GetHits() * 0.1);
                    if (Heal == 0)
                    {
                        Heal = 1;
                    }
                }
                else
                {
                    Heal = GetHits();
                }

                SetHitPoints((sbyte)(GetHitPoints() + Heal));
            }

            if (GetAbilityEnabled(109))
            {
                UpdatePathOfFrost();
            }
        }
    }

    // Unit.PAS:1375
    public virtual void NewTurnDone(sbyte Player)
    {
        if (Player == FPlayer && FPlayer > 0)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
            {
                SetFireMasteryProtection();
            }

            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.EarthMastery))
            {
                SetEarthMasteryStoneSkin();
            }

            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                SetLifeMasteryBlessing();
            }

            if ((((byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 16) != 0)
            {
                ExecuteLifeMasteryFearRole();
            }

            if ((((byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 32) != 0)
            {
                ExecuteDeathMasteryCurseRole();
            }
        }
    }

    // Unit.PAS:578
    public virtual int GetAbilityLevel(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetLevel(this);

    // Unit.PAS:552
    public virtual bool GetAbilityEnabled(int AbilityID)
    {
        if (GetAbilitySet(AbilityID))
        {
            return AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetEnabled(this);
        }

        return false;
    }

    // Unit.PAS:574
    public virtual string GetAbilityName(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetName(this);

    // Unit.PAS:582
    public virtual AbilityOwner GetAbilityOwner(int AbilityID)
    {
        if (this.GetAbSet(AbilityID))
        {
            return this;
        }

        return null;
    }

    // Unit.PAS:530
    public virtual int GetAbilityCount() => FAbilityCount;

    // Unit.PAS:747
    public virtual int GetExperience() => 0;

    // Unit.PAS:751
    public virtual void SetExperience(int Value)
    {
    }

    // Unit.PAS:753
    public virtual int GetNextLevelExperience() => 0;

    // Unit.PAS:739
    public virtual StringCollection GetDescription() => null;

    // Unit.PAS:494
    public virtual void SetPlayer(sbyte Player)
    {
        UnitUserList List;
        if (Player != FPlayer)
        {
            AddRef();
            ResetLifeMasteryBlessing();
            ResetLifeMasteryFear();
            ResetEarthMasteryStoneSkin();
            ResetFireMasteryProtection();
            List = FGroup;
            if (List != null)
            {
                List.DeleteUnit(this);
            }

            FPlayer = Player;
            Release();
        }
    }

    // Unit.PAS:513
    public virtual sbyte GetIndependentRelation()
    {
        sbyte Result;
        if (GetRace() >= 0)
        {
            Result = GetRace();
        }
        else
        {
            Result = AlignmentTables.AlignmentToIR(GetAlignment());
        }

        return Result;
    }

    // Unit.PAS:350
    public virtual int GetObtainValue() => 0;

    // Unit.PAS:346
    public virtual UnitSize GetUnitSize() => UnitSize.Medium;

    // Unit.PAS:260
    public virtual int GetUnitMoraleValue(sbyte Player, sbyte IndependentRelation)
    {
        sbyte Race;
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        TerrainType TerrainType;
        byte RawTerrain;
        int RaceIndex;
        int Morale;
        if (Player >= 0 && GetUnitType() != UnitType.Machine && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Race = GetRace();
            Morale = GetRelationValue(Player, IndependentRelation);
            if (Race != -1)
            {
                GetLocation(ref Xhx, ref Yhx, ref Lhx);
                if (Xhx != -1)
                {
                    RawTerrain = (byte)AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx).TerrainType;
                    RaceIndex = Race;
                    TerrainType = (TerrainType)RawTerrain;
                    if (City.RaceTerrain[RaceIndex].Penalty.Contains((byte)(int)TerrainType))
                    {
                        // { Read the original shared table, not the private copy. }
                        Morale -= 10;
                    }

                    if (City.RaceTerrain[RaceIndex].Bonus.Contains((byte)(int)TerrainType))
                    {
                        Morale += 10;
                    }
                }
            }

            if (GetAbilityEnabled(53))
            {
                Morale -= 40;
            }

            if (GetAbilityEnabled(108))
            {
                Morale -= 80;
            }

            if (Morale < 0)
            {
                Morale = 0;
            }

            if (Morale > 100)
            {
                Morale = 100;
            }
        }
        else
        {
            Morale = 50;
        }

        return Morale;
    }

    // Unit.PAS:743
    public virtual bool CanActivate() => true;

    // Unit.PAS:761
    public virtual void Activate()
    {
        AIGroup Group;
        FUnitStatus = FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Activated);
        AoWHSMap.Current.FUnitControl.RegisterUnit(this);
        if (FGroupID != 0 && FPlayer >= 0)
        {
            Group = AoWHSMap.Current.FAIGroupManager.FindID(FGroupID);
            if (Group != null)
            {
                Group.AddUnit(this);
            }
            else
            {
                FGroupID = 0;
            }
        }
        else
        {
            FGroupID = 0;
        }

        RegisterEnchantments();
        Changed();
    }

    // Unit.PAS:777
    public virtual void Deactivate()
    {
        UnitUserList Group;
        Group = FGroup;
        if (Group != null)
        {
            Group.DeleteUnit(this);
        }

        AoWHSMap.Current.FUnitControl.UnRegisterUnit(this);
        FUnitStatus = FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Activated);
        UnregisterEnchantments();
    }

    // Unit.PAS:1100
    public virtual void MovedTo(MapField OldField, MapField NewField, int MaxCost)
    {
        int Cost = default;
        bool Transported = default;
        EngineObject Army = default;
        Cost = MovePointCost(NewField.TerrainType, NewField.OverlayType);
        if (Cost < 0)
        {
            Cost = MaxCost;
        }
        else
        {
            if (Cost > MaxCost)
            {
                Cost = MaxCost;
            }
        }

        if (!AoWHSMap.Current.FUnlimitedMoves || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human)
        {
            if (Cost > GetMovePoints())
            {
                SetMovePoints(0);
            }
            else
            {
                SetMovePoints((sbyte)(GetMovePoints() - Cost));
            }
        }

        if (NewField.TerrainType == (int)TerrainType.Earth)
        {
            AoWHSMap.Current.ChangeTerrain(null, NewField.Xhx, NewField.Yhx, NewField.Lhx, 0, (int)TerrainType.Dirt);
        }

        if (OldField != null)
        {
            Army = Owner;
            Transported = Army is Army && ((Army)Army).Transporter(255) != null;
            if (!Transported)
            {
                if (OldField.Lhx == 0)
                {
                    if (GetAbilityEnabled(69))
                    {
                        AoWHSMap.Current.ChangeTerrainEx(null, OldField.Xhx, OldField.Yhx, OldField.Lhx, 1, PathOfLifeTerrainChange, null);
                    }
                    else
                    {
                        if (GetAbilityEnabled(68))
                        {
                            AoWHSMap.Current.ChangeTerrainEx(null, OldField.Xhx, OldField.Yhx, OldField.Lhx, 1, PathOfDecayTerrainChange, null);
                        }
                    }
                }

                if (GetAbilityEnabled(109))
                {
                    AoWHSMap.Current.ChangeTerrainEx(null, OldField.Xhx, OldField.Yhx, OldField.Lhx, 1, PathOfFrostTerrainChange, PathOfFrostTerrainChanged);
                }
            }

            if (GetAbilityEnabled(26))
            {
                ExecuteTrailOfDarkness(OldField);
            }
        }
    }

    // Unit.PAS:1890
    public virtual bool CanDisband(ref string Reason)
    {
        bool Result = AoWHSMap.Current.FSeatedPlayer == FPlayer;
        if (Result && GetTransporter() && Owner is Army)
        {
            Result = ArmyOwner.UnitsInTransporter() == 0;
        }

        return Result;
    }

    // Unit.PAS:1796
    public virtual bool CanJoin(sbyte Player)
    {
        bool Result;
        Alignment Alignment;
        DiplomaticRelation Relation;
        int I;
        bool condition = FGroup == null;
        if (!condition)
        {
            var behavior = FGroup.GroupClass.Behavior;
            condition = !Set<GroupBehavior>.Of(GroupBehavior.Guard, GroupBehavior.GuardArea).Contains(behavior);
        }

        if (condition)
        {
            if (GetRace() == -1)
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                Alignment = AoWHSSet.Current.FRaceResources.GetRaceResource(players.FPlayerRace).GetAlignment();
                if (GetAlignment() != Units.Alignment.None)
                {
                    Relation = AlignmentTables.dAlignmentToDiplomaticRelation(GetAlignment(), Alignment);
                    if (Relation == DiplomaticRelation.Unknown && this is Unit)
                    {
                        Result = false;
                        I = AoWHSMap.Current.FRaces.Count;
                        while (!Result && I != 0)
                        {
                            I -= 1;
                            bool test = AoWHSMap.Current.FRaces.GetRace(I).GetPlayerRelation(Player) >= Status.Polite;
                            if (test)
                            {
                                var unitResourceIndex = ((Unit)this).FUnitResource.UnitResourceIndex();
                                test = AoWHSSet.Current.FRaceResources.GetRaceResource(I).FUnitIndexes.IndexOf(unitResourceIndex) != -1;
                            }

                            Result = test;
                        }
                    }
                    else
                    {
                        Result = Relation == DiplomaticRelation.Peace;
                    }
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                var race = GetRace();
                Result = AoWHSMap.Current.FRaces.GetRace(race).GetPlayerRelation(Player) >= Status.Polite;
            }

            if (Result && FGroup != null && FGroup.GroupClass.Behavior == GroupBehavior.Hunt && ((HuntAIGroup)FGroup).HuntingPlayer(Player))
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:1835
    public virtual int JoinAmount(sbyte Player)
    {
        int Result;
        UnitUserList Group;
        Group = FGroup;
        if (Group != null && ((AIGroup)(object)Group).GroupClass.Behavior == GroupBehavior.Refuge)
        {
            Result = 0;
        }
        else
        {
            Result = Round((double)GetObtainValue() * 0.75);
        }

        return Result;
    }

    // Unit.PAS:1843
    public virtual bool OfferToJoin(sbyte Player)
    {
        bool Result;
        UnitUserList Group;
        Alignment Alignment;
        if (CanJoin(Player))
        {
            if (GetRace() != -1)
            {
                var race = GetRace();
                Result = AoWHSMap.Current.FRaces.GetRace(race).GetPlayerRelation(Player) >= Status.Polite;
            }
            else
            {
                Group = FGroup;
                if (Group != null && ((AIGroup)(object)Group).GroupClass.Behavior == GroupBehavior.Refuge)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                    Alignment = AoWHSSet.Current.FRaceResources.GetRaceResource(players.FPlayerRace).GetAlignment();
                    if ((GetAlignment() != Units.Alignment.None || GetUnitType() == UnitType.Machine) && AlignmentTables.dAlignmentToDiplomaticRelation(GetAlignment(), Alignment) != DiplomaticRelation.Peace)
                    {
                        Result = false;
                    }
                    else
                    {
                        Result = true;
                    }
                }
                else
                {
                    Result = false;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:1872
    public virtual void GetJoinMessage(sbyte Player, int Count, int Amount, JoinMode Mode, StringCollection Messages)
    {
        UnitUserList Group;
        Messages.Clear();
        switch (Mode)
        {
            case JoinMode.Default:
                Group = FGroup;
                if (Group != null && ((AIGroup)(object)Group).GroupClass.Behavior == GroupBehavior.Refuge && Amount == 0)
                {
                    if (Count == 0)
                    {
                        Messages.Text = AbilityRules.TranslateRStr(GameText.P2RefugeJoinRStr);
                    }
                    else
                    {
                        Messages.Text = AbilityRules.TranslateRStr(GameText.P2RefugeesJoinRStr);
                    }
                }
                else
                {
                    if (Count == 0)
                    {
                        Messages.Text = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.P2BribeUnitJoinRStr), [IntToStr(Amount)]);
                    }
                    else
                    {
                        Messages.Text = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.P2BribeUnitsJoinRStr), [IntToStr(Amount)]);
                    }
                }

                break;
        }
    }

    // Unit.PAS:844
    public virtual void UnitKilled()
    {
    }

    // Unit.PAS:1209
    public virtual void Killed()
    {
        UnitUserList Group;
        FUnitStatus = FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        ResetLifeMasteryBlessing();
        ResetLifeMasteryFear();
        ResetEarthMasteryStoneSkin();
        ResetFireMasteryProtection();
        UnregisterEnchantments();
        Group = FGroup;
        if (Group != null)
        {
            Group.DeleteUnit(this);
        }

        SetPlayer(-1);
        Changed();
    }

    // Unit.PAS:1224
    public virtual void Resurrect()
    {
        this.ExpandAbility(128);
        FUnitStatus = FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        SetHitPoints(1);
        SetPlayer(0);
    }

    // Unit.PAS:1231
    public virtual void Animate()
    {
        this.ExpandAbility(129);
        FUnitStatus = FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        SetHitPoints(1);
        SetPlayer(0);
    }

    // Unit.PAS:152
    // { A stack-argument assembler routine gets an EBP frame; remove it before jumping. }
    public void RetrieveDamageInfo(sbyte Attack, sbyte Damage, Set<DamageType> DamageTypes, ref int DamageValue, ref int ObtainValue)
    {
        int MaxDamage;
        MaxDamage = GetHitPoints() * 100;
        DamageValue = MeleeRound.StatisticsToDV(Attack, Damage, DamageTypes, GetDefense(), GetResistance(), GetImmunityTypes(), GetProtectionTypes());
        if (MaxDamage < DamageValue)
        {
            DamageValue = MaxDamage;
        }

        if (DamageValue > 0 && GetHitPoints() > 0)
        {
            if (MaxDamage > DamageValue)
            {
                ObtainValue = Round((double)GetObtainValue() * ((double)((double)DamageValue * 0.75) / MaxDamage));
            }
            else
            {
                ObtainValue = GetObtainValue();
            }
        }
        else
        {
            ObtainValue = 0;
        }
    }

    // A hero list is not an army or a unit list. Callers treat that owner as absent.
    internal Army ArmyOwner => Owner is HeroList ? null : (Army)Owner;

    internal UnitList UnitListOwner => Owner is HeroList ? null : (UnitList)Owner;

    // Unit.PAS:189
    public void GetLocation(ref sbyte Xhx, ref sbyte Yhx, ref sbyte Lhx)
    {
        if (Owner is UnitList)
        {
            UnitListOwner.GetLocation(ref Xhx, ref Yhx, ref Lhx);
        }
        else
        {
            Xhx = -1;
            Yhx = -1;
            Lhx = -1;
        }
    }

    // Unit.PAS:231
    public int GetRelationValue(sbyte Player, sbyte IndependentRelation)
    {
        int Result;
        sbyte Race;
        Race = GetRace();
        if (Player != 0)
        {
            if (Race != -1 && !GetAbilityEnabled(107))
            {
                Result = AoWHSMap.Current.FRaces.GetRace(Race).GetPlayerRelationValue(Player);
            }
            else
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                var playerAlignment = AoWHSSet.Current.FRaceResources.GetRaceResource(players.FPlayerRace).GetAlignment();
                var unitAlignment = GetAlignment();
                Result = AlignmentTables.P2AlignmentToStatusValue[(int)unitAlignment][(int)playerAlignment];
            }
        }
        else
        {
            var iRtoAlignment = AbilityRules.IRtoAlignment(IndependentRelation);
            var alignment = GetAlignment();
            Result = AlignmentTables.P2AlignmentToStatusValue[(int)alignment][(int)iRtoAlignment];
        }

        return Result;
    }

    // Unit.PAS:256
    public Status GetRelation(sbyte Player, sbyte IndependentRelation) => Race.StatusValueToStatus(GetRelationValue(Player, IndependentRelation));

    // Unit.PAS:301
    public Status UnitRelation(Troop AUnit)
    {
        Status Result;
        sbyte Race;
        sbyte OtherRace;
        Race = GetRace();
        OtherRace = AUnit.GetRace();
        if (Race != -1 && OtherRace != -1)
        {
            Result = AoWHSMap.Current.FRaces.GetRace(Race).GetRelation(OtherRace);
        }
        else
        {
            var alignment = AUnit.GetAlignment();
            Result = AlignmentTables.dAlignmentToStatus(GetAlignment(), alignment);
        }

        return Result;
    }

    // Unit.PAS:312
    public sbyte UpdateMoraleValue()
    {
        int Player;
        int Value;
        Troop AUnit;
        Player = FPlayer;
        Value = GetUnitMoraleValue((sbyte)Player, GetIndependentRelation());
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && GetUnitType() != UnitType.Machine && !(this is Leader) && Owner is Army)
        {
            if (GetUpkeep() != 0 && Player > 0 && !GetAbilityEnabled(155))
            {
                Value += AoWHSMap.Current.FPlayers.GetPlayers(Player).FMoraleValue;
            }

            for (int I = Owner.Count - 1; I >= 0; I--)
            {
                AUnit = (Troop)((EChangeNotifyNode)(object)Owner).FChildren.List[I];
                if (AUnit.GetAbilityEnabled(37) && AUnit != this)
                {
                    Value += 30;
                }
                else
                {
                    if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(UnitRelation(AUnit)))
                    {
                        Value -= 10;
                    }
                }
            }
        }

        if (Value > 125)
        {
            FMoraleValue = 125;
        }
        else
        {
            if (Value < -25)
            {
                FMoraleValue = -25;
            }
            else
            {
                FMoraleValue = (sbyte)Value;
            }
        }

        return FMoraleValue;
    }

    // Unit.PAS:342
    public Morale GetMorale() => AbilityRules.MoraleValueToMorale(FMoraleValue);

    // Unit.PAS:358
    public void UpdateEnchanted()
    {
        int I;
        AbilityTypeList List;
        bool Enchanted;
        List = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[4];
        Enchanted = false;
        I = List.Count;
        while (!Enchanted && I != 0)
        {
            I -= 1;
            Enchanted = GetAbilityEnabled(List.GetAbility(I).FAbilityID);
        }

        if (Enchanted)
        {
            FUnitStatus = FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Enchanted);
        }
        else
        {
            FUnitStatus = FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Enchanted);
        }
    }

    // Unit.PAS:375
    public void RemoveEnchantments()
    {
        Ability Ability;
        AbilityTypeList List;
        AddRef();
        List = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[4];
        for (int I = List.Count - 1; I >= 0; I--)
        {
            Ability = List.GetAbility(I);
            if (Ability is UnitEnchantmentAbility && GetAbilityEnabled(Ability.FAbilityID))
            {
                RemoveAbility(Ability.FAbilityID);
            }
        }

        Release();
    }

    // Unit.PAS:391
    public void RemoveHandicaps()
    {
        AddRef();
        RemoveAbility(93);
        RemoveAbility(98);
        RemoveAbility(53);
        Release();
    }

    // Unit.PAS:403
    public void RegisterEnchantments()
    {
        Ability Ability;
        AbilityTypeList List;
        UnitEnchantmentAbilityData Data;
        List = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[4];
        for (int I = List.Count - 1; I >= 0; I--)
        {
            Ability = List.GetAbility(I);
            if (Ability is UnitEnchantmentAbility && GetAbilityEnabled(Ability.FAbilityID))
            {
                Data = (UnitEnchantmentAbilityData)this.GetAbilityData(Ability.FAbilityID);
                if (Data.FEnchantment.FSource > -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(Data.FEnchantment.FSource).FMagicControl.RegisterEnchantment(Data.FEnchantment);
                }
            }
        }
    }

    // Unit.PAS:424
    public void UnregisterEnchantments()
    {
        Ability Ability;
        AbilityTypeList List;
        UnitEnchantmentAbilityData Data;
        List = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[4];
        for (int I = List.Count - 1; I >= 0; I--)
        {
            Ability = List.GetAbility(I);
            if (Ability is UnitEnchantmentAbility && GetAbilityEnabled(Ability.FAbilityID))
            {
                Data = (UnitEnchantmentAbilityData)this.GetAbilityData(Ability.FAbilityID);
                if (Data.FEnchantment.FSource > -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(Data.FEnchantment.FSource).FMagicControl.UnregisterEnchantment(Data.FEnchantment);
                }
            }
        }
    }

    // Unit.PAS:449
    public UnitEnchantment AbilityToEnchantment(int AbilityID)
    {
        UnitEnchantment Result;
        Ability Ability;
        UnitEnchantmentAbilityData Data;
        Ability = AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID);
        if (Ability is UnitEnchantmentAbility && GetAbilityEnabled(Ability.FAbilityID))
        {
            Data = (UnitEnchantmentAbilityData)this.GetAbilityData(Ability.FAbilityID);
            if (Data != null)
            {
                Result = Data.FEnchantment;
            }
            else
            {
                return null;
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Unit.PAS:463
    public bool CanDispelEnchantment(sbyte Player)
    {
        int I;
        AbilityTypeList List;
        UnitEnchantment Enchantment;
        List = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[4];
        I = List.Count;
        bool Result = false;
        while (!Result && I != 0)
        {
            I -= 1;
            Enchantment = AbilityToEnchantment(List.GetAbility(I).FAbilityID);
            if (Enchantment == null || Player == Enchantment.FSource)
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

    // Unit.PAS:481
    public void ListEnchantments(ECustomNode List)
    {
        AbilityTypeList Abilities;
        UnitEnchantment Enchantment;
        Abilities = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[4];
        for (int I = Abilities.Count - 1; I >= 0; I--)
        {
            Enchantment = AbilityToEnchantment(Abilities.GetAbility(I).FAbilityID);
            if (Enchantment != null)
            {
                List.AddChild(Enchantment);
            }
        }
    }

    // Unit.PAS:620
    public void UpdateDefensiveStrength()
    {
        FDefensiveStrength = (GetDefense() + 10 + GetResistance()) * (GetHitPoints() + 10);
        if (FDefensiveStrength < 0)
        {
            FDefensiveStrength = 1;
        }
    }

    // Unit.PAS:657
    public int GetStrength(DamageType DamageType)
    {
        if (!FUnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateDefensiveStrength();
            UpdateOffensiveStrength();
        }

        int Result = FOffensiveStrength[(int)DamageType];
        if (Result != 0)
        {
            Result = (Result + 10) * FDefensiveStrength;
        }

        return Result;
    }

    // Unit.PAS:664
    public int GetStrengthMax(Set<DamageType> DamageTypes)
    {
        int Result;
        DamageType DamageType;
        if (!FUnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateDefensiveStrength();
            UpdateOffensiveStrength();
        }

        if (DamageTypes != TroopConstants.TAbstractUnit_GetStrengthMax__AllDamageTypes)
        {
            Result = 0;
            for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
            {
                DamageType = (DamageType)damageTypeOrdinal;
                if (DamageTypes.Contains(DamageType))
                {
                    if (Result < FOffensiveStrength[(int)DamageType])
                    {
                        Result = FOffensiveStrength[(int)DamageType];
                    }
                }
            }

            if (Result != 0)
            {
                Result = (Result + 10) * FDefensiveStrength;
            }
        }
        else
        {
            if (FMaxOffensiveStrength != 0)
            {
                Result = (FMaxOffensiveStrength + 10) * FDefensiveStrength;
            }
            else
            {
                Result = 0;
            }
        }

        return Result;
    }

    // Unit.PAS:688
    public int GetCombatValue(Troop Target, byte Mode)
    {
        int Value;
        AbilityTypeList Abilities;
        Ability Ability;
        AbilityDamageValues Info = default;
        int Result = 0;
        Abilities = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[(int)AbilityType.Combat];
        for (int I = Abilities.Count - 1; I >= 0; I--)
        {
            Ability = Abilities.GetAbility(I);
            if (GetAbilityEnabled(Ability.FAbilityID))
            {
                Ability.GetDamageValueEx(this, Target, (WallSituation)Mode, ref Info);
                if (Info.SourceDamage > 0)
                {
                    Value = Info.SourceDamage * Info.SourceDamage / (Info.TargetDamage + Info.SourceDamage);
                    if (Value <= 0)
                    {
                        Value = 1;
                    }

                    if (Value > Result)
                    {
                        Result = Value;
                    }
                }
            }
        }

        return Result;
    }

    // Unit.PAS:797
    public int GetManaUpkeep()
    {
        int Result;
        Enchantment Enchantment;
        if (GetAbilityEnabled(155))
        {
            Enchantment = ((UnitEnchantmentAbilityData)this.GetAbilityData(155)).FEnchantment;
            if (Enchantment != null)
            {
                Result = Enchantment.FUpkeep;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Unit.PAS:831
    public bool GetTransporting()
    {
        bool Result;
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        MapField MapField;
        GetLocation(ref Xhx, ref Yhx, ref Lhx);
        if (Xhx != -1)
        {
            MapField = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx);
            Result = !ValidTerrain(MapField.TerrainType, MapField.OverlayType);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:922
    public void CreateMovePointTable(ref SByteArray16By16 Table)
    {
        Set<MoveType> Moves;
        int Points;
        sbyte Value;
        bool Faster;
        bool Slower;
        Moves = GetMoveTypes();
        if (AoWHSMap.Current.FCombat != null)
        {
            Moves = Moves - Set<MoveType>.Of(MoveType.Tunneling);
        }

        var moveTypesToInt = MovementTables.MoveTypesToInt(Moves);
        Table = MovementTables.MovePointTables[moveTypesToInt];
        if (FPlayer > 0)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.EnchantedRoads))
            {
                for (int Terrain = 0; Terrain <= 15; Terrain++)
                {
                    if (Table[Terrain][4 - -1] != -1)
                    {
                        Table[Terrain][4 - -1] = 2;
                    }
                }
            }

            if (GetAbilityEnabled(23) || GetAbilityEnabled(1) || GetAbilityEnabled(165))
            {
                Faster = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery);
                Slower = (((byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 4) != 0;
                if (Faster && !Slower)
                {
                    for (int Overlay = -1; Overlay <= 11; Overlay++)
                    {
                        for (int Terrain = 0; Terrain <= 15; Terrain++)
                        {
                            Value = Table[Terrain][Overlay - -1];
                            if (Value > 2)
                            {
                                Table[Terrain][Overlay - -1] = (sbyte)(int)(((uint)(Value + 1)) >> 1);
                            }
                        }
                    }
                }
                else
                {
                    if (!Faster && Slower)
                    {
                        for (int Overlay = -1; Overlay <= 11; Overlay++)
                        {
                            for (int Terrain = 0; Terrain <= 15; Terrain++)
                            {
                                if (Table[Terrain][Overlay - -1] >= 1)
                                {
                                    Table[Terrain][Overlay - -1] = (sbyte)(Table[Terrain][Overlay - -1] * 2);
                                }
                            }
                        }
                    }
                }
            }

            if (GetAbilityEnabled(152))
            {
                for (int Overlay = -1; Overlay <= 11; Overlay++)
                {
                    for (int Terrain = 0; Terrain <= 15; Terrain++)
                    {
                        Points = Table[Terrain][Overlay - -1];
                        if (Points > 2)
                        {
                            Points -= 1;
                            // { Original retains this redundant lower bound after Points > 2. }
                            if (Points < 2)
                            {
                                Points = 2;
                            }
                        }

                        Table[Terrain][Overlay - -1] = (sbyte)Points;
                    }
                }
            }

            if (GetAbilityEnabled(132))
            {
                for (int Overlay = -1; Overlay <= 11; Overlay++)
                {
                    for (int Terrain = 0; Terrain <= 15; Terrain++)
                    {
                        Points = Table[Terrain][Overlay - -1];
                        if (Points > 0)
                        {
                            Table[Terrain][Overlay - -1] = (sbyte)(Points + 2);
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:993
    public bool ValidTerrain(sbyte TerrainType, sbyte OverlayType)
    {
        bool Result;
        sbyte Cost;
        // { Original retrieves the movement set twice and retains the first dead store. }
        GetMoveTypes();
        var moveTypesToInt = MovementTables.MoveTypesToInt(GetMoveTypes());
        Cost = MovementTables.MovePointTables[moveTypesToInt][TerrainType][OverlayType - -1];
        if (Cost == -1 || Cost > GetMoves())
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Unit.PAS:1011
    public void ExecuteTrailOfDarkness(MapField MapField)
    {
        MapLevel Level;
        HexNumberWalkData Walk = default;
        AoWMapField Field;
        Level = (MapLevel)MapField.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, MapField.Xhx, MapField.Yhx, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.X < Level.Width && Walk.Y >= 0 && Walk.Y < Level.Height)
            {
                Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                if (Field.TerrainType != (int)TerrainType.Border)
                {
                    for (int Player = 1, last = AoWHSMap.Current.FPlayers.Count - 1; Player <= last; Player++)
                    {
                        if (Player != FPlayer && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.War && Field.GetExplored(Player) && !AoWHSMap.Current.VisibleForPlayer(Walk.X, Walk.Y, MapField.Lhx, (sbyte)Player))
                        {
                            Field.SetExplored(Player, false);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        AoWHSMap.Current.MapChangedRad(MapField.Xhx, MapField.Yhx, MapField.Lhx, 1);
    }

    // Unit.PAS:1038
    public void PathOfLifeTerrainChange(MapField MapField, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        if (TerrainType != (int)World.Map.TerrainType.Ice && TerrainType != (int)World.Map.TerrainType.Water && MapField.Lhx == 0)
        {
            TerrainType = (int)World.Map.TerrainType.Grass;
        }
    }

    // Unit.PAS:1043
    public void PathOfDecayTerrainChange(MapField MapField, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        if (TerrainType != (int)World.Map.TerrainType.Ice && TerrainType != (int)World.Map.TerrainType.Water && MapField.Lhx == 0)
        {
            TerrainType = (int)World.Map.TerrainType.Wasteland;
        }
    }

    // Unit.PAS:1048
    public void PathOfFrostTerrainChange(MapField MapField, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        FrozenWaterHexSprite Ice;
        if (TerrainType == (int)World.Map.TerrainType.Water || TerrainType == (int)World.Map.TerrainType.UndergroundWater)
        {
            if (TerrainType == (int)World.Map.TerrainType.Water)
            {
                TerrainType = (int)World.Map.TerrainType.Ice;
            }
            else
            {
                TerrainType = (int)World.Map.TerrainType.UndergroundIce;
            }
        }
        else
        {
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)TerrainType))
            {
                Ice = (FrozenWaterHexSprite)MapField.FindOwnedHS(typeof(FrozenWaterHexSprite));
                if (Ice != null)
                {
                    Ice.FDaysRemaining = 2;
                    Ice.FCasterPlayer = FPlayer;
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(1).With(2).With(4).With(5).Contains((byte)TerrainType) && MapField.Lhx == 0)
                {
                    TerrainType = (int)World.Map.TerrainType.Snow;
                }
            }
        }
    }

    // Unit.PAS:1066
    public void PathOfFrostTerrainChanged(MapField MapField, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
    {
        FrozenWaterHexSprite Ice;
        if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)MapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)OriginalTerrainType))
        {
            Ice = (FrozenWaterHexSprite)MapField.FindOwnedHS(typeof(FrozenWaterHexSprite));
            if (Ice == null)
            {
                Ice = new FrozenWaterHexSprite(null);
                AoWHSMap.Current.PlaceHX(Ice, MapField.Xhx, MapField.Yhx, MapField.Lhx);
            }

            Ice.FDaysRemaining = 2;
            Ice.FCasterPlayer = FPlayer;
        }
    }

    // Unit.PAS:1082
    public void UpdatePathOfFrost()
    {
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        MapField Field;
        FrozenWaterHexSprite Ice;
        GetLocation(ref Xhx, ref Yhx, ref Lhx);
        if (Xhx > -1)
        {
            Field = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx);
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)Field.TerrainType))
            {
                Ice = (FrozenWaterHexSprite)Field.FindOwnedHS(typeof(FrozenWaterHexSprite));
                if (Ice != null)
                {
                    Ice.FDaysRemaining = 2;
                    Ice.FCasterPlayer = FPlayer;
                }
            }
        }
    }

    // Unit.PAS:1134
    public bool ConcealedOnMapF(AoWMapField Field)
    {
        bool Result;
        if (!GetAbilityEnabled(54) && !Field.GetConcealmentActive())
        {
            Result = GetAbilityEnabled(139) && Field.Lhx != 0;
            if (Field.OverlayType != 1 || !(GetAbilityEnabled(145) || GetAbilityEnabled(162)))
            {
                // { Original terrain branches overwrite the underground concealment result. }
                switch ((short)Field.TerrainType)
                {
                    case 1:
                        Result = GetAbilityEnabled(140);
                        break;
                    case 3:
                        Result = GetAbilityEnabled(141);
                        break;
                    case 2:
                        Result = GetAbilityEnabled(142);
                        break;
                    case 5:
                        Result = GetAbilityEnabled(143);
                        break;
                    case 0:
                    case 10:
                        Result = GetAbilityEnabled(144);
                        break;
                    case 4:
                        Result = GetAbilityEnabled(138);
                        break;
                }
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Unit.PAS:1154
    public int ExecuteStormDamage(StormType Storm, int Distance)
    {
        int Result;
        sbyte Terrain;
        sbyte Overlay;
        Set<MoveType> Moves;
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        MapField Field;
        bool DoubleDamage;
        int Attack;
        Moves = GetMoveTypes();
        DoubleDamage = GetAbilityEnabled(23);
        if (!DoubleDamage && Moves.Contains(MoveType.Flying))
        {
            GetLocation(ref Xhx, ref Yhx, ref Lhx);
            if (Xhx != -1)
            {
                Field = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx);
                Terrain = Field.TerrainType;
                Overlay = Field.OverlayType;
                Moves = Moves - Set<MoveType>.Of(MoveType.Flying);
                var moveTypesToInt = MovementTables.MoveTypesToInt(Moves);
                DoubleDamage = MovementTables.MovePointTables[moveTypesToInt][Terrain][Overlay - -1] == 0;
            }
        }

        switch (Storm)
        {
            case StormType.Ice:
                Attack = 7;
                Result = 5;
                break;
            case StormType.Lightning:
                Attack = 8;
                Result = 7;
                break;
            case StormType.Death:
                Attack = 7;
                Result = 4;
                break;
            case StormType.Holy:
                Attack = 7;
                Result = 5;
                break;
            case StormType.Fire:
                Attack = 7;
                Result = 5;
                break;
            case StormType.Pestilence:
                Attack = 4;
                Result = 3;
                break;
            default:
                Attack = 0;
                Result = 0;
                break;
        }

        if (DoubleDamage)
        {
            Result = Result * 2;
        }

        Result = ExecuteDamageRole(Attack, Result, Set<DamageType>.Of(Army.StormDamageType[(int)Storm]));
        if (Result > GetHitPoints())
        {
            Result = GetHitPoints();
        }

        if (Result > 0)
        {
            switch (Storm)
            {
                case StormType.Death:
                    ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Death));
                    break;
                case StormType.Pestilence:
                    ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Poison));
                    break;
            }
        }

        SetHitPoints((sbyte)(GetHitPoints() - Result));
        return Result;
    }

    // Unit.PAS:1198
    // { Result is the final damage. }
    public int MovePointCost(sbyte TerrainType, sbyte OverlayType)
    {
        SByteArray16By16 Table = default;
        CreateMovePointTable(ref Table);
        return Table[TerrainType][OverlayType - -1];
    }

    // Unit.PAS:1238
    public void ResetEarthMasteryStoneSkin()
    {
        if (GetAbilityEnabled(153))
        {
            if (((UnitEnchantmentAbilityData)this.GetAbilityData(153)).FEnchantment.FSource == -2)
            {
                RemoveAbility(153);
            }
        }
    }

    // Unit.PAS:1246
    public void SetEarthMasteryStoneSkin()
    {
        if (GetUnitType() != UnitType.Machine && !GetAbilityEnabled(153))
        {
            if (this.ExpandAbility(153))
            {
                ((UnitEnchantmentAbilityData)this.GetAbilityData(153)).FEnchantment.SetUpkeep(0);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(153)).FEnchantment.SetSource(-2);
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(17);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(153)).FEnchantment.FDispelResistance = (sbyte)((UnitSpell)(object)spell).FUnitDispelResistance;
            }
        }
    }

    // Unit.PAS:1259
    public void ResetFireMasteryProtection()
    {
        if (GetAbilityEnabled(167))
        {
            if (((UnitEnchantmentAbilityData)this.GetAbilityData(167)).FEnchantment.FSource == -2)
            {
                RemoveAbility(167);
            }
        }
    }

    // Unit.PAS:1265
    public void SetFireMasteryProtection()
    {
        if (!GetAbilityEnabled(71) && !GetAbilityEnabled(167))
        {
            if (this.ExpandAbility(167))
            {
                ((UnitEnchantmentAbilityData)this.GetAbilityData(167)).FEnchantment.SetUpkeep(0);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(167)).FEnchantment.SetSource(-2);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(167)).FEnchantment.FDispelResistance = 7;
            }
        }
    }

    // Unit.PAS:1278
    public void ResetLifeMasteryBlessing()
    {
        if (GetAbilityEnabled(158))
        {
            if (((UnitEnchantmentAbilityData)this.GetAbilityData(158)).FEnchantment.FSource == -2)
            {
                RemoveAbility(158);
            }
        }
    }

    // Unit.PAS:1284
    public void SetLifeMasteryBlessing()
    {
        if (!GetAbilityEnabled(158))
        {
            if (this.ExpandAbility(158))
            {
                ((UnitEnchantmentAbilityData)this.GetAbilityData(158)).FEnchantment.SetUpkeep(0);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(158)).FEnchantment.SetSource(-2);
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(56);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(158)).FEnchantment.FDispelResistance = (sbyte)((UnitSpell)(object)spell).FUnitDispelResistance;
            }
        }
    }

    // Unit.PAS:1305
    public void ExecuteLifeMasteryFearRole()
    {
        DurationAbilityData Data;
        bool condition = !GetAbilityEnabled(53);
        if (condition)
        {
            var alignment = GetAlignment();
            condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
        }

        if (condition && !GetAbilityEnabled(67) && !GetAbilityEnabled(46) && HitChance.HitRole(4 - GetResistance()))
        {
            if (this.ExpandAbility(53))
            {
                Data = (DurationAbilityData)this.GetAbilityData(53);
                Data.FSource = 1;
                Data.FDuration = 0;
            }
        }
    }

    // Unit.PAS:1319
    public void ValidateLifeMastery()
    {
        sbyte Player;
        Player = FPlayer;
        if (Player > 0)
        {
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                ResetLifeMasteryBlessing();
            }

            if ((((byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 16) == 0)
            {
                ResetLifeMasteryFear();
            }
        }
    }

    // Unit.PAS:1332
    public void ExecuteDeathMasteryCurseRole()
    {
        DurationAbilityData Data;
        bool condition = !GetAbilityEnabled(93);
        if (condition)
        {
            var alignment = GetAlignment();
            condition = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
        }

        if (condition && HitChance.HitRole(6 - GetResistance()))
        {
            if (this.ExpandAbility(93))
            {
                Data = (DurationAbilityData)this.GetAbilityData(93);
                Data.FSource = 1;
            }
        }
    }

    // Unit.PAS:1344
    public void ValidateDeathMastery()
    {
        sbyte Player;
        Player = FPlayer;
        if (Player > 0)
        {
            if ((((byte)AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FEnemyEnchantments.Image) & 32) == 0)
            {
                if (GetAbilityEnabled(93))
                {
                    if (((DurationAbilityData)this.GetAbilityData(93)).FSource == 1)
                    {
                        RemoveAbility(93);
                    }
                }
            }
        }
    }

    // Unit.PAS:1391
    public int VisibilityRange()
    {
        int Result;
        int Level;
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        Level = GetAbilityLevel(64);
        if (Level != 0)
        {
            Result = Level * 2 + 4;
        }
        else
        {
            Result = 4;
        }

        GetLocation(ref Xhx, ref Yhx, ref Lhx);
        if (Lhx != 0 && !GetAbilityEnabled(39) || Lhx == 0 && FPlayer > 0 && (((byte)AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
        {
            Result = (int)(((uint)(Result + 1)) >> 1);
        }

        return Result;
    }

    // Unit.PAS:1404
    public int TrueVisionRange()
    {
        if (GetAbilityEnabled(41))
        {
            return VisibilityRange();
        }

        return 1;
    }

    // Unit.PAS:1428
    public bool CanPlace(int X, int Y, int L, sbyte Player, Set<UnitPlaceModeElement> Mode)
    {
        bool Result;
        AoWMapField Field;
        ArmyHexSprite ArmyHS;
        sbyte FieldPlayer;
        if (X >= 0 && Y >= 0 && X < AoWHSMap.Current.MapContainer.Width && Y < AoWHSMap.Current.MapContainer.Height)
        {
            Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L);
            FieldPlayer = Field.GetPlayer();
            if (Mode.Contains(UnitPlaceModeElement.NeglectEmptyPlayerLocation) || FieldPlayer == Player || FieldPlayer == -1 || Player == -1)
            {
                ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    if (Mode.Contains(UnitPlaceModeElement.CreateNewArmy) || (Player != -1 && ArmyHS.FArmy.FPlayer != Player) || !ArmyHS.FArmy.CanAddUnit(this))
                    {
                        Result = false;
                    }
                    else
                    {
                        Result = true;
                    }
                }
                else
                {
                    Result = ValidTerrain(Field.TerrainType, Field.OverlayType);
                }
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:1454
    public ArmyHexSprite Place(int X, int Y, int L, sbyte Player, Set<UnitPlaceModeElement> Mode)
    {
        HexNumberWalkData Walk = default;
        ArmyHexSprite ArmyHS;
        ArmyHexSprite Result = null;
        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Result == null && (Walk.Radius == 0 || Mode.Contains(UnitPlaceModeElement.ScanArea)) && Walk.Radius < 30)
        {
            if (CanPlace(Walk.X, Walk.Y, L, Player, Mode))
            {
                ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(Walk.X, Walk.Y, L).FindOwnedChild(131607);
                if (ArmyHS == null)
                {
                    ArmyHS = new ArmyHexSprite(null);
                    if (Player != -1)
                    {
                        ArmyHS.FArmy.SetPlayer(Player);
                    }

                    if (!ArmyHS.FArmy.AddUnit(this) || !AoWHSMap.Current.PlaceHX(ArmyHS, Walk.X, Walk.Y, L))
                    {
                        ArmyHS?.Dispose();
                        ArmyHS = null;
                    }

                    Result = ArmyHS;
                }
                else
                {
                    if (ArmyHS.FArmy.AddUnit(this))
                    {
                        Result = ArmyHS;
                    }
                    else
                    {
                        Result = null;
                    }
                }
            }

            if (Result == null)
            {
                HexNumberWalkData.WalkToNextHN(ref Walk);
            }
        }

        return Result;
    }

    // Unit.PAS:1486
    public void ValidateCrusader()
    {
        DurationAbilityData Data;
        Data = (DurationAbilityData)GetAbilityData(101);
        if (Data != null)
        {
            if (Data.FDuration <= 0)
            {
                Killed();
            }
        }
    }

    // Unit.PAS:1504
    public void Show(int X, int Y, in LockedSurface SD)
    {
        UnitShowInfo Info = default;
        Info.Direction = 3;
        Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
        Info.Animation = UnitAnimation.StandStill;
        Info.Frame = 0;
        Info.Looped = false;
        ShowEx(X, Y, SD, Info);
    }

    // Unit.PAS:1610
    public void ShowFace(in Rect R, in LockedSurface SD)
    {
        LibraryImage Image;
        Image = GetFace();
        if (Image != null)
        {
            Image.ShowStretched(R, SD);
        }

        if (GetAbilityEnabled(129))
        {
            if (AoWHSMap.Current != null)
            {
                if (AoWHSSet.Current.FGeneralImages[45] != null)
                {
                    AoWHSSet.Current.FGeneralImages[45].ShowStretched(R, SD);
                }
            }
        }
    }

    // Unit.PAS:1623
    public int ExecuteDamageRole(int Attack, int Damage, Set<DamageType> DamageTypes)
    {
        Set<DamageType> Active;
        int DC;
        int Result = 0;
        Active = DamageTypes - GetImmunityTypes();
        if (Active != Set<DamageType>.Of())
        {
            DC = Attack - GetDefense();
            Result = AbilityRules.ExecuteDamageRole(Damage, DC);
            if (Active - GetProtectionTypes() == Set<DamageType>.Of())
            {
                Result = (Result + 1) / 2;
            }
        }

        return Result;
    }

    // Unit.PAS:1635
    public bool ExecuteResistanceRole(int Attack, DamageType DamageType)
    {
        bool Result;
        if (!GetImmunityTypes().Contains(DamageType))
        {
            if (GetProtectionTypes().Contains(DamageType))
            {
                Attack -= 2;
            }

            Result = HitChance.HitRole(Attack - GetResistance());
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:1644
    public Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> DamageTypes)
    {
        Set<DamageType> Active;
        Set<DamageType> Protection;
        int Resistance;
        int EffectiveResistance;
        Set<DamageType> Result = Set<DamageType>.Of();
        Active = DamageTypes - GetImmunityTypes();
        if (Active != Set<DamageType>.Of())
        {
            Resistance = GetResistance();
            Protection = GetProtectionTypes();
            if (Active.Contains(DamageType.Fire) && !GetAbilityEnabled(7))
            {
                if (Protection.Contains(DamageType.Fire))
                {
                    EffectiveResistance = Resistance + 2;
                }
                else
                {
                    EffectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(4 - EffectiveResistance))
                {
                    Result = Result + Set<DamageType>.Of(DamageType.Fire);
                }
            }

            if (Active.Contains(DamageType.Lightning) && GetUnitType() != UnitType.Machine && !GetAbilityEnabled(9))
            {
                if (Protection.Contains(DamageType.Lightning))
                {
                    EffectiveResistance = Resistance + 2;
                }
                else
                {
                    EffectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(6 - EffectiveResistance))
                {
                    Result = Result + Set<DamageType>.Of(DamageType.Lightning);
                }
            }

            if (Active.Contains(DamageType.Poison) && !GetAbilityEnabled(10))
            {
                if (Protection.Contains(DamageType.Poison))
                {
                    EffectiveResistance = Resistance + 2;
                }
                else
                {
                    EffectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(3 - EffectiveResistance))
                {
                    Result = Result + Set<DamageType>.Of(DamageType.Poison);
                }
            }

            if (Active.Contains(DamageType.Death) && !GetAbilityEnabled(11))
            {
                if (Protection.Contains(DamageType.Death))
                {
                    EffectiveResistance = Resistance + 2;
                }
                else
                {
                    EffectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(4 - EffectiveResistance))
                {
                    Result = Result + Set<DamageType>.Of(DamageType.Death);
                }
            }

            if (Active.Contains(DamageType.Holy) && !GetAbilityEnabled(12))
            {
                if (Protection.Contains(DamageType.Holy))
                {
                    EffectiveResistance = Resistance + 2;
                }
                else
                {
                    EffectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(3 - EffectiveResistance) && GetUnitType() != UnitType.Machine)
                {
                    Result = Result + Set<DamageType>.Of(DamageType.Holy);
                }
            }

            if (Active.Contains(DamageType.Cold) && !GetAbilityEnabled(8))
            {
                if (Protection.Contains(DamageType.Cold))
                {
                    EffectiveResistance = Resistance + 2;
                }
                else
                {
                    EffectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(2 - EffectiveResistance))
                {
                    Result = Result + Set<DamageType>.Of(DamageType.Cold);
                }
            }
        }

        return Result;
    }

    // Unit.PAS:1699
    public Set<DamageType> ExecuteDamageEffects(Set<DamageType> DamageTypes)
    {
        Set<DamageType> Active;
        Set<DamageType> Result = Set<DamageType>.Of();
        Active = DamageTypes - GetImmunityTypes();
        // { Preserve EBX allocation and byte membership tests. }
        if (Active != Set<DamageType>.Of())
        {
            if (Active.Contains(DamageType.Poison) && this.ExpandAbility(96))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Poison);
            }

            if (Active.Contains(DamageType.Death) && this.ExpandAbility(93))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Death);
            }

            if (Active.Contains(DamageType.Holy) && this.ExpandAbility(98))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Holy);
            }
        }

        return Result;
    }

    // Unit.PAS:1747
    public DamageType OptimalDamageType(Set<DamageType> DamageTypes)
    {
        Set<DamageType> Active;
        Set<DamageType> Protection;
        DamageType Result = DamageType.None;
        Active = DamageTypes - GetImmunityTypes();
        if (Active != Set<DamageType>.Of())
        {
            Protection = GetProtectionTypes();
            if (Protection == Set<DamageType>.Of() || Active - Protection == Set<DamageType>.Of())
            {
                while (!Active.Contains(Result))
                {
                    Result = (DamageType)((int)Result - 1);
                }
            }
            else
            {
                while (!Active.Contains(Result) || Protection.Contains(Result))
                {
                    Result = (DamageType)((int)Result - 1);
                }
            }
        }

        return Result;
    }

    // Unit.PAS:1899
    public bool Disband()
    {
        bool Result;
        string Reason = "";
        DisbandUnitTokenEvent Event;
        if (CanDisband(ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            Event = new DisbandUnitTokenEvent(null);
            Event.FUnitID = FUnitID;
            Event.FPlayer = FPlayer;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // The producer ExecuteLifeMasteryFearRole writes the source marker on duration data.
    public void ResetLifeMasteryFear()
    {
        if (GetAbilityEnabled(53) && ((DurationAbilityData)GetAbilityData(53)).FSource == 1)
            RemoveAbility(53);
    }

    public virtual bool GetAbilitySet(int index) => (uint)index < (uint)FAbilityCount && (FAbilityBits[index >> 3] & (1 << (index & 7))) != 0;

    public override bool RemoveAbility(int ability)
    {
        AddRef();
        bool result = base.RemoveAbility(ability);
        Release();
        return result;
    }

    // Unit.PAS:713. TP2AbilityCalls is a layout view, not an instance type.
    public void ListAbilityInfo(StringCollection names, StringCollection types, StringCollection sources)
    {
        names.Clear();
        sources?.Clear();
        for (int i = 0; i < GetAbilityCount(); i++)
        {
            if (!GetAbilitySet(i)) continue;
            var owner = GetAbilityOwner(i);
            var ability = AoWHSSet.Current.FAbilityControl.GetAbility(i);
            var name = ability.GetName(owner);
            if (!IsEmpty(name)) names.AddObject(name, IntToObject(i));
            types?.AddObject(AbilityRules.TranslateRStr(GameTables.TAbilityTypeRStr[(int)ability.GetAbilityType()]), IntToObject(i));
            var source = ability.GetSourceName(owner);
            sources?.AddObject(source, IntToObject(i));
        }
    }

    // Unit.PAS:1715. Native hard casts retain identity across the intermediate local type.
    public Set<DamageType> ExecuteCombatDamageEffects(Battle combat, Set<DamageType> damageTypes)
    {
        Set<DamageType> result = default;
        var active = damageTypes - GetImmunityTypes();
        if (active == default(Set<DamageType>)) return result;
        if (active.Contains(DamageType.Fire) && ExpandAbility(127))
        {
            RemoveAbility(95);
            result += Set<DamageType>.Of(DamageType.Fire);
        }
        if (active.Contains(DamageType.Cold) && ExpandAbility(95))
        {
            var data = (UnitEnchantmentAbilityData)GetAbilityData(95);
            if (data != null) data.FEnchantmentPlayerIndex = (sbyte)combat.FCombatData.FCurrentPlayer;
            RemoveAbility(127);
            result += Set<DamageType>.Of(DamageType.Cold);
        }
        if (active.Contains(DamageType.Lightning) && ExpandAbility(92))
        {
            var data = (DurationAbilityData)GetAbilityData(92);
            if (data != null) data.FPlayerIndex = combat.FCombatData.FCurrentPlayer;
            result += Set<DamageType>.Of(DamageType.Lightning);
        }
        if (active.Contains(DamageType.Poison) && ExpandAbility(96))
            result += Set<DamageType>.Of(DamageType.Poison);
        if (active.Contains(DamageType.Death) && ExpandAbility(93))
            result += Set<DamageType>.Of(DamageType.Death);
        if (active.Contains(DamageType.Holy) && ExpandAbility(98))
            result += Set<DamageType>.Of(DamageType.Holy);
        return result;
    }

    public void UpdateOffensiveStrength()
    {
        var abilities = AoWHSSet.Current.FAbilityControl.FAbilitiesByType[(int)AbilityType.Combat];
        FMaxOffensiveStrength = 0;
        FOffensiveStrength = default;
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            var ability = abilities.GetAbility(i);
            if (!GetAbilityEnabled(ability.FAbilityID)) continue;
            int strength = 0;
            Set<DamageType> types = default;
            ability.GetOffensiveStrength(this, ref strength, ref types);
            if (strength > 255) strength = 255;
            if (types == default)
                Assert(false, $"Ability: {ability.GetName(null)}", @"D:\AoWDev\AoWE\inc\Unit.PAS", 0xE2B4);
            if (types == default) continue;
            if (FMaxOffensiveStrength < strength) FMaxOffensiveStrength = (byte)strength;
            for (int kind = (int)DamageType.Fire; kind <= (int)DamageType.None; kind++)
                if (types.Contains((DamageType)kind) && FOffensiveStrength[kind] < strength)
                    FOffensiveStrength[kind] = (byte)strength;
        }
    }

    internal static int P4UnitStrikeDV(Troop Source, Troop Target)
    {
        ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).FMeleeRound.CalculateFreeUnitSwing(Source, Target);
        return ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).FMeleeRound.GetAttackerDV();
    }

    public enum Morale : byte
    {
        Terrible = 0, Poor = 1, OK = 2, Good = 3, High = 4
    }

}
