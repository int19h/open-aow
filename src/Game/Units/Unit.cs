#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

public class Unit(CustomNode? owner) : Troop(owner)
{
    public new const int ClassId = 131_603;
    private int _experience;
    private int _movePoints;
    private int _hitPoints;

    public UnitResource? UnitResource { get; set; }
    public int AbilityAttack { get; set; }

    private int _abilityDefense;
    private int _abilityResistance;
    private int _abilityDamage;

    // Unit.PAS:2132
    // The original ReadWrite stores the resource index as a signed 16-bit value through the unsigned stream
    // call, and a unit with no resource stores -1. The load looks the resource up in the game set
    // and refuses a missing one, as the original ReadWrite does.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _experience = persister.Byte("Experience", value: _experience.TruncateToByte());
        _movePoints = persister.UnsignedByte(name: "MovePoints", value: _movePoints.TruncateToSByte());
        _hitPoints = persister.UnsignedByte(name: "HitPoints", value: _hitPoints.TruncateToSByte());
        if (persister.Loading)
        {
            short resourceIndex = unchecked((short)persister.UInt16("UnitResourceIndex", value: 0));
            UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(resourceIndex);
            UnitResource = resource;
            if (resource is null)
            {
                throw new KeyNotFoundException($"unit resource missing ({((int)resourceIndex).ToString(CultureInfo.InvariantCulture)})");
            }
        }
        else
        {
            short resourceIndex = UnitResource is not null ? (short)UnitResource.UnitResourceIndex() : (short)-1;
            _ = persister.UInt16("UnitResourceIndex", value: unchecked((ushort)resourceIndex));
        }
    }

    // Unit.PAS:2067
    public override void Changed()
    {
        base.Changed();
        AbilityAttack = GetAbilityAttackAll();
        _abilityDefense = GetAbilityDefenseAll();
        _abilityResistance = GetAbilityResistanceAll();
        _abilityDamage = GetAbilityDamageAll();
    }

    // Unit.PAS:2079
    public override int UnitLevel => UnitResource.Level;

    // Unit.PAS:2118
    public override sbyte GetRace() => UnitResource.Race;

    // Unit.PAS:2025
    public override int GetInherentAttack()
    {
        Rank rank = GetRank();
        return (RankTables.RankAttack[(int)rank] + UnitResource.Attack).TruncateToSByte();
    }

    // Unit.PAS:2033
    public override int GetInherentDefense()
    {
        Rank rank = GetRank();
        return (RankTables.RankDefense[(int)rank] + UnitResource.Defense).TruncateToSByte();
    }

    // Unit.PAS:2041
    public override int GetInherentDamage()
    {
        Rank rank = GetRank();
        return (RankTables.RankDamage[(int)rank] + UnitResource.Damage).TruncateToSByte();
    }

    // Unit.PAS:2049
    public override int InherentResistance => UnitResource.Resistance;

    // Unit.PAS:2027
    public override int GetAttack()
    {
        Rank rank = GetRank();
        int result = (RankTables.RankAttack[(int)rank] + (UnitResource.Attack + AbilityAttack)).TruncateToSByte();
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? 10 : result;
    }

    // Unit.PAS:2035
    public override int GetDefense()
    {
        Rank rank = GetRank();
        Morale morale = GetMorale();
        int result = (RankTables.RankDefense[(int)rank] + (UnitResource.Defense + _abilityDefense) + Hero.MoraleDefense[(int)morale]).TruncateToSByte();
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? 10 : result;
    }

    // Unit.PAS:2043
    public override int GetDamage()
    {
        Rank rank = GetRank();
        int result = (RankTables.RankDamage[(int)rank] + (UnitResource.Damage + _abilityDamage)).TruncateToSByte();
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? 10 : result;
    }

    // Unit.PAS:2051
    public override int GetResistance()
    {
        Morale morale = GetMorale();
        Rank rank = GetRank();
        int result = (Hero.MoraleResistance[(int)morale] + (UnitResource.Resistance + _abilityResistance) + RankTables.RankResistance[(int)rank]).TruncateToSByte();
        if (result < 1)
        {
            result = 1;
        }

        return result > 10 ? 10 : result;
    }

    // Unit.PAS:2075
    public override int GetHits()
    {
        Rank rank = GetRank();
        return (RankTables.RankHits[(int)rank] + UnitResource.Hits).TruncateToSByte();
    }

    // Unit.PAS:2077
    public override int Moves => UnitResource.Moves;

    // Unit.PAS:1930
    public override int GetMovePoints() => _movePoints;

    // Unit.PAS:1937
    public override void SetMovePoints(int value)
    {
        if (value > Moves)
        {
            value = Moves;
        }

        if (value < 0)
        {
            value = 0;
        }

        _movePoints = value;
    }

    // Unit.PAS:1943
    public override int GetHitPoints() => _hitPoints;

    // Unit.PAS:1945
    public override void SetHitPoints(sbyte value)
    {
        if (value > GetHits())
        {
            value = (sbyte)GetHits();
        }

        if (value < 0)
        {
            value = 0;
        }

        _hitPoints = value;
        UpdateDefensiveStrength();
    }

    // Unit.PAS:1972
    public override LibraryImage? GetFace()
    {
        if (!GetAbilityEnabled(126))
        {
            return GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(UnitResource.UnitGraphicsResourceIndex).GetFaceImage();
        }

        int faceIndex = ((CosmeticSurgeryEnchantment)(object?)GetAbilityData<UnitEnchantmentAbilityData>(126).Enchantment).FaceIndex;
        return GamePack.Current.FaceResources.GetFaceResource(faceIndex)?.GetCellImage();
    }

    // Unit.PAS:1917
    public override string GetName() => UnitResource.GetUnitName();

    // Unit.PAS:1932
    public override Alignment GetAlignment() => GetAbilityEnabled(107) ? Alignment.PureEvil : UnitResource.Alignment;

    // Unit.PAS:2081
    public override LibraryImage? GetPreviewImage() => UnitResource.GetPreviewImage();

    // Unit.PAS:2083
    public override int GetTransportCapacity()
    {
        int result = GetAbilityLevel(50).TruncateToByte();
        return result < UnitResource.GetTransportCapacity() ? UnitResource.GetTransportCapacity() : result;
    }

    // Unit.PAS:2088
    public override bool GetTransporter() => UnitResource.GetTransportCapacity() != 0;

    // Unit.PAS:1958
    public override UnitGender Gender => UnitResource.Gender;

    // Unit.PAS:1954
    public override BloodType BloodType => UnitResource.BloodType;

    // Unit.PAS:1956
    public override UnitType GetUnitType() => UnitResource.UnitType;

    // Unit.PAS:2108
    public override int UnitGraphicsResourceIndex => UnitResource.UnitGraphicsResourceIndex;

    // Unit.PAS:2120
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1)
        {
            SetAbilityCount(0);
            for (int rankOrdinal = (int)Rank.None, last = (int)GetRank(); rankOrdinal <= last; rankOrdinal++)
            {
                Rank rank = (Rank)rankOrdinal;
                UpdateDefaultAbilities(UnitResource.Abilities[(int)rank]);
            }
        }
    }

    // Unit.PAS:1986
    public override int GetExperience() => _experience;

    // Unit.PAS:1988
    public override void SetExperience(int value)
    {
        Rank oldRank = GetRank();
        if (value > 255)
        {
            value = 255;
        }

        _experience = value.TruncateToByte();
        Rank newRank = GetRank();
        if (oldRank != newRank)
        {
            for (int rankOrdinal = (int)(Rank)((int)oldRank + 1); rankOrdinal <= (int)newRank; rankOrdinal++)
            {
                Rank rank = (Rank)rankOrdinal;
                UpdateDefaultAbilities(UnitResource.Abilities[(int)rank]);
            }
        }

        Changed();
    }

    // Unit.PAS:2002
    public override int GetNextLevelExperience()
    {
        int level = UnitResource.Level;
        return _experience >= level * 2 ? _experience >= level * 6 ? 0 : level * 6 : level * 2;
    }

    // Unit.PAS:2000
    public override DelphiStringCollection Description => UnitResource.Description;

    // Unit.PAS:1928
    public override int GetObtainValue() => UnitResource.ObtainValue;

    // Unit.PAS:1952
    public override UnitSize UnitSize => UnitResource.UnitSize;

    // Unit.PAS:2110
    public override void Activate()
    {
        if (UnitResource is not null)
        {
            base.Activate();
            return;
        }

        _ = AddRef();
        _ = AddRef();
        _ = AddRef();
        Dispose();
    }

    // Unit.PAS:1919
    public override int Show(int X, int Y, in LockedSurface lockedSurface, in UnitShowInfo info)
    {
        int result = base.Show(X, Y, lockedSurface, info);
        if (info.Overlays.Contains(UnitOverlayModes.Rank))
        {
            switch (GetRank())
            {
                case Rank.Silver:
                    GamePack.Current.CombatImages[5].Show(X + 33, Y + 12, lockedSurface);
                    break;
                case Rank.Gold:
                    GamePack.Current.CombatImages[6].Show(X + 33, Y + 12, lockedSurface);
                    break;
                case Rank.None:
                default:
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
    public Rank GetRank() => _experience >= UnitResource.Level * 2 ? _experience >= UnitResource.Level * 6 ? Rank.Gold : Rank.Silver : Rank.None;

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
                default:
                    break;
            }
        }
    }

    // Unit.PAS:2090
    public void SetUnitResource(UnitResource? resource)
    {
        if (resource != UnitResource)
        {
            base.Clear();
            UnitResource = resource;
            if (UnitResource is not null)
            {
                _ = (sbyte)UpdateMoraleValue();
                for (int rankOrdinal = (int)Rank.None, last = (int)GetRank(); rankOrdinal <= last; rankOrdinal++)
                {
                    Rank rank = (Rank)rankOrdinal;
                    UpdateDefaultAbilities(UnitResource.Abilities[(int)rank]);
                }

                _hitPoints = GetHits();
                _movePoints = Moves;
            }
        }
    }

    public enum Rank : byte
    {
        None = 0, Silver = 1, Gold = 2
    }
}
