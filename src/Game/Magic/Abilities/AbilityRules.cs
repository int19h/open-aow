#nullable disable
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Aow.Engine;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public static class AbilityRules
{

    internal static Set<DamageType> AllDamageTypesForWallImmunity { get; } = Set<DamageType>.FromMask(1023UL);

    internal static Set<DamageType> WallDamageTypes { get; } = Set<DamageType>.FromMask(256UL);

    // The original reads the engine global and returns the text unchanged while it is nil. A bare
    // engine (no dictionary) returns it unchanged too, as GameEngine.TranslateText does.
    internal static string TranslateResourceString(GameEngine engine, string S) => engine?.Dictionary is not null ? engine.TranslateResourceString(S) : S;

    internal static int ExpectedDamageCapped(int damage, int attackDefense, int maximum)
    {
        int result;
        if (damage > 0)
        {
            result = ExpectedDamage(damage, attackDefense);
            // { Original updates only the unused Maximum parameter, leaving Result uncapped. }
            if (damage > maximum)
            {
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    internal static int ExpectedDamage(int damage, int attackDefense)
    {
        int result;
        if (damage > 0)
        {
            int threshold = 10 - (attackDefense * 2);
            if (threshold > 18)
            {
                result = damage;
            }
            else
            {
                if (threshold >= 0)
                {
                    result = ((((damage - 1) * (18 - threshold)) + 1) >>> 1) + damage + (18 - threshold);
                    if (threshold != 0)
                    {
                        result += 1;
                    }
                }
                else
                {
                    result = damage + 1 + (18 - threshold) + ((((damage - 1) * (18 - threshold)) + 1) >>> 1);
                    threshold = -threshold;
                    int correction = ((((damage - 1) * threshold * threshold) + threshold + 19) / (threshold + 19)) >>> 1;
                    threshold = correction + threshold + 1;
                    result -= threshold;
                }
            }

            result *= 5;
        }
        else
        {
            return 0;
        }

        return result;
    }

    internal static int ExecuteDamageRole(int damage, int attackDefense)
    {
        if (damage > 0)
        {
            int roll = Random(20);
            if (roll <= 1)
            {
                return 0;
            }
            else
            {
                if (roll >= 18)
                {
                    return damage;
                }
                else
                {
                    attackDefense = 10 - (attackDefense * 2);
                    return roll >= attackDefense && attackDefense < 18
                        ? ((((damage - 1) * (roll - attackDefense)) + ((18 - attackDefense) >>> 1)) / (18 - attackDefense)) + 1
                        : 0;
                }
            }
        }
        else
        {
            return 0;
        }
    }

    internal static int DamageValueToDamageEvaluation(int sourceDamageValue, int targetDamageValue, CombatObject source, CombatObject target)
    {
        int result;
        if (target.GetTargetPriority() != 0)
        {
            int targetPriority = target.GetTargetPriority();
            sbyte hitPoints = target.HitPoints;
            result = target.GetTargetPriority() + Round(targetPriority * ((double)sourceDamageValue / (hitPoints * 100)));
            if (targetDamageValue != 0)
            {
                int sourceTargetPriority = source.GetTargetPriority();
                sbyte sourceHitPoints = source.HitPoints;
                result -= source.GetTargetPriority() + Round(sourceTargetPriority * ((double)targetDamageValue / (sourceHitPoints * 100)));
            }
        }
        else
        {
            return sourceDamageValue + -99_999 - (source.GetTargetPriority() * targetDamageValue);
        }

        return result;
    }

    internal static int StatisticsToMaxDamage(int damage, Set<DamageType> damageTypes, Set<DamageType> immunityTypes, Set<DamageType> protectionTypes)
    {
        damageTypes -= immunityTypes;
        return damageTypes != Set<DamageType>.Of()
            ? damageTypes - protectionTypes == Set<DamageType>.Of() ? (damage + 1) >>> 1 : damage
            : 0;
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi damage calculation signature keeps resistance.")]
    internal static int StatisticsToLimitedDamageValue(int attack, int damage, Set<DamageType> damageTypes, int maximum, int defense, int resistance, Set<DamageType> immunityTypes, Set<DamageType> protectionTypes)
    {
        int limit = maximum;
        damageTypes -= immunityTypes;
        int result;
        if (damageTypes != Set<DamageType>.Of())
        {
            attack -= defense;
            if (damageTypes - protectionTypes == Set<DamageType>.Of())
            {
                damage = (damage + 1) >>> 1;
            }

            result = ExpectedDamageCapped(damage, attackDefense: attack, maximum: limit);
            int limit100 = limit * 100;
            if (result > limit100)
            {
                return limit100;
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    internal static int RoadConstructionCost(sbyte player, MovePath path, int movePoints)
    {
        int result = 0;
        int i = path.Count;
        int points = 0;
        while (i != 0 && points <= movePoints)
        {
            i -= 1;
            points += path[i].MoveCost;
            if (points <= movePoints)
            {
                PathStep item = path[i];
                MapField field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(item.Location);
                if (CanConstructRoad(player, field))
                {
                    result += 10;
                }
            }
        }

        return result;
    }

    internal static string Format(string S, params ReadOnlySpan<FormatArg> arguments)
    {
        try
        {
            return DelphiFormat.Format(S, args: arguments);
        }
        catch
        {
            return S;
        }
    }

    internal static void ShowDamageIcon(GameEngine engine, int x, int y, in LockedSurface surface, int damage, DamageType damageType)
    {
        if (damageType != DamageType.None)
        {
            GamePack.Current.CombatImages[(int)damageType + 10].Show(x + 15, y + 10, lockedSurface: surface);
            engine.CombatFont.Color = Set<DamageType>.Of(DamageType.Cold, DamageType.Lightning, DamageType.Magic, DamageType.Holy).Contains(damageType)
                ? 0
                : 16_777_215;

            engine.CombatFont.SetTextCentered(x + 34, y + 36, text: damage.ToString(CultureInfo.InvariantCulture), lockedSurface: surface);
        }
    }

    internal static Troop.Morale MoraleValueToMorale(int value)
    {
        if (value <= 20)
        {
            return Troop.Morale.Terrible;
        }
        else
        {
            if (value > 80)
            {
                return Troop.Morale.High;
            }
            else
            {
                switch (value)
                {
                    case >= 21 and <= 40:
                        return Troop.Morale.Poor;
                    case >= 41 and <= 60:
                        return Troop.Morale.Ok;
                    case >= 61 and <= 80:
                        return Troop.Morale.Good;
                    default:
                        // { Original leaks the exception without raising it. }
                        _ = new ArgumentException("Invalid MoralValue");
                        break;
                }
            }
        }

        return Troop.Morale.Ok;
    }

    internal static int PlaceRandomUnits(int X, int Y, int L, sbyte player, UnitIndexList units, int level, int value, Set<UnitType> types, GameEngine engine)
    {
        int result = 0;
        if (units.Count != 0)
        {
            int tries = 50;
            while (result < value && tries != 0)
            {
                tries -= 1;
                int index = units[Random(units.Count)];
                if (index != -1 && types.Contains(GamePack.Current.UnitResources.GetUnitResource(index).UnitType) && GamePack.Current.UnitResources.GetUnitResource(index).GetObtainValue() + result > value && GamePack.Current.UnitResources.GetUnitResource(index).Level <= level)
                {
                    int bestIndex = index;
                    int i = 20;
                    int difference = Math.Abs(GamePack.Current.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
                    do
                    {
                        index = units[Random(units.Count)];
                        if (index != -1 && types.Contains(GamePack.Current.UnitResources.GetUnitResource(index).UnitType))
                        {
                            int delta = Math.Abs(GamePack.Current.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
                            if (delta < difference)
                            {
                                difference = delta;
                                bestIndex = index;
                            }
                        }

                        i -= 1;
                    }
                    while (!(i == 0));
                    index = bestIndex;
                }

                if (index != -1 && types.Contains(GamePack.Current.UnitResources.GetUnitResource(index).UnitType) && GamePack.Current.UnitResources.GetUnitResource(index).Level <= level)
                {
                    Unit unit = new(null) { Holder = engine };
                    unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                    if (unit.Place(X, Y, L, player, mode: Set<UnitPlaceModeElement>.Of()) is not null)
                    {
                        result += unit.GetObtainValue();
                        tries = 50;
                    }

                    _ = unit.Release();
                }
            }
        }

        return result;
    }

    internal static Alignment IndependentRelationToAlignment(sbyte relation)
    {
        return relation < 0
            ? relation switch
            {
                -2 => Alignment.Good,
                -4 => Alignment.Evil,
                -3 => Alignment.Neutral,
                _ => Alignment.Neutral,
            }
            : GamePack.Current.RaceResources.GetRaceResource(relation).Alignment;
    }

    internal static DiplomaticRelation DiplomaticRelation(sbyte player, sbyte independentRelation, sbyte otherPlayer, sbyte otherIndependentRelation)
    {
        if (player == 0 || otherPlayer == 0)
        {
            sbyte race = player != 0
                ? StrategicMap.Current.Players.GetPlayers(player).PlayerRace
                : independentRelation >= 0 ? independentRelation : (sbyte)-1;

            sbyte otherRace = otherPlayer != 0
                ? StrategicMap.Current.Players.GetPlayers(otherPlayer).PlayerRace
                : otherIndependentRelation >= 0 ? otherIndependentRelation : (sbyte)-1;

            if (race == -1 || otherRace == -1)
            {
                Alignment alignment = race != -1
                    ? GamePack.Current.RaceResources.GetRaceResource(race).Alignment
                    : IndependentRelationToAlignment(independentRelation);

                Alignment otherAlignment = otherRace != -1
                    ? GamePack.Current.RaceResources.GetRaceResource(otherRace).Alignment
                    : IndependentRelationToAlignment(otherIndependentRelation);

                return AlignmentTables.AlignmentToDiplomaticRelation(alignment, otherAlignment);
            }
            else
            {
                return AlignmentTables.StatusToDiplomaticRelation(StrategicMap.Current.Races.GetRace(race).GetRelation(otherRace));
            }
        }
        else
        {
            return player != otherPlayer
                ? StrategicMap.Current.Players.GetDiplomaticRelation(player, otherPlayer)
                : Session.DiplomaticRelation.Alliance;
        }
    }

    internal static bool ValidAttackRelation(sbyte player, sbyte independentRelation, sbyte targetPlayer, sbyte targetRace)
    {
        if (player == 0)
        {
            if (targetPlayer != 0)
            {
                DiplomaticRelation relation = DiplomaticRelation(player, independentRelation, otherPlayer: targetPlayer, otherIndependentRelation: targetRace);
                return Set<DiplomaticRelation>.Of(Session.DiplomaticRelation.War, Session.DiplomaticRelation.Unknown).Contains(relation);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return StrategicMap.Current.Players.GetPlayers(player).ValidAttackRelation(targetPlayer, independentRelation: targetRace);
        }
    }

    internal static void PolarToCartesian(int degrees, int radius, out int X, out int Y)
    {
        float radians = (float)((double)(degrees * Math.PI) / 180);
        X = Round((double)Math.Cos(radians) * radius);
        Y = Round((double)Math.Sin(radians) * radius);
    }

    internal static string Encode(string text)
    {
        string result = "";
        for (int i = 1; i <= text.Length; i++)
        {
            result += Cp1252.Text((byte)(Cp1252.Byte(text[i - 1]) ^ ((text.Length + i + 5) & 0xFF)));
        }

        return result;
    }

    internal static int GetMovePathBattleFieldStrength(sbyte player, sbyte independentRelation, MovePath movePath, int targetRange, int range, int step, GameEngine engine)
    {
        int result;
        if (movePath is not null && movePath.Count > 0)
        {
            int distance = Round(range * 0.5);
            result = 0;
            int i = movePath.Count - 1;
            UnitUserList units = null;
            if (movePath.Count - targetRange < i)
            {
                i = movePath.Count - targetRange;
            }

            BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, player, relation: independentRelation, requests: Set<BattleFieldInfoRequestsElement>.Of(), damageTypes: s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            info.Requests += Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents, BattleFieldInfoRequestsElement.EffectiveStrength);
            if (step > 0)
            {
                units = new UnitUserList(null) { Holder = engine };
                PathStep item = movePath[^1];
                MapLevel level = StrategicMap.Current.MapContainer.MapLevels[item.Location.MapLevel];
                PathStep movePathItem = movePath[^1];
                PathStep item2 = movePath[^1];
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, item2.Location.X, movePathItem.Location.Y, radius: 0);
                while (walk.Radius <= step)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        MapField field = level.GetField(walk.X, walk.Y);
                        if ((field.FieldFlags & MapField.BattleFieldInfoFlag) != 0)
                        {
                            ArmyHexSprite army = field.GetArmyHexSprite();
                            if (army is not null && army.Army.Player != player)
                            {
                                _ = units.AddUnits(army.Army.Units);
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                info.Units = units.Units;
            }

            while (i >= 0)
            {
                info.Strength = 0;
                PathStep item = movePath[i];
                PathStep movePathItem = movePath[i];
                PathStep item2 = movePath[i];
                StrategicMap.Current.RetrieveBattleFieldInfo(item2.Location.X, movePathItem.Location.Y, item.Location.MapLevel, radius: range, ref info);
                if (info.Strength > result)
                {
                    result = info.Strength;
                }

                int j = i;
                while (true)
                {
                    bool condition = j >= 0;
                    if (condition)
                    {
                        PathStep item3 = movePath[j];
                        PathStep item4 = movePath[i];
                        condition = item3.Location.MapLevel == item4.Location.MapLevel;
                    }

                    if (!(condition && j > i - distance))
                    {
                        break;
                    }

                    j -= 1;
                }

                bool test = j == 0;
                if (!test)
                {
                    PathStep item3 = movePath[i];
                    PathStep item4 = movePath[j];
                    test = item3.Location.MapLevel != item4.Location.MapLevel;
                }

                if (test)
                {
                    if (j - 1 != i)
                    {
                        info.Strength = 0;
                        PathStep item3 = movePath[j + 1];
                        PathStep item4 = movePath[j + 1];
                        PathStep item5 = movePath[j + 1];
                        StrategicMap.Current.RetrieveBattleFieldInfo(item5.Location.X, item4.Location.Y, item3.Location.MapLevel, radius: range, ref info);
                        if (info.Strength > result)
                        {
                            result = info.Strength;
                        }
                    }
                }

                i = j;
            }

            units?.Dispose();

            StrategicMap.Current.AIExecutor.UseProcessTime(50);
        }
        else
        {
            return 0;
        }

        return result;
    }

    internal static EnhancementAbility CreateEnhancementAbility(int id, string name, Set<AbilitySelectionType> selectionTypes, GameEngine engine)
    {
        EnhancementAbility ability = new(null)
        {
            Holder = engine,
            AbilityId = id,
            AbilityName = name,
            SelectionTypes = selectionTypes
        };
        return ability;
    }

    internal static ReadOnlySpan<int> CommandAbilityIds => [29, 28, 148];

    internal static ReadOnlySpan<int> RaceColors => [14_693_424, 6_726_911, 11_520_812, 16_702_087, 6_352_705, 1_441_277, 10_863_076, 16_252_925, 11_759_804, 5_263_593, 4_351_675, 7_960_728, 11_053_224];

    private static readonly Set<DamageType> s_allDamageTypes = Set<DamageType>.FromMask(1023UL);

    internal static ImmutableArray<string> ExplosionSfx { get; } = ["SFX\\ATTACK\\STORM.WAV", "SFX\\ATTACK\\EXPLOD1.WAV"];

    internal static ReadOnlySpan<byte> ArmySelectionMasks => [0, 1, 3, 7, 15, 31, 63, 127, 255];

    internal static EngineClass MultiLevelDataClass { get; } = EngineClass.Of<MultiLevelAbilityData>();

    internal static int LimitedDamage(int attack, int defense, int damage)
    {
        if (defense > 10)
        {
            defense = 10;
        }

        return attack * defense * damage / 2;
    }

    internal static bool EndPlayerTurn(sbyte player, GameEngine engine)
    {
        EndTurnTokenEvent eventValue = new(null)
        {
            Holder = engine,
            Player = player,
            Day = StrategicMap.Current.Day,
            Mode = EndTurnTokenEventMode.EndTurn
        };
        StrategicMap.Current.TokenManager.TokenControls[player].AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    internal static bool CanConstructRoad(sbyte player, MapField field)
    {
        bool result = player == field.GetPlayer() || field.GetPlayer() == -1;
        result = result && Set<byte>.WithBounds(0, 255).WithRange(1, 5).Contains((byte)field.TerrainType);
        result = result && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(4).With(5).Contains((byte)field.OverlayType);
        // Original shared helper
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null;
    }

    public static void RetrieveStormDamageInfo(sbyte player, StormType storm, sbyte x, sbyte y,
                sbyte level, sbyte innerRadius, sbyte outerRadius, out StormDamageInfo info)
    {
        StormDamageQuery query = new()
        {
            Info = new()
            {
                Player = player,
                FriendlyRelations = Set<byte>.WithBounds(0, 7).With(0).With(2),
                EnemyRelations = Set<byte>.WithBounds(0, 7).With(1).With(3)
            }
        };
        MapLevel mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(level);
        MapFieldMessage message = new() { MessageId = 0x1151 };
        MapFieldMessageParameters param = message.Param;
        param[0] = (int)storm;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[2] = query;
        message.Param = param2;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: innerRadius);
        try
        {
            while (walk.Radius <= outerRadius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    MapFieldMessageParameters param3 = message.Param;
                    param3[1] = walk.Radius;
                    message.Param = param3;
                    mapLevel.GetField(walk.X, walk.Y).SendMapFieldMessage(ref message);
                }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        finally
        {
            info = query.Info;
        }
    }

    internal static Set<DamageType> AllDamageTypes { get; } = Set<DamageType>.FromMask(1023UL);

    internal static void TriggerStormDamage(StormType storm, sbyte x, sbyte y, sbyte mapLevel, sbyte innerRadius, sbyte outerRadius, sbyte player)
    {
        MapFieldMessage message = default;
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[mapLevel];
        message.MessageId = 4432;
        MapFieldMessageParameters param = message.Param;
        param[0] = (int)storm;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[2] = player;
        message.Param = param2;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: innerRadius);
        while (walk.Radius <= outerRadius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapFieldMessageParameters param3 = message.Param;
                param3[1] = walk.Radius;
                message.Param = param3;
                level.Field[walk.X, walk.Y].SendMapFieldMessage(ref message);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
