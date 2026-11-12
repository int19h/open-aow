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
public partial class UnitList : EChangeNotifyNode, IChildList<Troop>
{
    // The troop in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a troop, so the cast never fails.
    public Troop this[int index] => (Troop)GetChild(index);

    // A unit list takes troops only. A list of one kind of troop narrows that through
    // AcceptsTroop, and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop troop && AcceptsTroop(troop);

    protected virtual bool AcceptsTroop(Troop troop) => true;

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    public UnitList(ECustomNode owner) : base(owner)
    { }

    // Unit.PAS:2294
    public override void MainReadWrite(EStorageStream stream)
    {
        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                this[I].Release();
            }
        }
    }

    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                this[I].Release();
            }
        }
    }

    // Unit.PAS:2177
    public override void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            this[I].SetOwner(null);
        }

        ClearChildren();
    }

    // Unit.PAS:2184
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // Unit.PAS:2162
    public override int RemoveChild(EngineObject child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            child.Release();
        }

        return result;
    }

    // Unit.PAS:2167
    public override int AddChild(EngineObject child)
    {
        int result;
        CheckChild(child);
        var unit = child;
        if (this.CanAddUnit((Troop)child))
        {
            unit.AddRef();
            result = base.AddChild(child);
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Unit.PAS:2362
    public virtual sbyte GetPlayer() => -1;

    // Unit.PAS:2366
    public virtual int GetStrength(DamageType damageType)
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            result += this[I].GetStrength(damageType);
        }

        return result;
    }

    // Unit.PAS:2373
    public virtual int GetStrengthMax(Set<DamageType> damageTypes)
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            result += this[I].GetStrengthMax(damageTypes);
        }

        return result;
    }

    // Unit.PAS:2356
    public virtual void GetLocation(ref sbyte hexX, ref sbyte hexY, ref sbyte hexLevel)
    {
        hexX = -1;
        hexY = -1;
        hexLevel = -1;
    }

    // Unit.PAS:2338
    public virtual void SortUnits()
    {
        BeginUpdate();
        PackChildren();
        SortChildren<Troop>(CompareUnits);
        EndUpdate();
    }

    // Unit.PAS:2349
    public virtual void SortOnMoves()
    {
        BeginUpdate();
        PackChildren();
        SortChildren<Troop>(CompareUnitMoves);
        EndUpdate();
    }

    // Unit.PAS:2188
    public virtual bool CanAddUnit(Troop troop) => troop.CanAddToList(this);

    // Unit.PAS:2192
    public virtual bool CanDeleteUnit(Troop troop) => true;

    // Unit.PAS:2196
    public virtual bool AddUnit(Troop troop)
    {
        troop.SetOwner(this);
        return troop.Owner == this;
    }

    // Unit.PAS:2208
    public virtual bool DeleteUnit(Troop troop)
    {
        bool result;
        if (CanDeleteUnit(troop))
        {
            result = true;
            troop.SetOwner(null);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:2201
    public virtual bool AddUnits(UnitList units)
    {
        for (int I = units.Count - 1; I >= 0; I--)
        {
            AddUnit(units.GetUnit(I));
        }

        return true;
    }

    // Unit.PAS:2217
    public int IndexOf(Troop troop) => IndexOfChild(troop);

    // Unit.PAS:2221
    public int IndexOfId(int id)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].UnitId == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Unit.PAS:2231
    public int IndexOfBehavior(GroupBehavior behavior)
    {
        AIGroup group;
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            group = this[result].Group;
            if (group != null && group.GroupClass.Behavior == behavior)
            {
                return result;
            }
        }

        return -1;
    }

    // Unit.PAS:2243
    public int IndexOfGroup(AIGroup group)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].Group == group)
            {
                return result;
            }
        }

        return -1;
    }

    // Unit.PAS:2253
    public int IndexOfGroupId(int id)
    {
        AIGroup group;
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            group = this[result].Group;
            if (group != null && group.GroupId == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Unit.PAS:2265
    public Troop FindId(int id)
    {
        Troop result;
        int I;
        I = Count;
        while (I != 0)
        {
            I -= 1;
            result = this[I];
            if (result.UnitId == id)
            {
                return result;
            }
        }

        return null;
    }

    // Unit.PAS:2277
    public Troop GetUnit(int index)
    {
        // { Original accepts negative indexes and reads the backing array directly. }
        if (index < Count)
        {
            return this[index];
        }

        return null;
    }

    // Unit.PAS:2287
    public void SetUnit(int index, Troop troop)
    {
        CheckChild(troop);
        if (index >= Count)
        {
            // Pads the slots up to index with empty ones, as the original adds nil slots.
            PlaceChild(index, child: null);
        }

        if (GetChild(index) != null)
        {
            this[index].Release();
        }

        PlaceChild(index, child: troop);
        if (UpdateCount == 0)
        {
            Changed();
        }
    }

    private static int CompareUnits(Troop item1, Troop item2)
    {
        int s1;
        int s2;
        var troop = item1;
        s1 = troop.GetUnitLevel();
        if (troop.GetTransporter())
        {
            s1 += 500;
        }

        if (item1 is Hero)
        {
            s1 += 50;
            if (item1 is Leader)
            {
                s1 += 100;
            }
        }

        var troop2 = item2;
        s2 = troop2.GetUnitLevel();
        if (troop2.GetTransporter())
        {
            s2 += 500;
        }

        if (item2 is Hero)
        {
            s2 += 50;
            if (item2 is Leader)
            {
                s2 += 100;
            }
        }

        int result = s2 - s1;
        if (result == 0)
        {
            if (item1.UnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                s1 = 1;
            }
            else
            {
                s1 = 0;
            }

            if (item2.UnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                s2 = 1;
            }
            else
            {
                s2 = 0;
            }

            result = s2 - s1;
        }

        if (result == 0)
        {
            result = item2.GetUnitGraphicsResourceIndex() - item1.GetUnitGraphicsResourceIndex();
            if (result == 0)
            {
                result = item2.UnitId - item1.UnitId;
            }
        }

        return result;
    }

    private static int CompareUnitMoves(Troop item1, Troop item2) => item2.GetMoves() - item1.GetMoves();

    internal static sbyte UnitsToIndependentRelation(UnitList units, sbyte defaultRelation)
    {
        sbyte result;
        bool sameRace;
        bool evil;
        bool good;
        bool neutral;
        int I;
        int race;
        Troop troop;
        Alignment Alignment;
        sbyte relation;
        relation = -1;
        sameRace = true;
        I = units.Count;
        while (sameRace && I != 0)
        {
            I -= 1;
            race = units[I].GetRace();
            if (race != -1 && race != relation)
            {
                if (relation == -1)
                {
                    relation = (sbyte)race;
                }
                else
                {
                    sameRace = false;
                }
            }
        }

        if (sameRace && relation == -1)
        {
            relation = defaultRelation;
        }

        if (relation == -1)
        {
            sameRace = false;
        }

        I = units.Count;
        while (sameRace && I != 0)
        {
            I -= 1;
            bool condition = relation == units[I].GetRace();
            if (!condition)
            {
                var alignment = units[I].GetAlignment();
                condition = AoWHSSet.Current.RaceResources.GetRaceResource(relation).Alignments.Contains(alignment);
            }

            sameRace = condition;
        }

        if (!sameRace)
        {
            evil = true;
            good = true;
            neutral = true;
            I = 0;
            int last = units.Count - 1;
            for (; I <= last; I += 1)
            {
                troop = units[I];
                Alignment = troop.GetAlignment();
                evil = evil && AlignmentTables.EvilAlignments.Contains(Alignment) && (troop.GetRace() == -1 || Set<Alignment>.Of(Game.Units.Alignment.Evil, Game.Units.Alignment.PureEvil).Contains(Alignment));
                good = good && AlignmentTables.GoodAlignments.Contains(Alignment) && (troop.GetRace() == -1 || Set<Alignment>.Of(Game.Units.Alignment.PureGood, Game.Units.Alignment.Good).Contains(Alignment));
                neutral = neutral && AlignmentTables.NeutralAlignments.Contains(Alignment);
            }

            if (good && !evil)
            {
                result = -2;
            }
            else
            {
                if (evil && !good)
                {
                    result = -4;
                }
                else
                {
                    result = -3;
                }
            }
        }
        else
        {
            result = relation;
        }

        return result;
    }

    // Engine.pas:680

    // Engine.pas:680

}
