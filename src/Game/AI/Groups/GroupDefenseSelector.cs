// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// { TDamageStrengths is shared with TDefenseRequirements in intf_P1_01.inc. }
// defenders, selected defense location
public partial class GroupDefenseSelector : IDisposable
{
    // AIGroup.pas:1640
    public GroupDefenseSelector()
    {
        Defenders = new UnitUserList(null);
    }

    public UnitList Defenders { get; }

    public MapCoordinate DefenseLocation;

    // AIGroup.pas:1646
    public virtual void Dispose()
    {
        Defenders?.Dispose();
    }

    // AIGroup.pas:1652
    public bool GetDefenders(sbyte player, sbyte X, sbyte Y, sbyte L, UnitList units)
    {
        DefensePriority priority;
        int location;
        Player P;
        AIGroup group;
        bool result = false;
        location = Unsafe.BitCast<MapCoordinate, int>(HexGeometry.PackMapCoordinate(X, Y, L));
        priority = (DefensePriority)((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetDefensePriority();
        P = AoWHSMap.Current.Players.GetPlayers(player);
        for (int I = P.AIGroups.Count - 1; I >= 0; I--)
        {
            group = P.AIGroups.GetGroups(I);
            if (group.DefendLocation(location, priority))
            {
                units.AddUnits(group);
                result = true;
            }
        }

        return result;
    }

    // AIGroup.pas:1676
    public bool EnoughStrength(ref Int32Array10 strengths)
    {
        bool result = default;
        DamageType damageType;
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            damageType = (DamageType)damageTypeOrdinal;
            result = strengths[(int)damageType] <= 0;
            if (!result)
            {
                break;
            }
        }

        return result;
    }

    // AIGroup.pas:1687
    public bool UsefulUnit(Troop troop, ref Int32Array10 strengths)
    {
        bool result = default;
        DamageType damageType;
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            damageType = (DamageType)damageTypeOrdinal;
            result = strengths[(int)damageType] > 0 && troop.GetStrength(damageType) > 0;
            if (result)
            {
                break;
            }
        }

        return result;
    }

    // AIGroup.pas:1699
    public void UpdateDefenseLocation(AoWMapField mapField)
    {
        if (Unsafe.As<MapCoordinate, int>(ref DefenseLocation) == 16777215 || mapField.FindOwnedHexSprite(typeof(Structure)) != null)
        {
            var mapCoordinate = mapField.GetMapCoordinate();
            DefenseLocation.Packed = mapCoordinate;
        }
    }

    // AIGroup.pas:1728
    public bool Execute(AIGroup group, sbyte X, sbyte Y, sbyte L, UnitList units)
    {
        bool result = default;
        UnitUserList candidates = default;
        int start = default;
        int I = default;
        MapLevel level = default;
        Troop troop = default;
        AoWMapField mapField = default;
        DamageType damageType = default;
        DefenseRequirements strengths = default;
        HexNumberWalkData walk = default;
        Defenders.Clear();
        level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        DefenseLocation.Packed = 16777215;
        candidates = new UnitUserList(null);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= 1 && Defenders.Count != group.Count)
        {
            mapField = (AoWMapField)level.GetField(walk.X, walk.Y);
            if (mapField.GetPlayer() == group.Player)
            {
                candidates.Clear();
                mapField.GetDefenseRequirements(group.Player, ref strengths);
                if (!EnoughStrength(ref strengths.DamageStrengths))
                {
                    UpdateDefenseLocation(mapField);
                    GetDefenders(group.Player, (sbyte)walk.X, (sbyte)walk.Y, L, candidates);
                    s_defenseSortGroupId = group.GroupId;
                    s_defenseSortUnits = units;
                    candidates.SortChildren(CompareDefenseUnits);
                    I = 0;
                    while (candidates.Count != I && candidates.GetUnit(I).GroupId != group.GroupId)
                    {
                        troop = candidates.GetUnit(I);
                        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                        {
                            damageType = (DamageType)damageTypeOrdinal;
                            var strength = troop.GetStrength(damageType);
                            strengths.DamageStrengths[(int)damageType] -= strength;
                        }

                        I += 1;
                    }

                    start = I;
                    I = 0;
                    int last = Defenders.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        troop = Defenders.GetUnit(I);
                        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                        {
                            damageType = (DamageType)damageTypeOrdinal;
                            var strength = troop.GetStrength(damageType);
                            strengths.DamageStrengths[(int)damageType] -= strength;
                        }
                    }

                    I = start;
                    while (!EnoughStrength(ref strengths.DamageStrengths) && candidates.Count > I && candidates.GetUnit(I).GroupId == group.GroupId)
                    {
                        troop = candidates.GetUnit(I);
                        if (Defenders.IndexOfChild(troop) == -1 && UsefulUnit(troop, ref strengths.DamageStrengths))
                        {
                            Defenders.AddUnit(troop);
                            for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                            {
                                damageType = (DamageType)damageTypeOrdinal;
                                var strength = troop.GetStrength(damageType);
                                strengths.DamageStrengths[(int)damageType] -= strength;
                            }
                        }

                        I += 1;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        candidates?.Dispose();
        if (units != null)
        {
            result = true;
            I = units.Count;
            while (result && I != 0)
            {
                I -= 1;
                var unit = units.GetUnit(I);
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

        AoWHSMap.Current.AIExecutor.UseProcessTime(30);
        return result;
    }

    private static int s_defenseSortGroupId;
    private static UnitList s_defenseSortUnits;

    private static int CompareDefenseUnits(EngineObject item1, EngineObject item2)
    {
        int number = ((Troop)item1).Group.RecruitmentPriority - ((Troop)item2).Group.RecruitmentPriority;
        if (number == 0)
        {
            number = ((Troop)item1).GroupId - ((Troop)item2).GroupId;
            if (number == 0 && ((Troop)item1).GroupId == s_defenseSortGroupId)
            {
                if (s_defenseSortUnits != null)
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
                    number = ((Troop)item1).GetStrengthMax(AbilityRules.s_allDamageTypes) - ((Troop)item2).GetStrengthMax(AbilityRules.s_allDamageTypes);
                }
            }
        }
        return number;
    }

    // intf_P5_07.inc:320

    // intf_P5_07.inc:320

}
