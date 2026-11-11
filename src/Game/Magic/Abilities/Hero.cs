// Translated from AoWE.pas.
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

// FUpgradePending. FHeroResource. FHeroResourceIndex. FExperience. FUsedSkillPoints. FDeathDay.
// FDeathPlayer. FLevel. FHeroStatus. FHeroName. FNickname. FRace. FGender. unit statistics.
// FCastSpellInstantly. FMovePoints. FHitPoints. FPowerSource. FCastingPoints. FCastingMana.
// FCastingProgress. FLibraryID. FLibraryName. FRegistered
public partial class Hero : Troop
{
    public new const int ClassID = 131632;

    public int FFaceIndex;
    public HeroResource FHeroResource;
    public int FHeroResourceIndex;
    public int FExperience;
    public byte FLevel;
    public int FUsedSkillPoints;
    public bool FUpgradePending;
    public Set<HeroStatusElement> FHeroStatus;
    public int FDeathDay;
    public sbyte FDeathPlayer;
    public Set<HeroModeElement> FMode;
    public string FHeroName = "";
    public string FNickname = "";
    public sbyte FRace;
    public UnitGender FGender;
    public sbyte FUnitAttack;
    public sbyte FUnitDefense;
    public sbyte FUnitDamage;
    public sbyte FUnitHits;
    public sbyte FUnitMoves;
    public sbyte FUnitResistance;
    public HeroItems FHeroItems;
    public HeroInventory FInventory;
    public bool FCastSpellInstantly;
    public sbyte FMovePoints;
    public sbyte FHitPoints;
    public HeroPowerSource FPowerSource;
    public sbyte FCastingPoints;
    public int FCastingSpellID;
    public int FCastingMana;
    public int FCastingProgress;
    public int FLibraryID;
    public string FLibraryName = "";
    public bool FRegistered;

    // Hero.PAS:631
    public override void Dispose()
    {
        AddRef();
        AddRef();
        AddRef();
        // { Original repeats the reference increment four times. }
        AddRef();
        if (FRegistered && AoWHSMap.Current != null)
        {
            AoWHSMap.Current.FHeroControl.UnRegisterHero(this);
        }

        base.Dispose();
        FHeroItems?.Dispose();
        FInventory?.Dispose();
        FPowerSource?.Dispose();
    }

    // Hero.PAS:683
    public override void SetOwner(ECustomNode AOwner)
    {
        AddRef();
        base.SetOwner(AOwner);
        if (FUnitStatus.Contains(UnitStatusElement.Activated))
        {
            Changed();
        }

        Release();
    }

    // Hero.PAS:700
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // Hero.PAS:1625
    public override void ReadWrite(EStorageStream Stream)
    {
        sbyte Player = default;
        int OldExperience = default;
        Player = FPlayer;
        Stream.rwInteger(ref FHeroResourceIndex, 8);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            LinkToResource();
        }

        base.ReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && FMode.Contains(HeroModeElement.UpdatingFromLibrary))
        {
            FPlayer = Player;
        }

        Stream.rwInteger(ref FFaceIndex, 7);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Stream.rwInteger(ref OldExperience, 9);
        }
        else
        {
            OldExperience = 0;
        }

        Stream.rwString(ref FHeroName, 10);
        Stream.rwString(ref FNickname, 11);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 12);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FCastingPoints), 13);
        Stream.rwInteger(ref FCastingSpellID, 14);
        Stream.rwData(Bytes.Of(ref FGender, 1), 1, 15);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FUnitAttack), 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FUnitDefense), 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FUnitDamage), 18);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FUnitHits), 19);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FUnitMoves), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FUnitResistance), 21);
        Stream.rwByte(ref FLevel, 22);
        Stream.rwBool(ref FUpgradePending, 23);
        Stream.rwInteger(ref FExperience, 24);
        RwSet(ref FHeroStatus, 1, 25, Stream.rwData, 0);
        Stream.rwInteger(ref FDeathDay, 26);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDeathPlayer), 27);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && FExperience == 0)
        {
            FExperience = LevelToExperience((int)(((uint)OldExperience) >> 5) + 1);
        }

        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMovePoints), 29);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FHitPoints), 30);
        Stream.rwInteger(ref FCastingMana, 31);
        Stream.rwBool(ref FCastSpellInstantly, 33);
        Stream.rwInteger(ref FCastingProgress, 34);
        Stream.rwIntegerEx(ref FLibraryID, -1, 35);
        if (!FMode.Contains(HeroModeElement.UpdatingFromLibrary))
        {
            Stream.rwEObject(FInventory, 36);
            Stream.rwEObject(FHeroItems, 37);
        }

        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FLibraryName, 38);
        }

        Stream.rwInteger(ref FUsedSkillPoints, 39);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Loaded();
        }
    }

    // Hero.PAS:1694
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001989)
        {
            if (AoWHSMap.Current.FPlayers.GetCount() <= FPlayer && Owner == null)
            {
                SetPlayer(-1);
                return;
            }
        }
        else
        {
            if (Msg.MsgID == 65541)
            {
                EditDropMsg(ref Msg);
            }
        }
    }

    // Hero.PAS:1347
    public override bool GetAbSet(int index) => base.GetAbSet(index);

    // Hero.PAS:1344
    public override void SetAbSet(int index, byte value)
    {
        base.SetAbSet(index, value);
    }

    // Hero.PAS:697
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of(AbilitySelectionType.Editor);

    // Hero.PAS:716
    public override void Changed()
    {
        base.Changed();
        if (AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            AoWHSMap.Current.FHeroControl.HeroChanged(this);
        }
    }

    // Hero.PAS:930
    public override byte GetUpkeep() => (byte)(GetLevel() * 2 + 5);

    // Hero.PAS:680
    public override sbyte GetUnitLevel() => (sbyte)FLevel;

    // Hero.PAS:818
    public override sbyte GetRace() => FHeroResource.FRace;

    // Hero.PAS:1426
    public override sbyte GetInherentAttack() => (sbyte)(FHeroResource.FAttack + FUnitAttack);

    // Hero.PAS:1445
    public override sbyte GetInherentDefense() => (sbyte)(FHeroResource.FDefense + FUnitDefense);

    // Hero.PAS:1464
    public override sbyte GetInherentDamage() => (sbyte)(FHeroResource.FDamage + FUnitDamage);

    // Hero.PAS:1483
    public override sbyte GetInherentResistance() => (sbyte)(FHeroResource.FResistance + FUnitResistance);

    // Hero.PAS:1429
    public override sbyte GetAttack()
    {
        AbilityOwner AOwner;
        int Count;
        Count = GetAbilityCount();
        sbyte Result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= Count; I++)
        {
            AOwner = GetSuperlativeEnabledAbOwner(I);
            if (AOwner != null)
            {
                Result = (sbyte)(Result + AOwner.GetAbAttack(I));
            }
        }

        Result = (sbyte)(FHeroItems.GetAttack() + (FUnitAttack + Result + FHeroResource.FAttack));
        if (Result < 1)
        {
            Result = 1;
        }

        if (Result > 10)
        {
            Result = 10;
        }

        return Result;
    }

    // Hero.PAS:1448
    public override sbyte GetDefense()
    {
        AbilityOwner AOwner;
        int Count;
        Count = GetAbilityCount();
        sbyte Result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= Count; I++)
        {
            AOwner = GetSuperlativeEnabledAbOwner(I);
            if (AOwner != null)
            {
                Result = (sbyte)(Result + AOwner.GetAbDefense(I));
            }
        }

        var defense = FHeroItems.GetDefense();
        var morale = GetMorale();
        Result = (sbyte)(defense + (FUnitDefense + Result + FHeroResource.FDefense) + P2MoraleDefense[(int)morale]);
        if (Result < 1)
        {
            Result = 1;
        }

        if (Result > 10)
        {
            Result = 10;
        }

        return Result;
    }

    // Hero.PAS:1467
    public override sbyte GetDamage()
    {
        AbilityOwner AOwner;
        int Count;
        Count = GetAbilityCount();
        sbyte Result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= Count; I++)
        {
            AOwner = GetSuperlativeEnabledAbOwner(I);
            if (AOwner != null)
            {
                Result = (sbyte)(Result + AOwner.GetAbDamage(I));
            }
        }

        Result = (sbyte)(FHeroItems.GetDamage() + (FUnitDamage + Result + FHeroResource.FDamage));
        if (Result < 1)
        {
            Result = 1;
        }

        if (Result > 10)
        {
            Result = 10;
        }

        return Result;
    }

    // Hero.PAS:1486
    public override sbyte GetResistance()
    {
        AbilityOwner AOwner;
        int Count;
        Count = GetAbilityCount();
        sbyte Result = 0;
        // { Original includes the ability-count index in the scan. }
        for (int I = 0; I <= Count; I++)
        {
            AOwner = GetSuperlativeEnabledAbOwner(I);
            if (AOwner != null)
            {
                Result = (sbyte)(Result + AOwner.GetAbResistance(I));
            }
        }

        var resistance = FHeroItems.GetResistance();
        var morale = GetMorale();
        Result = (sbyte)(resistance + (FUnitResistance + Result + FHeroResource.FResistance) + P2MoraleResistance[(int)morale]);
        if (Result < 1)
        {
            Result = 1;
        }

        if (Result > 10)
        {
            Result = 10;
        }

        return Result;
    }

    // Hero.PAS:1502
    public override sbyte GetHits()
    {
        sbyte Result = (sbyte)(FHeroResource.FHits + FUnitHits);
        if (Result < 1)
        {
            Result = 1;
        }

        if (Result > 30)
        {
            Result = 30;
        }

        return Result;
    }

    // Hero.PAS:1509
    public override sbyte GetMoves()
    {
        sbyte Result = (sbyte)(FHeroResource.FMoves + FUnitMoves);
        if (Result < 1)
        {
            Result = 1;
        }

        if (Result > 50)
        {
            Result = 50;
        }

        return Result;
    }

    // Hero.PAS:842
    public override sbyte GetMovePoints() => FMovePoints;

    // Hero.PAS:845
    public override void SetMovePoints(sbyte Value)
    {
        if (GetMoves() < Value)
        {
            Value = GetMoves();
        }

        FMovePoints = Value;
    }

    // Hero.PAS:851
    public override sbyte GetHitPoints() => FHitPoints;

    // Hero.PAS:854
    public override void SetHitPoints(sbyte Value)
    {
        if (GetHits() < Value)
        {
            Value = GetHits();
        }

        if (Value < 0)
        {
            Value = 0;
        }

        FHitPoints = Value;
        UpdateDefensiveStrength();
    }

    // Hero.PAS:824
    public override Set<MoveType> GetMoveTypes()
    {
        Set<MoveType> Result = base.GetMoveTypes();
        return Result + FHeroItems.GetMoveTypes();
    }

    // Hero.PAS:830
    public override Set<DamageType> GetImmunityTypes()
    {
        Set<DamageType> Result = base.GetImmunityTypes();
        return Result + FHeroItems.GetImmunityTypes();
    }

    // Hero.PAS:836
    public override Set<DamageType> GetProtectionTypes()
    {
        Set<DamageType> Result = base.GetProtectionTypes();
        return Result + FHeroItems.GetProtectionTypes();
    }

    // Hero.PAS:1596
    public override LibraryImage GetFace()
    {
        LibraryImage Result;
        FaceResource Resource;
        int Index;
        if (!GetAbilityEnabled(126))
        {
            Resource = AoWHSSet.Current.FFaceResources.GetFaceResource(FFaceIndex);
            if (Resource != null)
            {
                Result = Resource.GetCellImage();
            }
            else
            {
                Result = null;
            }
        }
        else
        {
            Index = ((CosmeticSurgeryEnchantment)((UnitEnchantmentAbilityData)GetAbilityData(126)).FEnchantment).FFaceIndex;
            Result = AoWHSSet.Current.FFaceResources.GetFaceResource(Index).GetCellImage();
        }

        return Result;
    }

    // Hero.PAS:821
    public override string GetName() => FHeroName;

    // Hero.PAS:862
    public override Alignment GetAlignment()
    {
        Alignment Result;
        sbyte Race;
        if (GetAbilityEnabled(107))
        {
            Result = Alignment.PureEvil;
        }
        else
        {
            Race = FRace;
            if (AoWHSSet.Current.FRaceResources.GetRaceResource(Race) != null)
            {
                Result = AoWHSSet.Current.FRaceResources.GetRaceResource(Race).GetAlignment();
            }
            else
            {
                Result = FHeroResource.FAlignment;
            }
        }

        return Result;
    }

    // Hero.PAS:1538
    public override LibraryImage GetPreviewImage()
    {
        LibraryImage Result;
        if (FHeroResource != null)
        {
            Result = FHeroResource.GetPreviewImage();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Hero.PAS:732
    public override UnitGender GetGender() => FGender;

    // Hero.PAS:735
    public override UnitType GetUnitType() => UnitType.Humanoid;

    // Hero.PAS:1003
    public override int GetUnitGFXResourceIndex() => FHeroResource.FUnitGFXResourceIndex;

    // Hero.PAS:644
    public override int GetCampaignTransferPoints()
    {
        int Result = base.GetCampaignTransferPoints();
        return Result / 2;
    }

    // Hero.PAS:1522
    public override sbyte GetCastingPointsMax()
    {
        sbyte Result;
        if (GetAbilityEnabled(52))
        {
            Result = (sbyte)(GetAbilityLevel(52) * 10);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Hero.PAS:1528
    public override sbyte GetCastingPoints() => FCastingPoints;

    // Hero.PAS:1531
    public override void SetCastingPoints(sbyte Value)
    {
        if (Value > GetCastingPointsMax())
        {
            Value = GetCastingPointsMax();
        }

        FCastingPoints = Value;
        Changed();
    }

    // Hero.PAS:1516
    public override int GetPowerGeneration()
    {
        int Result;
        if (GetAbilityEnabled(52))
        {
            Result = GetAbilityLevel(52) * 5;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Hero.PAS:738
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.FDay == 1)
        {
            if ((GameTables.AoWEngine.FCampaign == null || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer) == null || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human) && (!AoWHSMap.Current.FLeadersFixed || !(this is Leader)))
            {
                FUsedSkillPoints = GetSkillPoints();
            }

            UpdateSettings();
        }
    }

    // Hero.PAS:1311
    public override void NewTurn(sbyte Player)
    {
        SpellCastEventLog Log;
        base.NewTurn(Player);
        if (Player == FPlayer)
        {
            ValidateHeroUpgrade();
            Player = GetCastingPointsMax();
            FCastingPoints = Player;
            if (FCastingSpellID != 0 && FCastingMana < FCastingProgress)
            {
                FCastingMana += Player;
                FCastingPoints = 0;
                if (FCastingMana > FCastingProgress)
                {
                    FCastingPoints = (sbyte)(FCastingMana - FCastingProgress);
                    FCastingMana = FCastingProgress;
                }

                if (FCastingMana == FCastingProgress)
                {
                    Log = new SpellCastEventLog(null);
                    Log.FPlayer = FPlayer;
                    Log.FUnitID = FUnitID;
                    Log.FSpellID = FCastingSpellID;
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FEventLogbook.AddEvent(Log);
                    if (AoWHSMap.Current.FSeatedPlayer == FPlayer && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.Human)
                    {
                        Log.Execute(false, ExecuteTrigger.Event, null);
                    }

                    Log.Release();
                }
            }
        }
    }

    // Hero.PAS:1418
    public override int GetAbilityLevel(int AbilityID)
    {
        AbilityOwner AOwner;
        AOwner = GetSuperlativeAbOwner(AbilityID);
        if (AOwner == null)
        {
            AOwner = this;
        }

        return AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetLevel(AOwner);
    }

    // Hero.PAS:1396
    public override bool GetAbilityEnabled(int AbilityID) => base.GetAbilityEnabled(AbilityID) || FHeroItems.GetAbilityEnabled(AbilityID) || FInventory.GetAbilityEnabled(AbilityID);

    // Hero.PAS:1403
    public override bool GetAbilitySet(int AbilityID) => base.GetAbilitySet(AbilityID) || FHeroItems.GetAbilitySet(AbilityID) || FInventory.GetAbilitySet(AbilityID);

    // Hero.PAS:1410
    public override string GetAbilityName(int AbilityID)
    {
        AbilityOwner AOwner;
        AOwner = GetSuperlativeAbOwner(AbilityID);
        if (AOwner == null)
        {
            AOwner = this;
        }

        return AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetName(AOwner);
    }

    // Hero.PAS:1393
    public override AbilityOwner GetAbilityOwner(int AbilityID) => GetSuperlativeAbOwner(AbilityID);

    // Hero.PAS:1353
    public override int GetAbilityCount()
    {
        int Count;
        int Result = GetAbCount();
        Count = FHeroItems.GetAbilityCount();
        if (Count > Result)
        {
            Result = Count;
        }

        Count = FInventory.GetAbilityCount();
        if (Count > Result)
        {
            Result = Count;
        }

        return Result;
    }

    // Hero.PAS:1028
    public override int GetExperience() => FExperience;

    // Hero.PAS:1031
    public override void SetExperience(int Value)
    {
        int Maximum;
        FExperience = Value;
        Maximum = LevelToExperience(GetLevelMax());
        if (Maximum < FExperience)
        {
            FExperience = Maximum;
        }

        Changed();
    }

    // Hero.PAS:1063
    public override int GetNextLevelExperience()
    {
        int Result;
        int Level;
        int Maximum;
        Level = GetLevel();
        Maximum = GetLevelMax();
        if (Level < Maximum)
        {
            Result = LevelToExperience(Level + 1);
        }
        else
        {
            Result = LevelToExperience(Level);
        }

        return Result;
    }

    // Hero.PAS:729
    public override StringCollection GetDescription() => null;

    // Hero.PAS:933
    public override void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            bool condition = FPlayer != -1;
            if (condition)
            {
                var count = AoWHSMap.Current.FPlayers.Count;
                condition = FPlayer < count;
            }

            if (condition)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FHeroes.RemoveChild(this);
            }

            base.SetPlayer(Player);
            if (FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FHeroes.AddChild(this);
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).ResetBaseHeroJoinDay();
            }
        }
    }

    // Hero.PAS:726
    public override int GetObtainValue() => GetLevel() * 50 + 75;

    // Hero.PAS:914
    public override bool CanActivate()
    {
        Hero Hero;
        bool Result = base.CanActivate();
        if (Result && !FRegistered && FUnitID != -1)
        {
            Hero = AoWHSMap.Current.FHeroControl.FindID(FUnitID);
            if (Hero == null || Hero == this)
            {
                Result = true;
            }
            else
            {
                if (Hero.Owner == null && CompatibleHero(Hero))
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
        }

        return Result;
    }

    // Hero.PAS:964
    public override void Activate()
    {
        Hero Hero;
        if (FUnitID != -1)
        {
            Hero = AoWHSMap.Current.FHeroControl.FindID(FUnitID);
            if (Hero != null && Hero != this && Hero.Owner == null && CompatibleHero(Hero))
            {
                AoWHSMap.Current.FHeroControl.UnRegisterHero(Hero);
            }
        }

        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            FLevel = (byte)GetLevel();
        }

        if (FHeroResourceIndex == -1)
        {
            SetHeroResourceIndex(0);
        }

        if (FPlayer > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer) == null && Owner == null)
        {
            FPlayer = -1;
        }

        if (Owner == null && FGroupID != 0)
        {
            FGroupID = 0;
        }

        base.Activate();
        if (AoWHSMap.Current != null)
        {
            if (!FMode.Contains(HeroModeElement.SkipLibraryUpdate) && (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || AoWHSMap.Current.FDay == 0))
            {
                UpdateFromLibrary();
            }

            if (!FRegistered)
            {
                AoWHSMap.Current.FHeroControl.RegisterHero(this);
            }

            if (FPlayer != -1 && Owner != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FHeroes.AddChild(this);
            }

            FInventory.RegisterItems();
            FHeroItems.RegisterItems();
        }
    }

    // Hero.PAS:993
    public override void Deactivate()
    {
        bool condition = FPlayer != -1;
        if (condition)
        {
            var count = AoWHSMap.Current.FPlayers.Count;
            condition = FPlayer < count;
        }

        if (condition)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FHeroes.RemoveChild(this);
        }

        base.Deactivate();
    }

    // Hero.PAS:765
    public override bool CanJoin(sbyte Player)
    {
        bool Result;
        Player APlayer;
        if (FRace != -1)
        {
            APlayer = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            var playerRelation = AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(Player);
            Result = Set<Status>.Of(Status.Polite, Status.Friendly).Contains(playerRelation);
            if (Result)
            {
                bool condition = FGroup == null || FGroup.GroupClass.Behavior != GroupBehavior.Refuge;
                if (condition)
                {
                    bool test = !GetAbilityEnabled(128) || Player != FDeathPlayer;
                    if (!test)
                    {
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                        test = FDeathDay + 2 < players.FDay;
                    }

                    condition = test;
                }

                if (condition)
                {
                    Result = Result && APlayer.FHeroes.Count < APlayer.GetHeroCountMax() && GetLevel() <= APlayer.GetHeroJoinLevelMax();
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:794
    public override bool OfferToJoin(sbyte Player)
    {
        bool Result;
        Player APlayer;
        int Seed;
        int Chance;
        if (CanJoin(Player))
        {
            APlayer = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            if (EmergedToPlayer() == -1)
            {
                if ((!GetAbilityEnabled(128) || Player != FDeathPlayer) && (FGroup == null || FGroup.GroupClass.Behavior != GroupBehavior.Refuge))
                {
                    Chance = APlayer.GetHeroJoinChance(GetLevel());
                    Seed = RandSeed;
                    RandSeed = (AoWHSMap.Current.FInitialRandomSeed + AoWHSMap.Current.FDay + FUnitID * 16) & 4088;
                    Result = Random(100) < Chance;
                    RandSeed = Seed;
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = Player == EmergedToPlayer();
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:706
    public override int ShowEx(int X, int Y, in LockedSurface SD, in UnitShowInfo Info)
    {
        int Result = base.ShowEx(X, Y, SD, Info);
        if (Info.Overlays.Contains(UnitOverlayModesElement.Rank))
        {
            GameTables.AoWEngine.FCombatFont.Color = 16777215;
            var level = GetLevel();
            GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 43, Y + 12, IntToStr(level), SD);
        }

        return Result;
    }

    // Hero.PAS:885
    public override void Killed()
    {
        HeroItemPosition Position;
        for (int I = FInventory.Count - 1; I >= 0; I--)
        {
            if (FInventory.GetItem(I) != null)
            {
                ExecutePlaceItem(FInventory.GetItem(I).FItemID, HeroItemPosition.Ground);
            }
        }

        for (int positionOrdinal = (int)HeroItemPosition.AttackSlot; positionOrdinal <= (int)HeroItemPosition.RightRingSlot; positionOrdinal++)
        {
            Position = (HeroItemPosition)positionOrdinal;
            if (FHeroItems.GetPositionItem(Position) != null)
            {
                ExecutePlaceItem(FHeroItems.GetPositionItem(Position).FItemID, HeroItemPosition.Ground);
            }
        }

        FDeathDay = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDay;
        FDeathPlayer = FPlayer;
        base.Killed();
        FHeroStatus = FHeroStatus + Set<HeroStatusElement>.Of(HeroStatusElement.Died);
        SetPlayer(-1);
        SetOwner(null);
    }

    // Hero.PAS:902
    public override void Resurrect()
    {
        base.Resurrect();
        FHeroStatus = FHeroStatus - Set<HeroStatusElement>.Of(HeroStatusElement.Died);
    }

    // Hero.PAS:908
    public override void Animate()
    {
        base.Animate();
        FHeroStatus = FHeroStatus - Set<HeroStatusElement>.Of(HeroStatusElement.Died);
    }

    // Hero.PAS:691
    public virtual string GetNickname() => FNickname;

    // Hero.PAS:694
    public virtual void SetNickname(string Value)
    {
        FNickname = Value;
    }

    // Hero.PAS:1615
    public virtual void SetHeroResourceIndex(int Value)
    {
        if (AoWHSSet.Current.FHeroResources.GetHeroResource(Value) != null && Value != FHeroResourceIndex)
        {
            FHeroResourceIndex = Value;
            FHeroResource = AoWHSSet.Current.FHeroResources.GetHeroResource(Value);
            ResetResourceSettings();
        }
    }

    // Hero.PAS:1674
    public virtual void EditDropMsg(ref EngineMessage Msg)
    {
        MapLevel Level;
        int L;
        int X = default;
        int Y = default;
        int Player;
        bool Placed;
        Level = Msg.Param[0].As<MapLevel>();
        L = Msg.Data[1];
        HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref X, ref Y);
        if (X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
        {
            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                Player = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetPlayer();
                if (Player == -1)
                {
                    Player = AoWHSMap.Current.FEditorPlayer;
                }
            }
            else
            {
                Player = -1;
            }

            Placed = Place(X, Y, L, (sbyte)Player, Set<UnitPlaceModeElement>.Of()) != null;
        }
        else
        {
            Placed = false;
        }

        if (Placed)
        {
            Msg.Result = 0;
        }
        else
        {
            Msg.Result = -1;
        }
    }

    // Hero.PAS:650
    public string GetLibraryName()
    {
        string Result = FLibraryName;
        if (IsEmpty(Result))
        {
            Result = AbilityRules.TranslateRStr(GameText.SystemRStr);
        }

        return Result;
    }

    // Hero.PAS:656
    public void UpdateFromLibraryBegin()
    {
        FMode = FMode + Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
    }

    // Hero.PAS:659
    public void UpdateFromLibraryEnd()
    {
        FMode = FMode - Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
    }

    // Hero.PAS:662
    public void UpdateFromLibrary()
    {
        Hero Hero;
        int ID;
        if (FLibraryID != -1)
        {
            Hero = AoWHSSet.Current.FHeroLibraries.FindHero(FLibraryName, FLibraryID);
            if (Hero != null)
            {
                ID = FUnitID;
                FMode = FMode + Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
                Assign(Hero, GameTables.AoWEngine);
                FMode = FMode - Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
                FUnitID = ID;
                Changed();
            }
        }
    }

    // Hero.PAS:703
    public bool GetLibraryHero() => FLibraryID != -1;

    // Hero.PAS:751
    public int UsedSkillPoints()
    {
        int Result = FUnitAttack * 5 + FUnitDefense * 5 + FUnitDamage * 10 + FUnitResistance * 5 + FUnitHits * 5 + FUnitMoves * 2;
        for (int I = GetAbCount() - 1; I >= 0; I--)
        {
            if (GetAbSet(I))
            {
                Result += AoWHSSet.Current.FAbilityControl.GetAbility(I).GetSkillPoints(this);
                if (FHeroResource.FAbilities.GetAbSet(I))
                {
                    Result -= AoWHSSet.Current.FAbilityControl.GetAbility(I).GetSkillPoints(FHeroResource.FAbilities);
                }
            }
        }

        return Result;
    }

    // Hero.PAS:782
    public sbyte EmergedToPlayer()
    {
        int I;
        I = AoWHSMap.Current.FPlayers.Count;
        sbyte Result = -1;
        while (I != 1 && Result == -1)
        {
            I -= 1;
            if (AoWHSMap.Current.FPlayers.GetPlayers(I).FEmergingHeroID == FUnitID)
            {
                Result = (sbyte)I;
            }
        }

        return Result;
    }

    // Hero.PAS:875
    public void DropItemsOnMap(AoWHSMap Map, sbyte X, sbyte Y, sbyte L)
    {
        HeroItemPosition Position;
        for (int I = FInventory.Count - 1; I >= 0; I--)
        {
            if (FInventory.GetItem(I) != null)
            {
                FInventory.GetItem(I).PlaceOnMap(Map, X, Y, L);
            }
        }

        for (int positionOrdinal = (int)HeroItemPosition.AttackSlot; positionOrdinal <= (int)HeroItemPosition.RightRingSlot; positionOrdinal++)
        {
            Position = (HeroItemPosition)positionOrdinal;
            if (FHeroItems.GetPositionItem(Position) != null)
            {
                FHeroItems.GetPositionItem(Position).PlaceOnMap(Map, X, Y, L);
            }
        }
    }

    // Hero.PAS:952
    public bool CompatibleHero(Hero Hero)
    {
        int GroupID;
        int OtherGroupID;
        GroupID = FGroupID;
        OtherGroupID = Hero.FGroupID;
        Hero.FGroupID = 0;
        FGroupID = 0;
        bool Result = Hero.Compare(this, GameTables.AoWEngine);
        Hero.FGroupID = OtherGroupID;
        FGroupID = GroupID;
        return Result;
    }

    // Hero.PAS:1006
    public int GetSkillPoints()
    {
        int Result;
        if (AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            var usedSkillPoints = UsedSkillPoints();
            Result = FLevel * 10 + 15 - usedSkillPoints - FUsedSkillPoints;
        }
        else
        {
            var usedSkillPoints = UsedSkillPoints();
            Result = FLevel * 10 + 15 - usedSkillPoints;
        }

        return Result;
    }

    // Hero.PAS:1013
    public int GetSkillPointsMax() => FLevel * 10 + 15 - FUsedSkillPoints;

    // Hero.PAS:1016
    public int GetLevelMax()
    {
        int Maximum;
        int Result = 30;
        if (AoWHSMap.Current != null && AoWHSMap.Current.FCampaignSettings != null && AoWHSMap.Current.FCampaignSettings.FHeroLevelMax > 0)
        {
            Maximum = AoWHSMap.Current.FCampaignSettings.FHeroLevelMax;
            if (Maximum < Result)
            {
                Result = Maximum;
            }
        }

        return Result;
    }

    // Hero.PAS:1040
    public int LevelToExperience(int Level)
    {
        int Total;
        int Quotient;
        Total = 0;
        for (int I = 1; I <= Level - 1; I++)
        {
            Quotient = I / 5;
            if (Quotient <= 5)
            {
                Total += HeroExperienceTable[Quotient].Experience;
            }
        }

        return Total;
    }

    // Hero.PAS:1049
    public sbyte ExperienceToLevel(int Experience)
    {
        int Level;
        int Total;
        int Maximum;
        Level = 1;
        Total = 0;
        Maximum = GetLevelMax();
        do
        {
            Total += HeroExperienceTable[Level / 5].Experience;
            if (Experience < Total)
            {
                break;
            }

            Level += 1;
        }
        while (!(Level >= Maximum));
        return (sbyte)Level;
    }

    // Hero.PAS:1071
    public sbyte GetLevel() => this.ExperienceToLevel(this.FExperience);

    // Hero.PAS:1076
    public void SetLevel(sbyte Value)
    {
        int Maximum;
        int Level;
        int Points;
        Maximum = GetLevelMax();
        if (Value < 1)
        {
            Value = 1;
        }

        if (Maximum < Value)
        {
            Value = (sbyte)Maximum;
        }

        Level = GetLevel();
        if (Level > Value)
        {
            Points = GetSkillPoints();
            while (Points >= 10 && Level != Value)
            {
                Level -= 1;
                Points -= 10;
            }

            Value = (sbyte)Level;
        }

        FExperience = LevelToExperience(Value);
        FLevel = (byte)Value;
    }

    // Hero.PAS:1097
    public HeroType GetHeroType() => FHeroResource.FHeroType;

    // Hero.PAS:1100
    public void SetUnitAttack(sbyte Value)
    {
        int Points;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value + FHeroResource.FAttack > 10)
        {
            Value = (sbyte)(10 - FHeroResource.FAttack);
        }

        if (Value > FUnitAttack)
        {
            Points = GetSkillPoints();
            while (Points >= 5 && Value != FUnitAttack)
            {
                FUnitAttack = (sbyte)(FUnitAttack + 1);
                Points -= 5;
            }
        }
        else
        {
            FUnitAttack = Value;
        }
    }

    // Hero.PAS:1117
    public void SetUnitDefense(sbyte Value)
    {
        int Points;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value + FHeroResource.FDefense > 10)
        {
            Value = (sbyte)(10 - FHeroResource.FDefense);
        }

        if (Value > FUnitDefense)
        {
            Points = GetSkillPoints();
            while (Points >= 5 && Value != FUnitDefense)
            {
                FUnitDefense = (sbyte)(FUnitDefense + 1);
                Points -= 5;
            }
        }
        else
        {
            FUnitDefense = Value;
        }
    }

    // Hero.PAS:1134
    public void SetUnitDamage(sbyte Value)
    {
        int Points;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value + FHeroResource.FDamage > 10)
        {
            Value = (sbyte)(10 - FHeroResource.FDamage);
        }

        if (Value > FUnitDamage)
        {
            Points = GetSkillPoints();
            while (Points >= 10 && Value != FUnitDamage)
            {
                FUnitDamage = (sbyte)(FUnitDamage + 1);
                Points -= 10;
            }
        }
        else
        {
            FUnitDamage = Value;
        }
    }

    // Hero.PAS:1151
    public void SetUnitHits(sbyte Value)
    {
        int Points;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value + FHeroResource.FHits > 30)
        {
            Value = (sbyte)(30 - FHeroResource.FHits);
        }

        if (Value > FUnitHits)
        {
            Points = GetSkillPoints();
            while (Points >= 5 && Value != FUnitHits)
            {
                FUnitHits = (sbyte)(FUnitHits + 1);
                Points -= 5;
            }
        }
        else
        {
            FUnitHits = Value;
        }
    }

    // Hero.PAS:1168
    public void SetUnitMoves(sbyte Value)
    {
        int Points;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value + FHeroResource.FMoves > 50)
        {
            Value = (sbyte)(50 - FHeroResource.FMoves);
        }

        if (Value > FUnitMoves)
        {
            Points = GetSkillPoints();
            while (Points >= 2 && Value != FUnitMoves)
            {
                FUnitMoves = (sbyte)(FUnitMoves + 1);
                Points -= 2;
            }
        }
        else
        {
            FUnitMoves = Value;
        }
    }

    // Hero.PAS:1185
    public void SetUnitResistance(sbyte Value)
    {
        int Points;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value + FHeroResource.FResistance > 10)
        {
            Value = (sbyte)(10 - FHeroResource.FResistance);
        }

        if (Value > FUnitResistance)
        {
            Points = GetSkillPoints();
            while (Points >= 5 && Value != FUnitResistance)
            {
                FUnitResistance = (sbyte)(FUnitResistance + 1);
                Points -= 5;
            }
        }
        else
        {
            FUnitResistance = Value;
        }
    }

    // Hero.PAS:1202
    public void LinkToResource()
    {
        FHeroResource = AoWHSSet.Current.FHeroResources.GetHeroResource(FHeroResourceIndex);
    }

    // Hero.PAS:1207
    public void Loaded()
    {
        if (FHeroResource != null)
        {
            UpdateDefaultAbilities(FHeroResource.FAbilities);
            SetUnitAttack(FUnitAttack);
            SetUnitDefense(FUnitDefense);
            SetUnitResistance(FUnitResistance);
            SetUnitDamage(FUnitDamage);
            SetUnitHits(FUnitHits);
            SetUnitMoves(FUnitMoves);
        }
    }

    // Hero.PAS:1221
    public void ListSpells(ref SpellList List)
    {
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.ListSpells(ref List, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
    }

    // Hero.PAS:1226
    public void ExecuteUpgradeHeroAI()
    {
        DoubleItemList Choices;
        int Points;
        int Index;
        DoubleItemList.DoubleItem Choice;
        Choices = new DoubleItemList();
        while (true)
        {
            Choices.Clear();
            Points = GetSkillPoints();
            if (Points >= 5 && FUnitAttack + FHeroResource.FAttack < 10)
            {
                Choices.AddAB(0, 10);
            }

            if (Points >= 5 && FUnitDefense + FHeroResource.FDefense < 10)
            {
                Choices.AddAB(1, 25);
            }

            if (Points >= 5 && FUnitResistance + FHeroResource.FResistance < 10)
            {
                Choices.AddAB(2, 10);
            }

            if (Points >= 10 && FUnitDamage + FHeroResource.FDamage < 10)
            {
                Choices.AddAB(3, 10);
            }

            if (Points >= 5 && FUnitHits + FHeroResource.FHits < 30)
            {
                Choices.AddAB(4, 10);
            }

            if (AoWHSSet.Current.FAbilityControl.GetAbility(46).CanExpand(this) && Points >= AoWHSSet.Current.FAbilityControl.GetAbility(46).ExpandCost(this))
            {
                Choices.AddAB(5, 10);
            }

            if (AoWHSSet.Current.FAbilityControl.GetAbility(22).CanExpand(this) && Points >= AoWHSSet.Current.FAbilityControl.GetAbility(22).ExpandCost(this))
            {
                Choices.AddAB(6, 10);
            }

            if (AoWHSSet.Current.FAbilityControl.GetAbility(52).CanExpand(this))
            {
                if (Points >= AoWHSSet.Current.FAbilityControl.GetAbility(52).ExpandCost(this))
                {
                    Choices.AddAB(7, 9999);
                }
                else
                {
                    Choices.AddAB(7, 100);
                }
            }

            if (Choices.Count != 0)
            {
                RandSeed = AoWHSMap.Current.Random(16777215);
                Index = Choices.RandomChanceItemB(0);
                Choice = Choices[Index];
                switch (Choice.A)
                {
                    case 0:
                        SetUnitAttack((sbyte)(FUnitAttack + 1));
                        break;
                    case 1:
                        SetUnitDefense((sbyte)(FUnitDefense + 1));
                        break;
                    case 2:
                        SetUnitResistance((sbyte)(FUnitResistance + 1));
                        break;
                    case 3:
                        SetUnitDamage((sbyte)(FUnitDamage + 1));
                        break;
                    case 4:
                        SetUnitHits((sbyte)(FUnitHits + 1));
                        break;
                    case 5:
                        ExpandAbility(46);
                        break;
                    case 6:
                        ExpandAbility(22);
                        break;
                    case 7:
                        if (Points >= AoWHSSet.Current.FAbilityControl.GetAbility(52).ExpandCost(this))
                        {
                            ExpandAbility(52);
                        }
                        else
                        {
                            Points = 0;
                        }

                        break;
                }
            }

            if (Choices.Count == 0 || GetSkillPoints() == 0 || Points == 0)
                break;
        }

        Choices?.Dispose();
    }

    // Hero.PAS:1269
    public void ValidateHeroUpgrade()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        int Level;
        int OldLevel;
        HeroUpgradeEventLog Log;
        GetLocation(ref X, ref Y, ref L);
        if (FPlayer != 0 && X != -1)
        {
            FUpgradePending = false;
            Level = GetLevel();
            if (Level < GetLevelMax())
            {
                if (FPlayer > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.CPU2)
                {
                    SetExperience(GetExperience() + (HeroExperienceTable[Level / 5].DailyExperience + 1));
                }
                else
                {
                    if (FPlayer > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.CPU3)
                    {
                        SetExperience(GetExperience() + (HeroExperienceTable[Level / 5].DailyExperience + 2));
                    }
                    else
                    {
                        if (FPlayer > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.CPU4)
                        {
                            SetExperience(GetExperience() + (HeroExperienceTable[Level / 5].DailyExperience + 3));
                        }
                        else
                        {
                            if (FPlayer > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.CPU5)
                            {
                                SetExperience(GetExperience() + (HeroExperienceTable[Level / 5].DailyExperience + 4));
                            }
                            else
                            {
                                SetExperience(GetExperience() + HeroExperienceTable[Level / 5].DailyExperience);
                            }
                        }
                    }
                }

                Level = GetLevel();
            }

            if (FLevel < Level)
            {
                FUpgradePending = true;
                OldLevel = FLevel;
                FLevel = (byte)Level;
                Log = new HeroUpgradeEventLog(null);
                Log.FOldLevel = (byte)OldLevel;
                Log.FNewLevel = FLevel;
                Log.FDay = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDay;
                Log.FPlayer = FPlayer;
                Log.FUnitID = FUnitID;
                EventLog.DistributeLocationEventLog(Log, FPlayer, X, Y, L, true);
                Log.Release();
                if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human)
                {
                    ExecuteUpgradeHeroAI();
                }
            }
        }
    }

    // Hero.PAS:1350
    public bool GetUnitAbilityEnabled(int AbilityID) => base.GetAbilityEnabled(AbilityID);

    // Hero.PAS:1363
    public AbilityOwner GetSuperlativeEnabledAbOwner(int AbilityID)
    {
        AbilityOwner Candidate;
        AbilityOwner Best;
        if (GetAbEnabled(AbilityID))
        {
            Best = this;
        }
        else
        {
            Best = null;
        }

        Candidate = FHeroItems.GetSuperlativeEnabledAbOwner(AbilityID);
        if (Candidate != null)
        {
            if (Best == null || Candidate.GetAbLevel(AbilityID) > Best.GetAbLevel(AbilityID))
            {
                Best = Candidate;
            }
        }

        Candidate = FInventory.GetSuperlativeEnabledAbOwner(AbilityID);
        if (Candidate != null)
        {
            if (Best == null || Candidate.GetAbLevel(AbilityID) > Best.GetAbLevel(AbilityID))
            {
                Best = Candidate;
            }
        }

        return Best;
    }

    // Hero.PAS:1378
    public AbilityOwner GetSuperlativeAbOwner(int AbilityID)
    {
        AbilityOwner Candidate;
        AbilityOwner Best;
        if (GetAbSet(AbilityID))
        {
            Best = this;
        }
        else
        {
            Best = null;
        }

        Candidate = FHeroItems.GetSuperlativeAbOwner(AbilityID);
        if (Candidate != null)
        {
            if (Best == null || Candidate.GetAbLevel(AbilityID) > Best.GetAbLevel(AbilityID))
            {
                Best = Candidate;
            }
        }

        Candidate = FInventory.GetSuperlativeAbOwner(AbilityID);
        if (Candidate != null)
        {
            if (Best == null || Candidate.GetAbLevel(AbilityID) > Best.GetAbLevel(AbilityID))
            {
                Best = Candidate;
            }
        }

        return Best;
    }

    // Hero.PAS:1544
    public void ResetSkillPoints()
    {
        AssignAb(FHeroResource.FAbilities);
        FUnitAttack = 0;
        FUnitDefense = 0;
        FUnitDamage = 0;
        FUnitHits = 0;
        FUnitMoves = 0;
        FUnitResistance = 0;
    }

    // Hero.PAS:1555
    public void ResetResourceSettings()
    {
        AssignAb(FHeroResource.FAbilities);
        FUnitAttack = 0;
        FUnitDefense = 0;
        FUnitDamage = 0;
        FUnitHits = 0;
        FUnitMoves = 0;
        FUnitResistance = 0;
        FExperience = 0;
        FRace = FHeroResource.FRace;
        if (AoWHSMap.Current != null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            FFaceIndex = FHeroResource.FFaceIndex;
        }

        Changed();
    }

    // Hero.PAS:1570
    public void UpdateSettings()
    {
        FMovePoints = GetMoves();
        FHitPoints = GetHits();
        FCastingPoints = GetCastingPointsMax();
        FLevel = (byte)ExperienceToLevel(FExperience);
        Changed();
    }

    // Hero.PAS:1579
    public void SetRace(sbyte Value)
    {
        bool Found;
        int Index;
        HeroResource Resource;
        if (FRace != Value)
        {
            Found = false;
            Index = AoWHSSet.Current.FHeroResources.GetCount();
            while (!Found && Index != 0)
            {
                Index -= 1;
                Resource = AoWHSSet.Current.FHeroResources.GetHeroResource(Index);
                Found = Resource != null && Resource.FRace == Value && GetHeroType() == Resource.FHeroType;
            }

            if (Found)
            {
                SetHeroResourceIndex(Index);
            }
        }
    }

    // Hero.PAS:1612
    public void SetFaceIndex(int Value)
    {
        FFaceIndex = Value;
    }

    // Hero.PAS:1708
    public HeroUpdateTokenEvent CreateHeroUpdateTE()
    {
        HeroUpdateTokenEvent Result = new HeroUpdateTokenEvent(null);
        Result.FUnitID = FUnitID;
        return Result;
    }

    // Hero.PAS:1714
    public ItemList GetItemsOnGround()
    {
        ItemList Result;
        CustomCombatMap Map;
        Map = AoWHSMap.Current.FCombatMap;
        if (Map != null)
        {
            Result = Map.GetHeroItemsOnGround(this);
        }
        else
        {
            Result = AoWHSMap.Current.GetHeroItemsOnGround(this);
        }

        return Result;
    }

    // Hero.PAS:1722
    public bool CanUseItem(int ItemID, ref string Reason)
    {
        bool Result;
        Item Item;
        Item = AoWHSMap.Current.FItemControl.FindItem(ItemID);
        if (Item != null)
        {
            Result = Item.CanUse(this, ref Reason);
        }
        else
        {
            Result = false;
            Reason = "";
        }

        return Result;
    }

    // Hero.PAS:1734
    public bool UseItem(int ItemID)
    {
        bool Result;
        Item Item;
        Item = AoWHSMap.Current.FItemControl.FindItem(ItemID);
        if (Item != null)
        {
            Result = Item.Use(this);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1742
    public bool CanPlaceItem(int ItemID, HeroItemPosition Position)
    {
        Item Item;
        sbyte IX = default;
        sbyte IY = default;
        sbyte IL = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = false;
        Item = AoWHSMap.Current.FItemControl.FindItem(ItemID);
        if (Item != null)
        {
            GetLocation(ref X, ref Y, ref L);
            Item.GetLocation(ref IX, ref IY, ref IL);
            if (AoWHSMap.Current.FCombatMap != null || (X == IX && Y == IY && L == IL))
            {
                if (Set<HeroItemPosition>.Of().WithRange(HeroItemPosition.AttackSlot, HeroItemPosition.RightRingSlot).Contains(Position))
                {
                    Result = FHeroItems.CanPlaceItem(Item, Position);
                }
                else
                {
                    if (Set<HeroItemPosition>.Of().WithRange(HeroItemPosition.InventorySlot1, HeroItemPosition.InventorySlot8).Contains(Position))
                    {
                        Result = FInventory.GetSlot(Position) == null;
                    }
                    else
                    {
                        Result = true;
                    }
                }
            }
        }

        return Result;
    }

    // Hero.PAS:1758
    public bool CanSwapItem(int ItemID1, int ItemID2)
    {
        Item Item1;
        Item Item2;
        bool Result = true;
        Item1 = AoWHSMap.Current.FItemControl.FindItem(ItemID1);
        Item2 = AoWHSMap.Current.FItemControl.FindItem(ItemID2);
        if (Item1 != null && Item2 != null && Item1 != Item2)
        {
            if (FHeroItems == Item1.Owner || FHeroItems == Item2.Owner)
            {
                Result = Item1.FItemType == Item2.FItemType;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1772
    public bool SwapItem(int ItemID1, int ItemID2)
    {
        bool Result;
        HeroUpdateTokenEvent TE;
        if (CanSwapItem(ItemID1, ItemID2) && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            TE = CreateHeroUpdateTE();
            TE.FMode = HeroUpdateTokenEvent.Mode.SwapItem;
            TE.FItemID = ItemID1;
            TE.FParameter = ItemID2;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1788
    public bool PlaceItem(int ItemID, HeroItemPosition Position)
    {
        bool Result;
        HeroUpdateTokenEvent TE;
        if (CanPlaceItem(ItemID, Position) && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            TE = CreateHeroUpdateTE();
            TE.FMode = HeroUpdateTokenEvent.Mode.PlaceItem;
            TE.FItemID = ItemID;
            TE.FParameter = (int)Position;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1804
    public bool ExecuteSwapItem(int ItemID1, int ItemID2)
    {
        bool Result;
        Item Item1;
        Item Item2;
        ItemList Owner1;
        ItemList Owner2;
        int Index1;
        int Index2;
        if (CanSwapItem(ItemID1, ItemID2))
        {
            Item1 = AoWHSMap.Current.FItemControl.FindItem(ItemID1);
            Item2 = AoWHSMap.Current.FItemControl.FindItem(ItemID2);
            Owner1 = (ItemList)Item1.Owner;
            Owner2 = (ItemList)Item2.Owner;
            Owner1.BeginUpdate();
            Owner2.BeginUpdate();
            Index1 = Item1.Owner.FindChildIndex(Item1);
            Index2 = Item2.Owner.FindChildIndex(Item2);
            Item1.SetOwner(null);
            Item2.SetOwner(null);
            if (Owner1.GetControlStyle().Contains(ControlStyleElement.ArrayMode))
            {
                Item2.SetArrayOwner(Owner1, Index1);
            }
            else
            {
                Item2.SetOwner(Owner1);
            }

            if (Owner2.GetControlStyle().Contains(ControlStyleElement.ArrayMode))
            {
                Item1.SetArrayOwner(Owner2, Index2);
            }
            else
            {
                Item1.SetOwner(Owner2);
            }

            Owner2.EndUpdate();
            Owner1.EndUpdate();
            Changed();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1831
    public bool AutoPlaceItem(Item Item)
    {
        HeroItemPosition Position;
        int Slot;
        bool Result = false;
        Position = FHeroItems.GetItemTypePosition(Item.FItemType);
        if (FHeroItems.CanPlaceItem(Item, Position))
        {
            FHeroItems.PlaceItem(Item, Position);
            Result = true;
        }
        else
        {
            Slot = FInventory.GetFreeSlot();
            if (Slot != -1)
            {
                Item.SetArrayOwner(FInventory, Slot);
                Result = true;
            }
        }

        return Result;
    }

    // Hero.PAS:1852
    public bool ExecutePlaceItem(int ItemID, HeroItemPosition Position)
    {
        bool Result;
        Item Item;
        CustomCombatMap Map;
        if (CanPlaceItem(ItemID, Position))
        {
            Item = AoWHSMap.Current.FItemControl.FindItem(ItemID);
            switch (Position)
            {
                case HeroItemPosition.Ground:
                    Map = AoWHSMap.Current.FCombatMap;
                    if (Map != null)
                    {
                        Map.DropHeroItem(this, Item);
                    }
                    else
                    {
                        AoWHSMap.Current.DropHeroItem(this, Item);
                    }

                    Changed();
                    break;
                case >= HeroItemPosition.AttackSlot and <= HeroItemPosition.RightRingSlot:
                    FHeroItems.PlaceItem(Item, Position);
                    Changed();
                    break;
                case >= HeroItemPosition.InventorySlot1 and <= HeroItemPosition.InventorySlot8:
                    Item.SetArrayOwner(FInventory, (int)Position - 6);
                    Changed();
                    break;
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1876
    public bool ExecuteStartCasting(int SpellID)
    {
        bool Result;
        Spell Spell;
        if (FCastingSpellID != 0)
        {
            ExecuteCancelCasting();
        }

        Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
        var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
        var castingMana = CastingMana(Spell);
        if (players.FMagicControl.FMana >= castingMana && CastingMana(Spell) > FCastingPoints)
        {
            FCastingProgress = CastingMana(Spell);
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FMana - FCastingProgress);
            FCastingMana = FCastingPoints;
            FCastingPoints = 0;
            if (AoWHSMap.Current.FSeatedPlayer == FPlayer)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(CastingXInProgressRStr), [Spell.FSpellName]);
                AoWHSMap.Current.ShowMessage(format);
            }

            FCastingSpellID = SpellID;
            Changed();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1898
    public void RemoveCastingReadyEventLog()
    {
        EventLogbook Logbook;
        Logbook = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FEventLogbook;
        for (int I = Logbook.GetCount() - 1; I >= 0; I--)
        {
            if (Logbook.GetLogs(I) is SpellCastEventLog && ((SpellCastEventLog)Logbook.GetLogs(I)).FUnitID == FUnitID)
            {
                Logbook.GetLogs(I).SetOwner(null);
            }
        }
    }

    // Hero.PAS:1907
    public bool ExecuteCancelCasting()
    {
        bool Result;
        if (FCastingSpellID != 0)
        {
            RemoveCastingReadyEventLog();
            if (GetCastingPointsMax() < FCastingPoints + FCastingMana)
            {
                FCastingPoints = GetCastingPointsMax();
            }
            else
            {
                FCastingPoints = (sbyte)(FCastingPoints + FCastingMana);
            }

            var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.SetMana(players.FMagicControl.FMana + FCastingProgress);
            FCastingSpellID = 0;
            FCastingMana = 0;
            FCastingProgress = 0;
            Changed();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:1925
    public void CastingDone(int SpellID, int Mana)
    {
        if (SpellID == FCastingSpellID)
        {
            Assert(CastingReady(), "", "Hero.PAS", 2080);
            FCastingMana = 0;
            FCastingProgress = 0;
            FCastingSpellID = 0;
            RemoveCastingReadyEventLog();
        }
        else
        {
            if (FCastingSpellID != 0)
            {
                ExecuteCancelCasting();
            }

            if (!AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.SetMana(players.FMagicControl.FMana - Mana);
                FCastingPoints = (sbyte)(FCastingPoints - Mana);
            }
        }
    }

    // Hero.PAS:2098
    public int CastingMana(Spell Spell)
    {
        int Result = Spell.FBaseManaCost;
        if (AoWHSMap.Current.FGlobalMagicControl.GetSphereManaDoubled(Spell.FMagicSphere))
        {
            Result = Result * 2;
        }

        return Result;
    }

    // Hero.PAS:2104
    public int CastingTurns(Spell Spell)
    {
        int Result;
        int Mana;
        int Points;
        Mana = CastingMana(Spell);
        Points = FCastingPoints + FCastingMana;
        if (GetCastingPointsMax() < Points)
        {
            Points = GetCastingPointsMax();
        }

        if (Points < Mana)
        {
            Result = (Mana - Points) / GetCastingPointsMax();
            if ((Mana - Points) % GetCastingPointsMax() != 0)
            {
                Result += 1;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Hero.PAS:2118
    public bool Casting() => FCastingSpellID != 0;

    // Hero.PAS:2121
    public bool CastingReady() => FCastingSpellID != 0 && FCastingMana == FCastingProgress;

    // Hero.PAS:2124
    public int CastingTurnsLeft()
    {
        int Result;
        if (FCastingSpellID != 0 && GetCastingPointsMax() != 0)
        {
            if (FCastingMana < FCastingProgress)
            {
                var castingPointsMax = GetCastingPointsMax();
                Result = (FCastingProgress - FCastingMana) / castingPointsMax;
                var castingPointsMax2 = GetCastingPointsMax();
                if ((FCastingProgress - FCastingMana) % castingPointsMax2 != 0)
                {
                    Result += 1;
                }
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Hero.PAS:2136
    public bool CanCastSpellInstantly(int SpellID, int Mana, ref string Reason)
    {
        bool Result;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
        {
            if (Mana <= FCastingPoints || SpellID == FCastingSpellID && Casting() && CastingReady())
            {
                Result = CanCastSpell(SpellID, ref Reason);
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(GameText.P2NotEnoughCastingPointsRStr);
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Hero.PAS:2149
    public bool CanCastSpell(int SpellID, ref string Reason)
    {
        bool Result;
        Spell Spell;
        if (FCastingSpellID == 0)
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
            var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            var castingMana = CastingMana(Spell);
            if (players.FMagicControl.FMana >= castingMana || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
            {
                Result = Spell.CanActivate(this, ref Reason);
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
            }
        }
        else
        {
            Result = SpellID == FCastingSpellID;
            if (Result && AoWHSSet.Current.FSpellControl.GetSpell(SpellID).FSpellType == SpellType.Global && AoWHSMap.Current.FCombatMap != null)
            {
                Reason = AbilityRules.TranslateRStr(CannotCastSpellInCombatRStr);
                Result = false;
            }
        }

        return Result;
    }

    // Hero.PAS:2176
    public bool CastSpell(int SpellID, ref string Reason)
    {
        bool Result;
        Spell Spell;
        HeroUpdateTokenEvent TE;
        if (CanCastSpell(SpellID, ref Reason))
        {
            if (FCastingSpellID == 0)
            {
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
                if (CastingMana(Spell) <= FCastingPoints || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
                {
                    Result = Spell.CanActivate(this, ref Reason) && Spell.Activate(this);
                }
                else
                {
                    if (!AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
                    {
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                        var castingMana = CastingMana(Spell);
                        if (players.FMagicControl.FMana >= castingMana)
                        {
                            TE = CreateHeroUpdateTE();
                            TE.FMode = HeroUpdateTokenEvent.Mode.StartCasting;
                            TE.FItemID = SpellID;
                            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
                            TE.Release();
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
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
                if (CastingReady())
                {
                    Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
                    Result = Spell.CanActivate(this, ref Reason) && Spell.Activate(this);
                }
                else
                {
                    Result = true;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:2220
    public bool CanCastCombatSpell(int SpellID, ref string Reason)
    {
        bool Result;
        Spell Spell;
        if (FCastingSpellID == 0)
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
            if (CastingMana(Spell) <= FCastingPoints || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                var castingMana = CastingMana(Spell);
                if (players.FMagicControl.FMana >= castingMana || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
                {
                    Result = Spell.CanActivateCombat(this, ref Reason);
                }
                else
                {
                    Result = false;
                    Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
                }
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(NotEnoughChannelingPointsRStr);
            }
        }
        else
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
            Result = Set<SpellType>.Of(SpellType.Combat, SpellType.Unit).Contains(Spell.FSpellType) && CanCastSpellInstantly(SpellID, CastingMana(Spell), ref Reason);
        }

        return Result;
    }

    // Hero.PAS:2252
    public bool CastCombatSpell(int SpellID, ref string Reason)
    {
        bool Result;
        Spell Spell;
        Spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
        if (CanCastCombatSpell(SpellID, ref Reason))
        {
            Result = Spell.CanActivateCombat(this, ref Reason) && Spell.ActivateCombat(this);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:2264
    public bool CancelCasting()
    {
        bool Result;
        HeroUpdateTokenEvent TE;
        // { Original tests -1 although zero is the idle spell ID. }
        if (FCastingSpellID != -1 && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            TE = CreateHeroUpdateTE();
            TE.FMode = HeroUpdateTokenEvent.Mode.CancelCasting;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string CastingXInProgressRStr = "Casting %s...";
    private static readonly string CannotCastSpellInCombatRStr = "Cannot cast spell in combat";
    private static readonly string NotEnoughChannelingPointsRStr = "Not enough channeling points";
    private static HeroLevelArray6 HeroExperienceTable = HeroLevelArray6.From([new HeroLevel { DailyExperience = 1, Experience = 15 }, new HeroLevel { DailyExperience = 1, Experience = 15 }, new HeroLevel { DailyExperience = 1, Experience = 20 }, new HeroLevel { DailyExperience = 1, Experience = 20 }, new HeroLevel { DailyExperience = 1, Experience = 25 }, new HeroLevel { DailyExperience = 1, Experience = 25 }]);
    internal static SByteArray5 P2MoraleResistance = SByteArray5.From([-2, -1, 0, 0, 0]);
    internal static SByteArray5 P2MoraleDefense = SByteArray5.From([-2, -1, 0, 0, 0]);

}
