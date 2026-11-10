// Translated from AoWE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

public partial class UnitSelectionControl : StringList
{
    // Ability.PAS:763
    public UnitSelectionControl()
    {
        FRacesEnabled = new BitList();
        FRacesEnabled.Default = true;
        FTypesEnabled = new BitList();
        FTypesEnabled.Default = true;
        FRaceUnits = new UnitCategoryList();
        FTypeUnits = new UnitCategoryList();
        FAllUnits = new UnitIndexList();
        Reset();
    }

    public byte FMinimumLevel;
    public byte FMaximumLevel;
    public Set<Alignment> FAlignments;
    public BitList FRacesEnabled;
    public BitList FTypesEnabled;
    public UnitCategoryList FRaceUnits;
    public UnitCategoryList FTypeUnits;
    public UnitIndexList FAllUnits;
    public int FSelectedUnitIndex;
    public bool FShowEmptyCategories;
    public bool FShowRaceCategories;
    public bool FShowTypeCategories;
    public bool FShowAllCategory;
    public Hero FSelectedHero;

    // Ability.PAS:776
    public override void Dispose()
    {
        Clear();
        FRacesEnabled?.Dispose();
        FTypesEnabled?.Dispose();
        FRaceUnits?.Dispose();
        FTypeUnits?.Dispose();
        FAllUnits?.Dispose();
        base.Dispose();
    }

    // Ability.PAS:787
    public void SetSelectedUnitIndex(int Value)
    {
        UnitResource Resource;
        if (AoWHSSet.Current != null)
        {
            Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(Value);
            if (Resource != null)
            {
                FSelectedUnitIndex = Value;
            }
            else
            {
                FSelectedUnitIndex = -1;
            }

            FSelectedHero = null;
        }
    }

    // Ability.PAS:801
    public void SetSelectedHero(Hero Value)
    {
        FSelectedHero = Value;
        FSelectedUnitIndex = -1;
    }

    // Ability.PAS:807
    public UnitIndexList GetUnitCategoryList(sbyte Category)
    {
        UnitIndexList Result;
        if (Category >= 0)
        {
            Result = FRaceUnits.GetCategory(Category);
        }
        else
        {
            if (Category == -16)
            {
                Result = FAllUnits;
            }
            else
            {
                Result = FTypeUnits.GetCategory(Math.Abs(Category) - 1);
            }
        }

        return Result;
    }

    // Ability.PAS:814
    public bool GetUnitCategoryEnabled(sbyte Category)
    {
        bool Result;
        if (Category >= 0)
        {
            Result = FRacesEnabled[Category];
        }
        else
        {
            Result = FTypesEnabled[Math.Abs(Category) - 1];
        }

        return Result;
    }

    // Ability.PAS:820
    public void SetUnitCategoryEnabled(sbyte Category, bool Value)
    {
        if (Category >= 0)
        {
            FRacesEnabled[Category] = Value;
        }
        else
        {
            FTypesEnabled[Math.Abs(Category) - 1] = Value;
        }
    }

    // Ability.PAS:826
    public void Reset()
    {
        FAlignments = alsAll;
        FMinimumLevel = 0;
        FMaximumLevel = 255;
        FRacesEnabled.Clear();
        FTypesEnabled.Clear();
        FAllUnits.Clear();
        FSelectedUnitIndex = -1;
        FSelectedHero = null;
        FShowRaceCategories = true;
        FShowAllCategory = false;
        FShowTypeCategories = true;
        if (AoWHSMap.Current != null)
        {
            for (int I = 0, last = AoWHSMap.Current.FRaces.GetCount() - 1; I <= last; I++)
            {
                if (AoWHSMap.Current.FRaces.GetRace(I) != null)
                {
                    var race = AoWHSMap.Current.FRaces.GetRace(I);
                    FRacesEnabled[I] = race.FEnabled;
                }
            }
        }
    }

    // Ability.PAS:847
    public void DisableAllCategories()
    {
        for (int I = 0, last = AoWHSSet.Current.FRaceResources.GetCount() - 1; I <= last; I++)
        {
            FRacesEnabled[I] = false;
        }

        for (int I = 0, last = FTypesEnabled.Count - 1; I <= last; I++)
        {
            FTypesEnabled[I] = false;
        }
    }

    // Ability.PAS:855
    public void UpdateSelection()
    {
        int Index;
        UnitResourceList Resources;
        UnitResource Resource;
        UnitType UnitType_;
        FRaceUnits.Clear();
        FTypeUnits.Clear();
        FAllUnits.Clear();
        Resources = AoWHSSet.Current.FUnitResources;
        // { Original intersects FAlignments with itself, ignoring the unit alignment. }
        if (FShowAllCategory)
        {
            for (int I = 0, last = Resources.GetCount() - 1; I <= last; I++)
            {
                if (Resources.GetUnitResource(I) != null)
                {
                    Resource = Resources.GetUnitResource(I);
                    if (Resource.FLevel >= FMinimumLevel && Resource.FLevel <= FMaximumLevel && (FAlignments & FAlignments) != Set<Alignment>.Of())
                    {
                        FAllUnits.Add(I);
                    }
                }
            }
        }

        if (FShowTypeCategories)
        {
            for (int I = 0, last = Resources.GetCount() - 1; I <= last; I++)
            {
                if (Resources.GetUnitResource(I) != null)
                {
                    Resource = Resources.GetUnitResource(I);
                    if (Resource.FLevel >= FMinimumLevel && Resource.FLevel <= FMaximumLevel && (FAlignments & FAlignments) != Set<Alignment>.Of())
                    {
                        FTypeUnits.AddUnit((int)Resource.FUnitType, I);
                    }
                }
            }
        }

        if (FShowRaceCategories)
        {
            // { Original bounds this race loop by the unit-resource count. }
            for (int I = 0, last = Resources.GetCount() - 1; I <= last; I++)
            {
                if (FRacesEnabled[I] && AoWHSSet.Current.FRaceResources.GetRaceResource(I) != null)
                {
                    for (int J = 0, lastJ = AoWHSSet.Current.FRaceResources.GetRaceResource(I).FUnitIndexes.Count - 1; J <= lastJ; J++)
                    {
                        Index = AoWHSSet.Current.FRaceResources.GetRaceResource(I).FUnitIndexes[J];
                        if (Index != -1)
                        {
                            Resource = Resources.GetUnitResource(Index);
                            if (Resource.FLevel >= FMinimumLevel && Resource.FLevel <= FMaximumLevel && (FAlignments & FAlignments) != Set<Alignment>.Of())
                            {
                                FRaceUnits.AddUnit(I, Index);
                            }
                        }
                    }
                }
            }
        }

        FTypeUnits.SortUnits();
        FRaceUnits.SortUnits();
        FAllUnits.SortUnits();
        Clear();
        if (FShowAllCategory && (FAllUnits.Count != 0 || FShowEmptyCategories))
        {
            AddObject("All", IntToObject(-16));
        }

        if (FShowTypeCategories)
        {
            for (int unitTypeOrdinal = (int)(UnitType)0; unitTypeOrdinal <= (int)(UnitType)2; unitTypeOrdinal++)
            {
                UnitType_ = (UnitType)unitTypeOrdinal;
                if (FTypesEnabled[(int)UnitType_] && FTypeUnits.GetCategory((int)UnitType_) != null && (FTypeUnits.GetCategory((int)UnitType_).Count != 0 || FShowEmptyCategories))
                {
                    AddObject(AbilityRules.TranslateRStr(UnitTypePluralRStr[(int)UnitType_]), IntToObject(-((int)UnitType_ + 1)));
                }
            }
        }

        if (FShowRaceCategories)
        {
            for (int I = 0, last = AoWHSSet.Current.FRaceResources.GetCount() - 1; I <= last; I++)
            {
                if (FRacesEnabled[I] && FRaceUnits.GetCategory(I) != null && (FRaceUnits.GetCategory(I).Count != 0 || FShowEmptyCategories))
                {
                    AddObject(AoWHSSet.Current.FRaceResources.GetRaceResource(I).FRaceName, IntToObject(I));
                }
            }
        }
    }

    private static Set<Alignment> alsAll = Set<Alignment>.FromMask(123uL);
    private static FixedArray_string_3 UnitTypePluralRStr = FixedArray_string_3.From(default(ReadOnlySpan<string>));

    // intf_P1_08.inc:32

    // intf_P1_08.inc:32

}
