// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited FChildren, UpdateCount
public partial class UnitList : EChangeNotifyNode
{

    public UnitList(ECustomNode owner) : base(owner)
    { }

    // Unit.PAS:2294
    public override void MainReadWrite(EStorageStream Stream)
    {
        base.MainReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                ((EngineObject)FChildren.List[I]).Release();
            }
        }
    }

    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                ((EngineObject)FChildren.List[I]).Release();
            }
        }
    }

    // Unit.PAS:2177
    public override void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((EngineObject)FChildren.List[I]).SetOwner(null);
        }

        FChildren.Clear();
    }

    // Unit.PAS:2184
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // Unit.PAS:2162
    public override int RemoveChild(EngineObject Child)
    {
        int Result = base.RemoveChild(Child);
        if (Result != -1)
        {
            Child.Release();
        }

        return Result;
    }

    // Unit.PAS:2167
    public override int AddChild(EngineObject Child)
    {
        int Result;
        var unit = Child;
        if (this.CanAddUnit((Troop)Child))
        {
            unit.AddRef();
            Result = base.AddChild(Child);
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Unit.PAS:2362
    public virtual sbyte GetPlayer() => -1;

    // Unit.PAS:2366
    public virtual int GetStrength(DamageType DamageType)
    {
        int Result = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            Result += ((Troop)FChildren.List[I]).GetStrength(DamageType);
        }

        return Result;
    }

    // Unit.PAS:2373
    public virtual int GetStrengthMax(Set<DamageType> DamageTypes)
    {
        int Result = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            Result += ((Troop)FChildren.List[I]).GetStrengthMax(DamageTypes);
        }

        return Result;
    }

    // Unit.PAS:2356
    public virtual void GetLocation(ref sbyte Xhx, ref sbyte Yhx, ref sbyte Lhx)
    {
        Xhx = -1;
        Yhx = -1;
        Lhx = -1;
    }

    // Unit.PAS:2338
    public virtual void SortUnits()
    {
        BeginUpdate();
        FChildren.Pack();
        FChildren.Sort(P2CompareUnits);
        EndUpdate();
    }

    // Unit.PAS:2349
    public virtual void SortOnMoves()
    {
        BeginUpdate();
        FChildren.Pack();
        FChildren.Sort(P2CompareUnitMoves);
        EndUpdate();
    }

    // Unit.PAS:2188
    public virtual bool CanAddUnit(Troop AUnit) => AUnit.CanAddToList(this);

    // Unit.PAS:2192
    public virtual bool CanDeleteUnit(Troop AUnit) => true;

    // Unit.PAS:2196
    public virtual bool AddUnit(Troop AUnit)
    {
        AUnit.SetOwner(this);
        return AUnit.Owner == this;
    }

    // Unit.PAS:2208
    public virtual bool DeleteUnit(Troop AUnit)
    {
        bool Result;
        if (CanDeleteUnit(AUnit))
        {
            Result = true;
            AUnit.SetOwner(null);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:2201
    public virtual bool AddUnits(UnitList Units)
    {
        for (int I = Units.Count - 1; I >= 0; I--)
        {
            AddUnit(Units.GetUnit(I));
        }

        return true;
    }

    // Unit.PAS:2217
    public int IndexOf(Troop AUnit) => FChildren.IndexOf(AUnit);

    // Unit.PAS:2221
    public int IndexOfID(int ID)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((Troop)FChildren.List[Result]).FUnitID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Unit.PAS:2231
    public int IndexOfBehavior(GroupBehavior Behavior)
    {
        AIGroup Group;
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            Group = ((Troop)FChildren.List[Result]).FGroup;
            if (Group != null && Group.GroupClass.Behavior == Behavior)
            {
                return Result;
            }
        }

        return -1;
    }

    // Unit.PAS:2243
    public int IndexOfGroup(AIGroup Group)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((Troop)FChildren.List[Result]).FGroup == Group)
            {
                return Result;
            }
        }

        return -1;
    }

    // Unit.PAS:2253
    public int IndexOfGroupID(int ID)
    {
        AIGroup Group;
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            Group = ((Troop)FChildren.List[Result]).FGroup;
            if (Group != null && Group.FGroupID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Unit.PAS:2265
    public Troop FindID(int ID)
    {
        Troop Result;
        int I;
        I = Count;
        while (I != 0)
        {
            I -= 1;
            Result = (Troop)FChildren.List[I];
            if (Result.FUnitID == ID)
            {
                return Result;
            }
        }

        return null;
    }

    // Unit.PAS:2277
    public Troop GetUnit(int Index)
    {
        // { Original accepts negative indexes and reads the backing array directly. }
        if (Index < Count)
        {
            return (Troop)FChildren.List[Index];
        }

        return null;
    }

    // Unit.PAS:2287
    public void SetUnit(int Index, Troop AUnit)
    {
        while (Index >= Count)
        {
            FChildren.Add(null);
        }

        if (FChildren.List[Index] != null)
        {
            ((EngineObject)FChildren.List[Index]).Release();
        }

        FChildren.List[Index] = AUnit;
        if (UpdateCount == 0)
        {
            Changed();
        }
    }

    private static int P2CompareUnits(object Item1, object Item2)
    {
        int S1;
        int S2;
        var unit = (Troop)Item1;
        S1 = unit.GetUnitLevel();
        if (unit.GetTransporter())
        {
            S1 += 500;
        }

        if ((Troop)Item1 is Hero)
        {
            S1 += 50;
            if ((Troop)Item1 is Leader)
            {
                S1 += 100;
            }
        }

        var unit2 = (Troop)Item2;
        S2 = unit2.GetUnitLevel();
        if (unit2.GetTransporter())
        {
            S2 += 500;
        }

        if ((Troop)Item2 is Hero)
        {
            S2 += 50;
            if ((Troop)Item2 is Leader)
            {
                S2 += 100;
            }
        }

        int Result = S2 - S1;
        if (Result == 0)
        {
            if (((Troop)Item1).FUnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                S1 = 1;
            }
            else
            {
                S1 = 0;
            }

            if (((Troop)Item2).FUnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                S2 = 1;
            }
            else
            {
                S2 = 0;
            }

            Result = S2 - S1;
        }

        if (Result == 0)
        {
            Result = ((Troop)Item2).GetUnitGFXResourceIndex() - ((Troop)Item1).GetUnitGFXResourceIndex();
            if (Result == 0)
            {
                Result = ((Troop)Item2).FUnitID - ((Troop)Item1).FUnitID;
            }
        }

        return Result;
    }

    private static int P2CompareUnitMoves(object Item1, object Item2) => ((Troop)Item2).GetMoves() - ((Troop)Item1).GetMoves();

    internal static sbyte UnitsToIndependentRelation(UnitList Units, sbyte DefaultRelation)
    {
        sbyte Result;
        bool SameRace;
        bool Evil;
        bool Good;
        bool Neutral;
        int I;
        int Race;
        Troop Unit_;
        Alignment Alignment;
        sbyte Relation;
        Relation = -1;
        SameRace = true;
        I = Units.FChildren.Count;
        while (SameRace && I != 0)
        {
            I -= 1;
            Race = ((Troop)Units.FChildren.List[I]).GetRace();
            if (Race != -1 && Race != Relation)
            {
                if (Relation == -1)
                {
                    Relation = (sbyte)Race;
                }
                else
                {
                    SameRace = false;
                }
            }
        }

        if (SameRace && Relation == -1)
        {
            Relation = DefaultRelation;
        }

        if (Relation == -1)
        {
            SameRace = false;
        }

        I = Units.FChildren.Count;
        while (SameRace && I != 0)
        {
            I -= 1;
            bool condition = Relation == ((Troop)Units.FChildren.List[I]).GetRace();
            if (!condition)
            {
                var alignment = ((Troop)Units.FChildren.List[I]).GetAlignment();
                condition = AoWHSSet.Current.FRaceResources.GetRaceResource(Relation).FAlignments.Contains(alignment);
            }

            SameRace = condition;
        }

        if (!SameRace)
        {
            Evil = true;
            Good = true;
            Neutral = true;
            I = 0;
            int last = Units.FChildren.Count - 1;
            for (; I <= last; I += 1)
            {
                Unit_ = (Troop)Units.FChildren.List[I];
                Alignment = Unit_.GetAlignment();
                Evil = Evil && AlignmentTables.alsEvil.Contains(Alignment) && (Unit_.GetRace() == -1 || Set<Alignment>.Of(Game.Units.Alignment.Evil, Game.Units.Alignment.PureEvil).Contains(Alignment));
                Good = Good && AlignmentTables.alsGood.Contains(Alignment) && (Unit_.GetRace() == -1 || Set<Alignment>.Of(Game.Units.Alignment.PureGood, Game.Units.Alignment.Good).Contains(Alignment));
                Neutral = Neutral && AlignmentTables.alsNeutral.Contains(Alignment);
            }

            if (Good && !Evil)
            {
                Result = -2;
            }
            else
            {
                if (Evil && !Good)
                {
                    Result = -4;
                }
                else
                {
                    Result = -3;
                }
            }
        }
        else
        {
            Result = Relation;
        }

        return Result;
    }

    // Engine.pas:680

    // Engine.pas:680

}
