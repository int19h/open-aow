
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class RosterTables
{
    internal static UnitEnchantmentAbility_Expand__TOriginalAssertText TUnitEnchantmentAbility_Expand__OriginalAssertFile = new UnitEnchantmentAbility_Expand__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 65, 111, 87, 65, 98, 46, 80, 65, 83, 0])
    };

    internal static UnitEnchantmentAbility_Expand__TOriginalAssertText TUnitEnchantmentAbility_Expand__OriginalAssertMessage = new UnitEnchantmentAbility_Expand__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = FixedArray_byte_128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static int GetUnitIndexAIPriority(int UnitIndex, sbyte Player, sbyte Relation)
    {
        int Result;
        UnitResource Resource;
        int Value;
        Unit Unit_;
        AdjustableUnit DefaultUnit;
        Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex);
        if (Resource != null && Resource.GetTransportCapacity() == 0)
        {
            if (Player != 0)
            {
                if (Resource.FRace != -1)
                {
                    Value = AoWHSMap.Current.FRaces.GetRace(Resource.FRace).GetPlayerRelationValue(Player);
                }
                else
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                    var alignment = AoWHSSet.Current.FRaceResources.GetRaceResource(players.FPlayerRace).GetAlignment();
                    Value = AlignmentTables.dAlignmentToStatusValue[(int)Resource.FAlignment][(int)alignment];
                }
            }
            else
            {
                var iRtoAlignment = AbilityRules.IRtoAlignment(Relation);
                Value = AlignmentTables.dAlignmentToStatusValue[(int)Resource.FAlignment][(int)iRtoAlignment];
            }

            if (Value >= 50)
            {
                Unit_ = new Unit(null);
                Unit_.SetUnitResource(Resource);
                DefaultUnit = new AdjustableUnit(null);
                DefaultUnit.MakeDefaultLevelUnit(1);
                if (Unit_.GetCombatValue(DefaultUnit, 0) > 0)
                {
                    Result = Unit_.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall)) / 100 + (Resource.FLevel * 100 + Resource.FMoves * 10 + Value / 2);
                    if (Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(57) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(58))
                    {
                        Result += 100;
                    }

                    if (Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(13))
                    {
                        Result += 150;
                    }

                    if (AoWHSMap.Current != null && AoWHSMap.Current.FDay > 20 && (Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(20) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(19) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(18) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(121) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(88) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(89)))
                    {
                        Result += 100;
                    }

                    if (Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(1) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(36) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(66) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(24) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(44) || Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(117) && Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(21))
                    {
                        Result += 100;
                    }

                    if (Resource.FAbilities[(int)Unit.Rank.None].GetAbSet(65))
                    {
                        Result = Result / 2;
                    }
                }
                else
                {
                    Result = -1;
                }

                DefaultUnit?.Dispose();
                Unit_?.Dispose();
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    internal static EClass P4UnitSelectionClass = EClass.Of<UnitUserList>();

    internal static Set<HeroType> AllHeroTypes = Set<HeroType>.Of(HeroType.Hero, HeroType.Leader, HeroType.HeroLeader, HeroType.Merchant);

}
