#nullable disable
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Magic.Abilities;

// FRefCount. FUnitID. FGroupID. FGroup: TAIGroup. FUnitStatus
public partial class Troop : AbilityOwner, IOwned<TroopOwner>, IOwnedSink<TroopOwner>
{
    [SuppressMessage("Style", "IDE0290", Justification = "Hero.cs:746 and Hero.cs:759 read UnitId from CanActivate and Activate while an owner AddChild runs during the base constructor. Delphi sees 0, and FindId looks up that id. The constructor then stores -1.")]
    public Troop(CustomNode owner) : base(owner)
    {
        GroupId = 0;
        UnitId = -1;
    }

    // A troop stands in an army or a unit list, or a hero stands in a hero list or a hero library
    // while no army holds it. TroopOwner reads that owner without a cast.
    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is Army or UnitList or UnitUserList or IHeroHolder;

    TroopOwner IOwned<TroopOwner>.Owner => TroopOwner.Of(Owner);

    void IOwnedSink<TroopOwner>.SetOwner(TroopOwner owner) => SetOwner(owner.Node);

    // SetOwner observes this count before the base body, as in Unit.PAS:95.
    public int RefCount { get; private set; } = 1;

    public int UnitId { get; set; }

    public int GroupId { get; set; }

    public AIGroup Group { get; set; }

    // group
    public sbyte Player { get; set; }

    public Set<UnitStatusElement> UnitStatus { get; set; }

    public sbyte MoraleValue { get; set; }

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

        StrategicMap.Current?.NotifyUnitDestroyed(this);

        base.Dispose();
    }

    // Unit.PAS:518
    public override void SetOwner(CustomNode owner)
    {
        if (owner != Owner)
        {
            _ = AddRef();
            _ = (Owner?.RemoveChild(this));

            SetOwnerWithoutNotify(owner);
            if (Owner is not null)
            {
                if (Owner.AddChild(this) == -1)
                {
                    SetOwnerWithoutNotify(null);
                }
            }

            _ = Release();
        }
    }

    // Unit.PAS:1762
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        GroupId = persister.Int32("GroupId", value: GroupId);
        UnitId = persister.Int32("UnitId", value: UnitId, fallback: -1);
        MoraleValue = PersistSupport.UnsignedByte(persister, name: "MoraleValue", value: MoraleValue);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        if (persister.Loading)
        {
            try
            {
                EngineObject legacy = null;
                legacy = persister.Created("LegacyGroup", objectValue: legacy);
                LegacyGroupBehavior = legacy is not null ? (byte)((GroupControl)(object)legacy).Behavior() : (byte)0;
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

    // Unit.PAS:127
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // Unit.PAS:132
    public override int Release()
    {
        RefCount -= 1;
        int count = RefCount;
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
            _ = UpdateMoraleValue();
            UpdateOffensiveStrength();
            UpdateDefensiveStrength();
            UpdateEnchanted();
            StrategicMap.Current?.NotifyUnitChanged(this);
        }

        if (UnitHolder is { } holder)
        {
            holder.Changed();
        }
    }

    // Unit.PAS:791
    public virtual byte GetUpkeep() => !GetAbilityEnabled(155) ? (byte)((UnitLevel + 1) * 2) : (byte)0;

    // Unit.PAS:809
    public virtual sbyte UnitLevel => 0;

    // Unit.PAS:200
    public virtual sbyte GetRace() => -1;

    // Unit.PAS:866
    public virtual sbyte GetInherentAttack() => 0;

    // Unit.PAS:874
    public virtual sbyte GetInherentDefense() => 0;

    // Unit.PAS:882
    public virtual sbyte GetInherentDamage() => 0;

    // Unit.PAS:890
    public virtual sbyte InherentResistance => 0;

    // Unit.PAS:534
    public virtual bool GetInherentAbility(int abilityId) => GetAbilitySetAt(abilityId) && GamePack.Current.AbilityControl[abilityId].GetInherent(this);

    // Unit.PAS:543
    public virtual int GetInherentAbilityLevel(int abilityId) => GetAbilitySetAt(abilityId)
        ? GamePack.Current.AbilityControl[abilityId].GetInherentLevel(this)
        : 0;

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
    public virtual sbyte Moves => 0;

    // Unit.PAS:817
    public virtual sbyte GetMovePoints() => 0;

    // Unit.PAS:821
    public virtual void SetMovePoints(sbyte value)
    {
    }

    // Unit.PAS:823
    public virtual sbyte GetHitPoints() => 0;

    // Unit.PAS:827
    public virtual void SetHitPoints(sbyte value) => UpdateDefensiveStrength();

    // Unit.PAS:973
    public virtual Set<MoveType> GetMoveTypes() => GetAbilityMoveTypesAll();

    // Unit.PAS:852
    public virtual Set<DamageType> GetImmunityTypes() => GetAbilityImmunityTypesAll() + Set<DamageType>.Of(DamageType.Wall);

    // Unit.PAS:858
    public virtual Set<DamageType> GetProtectionTypes() => GetAbilityProtectionTypesAll();

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
    public virtual UnitGender Gender => UnitGender.Neutral;

    // Unit.PAS:119
    public virtual BloodType BloodType => BloodType.Red;

    // Unit.PAS:862
    public virtual UnitType GetUnitType() => UnitType.Humanoid;

    // Unit.PAS:989
    public virtual int UnitGraphicsResourceIndex => -1;

    // Unit.PAS:1480
    public virtual bool CanAddToList(UnitCollection list) => true;

    // Unit.PAS:179
    public virtual Set<WallCombatFeaturesElement> GetWallCombatFeatures()
    {
        Set<WallCombatFeaturesElement> result = Set<WallCombatFeaturesElement>.Of();
        if (GetAbilityEnabled(1) || GetAbilityEnabled(165) || GetAbilityEnabled(36) || GetAbilityEnabled(66))
        {
            result += Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);
        }

        if (GetAbilityEnabled(117) || GetAbilityEnabled(24) || GetAbilityEnabled(65) || GetAbilityEnabled(44))
        {
            result += Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);
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

    // Only heroes cast spells. The casting members below let spell code work on
    // any troop: a troop that casts nothing answers that it cannot cast.
    public virtual int GetCastingSpellId() => 0;

    public virtual bool CanCastSpell(int spellId, ref string reason) => false;

    public virtual bool CanCastSpellInstantly(int spellId, int mana, ref string reason) => false;

    public virtual bool ExecuteStartCasting(int spellId) => false;

    // The sphere doubling is a property of the spell and the global
    // enchantments, not of the caster, so every troop shares it.
    public virtual int CastingMana(Spell spell)
    {
        int result = spell.BaseManaCost;
        if (StrategicMap.Current.GlobalMagicControl.GetSphereManaDoubled(spell.MagicSphere))
        {
            result *= 2;
        }

        return result;
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
        if (player == Player)
        {
            SetMovePoints(Moves);
            if (GetHits() != GetHitPoints() && !GetAbilityEnabled(93))
            {
                sbyte heal;
                if (!GetAbilityEnabled(49))
                {
                    heal = (sbyte)Round(GetHits() * 0.1);
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
        if (player == Player && Player > 0)
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
    public virtual int GetAbilityLevel(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetLevel(this);

    // Unit.PAS:552
    public virtual bool GetAbilityEnabled(int abilityId) => GetAbilitySet(abilityId) && GamePack.Current.AbilityControl[abilityId].GetEnabled(this);

    // Unit.PAS:574
    public virtual string GetAbilityName(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetName(this);

    // Unit.PAS:582
    public virtual AbilityOwner GetAbilityOwner(int abilityId) => GetAbilitySetAt(abilityId) ? this : (AbilityOwner)null;

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
    public virtual DelphiStringCollection Description => null;

    // Unit.PAS:494
    public virtual void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            _ = AddRef();
            ResetLifeMasteryBlessing();
            ResetLifeMasteryFear();
            ResetEarthMasteryStoneSkin();
            ResetFireMasteryProtection();
            AIGroup list = Group;
            _ = (list?.DeleteUnit(this));

            Player = player;
            _ = Release();
        }
    }

    // Unit.PAS:513
    public virtual sbyte GetIndependentRelation() => GetRace() >= 0 ? GetRace() : AlignmentTables.AlignmentToIndependentRelation(GetAlignment());

    // Unit.PAS:350
    public virtual int GetObtainValue() => 0;

    // Unit.PAS:346
    public virtual UnitSize UnitSize => UnitSize.Medium;

    // Unit.PAS:260
    public virtual int GetUnitMoraleValue(sbyte player, sbyte independentRelation)
    {
        int morale;
        if (player >= 0 && GetUnitType() != UnitType.Machine && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            sbyte race = GetRace();
            morale = GetRelationValue(player, independentRelation);
            if (race != -1)
            {
                MapCoordinate coordinate45 = GetLocation();
                sbyte hexX = coordinate45.X;
                sbyte hexY = coordinate45.Y;
                sbyte hexLevel = coordinate45.MapLevel;
                if (hexX != -1)
                {
                    byte rawTerrain = (byte)StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel).TerrainType;
                    int raceIndex = race;
                    TerrainType terrainType = (TerrainType)rawTerrain;
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
        UnitStatus += Set<UnitStatusElement>.Of(UnitStatusElement.Activated);
        StrategicMap.Current.UnitControl.RegisterUnit(this);
        if (GroupId != 0 && Player >= 0)
        {
            AIGroup group = StrategicMap.Current.AIGroupManager.Groups.FindId(GroupId);
            if (group is not null)
            {
                _ = group.AddUnit(this);
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
        AIGroup group = Group;
        _ = (group?.DeleteUnit(this));

        StrategicMap.Current.UnitControl.UnregisterUnit(this);
        UnitStatus -= Set<UnitStatusElement>.Of(UnitStatusElement.Activated);
        UnregisterEnchantments();
    }

    // Unit.PAS:1100
    public virtual void MovedTo(MapField oldField, MapField newField, int maxCost)
    {
        int cost = MovePointCost(newField.TerrainType, newField.OverlayType);
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
            _ = StrategicMap.Current.ChangeTerrain(null, newField.HexX, newField.HexY, newField.HexLevel, 0, (int)TerrainType.Dirt);
        }

        if (oldField is not null)
        {
            EngineObject army = Owner;
            bool transported = army is Army armyValue && armyValue.Transporter(255) is not null;
            if (!transported)
            {
                if (oldField.HexLevel == 0)
                {
                    if (GetAbilityEnabled(69))
                    {
                        _ = StrategicMap.Current.ChangeTerrain(null, oldField.HexX, oldField.HexY, oldField.HexLevel, 1, PathOfLifeTerrainChange, null);
                    }
                    else
                    {
                        if (GetAbilityEnabled(68))
                        {
                            _ = StrategicMap.Current.ChangeTerrain(null, oldField.HexX, oldField.HexY, oldField.HexLevel, 1, PathOfDecayTerrainChange, null);
                        }
                    }
                }

                if (GetAbilityEnabled(109))
                {
                    _ = StrategicMap.Current.ChangeTerrain(null, oldField.HexX, oldField.HexY, oldField.HexLevel, 1, PathOfFrostTerrainChange, PathOfFrostTerrainChanged);
                }
            }

            if (GetAbilityEnabled(26))
            {
                ExecuteTrailOfDarkness(oldField);
            }
        }
    }

    // Unit.PAS:1890
    public virtual bool CanDisband(string reason)
    {
        bool result = StrategicMap.Current.SeatedPlayer == Player;
        return result && GetTransporter() && Owner is Army ? ArmyOwner.UnitsInTransporter() == 0 : result;
    }

    // Unit.PAS:1796
    public virtual bool CanJoin(sbyte player)
    {
        bool condition = Group is null;
        if (!condition)
        {
            GroupBehavior behavior = Group.GroupClass.Behavior;
            condition = !Set<GroupBehavior>.Of(GroupBehavior.Guard, GroupBehavior.GuardArea).Contains(behavior);
        }

        bool result;
        if (condition)
        {
            if (GetRace() == -1)
            {
                Player players = StrategicMap.Current.Players.GetPlayers(player);
                Alignment alignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).Alignment;
                if (GetAlignment() != Alignment.None)
                {
                    DiplomaticRelation relation = AlignmentTables.AlignmentToDiplomaticRelation(GetAlignment(), alignment);
                    if (relation == DiplomaticRelation.Unknown && this is Unit unit)
                    {
                        result = false;
                        int i = StrategicMap.Current.Races.Count;
                        while (!result && i != 0)
                        {
                            i -= 1;
                            bool test = StrategicMap.Current.Races.GetRace(i).GetPlayerRelation(player) >= Status.Polite;
                            if (test)
                            {
                                int unitResourceIndex = unit.UnitResource.UnitResourceIndex();
                                test = GamePack.Current.RaceResources.GetRaceResource(i).UnitIndexes.IndexOf(unitResourceIndex) != -1;
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
                sbyte race = GetRace();
                result = StrategicMap.Current.Races.GetRace(race).GetPlayerRelation(player) >= Status.Polite;
            }

            if (result && Group is not null && Group.HuntingPlayer(player))
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Unit.PAS:1835
    public virtual int JoinAmount(sbyte player)
    {
        AIGroup group = Group;
        return group is not null && group.GroupClass.Behavior == GroupBehavior.Refuge ? 0 : Round(GetObtainValue() * 0.75);
    }

    // Unit.PAS:1843
    public virtual bool OfferToJoin(sbyte player)
    {
        if (CanJoin(player))
        {
            if (GetRace() != -1)
            {
                sbyte race = GetRace();
                return StrategicMap.Current.Races.GetRace(race).GetPlayerRelation(player) >= Status.Polite;
            }
            else
            {
                AIGroup group = Group;
                if (group is not null && group.GroupClass.Behavior == GroupBehavior.Refuge)
                {
                    Player players = StrategicMap.Current.Players.GetPlayers(player);
                    Alignment alignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).Alignment;
                    return (GetAlignment() == Alignment.None && GetUnitType() != UnitType.Machine) || AlignmentTables.AlignmentToDiplomaticRelation(GetAlignment(), alignment) == DiplomaticRelation.Peace;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    // Unit.PAS:1872
    public virtual void GetJoinMessage(sbyte player, int count, int amount, JoinMode mode, DelphiStringCollection messages)
    {
        messages.Clear();
        switch (mode)
        {
            case JoinMode.Default:
                AIGroup group = Group;
                messages.Text = group is not null && group.GroupClass.Behavior == GroupBehavior.Refuge && amount == 0
                    ? count == 0
                        ? AbilityRules.TranslateResourceString(GetEngine(), GameText.RefugeJoinText)
                        : AbilityRules.TranslateResourceString(GetEngine(), GameText.RefugeesJoinText)
                    : count == 0
                        ? AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.BribeUnitJoinFormat), [IntToStr(amount)])
                        : AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.BribeUnitsJoinFormat), [IntToStr(amount)]);

                break;
            default:
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
        UnitStatus += Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        ResetLifeMasteryBlessing();
        ResetLifeMasteryFear();
        ResetEarthMasteryStoneSkin();
        ResetFireMasteryProtection();
        UnregisterEnchantments();
        AIGroup group = Group;
        _ = (group?.DeleteUnit(this));

        SetPlayer(-1);
        Changed();
    }

    // Unit.PAS:1224
    public virtual void Resurrect()
    {
        _ = ExpandAbility(128);
        UnitStatus -= Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        SetHitPoints(1);
        SetPlayer(0);
    }

    // Unit.PAS:1231
    public virtual void Animate()
    {
        _ = ExpandAbility(129);
        UnitStatus -= Set<UnitStatusElement>.Of(UnitStatusElement.Killed);
        SetHitPoints(1);
        SetPlayer(0);
    }

    // Unit.PAS:152
    // { A stack-argument assembler routine gets an EBP frame; remove it before jumping. }
    public void RetrieveDamageInfo(sbyte attack, sbyte damage, Set<DamageType> damageTypes, out int damageValue, out int obtainValue)
    {
        int maxDamage = GetHitPoints() * 100;
        damageValue = MeleeRound.StatisticsToDamageValue(attack, damage, damageTypes, GetDefense(), GetResistance(), GetImmunityTypes(), GetProtectionTypes());
        if (maxDamage < damageValue)
        {
            damageValue = maxDamage;
        }

        obtainValue = damageValue > 0 && GetHitPoints() > 0
            ? maxDamage > damageValue ? Round(GetObtainValue() * ((double)(damageValue * 0.75) / maxDamage)) : GetObtainValue()
            : 0;
    }

    // The army this troop stands in, or null for every other owner: a hero list, a hero
    // library, a unit list, a unit user list, or no owner. The troop admits all of those owners,
    // so the read cannot cast. Callers treat a missing army as absent.
    internal Army ArmyOwner => Owner as Army;

    // The army or unit list that holds this troop, or null for a hero list or no owner.
    internal IUnitHolder UnitHolder => Owner as IUnitHolder;

    // The troops of the army or unit list that holds this troop. Without one it is the default
    // collection, which throws on use as the missing list did.
    internal UnitCollection OwnerUnits => UnitHolder?.Units ?? default;

    // Unit.PAS:189
    public MapCoordinate GetLocation() => UnitHolder is { } holder ? holder.GetLocation() : MapCoordinate.None;

    // Unit.PAS:231
    public int GetRelationValue(sbyte player, sbyte independentRelation)
    {
        sbyte race = GetRace();
        if (player != 0)
        {
            if (race != -1 && !GetAbilityEnabled(107))
            {
                return StrategicMap.Current.Races.GetRace(race).GetPlayerRelationValue(player);
            }
            else
            {
                Player players = StrategicMap.Current.Players.GetPlayers(player);
                Alignment playerAlignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).Alignment;
                Alignment unitAlignment = GetAlignment();
                return AlignmentTables.AlignmentToStatusValue[(int)unitAlignment][(int)playerAlignment];
            }
        }
        else
        {
            Alignment iRtoAlignment = AbilityRules.IndependentRelationToAlignment(independentRelation);
            Alignment alignment = GetAlignment();
            return AlignmentTables.AlignmentToStatusValue[(int)alignment][(int)iRtoAlignment];
        }
    }

    // Unit.PAS:256
    public Status GetRelation(sbyte player, sbyte independentRelation) => Race.StatusValueToStatus(GetRelationValue(player, independentRelation));

    // Unit.PAS:301
    public Status UnitRelation(Troop troop)
    {
        sbyte race = GetRace();
        sbyte otherRace = troop.GetRace();
        return race != -1 && otherRace != -1
            ? StrategicMap.Current.Races.GetRace(race).GetRelation(otherRace)
            : AlignmentTables.AlignmentToStatus(otherAlignment: troop.GetAlignment(), alignment: GetAlignment());
    }

    // Unit.PAS:312
    public sbyte UpdateMoraleValue()
    {
        int player = Player;
        int value = GetUnitMoraleValue((sbyte)player, GetIndependentRelation());
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && GetUnitType() != UnitType.Machine && this is not Leader && Owner is Army)
        {
            if (GetUpkeep() != 0 && player > 0 && !GetAbilityEnabled(155))
            {
                value += StrategicMap.Current.Players.GetPlayers(player).MoraleValue;
            }

            for (int i = Owner.Count - 1; i >= 0; i--)
            {
                Troop troop = OwnerUnits[i];
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

        MoraleValue = value > 125 ? (sbyte)125 : value < -25 ? (sbyte)-25 : (sbyte)value;

        return MoraleValue;
    }

    // Unit.PAS:342
    public Morale GetMorale() => AbilityRules.MoraleValueToMorale(MoraleValue);

    // Unit.PAS:358
    public void UpdateEnchanted()
    {
        AbilityTypeList list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        bool enchanted = false;
        int i = list.Count;
        while (!enchanted && i != 0)
        {
            i -= 1;
            enchanted = GetAbilityEnabled(list.GetAbility(i).AbilityId);
        }

        UnitStatus = enchanted
            ? UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Enchanted)
            : UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Enchanted);
    }

    // Unit.PAS:375
    public void RemoveEnchantments()
    {
        _ = AddRef();
        AbilityTypeList list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Ability ability = list.GetAbility(i);
            if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
            {
                _ = RemoveAbility(ability.AbilityId);
            }
        }

        _ = Release();
    }

    // Unit.PAS:391
    public void RemoveHandicaps()
    {
        _ = AddRef();
        _ = RemoveAbility(93);
        _ = RemoveAbility(98);
        _ = RemoveAbility(53);
        _ = Release();
    }

    // Unit.PAS:403
    public void RegisterEnchantments()
    {
        AbilityTypeList list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Ability ability = list.GetAbility(i);
            if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
            {
                UnitEnchantmentAbilityData data = GetAbilityData<UnitEnchantmentAbilityData>(ability.AbilityId);
                if (data.Enchantment.Source > -1)
                {
                    _ = StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).MagicControl.RegisterEnchantment(data.Enchantment);
                }
            }
        }
    }

    // Unit.PAS:424
    public void UnregisterEnchantments()
    {
        AbilityTypeList list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Ability ability = list.GetAbility(i);
            if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
            {
                UnitEnchantmentAbilityData data = GetAbilityData<UnitEnchantmentAbilityData>(ability.AbilityId);
                if (data.Enchantment.Source > -1)
                {
                    _ = StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).MagicControl.UnregisterEnchantment(data.Enchantment);
                }
            }
        }
    }

    // Unit.PAS:449
    public UnitEnchantment AbilityToEnchantment(int abilityId)
    {
        Ability ability = GamePack.Current.AbilityControl[abilityId];
        if (ability is UnitEnchantmentAbility && GetAbilityEnabled(ability.AbilityId))
        {
            UnitEnchantmentAbilityData data = GetAbilityData<UnitEnchantmentAbilityData>(ability.AbilityId);
            return data?.Enchantment;
        }
        else
        {
            return null;
        }
    }

    // Unit.PAS:463
    public bool CanDispelEnchantment(sbyte player)
    {
        AbilityTypeList list = GamePack.Current.AbilityControl.AbilitiesByType[4];
        int i = list.Count;
        bool result = false;
        while (!result && i != 0)
        {
            i -= 1;
            UnitEnchantment enchantment = AbilityToEnchantment(list.GetAbility(i).AbilityId);
            result = enchantment is not null && player != enchantment.Source;
        }

        return result;
    }

    // Unit.PAS:481
    public void ListEnchantments(CustomNode list)
    {
        AbilityTypeList abilities = GamePack.Current.AbilityControl.AbilitiesByType[4];
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            UnitEnchantment enchantment = AbilityToEnchantment(abilities.GetAbility(i).AbilityId);
            if (enchantment is not null)
            {
                _ = list.AddChild(enchantment);
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
        return result != 0 ? (result + 10) * _defensiveStrength : result;
    }

    // Unit.PAS:664
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        if (!UnitStatus.Contains(UnitStatusElement.Activated))
        {
            UpdateDefensiveStrength();
            UpdateOffensiveStrength();
        }

        int result;
        if (damageTypes != TroopConstants.AllDamageTypes)
        {
            result = 0;
            for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
            {
                DamageType damageType = (DamageType)damageTypeOrdinal;
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
                return (result + 10) * _defensiveStrength;
            }
        }
        else
        {
            return MaxOffensiveStrength != 0 ? (MaxOffensiveStrength + 10) * _defensiveStrength : 0;
        }

        return result;
    }

    // Unit.PAS:688
    public int GetCombatValue(Troop target, byte mode)
    {
        AbilityDamageValues info = default;
        int result = 0;
        AbilityTypeList abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            Ability ability = abilities.GetAbility(i);
            if (GetAbilityEnabled(ability.AbilityId))
            {
                ability.GetDamageValues(this, target, (WallSituation)mode, ref info);
                if (info.SourceDamage > 0)
                {
                    int value = info.SourceDamage * info.SourceDamage / (info.TargetDamage + info.SourceDamage);
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
        if (GetAbilityEnabled(155))
        {
            Enchantment enchantment = GetAbilityData<UnitEnchantmentAbilityData>(155).Enchantment;
            return enchantment is not null ? enchantment.Upkeep : 0;
        }
        else
        {
            return 0;
        }
    }

    // Unit.PAS:831
    public bool GetTransporting()
    {
        MapCoordinate coordinate46 = GetLocation();
        sbyte hexX = coordinate46.X;
        sbyte hexY = coordinate46.Y;
        sbyte hexLevel = coordinate46.MapLevel;
        if (hexX != -1)
        {
            MapField mapField = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
            return !ValidTerrain(mapField.TerrainType, mapField.OverlayType);
        }
        else
        {
            return false;
        }
    }

    // Unit.PAS:922
    public void CreateMovePointTable(out MovePointTable table)
    {
        Set<MoveType> moves = GetMoveTypes();
        if (StrategicMap.Current.Combat is not null)
        {
            moves -= Set<MoveType>.Of(MoveType.Tunneling);
        }

        int moveTypesToInt32 = MovementTables.MoveTypesToInt32(moves);
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
                bool faster = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery);
                bool slower = (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 4) != 0;
                if (faster && !slower)
                {
                    for (int overlay = -1; overlay <= 11; overlay++)
                    {
                        for (int terrain = 0; terrain <= 15; terrain++)
                        {
                            sbyte value = table[terrain][overlay - MovementTables.FirstOverlay];
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

            int points;
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
        // { Original retrieves the movement set twice and retains the first dead store. }
        _ = GetMoveTypes();
        int moveTypesToInt32 = MovementTables.MoveTypesToInt32(GetMoveTypes());
        sbyte cost = MovementTables.MovePointTables[moveTypesToInt32][terrainType][overlayType - MovementTables.FirstOverlay];
        return cost != -1 && cost <= Moves;
    }

    // Unit.PAS:1011
    public void ExecuteTrailOfDarkness(MapField mapField)
    {
        MapLevel level = mapField.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, mapField.HexX, mapField.HexY, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.X < level.Width && walk.Y >= 0 && walk.Y < level.Height)
            {
                MapField field = level.GetField(walk.X, walk.Y);
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
    public void PathOfLifeTerrainChange(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        if (terrainType is not (int)TerrainType.Ice and not (int)TerrainType.Water && mapField.HexLevel == 0)
        {
            terrainType = (int)TerrainType.Grass;
        }
    }

    // Unit.PAS:1043
    public void PathOfDecayTerrainChange(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        if (terrainType is not (int)TerrainType.Ice and not (int)TerrainType.Water && mapField.HexLevel == 0)
        {
            terrainType = (int)TerrainType.Wasteland;
        }
    }

    // Unit.PAS:1048
    public void PathOfFrostTerrainChange(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        if (terrainType is ((int)TerrainType.Water) or ((int)TerrainType.UndergroundWater))
        {
            terrainType = terrainType == (int)TerrainType.Water ? (sbyte)(int)TerrainType.Ice : (sbyte)(int)TerrainType.UndergroundIce;
        }
        else
        {
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)terrainType))
            {
                FrozenWaterHexSprite ice = mapField.FindOwnedHexSprite<FrozenWaterHexSprite>();
                if (ice is not null)
                {
                    ice.DaysRemaining = 2;
                    ice.CasterPlayer = Player;
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(1).With(2).With(4).With(5).Contains((byte)terrainType) && mapField.HexLevel == 0)
                {
                    terrainType = (int)TerrainType.Snow;
                }
            }
        }
    }

    // Unit.PAS:1066
    public void PathOfFrostTerrainChanged(MapField mapField, sbyte originalTerrainType, sbyte originalOverlayType)
    {
        if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)mapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)originalTerrainType))
        {
            FrozenWaterHexSprite ice = mapField.FindOwnedHexSprite<FrozenWaterHexSprite>();
            if (ice is null)
            {
                ice = new FrozenWaterHexSprite(null) { Holder = CreationHolder() };
                _ = StrategicMap.Current.PlaceHex(ice, mapField.HexX, mapField.HexY, mapField.HexLevel);
            }

            ice.DaysRemaining = 2;
            ice.CasterPlayer = Player;
        }
    }

    // Unit.PAS:1082
    public void UpdatePathOfFrost()
    {
        MapCoordinate coordinate47 = GetLocation();
        sbyte hexX = coordinate47.X;
        sbyte hexY = coordinate47.Y;
        sbyte hexLevel = coordinate47.MapLevel;
        if (hexX > -1)
        {
            MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)field.TerrainType))
            {
                FrozenWaterHexSprite ice = field.FindOwnedHexSprite<FrozenWaterHexSprite>();
                if (ice is not null)
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
                        return GetAbilityEnabled(140);
                    case 3:
                        return GetAbilityEnabled(141);
                    case 2:
                        return GetAbilityEnabled(142);
                    case 5:
                        return GetAbilityEnabled(143);
                    case 0:
                    case 10:
                        return GetAbilityEnabled(144);
                    case 4:
                        return GetAbilityEnabled(138);
                    default:
                        break;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }

        return result;
    }

    // Unit.PAS:1154
    public int ExecuteStormDamage(StormType storm, int distance)
    {
        Set<MoveType> moves = GetMoveTypes();
        bool doubleDamage = GetAbilityEnabled(23);
        if (!doubleDamage && moves.Contains(MoveType.Flying))
        {
            MapCoordinate coordinate48 = GetLocation();
            sbyte hexX = coordinate48.X;
            sbyte hexY = coordinate48.Y;
            sbyte hexLevel = coordinate48.MapLevel;
            if (hexX != -1)
            {
                MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
                sbyte terrain = field.TerrainType;
                sbyte overlay = field.OverlayType;
                moves -= Set<MoveType>.Of(MoveType.Flying);
                int moveTypesToInt32 = MovementTables.MoveTypesToInt32(moves);
                doubleDamage = MovementTables.MovePointTables[moveTypesToInt32][terrain][overlay - MovementTables.FirstOverlay] == 0;
            }
        }

        int result;
        int attack;
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
            case StormType.None:
            default:
                attack = 0;
                result = 0;
                break;
        }

        if (doubleDamage)
        {
            result *= 2;
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
                    _ = ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Death));
                    break;
                case StormType.Pestilence:
                    _ = ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Poison));
                    break;
                case StormType.None:
                case StormType.Fire:
                case StormType.Holy:
                case StormType.Ice:
                case StormType.Lightning:
                default:
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
        CreateMovePointTable(out MovePointTable table);
        return table[terrainType][overlayType - MovementTables.FirstOverlay];
    }

    // Unit.PAS:1238
    public void ResetEarthMasteryStoneSkin()
    {
        if (GetAbilityEnabled(153))
        {
            if (GetAbilityData<UnitEnchantmentAbilityData>(153).Enchantment.Source == -2)
            {
                _ = RemoveAbility(153);
            }
        }
    }

    // Unit.PAS:1246
    public void SetEarthMasteryStoneSkin()
    {
        if (GetUnitType() != UnitType.Machine && !GetAbilityEnabled(153))
        {
            if (ExpandAbility(153))
            {
                GetAbilityData<UnitEnchantmentAbilityData>(153).Enchantment.SetUpkeep(0);
                GetAbilityData<UnitEnchantmentAbilityData>(153).Enchantment.SetSource(-2);
                Spell spell = GamePack.Current.SpellControl[17];
                GetAbilityData<UnitEnchantmentAbilityData>(153).Enchantment.DispelResistance = (sbyte)((UnitSpell)(object)spell).UnitDispelResistance;
            }
        }
    }

    // Unit.PAS:1259
    public void ResetFireMasteryProtection()
    {
        if (GetAbilityEnabled(167))
        {
            if (GetAbilityData<UnitEnchantmentAbilityData>(167).Enchantment.Source == -2)
            {
                _ = RemoveAbility(167);
            }
        }
    }

    // Unit.PAS:1265
    public void SetFireMasteryProtection()
    {
        if (!GetAbilityEnabled(71) && !GetAbilityEnabled(167))
        {
            if (ExpandAbility(167))
            {
                GetAbilityData<UnitEnchantmentAbilityData>(167).Enchantment.SetUpkeep(0);
                GetAbilityData<UnitEnchantmentAbilityData>(167).Enchantment.SetSource(-2);
                GetAbilityData<UnitEnchantmentAbilityData>(167).Enchantment.DispelResistance = 7;
            }
        }
    }

    // Unit.PAS:1278
    public void ResetLifeMasteryBlessing()
    {
        if (GetAbilityEnabled(158))
        {
            if (GetAbilityData<UnitEnchantmentAbilityData>(158).Enchantment.Source == -2)
            {
                _ = RemoveAbility(158);
            }
        }
    }

    // Unit.PAS:1284
    public void SetLifeMasteryBlessing()
    {
        if (!GetAbilityEnabled(158))
        {
            if (ExpandAbility(158))
            {
                GetAbilityData<UnitEnchantmentAbilityData>(158).Enchantment.SetUpkeep(0);
                GetAbilityData<UnitEnchantmentAbilityData>(158).Enchantment.SetSource(-2);
                Spell spell = GamePack.Current.SpellControl[56];
                GetAbilityData<UnitEnchantmentAbilityData>(158).Enchantment.DispelResistance = (sbyte)((UnitSpell)(object)spell).UnitDispelResistance;
            }
        }
    }

    // Unit.PAS:1305
    public void ExecuteLifeMasteryFearRole()
    {
        bool condition = !GetAbilityEnabled(53);
        if (condition)
        {
            Alignment alignment = GetAlignment();
            condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
        }

        if (condition && !GetAbilityEnabled(67) && !GetAbilityEnabled(46) && HitChance.HitRole(4 - GetResistance()))
        {
            if (ExpandAbility(53))
            {
                DurationAbilityData data = GetAbilityData<DurationAbilityData>(53);
                data.Source = 1;
                data.Duration = 0;
            }
        }
    }

    // Unit.PAS:1319
    public void ValidateLifeMastery()
    {
        sbyte player = Player;
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
        bool condition = !GetAbilityEnabled(93);
        if (condition)
        {
            Alignment alignment = GetAlignment();
            condition = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
        }

        if (condition && HitChance.HitRole(6 - GetResistance()))
        {
            if (ExpandAbility(93))
            {
                DurationAbilityData data = GetAbilityData<DurationAbilityData>(93);
                data.Source = 1;
            }
        }
    }

    // Unit.PAS:1344
    public void ValidateDeathMastery()
    {
        sbyte player = Player;
        if (player > 0)
        {
            if ((((byte)StrategicMap.Current.Players.GetPlayers(player).MagicControl.EnemyEnchantments.Image) & 32) == 0)
            {
                if (GetAbilityEnabled(93))
                {
                    if (GetAbilityData<DurationAbilityData>(93).Source == 1)
                    {
                        _ = RemoveAbility(93);
                    }
                }
            }
        }
    }

    // Unit.PAS:1391
    public int VisibilityRange()
    {
        int level = GetAbilityLevel(64);
        int result = level != 0 ? (level * 2) + 4 : 4;

        MapCoordinate coordinate49 = GetLocation();
        _ = coordinate49.X;
        _ = coordinate49.Y;
        sbyte hexLevel = coordinate49.MapLevel;
        return (hexLevel != 0 && !GetAbilityEnabled(39)) || (hexLevel == 0 && Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 8) != 0)
            ? (int)(((uint)(result + 1)) >> 1)
            : result;
    }

    // Unit.PAS:1404
    public int TrueVisionRange() => GetAbilityEnabled(41) ? VisibilityRange() : 1;

    // Unit.PAS:1428
    public bool CanPlace(int X, int Y, int L, sbyte player, Set<UnitPlaceModeElement> mode)
    {
        if (X >= 0 && Y >= 0 && X < StrategicMap.Current.MapContainer.Width && Y < StrategicMap.Current.MapContainer.Height)
        {
            MapField field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
            sbyte fieldPlayer = field.GetPlayer();
            if (mode.Contains(UnitPlaceModeElement.NeglectEmptyPlayerLocation) || fieldPlayer == player || fieldPlayer == -1 || player == -1)
            {
                ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
                return armyHexSprite is not null
                    ? !mode.Contains(UnitPlaceModeElement.CreateNewArmy) && (player == -1 || armyHexSprite.Army.Player == player) && armyHexSprite.Army.CanAddUnit(this)
                    : ValidTerrain(field.TerrainType, field.OverlayType);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Unit.PAS:1454
    public ArmyHexSprite Place(int X, int Y, int L, sbyte player, Set<UnitPlaceModeElement> mode)
    {
        ArmyHexSprite result = null;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, 0);
        while (result is null && (walk.Radius == 0 || mode.Contains(UnitPlaceModeElement.ScanArea)) && walk.Radius < 30)
        {
            if (CanPlace(walk.X, walk.Y, L, player, mode))
            {
                ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(walk.X, walk.Y, L).GetArmyHexSprite();
                if (armyHexSprite is null)
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
                    result = armyHexSprite.Army.AddUnit(this) ? armyHexSprite : null;
                }
            }

            if (result is null)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // Unit.PAS:1486
    public void ValidateCrusader()
    {
        DurationAbilityData data = GetAbilityData<DurationAbilityData>(101);
        if (data is not null)
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
        _ = Show(X, Y, lockedSurface, info);
    }

    // Unit.PAS:1610
    public void ShowFace(in Rect R, in LockedSurface lockedSurface)
    {
        LibraryImage image = GetFace();
        image?.ShowStretched(R, lockedSurface);

        if (GetAbilityEnabled(129))
        {
            if (StrategicMap.Current is not null)
            {
                GamePack.Current.GeneralImages[45]?.ShowStretched(R, lockedSurface);
            }
        }
    }

    // Unit.PAS:1623
    public int ExecuteDamageRole(int attack, int damage, Set<DamageType> damageTypes)
    {
        int result = 0;
        Set<DamageType> active = damageTypes - GetImmunityTypes();
        if (active != Set<DamageType>.Of())
        {
            int displayController = attack - GetDefense();
            result = AbilityRules.ExecuteDamageRole(damage, displayController);
            if (active - GetProtectionTypes() == Set<DamageType>.Of())
            {
                return (result + 1) / 2;
            }
        }

        return result;
    }

    // Unit.PAS:1635
    public bool ExecuteResistanceRole(int attack, DamageType damageType)
    {
        if (!GetImmunityTypes().Contains(damageType))
        {
            if (GetProtectionTypes().Contains(damageType))
            {
                attack -= 2;
            }

            return HitChance.HitRole(attack - GetResistance());
        }
        else
        {
            return false;
        }
    }

    // Unit.PAS:1644
    public Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> damageTypes)
    {
        Set<DamageType> result = Set<DamageType>.Of();
        Set<DamageType> active = damageTypes - GetImmunityTypes();
        if (active != Set<DamageType>.Of())
        {
            int resistance = GetResistance();
            Set<DamageType> protection = GetProtectionTypes();
            int effectiveResistance;
            if (active.Contains(DamageType.Fire) && !GetAbilityEnabled(7))
            {
                effectiveResistance = protection.Contains(DamageType.Fire) ? resistance + 2 : GetResistance();

                if (HitChance.HitRole(4 - effectiveResistance))
                {
                    result += Set<DamageType>.Of(DamageType.Fire);
                }
            }

            if (active.Contains(DamageType.Lightning) && GetUnitType() != UnitType.Machine && !GetAbilityEnabled(9))
            {
                effectiveResistance = protection.Contains(DamageType.Lightning) ? resistance + 2 : GetResistance();

                if (HitChance.HitRole(6 - effectiveResistance))
                {
                    result += Set<DamageType>.Of(DamageType.Lightning);
                }
            }

            if (active.Contains(DamageType.Poison) && !GetAbilityEnabled(10))
            {
                effectiveResistance = protection.Contains(DamageType.Poison) ? resistance + 2 : GetResistance();

                if (HitChance.HitRole(3 - effectiveResistance))
                {
                    result += Set<DamageType>.Of(DamageType.Poison);
                }
            }

            if (active.Contains(DamageType.Death) && !GetAbilityEnabled(11))
            {
                effectiveResistance = protection.Contains(DamageType.Death) ? resistance + 2 : GetResistance();

                if (HitChance.HitRole(4 - effectiveResistance))
                {
                    result += Set<DamageType>.Of(DamageType.Death);
                }
            }

            if (active.Contains(DamageType.Holy) && !GetAbilityEnabled(12))
            {
                effectiveResistance = protection.Contains(DamageType.Holy) ? resistance + 2 : GetResistance();

                if (HitChance.HitRole(3 - effectiveResistance) && GetUnitType() != UnitType.Machine)
                {
                    result += Set<DamageType>.Of(DamageType.Holy);
                }
            }

            if (active.Contains(DamageType.Cold) && !GetAbilityEnabled(8))
            {
                effectiveResistance = protection.Contains(DamageType.Cold) ? resistance + 2 : GetResistance();

                if (HitChance.HitRole(2 - effectiveResistance))
                {
                    result += Set<DamageType>.Of(DamageType.Cold);
                }
            }
        }

        return result;
    }

    // Unit.PAS:1699
    public Set<DamageType> ExecuteDamageEffects(Set<DamageType> damageTypes)
    {
        Set<DamageType> result = Set<DamageType>.Of();
        Set<DamageType> active = damageTypes - GetImmunityTypes();
        // { Preserve EBX allocation and byte membership tests. }
        if (active != Set<DamageType>.Of())
        {
            if (active.Contains(DamageType.Poison) && ExpandAbility(96))
            {
                result += Set<DamageType>.Of(DamageType.Poison);
            }

            if (active.Contains(DamageType.Death) && ExpandAbility(93))
            {
                result += Set<DamageType>.Of(DamageType.Death);
            }

            if (active.Contains(DamageType.Holy) && ExpandAbility(98))
            {
                result += Set<DamageType>.Of(DamageType.Holy);
            }
        }

        return result;
    }

    // Unit.PAS:1747
    public DamageType OptimalDamageType(Set<DamageType> damageTypes)
    {
        DamageType result = DamageType.None;
        Set<DamageType> active = damageTypes - GetImmunityTypes();
        if (active != Set<DamageType>.Of())
        {
            Set<DamageType> protection = GetProtectionTypes();
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
        string reason = "";
        if (CanDisband(reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            DisbandUnitTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                UnitId = UnitId,
                Player = Player
            };
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // The producer ExecuteLifeMasteryFearRole writes the source marker on duration data.
    public void ResetLifeMasteryFear()
    {
        if (GetAbilityEnabled(53) && GetAbilityData<DurationAbilityData>(53).Source == 1)
        {
            _ = RemoveAbility(53);
        }
    }

    public virtual bool GetAbilitySet(int index) => (uint)index < (uint)AbilityCount && (AbilityBits[index >> 3] & (1 << (index & 7))) != 0;

    public override bool RemoveAbility(int ability)
    {
        _ = AddRef();
        bool result = base.RemoveAbility(ability);
        _ = Release();
        return result;
    }

    // Unit.PAS:713. TP2AbilityCalls is a layout view, not an instance type.
    public void ListAbilityInfo(DelphiStringCollection names, DelphiStringCollection types, DelphiStringCollection sources)
    {
        names.Clear();
        sources?.Clear();
        for (int i = 0; i < GetAbilityCount(); i++)
        {
            if (!GetAbilitySet(i))
            {
                continue;
            }

            AbilityOwner owner = GetAbilityOwner(i);
            Ability ability = GamePack.Current.AbilityControl[i];
            string name = ability.GetName(owner);
            if (!IsEmpty(name))
            {
                _ = names.AddObject(name, IntToObject(i));
            }

            _ = (types?.AddObject(AbilityRules.TranslateResourceString(GetEngine(), GameTables.AbilityTypeTexts[(int)ability.GetAbilityType()]), IntToObject(i)));
            string source = ability.GetSourceName(owner);
            _ = (sources?.AddObject(source, IntToObject(i)));
        }
    }

    // Unit.PAS:1715. Native hard casts retain identity across the intermediate local type.
    public Set<DamageType> ExecuteCombatDamageEffects(Battle combat, Set<DamageType> damageTypes)
    {
        Set<DamageType> result = default;
        Set<DamageType> active = damageTypes - GetImmunityTypes();
        if (active == default)
        {
            return result;
        }

        if (active.Contains(DamageType.Fire) && ExpandAbility(127))
        {
            _ = RemoveAbility(95);
            result += Set<DamageType>.Of(DamageType.Fire);
        }
        if (active.Contains(DamageType.Cold) && ExpandAbility(95))
        {
            UnitEnchantmentAbilityData data = GetAbilityData<UnitEnchantmentAbilityData>(95);
            data?.EnchantmentPlayerIndex = (sbyte)combat.CombatData.CurrentPlayer;

            _ = RemoveAbility(127);
            result += Set<DamageType>.Of(DamageType.Cold);
        }
        if (active.Contains(DamageType.Lightning) && ExpandAbility(92))
        {
            DurationAbilityData data = GetAbilityData<DurationAbilityData>(92);
            data?.PlayerIndex = combat.CombatData.CurrentPlayer;

            result += Set<DamageType>.Of(DamageType.Lightning);
        }
        if (active.Contains(DamageType.Poison) && ExpandAbility(96))
        {
            result += Set<DamageType>.Of(DamageType.Poison);
        }

        if (active.Contains(DamageType.Death) && ExpandAbility(93))
        {
            result += Set<DamageType>.Of(DamageType.Death);
        }

        if (active.Contains(DamageType.Holy) && ExpandAbility(98))
        {
            result += Set<DamageType>.Of(DamageType.Holy);
        }

        return result;
    }

    public void UpdateOffensiveStrength()
    {
        AbilityTypeList abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
        MaxOffensiveStrength = 0;
        Array.Clear(_offensiveStrength);
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            Ability ability = abilities.GetAbility(i);
            if (!GetAbilityEnabled(ability.AbilityId))
            {
                continue;
            }

            int strength = 0;
            Set<DamageType> types = default;
            ability.GetOffensiveStrength(this, ref strength, ref types);
            if (strength > 255)
            {
                strength = 255;
            }

            if (types == default)
            {
                Assert(false, $"Ability: {ability.GetName(null)}", @"D:\AoWDev\AoWE\inc\Unit.PAS", 0xE2B4);
            }

            if (types == default)
            {
                continue;
            }

            if (MaxOffensiveStrength < strength)
            {
                MaxOffensiveStrength = (byte)strength;
            }

            for (int kind = (int)DamageType.Fire; kind <= (int)DamageType.None; kind++)
            {
                if (types.Contains((DamageType)kind) && _offensiveStrength[kind] < strength)
                {
                    _offensiveStrength[kind] = (byte)strength;
                }
            }
        }
    }

    internal static int UnitStrikeDamageValue(Troop source, Troop target)
    {
        GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).MeleeRound.CalculateFreeUnitSwing(source, target);
        return GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).MeleeRound.GetAttackerDamageValue();
    }

    public enum Morale : byte
    {
        Terrible = 0, Poor = 1, Ok = 2, Good = 3, High = 4
    }
}
