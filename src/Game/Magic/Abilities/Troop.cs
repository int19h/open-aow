// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

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
public partial class Troop : AbilityOwner, IOwned<TroopOwner>, IOwnedSink<TroopOwner>
{
    // A troop stands in a unit list, or a hero stands in a hero list while no army holds it.
    // TroopOwner reads that owner without a cast.
    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is UnitList or HeroList;

    TroopOwner IOwned<TroopOwner>.Owner => TroopOwner.Of(Owner);

    void IOwnedSink<TroopOwner>.SetOwner(TroopOwner owner) => SetOwner(owner.Node);

    // SetOwner observes this count before the base body, as in Unit.PAS:95.
    public int RefCount { get; private set; } = 1;

    public int UnitId;
    public int GroupId;

    public AIGroup Group { get; set; }

    // group
    public sbyte Player;

    public Set<UnitStatusElement> UnitStatus { get; set; }

    public sbyte MoraleValue;
    private readonly byte[] _offensiveStrength = new byte[10];

    public byte MaxOffensiveStrength { get; private set; }

    private int _defensiveStrength;

    public byte LegacyGroupBehavior { get; private set; }

    // Unit.PAS:108
    public override void Dispose()
    {
        if (UnitStatus.Contains(UnitStatusElement.Activated))
        {
            Deactivate();
        }

        if (StrategicMap.Current != null)
        {
            StrategicMap.Current.NotifyUnitDestroyed(this);
        }

        base.Dispose();
    }

    // Unit.PAS:518
    public override void SetOwner(ECustomNode owner)
    {
        if (owner != Owner)
        {
            AddRef();
            if (Owner != null)
            {
                Owner.RemoveChild(this);
            }

            SetOwnerWithoutNotify(owner);
            if (Owner != null)
            {
                if (Owner.AddChild(this) == -1)
                {
                    SetOwnerWithoutNotify(null);
                }
            }

            Release();
        }
    }

    // Unit.PAS:1762
    public override void ReadWrite(StorageStream stream)
    {
        EngineObject legacy;
        base.ReadWrite(stream);
        stream.Int32(ref GroupId, 4);
        stream.Int32(ref UnitId, -1, 5);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref MoraleValue), 6);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 47);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            try
            {
                legacy = null;
                stream.Created(ref legacy, null, 48);
                if (legacy != null)
                {
                    LegacyGroupBehavior = (byte)((GroupControl)(object)legacy).Behavior();
                }
                else
                {
                    LegacyGroupBehavior = 0;
                }
            }
            catch
            {
            }
        }
        else
        {
            // { Original retains the obsolete object loaded for behavior conversion. }
            LegacyGroupBehavior = 0;
        }
    }

    // Unit.PAS:95
    public Troop(ECustomNode owner) : base(owner)
    {
        GroupId = 0;
        UnitId = -1;
    }

    // Unit.PAS:127
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // Unit.PAS:132
    public override int Release()
    {
        int count;
        RefCount -= 1;
        count = RefCount;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // Unit.PAS:1494
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
            return;
        }
        else
        {
            if (message.MessageId == 537001987)
            {
                NewTurnDone((sbyte)message.Payload.Int);
            }
            else
            {
                if (message.MessageId == 537001985)
                {
                    NewDay();
                }
                else
                {
                    if (message.MessageId == 537002240)
                    {
                        ValidateLifeMastery();
                    }
                    else
                    {
                        if (message.MessageId == 537002241)
                        {
                            ValidateDeathMastery();
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:590
    public override string GetOwnerName() => AbilityRules.TranslateResourceString(GetEngine(), GameText.InherentText);

    // Unit.PAS:171
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => TroopConstants.DefaultAbilitySelectionTypes;

    // Unit.PAS:1411
    public override void Changed()
    {
        if (UnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateMoraleValue();
            UpdateOffensiveStrength();
            UpdateDefensiveStrength();
            UpdateEnchanted();
            if (StrategicMap.Current != null)
            {
                StrategicMap.Current.NotifyUnitChanged(this);
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
    public virtual bool GetInherentAbility(int abilityId)
    {
        if (this.GetAbilitySetAt(abilityId))
        {
            return GamePack.Current.AbilityControl.GetAbility(abilityId).GetInherent(this);
        }

        return false;
    }

    // Unit.PAS:543
    public virtual int GetInherentAbilityLevel(int abilityId)
    {
        if (this.GetAbilitySetAt(abilityId))
        {
            return GamePack.Current.AbilityControl.GetAbility(abilityId).GetInherentLevel(this);
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
    public virtual void SetMovePoints(sbyte value)
    {
    }

    // Unit.PAS:823
    public virtual sbyte GetHitPoints() => 0;

    // Unit.PAS:827
    public virtual void SetHitPoints(sbyte value)
    {
        UpdateDefensiveStrength();
    }

    // Unit.PAS:973
    public virtual Set<MoveType> GetMoveTypes() => GetAbilityMoveTypesAll();

    // Unit.PAS:852
    public virtual Set<DamageType> GetImmunityTypes() => this.GetAbilityImmunityTypesAll() + Set<DamageType>.Of(DamageType.Wall);

    // Unit.PAS:858
    public virtual Set<DamageType> GetProtectionTypes() => this.GetAbilityProtectionTypesAll();

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
    public virtual int GetUnitGraphicsResourceIndex() => -1;

    // Unit.PAS:1480
    public virtual bool CanAddToList(UnitList list) => true;

    // Unit.PAS:179
    public virtual Set<WallCombatFeaturesElement> GetWallCombatFeatures()
    {
        Set<WallCombatFeaturesElement> result = Set<WallCombatFeaturesElement>.Of();
        if (GetAbilityEnabled(1) || GetAbilityEnabled(165) || GetAbilityEnabled(36) || GetAbilityEnabled(66))
        {
            result = result + Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);
        }

        if (GetAbilityEnabled(117) || GetAbilityEnabled(24) || GetAbilityEnabled(65) || GetAbilityEnabled(44))
        {
            result = result + Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);
        }

        return result;
    }

    // Unit.PAS:123
    public virtual int GetCampaignTransferPoints() => GetObtainValue();

    // Unit.PAS:910
    public virtual sbyte GetCastingPointsMax() => 0;

    // Unit.PAS:914
    public virtual sbyte GetCastingPoints() => 0;

    // Unit.PAS:918
    public virtual void SetCastingPoints(sbyte value)
    {
    }

    // Unit.PAS:906
    public virtual int GetPowerGeneration() => 0;

    // Unit.PAS:1484
    public virtual void NewDay()
    {
    }

    // Unit.PAS:1355
    public virtual void NewTurn(sbyte player)
    {
        sbyte heal;
        if (player == Player)
        {
            SetMovePoints(GetMoves());
            if (GetHits() != GetHitPoints() && !GetAbilityEnabled(93))
            {
                if (!GetAbilityEnabled(49))
                {
                    heal = (sbyte)Round((double)GetHits() * 0.1);
                    if (heal == 0)
                    {
                        heal = 1;
                    }
                }
                else
                {
                    heal = GetHits();
                }

                SetHitPoints((sbyte)(GetHitPoints() + heal));
            }

            if (GetAbilityEnabled(109))
            {
                UpdatePathOfFrost();
            }
        }
    }

    // Unit.PAS:1375
    public virtual void NewTurnDone(sbyte player)
    {
        if (player == Player && this.Player > 0)
        {
            if (StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
            {
                SetFireMasteryProtection();
            }

            if (StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.EarthMastery))
            {
                SetEarthMasteryStoneSkin();
            }

            if (StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                SetLifeMasteryBlessing();
            }

            if ((((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 16) != 0)
            {
                ExecuteLifeMasteryFearRole();
            }

            if ((((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 32) != 0)
            {
                ExecuteDeathMasteryCurseRole();
            }
        }
    }

    // Unit.PAS:578
    public virtual int GetAbilityLevel(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetLevel(this);

    // Unit.PAS:552
    public virtual bool GetAbilityEnabled(int abilityId)
    {
        if (GetAbilitySet(abilityId))
        {
            return GamePack.Current.AbilityControl.GetAbility(abilityId).GetEnabled(this);
        }

        return false;
    }

    // Unit.PAS:574
    public virtual string GetAbilityName(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetName(this);

    // Unit.PAS:582
    public virtual AbilityOwner GetAbilityOwner(int abilityId)
    {
        if (this.GetAbilitySetAt(abilityId))
        {
            return this;
        }

        return null;
    }

    // Unit.PAS:530
    public virtual int GetAbilityCount() => AbilityCount;

    // Unit.PAS:747
    public virtual int GetExperience() => 0;

    // Unit.PAS:751
    public virtual void SetExperience(int value)
    {
    }

    // Unit.PAS:753
    public virtual int GetNextLevelExperience() => 0;

    // Unit.PAS:739
    public virtual StringCollection GetDescription() => null;

    // Unit.PAS:494
    public virtual void SetPlayer(sbyte player)
    {
        UnitUserList list;
        if (player != Player)
        {
            AddRef();
            ResetLifeMasteryBlessing();
            ResetLifeMasteryFear();
            ResetEarthMasteryStoneSkin();
            ResetFireMasteryProtection();
            list = Group;
            if (list != null)
            {
                list.DeleteUnit(this);
            }

            Player = player;
            Release();
        }
    }

    // Unit.PAS:513
    public virtual sbyte GetIndependentRelation()
    {
        sbyte result;
        if (GetRace() >= 0)
        {
            result = GetRace();
        }
        else
        {
            result = AlignmentTables.AlignmentToIndependentRelation(GetAlignment());
        }

        return result;
    }

    // Unit.PAS:350
    public virtual int GetObtainValue() => 0;

    // Unit.PAS:346
    public virtual UnitSize GetUnitSize() => UnitSize.Medium;

    // Unit.PAS:260
    public virtual int GetUnitMoraleValue(sbyte player, sbyte independentRelation)
    {
        sbyte race;
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        TerrainType terrainType;
        byte rawTerrain;
        int raceIndex;
        int morale;
        if (player >= 0 && GetUnitType() != UnitType.Machine && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            race = GetRace();
            morale = GetRelationValue(player, independentRelation);
            if (race != -1)
            {
                GetLocation(out hexX, out hexY, out hexLevel);
                if (hexX != -1)
                {
                    rawTerrain = (byte)StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel).TerrainType;
                    raceIndex = race;
                    terrainType = (TerrainType)rawTerrain;
                    if (City.RaceTerrain[raceIndex].Penalty.Contains((byte)(int)terrainType))
                    {
                        // { Read the original shared table, not the private copy. }
                        morale -= 10;
                    }

                    if (City.RaceTerrain[raceIndex].Bonus.Contains((byte)(int)terrainType))
                    {
                        morale += 10;
                    }
                }
            }

            if (GetAbilityEnabled(53))
            {
                morale -= 40;
            }

            if (GetAbilityEnabled(108))
            {
                morale -= 80;
            }

            if (morale < 0)
            {
                morale = 0;
            }

            if (morale > 100)
            {
                morale = 100;
            }
        }
        else
        {
            morale = 50;
        }

        return morale;
    }

    // Unit.PAS:743
    public virtual bool CanActivate() => true;

    // Unit.PAS:761
    public virtual void Activate()
    {
        AIGroup group;
        UnitStatus = UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Activated);
        StrategicMap.Current.UnitControl.RegisterUnit(this);
        if (GroupId != 0 && Player >= 0)
        {
            group = StrategicMap.Current.AIGroupManager.FindId(GroupId);
            if (group != null)
            {
                group.AddUnit(this);
            }
            else
            {
                GroupId = 0;
            }
        }
        else
        {
            GroupId = 0;
        }

        RegisterEnchantments();
        Changed();
    }

    // Unit.PAS:777
    public virtual void Deactivate()
    {
        UnitUserList group;
        group = Group;
        if (group != null)
        {
            group.DeleteUnit(this);
        }

        StrategicMap.Current.UnitControl.UnregisterUnit(this);
        UnitStatus = UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Activated);
        UnregisterEnchantments();
    }

    // Unit.PAS:1100
    public virtual void MovedTo(MapField oldField, MapField newField, int maxCost)
    {
        int cost = default;
        bool transported = default;
        EngineObject army = default;
        cost = MovePointCost(newField.TerrainType, newField.OverlayType);
        if (cost < 0)
        {
            cost = maxCost;
        }
        else
        {
            if (cost > maxCost)
            {
                cost = maxCost;
            }
        }

        if (!StrategicMap.Current.UnlimitedMoves || StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human)
        {
            if (cost > GetMovePoints())
            {
                SetMovePoints(0);
            }
            else
            {
                SetMovePoints((sbyte)(GetMovePoints() - cost));
            }
        }

        if (newField.TerrainType == (int)TerrainType.Earth)
        {
            StrategicMap.Current.ChangeTerrain(null, newField.HexX, newField.HexY, newField.HexLevel, 0, (int)TerrainType.Dirt);
        }

        if (oldField != null)
        {
            army = Owner;
            transported = army is Army && ((Army)army).Transporter(255) != null;
            if (!transported)
            {
                if (oldField.HexLevel == 0)
                {
                    if (GetAbilityEnabled(69))
                    {
                        StrategicMap.Current.ChangeTerrain(null, oldField.HexX, oldField.HexY, oldField.HexLevel, 1, PathOfLifeTerrainChange, null);
                    }
                    else
                    {
                        if (GetAbilityEnabled(68))
                        {
                            StrategicMap.Current.ChangeTerrain(null, oldField.HexX, oldField.HexY, oldField.HexLevel, 1, PathOfDecayTerrainChange, null);
                        }
                    }
                }

                if (GetAbilityEnabled(109))
                {
                    StrategicMap.Current.ChangeTerrain(null, oldField.HexX, oldField.HexY, oldField.HexLevel, 1, PathOfFrostTerrainChange, PathOfFrostTerrainChanged);
                }
            }

            if (GetAbilityEnabled(26))
            {
                ExecuteTrailOfDarkness(oldField);
            }
        }
    }

    // Unit.PAS:1890
    public virtual bool CanDisband(ref string reason)
    {
        bool result = StrategicMap.Current.SeatedPlayer == Player;
        if (result && GetTransporter() && Owner is Army)
        {
            result = ArmyOwner.UnitsInTransporter() == 0;
        }

        return result;
    }

    // Unit.PAS:1796
    public virtual bool CanJoin(sbyte player)
    {
        bool result;
        Alignment alignment;
        DiplomaticRelation relation;
        int I;
        bool condition = Group == null;
        if (!condition)
        {
            var behavior = Group.GroupClass.Behavior;
            condition = !Set<GroupBehavior>.Of(GroupBehavior.Guard, GroupBehavior.GuardArea).Contains(behavior);
        }

        if (condition)
        {
            if (GetRace() == -1)
            {
                var players = StrategicMap.Current.Players.GetPlayers(player);
                alignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).GetAlignment();
                if (GetAlignment() != Units.Alignment.None)
                {
                    relation = AlignmentTables.AlignmentToDiplomaticRelation(GetAlignment(), alignment);
                    if (relation == DiplomaticRelation.Unknown && this is Unit)
                    {
                        result = false;
                        I = StrategicMap.Current.Races.Count;
                        while (!result && I != 0)
                        {
                            I -= 1;
                            bool test = StrategicMap.Current.Races.GetRace(I).GetPlayerRelation(player) >= Status.Polite;
                            if (test)
                            {
                                var unitResourceIndex = ((Unit)this).UnitResource.UnitResourceIndex();
                                test = GamePack.Current.RaceResources.GetRaceResource(I).UnitIndexes.IndexOf(unitResourceIndex) != -1;
                            }

                            result = test;
                        }
                    }
                    else
                    {
                        result = relation == DiplomaticRelation.Peace;
                    }
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                var race = GetRace();
                result = StrategicMap.Current.Races.GetRace(race).GetPlayerRelation(player) >= Status.Polite;
            }

            if (result && Group != null && Group.GroupClass.Behavior == GroupBehavior.Hunt && ((HuntAIGroup)Group).HuntingPlayer(player))
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:1835
    public virtual int JoinAmount(sbyte player)
    {
        int result;
        UnitUserList group;
        group = Group;
        if (group != null && ((AIGroup)(object)group).GroupClass.Behavior == GroupBehavior.Refuge)
        {
            result = 0;
        }
        else
        {
            result = Round((double)GetObtainValue() * 0.75);
        }

        return result;
    }

    // Unit.PAS:1843
    public virtual bool OfferToJoin(sbyte player)
    {
        bool result;
        UnitUserList group;
        Alignment alignment;
        if (CanJoin(player))
        {
            if (GetRace() != -1)
            {
                var race = GetRace();
                result = StrategicMap.Current.Races.GetRace(race).GetPlayerRelation(player) >= Status.Polite;
            }
            else
            {
                group = Group;
                if (group != null && ((AIGroup)(object)group).GroupClass.Behavior == GroupBehavior.Refuge)
                {
                    var players = StrategicMap.Current.Players.GetPlayers(player);
                    alignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).GetAlignment();
                    if ((GetAlignment() != Units.Alignment.None || GetUnitType() == UnitType.Machine) && AlignmentTables.AlignmentToDiplomaticRelation(GetAlignment(), alignment) != DiplomaticRelation.Peace)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                else
                {
                    result = false;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:1872
    public virtual void GetJoinMessage(sbyte player, int count, int amount, JoinMode mode, StringCollection messages)
    {
        UnitUserList group;
        messages.Clear();
        switch (mode)
        {
            case JoinMode.Default:
                group = Group;
                if (group != null && ((AIGroup)(object)group).GroupClass.Behavior == GroupBehavior.Refuge && amount == 0)
                {
                    if (count == 0)
                    {
                        messages.Text = AbilityRules.TranslateResourceString(GetEngine(), GameText.RefugeJoinText);
                    }
                    else
                    {
                        messages.Text = AbilityRules.TranslateResourceString(GetEngine(), GameText.RefugeesJoinText);
                    }
                }
                else
                {
                    if (count == 0)
                    {
                        messages.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.BribeUnitJoinFormat), [IntToStr(amount)]);
                    }
                    else
                    {
                        messages.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.BribeUnitsJoinFormat), [IntToStr(amount)]);
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
        UnitUserList group;
        UnitStatus = UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        ResetLifeMasteryBlessing();
        ResetLifeMasteryFear();
        ResetEarthMasteryStoneSkin();
        ResetFireMasteryProtection();
        UnregisterEnchantments();
        group = Group;
        if (group != null)
        {
            group.DeleteUnit(this);
        }

        SetPlayer(-1);
        Changed();
    }

    // Unit.PAS:1224
    public virtual void Resurrect()
    {
        this.ExpandAbility(128);
        UnitStatus = UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        SetHitPoints(1);
        SetPlayer(0);
    }

    // Unit.PAS:1231
    public virtual void Animate()
    {
        this.ExpandAbility(129);
        UnitStatus = UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        SetHitPoints(1);
        SetPlayer(0);
    }

    // Unit.PAS:152
    // { A stack-argument assembler routine gets an EBP frame; remove it before jumping. }
    public void RetrieveDamageInfo(sbyte attack, sbyte damage, Set<DamageType> damageTypes, out int damageValue, out int obtainValue)
    {
        int maxDamage;
        maxDamage = GetHitPoints() * 100;
        damageValue = MeleeRound.StatisticsToDamageValue(attack, damage, damageTypes, GetDefense(), GetResistance(), GetImmunityTypes(), GetProtectionTypes());
        if (maxDamage < damageValue)
        {
            damageValue = maxDamage;
        }

        if (damageValue > 0 && GetHitPoints() > 0)
        {
            if (maxDamage > damageValue)
            {
                obtainValue = Round((double)GetObtainValue() * ((double)((double)damageValue * 0.75) / maxDamage));
            }
            else
            {
                obtainValue = GetObtainValue();
            }
        }
        else
        {
            obtainValue = 0;
        }
    }

    // A hero list is not an army or a unit list. Callers treat that owner as absent. A unit list
    // that is not an army fails the cast.
    internal Army ArmyOwner => Owner is HeroList ? null : (Army)Owner;

    // The owner is a unit list or a hero list (AcceptsOwner), so this reads the unit list or null.
    internal UnitList UnitListOwner => Owner as UnitList;

    // Unit.PAS:189
    public void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel)
    {
        if (Owner is UnitList)
        {
            UnitListOwner.GetLocation(out hexX, out hexY, out hexLevel);
        }
        else
        {
            hexX = -1;
            hexY = -1;
            hexLevel = -1;
        }
    }

    // Unit.PAS:231
    public int GetRelationValue(sbyte player, sbyte independentRelation)
    {
        int result;
        sbyte race;
        race = GetRace();
        if (player != 0)
        {
            if (race != -1 && !GetAbilityEnabled(107))
            {
                result = StrategicMap.Current.Races.GetRace(race).GetPlayerRelationValue(player);
            }
            else
            {
                var players = StrategicMap.Current.Players.GetPlayers(player);
                var playerAlignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).GetAlignment();
                var unitAlignment = GetAlignment();
                result = AlignmentTables.AlignmentToStatusValue[(int)unitAlignment][(int)playerAlignment];
            }
        }
        else
        {
            var iRtoAlignment = AbilityRules.IndependentRelationToAlignment(independentRelation);
            var alignment = GetAlignment();
            result = AlignmentTables.AlignmentToStatusValue[(int)alignment][(int)iRtoAlignment];
        }

        return result;
    }

    // Unit.PAS:256
    public Status GetRelation(sbyte player, sbyte independentRelation) => Race.StatusValueToStatus(GetRelationValue(player, independentRelation));

    // Unit.PAS:301
    public Status UnitRelation(Troop troop)
    {
        Status result;
        sbyte race;
        sbyte otherRace;
        race = GetRace();
        otherRace = troop.GetRace();
        if (race != -1 && otherRace != -1)
        {
            result = StrategicMap.Current.Races.GetRace(race).GetRelation(otherRace);
        }
        else
        {
            var alignment = troop.GetAlignment();
            result = AlignmentTables.AlignmentToStatus(GetAlignment(), alignment);
        }

        return result;
    }

    // Unit.PAS:312
    public sbyte UpdateMoraleValue()
    {
        int player;
        int value;
        Troop troop;
        player = Player;
        value = GetUnitMoraleValue((sbyte)player, GetIndependentRelation());
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && GetUnitType() != UnitType.Machine && !(this is Leader) && Owner is Army)
        {
            if (GetUpkeep() != 0 && player > 0 && !GetAbilityEnabled(155))
            {
                value += StrategicMap.Current.Players.GetPlayers(player).MoraleValue;
            }

            for (int I = Owner.Count - 1; I >= 0; I--)
            {
                troop = UnitListOwner[I];
                if (troop.GetAbilityEnabled(37) && troop != this)
                {
                    value += 30;
                }
                else
                {
                    if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(UnitRelation(troop)))
                    {
                        value -= 10;
                    }
                }
            }
        }

        if (value > 125)
        {
            MoraleValue = 125;
        }
        else
        {
            if (value < -25)
            {
                MoraleValue = -25;
            }
            else
            {
                MoraleValue = (sbyte)value;
            }
        }

        return MoraleValue;
    }

    // Unit.PAS:342
    public Morale GetMorale() => AbilityRules.MoraleValueToMorale(MoraleValue);

    // Unit.PAS:358
    public void UpdateEnchanted()
    {
        int I;
        AbilityTypeList list;
        bool enchanted;
        list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        enchanted = false;
        I = list.Count;
        while (!enchanted && I != 0)
        {
            I -= 1;
            enchanted = GetAbilityEnabled(list.GetAbility(I).AbilityId);
        }

        if (enchanted)
        {
            UnitStatus = UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Enchanted);
        }
        else
        {
            UnitStatus = UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Enchanted);
        }
    }

    // Unit.PAS:375
    public void RemoveEnchantments()
    {
        Ability ability;
        AbilityTypeList list;
        AddRef();
        list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int I = list.Count - 1; I >= 0; I--)
        {
            ability = list.GetAbility(I);
            if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
            {
                RemoveAbility(ability.AbilityId);
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
        Ability ability;
        AbilityTypeList list;
        UnitEnchantmentAbilityData data;
        list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int I = list.Count - 1; I >= 0; I--)
        {
            ability = list.GetAbility(I);
            if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
            {
                data = (UnitEnchantmentAbilityData)this.GetAbilityData(ability.AbilityId);
                if (data.Enchantment.Source > -1)
                {
                    StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).MagicControl.RegisterEnchantment(data.Enchantment);
                }
            }
        }
    }

    // Unit.PAS:424
    public void UnregisterEnchantments()
    {
        Ability ability;
        AbilityTypeList list;
        UnitEnchantmentAbilityData data;
        list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int I = list.Count - 1; I >= 0; I--)
        {
            ability = list.GetAbility(I);
            if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
            {
                data = (UnitEnchantmentAbilityData)this.GetAbilityData(ability.AbilityId);
                if (data.Enchantment.Source > -1)
                {
                    StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).MagicControl.UnregisterEnchantment(data.Enchantment);
                }
            }
        }
    }

    // Unit.PAS:449
    public UnitEnchantment AbilityToEnchantment(int abilityId)
    {
        UnitEnchantment result;
        Ability ability;
        UnitEnchantmentAbilityData data;
        ability = GamePack.Current.AbilityControl.GetAbility(abilityId);
        if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
        {
            data = (UnitEnchantmentAbilityData)this.GetAbilityData(ability.AbilityId);
            if (data != null)
            {
                result = data.Enchantment;
            }
            else
            {
                return null;
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Unit.PAS:463
    public bool CanDispelEnchantment(sbyte player)
    {
        int I;
        AbilityTypeList list;
        UnitEnchantment enchantment;
        list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        I = list.Count;
        bool result = false;
        while (!result && I != 0)
        {
            I -= 1;
            enchantment = AbilityToEnchantment(list.GetAbility(I).AbilityId);
            if (enchantment == null || player == enchantment.Source)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // Unit.PAS:481
    public void ListEnchantments(ECustomNode list)
    {
        AbilityTypeList abilities;
        UnitEnchantment enchantment;
        abilities = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int I = abilities.Count - 1; I >= 0; I--)
        {
            enchantment = AbilityToEnchantment(abilities.GetAbility(I).AbilityId);
            if (enchantment != null)
            {
                list.AddChild(enchantment);
            }
        }
    }

    // Unit.PAS:620
    public void UpdateDefensiveStrength()
    {
        _defensiveStrength = (GetDefense() + 10 + GetResistance()) * (GetHitPoints() + 10);
        if (_defensiveStrength < 0)
        {
            _defensiveStrength = 1;
        }
    }

    // Unit.PAS:657
    public int GetStrength(DamageType damageType)
    {
        if (!UnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateDefensiveStrength();
            UpdateOffensiveStrength();
        }

        int result = _offensiveStrength[(int)damageType];
        if (result != 0)
        {
            result = (result + 10) * _defensiveStrength;
        }

        return result;
    }

    // Unit.PAS:664
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        int result;
        DamageType damageType;
        if (!UnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateDefensiveStrength();
            UpdateOffensiveStrength();
        }

        if (damageTypes != TroopConstants.AllDamageTypes)
        {
            result = 0;
            for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
            {
                damageType = (DamageType)damageTypeOrdinal;
                if (damageTypes.Contains(damageType))
                {
                    if (result < _offensiveStrength[(int)damageType])
                    {
                        result = _offensiveStrength[(int)damageType];
                    }
                }
            }

            if (result != 0)
            {
                result = (result + 10) * _defensiveStrength;
            }
        }
        else
        {
            if (MaxOffensiveStrength != 0)
            {
                result = (MaxOffensiveStrength + 10) * _defensiveStrength;
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }

    // Unit.PAS:688
    public int GetCombatValue(Troop target, byte mode)
    {
        int value;
        AbilityTypeList abilities;
        Ability ability;
        AbilityDamageValues info = default;
        int result = 0;
        abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
        for (int I = abilities.Count - 1; I >= 0; I--)
        {
            ability = abilities.GetAbility(I);
            if (GetAbilityEnabled(ability.AbilityId))
            {
                ability.GetDamageValues(this, target, (WallSituation)mode, ref info);
                if (info.SourceDamage > 0)
                {
                    value = info.SourceDamage * info.SourceDamage / (info.TargetDamage + info.SourceDamage);
                    if (value <= 0)
                    {
                        value = 1;
                    }

                    if (value > result)
                    {
                        result = value;
                    }
                }
            }
        }

        return result;
    }

    // Unit.PAS:797
    public int GetManaUpkeep()
    {
        int result;
        Enchantment enchantment;
        if (GetAbilityEnabled(155))
        {
            enchantment = ((UnitEnchantmentAbilityData)this.GetAbilityData(155)).Enchantment;
            if (enchantment != null)
            {
                result = enchantment.Upkeep;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Unit.PAS:831
    public bool GetTransporting()
    {
        bool result;
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        MapField mapField;
        GetLocation(out hexX, out hexY, out hexLevel);
        if (hexX != -1)
        {
            mapField = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
            result = !ValidTerrain(mapField.TerrainType, mapField.OverlayType);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:922
    public void CreateMovePointTable(out MovePointTable table)
    {
        Set<MoveType> moves;
        int points;
        sbyte value;
        bool faster;
        bool slower;
        moves = GetMoveTypes();
        if (StrategicMap.Current.Combat != null)
        {
            moves = moves - Set<MoveType>.Of(MoveType.Tunneling);
        }

        var moveTypesToInt32 = MovementTables.MoveTypesToInt32(moves);
        table = MovementTables.MovePointTables[moveTypesToInt32];
        if (Player > 0)
        {
            if (StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.EnchantedRoads))
            {
                for (int terrain = 0; terrain <= 15; terrain++)
                {
                    if (table[terrain][4 - MovementTables.FirstOverlay] != -1)
                    {
                        table[terrain][4 - MovementTables.FirstOverlay] = 2;
                    }
                }
            }

            if (GetAbilityEnabled(23) || GetAbilityEnabled(1) || GetAbilityEnabled(165))
            {
                faster = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery);
                slower = (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 4) != 0;
                if (faster && !slower)
                {
                    for (int overlay = -1; overlay <= 11; overlay++)
                    {
                        for (int terrain = 0; terrain <= 15; terrain++)
                        {
                            value = table[terrain][overlay - MovementTables.FirstOverlay];
                            if (value > 2)
                            {
                                table[terrain][overlay - MovementTables.FirstOverlay] = (sbyte)(int)(((uint)(value + 1)) >> 1);
                            }
                        }
                    }
                }
                else
                {
                    if (!faster && slower)
                    {
                        for (int overlay = -1; overlay <= 11; overlay++)
                        {
                            for (int terrain = 0; terrain <= 15; terrain++)
                            {
                                if (table[terrain][overlay - MovementTables.FirstOverlay] >= 1)
                                {
                                    table[terrain][overlay - MovementTables.FirstOverlay] = (sbyte)(table[terrain][overlay - MovementTables.FirstOverlay] * 2);
                                }
                            }
                        }
                    }
                }
            }

            if (GetAbilityEnabled(152))
            {
                for (int overlay = -1; overlay <= 11; overlay++)
                {
                    for (int terrain = 0; terrain <= 15; terrain++)
                    {
                        points = table[terrain][overlay - MovementTables.FirstOverlay];
                        if (points > 2)
                        {
                            points -= 1;
                            // { Original retains this redundant lower bound after Points > 2. }
                            if (points < 2)
                            {
                                points = 2;
                            }
                        }

                        table[terrain][overlay - MovementTables.FirstOverlay] = (sbyte)points;
                    }
                }
            }

            if (GetAbilityEnabled(132))
            {
                for (int overlay = -1; overlay <= 11; overlay++)
                {
                    for (int terrain = 0; terrain <= 15; terrain++)
                    {
                        points = table[terrain][overlay - MovementTables.FirstOverlay];
                        if (points > 0)
                        {
                            table[terrain][overlay - MovementTables.FirstOverlay] = (sbyte)(points + 2);
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:993
    public bool ValidTerrain(sbyte terrainType, sbyte overlayType)
    {
        bool result;
        sbyte cost;
        // { Original retrieves the movement set twice and retains the first dead store. }
        GetMoveTypes();
        var moveTypesToInt32 = MovementTables.MoveTypesToInt32(GetMoveTypes());
        cost = MovementTables.MovePointTables[moveTypesToInt32][terrainType][overlayType - MovementTables.FirstOverlay];
        if (cost == -1 || cost > GetMoves())
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Unit.PAS:1011
    public void ExecuteTrailOfDarkness(MapField mapField)
    {
        MapLevel level;
        HexNumberWalkData walk = default;
        MapField field;
        level = mapField.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, mapField.HexX, mapField.HexY, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.X < level.Width && walk.Y >= 0 && walk.Y < level.Height)
            {
                field = level.GetField(walk.X, walk.Y);
                if (field.TerrainType != (int)TerrainType.Border)
                {
                    for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
                    {
                        if (player != Player && StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.War && field.GetExplored(player) && !StrategicMap.Current.VisibleForPlayer(walk.X, walk.Y, mapField.HexLevel, (sbyte)player))
                        {
                            field.SetExplored(player, false);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        StrategicMap.Current.MapChangedRadius(mapField.HexX, mapField.HexY, mapField.HexLevel, 1);
    }

    // Unit.PAS:1038
    public void PathOfLifeTerrainChange(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        if (terrainType != (int)World.Map.TerrainType.Ice && terrainType != (int)World.Map.TerrainType.Water && mapField.HexLevel == 0)
        {
            terrainType = (int)World.Map.TerrainType.Grass;
        }
    }

    // Unit.PAS:1043
    public void PathOfDecayTerrainChange(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        if (terrainType != (int)World.Map.TerrainType.Ice && terrainType != (int)World.Map.TerrainType.Water && mapField.HexLevel == 0)
        {
            terrainType = (int)World.Map.TerrainType.Wasteland;
        }
    }

    // Unit.PAS:1048
    public void PathOfFrostTerrainChange(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        FrozenWaterHexSprite ice;
        if (terrainType == (int)World.Map.TerrainType.Water || terrainType == (int)World.Map.TerrainType.UndergroundWater)
        {
            if (terrainType == (int)World.Map.TerrainType.Water)
            {
                terrainType = (int)World.Map.TerrainType.Ice;
            }
            else
            {
                terrainType = (int)World.Map.TerrainType.UndergroundIce;
            }
        }
        else
        {
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)terrainType))
            {
                ice = (FrozenWaterHexSprite)mapField.FindOwnedHexSprite(typeof(FrozenWaterHexSprite));
                if (ice != null)
                {
                    ice.DaysRemaining = 2;
                    ice.CasterPlayer = Player;
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(1).With(2).With(4).With(5).Contains((byte)terrainType) && mapField.HexLevel == 0)
                {
                    terrainType = (int)World.Map.TerrainType.Snow;
                }
            }
        }
    }

    // Unit.PAS:1066
    public void PathOfFrostTerrainChanged(MapField mapField, sbyte originalTerrainType, sbyte originalOverlayType)
    {
        FrozenWaterHexSprite ice;
        if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)mapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)originalTerrainType))
        {
            ice = (FrozenWaterHexSprite)mapField.FindOwnedHexSprite(typeof(FrozenWaterHexSprite));
            if (ice == null)
            {
                ice = new FrozenWaterHexSprite(null) { Holder = CreationHolder() };
                StrategicMap.Current.PlaceHex(ice, mapField.HexX, mapField.HexY, mapField.HexLevel);
            }

            ice.DaysRemaining = 2;
            ice.CasterPlayer = Player;
        }
    }

    // Unit.PAS:1082
    public void UpdatePathOfFrost()
    {
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        MapField field;
        FrozenWaterHexSprite ice;
        GetLocation(out hexX, out hexY, out hexLevel);
        if (hexX > -1)
        {
            field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)field.TerrainType))
            {
                ice = (FrozenWaterHexSprite)field.FindOwnedHexSprite(typeof(FrozenWaterHexSprite));
                if (ice != null)
                {
                    ice.DaysRemaining = 2;
                    ice.CasterPlayer = Player;
                }
            }
        }
    }

    // Unit.PAS:1134
    public bool ConcealedOnMapField(MapField field)
    {
        bool result;
        if (!GetAbilityEnabled(54) && !field.GetConcealmentActive())
        {
            result = GetAbilityEnabled(139) && field.HexLevel != 0;
            if (field.OverlayType != 1 || !(GetAbilityEnabled(145) || GetAbilityEnabled(162)))
            {
                // { Original terrain branches overwrite the underground concealment result. }
                switch ((short)field.TerrainType)
                {
                    case 1:
                        result = GetAbilityEnabled(140);
                        break;
                    case 3:
                        result = GetAbilityEnabled(141);
                        break;
                    case 2:
                        result = GetAbilityEnabled(142);
                        break;
                    case 5:
                        result = GetAbilityEnabled(143);
                        break;
                    case 0:
                    case 10:
                        result = GetAbilityEnabled(144);
                        break;
                    case 4:
                        result = GetAbilityEnabled(138);
                        break;
                }
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Unit.PAS:1154
    public int ExecuteStormDamage(StormType storm, int distance)
    {
        int result;
        sbyte terrain;
        sbyte overlay;
        Set<MoveType> moves;
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        MapField field;
        bool doubleDamage;
        int attack;
        moves = GetMoveTypes();
        doubleDamage = GetAbilityEnabled(23);
        if (!doubleDamage && moves.Contains(MoveType.Flying))
        {
            GetLocation(out hexX, out hexY, out hexLevel);
            if (hexX != -1)
            {
                field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
                terrain = field.TerrainType;
                overlay = field.OverlayType;
                moves = moves - Set<MoveType>.Of(MoveType.Flying);
                var moveTypesToInt32 = MovementTables.MoveTypesToInt32(moves);
                doubleDamage = MovementTables.MovePointTables[moveTypesToInt32][terrain][overlay - MovementTables.FirstOverlay] == 0;
            }
        }

        switch (storm)
        {
            case StormType.Ice:
                attack = 7;
                result = 5;
                break;
            case StormType.Lightning:
                attack = 8;
                result = 7;
                break;
            case StormType.Death:
                attack = 7;
                result = 4;
                break;
            case StormType.Holy:
                attack = 7;
                result = 5;
                break;
            case StormType.Fire:
                attack = 7;
                result = 5;
                break;
            case StormType.Pestilence:
                attack = 4;
                result = 3;
                break;
            default:
                attack = 0;
                result = 0;
                break;
        }

        if (doubleDamage)
        {
            result = result * 2;
        }

        result = ExecuteDamageRole(attack, result, Set<DamageType>.Of(Army.StormDamageType[(int)storm]));
        if (result > GetHitPoints())
        {
            result = GetHitPoints();
        }

        if (result > 0)
        {
            switch (storm)
            {
                case StormType.Death:
                    ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Death));
                    break;
                case StormType.Pestilence:
                    ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Poison));
                    break;
            }
        }

        SetHitPoints((sbyte)(GetHitPoints() - result));
        return result;
    }

    // Unit.PAS:1198
    // { Result is the final damage. }
    public int MovePointCost(sbyte terrainType, sbyte overlayType)
    {
        MovePointTable table = default;
        CreateMovePointTable(out table);
        return table[terrainType][overlayType - MovementTables.FirstOverlay];
    }

    // Unit.PAS:1238
    public void ResetEarthMasteryStoneSkin()
    {
        if (GetAbilityEnabled(153))
        {
            if (((UnitEnchantmentAbilityData)this.GetAbilityData(153)).Enchantment.Source == -2)
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
                ((UnitEnchantmentAbilityData)this.GetAbilityData(153)).Enchantment.SetUpkeep(0);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(153)).Enchantment.SetSource(-2);
                var spell = GamePack.Current.SpellControl.GetSpell(17);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(153)).Enchantment.DispelResistance = (sbyte)((UnitSpell)(object)spell).UnitDispelResistance;
            }
        }
    }

    // Unit.PAS:1259
    public void ResetFireMasteryProtection()
    {
        if (GetAbilityEnabled(167))
        {
            if (((UnitEnchantmentAbilityData)this.GetAbilityData(167)).Enchantment.Source == -2)
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
                ((UnitEnchantmentAbilityData)this.GetAbilityData(167)).Enchantment.SetUpkeep(0);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(167)).Enchantment.SetSource(-2);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(167)).Enchantment.DispelResistance = 7;
            }
        }
    }

    // Unit.PAS:1278
    public void ResetLifeMasteryBlessing()
    {
        if (GetAbilityEnabled(158))
        {
            if (((UnitEnchantmentAbilityData)this.GetAbilityData(158)).Enchantment.Source == -2)
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
                ((UnitEnchantmentAbilityData)this.GetAbilityData(158)).Enchantment.SetUpkeep(0);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(158)).Enchantment.SetSource(-2);
                var spell = GamePack.Current.SpellControl.GetSpell(56);
                ((UnitEnchantmentAbilityData)this.GetAbilityData(158)).Enchantment.DispelResistance = (sbyte)((UnitSpell)(object)spell).UnitDispelResistance;
            }
        }
    }

    // Unit.PAS:1305
    public void ExecuteLifeMasteryFearRole()
    {
        DurationAbilityData data;
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
                data = (DurationAbilityData)this.GetAbilityData(53);
                data.Source = 1;
                data.Duration = 0;
            }
        }
    }

    // Unit.PAS:1319
    public void ValidateLifeMastery()
    {
        sbyte player;
        player = Player;
        if (player > 0)
        {
            if (!StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                ResetLifeMasteryBlessing();
            }

            if ((((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 16) == 0)
            {
                ResetLifeMasteryFear();
            }
        }
    }

    // Unit.PAS:1332
    public void ExecuteDeathMasteryCurseRole()
    {
        DurationAbilityData data;
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
                data = (DurationAbilityData)this.GetAbilityData(93);
                data.Source = 1;
            }
        }
    }

    // Unit.PAS:1344
    public void ValidateDeathMastery()
    {
        sbyte player;
        player = Player;
        if (player > 0)
        {
            if ((((byte)StrategicMap.Current.Players.GetPlayers(player).MagicControl.EnemyEnchantments.Image) & 32) == 0)
            {
                if (GetAbilityEnabled(93))
                {
                    if (((DurationAbilityData)this.GetAbilityData(93)).Source == 1)
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
        int result;
        int level;
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        level = GetAbilityLevel(64);
        if (level != 0)
        {
            result = level * 2 + 4;
        }
        else
        {
            result = 4;
        }

        GetLocation(out hexX, out hexY, out hexLevel);
        if (hexLevel != 0 && !GetAbilityEnabled(39) || hexLevel == 0 && Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 8) != 0)
        {
            result = (int)(((uint)(result + 1)) >> 1);
        }

        return result;
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
    public bool CanPlace(int X, int Y, int L, sbyte player, Set<UnitPlaceModeElement> mode)
    {
        bool result;
        MapField field;
        ArmyHexSprite armyHexSprite;
        sbyte fieldPlayer;
        if (X >= 0 && Y >= 0 && X < StrategicMap.Current.MapContainer.Width && Y < StrategicMap.Current.MapContainer.Height)
        {
            field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
            fieldPlayer = field.GetPlayer();
            if (mode.Contains(UnitPlaceModeElement.NeglectEmptyPlayerLocation) || fieldPlayer == player || fieldPlayer == -1 || player == -1)
            {
                armyHexSprite = (ArmyHexSprite)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131607);
                if (armyHexSprite != null)
                {
                    if (mode.Contains(UnitPlaceModeElement.CreateNewArmy) || (player != -1 && armyHexSprite.Army.Player != player) || !armyHexSprite.Army.CanAddUnit(this))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                else
                {
                    result = ValidTerrain(field.TerrainType, field.OverlayType);
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:1454
    public ArmyHexSprite Place(int X, int Y, int L, sbyte player, Set<UnitPlaceModeElement> mode)
    {
        HexNumberWalkData walk = default;
        ArmyHexSprite armyHexSprite;
        ArmyHexSprite result = null;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (result == null && (walk.Radius == 0 || mode.Contains(UnitPlaceModeElement.ScanArea)) && walk.Radius < 30)
        {
            if (CanPlace(walk.X, walk.Y, L, player, mode))
            {
                armyHexSprite = (ArmyHexSprite)StrategicMap.Current.MapContainer.GetField(walk.X, walk.Y, L).FindOwnedChild(131607);
                if (armyHexSprite == null)
                {
                    armyHexSprite = new ArmyHexSprite(null) { Holder = CreationHolder() };
                    if (player != -1)
                    {
                        armyHexSprite.Army.SetPlayer(player);
                    }

                    if (!armyHexSprite.Army.AddUnit(this) || !StrategicMap.Current.PlaceHex(armyHexSprite, walk.X, walk.Y, L))
                    {
                        armyHexSprite?.Dispose();
                        armyHexSprite = null;
                    }

                    result = armyHexSprite;
                }
                else
                {
                    if (armyHexSprite.Army.AddUnit(this))
                    {
                        result = armyHexSprite;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }

            if (result == null)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // Unit.PAS:1486
    public void ValidateCrusader()
    {
        DurationAbilityData data;
        data = (DurationAbilityData)GetAbilityData(101);
        if (data != null)
        {
            if (data.Duration <= 0)
            {
                Killed();
            }
        }
    }

    // Unit.PAS:1504
    public void Show(int X, int Y, in LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        info.Direction = 3;
        info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
        info.Animation = UnitAnimation.StandStill;
        info.Frame = 0;
        info.Looped = false;
        Show(X, Y, lockedSurface, info);
    }

    // Unit.PAS:1610
    public void ShowFace(in Rect R, in LockedSurface lockedSurface)
    {
        LibraryImage image;
        image = GetFace();
        if (image != null)
        {
            image.ShowStretched(R, lockedSurface);
        }

        if (GetAbilityEnabled(129))
        {
            if (StrategicMap.Current != null)
            {
                if (GamePack.Current.GeneralImages[45] != null)
                {
                    GamePack.Current.GeneralImages[45].ShowStretched(R, lockedSurface);
                }
            }
        }
    }

    // Unit.PAS:1623
    public int ExecuteDamageRole(int attack, int damage, Set<DamageType> damageTypes)
    {
        Set<DamageType> active;
        int displayController;
        int result = 0;
        active = damageTypes - GetImmunityTypes();
        if (active != Set<DamageType>.Of())
        {
            displayController = attack - GetDefense();
            result = AbilityRules.ExecuteDamageRole(damage, displayController);
            if (active - GetProtectionTypes() == Set<DamageType>.Of())
            {
                result = (result + 1) / 2;
            }
        }

        return result;
    }

    // Unit.PAS:1635
    public bool ExecuteResistanceRole(int attack, DamageType damageType)
    {
        bool result;
        if (!GetImmunityTypes().Contains(damageType))
        {
            if (GetProtectionTypes().Contains(damageType))
            {
                attack -= 2;
            }

            result = HitChance.HitRole(attack - GetResistance());
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:1644
    public Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> damageTypes)
    {
        Set<DamageType> active;
        Set<DamageType> protection;
        int resistance;
        int effectiveResistance;
        Set<DamageType> result = Set<DamageType>.Of();
        active = damageTypes - GetImmunityTypes();
        if (active != Set<DamageType>.Of())
        {
            resistance = GetResistance();
            protection = GetProtectionTypes();
            if (active.Contains(DamageType.Fire) && !GetAbilityEnabled(7))
            {
                if (protection.Contains(DamageType.Fire))
                {
                    effectiveResistance = resistance + 2;
                }
                else
                {
                    effectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(4 - effectiveResistance))
                {
                    result = result + Set<DamageType>.Of(DamageType.Fire);
                }
            }

            if (active.Contains(DamageType.Lightning) && GetUnitType() != UnitType.Machine && !GetAbilityEnabled(9))
            {
                if (protection.Contains(DamageType.Lightning))
                {
                    effectiveResistance = resistance + 2;
                }
                else
                {
                    effectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(6 - effectiveResistance))
                {
                    result = result + Set<DamageType>.Of(DamageType.Lightning);
                }
            }

            if (active.Contains(DamageType.Poison) && !GetAbilityEnabled(10))
            {
                if (protection.Contains(DamageType.Poison))
                {
                    effectiveResistance = resistance + 2;
                }
                else
                {
                    effectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(3 - effectiveResistance))
                {
                    result = result + Set<DamageType>.Of(DamageType.Poison);
                }
            }

            if (active.Contains(DamageType.Death) && !GetAbilityEnabled(11))
            {
                if (protection.Contains(DamageType.Death))
                {
                    effectiveResistance = resistance + 2;
                }
                else
                {
                    effectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(4 - effectiveResistance))
                {
                    result = result + Set<DamageType>.Of(DamageType.Death);
                }
            }

            if (active.Contains(DamageType.Holy) && !GetAbilityEnabled(12))
            {
                if (protection.Contains(DamageType.Holy))
                {
                    effectiveResistance = resistance + 2;
                }
                else
                {
                    effectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(3 - effectiveResistance) && GetUnitType() != UnitType.Machine)
                {
                    result = result + Set<DamageType>.Of(DamageType.Holy);
                }
            }

            if (active.Contains(DamageType.Cold) && !GetAbilityEnabled(8))
            {
                if (protection.Contains(DamageType.Cold))
                {
                    effectiveResistance = resistance + 2;
                }
                else
                {
                    effectiveResistance = GetResistance();
                }

                if (HitChance.HitRole(2 - effectiveResistance))
                {
                    result = result + Set<DamageType>.Of(DamageType.Cold);
                }
            }
        }

        return result;
    }

    // Unit.PAS:1699
    public Set<DamageType> ExecuteDamageEffects(Set<DamageType> damageTypes)
    {
        Set<DamageType> active;
        Set<DamageType> result = Set<DamageType>.Of();
        active = damageTypes - GetImmunityTypes();
        // { Preserve EBX allocation and byte membership tests. }
        if (active != Set<DamageType>.Of())
        {
            if (active.Contains(DamageType.Poison) && this.ExpandAbility(96))
            {
                result = result + Set<DamageType>.Of(DamageType.Poison);
            }

            if (active.Contains(DamageType.Death) && this.ExpandAbility(93))
            {
                result = result + Set<DamageType>.Of(DamageType.Death);
            }

            if (active.Contains(DamageType.Holy) && this.ExpandAbility(98))
            {
                result = result + Set<DamageType>.Of(DamageType.Holy);
            }
        }

        return result;
    }

    // Unit.PAS:1747
    public DamageType OptimalDamageType(Set<DamageType> damageTypes)
    {
        Set<DamageType> active;
        Set<DamageType> protection;
        DamageType result = DamageType.None;
        active = damageTypes - GetImmunityTypes();
        if (active != Set<DamageType>.Of())
        {
            protection = GetProtectionTypes();
            if (protection == Set<DamageType>.Of() || active - protection == Set<DamageType>.Of())
            {
                while (!active.Contains(result))
                {
                    result = (DamageType)((int)result - 1);
                }
            }
            else
            {
                while (!active.Contains(result) || protection.Contains(result))
                {
                    result = (DamageType)((int)result - 1);
                }
            }
        }

        return result;
    }

    // Unit.PAS:1899
    public bool Disband()
    {
        bool result;
        string reason = "";
        DisbandUnitTokenEvent eventValue;
        if (CanDisband(ref reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            eventValue = new DisbandUnitTokenEvent(null) { Holder = CreationHolder() };
            eventValue.UnitId = UnitId;
            eventValue.Player = Player;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // The producer ExecuteLifeMasteryFearRole writes the source marker on duration data.
    public void ResetLifeMasteryFear()
    {
        if (GetAbilityEnabled(53) && ((DurationAbilityData)GetAbilityData(53)).Source == 1)
            RemoveAbility(53);
    }

    public virtual bool GetAbilitySet(int index) => (uint)index < (uint)AbilityCount && (AbilityBits[index >> 3] & (1 << (index & 7))) != 0;

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
            var ability = GamePack.Current.AbilityControl.GetAbility(i);
            var name = ability.GetName(owner);
            if (!IsEmpty(name)) names.AddObject(name, IntToObject(i));
            types?.AddObject(AbilityRules.TranslateResourceString(GetEngine(), GameTables.AbilityTypeTexts[(int)ability.GetAbilityType()]), IntToObject(i));
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
            if (data != null) data.EnchantmentPlayerIndex = (sbyte)combat.CombatData.CurrentPlayer;
            RemoveAbility(127);
            result += Set<DamageType>.Of(DamageType.Cold);
        }
        if (active.Contains(DamageType.Lightning) && ExpandAbility(92))
        {
            var data = (DurationAbilityData)GetAbilityData(92);
            if (data != null) data.PlayerIndex = combat.CombatData.CurrentPlayer;
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
        var abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
        MaxOffensiveStrength = 0;
        Array.Clear(_offensiveStrength);
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            var ability = abilities.GetAbility(i);
            if (!GetAbilityEnabled(ability.AbilityId)) continue;
            int strength = 0;
            Set<DamageType> types = default;
            ability.GetOffensiveStrength(this, ref strength, ref types);
            if (strength > 255) strength = 255;
            if (types == default)
                Assert(false, $"Ability: {ability.GetName(null)}", @"D:\AoWDev\AoWE\inc\Unit.PAS", 0xE2B4);
            if (types == default) continue;
            if (MaxOffensiveStrength < strength) MaxOffensiveStrength = (byte)strength;
            for (int kind = (int)DamageType.Fire; kind <= (int)DamageType.None; kind++)
                if (types.Contains((DamageType)kind) && _offensiveStrength[kind] < strength)
                    _offensiveStrength[kind] = (byte)strength;
        }
    }

    internal static int UnitStrikeDamageValue(Troop source, Troop target)
    {
        ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).MeleeRound.CalculateFreeUnitSwing(source, target);
        return ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).MeleeRound.GetAttackerDamageValue();
    }

    public enum Morale : byte
    {
        Terrible = 0, Poor = 1, Ok = 2, Good = 3, High = 4
    }

}
