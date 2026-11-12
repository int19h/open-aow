using System.Collections.Immutable;

using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

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

    internal static string TranslateResourceString(string S)
    {
        if (GameTables.GameEngine != null)
        {
            return GameTables.GameEngine.TranslateResourceString(S);
        }

        return S;
    }

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
            threshold = 10 - attackDefense * 2;
            if (threshold > 18)
            {
                result = damage;
            }
            else
            {
                if (threshold >= 0)
                {
                    result = (int)(((uint)((damage - 1) * (18 - threshold) + 1)) >> 1) + (damage + (18 - threshold));
                    if (threshold != 0)
                    {
                        result += 1;
                    }
                }
                else
                {
                    result = damage + 1 + (18 - threshold) + (int)(((uint)((damage - 1) * (18 - threshold) + 1)) >> 1);
                    threshold = -threshold;
                    correction = (int)(((uint)(((damage - 1) * threshold * threshold + (threshold + 19)) / (threshold + 19))) >> 1);
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
                    attackDefense = 10 - attackDefense * 2;
                    if (roll >= attackDefense && attackDefense < 18)
                    {
                        result = ((damage - 1) * (roll - attackDefense) + (int)(((uint)(18 - attackDefense)) >> 1)) / (18 - attackDefense) + 1;
                    }
                    else
                    {
                        result = 0;
                    }
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
            var targetPriority = target.GetTargetPriority();
            var hitPoints = target.GetHitPoints();
            result = target.GetTargetPriority() + Round((double)targetPriority * ((double)sourceDamageValue / (hitPoints * 100)));
            if (targetDamageValue != 0)
            {
                var sourceTargetPriority = source.GetTargetPriority();
                var sourceHitPoints = source.GetHitPoints();
                result = result - (source.GetTargetPriority() + Round((double)sourceTargetPriority * ((double)targetDamageValue / (sourceHitPoints * 100))));
            }
        }
        else
        {
            result = sourceDamageValue + -99999 - source.GetTargetPriority() * targetDamageValue;
        }

        return result;
    }

    internal static int StatisticsToMaxDamage(int damage, Set<DamageType> damageTypes, Set<DamageType> immunityTypes, Set<DamageType> protectionTypes)
    {
        int result;
        damageTypes = damageTypes - immunityTypes;
        if (damageTypes != Set<DamageType>.Of())
        {
            if (damageTypes - protectionTypes == Set<DamageType>.Of())
            {
                result = (int)(((uint)(damage + 1)) >> 1);
            }
            else
            {
                result = damage;
            }
        }
        else
        {
            result = 0;
        }

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

    internal static void MaximizeMovePointTable(Span<byte> table, ReadOnlySpan<byte> other)
    {
        sbyte oldValue;
        sbyte value;
        for (int I = 0; I <= 12; I++)
        {
            for (int J = 0; J <= 15; J++)
            {
                oldValue = MemoryMarshal.Cast<byte, MovePointTable>(table)[0][J][I];
                value = MemoryMarshal.Cast<byte, MovePointTable>(other)[0][J][I];
                if (oldValue != -1 && (oldValue < value || value == -1))
                {
                    MemoryMarshal.Cast<byte, MovePointTable>(table)[0][J][I] = value;
                }
            }
        }
    }

    internal static int RoadConstructionCost(sbyte player, MovePath path, int movePoints)
    {
        int I;
        int points;
        AoWMapField field;
        int result = 0;
        I = path.Count;
        points = 0;
        while (I != 0 && points <= movePoints)
        {
            I -= 1;
            points += path[I].Tag;
            if (points <= movePoints)
            {
                var item = path[I];
                field = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(item.Packed);
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

    internal static void ShowDamageIcon(int X, int Y, in LockedSurface surface, int damage, DamageType damageType)
    {
        if (damageType != DamageType.None)
        {
            AoWHSSet.Current.CombatImages[(int)damageType + 10].Show(X + 15, Y + 10, surface);
            if (Set<DamageType>.Of(DamageType.Cold, DamageType.Lightning, DamageType.Magic, DamageType.Holy).Contains(damageType))
            {
                GameTables.GameEngine.CombatFont.Color = 0;
            }
            else
            {
                GameTables.GameEngine.CombatFont.Color = 16777215;
            }

            GameTables.GameEngine.CombatFont.SetTextCentered(X + 34, Y + 36, IntToStr(damage), surface);
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
        Status result;
        if (A == B)
        {
            result = Status.Friendly;
        }
        else
        {
            if ((A & B) == Set<Alignment>.Of())
            {
                result = Status.Hostile;
            }
            else
            {
                result = Status.Neutral;
            }
        }

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

    internal static int PlaceRandomUnits(int X, int Y, int L, sbyte player, UnitIndexList units, int level, int value, Set<UnitType> types)
    {
        int index;
        int tries;
        int difference;
        int bestIndex;
        int I;
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
                if (index != -1 && types.Contains(AoWHSSet.Current.UnitResources.GetUnitResource(index).UnitType) && AoWHSSet.Current.UnitResources.GetUnitResource(index).GetObtainValue() + result > value && AoWHSSet.Current.UnitResources.GetUnitResource(index).Level <= level)
                {
                    bestIndex = index;
                    I = 20;
                    difference = Math.Abs(AoWHSSet.Current.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
                    do
                    {
                        index = units[Random(units.Count)];
                        if (index != -1 && types.Contains(AoWHSSet.Current.UnitResources.GetUnitResource(index).UnitType))
                        {
                            delta = Math.Abs(AoWHSSet.Current.UnitResources.GetUnitResource(index).GetObtainValue() - (value - result));
                            if (delta < difference)
                            {
                                difference = delta;
                                bestIndex = index;
                            }
                        }

                        I -= 1;
                    }
                    while (!(I == 0));
                    index = bestIndex;
                }

                if (index != -1 && types.Contains(AoWHSSet.Current.UnitResources.GetUnitResource(index).UnitType) && AoWHSSet.Current.UnitResources.GetUnitResource(index).Level <= level)
                {
                    unit = new Unit(null);
                    unit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(index));
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
            result = AoWHSSet.Current.RaceResources.GetRaceResource(relation).GetAlignment();
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
            if (player != 0)
            {
                race = AoWHSMap.Current.Players.GetPlayers(player).PlayerRace;
            }
            else
            {
                if (independentRelation >= 0)
                {
                    race = independentRelation;
                }
                else
                {
                    race = -1;
                }
            }

            if (otherPlayer != 0)
            {
                otherRace = AoWHSMap.Current.Players.GetPlayers(otherPlayer).PlayerRace;
            }
            else
            {
                if (otherIndependentRelation >= 0)
                {
                    otherRace = otherIndependentRelation;
                }
                else
                {
                    otherRace = -1;
                }
            }

            if (race == -1 || otherRace == -1)
            {
                if (race != -1)
                {
                    alignment = AoWHSSet.Current.RaceResources.GetRaceResource(race).GetAlignment();
                }
                else
                {
                    alignment = IndependentRelationToAlignment(independentRelation);
                }

                if (otherRace != -1)
                {
                    otherAlignment = AoWHSSet.Current.RaceResources.GetRaceResource(otherRace).GetAlignment();
                }
                else
                {
                    otherAlignment = IndependentRelationToAlignment(otherIndependentRelation);
                }

                result = AlignmentTables.AlignmentToDiplomaticRelation(alignment, otherAlignment);
            }
            else
            {
                result = AlignmentTables.StatusToDiplomaticRelation(AoWHSMap.Current.Races.GetRace(race).GetRelation(otherRace));
            }
        }
        else
        {
            if (player != otherPlayer)
            {
                result = AoWHSMap.Current.Players.GetDiplomaticRelation(player, otherPlayer);
            }
            else
            {
                result = Session.DiplomaticRelation.Alliance;
            }
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
            result = AoWHSMap.Current.Players.GetPlayers(player).ValidAttackRelation(targetPlayer, targetRace);
        }

        return result;
    }

    internal static void PolarToCartesian(int degrees, int radius, ref int X, ref int Y)
    {
        float radians;
        radians = (float)((double)((double)degrees * Math.PI) / 180);
        X = Round((double)Math.Cos(radians) * radius);
        Y = Round((double)Math.Sin(radians) * radius);
    }

    internal static string Encode(string text, int mode)
    {
        string result = "";
        for (int I = 1; I <= text.Length; I++)
        {
            result = result + Cp1252.Text((byte)(Cp1252.Byte(text[I - 1]) ^ ((text.Length + (I + 5)) & 255)));
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

    internal static int GetMovePathBattleFieldStrength(sbyte player, sbyte independentRelation, MovePath movePath, int targetRange, int range, int step)
    {
        int result;
        UnitUserList units;
        int I;
        int J;
        int distance;
        HexNumberWalkData walk = default;
        BattleFieldInfo info = default;
        MapLevel level;
        AoWMapField field;
        ArmyHexSprite army;
        if (movePath != null && movePath.Count > 0)
        {
            distance = Round((double)range * 0.5);
            result = 0;
            I = movePath.Count - 1;
            units = null;
            if (movePath.Count - targetRange < I)
            {
                I = movePath.Count - targetRange;
            }

            BattleFieldInfo.SetupBattleFieldInfo(ref info, player, independentRelation, Set<BattleFieldInfoRequestsElement>.Of(), s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents, BattleFieldInfoRequestsElement.EffectiveStrength);
            if (step > 0)
            {
                units = new UnitUserList(null);
                var item = movePath[movePath.Count - 1];
                level = AoWHSMap.Current.MapContainer.MapLevels[item.MapLevel];
                var movePathItem = movePath[movePath.Count - 1];
                var item2 = movePath[movePath.Count - 1];
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, item2.X, movePathItem.Y, 0);
                while (walk.Radius <= step)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        field = (AoWMapField)level.GetField(walk.X, walk.Y);
                        if ((field.FieldFlags & 67108864) != 0)
                        {
                            army = field.GetArmyHexSprite();
                            if (army != null && army.Army.Player != player)
                            {
                                units.AddUnits(army.Army);
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                info.Units = units;
            }

            while (I >= 0)
            {
                info.Strength = 0;
                var item = movePath[I];
                var movePathItem = movePath[I];
                var item2 = movePath[I];
                AoWHSMap.Current.RetrieveBattleFieldInfo(item2.X, movePathItem.Y, item.MapLevel, range, ref info);
                if (info.Strength > result)
                {
                    result = info.Strength;
                }

                J = I;
                while (true)
                {
                    bool condition = J >= 0;
                    if (condition)
                    {
                        var item3 = movePath[J];
                        var item4 = movePath[I];
                        condition = item3.MapLevel == item4.MapLevel;
                    }

                    if (!(condition && J > I - distance))
                        break;
                    J -= 1;
                }

                bool test = J == 0;
                if (!test)
                {
                    var item3 = movePath[I];
                    var item4 = movePath[J];
                    test = item3.MapLevel != item4.MapLevel;
                }

                if (test)
                {
                    if (J - 1 != I)
                    {
                        info.Strength = 0;
                        var item3 = movePath[J + 1];
                        var item4 = movePath[J + 1];
                        var item5 = movePath[J + 1];
                        AoWHSMap.Current.RetrieveBattleFieldInfo(item5.X, item4.Y, item3.MapLevel, range, ref info);
                        if (info.Strength > result)
                        {
                            result = info.Strength;
                        }
                    }
                }

                I = J;
            }

            if (units != null)
            {
                units?.Dispose();
            }

            AoWHSMap.Current.AIExecutor.UseProcessTime(50);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static EnhancementAbility CreateEnhancementAbility(int id, string name, Set<AbilitySelectionType> selectionTypes)
    {
        EnhancementAbility ability;
        ability = new EnhancementAbility(null);
        ability.AbilityId = id;
        ability.AbilityName = name;
        ability.SelectionTypes = selectionTypes;
        return ability;
    }

    internal static ReadOnlySpan<int> CommandAbilityIds => [29, 28, 148];

    internal static ReadOnlySpan<int> RaceColors => [14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224];

    internal static Set<DamageType> s_allDamageTypes = Set<DamageType>.FromMask(1023UL);

    internal static ImmutableArray<string> ExplosionSfx { get; } = ["SFX\\ATTACK\\STORM.WAV", "SFX\\ATTACK\\EXPLOD1.WAV"];

    internal static int AiDebugChannel { get; set; }

    internal static ReadOnlySpan<byte> ArmySelectionMasks => [0, 1, 3, 7, 15, 31, 63, 127, 255];

    internal static int TemporaryDebugChannel { get; set; }

    internal static EngineClass MultiLevelDataClass { get; } = EngineClass.Of<MultiLevelAbilityData>();

    internal static int LimitedDamage(int attack, int defense, int damage)
    {
        if (defense > 10)
        {
            defense = 10;
        }

        return attack * defense * damage / 2;
    }

    internal static bool EndPlayerTurn(sbyte player)
    {
        EndTurnTokenEvent eventValue;
        eventValue = new EndTurnTokenEvent(null);
        eventValue.Player = player;
        eventValue.Day = AoWHSMap.Current.Day;
        eventValue.Mode = EndTurnTokenEventMode.EndTurn;
        AoWHSMap.Current.TokenManager.TokenControls[player].AddEvent(eventValue);
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

    internal static bool CanConstructRoad(sbyte player, AoWMapField field)
    {
        bool result = player == field.GetPlayer() || field.GetPlayer() == -1;
        result = result && Set<byte>.WithBounds(0, 255).WithRange(1, 5).Contains((byte)field.TerrainType);
        result = result && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(4).With(5).Contains((byte)field.OverlayType);
        // Original shared helper
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) == null;
    }

    public static void RetrieveStormDamageInfo(sbyte player, StormType storm, sbyte x, sbyte y,
                sbyte level, sbyte innerRadius, sbyte outerRadius, ref StormDamageInfo info)
    {
        var query = new StormDamageQuery
        {
            Info = new()
            {
                Player = player,
                FriendlyRelations = Set<byte>.WithBounds(0, 7).With(0).With(2),
                EnemyRelations = Set<byte>.WithBounds(0, 7).With(1).With(3)
            }
        };
        var mapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(level);
        var message = new MapFieldMessage { MessageId = 0x1151 };
        message.Param[0] = (int)storm;
        message.Param[2] = query;
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, x, y, innerRadius);
        try
        {
            while (walk.Radius <= outerRadius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    message.Param[1] = walk.Radius;
                    mapLevel.GetField(walk.X, walk.Y).SendMapFieldMessage(ref message);
                }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        finally { info = query.Info; }
    }

    internal static Set<MoveType> ToMoveTypes(int value) => Set<MoveType>.FromMask((byte)value);

    internal static Set<DamageType> AllDamageTypes { get; } = Set<DamageType>.FromMask(1023UL);

    internal static void TriggerStormDamage(StormType storm, sbyte X, sbyte Y, sbyte L, sbyte innerRadius, sbyte outerRadius, sbyte player)
    {
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        MapLevel level;
        level = AoWHSMap.Current.MapContainer.MapLevels[L];
        message.MessageId = 4432;
        message.Param[0] = (int)storm;
        message.Param[2] = player;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, innerRadius);
        while (walk.Radius <= outerRadius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                message.Param[1] = walk.Radius;
                level.Field[walk.X, walk.Y].SendMapFieldMessage(ref message);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

}
