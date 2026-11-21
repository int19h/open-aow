#nullable disable warnings

using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

internal static class RosterTables
{

    internal static int GetUnitIndexAIPriority(int unitIndex, sbyte player, sbyte relation, GameEngine? engine)
    {
        UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(unitIndex);
        int result;
        if (resource is not null && resource.GetTransportCapacity() == 0)
        {
            int value;
            if (player != 0)
            {
                if (resource.Race != -1)
                {
                    value = StrategicMap.Current.Races.GetRace(resource.Race).GetPlayerRelationValue(player);
                }
                else
                {
                    Player? players = StrategicMap.Current.Players.GetPlayers(player);
                    Alignment alignment = GamePack.Current.RaceResources.GetRaceResource(players.PlayerRace).Alignment;
                    value = AlignmentTables.AlignmentStatusValues[(int)resource.Alignment][(int)alignment];
                }
            }
            else
            {
                Alignment iRtoAlignment = AbilityRules.IndependentRelationToAlignment(relation);
                value = AlignmentTables.AlignmentStatusValues[(int)resource.Alignment][(int)iRtoAlignment];
            }

            if (value >= 50)
            {
                Unit unit = new(null) { Holder = engine };
                unit.SetUnitResource(resource);
                HypotheticalUnit defaultUnit = new(null) { Holder = engine };
                defaultUnit.MakeDefaultLevelUnit(1);
                if (unit.GetCombatValue(defaultUnit, mode: 0) > 0)
                {
                    result = (unit.GetStrengthMax(AbilityRules.AllDamageTypes & ~DamageTypes.Wall) / 100) + (resource.Level * 100) + (resource.Moves * 10) + (value / 2);
                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(57) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(58))
                    {
                        result += 100;
                    }

                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(13))
                    {
                        result += 150;
                    }

                    if (StrategicMap.Current is not null && StrategicMap.Current.Day > 20 && (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(20) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(19) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(18) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(121) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(88) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(89)))
                    {
                        result += 100;
                    }

                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(1) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(36) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(66) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(24) || resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(44) || (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(117) && resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(21)))
                    {
                        result += 100;
                    }

                    if (resource.Abilities[(int)Unit.Rank.None].GetAbilitySetAt(65))
                    {
                        result /= 2;
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
                return -1;
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    internal static HeroTypes AllHeroTypes { get; } = HeroTypes.Hero | HeroTypes.Leader | HeroTypes.HeroLeader | HeroTypes.Merchant;

}
