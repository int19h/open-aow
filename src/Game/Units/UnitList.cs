// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited FChildren, UpdateCount
public partial class UnitList : EChangeNotifyNode, IChildList<Troop>, IUnitHolder
{
    // The troop in slot index, without a range check.
    public Troop this[int index] => Units[index];

    // A unit list takes troops only. A list of one kind of troop narrows that through
    // AcceptsTroop, and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop troop && AcceptsTroop(troop);

    protected virtual bool AcceptsTroop(Troop troop) => true;

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    public UnitList(ECustomNode owner) : base(owner)
    { }

    /// <summary>The troops of this list.</summary>
    public UnitCollection Units => new(this);

    // Unit.PAS:2294
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
    public int GetStrength(DamageType damageType) => Units.GetStrength(damageType);

    // Unit.PAS:2373
    public int GetStrengthMax(Set<DamageType> damageTypes) => Units.GetStrengthMax(damageTypes);

    // Unit.PAS:2356
    public virtual void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel)
    {
        hexX = -1;
        hexY = -1;
        hexLevel = -1;
    }

    // Unit.PAS:2338
    public void SortUnits() => Units.Sort();

    // Unit.PAS:2349
    public void SortOnMoves() => Units.SortOnMoves();

    // Unit.PAS:2188
    public virtual bool CanAddUnit(Troop troop) => troop.CanAddToList(Units);

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
    public virtual bool AddUnits(UnitCollection units)
    {
        for (int I = units.Count - 1; I >= 0; I--)
        {
            AddUnit(units.GetUnit(I));
        }

        return true;
    }

    // Unit.PAS:2217
    public int IndexOf(Troop troop) => Units.IndexOf(troop);

    // Unit.PAS:2221
    public int IndexOfId(int id) => Units.IndexOfId(id);

    // Unit.PAS:2231
    public int IndexOfBehavior(GroupBehavior behavior) => Units.IndexOfBehavior(behavior);

    // Unit.PAS:2243
    public int IndexOfGroup(AIGroup group) => Units.IndexOfGroup(group);

    // Unit.PAS:2253
    public int IndexOfGroupId(int id) => Units.IndexOfGroupId(id);

    // Unit.PAS:2265
    public Troop FindId(int id) => Units.FindId(id);

    // Unit.PAS:2277
    public Troop GetUnit(int index) => Units.GetUnit(index);

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
                condition = GamePack.Current.RaceResources.GetRaceResource(relation).Alignments.Contains(alignment);
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

}
