#nullable disable
// Translated from AoWE.pas.
using System.Collections.Immutable;

using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Magic.Abilities;

// UpgradePending. HeroResource. HeroResourceIndex. Experience. _usedSkillPoints. DeathDay.
// DeathPlayer. Level. HeroStatus. HeroName. Nickname. Race. Gender. unit statistics.
// CastSpellInstantly. MovePoints. HitPoints. PowerSource. CastingPoints. CastingManaValue.
// CastingProgress. LibraryId. LibraryName. Registered
public partial class Hero : Troop
{
    public new const int ClassId = 131632;

    public int FaceIndex { get; private set; }

    public HeroResource HeroResource { get; set; }

    public int HeroResourceIndex { get; set; }
    public int Experience { get; set; }
    public byte Level { get; private set; }

    private int _usedSkillPoints;

    public bool UpgradePending { get; set; }
    public Set<HeroStatusElement> HeroStatus { get; private set; }

    private int _deathDay;
    private sbyte _deathPlayer;

    public Set<HeroModeElement> Mode { get; set; }

    public string HeroName { get; set; } = "";
    public string Nickname { get; private set; } = "";
    public sbyte Race { get; private set; }

    private UnitGender _gender;

    public sbyte UnitAttack { get; set; }
    public sbyte UnitDefense { get; private set; }
    public sbyte UnitDamage { get; private set; }
    public sbyte UnitHits { get; private set; }
    public sbyte UnitMoves { get; private set; }
    public sbyte UnitResistance { get; private set; }

    public HeroItems HeroItems { get; }
    public HeroInventory Inventory { get; }

    private bool _castSpellInstantly;
    private sbyte _movePoints;

    public sbyte HitPoints { get; set; }

    public HeroPowerSource PowerSource { get; }

    public sbyte CastingPoints { get; private set; }
    public int CastingSpellId { get; private set; }

    public override int GetCastingSpellId() => CastingSpellId;

    public int CastingManaValue { get; private set; }
    public int CastingProgress { get; private set; }
    public int LibraryId { get; set; }
    public string LibraryName { get; set; } = "";

    public bool Registered { get; set; }

    // Hero.PAS:631
    public override void Dispose()
    {
        _ = AddRef();
        _ = AddRef();
        _ = AddRef();
        // { Original repeats the reference increment four times. }
        _ = AddRef();
        if (Registered && StrategicMap.Current is not null)
        {
            StrategicMap.Current.HeroControl.UnregisterHero(this);
        }

        base.Dispose();
        HeroItems?.Dispose();
        Inventory?.Dispose();
        PowerSource?.Dispose();
    }

    // Hero.PAS:683
    public override void SetOwner(CustomNode owner)
    {
        _ = AddRef();
        base.SetOwner(owner);
        if (UnitStatus.Contains(UnitStatusElement.Activated))
        {
            Changed();
        }

        _ = Release();
    }

    // Hero.PAS:1625
    public override void PersistFields(IPersister persister)
    {
        int oldExperience = default;
        sbyte player = Player;
        HeroResourceIndex = persister.Int32("HeroResourceIndex", value: HeroResourceIndex);
        if (persister.Loading)
        {
            LinkToResource();
        }

        base.PersistFields(persister);
        if (persister.Loading && Mode.Contains(HeroModeElement.UpdatingFromLibrary))
        {
            Player = player;
        }

        FaceIndex = persister.Int32("FaceIndex", value: FaceIndex);
        oldExperience = persister.Loading ? persister.Int32("OldExperience", value: oldExperience) : 0;

        HeroName = persister.String("HeroName", value: HeroName);
        Nickname = persister.String("Nickname", value: Nickname);
        Race = PersistSupport.UnsignedByte(persister, name: "Race", value: Race);
        CastingPoints = PersistSupport.UnsignedByte(persister, name: "CastingPoints", value: CastingPoints);
        CastingSpellId = persister.Int32("CastingSpellId", value: CastingSpellId);
        _gender = persister.Enum("Gender", value: _gender);
        UnitAttack = PersistSupport.UnsignedByte(persister, name: "UnitAttack", value: UnitAttack);
        UnitDefense = PersistSupport.UnsignedByte(persister, name: "UnitDefense", value: UnitDefense);
        UnitDamage = PersistSupport.UnsignedByte(persister, name: "UnitDamage", value: UnitDamage);
        UnitHits = PersistSupport.UnsignedByte(persister, name: "UnitHits", value: UnitHits);
        UnitMoves = PersistSupport.UnsignedByte(persister, name: "UnitMoves", value: UnitMoves);
        UnitResistance = PersistSupport.UnsignedByte(persister, name: "UnitResistance", value: UnitResistance);
        Level = persister.Byte("Level", value: Level);
        UpgradePending = persister.Boolean("UpgradePending", value: UpgradePending);
        Experience = persister.Int32("Experience", value: Experience);
        HeroStatus = persister.Set("HeroStatus", value: HeroStatus);
        _deathDay = persister.Int32("DeathDay", value: _deathDay);
        _deathPlayer = PersistSupport.UnsignedByte(persister, name: "DeathPlayer", value: _deathPlayer);
        if (persister.Loading && Experience == 0)
        {
            Experience = LevelToExperience((oldExperience >>> 5) + 1);
        }

        _movePoints = PersistSupport.UnsignedByte(persister, name: "MovePoints", value: _movePoints);
        HitPoints = PersistSupport.UnsignedByte(persister, name: "HitPoints", value: HitPoints);
        CastingManaValue = persister.Int32("CastingManaValue", value: CastingManaValue);
        _castSpellInstantly = persister.Boolean("CastSpellInstantly", value: _castSpellInstantly);
        CastingProgress = persister.Int32("CastingProgress", value: CastingProgress);
        LibraryId = persister.Int32("LibraryId", value: LibraryId, fallback: -1);
        if (!Mode.Contains(HeroModeElement.UpdatingFromLibrary))
        {
            persister.Object("Inventory", objectValue: Inventory);
            persister.Object("HeroItems", objectValue: HeroItems);
        }
        else
        {
            // The original ReadWrite leaves the stored items unread while the hero updates from its library.
            persister.Skip("Inventory");
            persister.Skip("HeroItems");
        }

        // The original leaves the library name out under the text-only mode of the translate stream.
        if (!PersistSupport.Translating(persister))
        {
            LibraryName = persister.String("LibraryName", value: LibraryName);
        }
        _usedSkillPoints = persister.Int32("UsedSkillPoints", value: _usedSkillPoints);
        if (persister.Loading)
        {
            Loaded();
        }
    }

    // Hero.PAS:1694
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            if (StrategicMap.Current.Players.GetCount() <= Player && Owner is null)
            {
                SetPlayer(-1);
                return;
            }
        }
        else
        {
            if (message.MessageId == 65541)
            {
                EditDropMessage(ref message);
            }
        }
    }

    // Hero.PAS:1347
    public override bool GetAbilitySetAt(int index) => base.GetAbilitySetAt(index);

    // Hero.PAS:1344
    public override void SetAbilitySet(int index, byte value) => base.SetAbilitySet(index, value);

    // Hero.PAS:697
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of(AbilitySelectionType.Editor);

    // Hero.PAS:716
    public override void Changed()
    {
        base.Changed();
        if (StrategicMap.Current is not null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            StrategicMap.Current.HeroControl.NotifyHeroChanged(this);
        }
    }

    // Hero.PAS:930
    public override byte GetUpkeep() => (byte)((GetLevel() * 2) + 5);

    // Hero.PAS:680
    public override sbyte UnitLevel => (sbyte)Level;

    // Hero.PAS:818
    public override sbyte GetRace() => HeroResource.Race;

    // Hero.PAS:1426
    public override sbyte GetInherentAttack() => (sbyte)(HeroResource.Attack + UnitAttack);

    // Hero.PAS:1445
    public override sbyte GetInherentDefense() => (sbyte)(HeroResource.Defense + UnitDefense);

    // Hero.PAS:1464
    public override sbyte GetInherentDamage() => (sbyte)(HeroResource.Damage + UnitDamage);

    // Hero.PAS:1483
    public override sbyte InherentResistance => (sbyte)(HeroResource.Resistance + UnitResistance);

    // Hero.PAS:1429
    public override sbyte GetAttack()
    {
        int count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int i = 0; i <= count; i++)
        {
            AbilityOwner owner = GetSuperlativeEnabledAbilityOwner(i);
            if (owner is not null)
            {
                result = (sbyte)(result + owner.GetAbilityAttack(i));
            }
        }

        result = (sbyte)(HeroItems.GetAttack() + (UnitAttack + result + HeroResource.Attack));
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? (sbyte)10 : result;
    }

    // Hero.PAS:1448
    public override sbyte GetDefense()
    {
        int count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int i = 0; i <= count; i++)
        {
            AbilityOwner owner = GetSuperlativeEnabledAbilityOwner(i);
            if (owner is not null)
            {
                result = (sbyte)(result + owner.GetAbilityDefense(i));
            }
        }

        sbyte defense = HeroItems.GetDefense();
        Morale morale = GetMorale();
        result = (sbyte)(defense + (UnitDefense + result + HeroResource.Defense) + MoraleDefense[(int)morale]);
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? (sbyte)10 : result;
    }

    // Hero.PAS:1467
    public override sbyte GetDamage()
    {
        int count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int i = 0; i <= count; i++)
        {
            AbilityOwner owner = GetSuperlativeEnabledAbilityOwner(i);
            if (owner is not null)
            {
                result = (sbyte)(result + owner.GetAbilityDamage(i));
            }
        }

        result = (sbyte)(HeroItems.GetDamage() + (UnitDamage + result + HeroResource.Damage));
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? (sbyte)10 : result;
    }

    // Hero.PAS:1486
    public override sbyte GetResistance()
    {
        int count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int i = 0; i <= count; i++)
        {
            AbilityOwner owner = GetSuperlativeEnabledAbilityOwner(i);
            if (owner is not null)
            {
                result = (sbyte)(result + owner.GetAbilityResistance(i));
            }
        }

        sbyte resistance = HeroItems.GetResistance();
        Morale morale = GetMorale();
        result = (sbyte)(resistance + (UnitResistance + result + HeroResource.Resistance) + MoraleResistance[(int)morale]);
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? (sbyte)10 : result;
    }

    // Hero.PAS:1502
    public override sbyte GetHits()
    {
        sbyte result = (sbyte)(HeroResource.Hits + UnitHits);
        if (result < 1)
        {
            result = 1;
        }

        return result > 30 ? (sbyte)30 : result;
    }

    // Hero.PAS:1509
    public override sbyte Moves
    {
        get
        {
            sbyte result = (sbyte)(HeroResource.Moves + UnitMoves);
            if (result < 1)
            {
                result = 1;
            }

            return result > 50 ? (sbyte)50 : result;
        }
    }

    // Hero.PAS:842
    public override sbyte GetMovePoints() => _movePoints;

    // Hero.PAS:845
    public override void SetMovePoints(sbyte value)
    {
        if (Moves < value)
        {
            value = Moves;
        }

        _movePoints = value;
    }

    // Hero.PAS:851
    public override sbyte GetHitPoints() => HitPoints;

    // Hero.PAS:854
    public override void SetHitPoints(sbyte value)
    {
        if (GetHits() < value)
        {
            value = GetHits();
        }

        if (value < 0)
        {
            value = 0;
        }

        HitPoints = value;
        UpdateDefensiveStrength();
    }

    // Hero.PAS:824
    public override Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> result = base.GetMoveTypes();
        return result + HeroItems.GetMoveTypes();
    }

    // Hero.PAS:830
    public override Set<DamageType> GetImmunityTypes()
    {
        Set<DamageType> result = base.GetImmunityTypes();
        return result + HeroItems.GetImmunityTypes();
    }

    // Hero.PAS:836
    public override Set<DamageType> GetProtectionTypes()
    {
        Set<DamageType> result = base.GetProtectionTypes();
        return result + HeroItems.GetProtectionTypes();
    }

    // Hero.PAS:1596
    public override LibraryImage GetFace()
    {
        if (!GetAbilityEnabled(126))
        {
            FaceResource resource = GamePack.Current.FaceResources.GetFaceResource(FaceIndex);
            return resource?.GetCellImage();
        }
        else
        {
            int index = ((CosmeticSurgeryEnchantment)GetAbilityData<UnitEnchantmentAbilityData>(126).Enchantment).FaceIndex;
            return GamePack.Current.FaceResources.GetFaceResource(index).GetCellImage();
        }
    }

    // Hero.PAS:821
    public override string GetName() => HeroName;

    // Hero.PAS:862
    public override Alignment GetAlignment()
    {
        if (GetAbilityEnabled(107))
        {
            return Alignment.PureEvil;
        }
        else
        {
            sbyte race = Race;
            return GamePack.Current.RaceResources.GetRaceResource(race) is not null
                ? GamePack.Current.RaceResources.GetRaceResource(race).Alignment
                : HeroResource.Alignment;
        }
    }

    // Hero.PAS:1538
    public override LibraryImage GetPreviewImage() => HeroResource?.GetPreviewImage();

    // Hero.PAS:732
    public override UnitGender Gender => _gender;

    // Hero.PAS:735
    public override UnitType GetUnitType() => UnitType.Humanoid;

    // Hero.PAS:1003
    public override int UnitGraphicsResourceIndex => HeroResource.UnitGraphicsResourceIndex;

    // Hero.PAS:644
    public override int GetCampaignTransferPoints()
    {
        int result = base.GetCampaignTransferPoints();
        return result / 2;
    }

    // Hero.PAS:1522
    public override sbyte GetCastingPointsMax() => GetAbilityEnabled(52) ? (sbyte)(GetAbilityLevel(52) * 10) : (sbyte)0;

    // Hero.PAS:1528
    public override sbyte GetCastingPoints() => CastingPoints;

    // Hero.PAS:1531
    public override void SetCastingPoints(sbyte value)
    {
        if (value > GetCastingPointsMax())
        {
            value = GetCastingPointsMax();
        }

        CastingPoints = value;
        Changed();
    }

    // Hero.PAS:1516
    public override int GetPowerGeneration() => GetAbilityEnabled(52) ? GetAbilityLevel(52) * 5 : 0;

    // Hero.PAS:738
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1)
        {
            if ((GetEngine().Campaign is null || StrategicMap.Current.Players.GetPlayers(Player) is null || StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human) && (!StrategicMap.Current.LeadersFixed || this is not Leader))
            {
                _usedSkillPoints = GetSkillPoints();
            }

            UpdateSettings();
        }
    }

    // Hero.PAS:1311
    public override void NewTurn(sbyte player)
    {
        base.NewTurn(player);
        if (player == Player)
        {
            ValidateHeroUpgrade();
            player = GetCastingPointsMax();
            CastingPoints = player;
            if (CastingSpellId != 0 && CastingManaValue < CastingProgress)
            {
                CastingManaValue += player;
                CastingPoints = 0;
                if (CastingManaValue > CastingProgress)
                {
                    CastingPoints = (sbyte)(CastingManaValue - CastingProgress);
                    CastingManaValue = CastingProgress;
                }

                if (CastingManaValue == CastingProgress)
                {
                    SpellCastEventLog log = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = Player,
                        UnitId = UnitId,
                        SpellId = CastingSpellId
                    };
                    StrategicMap.Current.Players.GetPlayers(Player).EventLogbook.AddEvent(log);
                    if (StrategicMap.Current.SeatedPlayer == Player && StrategicMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Human)
                    {
                        _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
                    }

                    _ = log.Release();
                }
            }
        }
    }

    // Hero.PAS:1418
    public override int GetAbilityLevel(int abilityId)
    {
        AbilityOwner owner = GetSuperlativeAbilityOwner(abilityId) ?? this;
        return GamePack.Current.AbilityControl[abilityId].GetLevel(owner);
    }

    // Hero.PAS:1396
    public override bool GetAbilityEnabled(int abilityId) => base.GetAbilityEnabled(abilityId) || HeroItems.GetAbilityEnabled(abilityId) || Inventory.GetAbilityEnabled(abilityId);

    // Hero.PAS:1403
    public override bool GetAbilitySet(int abilityId) => base.GetAbilitySet(abilityId) || HeroItems.GetAbilitySet(abilityId) || Inventory.GetAbilitySet(abilityId);

    // Hero.PAS:1410
    public override string GetAbilityName(int abilityId)
    {
        AbilityOwner owner = GetSuperlativeAbilityOwner(abilityId) ?? this;
        return GamePack.Current.AbilityControl[abilityId].GetName(owner);
    }

    // Hero.PAS:1393
    public override AbilityOwner GetAbilityOwner(int abilityId) => GetSuperlativeAbilityOwner(abilityId);

    // Hero.PAS:1353
    public override int GetAbilityCount()
    {
        int result = OwnAbilityCount;
        int count = HeroItems.GetAbilityCount();
        if (count > result)
        {
            result = count;
        }

        count = Inventory.GetAbilityCount();
        return count > result ? count : result;
    }

    // Hero.PAS:1028
    public override int GetExperience() => Experience;

    // Hero.PAS:1031
    public override void SetExperience(int value)
    {
        Experience = value;
        int maximum = LevelToExperience(LevelMax);
        if (maximum < Experience)
        {
            Experience = maximum;
        }

        Changed();
    }

    // Hero.PAS:1063
    public override int GetNextLevelExperience()
    {
        int level = GetLevel();
        int maximum = LevelMax;
        return level < maximum ? LevelToExperience(level + 1) : LevelToExperience(level);
    }

    // Hero.PAS:729
    public override DelphiStringCollection Description => null;

    // Hero.PAS:933
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            bool condition = Player != -1;
            if (condition)
            {
                int count = StrategicMap.Current.Players.Count;
                condition = Player < count;
            }

            if (condition)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(PowerSource);
                _ = StrategicMap.Current.Players.GetPlayers(Player).Heroes.RemoveChild(this);
            }

            base.SetPlayer(player);
            if (Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).Heroes.AddChild(this);
                _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(PowerSource);
                StrategicMap.Current.Players.GetPlayers(Player).ResetBaseHeroJoinDay();
            }
        }
    }

    // Hero.PAS:726
    public override int GetObtainValue() => (GetLevel() * 50) + 75;

    // Hero.PAS:914
    public override bool CanActivate()
    {
        bool result = base.CanActivate();
        if (result && !Registered && UnitId != -1)
        {
            Hero hero = StrategicMap.Current.HeroControl.FindId(UnitId);
            return hero is null || hero == this || (hero.Owner is null && CompatibleHero(hero));
        }

        return result;
    }

    // Hero.PAS:964
    public override void Activate()
    {
        if (UnitId != -1)
        {
            Hero hero = StrategicMap.Current.HeroControl.FindId(UnitId);
            if (hero is not null && hero != this && hero.Owner is null && CompatibleHero(hero))
            {
                StrategicMap.Current.HeroControl.UnregisterHero(hero);
            }
        }

        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Level = (byte)GetLevel();
        }

        if (HeroResourceIndex == -1)
        {
            SetHeroResourceIndex(0);
        }

        if (Player > 0 && StrategicMap.Current.Players.GetPlayers(Player) is null && Owner is null)
        {
            Player = -1;
        }

        if (Owner is null && GroupId != 0)
        {
            GroupId = 0;
        }

        base.Activate();
        if (StrategicMap.Current is not null)
        {
            if (!Mode.Contains(HeroModeElement.SkipLibraryUpdate) && (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || StrategicMap.Current.Day == 0))
            {
                UpdateFromLibrary();
            }

            if (!Registered)
            {
                _ = StrategicMap.Current.HeroControl.RegisterHero(this);
            }

            if (Player != -1 && Owner is not null)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(PowerSource);
                _ = StrategicMap.Current.Players.GetPlayers(Player).Heroes.AddChild(this);
            }

            Inventory.Items.RegisterItems();
            HeroItems.Items.RegisterItems();
        }
    }

    // Hero.PAS:993
    public override void Deactivate()
    {
        bool condition = Player != -1;
        if (condition)
        {
            int count = StrategicMap.Current.Players.Count;
            condition = Player < count;
        }

        if (condition)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(PowerSource);
            _ = StrategicMap.Current.Players.GetPlayers(Player).Heroes.RemoveChild(this);
        }

        base.Deactivate();
    }

    // Hero.PAS:765
    public override bool CanJoin(sbyte player)
    {
        bool result;
        if (Race != -1)
        {
            Player playerLocal = StrategicMap.Current.Players.GetPlayers(player);
            Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
            result = Set<Status>.Of(Status.Polite, Status.Friendly).Contains(playerRelation);
            if (result)
            {
                bool condition = Group is null || Group.GroupClass.Behavior != GroupBehavior.Refuge;
                if (condition)
                {
                    bool test = !GetAbilityEnabled(128) || player != _deathPlayer;
                    if (!test)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(player);
                        test = _deathDay + 2 < players.Day;
                    }

                    condition = test;
                }

                if (condition)
                {
                    return result && playerLocal.Heroes.Count < playerLocal.HeroCountMax && GetLevel() <= playerLocal.GetHeroJoinLevelMax();
                }
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Hero.PAS:794
    public override bool OfferToJoin(sbyte player)
    {
        bool result;
        if (CanJoin(player))
        {
            Player playerLocal = StrategicMap.Current.Players.GetPlayers(player);
            if (EmergedToPlayer() == -1)
            {
                if ((!GetAbilityEnabled(128) || player != _deathPlayer) && (Group is null || Group.GroupClass.Behavior != GroupBehavior.Refuge))
                {
                    int chance = playerLocal.GetHeroJoinChance(GetLevel());
                    int seed = RandSeed;
                    RandSeed = (StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + (UnitId * 16)) & 4088;
                    result = Random(100) < chance;
                    RandSeed = seed;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return player == EmergedToPlayer();
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Hero.PAS:706
    public override int Show(int X, int Y, in LockedSurface lockedSurface, in UnitShowInfo info)
    {
        int result = base.Show(X, Y, lockedSurface, info);
        if (info.Overlays.Contains(UnitOverlayModesElement.Rank))
        {
            GetEngine().CombatFont.Color = 16777215;
            sbyte level = GetLevel();
            GetEngine().CombatFont.SetTextCentered(X + 43, Y + 12, text: IntToStr(level), lockedSurface);
        }

        return result;
    }

    // Hero.PAS:885
    public override void Killed()
    {
        for (int i = Inventory.Items.Count - 1; i >= 0; i--)
        {
            if (Inventory.Items.GetItem(i) is not null)
            {
                _ = ExecutePlaceItem(Inventory.Items.GetItem(i).ItemId, position: HeroItemPosition.Ground);
            }
        }

        for (int positionOrdinal = (int)HeroItemPosition.AttackSlot; positionOrdinal <= (int)HeroItemPosition.RightRingSlot; positionOrdinal++)
        {
            HeroItemPosition position = (HeroItemPosition)positionOrdinal;
            if (HeroItems.GetPositionItem(position) is not null)
            {
                _ = ExecutePlaceItem(HeroItems.GetPositionItem(position).ItemId, position: HeroItemPosition.Ground);
            }
        }

        _deathDay = StrategicMap.Current.Players.GetPlayers(Player).Day;
        _deathPlayer = Player;
        base.Killed();
        HeroStatus += Set<HeroStatusElement>.Of(HeroStatusElement.Died);
        SetPlayer(-1);
        SetOwner(null);
    }

    // Hero.PAS:902
    public override void Resurrect()
    {
        base.Resurrect();
        HeroStatus -= Set<HeroStatusElement>.Of(HeroStatusElement.Died);
    }

    // Hero.PAS:908
    public override void Animate()
    {
        base.Animate();
        HeroStatus -= Set<HeroStatusElement>.Of(HeroStatusElement.Died);
    }

    // Hero.PAS:691
    public virtual string GetNickname() => Nickname;

    // Hero.PAS:694
    public virtual void SetNickname(string value) => Nickname = value;

    // Hero.PAS:1615
    public virtual void SetHeroResourceIndex(int value)
    {
        if (GamePack.Current.HeroResources.GetHeroResource(value) is not null && value != HeroResourceIndex)
        {
            HeroResourceIndex = value;
            HeroResource = GamePack.Current.HeroResources.GetHeroResource(value);
            ResetResourceSettings();
        }
    }

    // Hero.PAS:1674
    public virtual void EditDropMessage(ref EngineMessage message)
    {
        MapLevel level = message.Param[0].As<MapLevel>();
        int l = message.Data[1];
        HexGeometry.PixelToHex(message.Payload.Int, vertical: message.Data[0], out int x, out int y);
        bool placed;
        if (x >= 0 && y >= 0 && x < level.Width && y < level.Height)
        {
            int player;
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                player = StrategicMap.Current.MapContainer.GetField(x, y, l).GetPlayer();
                if (player == -1)
                {
                    player = StrategicMap.Current.EditorPlayer;
                }
            }
            else
            {
                player = -1;
            }

            placed = Place(x, y, l, player: (sbyte)player, mode: Set<UnitPlaceModeElement>.Of()) is not null;
        }
        else
        {
            placed = false;
        }

        message.Result = placed ? 0 : -1;
    }

    // Hero.PAS:650
    public string GetLibraryName()
    {
        string result = LibraryName;
        return IsEmpty(result) ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SystemText) : result;
    }

    // Hero.PAS:656
    public void UpdateFromLibraryBegin() => Mode += Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);

    // Hero.PAS:659
    public void UpdateFromLibraryEnd() => Mode -= Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);

    // Hero.PAS:662
    public void UpdateFromLibrary()
    {
        if (LibraryId != -1)
        {
            Hero hero = GamePack.Current.HeroLibraries.FindHero(LibraryName, LibraryId);
            if (hero is not null)
            {
                int id = UnitId;
                Mode += Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
                Assign(hero, engine: GetEngine());
                Mode -= Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
                UnitId = id;
                Changed();
            }
        }
    }

    // Hero.PAS:703
    public bool LibraryHero => LibraryId != -1;

    // Hero.PAS:751
    public int UsedSkillPoints()
    {
        int result = (UnitAttack * 5) + (UnitDefense * 5) + (UnitDamage * 10) + (UnitResistance * 5) + (UnitHits * 5) + (UnitMoves * 2);
        for (int i = OwnAbilityCount - 1; i >= 0; i--)
        {
            if (GetAbilitySetAt(i))
            {
                result += GamePack.Current.AbilityControl[i].GetSkillPoints(this);
                if (HeroResource.Abilities.GetAbilitySetAt(i))
                {
                    result -= GamePack.Current.AbilityControl[i].GetSkillPoints(HeroResource.Abilities);
                }
            }
        }

        return result;
    }

    // Hero.PAS:782
    public sbyte EmergedToPlayer()
    {
        int i = StrategicMap.Current.Players.Count;
        sbyte result = -1;
        while (i != 1 && result == -1)
        {
            i -= 1;
            if (StrategicMap.Current.Players.GetPlayers(i).EmergingHeroId == UnitId)
            {
                result = (sbyte)i;
            }
        }

        return result;
    }

    // Hero.PAS:875
    public void DropItemsOnMap(StrategicMap map, sbyte X, sbyte Y, sbyte L)
    {
        for (int i = Inventory.Items.Count - 1; i >= 0; i--)
        {
            _ = (Inventory.Items.GetItem(i)?.PlaceOnMap(map, X, Y, L));
        }

        for (int positionOrdinal = (int)HeroItemPosition.AttackSlot; positionOrdinal <= (int)HeroItemPosition.RightRingSlot; positionOrdinal++)
        {
            HeroItemPosition position = (HeroItemPosition)positionOrdinal;
            _ = (HeroItems.GetPositionItem(position)?.PlaceOnMap(map, X, Y, L));
        }
    }

    // Hero.PAS:952
    public bool CompatibleHero(Hero hero)
    {
        int groupId = GroupId;
        int otherGroupId = hero.GroupId;
        hero.GroupId = 0;
        GroupId = 0;
        bool result = hero.Compare(this, engine: GetEngine());
        hero.GroupId = otherGroupId;
        GroupId = groupId;
        return result;
    }

    // Hero.PAS:1006
    public int GetSkillPoints()
    {
        if (StrategicMap.Current is not null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            int usedSkillPoints = UsedSkillPoints();
            return (Level * 10) + 15 - usedSkillPoints - _usedSkillPoints;
        }
        else
        {
            int usedSkillPoints = UsedSkillPoints();
            return (Level * 10) + 15 - usedSkillPoints;
        }
    }

    // Hero.PAS:1013
    public int SkillPointsMax => (Level * 10) + 15 - _usedSkillPoints;

    // Hero.PAS:1016
    public int LevelMax
    {
        get
        {
            int result = 30;
            if (StrategicMap.Current is not null && StrategicMap.Current.CampaignSettings is not null && StrategicMap.Current.CampaignSettings.HeroLevelMax > 0)
            {
                int maximum = StrategicMap.Current.CampaignSettings.HeroLevelMax;
                if (maximum < result)
                {
                    return maximum;
                }
            }

            return result;
        }
    }

    // Hero.PAS:1040
    public int LevelToExperience(int level)
    {
        int total = 0;
        for (int i = 1; i <= level - 1; i++)
        {
            int quotient = i / 5;
            if (quotient <= 5)
            {
                total += s_heroExperienceTable[quotient].Experience;
            }
        }

        return total;
    }

    // Hero.PAS:1049
    public sbyte ExperienceToLevel(int experience)
    {
        int level = 1;
        int total = 0;
        int maximum = LevelMax;
        do
        {
            total += s_heroExperienceTable[level / 5].Experience;
            if (experience < total)
            {
                break;
            }

            level += 1;
        }
        while (!(level >= maximum));
        return (sbyte)level;
    }

    // Hero.PAS:1071
    public sbyte GetLevel() => ExperienceToLevel(Experience);

    // Hero.PAS:1076
    public void SetLevel(sbyte value)
    {
        int maximum = LevelMax;
        if (value < 1)
        {
            value = 1;
        }

        if (maximum < value)
        {
            value = (sbyte)maximum;
        }

        int level = GetLevel();
        if (level > value)
        {
            int points = GetSkillPoints();
            while (points >= 10 && level != value)
            {
                level -= 1;
                points -= 10;
            }

            value = (sbyte)level;
        }

        Experience = LevelToExperience(value);
        Level = (byte)value;
    }

    // Hero.PAS:1097
    public HeroType HeroType => HeroResource.HeroType;

    // Hero.PAS:1100
    public void SetUnitAttack(sbyte value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value + HeroResource.Attack > 10)
        {
            value = (sbyte)(10 - HeroResource.Attack);
        }

        if (value > UnitAttack)
        {
            int points = GetSkillPoints();
            while (points >= 5 && value != UnitAttack)
            {
                UnitAttack = (sbyte)(UnitAttack + 1);
                points -= 5;
            }
        }
        else
        {
            UnitAttack = value;
        }
    }

    // Hero.PAS:1117
    public void SetUnitDefense(sbyte value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value + HeroResource.Defense > 10)
        {
            value = (sbyte)(10 - HeroResource.Defense);
        }

        if (value > UnitDefense)
        {
            int points = GetSkillPoints();
            while (points >= 5 && value != UnitDefense)
            {
                UnitDefense = (sbyte)(UnitDefense + 1);
                points -= 5;
            }
        }
        else
        {
            UnitDefense = value;
        }
    }

    // Hero.PAS:1134
    public void SetUnitDamage(sbyte value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value + HeroResource.Damage > 10)
        {
            value = (sbyte)(10 - HeroResource.Damage);
        }

        if (value > UnitDamage)
        {
            int points = GetSkillPoints();
            while (points >= 10 && value != UnitDamage)
            {
                UnitDamage = (sbyte)(UnitDamage + 1);
                points -= 10;
            }
        }
        else
        {
            UnitDamage = value;
        }
    }

    // Hero.PAS:1151
    public void SetUnitHits(sbyte value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value + HeroResource.Hits > 30)
        {
            value = (sbyte)(30 - HeroResource.Hits);
        }

        if (value > UnitHits)
        {
            int points = GetSkillPoints();
            while (points >= 5 && value != UnitHits)
            {
                UnitHits = (sbyte)(UnitHits + 1);
                points -= 5;
            }
        }
        else
        {
            UnitHits = value;
        }
    }

    // Hero.PAS:1168
    public void SetUnitMoves(sbyte value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value + HeroResource.Moves > 50)
        {
            value = (sbyte)(50 - HeroResource.Moves);
        }

        if (value > UnitMoves)
        {
            int points = GetSkillPoints();
            while (points >= 2 && value != UnitMoves)
            {
                UnitMoves = (sbyte)(UnitMoves + 1);
                points -= 2;
            }
        }
        else
        {
            UnitMoves = value;
        }
    }

    // Hero.PAS:1185
    public void SetUnitResistance(sbyte value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value + HeroResource.Resistance > 10)
        {
            value = (sbyte)(10 - HeroResource.Resistance);
        }

        if (value > UnitResistance)
        {
            int points = GetSkillPoints();
            while (points >= 5 && value != UnitResistance)
            {
                UnitResistance = (sbyte)(UnitResistance + 1);
                points -= 5;
            }
        }
        else
        {
            UnitResistance = value;
        }
    }

    // Hero.PAS:1202
    public void LinkToResource() => HeroResource = GamePack.Current.HeroResources.GetHeroResource(HeroResourceIndex);

    // Hero.PAS:1207
    public void Loaded()
    {
        if (HeroResource is not null)
        {
            UpdateDefaultAbilities(HeroResource.Abilities);
            SetUnitAttack(UnitAttack);
            SetUnitDefense(UnitDefense);
            SetUnitResistance(UnitResistance);
            SetUnitDamage(UnitDamage);
            SetUnitHits(UnitHits);
            SetUnitMoves(UnitMoves);
        }
    }

    // Hero.PAS:1221
    public void ListSpells(SpellList list) => StrategicMap.Current.Players.GetPlayers(Player).MagicControl.ListSpells(list, spellTypes: Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));

    // Hero.PAS:1226
    public void ExecuteUpgradeHeroAI()
    {
        DoubleItemList choices = new();
        while (true)
        {
            choices.Clear();
            int points = GetSkillPoints();
            if (points >= 5 && UnitAttack + HeroResource.Attack < 10)
            {
                _ = choices.AddAbility(0, 10);
            }

            if (points >= 5 && UnitDefense + HeroResource.Defense < 10)
            {
                _ = choices.AddAbility(1, 25);
            }

            if (points >= 5 && UnitResistance + HeroResource.Resistance < 10)
            {
                _ = choices.AddAbility(2, 10);
            }

            if (points >= 10 && UnitDamage + HeroResource.Damage < 10)
            {
                _ = choices.AddAbility(3, 10);
            }

            if (points >= 5 && UnitHits + HeroResource.Hits < 30)
            {
                _ = choices.AddAbility(4, 10);
            }

            if (GamePack.Current.AbilityControl[46].CanExpand(this) && points >= GamePack.Current.AbilityControl[46].ExpandCost(this))
            {
                _ = choices.AddAbility(5, 10);
            }

            if (GamePack.Current.AbilityControl[22].CanExpand(this) && points >= GamePack.Current.AbilityControl[22].ExpandCost(this))
            {
                _ = choices.AddAbility(6, 10);
            }

            if (GamePack.Current.AbilityControl[52].CanExpand(this))
            {
                _ = points >= GamePack.Current.AbilityControl[52].ExpandCost(this)
                    ? choices.AddAbility(7, 9999)
                    : choices.AddAbility(7, 100);
            }

            if (choices.Count != 0)
            {
                RandSeed = StrategicMap.Current.Random(16777215);
                int index = choices.RandomChanceItemB(0);
                DoubleItemList.DoubleItem choice = choices[index];
                switch (choice.First)
                {
                    case 0:
                        SetUnitAttack((sbyte)(UnitAttack + 1));
                        break;
                    case 1:
                        SetUnitDefense((sbyte)(UnitDefense + 1));
                        break;
                    case 2:
                        SetUnitResistance((sbyte)(UnitResistance + 1));
                        break;
                    case 3:
                        SetUnitDamage((sbyte)(UnitDamage + 1));
                        break;
                    case 4:
                        SetUnitHits((sbyte)(UnitHits + 1));
                        break;
                    case 5:
                        _ = ExpandAbility(46);
                        break;
                    case 6:
                        _ = ExpandAbility(22);
                        break;
                    case 7:
                        if (points >= GamePack.Current.AbilityControl[52].ExpandCost(this))
                        {
                            _ = ExpandAbility(52);
                        }
                        else
                        {
                            points = 0;
                        }

                        break;
                    default:
                        break;
                }
            }

            if (choices.Count == 0 || GetSkillPoints() == 0 || points == 0)
            {
                break;
            }
        }

        choices?.Dispose();
    }

    // Hero.PAS:1269
    public void ValidateHeroUpgrade()
    {
        MapCoordinate coordinate42 = GetLocation();
        sbyte x = coordinate42.X;
        sbyte y = coordinate42.Y;
        sbyte l = coordinate42.MapLevel;
        if (Player != 0 && x != -1)
        {
            UpgradePending = false;
            int level = GetLevel();
            if (level < LevelMax)
            {
                if (Player > 0 && StrategicMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu2)
                {
                    SetExperience(GetExperience() + s_heroExperienceTable[level / 5].DailyExperience + 1);
                }
                else
                {
                    if (Player > 0 && StrategicMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu3)
                    {
                        SetExperience(GetExperience() + s_heroExperienceTable[level / 5].DailyExperience + 2);
                    }
                    else
                    {
                        if (Player > 0 && StrategicMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu4)
                        {
                            SetExperience(GetExperience() + s_heroExperienceTable[level / 5].DailyExperience + 3);
                        }
                        else
                        {
                            if (Player > 0 && StrategicMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu5)
                            {
                                SetExperience(GetExperience() + s_heroExperienceTable[level / 5].DailyExperience + 4);
                            }
                            else
                            {
                                SetExperience(GetExperience() + s_heroExperienceTable[level / 5].DailyExperience);
                            }
                        }
                    }
                }

                level = GetLevel();
            }

            if (Level < level)
            {
                UpgradePending = true;
                int oldLevel = Level;
                Level = (byte)level;
                HeroUpgradeEventLog log = new(null)
                {
                    Holder = CreationHolder(),
                    OldLevel = (byte)oldLevel,
                    NewLevel = Level,
                    UpgradeDay = StrategicMap.Current.Players.GetPlayers(Player).Day,
                    Player = Player,
                    UnitId = UnitId
                };
                EventLog.DistributeLocationEventLog(log, Player, x, y, l, execute: true);
                _ = log.Release();
                if (StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human)
                {
                    ExecuteUpgradeHeroAI();
                }
            }
        }
    }

    // Hero.PAS:1350
    public bool GetUnitAbilityEnabled(int abilityId) => base.GetAbilityEnabled(abilityId);

    // Hero.PAS:1363
    public AbilityOwner GetSuperlativeEnabledAbilityOwner(int abilityId)
    {
        AbilityOwner best = GetOwnAbilityEnabled(abilityId) ? this : (AbilityOwner)null;
        AbilityOwner candidate = HeroItems.GetSuperlativeEnabledAbilityOwner(abilityId);
        if (candidate is not null)
        {
            if (best is null || candidate.GetOwnAbilityLevel(abilityId) > best.GetOwnAbilityLevel(abilityId))
            {
                best = candidate;
            }
        }

        candidate = Inventory.GetSuperlativeEnabledAbilityOwner(abilityId);
        if (candidate is not null)
        {
            if (best is null || candidate.GetOwnAbilityLevel(abilityId) > best.GetOwnAbilityLevel(abilityId))
            {
                best = candidate;
            }
        }

        return best;
    }

    // Hero.PAS:1378
    public AbilityOwner GetSuperlativeAbilityOwner(int abilityId)
    {
        AbilityOwner best = GetAbilitySetAt(abilityId) ? this : (AbilityOwner)null;
        AbilityOwner candidate = HeroItems.GetSuperlativeAbilityOwner(abilityId);
        if (candidate is not null)
        {
            if (best is null || candidate.GetOwnAbilityLevel(abilityId) > best.GetOwnAbilityLevel(abilityId))
            {
                best = candidate;
            }
        }

        candidate = Inventory.GetSuperlativeAbilityOwner(abilityId);
        if (candidate is not null)
        {
            if (best is null || candidate.GetOwnAbilityLevel(abilityId) > best.GetOwnAbilityLevel(abilityId))
            {
                best = candidate;
            }
        }

        return best;
    }

    // Hero.PAS:1544
    public void ResetSkillPoints()
    {
        AssignAbility(HeroResource.Abilities);
        UnitAttack = 0;
        UnitDefense = 0;
        UnitDamage = 0;
        UnitHits = 0;
        UnitMoves = 0;
        UnitResistance = 0;
    }

    // Hero.PAS:1555
    public void ResetResourceSettings()
    {
        AssignAbility(HeroResource.Abilities);
        UnitAttack = 0;
        UnitDefense = 0;
        UnitDamage = 0;
        UnitHits = 0;
        UnitMoves = 0;
        UnitResistance = 0;
        Experience = 0;
        Race = HeroResource.Race;
        if (StrategicMap.Current is not null && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            FaceIndex = HeroResource.FaceIndex;
        }

        Changed();
    }

    // Hero.PAS:1570
    public void UpdateSettings()
    {
        _movePoints = Moves;
        HitPoints = GetHits();
        CastingPoints = GetCastingPointsMax();
        Level = (byte)ExperienceToLevel(Experience);
        Changed();
    }

    // Hero.PAS:1579
    public void SetRace(sbyte value)
    {
        if (Race != value)
        {
            bool found = false;
            int index = GamePack.Current.HeroResources.GetCount();
            while (!found && index != 0)
            {
                index -= 1;
                HeroResource resource = GamePack.Current.HeroResources.GetHeroResource(index);
                found = resource is not null && resource.Race == value && HeroType == resource.HeroType;
            }

            if (found)
            {
                SetHeroResourceIndex(index);
            }
        }
    }

    // Hero.PAS:1612
    public void SetFaceIndex(int value) => FaceIndex = value;

    // Hero.PAS:1708
    public HeroUpdateTokenEvent CreateHeroUpdateTokenEvent()
    {
        return new(null)
        {
            Holder = CreationHolder(),
            UnitId = UnitId
        };
    }

    // Hero.PAS:1714
    public ItemList GetItemsOnGround()
    {
        CombatMap map = StrategicMap.Current.CombatMap;
        return map is not null ? map.GetHeroItemsOnGround(this) : StrategicMap.Current.GetHeroItemsOnGround(this);
    }

    // Hero.PAS:1722
    public bool CanUseItem(int itemId, ref string reason)
    {
        Item item = StrategicMap.Current.ItemControl.FindItem(itemId);
        if (item is not null)
        {
            return item.CanUse(this, ref reason);
        }
        else
        {
            reason = "";
        }

        return false;
    }

    // Hero.PAS:1734
    public bool UseItem(int itemId)
    {
        Item item = StrategicMap.Current.ItemControl.FindItem(itemId);
        return item is not null && item.Use(this);
    }

    // Hero.PAS:1742
    public bool CanPlaceItem(int itemId, HeroItemPosition position)
    {
        Item item = StrategicMap.Current.ItemControl.FindItem(itemId);
        if (item is not null)
        {
            MapCoordinate coordinate43 = GetLocation();
            sbyte x = coordinate43.X;
            sbyte y = coordinate43.Y;
            sbyte l = coordinate43.MapLevel;
            MapCoordinate coordinate44 = item.GetLocation();
            sbyte ix = coordinate44.X;
            sbyte iy = coordinate44.Y;
            sbyte imageLibrary = coordinate44.MapLevel;
            if (StrategicMap.Current.CombatMap is not null || (x == ix && y == iy && l == imageLibrary))
            {
                return Set<HeroItemPosition>.Of().WithRange(HeroItemPosition.AttackSlot, HeroItemPosition.RightRingSlot).Contains(position)
                    ? HeroItems.CanPlaceItem(item, position)
                    : !Set<HeroItemPosition>.Of().WithRange(HeroItemPosition.InventorySlot1, HeroItemPosition.InventorySlot8).Contains(position) || Inventory.GetSlot(position) is null;
            }
        }

        return false;
    }

    // Hero.PAS:1758
    public bool CanSwapItem(int itemId1, int itemId2)
    {
        Item item1 = StrategicMap.Current.ItemControl.FindItem(itemId1);
        Item item2 = StrategicMap.Current.ItemControl.FindItem(itemId2);
        if (item1 is not null && item2 is not null && item1 != item2)
        {
            if (HeroItems == item1.Owner || HeroItems == item2.Owner)
            {
                return item1.ItemType == item2.ItemType;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    // Hero.PAS:1772
    public bool SwapItem(int itemId1, int itemId2)
    {
        if (CanSwapItem(itemId1, itemId2) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            HeroUpdateTokenEvent tokenEvent = CreateHeroUpdateTokenEvent();
            tokenEvent.Mode = HeroUpdateTokenEventMode.SwapItem;
            tokenEvent.ItemId = itemId1;
            tokenEvent.Parameter = itemId2;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:1788
    public bool PlaceItem(int itemId, HeroItemPosition position)
    {
        if (CanPlaceItem(itemId, position) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            HeroUpdateTokenEvent tokenEvent = CreateHeroUpdateTokenEvent();
            tokenEvent.Mode = HeroUpdateTokenEventMode.PlaceItem;
            tokenEvent.ItemId = itemId;
            tokenEvent.Parameter = (int)position;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:1804
    public bool ExecuteSwapItem(int itemId1, int itemId2)
    {
        if (CanSwapItem(itemId1, itemId2))
        {
            Item item1 = StrategicMap.Current.ItemControl.FindItem(itemId1);
            Item item2 = StrategicMap.Current.ItemControl.FindItem(itemId2);
            // An item admits only an item owner. Another owner fails in Of. Null stays null and
            // BeginUpdate fails on it.
            ItemOwner owner1 = ItemOwner.Of(item1.Owner);
            ItemOwner owner2 = ItemOwner.Of(item2.Owner);
            using (owner1.Observable.BeginUpdate())
            using (owner2.Observable.BeginUpdate())
            {
                int index1 = item1.Owner.FindChildIndex(item1);
                int index2 = item2.Owner.FindChildIndex(item2);
                item1.SetOwner(null);
                item2.SetOwner(null);
                if (owner1.Observable.GetControlStyle().Contains(ControlStyleElement.ArrayMode))
                {
                    item2.SetArrayOwner(owner1.Observable, index: index1);
                }
                else
                {
                    item2.SetOwner(owner1.Observable);
                }

                if (owner2.Observable.GetControlStyle().Contains(ControlStyleElement.ArrayMode))
                {
                    item1.SetArrayOwner(owner2.Observable, index: index2);
                }
                else
                {
                    item1.SetOwner(owner2.Observable);
                }
            }
            Changed();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:1831
    public bool AutoPlaceItem(Item item)
    {
        HeroItemPosition position = HeroItems.GetItemTypePosition(item.ItemType);
        if (HeroItems.CanPlaceItem(item, position))
        {
            _ = HeroItems.PlaceItem(item, position);
            return true;
        }
        else
        {
            int slot = Inventory.GetFreeSlot();
            if (slot != -1)
            {
                item.SetArrayOwner(Inventory, index: slot);
                return true;
            }
        }

        return false;
    }

    // Hero.PAS:1852
    public bool ExecutePlaceItem(int itemId, HeroItemPosition position)
    {
        if (CanPlaceItem(itemId, position))
        {
            Item item = StrategicMap.Current.ItemControl.FindItem(itemId);
            switch (position)
            {
                case HeroItemPosition.Ground:
                    CombatMap map = StrategicMap.Current.CombatMap;
                    if (map is not null)
                    {
                        map.DropHeroItem(this, item);
                    }
                    else
                    {
                        StrategicMap.Current.DropHeroItem(this, item);
                    }

                    Changed();
                    break;
                case HeroItemPosition.AttackSlot:
                case HeroItemPosition.HeadSlot:
                case HeroItemPosition.DefenseSlot:
                case HeroItemPosition.LeftRingSlot:
                case HeroItemPosition.TorsoSlot:
                case HeroItemPosition.RightRingSlot:
                    _ = HeroItems.PlaceItem(item, position);
                    Changed();
                    break;
                case HeroItemPosition.InventorySlot1:
                case HeroItemPosition.InventorySlot2:
                case HeroItemPosition.InventorySlot3:
                case HeroItemPosition.InventorySlot4:
                case HeroItemPosition.InventorySlot5:
                case HeroItemPosition.InventorySlot6:
                case HeroItemPosition.InventorySlot7:
                case HeroItemPosition.InventorySlot8:
                    item.SetArrayOwner(Inventory, index: (int)position - 6);
                    Changed();
                    break;
                case HeroItemPosition.None:
                default:
                    break;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:1876
    public override bool ExecuteStartCasting(int spellId)
    {
        if (CastingSpellId != 0)
        {
            _ = ExecuteCancelCasting();
        }

        Spell spell = GamePack.Current.SpellControl[spellId];
        Player players = StrategicMap.Current.Players.GetPlayers(Player);
        int castingMana = CastingMana(spell);
        if (players.MagicControl.Mana >= castingMana && CastingMana(spell) > CastingPoints)
        {
            CastingProgress = CastingMana(spell);
            Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player);
            StrategicMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana - CastingProgress);
            CastingManaValue = CastingPoints;
            CastingPoints = 0;
            if (StrategicMap.Current.SeatedPlayer == Player)
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_castingInProgressFormat), arguments: [spell.SpellName]);
                StrategicMap.Current.ShowMessage(format);
            }

            CastingSpellId = spellId;
            Changed();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:1898
    public void RemoveCastingReadyEventLog()
    {
        EventLogbook logbook = StrategicMap.Current.Players.GetPlayers(Player).EventLogbook;
        for (int i = logbook.GetCount() - 1; i >= 0; i--)
        {
            if (logbook[i] is SpellCastEventLog spellCast && spellCast.UnitId == UnitId)
            {
                spellCast.SetOwner(null);
            }
        }
    }

    // Hero.PAS:1907
    public bool ExecuteCancelCasting()
    {
        if (CastingSpellId != 0)
        {
            RemoveCastingReadyEventLog();
            CastingPoints = GetCastingPointsMax() < CastingPoints + CastingManaValue
                ? GetCastingPointsMax()
                : (sbyte)(CastingPoints + CastingManaValue);

            Player players = StrategicMap.Current.Players.GetPlayers(Player);
            StrategicMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(players.MagicControl.Mana + CastingProgress);
            CastingSpellId = 0;
            CastingManaValue = 0;
            CastingProgress = 0;
            Changed();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:1925
    public void CastingDone(int spellId, int mana)
    {
        if (spellId == CastingSpellId)
        {
            Assert(CastingReady(), message: "", file: "Hero.PAS", line: 2080);
            CastingManaValue = 0;
            CastingProgress = 0;
            CastingSpellId = 0;
            RemoveCastingReadyEventLog();
        }
        else
        {
            if (CastingSpellId != 0)
            {
                _ = ExecuteCancelCasting();
            }

            if (!StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
            {
                Player players = StrategicMap.Current.Players.GetPlayers(Player);
                StrategicMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(players.MagicControl.Mana - mana);
                CastingPoints = (sbyte)(CastingPoints - mana);
            }
        }
    }

    // Hero.PAS:2104
    public int CastingTurns(Spell spell)
    {
        int mana = CastingMana(spell);
        int points = CastingPoints + CastingManaValue;
        if (GetCastingPointsMax() < points)
        {
            points = GetCastingPointsMax();
        }

        int result;
        if (points < mana)
        {
            result = (mana - points) / GetCastingPointsMax();
            if ((mana - points) % GetCastingPointsMax() != 0)
            {
                result += 1;
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // Hero.PAS:2118
    public bool Casting() => CastingSpellId != 0;

    // Hero.PAS:2121
    public bool CastingReady() => CastingSpellId != 0 && CastingManaValue == CastingProgress;

    // Hero.PAS:2124
    public int CastingTurnsLeft()
    {
        int result;
        if (CastingSpellId != 0 && GetCastingPointsMax() != 0)
        {
            if (CastingManaValue < CastingProgress)
            {
                sbyte castingPointsMax = GetCastingPointsMax();
                result = (CastingProgress - CastingManaValue) / castingPointsMax;
                sbyte castingPointsMax2 = GetCastingPointsMax();
                if ((CastingProgress - CastingManaValue) % castingPointsMax2 != 0)
                {
                    result += 1;
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    // Hero.PAS:2136
    public override bool CanCastSpellInstantly(int spellId, int mana, ref string reason)
    {
        if (!StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
        {
            if (mana <= CastingPoints || (spellId == CastingSpellId && Casting() && CastingReady()))
            {
                return CanCastSpell(spellId, ref reason);
            }
            else
            {
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughChannelingPointsText);
            }
        }
        else
        {
            return true;
        }

        return false;
    }

    // Hero.PAS:2149
    public override bool CanCastSpell(int spellId, ref string reason)
    {
        bool result;
        if (CastingSpellId == 0)
        {
            Spell spell = GamePack.Current.SpellControl[spellId];
            Player players = StrategicMap.Current.Players.GetPlayers(Player);
            int castingMana = CastingMana(spell);
            if (players.MagicControl.Mana >= castingMana || StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
            {
                return spell.CanActivate(this, messageText: ref reason);
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughManaText);
            }
        }
        else
        {
            result = spellId == CastingSpellId;
            if (result && GamePack.Current.SpellControl[spellId].SpellType == SpellType.Global && StrategicMap.Current.CombatMap is not null)
            {
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_cannotCastSpellInCombatText);
                return false;
            }
        }

        return result;
    }

    // Hero.PAS:2176
    public bool CastSpell(int spellId, ref string reason)
    {
        if (CanCastSpell(spellId, ref reason))
        {
            Spell spell;
            if (CastingSpellId == 0)
            {
                spell = GamePack.Current.SpellControl[spellId];
                if (CastingMana(spell) <= CastingPoints || StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    return spell.CanActivate(this, messageText: ref reason) && spell.Activate(this);
                }
                else
                {
                    if (!StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(Player);
                        int castingMana = CastingMana(spell);
                        if (players.MagicControl.Mana >= castingMana)
                        {
                            HeroUpdateTokenEvent tokenEvent = CreateHeroUpdateTokenEvent();
                            tokenEvent.Mode = HeroUpdateTokenEventMode.StartCasting;
                            tokenEvent.ItemId = spellId;
                            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
                            _ = tokenEvent.Release();
                            return true;
                        }
                        else
                        {
                            reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughManaText);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (CastingReady())
                {
                    spell = GamePack.Current.SpellControl[spellId];
                    return spell.CanActivate(this, messageText: ref reason) && spell.Activate(this);
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    // Hero.PAS:2220
    public bool CanCastCombatSpell(int spellId, ref string reason)
    {
        bool result;
        Spell spell;
        if (CastingSpellId == 0)
        {
            spell = GamePack.Current.SpellControl[spellId];
            if (CastingMana(spell) <= CastingPoints || StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
            {
                Player players = StrategicMap.Current.Players.GetPlayers(Player);
                int castingMana = CastingMana(spell);
                if (players.MagicControl.Mana >= castingMana || StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    return spell.CanActivateCombat(this, ref reason);
                }
                else
                {
                    result = false;
                    reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughManaText);
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_notEnoughChannelingPointsText);
            }
        }
        else
        {
            spell = GamePack.Current.SpellControl[spellId];
            return Set<SpellType>.Of(SpellType.Combat, SpellType.Unit).Contains(spell.SpellType) && CanCastSpellInstantly(spellId, mana: CastingMana(spell), ref reason);
        }

        return result;
    }

    // Hero.PAS:2252
    public bool CastCombatSpell(int spellId, ref string reason)
    {
        Spell spell = GamePack.Current.SpellControl[spellId];
        return CanCastCombatSpell(spellId, ref reason) && spell.CanActivateCombat(this, ref reason) && spell.ActivateCombat(this);
    }

    // Hero.PAS:2264
    public bool CancelCasting()
    {
        // { Original tests -1 although zero is the idle spell ID. }
        if (CastingSpellId != -1 && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            HeroUpdateTokenEvent tokenEvent = CreateHeroUpdateTokenEvent();
            tokenEvent.Mode = HeroUpdateTokenEventMode.CancelCasting;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_castingInProgressFormat = "Casting %s...";
    private static readonly string s_cannotCastSpellInCombatText = "Cannot cast spell in combat";
    private static readonly string s_notEnoughChannelingPointsText = "Not enough channeling points";
    private static readonly ImmutableArray<HeroLevel> s_heroExperienceTable = [new HeroLevel(DailyExperience: 1, Experience: 15), new HeroLevel(DailyExperience: 1, Experience: 15), new HeroLevel(DailyExperience: 1, Experience: 20), new HeroLevel(DailyExperience: 1, Experience: 20), new HeroLevel(DailyExperience: 1, Experience: 25), new HeroLevel(DailyExperience: 1, Experience: 25)];

    internal static ReadOnlySpan<sbyte> MoraleResistance => [-2, -1, 0, 0, 0];
    internal static ReadOnlySpan<sbyte> MoraleDefense => [-2, -1, 0, 0, 0];

}
