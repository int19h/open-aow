// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

public partial class Unit : Troop
{
    public new const int ClassID = 131603;

    public Unit(ECustomNode owner) : base(owner)
    { }

    public byte FExperience;
    public sbyte FMovePoints;
    public sbyte FHitPoints;
    public UnitResource FUnitResource;
    public sbyte FAbAttack;
    public sbyte FAbDefense;
    public sbyte FAbResistance;
    public sbyte FAbDamage;

    // Unit.PAS:2132
    public override void ReadWrite(EStorageStream Stream)
    {
        short ResourceIndex = default;
        UnitResource Resource;
        base.ReadWrite(Stream);
        Stream.rwByte(ref FExperience, 7);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMovePoints), 8);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FHitPoints), 9);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly)) == Set<StorageModeElement>.Of())
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Compare))
            {
                ResourceIndex = (short)FUnitResource.UnitResourceIndex();
                Stream.rwWord(ref Unsafe.As<short, ushort>(ref ResourceIndex), 10);
            }
            else
            {
                if (Stream.StorageMode().Contains(StorageModeElement.Read))
                {
                    Stream.rwWord(ref Unsafe.As<short, ushort>(ref ResourceIndex), 10);
                    Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(ResourceIndex);
                    FUnitResource = Resource;
                    if (Resource == null)
                    {
                        throw new KeyNotFoundException("unit resource missing (" + IntToStr(ResourceIndex) + ")");
                    }
                }
                else
                {
                    Resource = FUnitResource;
                    if (Resource != null)
                    {
                        ResourceIndex = (short)Resource.UnitResourceIndex();
                    }
                    else
                    {
                        ResourceIndex = -1;
                    }

                    Stream.rwWord(ref Unsafe.As<short, ushort>(ref ResourceIndex), 10);
                }
            }
        }
    }

    // Unit.PAS:2067
    public override void Changed()
    {
        base.Changed();
        FAbAttack = this.GetAbAttackAll();
        FAbDefense = this.GetAbDefenseAll();
        FAbResistance = this.GetAbResistanceAll();
        FAbDamage = this.GetAbDamageAll();
    }

    // Unit.PAS:2079
    public override sbyte GetUnitLevel() => FUnitResource.FLevel;

    // Unit.PAS:2118
    public override sbyte GetRace() => FUnitResource.FRace;

    // Unit.PAS:2025
    public override sbyte GetInherentAttack()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.P2RankAttack[(int)rank] + FUnitResource.FAttack);
    }

    // Unit.PAS:2033
    public override sbyte GetInherentDefense()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.P2RankDefense[(int)rank] + FUnitResource.FDefense);
    }

    // Unit.PAS:2041
    public override sbyte GetInherentDamage()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.P2RankDamage[(int)rank] + FUnitResource.FDamage);
    }

    // Unit.PAS:2049
    public override sbyte GetInherentResistance() => FUnitResource.FResistance;

    // Unit.PAS:2027
    public override sbyte GetAttack()
    {
        var rank = GetRank();
        sbyte Result = (sbyte)(RankTables.P2RankAttack[(int)rank] + (FUnitResource.FAttack + FAbAttack));
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

    // Unit.PAS:2035
    public override sbyte GetDefense()
    {
        var rank = GetRank();
        var morale = GetMorale();
        sbyte Result = (sbyte)(RankTables.P2RankDefense[(int)rank] + (FUnitResource.FDefense + FAbDefense) + Hero.P2MoraleDefense[(int)morale]);
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

    // Unit.PAS:2043
    public override sbyte GetDamage()
    {
        var rank = GetRank();
        sbyte Result = (sbyte)(RankTables.P2RankDamage[(int)rank] + (FUnitResource.FDamage + FAbDamage));
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

    // Unit.PAS:2051
    public override sbyte GetResistance()
    {
        var morale = GetMorale();
        var rank = GetRank();
        sbyte Result = (sbyte)(Hero.P2MoraleResistance[(int)morale] + (FUnitResource.FResistance + FAbResistance) + RankTables.P2RankResistance[(int)rank]);
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

    // Unit.PAS:2075
    public override sbyte GetHits()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.P2RankHits[(int)rank] + FUnitResource.FHits);
    }

    // Unit.PAS:2077
    public override sbyte GetMoves() => FUnitResource.FMoves;

    // Unit.PAS:1930
    public override sbyte GetMovePoints() => FMovePoints;

    // Unit.PAS:1937
    public override void SetMovePoints(sbyte Value)
    {
        if (Value > GetMoves())
        {
            Value = GetMoves();
        }

        if (Value < 0)
        {
            Value = 0;
        }

        FMovePoints = Value;
    }

    // Unit.PAS:1943
    public override sbyte GetHitPoints() => FHitPoints;

    // Unit.PAS:1945
    public override void SetHitPoints(sbyte Value)
    {
        if (Value > GetHits())
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

    // Unit.PAS:1972
    public override LibraryImage GetFace()
    {
        LibraryImage Result;
        int FaceIndex;
        if (!GetAbilityEnabled(126))
        {
            return AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(FUnitResource.FUnitGFXResourceIndex).GetFaceImage();
        }

        FaceIndex = ((CosmeticSurgeryEnchantment)(object)((UnitEnchantmentAbilityData)this.GetAbilityData(126)).FEnchantment).FFaceIndex;
        if (AoWHSSet.Current.FFaceResources.GetFaceResource(FaceIndex) != null)
        {
            Result = AoWHSSet.Current.FFaceResources.GetFaceResource(FaceIndex).GetCellImage();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Unit.PAS:1917
    public override string GetName() => FUnitResource.GetUnitName();

    // Unit.PAS:1932
    public override Alignment GetAlignment()
    {
        if (GetAbilityEnabled(107))
        {
            return Alignment.PureEvil;
        }

        return FUnitResource.FAlignment;
    }

    // Unit.PAS:2081
    public override LibraryImage GetPreviewImage() => FUnitResource.GetPreviewImage();

    // Unit.PAS:2083
    public override byte GetTransportCapacity()
    {
        byte Result = (byte)GetAbilityLevel(50);
        if (Result < FUnitResource.GetTransportCapacity())
        {
            Result = FUnitResource.GetTransportCapacity();
        }

        return Result;
    }

    // Unit.PAS:2088
    public override bool GetTransporter() => FUnitResource.GetTransportCapacity() != 0;

    // Unit.PAS:1958
    public override UnitGender GetGender() => FUnitResource.FGender;

    // Unit.PAS:1954
    public override BloodType GetBloodType() => FUnitResource.FBloodType;

    // Unit.PAS:1956
    public override UnitType GetUnitType() => FUnitResource.FUnitType;

    // Unit.PAS:2108
    public override int GetUnitGFXResourceIndex() => FUnitResource.FUnitGFXResourceIndex;

    // Unit.PAS:2120
    public override void NewDay()
    {
        Rank Rank;
        base.NewDay();
        if (AoWHSMap.Current.FDay == 1)
        {
            SetAbCount(0);
            for (int rankOrdinal = (int)Unit.Rank.None, last = (int)GetRank(); rankOrdinal <= last; rankOrdinal++)
            {
                Rank = (Rank)rankOrdinal;
                UpdateDefaultAbilities(FUnitResource.FAbilities[(int)Rank]);
            }
        }
    }

    // Unit.PAS:1986
    public override int GetExperience() => FExperience;

    // Unit.PAS:1988
    public override void SetExperience(int Value)
    {
        Rank OldRank;
        Rank NewRank;
        Rank Rank;
        OldRank = GetRank();
        if (Value > 255)
        {
            Value = 255;
        }

        FExperience = (byte)Value;
        NewRank = GetRank();
        if (OldRank != NewRank)
        {
            for (int rankOrdinal = (int)(Rank)((int)OldRank + 1); rankOrdinal <= (int)NewRank; rankOrdinal++)
            {
                Rank = (Rank)rankOrdinal;
                UpdateDefaultAbilities(FUnitResource.FAbilities[(int)Rank]);
            }
        }

        Changed();
    }

    // Unit.PAS:2002
    public override int GetNextLevelExperience()
    {
        int Result;
        int Level;
        Level = FUnitResource.FLevel;
        if (FExperience >= Level * 2)
        {
            if (FExperience >= Level * 6)
            {
                Result = 0;
            }
            else
            {
                return Level * 6;
            }
        }
        else
        {
            Result = Level * 2;
        }

        return Result;
    }

    // Unit.PAS:2000
    public override StringCollection GetDescription() => FUnitResource.FDescription;

    // Unit.PAS:1928
    public override int GetObtainValue() => FUnitResource.FObtainValue;

    // Unit.PAS:1952
    public override UnitSize GetUnitSize() => FUnitResource.FUnitSize;

    // Unit.PAS:2110
    public override void Activate()
    {
        if (FUnitResource != null)
        {
            base.Activate();
            return;
        }

        AddRef();
        AddRef();
        AddRef();
        Dispose();
    }

    // Unit.PAS:1919
    public override int ShowEx(int X, int Y, in LockedSurface SD, in UnitShowInfo Info)
    {
        int Result = base.ShowEx(X, Y, SD, Info);
        if (Info.Overlays.Contains(UnitOverlayModesElement.Rank))
        {
            switch (GetRank())
            {
                case Rank.Silver:
                    AoWHSSet.Current.FCombatImages[5].Show(X + 33, Y + 12, SD);
                    break;
                case Rank.Gold:
                    AoWHSSet.Current.FCombatImages[6].Show(X + 33, Y + 12, SD);
                    break;
            }
        }

        return Result;
    }

    // Unit.PAS:1966
    public override void UnitKilled()
    {
    }

    // Unit.PAS:1960
    public override void Killed()
    {
        base.Killed();
        Deactivate();
        SetOwner(null);
    }

    // Unit.PAS:2009
    public Rank GetRank()
    {
        Rank Result;
        if (FExperience >= FUnitResource.FLevel * 2)
        {
            if (FExperience >= FUnitResource.FLevel * 6)
            {
                Result = Rank.Gold;
            }
            else
            {
                return Rank.Silver;
            }
        }
        else
        {
            Result = Rank.None;
        }

        return Result;
    }

    // Unit.PAS:2016
    public void SetRank(Rank Value)
    {
        if (Value != GetRank())
        {
            switch (Value)
            {
                case Rank.None:
                    FExperience = 0;
                    break;
                case Rank.Silver:
                    SetExperience(FUnitResource.FLevel * 2);
                    break;
                case Rank.Gold:
                    SetExperience(FUnitResource.FLevel * 6);
                    break;
            }
        }
    }

    // Unit.PAS:2090
    public void SetUnitResource(UnitResource Resource)
    {
        Rank Rank;
        if (Resource != FUnitResource)
        {
            base.Clear();
            FUnitResource = Resource;
            if (FUnitResource != null)
            {
                UpdateMoraleValue();
                for (int rankOrdinal = (int)Unit.Rank.None, last = (int)GetRank(); rankOrdinal <= last; rankOrdinal++)
                {
                    Rank = (Rank)rankOrdinal;
                    UpdateDefaultAbilities(FUnitResource.FAbilities[(int)Rank]);
                }

                FHitPoints = GetHits();
                FMovePoints = GetMoves();
            }
        }
    }

    public enum Rank : byte
    {
        None = 0, Silver = 1, Gold = 2
    }

}
