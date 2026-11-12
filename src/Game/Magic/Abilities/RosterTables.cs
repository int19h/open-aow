
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class RosterTables
{

    internal static int GetUnitIndexAIPriority(int unitIndex, sbyte player, sbyte relation)
    {
        int result;
        UnitResource resource;
        int value;
        Unit unit;
        HypotheticalUnit defaultUnit;
        resource = AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex);
        if (resource != null && resource.GetTransportCapacity() == 0)
        {
            if (player != 0)
            {
                if (resource.Race != -1)
                {
                    value = AoWHSMap.Current.Races.GetRace(resource.Race).GetPlayerRelationValue(player);
                }
                else
                {
                    var players = AoWHSMap.Current.Players.GetPlayers(player);
                    var alignment = AoWHSSet.Current.RaceResources.GetRaceResource(players.PlayerRace).GetAlignment();
                    value = AlignmentTables.AlignmentStatusValues[(int)resource.Alignment][(int)alignment];
                }
            }
            else
            {
                var iRtoAlignment = AbilityRules.IndependentRelationToAlignment(relation);
                value = AlignmentTables.AlignmentStatusValues[(int)resource.Alignment][(int)iRtoAlignment];
            }

            if (value >= 50)
            {
                unit = new Unit(null);
                unit.SetUnitResource(resource);
                defaultUnit = new HypotheticalUnit(null);
                defaultUnit.MakeDefaultLevelUnit(1);
                if (unit.GetCombatValue(defaultUnit, 0) > 0)
                {
                    result = unit.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall)) / 100 + (resource.Level * 100 + resource.Moves * 10 + value / 2);
                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(57) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(58))
                    {
                        result += 100;
                    }

                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(13))
                    {
                        result += 150;
                    }

                    if (AoWHSMap.Current != null && AoWHSMap.Current.Day > 20 && (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(20) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(19) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(18) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(121) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(88) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(89)))
                    {
                        result += 100;
                    }

                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(1) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(36) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(66) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(24) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(44) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(117) && resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(21))
                    {
                        result += 100;
                    }

                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(65))
                    {
                        result = result / 2;
                    }
                }
                else
                {
                    result = -1;
                }

                defaultUnit?.Dispose();
                unit?.Dispose();
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    internal static Set<HeroType> AllHeroTypes { get; } = Set<HeroType>.Of(HeroType.Hero, HeroType.Leader, HeroType.HeroLeader, HeroType.Merchant);

}
