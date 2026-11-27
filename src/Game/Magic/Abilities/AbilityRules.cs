#nullable disable warnings
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
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public static class AbilityRules
{

    internal static DamageTypes AllDamageTypesForWallImmunity { get; } = DamageTypes.Fire | DamageTypes.Cold | DamageTypes.Lightning | DamageTypes.Magic | DamageTypes.Poison | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Physical | DamageTypes.Wall | DamageTypes.NonSpecific;

    internal static DamageTypes WallDamageTypes { get; } = DamageTypes.Wall;

    // The original reads the engine global and returns the text unchanged while it is nil. A bare
    // engine (no dictionary) returns it unchanged too, as GameEngine.TranslateText does.
    internal static string TranslateResourceString(GameEngine? engine, string S) => engine?.Dictionary is not null ? engine.TranslateResourceString(S) : S;

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
            int hitPoints = target.HitPoints;
            result = target.GetTargetPriority() + Round(targetPriority * ((double)sourceDamageValue / (hitPoints * 100)));
            if (targetDamageValue != 0)
            {
                int sourceTargetPriority = source.GetTargetPriority();
                int sourceHitPoints = source.HitPoints;
                result -= source.GetTargetPriority() + Round(sourceTargetPriority * ((double)targetDamageValue / (sourceHitPoints * 100)));
            }
        }
        else
        {
            return sourceDamageValue + -99_999 - (source.GetTargetPriority() * targetDamageValue);
        }

        return result;
    }

    internal static int StatisticsToMaxDamage(int damage, DamageTypes damageTypes, DamageTypes immunityTypes, DamageTypes protectionTypes)
    {
        damageTypes &= ~immunityTypes;
        return damageTypes != DamageTypes.None
            ? (damageTypes & ~protectionTypes) == DamageTypes.None ? (damage + 1) >>> 1 : damage
            : 0;
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi damage calculation signature keeps resistance.")]
    internal static int StatisticsToLimitedDamageValue(int attack, int damage, DamageTypes damageTypes, int maximum, int defense, int resistance, DamageTypes immunityTypes, DamageTypes protectionTypes)
    {
        int limit = maximum;
        damageTypes &= ~immunityTypes;
        int result;
        if (damageTypes != DamageTypes.None)
        {
            attack -= defense;
            if ((damageTypes & ~protectionTypes) == DamageTypes.None)
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

    internal static int RoadConstructionCost(PlayerId? player, MovePath path, int movePoints, GameEngine engine)
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
                MapField field = engine.StrategicMap.MapContainer.GetFieldMapCoordinate(item.Location);
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

    internal static void ShowDamageIcon(GameEngine? engine, int x, int y, in LockedSurface surface, int damage, DamageType damageType)
    {
        if (damageType != DamageType.NonSpecific)
        {
            engine.GamePack.CombatImages[(int)damageType + 10].Show(x + 15, y + 10, lockedSurface: surface);
            engine.CombatFont.Color = damageType is (>= DamageType.Cold and <= DamageType.Magic) or DamageType.Holy
                ? Color.Black
                : Color.White;

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

    internal static int PlaceRandomUnits(int X, int Y, int L, PlayerId? player, UnitIndexList units, int level, int value, UnitTypes types, GameEngine? engine)
    {
        int result = 0;
        if (units.Count != 0)
        {
            int tries = 50;
            while (result < value && tries != 0)
            {
                tries -= 1;
                int index = units[Random(units.Count)];
                if (index != -1 && types.Contains(engine.GamePack.UnitResources.GetUnitResource(index).UnitType.ToFlag()) && engine.GamePack.UnitResources.GetUnitResource(index).GetObtainValue() + result > value && engine.GamePack.UnitResources.GetUnitResource(index).Level <= level)
                {
                    int bestIndex = index;
                    int i = 20;
                    int difference = Math.Abs(engine.GamePack.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
                    do
                    {
                        index = units[Random(units.Count)];
                        if (index != -1 && types.Contains(engine.GamePack.UnitResources.GetUnitResource(index).UnitType.ToFlag()))
                        {
                            int delta = Math.Abs(engine.GamePack.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
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

                if (index != -1 && types.Contains(engine.GamePack.UnitResources.GetUnitResource(index).UnitType.ToFlag()) && engine.GamePack.UnitResources.GetUnitResource(index).Level <= level)
                {
                    Unit unit = new(null) { Holder = engine };
                    unit.SetUnitResource(engine.GamePack.UnitResources.GetUnitResource(index));
                    if (unit.Place(X, Y, L, player, mode: UnitPlaceModes.None) is not null)
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

    internal static Alignment IndependentRelationToAlignment(int relation, GameEngine engine)
    {
        return relation < 0
            ? relation switch
            {
                -2 => Alignment.Good,
                -4 => Alignment.Evil,
                -3 => Alignment.Neutral,
                _ => Alignment.Neutral,
            }
            : engine.GamePack.RaceResources.GetRaceResource(relation).Alignment;
    }

    internal static DiplomaticRelation DiplomaticRelation(PlayerId? player, int independentRelation, PlayerId? otherPlayer, int otherIndependentRelation, GameEngine engine)
    {
        if (player == PlayerId.Independent || otherPlayer == PlayerId.Independent)
        {
            int race = player != PlayerId.Independent ? engine.StrategicMap.Players.GetPlayers(player).PlayerRace
                : independentRelation >= 0 ? independentRelation : -1;

            int otherRace = otherPlayer != PlayerId.Independent ? engine.StrategicMap.Players.GetPlayers(otherPlayer).PlayerRace
                : otherIndependentRelation >= 0 ? otherIndependentRelation : -1;

            if (race == -1 || otherRace == -1)
            {
                Alignment alignment = race != -1
                    ? engine.GamePack.RaceResources.GetRaceResource(race).Alignment
                    : IndependentRelationToAlignment(independentRelation, engine: engine);

                Alignment otherAlignment = otherRace != -1
                    ? engine.GamePack.RaceResources.GetRaceResource(otherRace).Alignment
                    : IndependentRelationToAlignment(otherIndependentRelation, engine: engine);

                return AlignmentTables.AlignmentToDiplomaticRelation(alignment, otherAlignment);
            }
            else
            {
                return AlignmentTables.StatusToDiplomaticRelation(engine.StrategicMap.Races.GetRace(race).GetRelation(otherRace));
            }
        }
        else
        {
            return player != otherPlayer
                ? engine.StrategicMap.Players.GetDiplomaticRelation(player, otherPlayer)
                : Session.DiplomaticRelation.Alliance;
        }
    }

    internal static bool ValidAttackRelation(PlayerId? player, int independentRelation, PlayerId? targetPlayer, int targetRace, GameEngine engine)
    {
        if (player == PlayerId.Independent)
        {
            if (targetPlayer != PlayerId.Independent)
            {
                DiplomaticRelation relation = DiplomaticRelation(player, independentRelation, otherPlayer: targetPlayer, otherIndependentRelation: targetRace, engine: engine);
                return relation is Session.DiplomaticRelation.War or Session.DiplomaticRelation.Unknown;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return engine.StrategicMap.Players.GetPlayers(player).ValidAttackRelation(targetPlayer, independentRelation: targetRace);
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

    internal static int GetMovePathBattleFieldStrength(PlayerId? player, int independentRelation, MovePath movePath, int targetRange, int range, int step, GameEngine? engine)
    {
        int result;
        if (movePath is not null && movePath.Count > 0)
        {
            int distance = Round(range * 0.5);
            result = 0;
            int i = movePath.Count - 1;
            UnitUserList? units = null;
            if (movePath.Count - targetRange < i)
            {
                i = movePath.Count - targetRange;
            }

            BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, player, relation: independentRelation, requests: BattleFieldInfoRequests.None, damageTypes: s_allDamageTypes & ~DamageTypes.Wall);
            info.FriendlyRelations = DiplomaticRelations.None;
            info.Requests |= BattleFieldInfoRequests.IgnorePassiveIndependents | BattleFieldInfoRequests.EffectiveStrength;
            if (step > 0)
            {
                units = new UnitUserList(null) { Holder = engine };
                PathStep item = movePath[^1];
                MapLevel level = engine.StrategicMap.MapContainer.MapLevels[item.Location.MapLevel];
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
                engine.StrategicMap.RetrieveBattleFieldInfo(item2.Location.X, movePathItem.Location.Y, item.Location.MapLevel, radius: range, ref info);
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
                        engine.StrategicMap.RetrieveBattleFieldInfo(item5.Location.X, item4.Location.Y, item3.Location.MapLevel, radius: range, ref info);
                        if (info.Strength > result)
                        {
                            result = info.Strength;
                        }
                    }
                }

                i = j;
            }

            units?.Dispose();

            engine.StrategicMap.AIExecutor.UseProcessTime(50);
        }
        else
        {
            return 0;
        }

        return result;
    }

    internal static EnhancementAbility CreateEnhancementAbility(int id, string name, AbilitySelectionTypes selectionTypes, GameEngine engine)
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

    internal static readonly Color[] RaceColors = [new(48, 52, 224), new(255, 164, 102), new(44, 203, 175), new(135, 218, 254), new(65, 239, 96), new(253, 253, 21), new(228, 193, 165), new(253, 255, 247), new(188, 112, 179), new(233, 80, 80), new(187, 102, 66), new(152, 120, 121), new(168, 168, 168)];

    private static readonly DamageTypes s_allDamageTypes = DamageTypes.Fire | DamageTypes.Cold | DamageTypes.Lightning | DamageTypes.Magic | DamageTypes.Poison | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Physical | DamageTypes.Wall | DamageTypes.NonSpecific;

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

    internal static bool EndPlayerTurn(PlayerId? player, GameEngine? engine)
    {
        StrategicMap strategicMap = engine.StrategicMap;
        EndTurnTokenEvent eventValue = new(null)
        {
            Holder = engine,
            Player = player,
            Day = strategicMap.Day,
            Mode = EndTurnTokenEventMode.EndTurn
        };
        strategicMap.TokenManager.TokenControls[player].AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    internal static bool CanConstructRoad(PlayerId? player, MapField field)
    {
        bool result = player == field.GetPlayer() || field.GetPlayer() is null;
        result = result && field.Terrain is >= TerrainType.Grass and <= TerrainType.Wasteland;
        result = result && field.OverlayType is not (OverlayType.Mountain or OverlayType.Hill or OverlayType.Road or OverlayType.Bridge);
        // Original shared helper
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null;
    }

    public static void RetrieveStormDamageInfo(PlayerId? player, StormType storm, int x, int y,
                int level, int innerRadius, int outerRadius, out StormDamageInfo info, GameEngine engine)
    {
        StormDamageQuery query = new()
        {
            Info = new()
            {
                Player = player.GetValueOrDefault(),
                FriendlyRelations = DiplomaticRelations.Peace | DiplomaticRelations.Alliance,
                EnemyRelations = DiplomaticRelations.War | DiplomaticRelations.Unknown
            }
        };
        MapLevel mapLevel = engine.StrategicMap.MapContainer.GetMapLevel(level);
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

    internal static DamageTypes AllDamageTypes { get; } = DamageTypes.Fire | DamageTypes.Cold | DamageTypes.Lightning | DamageTypes.Magic | DamageTypes.Poison | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Physical | DamageTypes.Wall | DamageTypes.NonSpecific;

    internal static void TriggerStormDamage(StormType storm, int x, int y, int mapLevel, int innerRadius, int outerRadius, PlayerId? player, GameEngine engine)
    {
        MapFieldMessage message = default;
        MapLevel level = engine.StrategicMap.MapContainer.MapLevels[mapLevel];
        message.MessageId = 4432;
        MapFieldMessageParameters param = message.Param;
        param[0] = (int)storm;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[2] = Session.MsgArgIds.FromPlayer(player);
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
