// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

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
    public new const int ClassId = 131603;

    public Unit(ECustomNode owner) : base(owner)
    { }

    private byte _experience;
    private sbyte _movePoints;
    private sbyte _hitPoints;

    public UnitResource UnitResource { get; set; }
    public sbyte AbilityAttack { get; set; }

    private sbyte _abilityDefense;
    private sbyte _abilityResistance;
    private sbyte _abilityDamage;

    // Unit.PAS:2132
    public override void ReadWrite(StorageStream stream)
    {
        short resourceIndex = default;
        UnitResource resource;
        base.ReadWrite(stream);
        stream.Byte(ref _experience, 7);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref _movePoints), 8);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref _hitPoints), 9);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly)) == Set<StorageModeElement>.Of())
        {
            if (stream.StorageMode().Contains(StorageModeElement.Compare))
            {
                resourceIndex = (short)UnitResource.UnitResourceIndex();
                stream.UInt16(ref Unsafe.As<short, ushort>(ref resourceIndex), 10);
            }
            else
            {
                if (stream.StorageMode().Contains(StorageModeElement.Read))
                {
                    stream.UInt16(ref Unsafe.As<short, ushort>(ref resourceIndex), 10);
                    resource = GamePack.Current.UnitResources.GetUnitResource(resourceIndex);
                    UnitResource = resource;
                    if (resource == null)
                    {
                        throw new KeyNotFoundException("unit resource missing (" + IntToStr(resourceIndex) + ")");
                    }
                }
                else
                {
                    resource = UnitResource;
                    if (resource != null)
                    {
                        resourceIndex = (short)resource.UnitResourceIndex();
                    }
                    else
                    {
                        resourceIndex = -1;
                    }

                    stream.UInt16(ref Unsafe.As<short, ushort>(ref resourceIndex), 10);
                }
            }
        }
    }

    // ReadWrite stores the resource index as a signed 16-bit value through the unsigned stream
    // call, and a unit with no resource stores -1. The load looks the resource up in the game set
    // and refuses a missing one, as ReadWrite does.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _experience = persister.Byte("Experience", value: _experience);
        _movePoints = PersistSupport.UnsignedByte(persister, name: "MovePoints", value: _movePoints);
        _hitPoints = PersistSupport.UnsignedByte(persister, name: "HitPoints", value: _hitPoints);
        if (persister.Loading)
        {
            short resourceIndex = unchecked((short)persister.UInt16("UnitResourceIndex", value: 0));
            var resource = GamePack.Current.UnitResources.GetUnitResource(resourceIndex);
            UnitResource = resource;
            if (resource == null)
            {
                throw new KeyNotFoundException("unit resource missing (" + IntToStr(resourceIndex) + ")");
            }
        }
        else
        {
            short resourceIndex = UnitResource != null ? (short)UnitResource.UnitResourceIndex() : (short)-1;
            persister.UInt16("UnitResourceIndex", value: unchecked((ushort)resourceIndex));
        }
    }

    // Unit.PAS:2067
    public override void Changed()
    {
        base.Changed();
        AbilityAttack = this.GetAbilityAttackAll();
        _abilityDefense = this.GetAbilityDefenseAll();
        _abilityResistance = this.GetAbilityResistanceAll();
        _abilityDamage = this.GetAbilityDamageAll();
    }

    // Unit.PAS:2079
    public override sbyte GetUnitLevel() => UnitResource.Level;

    // Unit.PAS:2118
    public override sbyte GetRace() => UnitResource.Race;

    // Unit.PAS:2025
    public override sbyte GetInherentAttack()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.RankAttack[(int)rank] + UnitResource.Attack);
    }

    // Unit.PAS:2033
    public override sbyte GetInherentDefense()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.RankDefense[(int)rank] + UnitResource.Defense);
    }

    // Unit.PAS:2041
    public override sbyte GetInherentDamage()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.RankDamage[(int)rank] + UnitResource.Damage);
    }

    // Unit.PAS:2049
    public override sbyte GetInherentResistance() => UnitResource.Resistance;

    // Unit.PAS:2027
    public override sbyte GetAttack()
    {
        var rank = GetRank();
        sbyte result = (sbyte)(RankTables.RankAttack[(int)rank] + (UnitResource.Attack + AbilityAttack));
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

    // Unit.PAS:2035
    public override sbyte GetDefense()
    {
        var rank = GetRank();
        var morale = GetMorale();
        sbyte result = (sbyte)(RankTables.RankDefense[(int)rank] + (UnitResource.Defense + _abilityDefense) + Hero.MoraleDefense[(int)morale]);
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

    // Unit.PAS:2043
    public override sbyte GetDamage()
    {
        var rank = GetRank();
        sbyte result = (sbyte)(RankTables.RankDamage[(int)rank] + (UnitResource.Damage + _abilityDamage));
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

    // Unit.PAS:2051
    public override sbyte GetResistance()
    {
        var morale = GetMorale();
        var rank = GetRank();
        sbyte result = (sbyte)(Hero.MoraleResistance[(int)morale] + (UnitResource.Resistance + _abilityResistance) + RankTables.RankResistance[(int)rank]);
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

    // Unit.PAS:2075
    public override sbyte GetHits()
    {
        var rank = GetRank();
        return (sbyte)(RankTables.RankHits[(int)rank] + UnitResource.Hits);
    }

    // Unit.PAS:2077
    public override sbyte GetMoves() => UnitResource.Moves;

    // Unit.PAS:1930
    public override sbyte GetMovePoints() => _movePoints;

    // Unit.PAS:1937
    public override void SetMovePoints(sbyte value)
    {
        if (value > GetMoves())
        {
            value = GetMoves();
        }

        if (value < 0)
        {
            value = 0;
        }

        _movePoints = value;
    }

    // Unit.PAS:1943
    public override sbyte GetHitPoints() => _hitPoints;

    // Unit.PAS:1945
    public override void SetHitPoints(sbyte value)
    {
        if (value > GetHits())
        {
            value = GetHits();
        }

        if (value < 0)
        {
            value = 0;
        }

        _hitPoints = value;
        UpdateDefensiveStrength();
    }

    // Unit.PAS:1972
    public override LibraryImage GetFace()
    {
        LibraryImage result;
        int faceIndex;
        if (!GetAbilityEnabled(126))
        {
            return GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(UnitResource.UnitGraphicsResourceIndex).GetFaceImage();
        }

        faceIndex = ((CosmeticSurgeryEnchantment)(object)((UnitEnchantmentAbilityData)this.GetAbilityData(126)).Enchantment).FaceIndex;
        if (GamePack.Current.FaceResources.GetFaceResource(faceIndex) != null)
        {
            result = GamePack.Current.FaceResources.GetFaceResource(faceIndex).GetCellImage();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Unit.PAS:1917
    public override string GetName() => UnitResource.GetUnitName();

    // Unit.PAS:1932
    public override Alignment GetAlignment()
    {
        if (GetAbilityEnabled(107))
        {
            return Alignment.PureEvil;
        }

        return UnitResource.Alignment;
    }

    // Unit.PAS:2081
    public override LibraryImage GetPreviewImage() => UnitResource.GetPreviewImage();

    // Unit.PAS:2083
    public override byte GetTransportCapacity()
    {
        byte result = (byte)GetAbilityLevel(50);
        if (result < UnitResource.GetTransportCapacity())
        {
            result = UnitResource.GetTransportCapacity();
        }

        return result;
    }

    // Unit.PAS:2088
    public override bool GetTransporter() => UnitResource.GetTransportCapacity() != 0;

    // Unit.PAS:1958
    public override UnitGender GetGender() => UnitResource.Gender;

    // Unit.PAS:1954
    public override BloodType GetBloodType() => UnitResource.BloodType;

    // Unit.PAS:1956
    public override UnitType GetUnitType() => UnitResource.UnitType;

    // Unit.PAS:2108
    public override int GetUnitGraphicsResourceIndex() => UnitResource.UnitGraphicsResourceIndex;

    // Unit.PAS:2120
    public override void NewDay()
    {
        Rank rank;
        base.NewDay();
        if (StrategicMap.Current.Day == 1)
        {
            SetAbilityCount(0);
            for (int rankOrdinal = (int)Unit.Rank.None, last = (int)GetRank(); rankOrdinal <= last; rankOrdinal++)
            {
                rank = (Rank)rankOrdinal;
                UpdateDefaultAbilities(UnitResource.Abilities[(int)rank]);
            }
        }
    }

    // Unit.PAS:1986
    public override int GetExperience() => _experience;

    // Unit.PAS:1988
    public override void SetExperience(int value)
    {
        Rank oldRank;
        Rank newRank;
        Rank rank;
        oldRank = GetRank();
        if (value > 255)
        {
            value = 255;
        }

        _experience = (byte)value;
        newRank = GetRank();
        if (oldRank != newRank)
        {
            for (int rankOrdinal = (int)(Rank)((int)oldRank + 1); rankOrdinal <= (int)newRank; rankOrdinal++)
            {
                rank = (Rank)rankOrdinal;
                UpdateDefaultAbilities(UnitResource.Abilities[(int)rank]);
            }
        }

        Changed();
    }

    // Unit.PAS:2002
    public override int GetNextLevelExperience()
    {
        int result;
        int level;
        level = UnitResource.Level;
        if (_experience >= level * 2)
        {
            if (_experience >= level * 6)
            {
                result = 0;
            }
            else
            {
                return level * 6;
            }
        }
        else
        {
            result = level * 2;
        }

        return result;
    }

    // Unit.PAS:2000
    public override StringCollection GetDescription() => UnitResource.Description;

    // Unit.PAS:1928
    public override int GetObtainValue() => UnitResource.ObtainValue;

    // Unit.PAS:1952
    public override UnitSize GetUnitSize() => UnitResource.UnitSize;

    // Unit.PAS:2110
    public override void Activate()
    {
        if (UnitResource != null)
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
    public override int Show(int X, int Y, in LockedSurface lockedSurface, in UnitShowInfo info)
    {
        int result = base.Show(X, Y, lockedSurface, info);
        if (info.Overlays.Contains(UnitOverlayModesElement.Rank))
        {
            switch (GetRank())
            {
                case Rank.Silver:
                    GamePack.Current.CombatImages[5].Show(X + 33, Y + 12, lockedSurface);
                    break;
                case Rank.Gold:
                    GamePack.Current.CombatImages[6].Show(X + 33, Y + 12, lockedSurface);
                    break;
            }
        }

        return result;
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
        Rank result;
        if (_experience >= UnitResource.Level * 2)
        {
            if (_experience >= UnitResource.Level * 6)
            {
                result = Rank.Gold;
            }
            else
            {
                return Rank.Silver;
            }
        }
        else
        {
            result = Rank.None;
        }

        return result;
    }

    // Unit.PAS:2016
    public void SetRank(Rank value)
    {
        if (value != GetRank())
        {
            switch (value)
            {
                case Rank.None:
                    _experience = 0;
                    break;
                case Rank.Silver:
                    SetExperience(UnitResource.Level * 2);
                    break;
                case Rank.Gold:
                    SetExperience(UnitResource.Level * 6);
                    break;
            }
        }
    }

    // Unit.PAS:2090
    public void SetUnitResource(UnitResource resource)
    {
        Rank rank;
        if (resource != UnitResource)
        {
            base.Clear();
            UnitResource = resource;
            if (UnitResource != null)
            {
                UpdateMoraleValue();
                for (int rankOrdinal = (int)Unit.Rank.None, last = (int)GetRank(); rankOrdinal <= last; rankOrdinal++)
                {
                    rank = (Rank)rankOrdinal;
                    UpdateDefaultAbilities(UnitResource.Abilities[(int)rank]);
                }

                _hitPoints = GetHits();
                _movePoints = GetMoves();
            }
        }
    }

    public enum Rank : byte
    {
        None = 0, Silver = 1, Gold = 2
    }

}
