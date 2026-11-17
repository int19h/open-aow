#nullable disable
using System.Collections.Immutable;
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
using static Aow.Rtl.RtlUnit;

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
            result = 0;
        }

        return result;
    }

    internal static int ExpectedDamage(int damage, int attackDefense)
    {
        int result;
        int threshold;
        int correction;
        if (damage > 0)
        {
            threshold = 10 - (attackDefense * 2);
            if (threshold > 18)
            {
                result = damage;
            }
            else
            {
                if (threshold >= 0)
                {
                    result = (int)(((uint)(((damage - 1) * (18 - threshold)) + 1)) >> 1) + damage + (18 - threshold);
                    if (threshold != 0)
                    {
                        result += 1;
                    }
                }
                else
                {
                    result = damage + 1 + (18 - threshold) + (int)(((uint)(((damage - 1) * (18 - threshold)) + 1)) >> 1);
                    threshold = -threshold;
                    correction = (int)(((uint)((((damage - 1) * threshold * threshold) + threshold + 19) / (threshold + 19))) >> 1);
                    threshold = correction + threshold + 1;
                    result = result - threshold;
                }
            }

            result = result * 5;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static int ExecuteDamageRole(int damage, int attackDefense)
    {
        int result;
        int roll;
        if (damage > 0)
        {
            roll = Random(20);
            if (roll <= 1)
            {
                result = 0;
            }
            else
            {
                if (roll >= 18)
                {
                    result = damage;
                }
                else
                {
                    attackDefense = 10 - (attackDefense * 2);
                    result = roll >= attackDefense && attackDefense < 18
                        ? ((((damage - 1) * (roll - attackDefense)) + (int)(((uint)(18 - attackDefense)) >> 1)) / (18 - attackDefense)) + 1
                        : 0;
                }
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static int DamageValueToDamageEvaluation(int sourceDamageValue, int targetDamageValue, CombatObject source, CombatObject target)
    {
        int result;
        if (target.GetTargetPriority() != 0)
        {
            int targetPriority = target.GetTargetPriority();
            sbyte hitPoints = target.GetHitPoints();
            result = target.GetTargetPriority() + Round(targetPriority * ((double)sourceDamageValue / (hitPoints * 100)));
            if (targetDamageValue != 0)
            {
                int sourceTargetPriority = source.GetTargetPriority();
                sbyte sourceHitPoints = source.GetHitPoints();
                result = result - (source.GetTargetPriority() + Round(sourceTargetPriority * ((double)targetDamageValue / (sourceHitPoints * 100))));
            }
        }
        else
        {
            result = sourceDamageValue + -99999 - (source.GetTargetPriority() * targetDamageValue);
        }

        return result;
    }

    internal static int StatisticsToMaxDamage(int damage, Set<DamageType> damageTypes, Set<DamageType> immunityTypes, Set<DamageType> protectionTypes)
    {
        int result;
        damageTypes = damageTypes - immunityTypes;
        result = damageTypes != Set<DamageType>.Of()
            ? damageTypes - protectionTypes == Set<DamageType>.Of() ? (int)(((uint)(damage + 1)) >> 1) : damage
            : 0;

        return result;
    }

    internal static int StatisticsToLimitedDamageValue(int attack, int damage, Set<DamageType> damageTypes, int maximum, int defense, int resistance, Set<DamageType> immunityTypes, Set<DamageType> protectionTypes)
    {
        int result;
        int limit;
        int limit100;
        limit = maximum;
        damageTypes = damageTypes - immunityTypes;
        if (damageTypes != Set<DamageType>.Of())
        {
            attack = attack - defense;
            if (damageTypes - protectionTypes == Set<DamageType>.Of())
            {
                damage = (int)(((uint)(damage + 1)) >> 1);
            }

            result = ExpectedDamageCapped(damage, attack, limit);
            limit100 = limit * 100;
            if (result > limit100)
            {
                result = limit100;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static int RoadConstructionCost(sbyte player, MovePath path, int movePoints)
    {
        int i;
        int points;
        MapField field;
        int result = 0;
        i = path.Count;
        points = 0;
        while (i != 0 && points <= movePoints)
        {
            i -= 1;
            points += path[i].MoveCost;
            if (points <= movePoints)
            {
                PathStep item = path[i];
                field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(item.Location);
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
        string result;
        try
        {
            result = RtlUnit.Format(S, arguments);
        }
        catch
        {
            result = S;
        }

        return result;
    }

    internal static void ShowDamageIcon(GameEngine engine, int x, int y, in LockedSurface surface, int damage, DamageType damageType)
    {
        if (damageType != DamageType.None)
        {
            GamePack.Current.CombatImages[(int)damageType + 10].Show(x + 15, y + 10, surface);
            engine.CombatFont.Color = Set<DamageType>.Of(DamageType.Cold, DamageType.Lightning, DamageType.Magic, DamageType.Holy).Contains(damageType)
                ? 0
                : 16777215;

            engine.CombatFont.SetTextCentered(x + 34, y + 36, IntToStr(damage), surface);
        }
    }

    internal static int MoraleToMoraleValue(int value)
    {
        int result;
        switch (value & 127)
        {
            case 0:
                result = 10;
                break;
            case 1:
                result = 30;
                break;
            case 2:
                result = 50;
                break;
            case 3:
                result = 70;
                break;
            case 4:
                result = 90;
                break;
            default:
                result = 50;
                // { Original leaks the exception without raising it. }
                new ArgumentException("Invalid TMoral Value");
                break;
        }

        return result;
    }

    internal static Troop.Morale MoraleValueToMorale(int value)
    {
        Troop.Morale result;
        if (value <= 20)
        {
            result = Troop.Morale.Terrible;
        }
        else
        {
            if (value > 80)
            {
                result = Troop.Morale.High;
            }
            else
            {
                switch (value)
                {
                    case >= 21 and <= 40:
                        result = Troop.Morale.Poor;
                        break;
                    case >= 41 and <= 60:
                        result = Troop.Morale.Ok;
                        break;
                    case >= 61 and <= 80:
                        result = Troop.Morale.Good;
                        break;
                    default:
                        result = Troop.Morale.Ok;
                        // { Original leaks the exception without raising it. }
                        new ArgumentException("Invalid MoralValue");
                        break;
                }
            }
        }

        return result;
    }

    internal static Status AlignmentsToStatus(Set<Alignment> A, Set<Alignment> B)
    {
        Status result = A == B ? Status.Friendly : (A & B) == Set<Alignment>.Of() ? Status.Hostile : Status.Neutral;
        return result;
    }

    internal static int AlignmentsToInt32(Set<Alignment> alignments)
    {
        int result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return result;
    }

    internal static Set<Alignment> ToAlignments(int value)
    {
        Set<Alignment> result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return result;
    }

    internal static int PlaceRandomUnits(int X, int Y, int L, sbyte player, UnitIndexList units, int level, int value, Set<UnitType> types, GameEngine engine)
    {
        int index;
        int tries;
        int difference;
        int bestIndex;
        int i;
        int delta;
        Unit unit;
        int result = 0;
        tries = 50;
        if (units.Count != 0)
        {
            while (result < value && tries != 0)
            {
                tries -= 1;
                index = units[Random(units.Count)];
                if (index != -1 && types.Contains(GamePack.Current.UnitResources.GetUnitResource(index).UnitType) && GamePack.Current.UnitResources.GetUnitResource(index).GetObtainValue() + result > value && GamePack.Current.UnitResources.GetUnitResource(index).Level <= level)
                {
                    bestIndex = index;
                    i = 20;
                    difference = Math.Abs(GamePack.Current.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
                    do
                    {
                        index = units[Random(units.Count)];
                        if (index != -1 && types.Contains(GamePack.Current.UnitResources.GetUnitResource(index).UnitType))
                        {
                            delta = Math.Abs(GamePack.Current.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
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
                    unit = new Unit(null) { Holder = engine };
                    unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                    if (unit.Place(X, Y, L, player, Set<UnitPlaceModeElement>.Of()) != null)
                    {
                        result += unit.GetObtainValue();
                        tries = 50;
                    }

                    unit.Release();
                }
            }
        }

        return result;
    }

    internal static Alignment IndependentRelationToAlignment(sbyte relation)
    {
        Alignment result;
        if (relation < 0)
        {
            switch (relation)
            {
                case -2:
                    result = Alignment.Good;
                    break;
                case -4:
                    result = Alignment.Evil;
                    break;
                case -3:
                    result = Alignment.Neutral;
                    break;
                default:
                    result = Alignment.Neutral;
                    break;
            }
        }
        else
        {
            result = GamePack.Current.RaceResources.GetRaceResource(relation).GetAlignment();
        }

        return result;
    }

    internal static DiplomaticRelation DiplomaticRelation(sbyte player, sbyte independentRelation, sbyte otherPlayer, sbyte otherIndependentRelation)
    {
        DiplomaticRelation result;
        sbyte race;
        sbyte otherRace;
        Alignment alignment;
        Alignment otherAlignment;
        if (player == 0 || otherPlayer == 0)
        {
            race = player != 0
                ? StrategicMap.Current.Players.GetPlayers(player).PlayerRace
                : independentRelation >= 0 ? independentRelation : (sbyte)-1;

            otherRace = otherPlayer != 0
                ? StrategicMap.Current.Players.GetPlayers(otherPlayer).PlayerRace
                : otherIndependentRelation >= 0 ? otherIndependentRelation : (sbyte)-1;

            if (race == -1 || otherRace == -1)
            {
                alignment = race != -1
                    ? GamePack.Current.RaceResources.GetRaceResource(race).GetAlignment()
                    : IndependentRelationToAlignment(independentRelation);

                otherAlignment = otherRace != -1
                    ? GamePack.Current.RaceResources.GetRaceResource(otherRace).GetAlignment()
                    : IndependentRelationToAlignment(otherIndependentRelation);

                result = AlignmentTables.AlignmentToDiplomaticRelation(alignment, otherAlignment);
            }
            else
            {
                result = AlignmentTables.StatusToDiplomaticRelation(StrategicMap.Current.Races.GetRace(race).GetRelation(otherRace));
            }
        }
        else
        {
            result = player != otherPlayer
                ? StrategicMap.Current.Players.GetDiplomaticRelation(player, otherPlayer)
                : Session.DiplomaticRelation.Alliance;
        }

        return result;
    }

    internal static bool ValidAttackRelation(sbyte player, sbyte independentRelation, sbyte targetPlayer, sbyte targetRace)
    {
        bool result;
        DiplomaticRelation relation;
        if (player == 0)
        {
            if (targetPlayer != 0)
            {
                relation = DiplomaticRelation(player, independentRelation, targetPlayer, targetRace);
                result = Set<DiplomaticRelation>.Of(Session.DiplomaticRelation.War, Session.DiplomaticRelation.Unknown).Contains(relation);
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = StrategicMap.Current.Players.GetPlayers(player).ValidAttackRelation(targetPlayer, targetRace);
        }

        return result;
    }

    internal static void PolarToCartesian(int degrees, int radius, out int X, out int Y)
    {
        float radians;
        radians = (float)((double)(degrees * Math.PI) / 180);
        X = Round((double)Math.Cos(radians) * radius);
        Y = Round((double)Math.Sin(radians) * radius);
    }

    internal static string Encode(string text, int mode)
    {
        string result = "";
        for (int i = 1; i <= text.Length; i++)
        {
            result = result + Cp1252.Text((byte)(Cp1252.Byte(text[i - 1]) ^ ((text.Length + i + 5) & 255)));
        }

        return result;
    }

    internal static Set<DamageType> InvertDamageTypes(Set<DamageType> types)
    {
        DamageType damageType;
        Set<DamageType> result = Set<DamageType>.Of();
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            damageType = (DamageType)damageTypeOrdinal;
            if (!types.Contains(damageType))
            {
                result = result + Set<DamageType>.Of(damageType);
            }
        }

        return result;
    }

    internal static int GetMovePathBattleFieldStrength(sbyte player, sbyte independentRelation, MovePath movePath, int targetRange, int range, int step, GameEngine engine)
    {
        int result;
        UnitUserList units;
        int i;
        int j;
        int distance;
        HexNumberWalkData walk = default;
        BattleFieldInfo info = default;
        MapLevel level;
        MapField field;
        ArmyHexSprite army;
        if (movePath != null && movePath.Count > 0)
        {
            distance = Round(range * 0.5);
            result = 0;
            i = movePath.Count - 1;
            units = null;
            if (movePath.Count - targetRange < i)
            {
                i = movePath.Count - targetRange;
            }

            BattleFieldInfo.SetupBattleFieldInfo(out info, player, independentRelation, Set<BattleFieldInfoRequestsElement>.Of(), s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents, BattleFieldInfoRequestsElement.EffectiveStrength);
            if (step > 0)
            {
                units = new UnitUserList(null) { Holder = engine };
                PathStep item = movePath[^1];
                level = StrategicMap.Current.MapContainer.MapLevels[item.Location.MapLevel];
                PathStep movePathItem = movePath[^1];
                PathStep item2 = movePath[^1];
                HexNumberWalkData.InitializeHexNumberWalk(out walk, item2.Location.X, movePathItem.Location.Y, 0);
                while (walk.Radius <= step)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        field = level.GetField(walk.X, walk.Y);
                        if ((field.FieldFlags & MapField.BattleFieldInfoFlag) != 0)
                        {
                            army = field.GetArmyHexSprite();
                            if (army != null && army.Army.Player != player)
                            {
                                units.AddUnits(army.Army.Units);
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
                StrategicMap.Current.RetrieveBattleFieldInfo(item2.Location.X, movePathItem.Location.Y, item.Location.MapLevel, range, ref info);
                if (info.Strength > result)
                {
                    result = info.Strength;
                }

                j = i;
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
                        StrategicMap.Current.RetrieveBattleFieldInfo(item5.Location.X, item4.Location.Y, item3.Location.MapLevel, range, ref info);
                        if (info.Strength > result)
                        {
                            result = info.Strength;
                        }
                    }
                }

                i = j;
            }

            if (units != null)
            {
                units?.Dispose();
            }

            StrategicMap.Current.AIExecutor.UseProcessTime(50);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static EnhancementAbility CreateEnhancementAbility(int id, string name, Set<AbilitySelectionType> selectionTypes, GameEngine engine)
    {
        EnhancementAbility ability;
        ability = new EnhancementAbility(null) { Holder = engine };
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.SelectionTypes = selectionTypes;
        return ability;
    }

    internal static ReadOnlySpan<int> CommandAbilityIds => [29, 28, 148];

    internal static ReadOnlySpan<int> RaceColors => [14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224];

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
        EndTurnTokenEvent eventValue;
        eventValue = new EndTurnTokenEvent(null) { Holder = engine };
        eventValue.Player = player;
        eventValue.Day = StrategicMap.Current.Day;
        eventValue.Mode = EndTurnTokenEventMode.EndTurn;
        StrategicMap.Current.TokenManager.TokenControls[player].AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    internal static int OffensiveStrength(int attack, int damage, int repeats)
    {
        if (damage > 10)
        {
            damage = 10;
        }

        return attack * damage * repeats / 2;
    }

    internal static bool CanConstructRoad(sbyte player, MapField field)
    {
        bool result = player == field.GetPlayer() || field.GetPlayer() == -1;
        result = result && Set<byte>.WithBounds(0, 255).WithRange(1, 5).Contains((byte)field.TerrainType);
        result = result && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(4).With(5).Contains((byte)field.OverlayType);
        // Original shared helper
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) == null;
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
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, innerRadius);
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
        finally { info = query.Info; }
    }

    internal static Set<MoveType> ToMoveTypes(int value) => Set<MoveType>.FromMask((byte)value);

    internal static Set<DamageType> AllDamageTypes { get; } = Set<DamageType>.FromMask(1023UL);

    internal static void TriggerStormDamage(StormType storm, sbyte x, sbyte y, sbyte mapLevel, sbyte innerRadius, sbyte outerRadius, sbyte player)
    {
        MapFieldMessage message = default;
        MapLevel level;
        level = StrategicMap.Current.MapContainer.MapLevels[mapLevel];
        message.MessageId = 4432;
        MapFieldMessageParameters param = message.Param;
        param[0] = (int)storm;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[2] = player;
        message.Param = param2;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, innerRadius);
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
