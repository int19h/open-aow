// Translated from AoWE.pas.
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// UpgradePending. HeroResource. HeroResourceIndex. Experience. _usedSkillPoints. DeathDay.
// DeathPlayer. Level. HeroStatus. HeroName. Nickname. Race. Gender. unit statistics.
// CastSpellInstantly. MovePoints. HitPoints. PowerSource. CastingPoints. CastingManaValue.
// CastingProgress. LibraryId. LibraryName. Registered
public partial class Hero : Troop
{
    public new const int ClassId = 131632;

    public int FaceIndex;

    public HeroResource HeroResource { get; set; }

    public int HeroResourceIndex;
    public int Experience;
    public byte Level;
    private int _usedSkillPoints;
    public bool UpgradePending;
    public Set<HeroStatusElement> HeroStatus;
    private int _deathDay;
    private sbyte _deathPlayer;

    public Set<HeroModeElement> Mode { get; set; }

    public string HeroName = "";
    public string Nickname = "";
    public sbyte Race;
    private UnitGender _gender;
    public sbyte UnitAttack;
    public sbyte UnitDefense;
    public sbyte UnitDamage;
    public sbyte UnitHits;
    public sbyte UnitMoves;
    public sbyte UnitResistance;

    public HeroItems HeroItems { get; }
    public HeroInventory Inventory { get; }

    private bool _castSpellInstantly;
    private sbyte _movePoints;
    public sbyte HitPoints;

    public HeroPowerSource PowerSource { get; }

    public sbyte CastingPoints;
    public int CastingSpellId;
    public int CastingManaValue;
    public int CastingProgress;
    public int LibraryId;
    public string LibraryName = "";

    public bool Registered { get; set; }

    // Hero.PAS:631
    public override void Dispose()
    {
        AddRef();
        AddRef();
        AddRef();
        // { Original repeats the reference increment four times. }
        AddRef();
        if (Registered && AoWHSMap.Current != null)
        {
            AoWHSMap.Current.HeroControl.UnregisterHero(this);
        }

        base.Dispose();
        HeroItems?.Dispose();
        Inventory?.Dispose();
        PowerSource?.Dispose();
    }

    // Hero.PAS:683
    public override void SetOwner(ECustomNode owner)
    {
        AddRef();
        base.SetOwner(owner);
        if (UnitStatus.Contains(UnitStatusElement.Activated))
        {
            Changed();
        }

        Release();
    }

    // Hero.PAS:700
    public override GameEngine GetEngine() => GameTables.GameEngine;

    // Hero.PAS:1625
    public override void ReadWrite(EStorageStream stream)
    {
        sbyte player = default;
        int oldExperience = default;
        player = Player;
        stream.rwInteger(ref HeroResourceIndex, 8);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            LinkToResource();
        }

        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && Mode.Contains(HeroModeElement.UpdatingFromLibrary))
        {
            Player = player;
        }

        stream.rwInteger(ref FaceIndex, 7);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            stream.rwInteger(ref oldExperience, 9);
        }
        else
        {
            oldExperience = 0;
        }

        stream.rwString(ref HeroName, 10);
        stream.rwString(ref Nickname, 11);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Race), 12);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref CastingPoints), 13);
        stream.rwInteger(ref CastingSpellId, 14);
        stream.rwData(Bytes.Of(ref _gender, 1), 1, 15);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref UnitAttack), 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref UnitDefense), 17);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref UnitDamage), 18);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref UnitHits), 19);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref UnitMoves), 20);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref UnitResistance), 21);
        stream.rwByte(ref Level, 22);
        stream.rwBool(ref UpgradePending, 23);
        stream.rwInteger(ref Experience, 24);
        RwSet(ref HeroStatus, 1, 25, stream.rwData, 0);
        stream.rwInteger(ref _deathDay, 26);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _deathPlayer), 27);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && Experience == 0)
        {
            Experience = LevelToExperience((int)(((uint)oldExperience) >> 5) + 1);
        }

        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _movePoints), 29);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HitPoints), 30);
        stream.rwInteger(ref CastingManaValue, 31);
        stream.rwBool(ref _castSpellInstantly, 33);
        stream.rwInteger(ref CastingProgress, 34);
        stream.rwIntegerEx(ref LibraryId, -1, 35);
        if (!Mode.Contains(HeroModeElement.UpdatingFromLibrary))
        {
            stream.rwEObject(Inventory, 36);
            stream.rwEObject(HeroItems, 37);
        }

        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref LibraryName, 38);
        }

        stream.rwInteger(ref _usedSkillPoints, 39);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
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
            if (AoWHSMap.Current.Players.GetCount() <= Player && Owner == null)
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
    public override void SetAbilitySet(int index, byte value)
    {
        base.SetAbilitySet(index, value);
    }

    // Hero.PAS:697
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of(AbilitySelectionType.Editor);

    // Hero.PAS:716
    public override void Changed()
    {
        base.Changed();
        if (AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            AoWHSMap.Current.HeroControl.HeroChanged(this);
        }
    }

    // Hero.PAS:930
    public override byte GetUpkeep() => (byte)(GetLevel() * 2 + 5);

    // Hero.PAS:680
    public override sbyte GetUnitLevel() => (sbyte)Level;

    // Hero.PAS:818
    public override sbyte GetRace() => HeroResource.Race;

    // Hero.PAS:1426
    public override sbyte GetInherentAttack() => (sbyte)(HeroResource.Attack + UnitAttack);

    // Hero.PAS:1445
    public override sbyte GetInherentDefense() => (sbyte)(HeroResource.Defense + UnitDefense);

    // Hero.PAS:1464
    public override sbyte GetInherentDamage() => (sbyte)(HeroResource.Damage + UnitDamage);

    // Hero.PAS:1483
    public override sbyte GetInherentResistance() => (sbyte)(HeroResource.Resistance + UnitResistance);

    // Hero.PAS:1429
    public override sbyte GetAttack()
    {
        AbilityOwner owner;
        int count;
        count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= count; I++)
        {
            owner = GetSuperlativeEnabledAbilityOwner(I);
            if (owner != null)
            {
                result = (sbyte)(result + owner.GetAbilityAttack(I));
            }
        }

        result = (sbyte)(HeroItems.GetAttack() + (UnitAttack + result + HeroResource.Attack));
        if (result < 1)
        {
            result = 1;
        }

        if (result > 10)
        {
            result = 10;
        }

        return result;
    }

    // Hero.PAS:1448
    public override sbyte GetDefense()
    {
        AbilityOwner owner;
        int count;
        count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= count; I++)
        {
            owner = GetSuperlativeEnabledAbilityOwner(I);
            if (owner != null)
            {
                result = (sbyte)(result + owner.GetAbilityDefense(I));
            }
        }

        var defense = HeroItems.GetDefense();
        var morale = GetMorale();
        result = (sbyte)(defense + (UnitDefense + result + HeroResource.Defense) + MoraleDefense[(int)morale]);
        if (result < 1)
        {
            result = 1;
        }

        if (result > 10)
        {
            result = 10;
        }

        return result;
    }

    // Hero.PAS:1467
    public override sbyte GetDamage()
    {
        AbilityOwner owner;
        int count;
        count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= count; I++)
        {
            owner = GetSuperlativeEnabledAbilityOwner(I);
            if (owner != null)
            {
                result = (sbyte)(result + owner.GetAbilityDamage(I));
            }
        }

        result = (sbyte)(HeroItems.GetDamage() + (UnitDamage + result + HeroResource.Damage));
        if (result < 1)
        {
            result = 1;
        }

        if (result > 10)
        {
            result = 10;
        }

        return result;
    }

    // Hero.PAS:1486
    public override sbyte GetResistance()
    {
        AbilityOwner owner;
        int count;
        count = GetAbilityCount();
        sbyte result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= count; I++)
        {
            owner = GetSuperlativeEnabledAbilityOwner(I);
            if (owner != null)
            {
                result = (sbyte)(result + owner.GetAbilityResistance(I));
            }
        }

        var resistance = HeroItems.GetResistance();
        var morale = GetMorale();
        result = (sbyte)(resistance + (UnitResistance + result + HeroResource.Resistance) + MoraleResistance[(int)morale]);
        if (result < 1)
        {
            result = 1;
        }

        if (result > 10)
        {
            result = 10;
        }

        return result;
    }

    // Hero.PAS:1502
    public override sbyte GetHits()
    {
        sbyte result = (sbyte)(HeroResource.Hits + UnitHits);
        if (result < 1)
        {
            result = 1;
        }

        if (result > 30)
        {
            result = 30;
        }

        return result;
    }

    // Hero.PAS:1509
    public override sbyte GetMoves()
    {
        sbyte result = (sbyte)(HeroResource.Moves + UnitMoves);
        if (result < 1)
        {
            result = 1;
        }

        if (result > 50)
        {
            result = 50;
        }

        return result;
    }

    // Hero.PAS:842
    public override sbyte GetMovePoints() => _movePoints;

    // Hero.PAS:845
    public override void SetMovePoints(sbyte value)
    {
        if (GetMoves() < value)
        {
            value = GetMoves();
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
        LibraryImage result;
        FaceResource resource;
        int index;
        if (!GetAbilityEnabled(126))
        {
            resource = AoWHSSet.Current.FaceResources.GetFaceResource(FaceIndex);
            if (resource != null)
            {
                result = resource.GetCellImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            index = ((CosmeticSurgeryEnchantment)((UnitEnchantmentAbilityData)GetAbilityData(126)).Enchantment).FaceIndex;
            result = AoWHSSet.Current.FaceResources.GetFaceResource(index).GetCellImage();
        }

        return result;
    }

    // Hero.PAS:821
    public override string GetName() => HeroName;

    // Hero.PAS:862
    public override Alignment GetAlignment()
    {
        Alignment result;
        sbyte race;
        if (GetAbilityEnabled(107))
        {
            result = Alignment.PureEvil;
        }
        else
        {
            race = Race;
            if (AoWHSSet.Current.RaceResources.GetRaceResource(race) != null)
            {
                result = AoWHSSet.Current.RaceResources.GetRaceResource(race).GetAlignment();
            }
            else
            {
                result = HeroResource.Alignment;
            }
        }

        return result;
    }

    // Hero.PAS:1538
    public override LibraryImage GetPreviewImage()
    {
        LibraryImage result;
        if (HeroResource != null)
        {
            result = HeroResource.GetPreviewImage();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:732
    public override UnitGender GetGender() => _gender;

    // Hero.PAS:735
    public override UnitType GetUnitType() => UnitType.Humanoid;

    // Hero.PAS:1003
    public override int GetUnitGraphicsResourceIndex() => HeroResource.UnitGraphicsResourceIndex;

    // Hero.PAS:644
    public override int GetCampaignTransferPoints()
    {
        int result = base.GetCampaignTransferPoints();
        return result / 2;
    }

    // Hero.PAS:1522
    public override sbyte GetCastingPointsMax()
    {
        sbyte result;
        if (GetAbilityEnabled(52))
        {
            result = (sbyte)(GetAbilityLevel(52) * 10);
        }
        else
        {
            result = 0;
        }

        return result;
    }

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
    public override int GetPowerGeneration()
    {
        int result;
        if (GetAbilityEnabled(52))
        {
            result = GetAbilityLevel(52) * 5;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Hero.PAS:738
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.Day == 1)
        {
            if ((GameTables.GameEngine.Campaign == null || AoWHSMap.Current.Players.GetPlayers(Player) == null || AoWHSMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human) && (!AoWHSMap.Current.LeadersFixed || !(this is Leader)))
            {
                _usedSkillPoints = GetSkillPoints();
            }

            UpdateSettings();
        }
    }

    // Hero.PAS:1311
    public override void NewTurn(sbyte player)
    {
        SpellCastEventLog log;
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
                    log = new SpellCastEventLog(null);
                    log._player = Player;
                    log.UnitId = UnitId;
                    log.SpellId = CastingSpellId;
                    AoWHSMap.Current.Players.GetPlayers(Player).EventLogbook.AddEvent(log);
                    if (AoWHSMap.Current.SeatedPlayer == Player && AoWHSMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Human)
                    {
                        log.Execute(false, ExecuteTrigger.Event, null);
                    }

                    log.Release();
                }
            }
        }
    }

    // Hero.PAS:1418
    public override int GetAbilityLevel(int abilityId)
    {
        AbilityOwner owner;
        owner = GetSuperlativeAbilityOwner(abilityId);
        if (owner == null)
        {
            owner = this;
        }

        return AoWHSSet.Current.AbilityControl.GetAbility(abilityId).GetLevel(owner);
    }

    // Hero.PAS:1396
    public override bool GetAbilityEnabled(int abilityId) => base.GetAbilityEnabled(abilityId) || HeroItems.GetAbilityEnabled(abilityId) || Inventory.GetAbilityEnabled(abilityId);

    // Hero.PAS:1403
    public override bool GetAbilitySet(int abilityId) => base.GetAbilitySet(abilityId) || HeroItems.GetAbilitySet(abilityId) || Inventory.GetAbilitySet(abilityId);

    // Hero.PAS:1410
    public override string GetAbilityName(int abilityId)
    {
        AbilityOwner owner;
        owner = GetSuperlativeAbilityOwner(abilityId);
        if (owner == null)
        {
            owner = this;
        }

        return AoWHSSet.Current.AbilityControl.GetAbility(abilityId).GetName(owner);
    }

    // Hero.PAS:1393
    public override AbilityOwner GetAbilityOwner(int abilityId) => GetSuperlativeAbilityOwner(abilityId);

    // Hero.PAS:1353
    public override int GetAbilityCount()
    {
        int count;
        int result = GetAbCount();
        count = HeroItems.GetAbilityCount();
        if (count > result)
        {
            result = count;
        }

        count = Inventory.GetAbilityCount();
        if (count > result)
        {
            result = count;
        }

        return result;
    }

    // Hero.PAS:1028
    public override int GetExperience() => Experience;

    // Hero.PAS:1031
    public override void SetExperience(int value)
    {
        int maximum;
        Experience = value;
        maximum = LevelToExperience(GetLevelMax());
        if (maximum < Experience)
        {
            Experience = maximum;
        }

        Changed();
    }

    // Hero.PAS:1063
    public override int GetNextLevelExperience()
    {
        int result;
        int level;
        int maximum;
        level = GetLevel();
        maximum = GetLevelMax();
        if (level < maximum)
        {
            result = LevelToExperience(level + 1);
        }
        else
        {
            result = LevelToExperience(level);
        }

        return result;
    }

    // Hero.PAS:729
    public override StringCollection GetDescription() => null;

    // Hero.PAS:933
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            bool condition = Player != -1;
            if (condition)
            {
                var count = AoWHSMap.Current.Players.Count;
                condition = Player < count;
            }

            if (condition)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(PowerSource);
                AoWHSMap.Current.Players.GetPlayers(Player).Heroes.RemoveChild(this);
            }

            base.SetPlayer(player);
            if (Player != -1)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).Heroes.AddChild(this);
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(PowerSource);
                AoWHSMap.Current.Players.GetPlayers(Player).ResetBaseHeroJoinDay();
            }
        }
    }

    // Hero.PAS:726
    public override int GetObtainValue() => GetLevel() * 50 + 75;

    // Hero.PAS:914
    public override bool CanActivate()
    {
        Hero hero;
        bool result = base.CanActivate();
        if (result && !Registered && UnitId != -1)
        {
            hero = AoWHSMap.Current.HeroControl.FindId(UnitId);
            if (hero == null || hero == this)
            {
                result = true;
            }
            else
            {
                if (hero.Owner == null && CompatibleHero(hero))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
        }

        return result;
    }

    // Hero.PAS:964
    public override void Activate()
    {
        Hero hero;
        if (UnitId != -1)
        {
            hero = AoWHSMap.Current.HeroControl.FindId(UnitId);
            if (hero != null && hero != this && hero.Owner == null && CompatibleHero(hero))
            {
                AoWHSMap.Current.HeroControl.UnregisterHero(hero);
            }
        }

        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Level = (byte)GetLevel();
        }

        if (HeroResourceIndex == -1)
        {
            SetHeroResourceIndex(0);
        }

        if (Player > 0 && AoWHSMap.Current.Players.GetPlayers(Player) == null && Owner == null)
        {
            Player = -1;
        }

        if (Owner == null && GroupId != 0)
        {
            GroupId = 0;
        }

        base.Activate();
        if (AoWHSMap.Current != null)
        {
            if (!Mode.Contains(HeroModeElement.SkipLibraryUpdate) && (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || AoWHSMap.Current.Day == 0))
            {
                UpdateFromLibrary();
            }

            if (!Registered)
            {
                AoWHSMap.Current.HeroControl.RegisterHero(this);
            }

            if (Player != -1 && Owner != null)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(PowerSource);
                AoWHSMap.Current.Players.GetPlayers(Player).Heroes.AddChild(this);
            }

            Inventory.RegisterItems();
            HeroItems.RegisterItems();
        }
    }

    // Hero.PAS:993
    public override void Deactivate()
    {
        bool condition = Player != -1;
        if (condition)
        {
            var count = AoWHSMap.Current.Players.Count;
            condition = Player < count;
        }

        if (condition)
        {
            AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(PowerSource);
            AoWHSMap.Current.Players.GetPlayers(Player).Heroes.RemoveChild(this);
        }

        base.Deactivate();
    }

    // Hero.PAS:765
    public override bool CanJoin(sbyte player)
    {
        bool result;
        Player playerLocal;
        if (Race != -1)
        {
            playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
            var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
            result = Set<Status>.Of(Status.Polite, Status.Friendly).Contains(playerRelation);
            if (result)
            {
                bool condition = Group == null || Group.GroupClass.Behavior != GroupBehavior.Refuge;
                if (condition)
                {
                    bool test = !GetAbilityEnabled(128) || player != _deathPlayer;
                    if (!test)
                    {
                        var players = AoWHSMap.Current.Players.GetPlayers(player);
                        test = _deathDay + 2 < players.Day;
                    }

                    condition = test;
                }

                if (condition)
                {
                    result = result && playerLocal.Heroes.Count < playerLocal.GetHeroCountMax() && GetLevel() <= playerLocal.GetHeroJoinLevelMax();
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:794
    public override bool OfferToJoin(sbyte player)
    {
        bool result;
        Player playerLocal;
        int seed;
        int chance;
        if (CanJoin(player))
        {
            playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
            if (EmergedToPlayer() == -1)
            {
                if ((!GetAbilityEnabled(128) || player != _deathPlayer) && (Group == null || Group.GroupClass.Behavior != GroupBehavior.Refuge))
                {
                    chance = playerLocal.GetHeroJoinChance(GetLevel());
                    seed = RandSeed;
                    RandSeed = (AoWHSMap.Current.InitialRandomSeed + AoWHSMap.Current.Day + UnitId * 16) & 4088;
                    result = Random(100) < chance;
                    RandSeed = seed;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = player == EmergedToPlayer();
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:706
    public override int Show(int X, int Y, in LockedSurface lockedSurface, in UnitShowInfo info)
    {
        int result = base.Show(X, Y, lockedSurface, info);
        if (info.Overlays.Contains(UnitOverlayModesElement.Rank))
        {
            GameTables.GameEngine.CombatFont.Color = 16777215;
            var level = GetLevel();
            GameTables.GameEngine.CombatFont.SetTextCentered(X + 43, Y + 12, IntToStr(level), lockedSurface);
        }

        return result;
    }

    // Hero.PAS:885
    public override void Killed()
    {
        HeroItemPosition position;
        for (int I = Inventory.Count - 1; I >= 0; I--)
        {
            if (Inventory.GetItem(I) != null)
            {
                ExecutePlaceItem(Inventory.GetItem(I).ItemId, HeroItemPosition.Ground);
            }
        }

        for (int positionOrdinal = (int)HeroItemPosition.AttackSlot; positionOrdinal <= (int)HeroItemPosition.RightRingSlot; positionOrdinal++)
        {
            position = (HeroItemPosition)positionOrdinal;
            if (HeroItems.GetPositionItem(position) != null)
            {
                ExecutePlaceItem(HeroItems.GetPositionItem(position).ItemId, HeroItemPosition.Ground);
            }
        }

        _deathDay = AoWHSMap.Current.Players.GetPlayers(Player).Day;
        _deathPlayer = Player;
        base.Killed();
        HeroStatus = HeroStatus + Set<HeroStatusElement>.Of(HeroStatusElement.Died);
        SetPlayer(-1);
        SetOwner(null);
    }

    // Hero.PAS:902
    public override void Resurrect()
    {
        base.Resurrect();
        HeroStatus = HeroStatus - Set<HeroStatusElement>.Of(HeroStatusElement.Died);
    }

    // Hero.PAS:908
    public override void Animate()
    {
        base.Animate();
        HeroStatus = HeroStatus - Set<HeroStatusElement>.Of(HeroStatusElement.Died);
    }

    // Hero.PAS:691
    public virtual string GetNickname() => Nickname;

    // Hero.PAS:694
    public virtual void SetNickname(string value)
    {
        Nickname = value;
    }

    // Hero.PAS:1615
    public virtual void SetHeroResourceIndex(int value)
    {
        if (AoWHSSet.Current.HeroResources.GetHeroResource(value) != null && value != HeroResourceIndex)
        {
            HeroResourceIndex = value;
            HeroResource = AoWHSSet.Current.HeroResources.GetHeroResource(value);
            ResetResourceSettings();
        }
    }

    // Hero.PAS:1674
    public virtual void EditDropMessage(ref EngineMessage message)
    {
        MapLevel level;
        int L;
        int X = default;
        int Y = default;
        int player;
        bool placed;
        level = message.Param[0].As<MapLevel>();
        L = message.Data[1];
        HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], ref X, ref Y);
        if (X >= 0 && Y >= 0 && X < level.Width && Y < level.Height)
        {
            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                player = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetPlayer();
                if (player == -1)
                {
                    player = AoWHSMap.Current.EditorPlayer;
                }
            }
            else
            {
                player = -1;
            }

            placed = Place(X, Y, L, (sbyte)player, Set<UnitPlaceModeElement>.Of()) != null;
        }
        else
        {
            placed = false;
        }

        if (placed)
        {
            message.Result = 0;
        }
        else
        {
            message.Result = -1;
        }
    }

    // Hero.PAS:650
    public string GetLibraryName()
    {
        string result = LibraryName;
        if (IsEmpty(result))
        {
            result = AbilityRules.TranslateResourceString(GameText.SystemText);
        }

        return result;
    }

    // Hero.PAS:656
    public void UpdateFromLibraryBegin()
    {
        Mode = Mode + Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
    }

    // Hero.PAS:659
    public void UpdateFromLibraryEnd()
    {
        Mode = Mode - Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
    }

    // Hero.PAS:662
    public void UpdateFromLibrary()
    {
        Hero hero;
        int id;
        if (LibraryId != -1)
        {
            hero = AoWHSSet.Current.HeroLibraries.FindHero(LibraryName, LibraryId);
            if (hero != null)
            {
                id = UnitId;
                Mode = Mode + Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
                Assign(hero, GameTables.GameEngine);
                Mode = Mode - Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
                UnitId = id;
                Changed();
            }
        }
    }

    // Hero.PAS:703
    public bool GetLibraryHero() => LibraryId != -1;

    // Hero.PAS:751
    public int UsedSkillPoints()
    {
        int result = UnitAttack * 5 + UnitDefense * 5 + UnitDamage * 10 + UnitResistance * 5 + UnitHits * 5 + UnitMoves * 2;
        for (int I = GetAbCount() - 1; I >= 0; I--)
        {
            if (GetAbilitySetAt(I))
            {
                result += AoWHSSet.Current.AbilityControl.GetAbility(I).GetSkillPoints(this);
                if (HeroResource.Abilities.GetAbilitySetAt(I))
                {
                    result -= AoWHSSet.Current.AbilityControl.GetAbility(I).GetSkillPoints(HeroResource.Abilities);
                }
            }
        }

        return result;
    }

    // Hero.PAS:782
    public sbyte EmergedToPlayer()
    {
        int I;
        I = AoWHSMap.Current.Players.Count;
        sbyte result = -1;
        while (I != 1 && result == -1)
        {
            I -= 1;
            if (AoWHSMap.Current.Players.GetPlayers(I).EmergingHeroId == UnitId)
            {
                result = (sbyte)I;
            }
        }

        return result;
    }

    // Hero.PAS:875
    public void DropItemsOnMap(AoWHSMap map, sbyte X, sbyte Y, sbyte L)
    {
        HeroItemPosition position;
        for (int I = Inventory.Count - 1; I >= 0; I--)
        {
            if (Inventory.GetItem(I) != null)
            {
                Inventory.GetItem(I).PlaceOnMap(map, X, Y, L);
            }
        }

        for (int positionOrdinal = (int)HeroItemPosition.AttackSlot; positionOrdinal <= (int)HeroItemPosition.RightRingSlot; positionOrdinal++)
        {
            position = (HeroItemPosition)positionOrdinal;
            if (HeroItems.GetPositionItem(position) != null)
            {
                HeroItems.GetPositionItem(position).PlaceOnMap(map, X, Y, L);
            }
        }
    }

    // Hero.PAS:952
    public bool CompatibleHero(Hero hero)
    {
        int groupId;
        int otherGroupId;
        groupId = GroupId;
        otherGroupId = hero.GroupId;
        hero.GroupId = 0;
        GroupId = 0;
        bool result = hero.Compare(this, GameTables.GameEngine);
        hero.GroupId = otherGroupId;
        GroupId = groupId;
        return result;
    }

    // Hero.PAS:1006
    public int GetSkillPoints()
    {
        int result;
        if (AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            var usedSkillPoints = UsedSkillPoints();
            result = Level * 10 + 15 - usedSkillPoints - _usedSkillPoints;
        }
        else
        {
            var usedSkillPoints = UsedSkillPoints();
            result = Level * 10 + 15 - usedSkillPoints;
        }

        return result;
    }

    // Hero.PAS:1013
    public int GetSkillPointsMax() => Level * 10 + 15 - _usedSkillPoints;

    // Hero.PAS:1016
    public int GetLevelMax()
    {
        int maximum;
        int result = 30;
        if (AoWHSMap.Current != null && AoWHSMap.Current.CampaignSettings != null && AoWHSMap.Current.CampaignSettings.HeroLevelMax > 0)
        {
            maximum = AoWHSMap.Current.CampaignSettings.HeroLevelMax;
            if (maximum < result)
            {
                result = maximum;
            }
        }

        return result;
    }

    // Hero.PAS:1040
    public int LevelToExperience(int level)
    {
        int total;
        int quotient;
        total = 0;
        for (int I = 1; I <= level - 1; I++)
        {
            quotient = I / 5;
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
        int level;
        int total;
        int maximum;
        level = 1;
        total = 0;
        maximum = GetLevelMax();
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
    public sbyte GetLevel() => this.ExperienceToLevel(this.Experience);

    // Hero.PAS:1076
    public void SetLevel(sbyte value)
    {
        int maximum;
        int level;
        int points;
        maximum = GetLevelMax();
        if (value < 1)
        {
            value = 1;
        }

        if (maximum < value)
        {
            value = (sbyte)maximum;
        }

        level = GetLevel();
        if (level > value)
        {
            points = GetSkillPoints();
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
    public HeroType GetHeroType() => HeroResource.HeroType;

    // Hero.PAS:1100
    public void SetUnitAttack(sbyte value)
    {
        int points;
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
            points = GetSkillPoints();
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
        int points;
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
            points = GetSkillPoints();
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
        int points;
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
            points = GetSkillPoints();
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
        int points;
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
            points = GetSkillPoints();
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
        int points;
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
            points = GetSkillPoints();
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
        int points;
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
            points = GetSkillPoints();
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
    public void LinkToResource()
    {
        HeroResource = AoWHSSet.Current.HeroResources.GetHeroResource(HeroResourceIndex);
    }

    // Hero.PAS:1207
    public void Loaded()
    {
        if (HeroResource != null)
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
    public void ListSpells(ref SpellList list)
    {
        AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.ListSpells(ref list, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
    }

    // Hero.PAS:1226
    public void ExecuteUpgradeHeroAI()
    {
        DoubleItemList choices;
        int points;
        int index;
        DoubleItemList.DoubleItem choice;
        choices = new DoubleItemList();
        while (true)
        {
            choices.Clear();
            points = GetSkillPoints();
            if (points >= 5 && UnitAttack + HeroResource.Attack < 10)
            {
                choices.AddAbility(0, 10);
            }

            if (points >= 5 && UnitDefense + HeroResource.Defense < 10)
            {
                choices.AddAbility(1, 25);
            }

            if (points >= 5 && UnitResistance + HeroResource.Resistance < 10)
            {
                choices.AddAbility(2, 10);
            }

            if (points >= 10 && UnitDamage + HeroResource.Damage < 10)
            {
                choices.AddAbility(3, 10);
            }

            if (points >= 5 && UnitHits + HeroResource.Hits < 30)
            {
                choices.AddAbility(4, 10);
            }

            if (AoWHSSet.Current.AbilityControl.GetAbility(46).CanExpand(this) && points >= AoWHSSet.Current.AbilityControl.GetAbility(46).ExpandCost(this))
            {
                choices.AddAbility(5, 10);
            }

            if (AoWHSSet.Current.AbilityControl.GetAbility(22).CanExpand(this) && points >= AoWHSSet.Current.AbilityControl.GetAbility(22).ExpandCost(this))
            {
                choices.AddAbility(6, 10);
            }

            if (AoWHSSet.Current.AbilityControl.GetAbility(52).CanExpand(this))
            {
                if (points >= AoWHSSet.Current.AbilityControl.GetAbility(52).ExpandCost(this))
                {
                    choices.AddAbility(7, 9999);
                }
                else
                {
                    choices.AddAbility(7, 100);
                }
            }

            if (choices.Count != 0)
            {
                RandSeed = AoWHSMap.Current.Random(16777215);
                index = choices.RandomChanceItemB(0);
                choice = choices[index];
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
                        ExpandAbility(46);
                        break;
                    case 6:
                        ExpandAbility(22);
                        break;
                    case 7:
                        if (points >= AoWHSSet.Current.AbilityControl.GetAbility(52).ExpandCost(this))
                        {
                            ExpandAbility(52);
                        }
                        else
                        {
                            points = 0;
                        }

                        break;
                }
            }

            if (choices.Count == 0 || GetSkillPoints() == 0 || points == 0)
                break;
        }

        choices?.Dispose();
    }

    // Hero.PAS:1269
    public void ValidateHeroUpgrade()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        int level;
        int oldLevel;
        HeroUpgradeEventLog log;
        GetLocation(ref X, ref Y, ref L);
        if (Player != 0 && X != -1)
        {
            UpgradePending = false;
            level = GetLevel();
            if (level < GetLevelMax())
            {
                if (Player > 0 && AoWHSMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu2)
                {
                    SetExperience(GetExperience() + (s_heroExperienceTable[level / 5].DailyExperience + 1));
                }
                else
                {
                    if (Player > 0 && AoWHSMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu3)
                    {
                        SetExperience(GetExperience() + (s_heroExperienceTable[level / 5].DailyExperience + 2));
                    }
                    else
                    {
                        if (Player > 0 && AoWHSMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu4)
                        {
                            SetExperience(GetExperience() + (s_heroExperienceTable[level / 5].DailyExperience + 3));
                        }
                        else
                        {
                            if (Player > 0 && AoWHSMap.Current.Players.GetPlayers(Player).PlayerType == PlayerType.Cpu5)
                            {
                                SetExperience(GetExperience() + (s_heroExperienceTable[level / 5].DailyExperience + 4));
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
                oldLevel = Level;
                this.Level = (byte)level;
                log = new HeroUpgradeEventLog(null);
                log.OldLevel = (byte)oldLevel;
                log.NewLevel = Level;
                log._day = AoWHSMap.Current.Players.GetPlayers(Player).Day;
                log._player = Player;
                log.UnitId = UnitId;
                EventLog.DistributeLocationEventLog(log, Player, X, Y, L, true);
                log.Release();
                if (AoWHSMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human)
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
        AbilityOwner candidate;
        AbilityOwner best;
        if (GetAbEnabled(abilityId))
        {
            best = this;
        }
        else
        {
            best = null;
        }

        candidate = HeroItems.GetSuperlativeEnabledAbilityOwner(abilityId);
        if (candidate != null)
        {
            if (best == null || candidate.GetAbLevel(abilityId) > best.GetAbLevel(abilityId))
            {
                best = candidate;
            }
        }

        candidate = Inventory.GetSuperlativeEnabledAbilityOwner(abilityId);
        if (candidate != null)
        {
            if (best == null || candidate.GetAbLevel(abilityId) > best.GetAbLevel(abilityId))
            {
                best = candidate;
            }
        }

        return best;
    }

    // Hero.PAS:1378
    public AbilityOwner GetSuperlativeAbilityOwner(int abilityId)
    {
        AbilityOwner candidate;
        AbilityOwner best;
        if (GetAbilitySetAt(abilityId))
        {
            best = this;
        }
        else
        {
            best = null;
        }

        candidate = HeroItems.GetSuperlativeAbilityOwner(abilityId);
        if (candidate != null)
        {
            if (best == null || candidate.GetAbLevel(abilityId) > best.GetAbLevel(abilityId))
            {
                best = candidate;
            }
        }

        candidate = Inventory.GetSuperlativeAbilityOwner(abilityId);
        if (candidate != null)
        {
            if (best == null || candidate.GetAbLevel(abilityId) > best.GetAbLevel(abilityId))
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
        if (AoWHSMap.Current != null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            FaceIndex = HeroResource.FaceIndex;
        }

        Changed();
    }

    // Hero.PAS:1570
    public void UpdateSettings()
    {
        _movePoints = GetMoves();
        HitPoints = GetHits();
        CastingPoints = GetCastingPointsMax();
        Level = (byte)ExperienceToLevel(Experience);
        Changed();
    }

    // Hero.PAS:1579
    public void SetRace(sbyte value)
    {
        bool found;
        int index;
        HeroResource resource;
        if (Race != value)
        {
            found = false;
            index = AoWHSSet.Current.HeroResources.GetCount();
            while (!found && index != 0)
            {
                index -= 1;
                resource = AoWHSSet.Current.HeroResources.GetHeroResource(index);
                found = resource != null && resource.Race == value && GetHeroType() == resource.HeroType;
            }

            if (found)
            {
                SetHeroResourceIndex(index);
            }
        }
    }

    // Hero.PAS:1612
    public void SetFaceIndex(int value)
    {
        FaceIndex = value;
    }

    // Hero.PAS:1708
    public HeroUpdateTokenEvent CreateHeroUpdateTokenEvent()
    {
        HeroUpdateTokenEvent result = new HeroUpdateTokenEvent(null);
        result.UnitId = UnitId;
        return result;
    }

    // Hero.PAS:1714
    public ItemList GetItemsOnGround()
    {
        ItemList result;
        CustomCombatMap map;
        map = AoWHSMap.Current.CombatMap;
        if (map != null)
        {
            result = map.GetHeroItemsOnGround(this);
        }
        else
        {
            result = AoWHSMap.Current.GetHeroItemsOnGround(this);
        }

        return result;
    }

    // Hero.PAS:1722
    public bool CanUseItem(int itemId, ref string reason)
    {
        bool result;
        Item item;
        item = AoWHSMap.Current.ItemControl.FindItem(itemId);
        if (item != null)
        {
            result = item.CanUse(this, ref reason);
        }
        else
        {
            result = false;
            reason = "";
        }

        return result;
    }

    // Hero.PAS:1734
    public bool UseItem(int itemId)
    {
        bool result;
        Item item;
        item = AoWHSMap.Current.ItemControl.FindItem(itemId);
        if (item != null)
        {
            result = item.Use(this);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1742
    public bool CanPlaceItem(int itemId, HeroItemPosition position)
    {
        Item item;
        sbyte ix = default;
        sbyte iy = default;
        sbyte imageLibrary = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool result = false;
        item = AoWHSMap.Current.ItemControl.FindItem(itemId);
        if (item != null)
        {
            GetLocation(ref X, ref Y, ref L);
            item.GetLocation(ref ix, ref iy, ref imageLibrary);
            if (AoWHSMap.Current.CombatMap != null || (X == ix && Y == iy && L == imageLibrary))
            {
                if (Set<HeroItemPosition>.Of().WithRange(HeroItemPosition.AttackSlot, HeroItemPosition.RightRingSlot).Contains(position))
                {
                    result = HeroItems.CanPlaceItem(item, position);
                }
                else
                {
                    if (Set<HeroItemPosition>.Of().WithRange(HeroItemPosition.InventorySlot1, HeroItemPosition.InventorySlot8).Contains(position))
                    {
                        result = Inventory.GetSlot(position) == null;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
        }

        return result;
    }

    // Hero.PAS:1758
    public bool CanSwapItem(int itemId1, int itemId2)
    {
        Item item1;
        Item item2;
        bool result = true;
        item1 = AoWHSMap.Current.ItemControl.FindItem(itemId1);
        item2 = AoWHSMap.Current.ItemControl.FindItem(itemId2);
        if (item1 != null && item2 != null && item1 != item2)
        {
            if (HeroItems == item1.Owner || HeroItems == item2.Owner)
            {
                result = item1.ItemType == item2.ItemType;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1772
    public bool SwapItem(int itemId1, int itemId2)
    {
        bool result;
        HeroUpdateTokenEvent tokenEvent;
        if (CanSwapItem(itemId1, itemId2) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            tokenEvent = CreateHeroUpdateTokenEvent();
            tokenEvent.Mode = HeroUpdateTokenEventMode.SwapItem;
            tokenEvent.ItemId = itemId1;
            tokenEvent.Parameter = itemId2;
            AoWHSMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1788
    public bool PlaceItem(int itemId, HeroItemPosition position)
    {
        bool result;
        HeroUpdateTokenEvent tokenEvent;
        if (CanPlaceItem(itemId, position) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            tokenEvent = CreateHeroUpdateTokenEvent();
            tokenEvent.Mode = HeroUpdateTokenEventMode.PlaceItem;
            tokenEvent.ItemId = itemId;
            tokenEvent.Parameter = (int)position;
            AoWHSMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1804
    public bool ExecuteSwapItem(int itemId1, int itemId2)
    {
        bool result;
        Item item1;
        Item item2;
        ItemList owner1;
        ItemList owner2;
        int index1;
        int index2;
        if (CanSwapItem(itemId1, itemId2))
        {
            item1 = AoWHSMap.Current.ItemControl.FindItem(itemId1);
            item2 = AoWHSMap.Current.ItemControl.FindItem(itemId2);
            owner1 = item1.Owner;
            owner2 = item2.Owner;
            owner1.BeginUpdate();
            owner2.BeginUpdate();
            index1 = item1.Owner.FindChildIndex(item1);
            index2 = item2.Owner.FindChildIndex(item2);
            item1.SetOwner(null);
            item2.SetOwner(null);
            if (owner1.GetControlStyle().Contains(ControlStyleElement.ArrayMode))
            {
                item2.SetArrayOwner(owner1, index1);
            }
            else
            {
                item2.SetOwner(owner1);
            }

            if (owner2.GetControlStyle().Contains(ControlStyleElement.ArrayMode))
            {
                item1.SetArrayOwner(owner2, index2);
            }
            else
            {
                item1.SetOwner(owner2);
            }

            owner2.EndUpdate();
            owner1.EndUpdate();
            Changed();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1831
    public bool AutoPlaceItem(Item item)
    {
        HeroItemPosition position;
        int slot;
        bool result = false;
        position = HeroItems.GetItemTypePosition(item.ItemType);
        if (HeroItems.CanPlaceItem(item, position))
        {
            HeroItems.PlaceItem(item, position);
            result = true;
        }
        else
        {
            slot = Inventory.GetFreeSlot();
            if (slot != -1)
            {
                item.SetArrayOwner(Inventory, slot);
                result = true;
            }
        }

        return result;
    }

    // Hero.PAS:1852
    public bool ExecutePlaceItem(int itemId, HeroItemPosition position)
    {
        bool result;
        Item item;
        CustomCombatMap map;
        if (CanPlaceItem(itemId, position))
        {
            item = AoWHSMap.Current.ItemControl.FindItem(itemId);
            switch (position)
            {
                case HeroItemPosition.Ground:
                    map = AoWHSMap.Current.CombatMap;
                    if (map != null)
                    {
                        map.DropHeroItem(this, item);
                    }
                    else
                    {
                        AoWHSMap.Current.DropHeroItem(this, item);
                    }

                    Changed();
                    break;
                case >= HeroItemPosition.AttackSlot and <= HeroItemPosition.RightRingSlot:
                    HeroItems.PlaceItem(item, position);
                    Changed();
                    break;
                case >= HeroItemPosition.InventorySlot1 and <= HeroItemPosition.InventorySlot8:
                    item.SetArrayOwner(Inventory, (int)position - 6);
                    Changed();
                    break;
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1876
    public bool ExecuteStartCasting(int spellId)
    {
        bool result;
        Spell spell;
        if (CastingSpellId != 0)
        {
            ExecuteCancelCasting();
        }

        spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
        var players = AoWHSMap.Current.Players.GetPlayers(Player);
        var castingMana = CastingMana(spell);
        if (players.MagicControl.Mana >= castingMana && CastingMana(spell) > CastingPoints)
        {
            CastingProgress = CastingMana(spell);
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(Player);
            AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana - CastingProgress);
            CastingManaValue = CastingPoints;
            CastingPoints = 0;
            if (AoWHSMap.Current.SeatedPlayer == Player)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castingInProgressFormat), [spell.SpellName]);
                AoWHSMap.Current.ShowMessage(format);
            }

            CastingSpellId = spellId;
            Changed();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1898
    public void RemoveCastingReadyEventLog()
    {
        EventLogbook logbook;
        logbook = AoWHSMap.Current.Players.GetPlayers(Player).EventLogbook;
        for (int I = logbook.GetCount() - 1; I >= 0; I--)
        {
            if (logbook.GetLogs(I) is SpellCastEventLog && ((SpellCastEventLog)logbook.GetLogs(I)).UnitId == UnitId)
            {
                logbook.GetLogs(I).SetOwner(null);
            }
        }
    }

    // Hero.PAS:1907
    public bool ExecuteCancelCasting()
    {
        bool result;
        if (CastingSpellId != 0)
        {
            RemoveCastingReadyEventLog();
            if (GetCastingPointsMax() < CastingPoints + CastingManaValue)
            {
                CastingPoints = GetCastingPointsMax();
            }
            else
            {
                CastingPoints = (sbyte)(CastingPoints + CastingManaValue);
            }

            var players = AoWHSMap.Current.Players.GetPlayers(Player);
            AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(players.MagicControl.Mana + CastingProgress);
            CastingSpellId = 0;
            CastingManaValue = 0;
            CastingProgress = 0;
            Changed();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:1925
    public void CastingDone(int spellId, int mana)
    {
        if (spellId == CastingSpellId)
        {
            Assert(CastingReady(), "", "Hero.PAS", 2080);
            CastingManaValue = 0;
            CastingProgress = 0;
            CastingSpellId = 0;
            RemoveCastingReadyEventLog();
        }
        else
        {
            if (CastingSpellId != 0)
            {
                ExecuteCancelCasting();
            }

            if (!AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
            {
                var players = AoWHSMap.Current.Players.GetPlayers(Player);
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(players.MagicControl.Mana - mana);
                CastingPoints = (sbyte)(CastingPoints - mana);
            }
        }
    }

    // Hero.PAS:2098
    public int CastingMana(Spell spell)
    {
        int result = spell.BaseManaCost;
        if (AoWHSMap.Current.GlobalMagicControl.GetSphereManaDoubled(spell.MagicSphere))
        {
            result = result * 2;
        }

        return result;
    }

    // Hero.PAS:2104
    public int CastingTurns(Spell spell)
    {
        int result;
        int mana;
        int points;
        mana = CastingMana(spell);
        points = CastingPoints + CastingManaValue;
        if (GetCastingPointsMax() < points)
        {
            points = GetCastingPointsMax();
        }

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
            result = 0;
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
                var castingPointsMax = GetCastingPointsMax();
                result = (CastingProgress - CastingManaValue) / castingPointsMax;
                var castingPointsMax2 = GetCastingPointsMax();
                if ((CastingProgress - CastingManaValue) % castingPointsMax2 != 0)
                {
                    result += 1;
                }
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Hero.PAS:2136
    public bool CanCastSpellInstantly(int spellId, int mana, ref string reason)
    {
        bool result;
        if (!AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
        {
            if (mana <= CastingPoints || spellId == CastingSpellId && Casting() && CastingReady())
            {
                result = CanCastSpell(spellId, ref reason);
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GameText.NotEnoughChannelingPointsText);
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Hero.PAS:2149
    public bool CanCastSpell(int spellId, ref string reason)
    {
        bool result;
        Spell spell;
        if (CastingSpellId == 0)
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
            var players = AoWHSMap.Current.Players.GetPlayers(Player);
            var castingMana = CastingMana(spell);
            if (players.MagicControl.Mana >= castingMana || AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
            {
                result = spell.CanActivate(this, ref reason);
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GameText.NotEnoughManaText);
            }
        }
        else
        {
            result = spellId == CastingSpellId;
            if (result && AoWHSSet.Current.SpellControl.GetSpell(spellId).SpellType == SpellType.Global && AoWHSMap.Current.CombatMap != null)
            {
                reason = AbilityRules.TranslateResourceString(s_cannotCastSpellInCombatText);
                result = false;
            }
        }

        return result;
    }

    // Hero.PAS:2176
    public bool CastSpell(int spellId, ref string reason)
    {
        bool result;
        Spell spell;
        HeroUpdateTokenEvent tokenEvent;
        if (CanCastSpell(spellId, ref reason))
        {
            if (CastingSpellId == 0)
            {
                spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
                if (CastingMana(spell) <= CastingPoints || AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    result = spell.CanActivate(this, ref reason) && spell.Activate(this);
                }
                else
                {
                    if (!AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
                    {
                        var players = AoWHSMap.Current.Players.GetPlayers(Player);
                        var castingMana = CastingMana(spell);
                        if (players.MagicControl.Mana >= castingMana)
                        {
                            tokenEvent = CreateHeroUpdateTokenEvent();
                            tokenEvent.Mode = HeroUpdateTokenEventMode.StartCasting;
                            tokenEvent.ItemId = spellId;
                            AoWHSMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
                            tokenEvent.Release();
                            result = true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GameText.NotEnoughManaText);
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
                if (CastingReady())
                {
                    spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
                    result = spell.CanActivate(this, ref reason) && spell.Activate(this);
                }
                else
                {
                    result = true;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:2220
    public bool CanCastCombatSpell(int spellId, ref string reason)
    {
        bool result;
        Spell spell;
        if (CastingSpellId == 0)
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
            if (CastingMana(spell) <= CastingPoints || AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
            {
                var players = AoWHSMap.Current.Players.GetPlayers(Player);
                var castingMana = CastingMana(spell);
                if (players.MagicControl.Mana >= castingMana || AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    result = spell.CanActivateCombat(this, ref reason);
                }
                else
                {
                    result = false;
                    reason = AbilityRules.TranslateResourceString(GameText.NotEnoughManaText);
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(s_notEnoughChannelingPointsText);
            }
        }
        else
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
            result = Set<SpellType>.Of(SpellType.Combat, SpellType.Unit).Contains(spell.SpellType) && CanCastSpellInstantly(spellId, CastingMana(spell), ref reason);
        }

        return result;
    }

    // Hero.PAS:2252
    public bool CastCombatSpell(int spellId, ref string reason)
    {
        bool result;
        Spell spell;
        spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
        if (CanCastCombatSpell(spellId, ref reason))
        {
            result = spell.CanActivateCombat(this, ref reason) && spell.ActivateCombat(this);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:2264
    public bool CancelCasting()
    {
        bool result;
        HeroUpdateTokenEvent tokenEvent;
        // { Original tests -1 although zero is the idle spell ID. }
        if (CastingSpellId != -1 && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            tokenEvent = CreateHeroUpdateTokenEvent();
            tokenEvent.Mode = HeroUpdateTokenEventMode.CancelCasting;
            AoWHSMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_castingInProgressFormat = "Casting %s...";
    private static readonly string s_cannotCastSpellInCombatText = "Cannot cast spell in combat";
    private static readonly string s_notEnoughChannelingPointsText = "Not enough channeling points";
    private static readonly ImmutableArray<HeroLevel> s_heroExperienceTable = [new HeroLevel { DailyExperience = 1, Experience = 15 }, new HeroLevel { DailyExperience = 1, Experience = 15 }, new HeroLevel { DailyExperience = 1, Experience = 20 }, new HeroLevel { DailyExperience = 1, Experience = 20 }, new HeroLevel { DailyExperience = 1, Experience = 25 }, new HeroLevel { DailyExperience = 1, Experience = 25 }];

    internal static ReadOnlySpan<sbyte> MoraleResistance => [-2, -1, 0, 0, 0];
    internal static ReadOnlySpan<sbyte> MoraleDefense => [-2, -1, 0, 0, 0];

}
