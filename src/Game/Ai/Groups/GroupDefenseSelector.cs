// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// { TDamageStrengths is shared with TDefenseRequirements in intf_P1_01.inc. }
// defenders, selected defense location
public partial class GroupDefenseSelector : IDisposable
{
    // AIGroup.pas:1640
    public GroupDefenseSelector()
    {
        FDefenders = new UnitUserList(null);
    }

    public UnitList FDefenders;
    public XYL FDefenseLocation;

    // AIGroup.pas:1646
    public virtual void Dispose()
    {
        FDefenders?.Dispose();
    }

    // AIGroup.pas:1652
    public bool GetDefenders(sbyte Player, sbyte X, sbyte Y, sbyte L, UnitList Units)
    {
        DefensePriority Priority;
        int Location;
        Player P;
        AIGroup Group;
        bool Result = false;
        Location = Unsafe.BitCast<XYL, int>(HexGeometry.PackXYL(X, Y, L));
        Priority = (DefensePriority)((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetDefensePriority();
        P = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        for (int I = P.FAIGroups.Count - 1; I >= 0; I--)
        {
            Group = P.FAIGroups.GetGroups(I);
            if (Group.DefendLocation(Location, Priority))
            {
                Units.AddUnits(Group);
                Result = true;
            }
        }

        return Result;
    }

    // AIGroup.pas:1676
    public bool EnoughStrength(ref FixedArray_int_10 Strengths)
    {
        bool Result = default;
        DamageType DamageType;
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            DamageType = (DamageType)damageTypeOrdinal;
            Result = Strengths[(int)DamageType] <= 0;
            if (!Result)
            {
                break;
            }
        }

        return Result;
    }

    // AIGroup.pas:1687
    public bool UsefullUnit(AbstractUnit AUnit, ref FixedArray_int_10 Strengths)
    {
        bool Result = default;
        DamageType DamageType;
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            DamageType = (DamageType)damageTypeOrdinal;
            Result = Strengths[(int)DamageType] > 0 && AUnit.GetStrength(DamageType) > 0;
            if (Result)
            {
                break;
            }
        }

        return Result;
    }

    // AIGroup.pas:1699
    public void UpdateDefenseLocation(AoWMapField MapF)
    {
        if (Unsafe.As<XYL, int>(ref FDefenseLocation) == 16777215 || MapF.FindOwnedHS(typeof(Structure)) != null)
        {
            var xyl = MapF.GetXYL();
            FDefenseLocation.Packed = xyl;
        }
    }

    // AIGroup.pas:1728
    public bool Execute(AIGroup Group, sbyte X, sbyte Y, sbyte L, UnitList Units)
    {
        bool Result = default;
        UnitUserList Candidates = default;
        int Start = default;
        int I = default;
        MapLevel Level = default;
        AbstractUnit AUnit = default;
        AoWMapField MapF = default;
        DamageType DamageType = default;
        DefenseRequirements Strengths = default;
        HNWalkData Walk = default;
        FDefenders.Clear();
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        FDefenseLocation.Packed = 16777215;
        Candidates = new UnitUserList(null);
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= 1 && FDefenders.Count != Group.Count)
        {
            MapF = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
            if (MapF.GetPlayer() == Group.FPlayer)
            {
                Candidates.Clear();
                MapF.GetDefenseRequirements(Group.FPlayer, ref Strengths);
                if (!EnoughStrength(ref Strengths.DamageStrengths))
                {
                    UpdateDefenseLocation(MapF);
                    GetDefenders(Group.FPlayer, (sbyte)Walk.X, (sbyte)Walk.Y, L, Candidates);
                    DefenseSortGroupID = Group.FGroupID;
                    DefenseSortUnits = Units;
                    Candidates.FChildren.Sort(CompareDefenseUnits);
                    I = 0;
                    while (Candidates.Count != I && Candidates.GetUnit(I).FGroupID != Group.FGroupID)
                    {
                        AUnit = Candidates.GetUnit(I);
                        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                        {
                            DamageType = (DamageType)damageTypeOrdinal;
                            var strength = AUnit.GetStrength(DamageType);
                            Strengths.DamageStrengths[(int)DamageType] -= strength;
                        }

                        I += 1;
                    }

                    Start = I;
                    I = 0;
                    int last = FDefenders.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        AUnit = FDefenders.GetUnit(I);
                        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                        {
                            DamageType = (DamageType)damageTypeOrdinal;
                            var strength = AUnit.GetStrength(DamageType);
                            Strengths.DamageStrengths[(int)DamageType] -= strength;
                        }
                    }

                    I = Start;
                    while (!EnoughStrength(ref Strengths.DamageStrengths) && Candidates.Count > I && Candidates.GetUnit(I).FGroupID == Group.FGroupID)
                    {
                        AUnit = Candidates.GetUnit(I);
                        if (FDefenders.FChildren.IndexOf(AUnit) == -1 && UsefullUnit(AUnit, ref Strengths.DamageStrengths))
                        {
                            FDefenders.AddUnit(AUnit);
                            for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                            {
                                DamageType = (DamageType)damageTypeOrdinal;
                                var strength = AUnit.GetStrength(DamageType);
                                Strengths.DamageStrengths[(int)DamageType] -= strength;
                            }
                        }

                        I += 1;
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }

        Candidates?.Dispose();
        if (Units != null)
        {
            Result = true;
            I = Units.Count;
            while (Result && I != 0)
            {
                I -= 1;
                var unit = Units.GetUnit(I);
                if (FDefenders.IndexOf(unit) != -1)
                {
                    Result = false;
                }
            }
        }
        else
        {
            Result = FDefenders.Count == 0;
        }

        AoWHSMap.Current.FAIExecuter.UseProcessTime(30);
        return Result;
    }

    private static int DefenseSortGroupID;
    private static UnitList DefenseSortUnits;

    private static int CompareDefenseUnits(object Item1, object Item2)
    {
        int num = ((AbstractUnit)Item1).FGroup.FRecruitmentPriority - ((AbstractUnit)Item2).FGroup.FRecruitmentPriority;
        if (num == 0)
        {
            num = ((AbstractUnit)Item1).FGroupID - ((AbstractUnit)Item2).FGroupID;
            if (num == 0 && ((AbstractUnit)Item1).FGroupID == DefenseSortGroupID)
            {
                if (DefenseSortUnits != null)
                {
                    bool flag = ((DelphiList)DefenseSortUnits.FChildren).IndexOf(Item1) != -1;
                    bool flag2 = ((DelphiList)DefenseSortUnits.FChildren).IndexOf(Item2) != -1;
                    if (flag && !flag2)
                    {
                        num = 1;
                    }
                    else if (flag2 && !flag)
                    {
                        num = -1;
                    }
                }
                if (num == 0)
                {
                    num = ((AbstractUnit)Item1).GetStrengthMax(AbilityRules.dtAll) - ((AbstractUnit)Item2).GetStrengthMax(AbilityRules.dtAll);
                }
            }
        }
        return num;
    }

    // intf_P5_07.inc:320

    // intf_P5_07.inc:320

}
