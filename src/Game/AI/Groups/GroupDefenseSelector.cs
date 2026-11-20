#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

// { TDamageStrengths is shared with TDefenseRequirements in intf_P1_01.inc. }
// defenders, selected defense location
public sealed class GroupDefenseSelector : IDisposable
{
    // AIGroup.pas:1640
    public GroupDefenseSelector(EngineObject creator)
    {
        Creator = creator;
        Defenders = new UnitUserList(null) { Holder = Creator.CreationHolder() };
    }

    // The node that created this selector. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    public UnitUserList Defenders { get; }

    public MapCoordinate DefenseLocation { get; private set; }

    // AIGroup.pas:1646
    public void Dispose() => Defenders?.Dispose();

    // AIGroup.pas:1652
    public bool GetDefenders(sbyte player, sbyte X, sbyte Y, sbyte L, UnitUserList units)
    {
        bool result = false;
        MapCoordinate location = new(X, Y, L);
        DefensePriority priority = (DefensePriority)StrategicMap.Current.MapContainer.GetField(X, Y, L).GetDefensePriority();
        Player p = StrategicMap.Current.Players.GetPlayers(player);
        for (int i = p.AIGroups.Count - 1; i >= 0; i--)
        {
            AIGroup group = p.AIGroups.GetGroups(i);
            if (group.DefendLocation(location, priority))
            {
                _ = units.AddUnits(group.Units);
                result = true;
            }
        }

        return result;
    }

    // AIGroup.pas:1676
    public bool EnoughStrength(ReadOnlySpan<int> strengths)
    {
        bool result = default;
        for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.None; damageTypeOrdinal++)
        {
            DamageType damageType = (DamageType)damageTypeOrdinal;
            result = strengths[(int)damageType] <= 0;
            if (!result)
            {
                break;
            }
        }

        return result;
    }

    // AIGroup.pas:1687
    public bool UsefulUnit(Troop troop, ReadOnlySpan<int> strengths)
    {
        bool result = default;
        for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.None; damageTypeOrdinal++)
        {
            DamageType damageType = (DamageType)damageTypeOrdinal;
            result = strengths[(int)damageType] > 0 && troop.GetStrength(damageType) > 0;
            if (result)
            {
                break;
            }
        }

        return result;
    }

    // AIGroup.pas:1699
    public void UpdateDefenseLocation(MapField mapField)
    {
        if (DefenseLocation == MapCoordinate.None || mapField.FindOwnedHexSprite(typeof(Structure)) is not null)
        {
            MapCoordinate mapCoordinate = mapField.MapCoordinate;
            DefenseLocation = mapCoordinate;
        }
    }

    // AIGroup.pas:1728
    public bool Execute(AIGroup group, sbyte X, sbyte Y, sbyte L, UnitUserList units)
    {
        DefenseRequirements strengths = default;
        Defenders.Clear();
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(L);
        DefenseLocation = MapCoordinate.None;
        UnitUserList candidates = new(null) { Holder = Creator.CreationHolder() };
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        int i;
        while (walk.Radius <= 1 && Defenders.Count != group.Units.Count)
        {
            MapField mapField = level.GetField(walk.X, walk.Y);
            if (mapField.GetPlayer() == group.Player)
            {
                candidates.Clear();
                mapField.GetDefenseRequirements(group.Player, requirements: out strengths);
                Array10<int> openedStrengths = strengths.DamageStrengths;
                if (!EnoughStrength(openedStrengths))
                {
                    UpdateDefenseLocation(mapField);
                    _ = GetDefenders(group.Player, (sbyte)walk.X, (sbyte)walk.Y, L, units: candidates);
                    s_defenseSortGroupId = group.GroupId;
                    s_defenseSortUnits = units;
                    candidates.SortChildren<Troop>(CompareDefenseUnits);
                    i = 0;
                    Troop troop;
                    DamageType damageType;
                    while (candidates.Count != i && candidates.GetUnit(i).GroupId != group.GroupId)
                    {
                        troop = candidates.GetUnit(i);
                        for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.None; damageTypeOrdinal++)
                        {
                            damageType = (DamageType)damageTypeOrdinal;
                            int strength = troop.GetStrength(damageType);
                            Array10<int> damageStrengths = strengths.DamageStrengths;
                            damageStrengths[(int)damageType] -= strength;
                            strengths.DamageStrengths = damageStrengths;
                        }

                        i += 1;
                    }

                    int start = i;
                    i = 0;
                    int last = Defenders.Count - 1;
                    for (; i <= last; i += 1)
                    {
                        troop = Defenders.GetUnit(i);
                        for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.None; damageTypeOrdinal++)
                        {
                            damageType = (DamageType)damageTypeOrdinal;
                            int strength = troop.GetStrength(damageType);
                            Array10<int> damageStrengths2 = strengths.DamageStrengths;
                            damageStrengths2[(int)damageType] -= strength;
                            strengths.DamageStrengths = damageStrengths2;
                        }
                    }

                    i = start;
                    while (true)
                    {
                        Array10<int> required = strengths.DamageStrengths;
                        if (EnoughStrength(required) || candidates.Count <= i || candidates.GetUnit(i).GroupId != group.GroupId)
                        {
                            break;
                        }

                        troop = candidates.GetUnit(i);
                        if (Defenders.IndexOfChild(troop) == -1 && UsefulUnit(troop, strengths: required))
                        {
                            _ = Defenders.AddUnit(troop);
                            for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.None; damageTypeOrdinal++)
                            {
                                damageType = (DamageType)damageTypeOrdinal;
                                int strength = troop.GetStrength(damageType);
                                Array10<int> damageStrengths3 = strengths.DamageStrengths;
                                damageStrengths3[(int)damageType] -= strength;
                                strengths.DamageStrengths = damageStrengths3;
                            }
                        }

                        i += 1;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        candidates?.Dispose();
        bool result;
        if (units is not null)
        {
            result = true;
            i = units.Count;
            while (result && i != 0)
            {
                i -= 1;
                Troop unit = units.GetUnit(i);
                if (Defenders.IndexOf(unit) != -1)
                {
                    result = false;
                }
            }
        }
        else
        {
            result = Defenders.Count == 0;
        }

        StrategicMap.Current.AIExecutor.UseProcessTime(30);
        return result;
    }

    private static int s_defenseSortGroupId;
    private static UnitUserList s_defenseSortUnits;

    private static int CompareDefenseUnits(Troop item1, Troop item2)
    {
        int number = item1.Group.RecruitmentPriority - item2.Group.RecruitmentPriority;
        if (number == 0)
        {
            number = item1.GroupId - item2.GroupId;
            if (number == 0 && item1.GroupId == s_defenseSortGroupId)
            {
                if (s_defenseSortUnits is not null)
                {
                    bool flag = s_defenseSortUnits.IndexOfChild(item1) != -1;
                    bool flag2 = s_defenseSortUnits.IndexOfChild(item2) != -1;
                    if (flag && !flag2)
                    {
                        number = 1;
                    }
                    else if (flag2 && !flag)
                    {
                        number = -1;
                    }
                }
                if (number == 0)
                {
                    number = item1.GetStrengthMax(AbilityRules.AllDamageTypes) - item2.GetStrengthMax(AbilityRules.AllDamageTypes);
                }
            }
        }
        return number;
    }
}
