#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// inherited FChildren, UpdateCount
public class UnitList(CustomNode? owner) : ObservableNode(owner), IChildList<Troop>, IUnitHolder
{
    // The troop in slot index, without a range check.
    public Troop this[int index] => Units[index];

    // A unit list takes troops only. A list of one kind of troop narrows that through
    // AcceptsTroop, and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop troop && AcceptsTroop(troop);

    protected virtual bool AcceptsTroop(Troop troop) => true;

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    /// <summary>The troops of this list.</summary>
    public UnitCollection Units => new(this);

    // Unit.PAS:2294
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                _ = this[i].Release();
            }
        }
    }

    // Unit.PAS:2177
    public override void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            this[i].SetOwner(null);
        }

        ClearChildren();
    }

    // Unit.PAS:2184
    public override void DestroyAllChildren() => Clear();

    // Unit.PAS:2162
    public override int RemoveChild(EngineObject? child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            _ = child.Release();
        }

        return result;
    }

    // Unit.PAS:2167
    public override int AddChild(EngineObject? child)
    {
        // The check refuses a child that is not a troop before the troop is read.
        CheckChild(child);
        EngineObject? unit = child;
        if (CanAddUnit((Troop?)child))
        {
            _ = unit.AddRef();
            return base.AddChild(child);
        }
        else
        {
            return -1;
        }
    }

    // Unit.PAS:2362
    public virtual sbyte GetPlayer() => -1;

    // Unit.PAS:2366
    public int GetStrength(DamageType damageType) => Units.GetStrength(damageType);

    // Unit.PAS:2373
    public int GetStrengthMax(DamageTypes damageTypes) => Units.GetStrengthMax(damageTypes);

    // Unit.PAS:2356
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // Unit.PAS:2338
    public void SortUnits() => Units.Sort();

    // Unit.PAS:2349
    public void SortOnMoves() => Units.SortOnMoves();

    // Unit.PAS:2188
    public virtual bool CanAddUnit(Troop? troop) => troop.CanAddToList(Units);

    // Unit.PAS:2192
    public virtual bool CanDeleteUnit(Troop? troop) => true;

    // Unit.PAS:2196
    public virtual bool AddUnit(Troop? troop)
    {
        troop.SetOwner(this);
        return troop.Owner == this;
    }

    // Unit.PAS:2208
    public virtual bool DeleteUnit(Troop? troop)
    {
        if (CanDeleteUnit(troop))
        {
            troop.SetOwner(null);
        }
        else
        {
            return false;
        }

        return true;
    }

    // Unit.PAS:2201
    public virtual bool AddUnits(UnitCollection units)
    {
        for (int i = units.Count - 1; i >= 0; i--)
        {
            _ = AddUnit(units.GetUnit(i));
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
    public Troop? FindId(int id) => Units.FindId(id);

    // Unit.PAS:2277
    public Troop? GetUnit(int index) => Units.GetUnit(index);

    // Unit.PAS:2287
    public void SetUnit(int index, Troop troop)
    {
        CheckChild(troop);
        EngineObject? previous = index < Count ? GetChild(index) : null;
        if (index >= Count)
        {
            // Pads the slots up to index with empty ones, as the original adds nil slots.
            PlaceChild(index, child: null);
        }

        if (GetChild(index) is not null)
        {
            _ = this[index].Release();
        }

        PlaceChild(index, child: troop);
        if (UpdateCount == 0)
        {
            RaiseChange(ObservableChange.Replaced(index, item: troop, previous: previous));
        }
    }

    internal static sbyte UnitsToIndependentRelation(UnitUserList units, sbyte defaultRelation)
    {
        sbyte relation = -1;
        bool sameRace = true;
        int i = units.Count;
        while (sameRace && i != 0)
        {
            i -= 1;
            int race = units[i].GetRace();
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

        i = units.Count;
        while (sameRace && i != 0)
        {
            i -= 1;
            bool condition = relation == units[i].GetRace();
            if (!condition)
            {
                Alignment alignment = units[i].GetAlignment();
                condition = GamePack.Current.RaceResources.GetRaceResource(relation).Alignments.Contains(alignment.ToFlag());
            }

            sameRace = condition;
        }
        if (!sameRace)
        {
            bool evil = true;
            bool good = true;
            bool neutral = true;
            i = 0;
            int last = units.Count - 1;
            for (; i <= last; i += 1)
            {
                Troop troop = units[i];
                Alignment alignmentAlignment = troop.GetAlignment();
                evil = evil && AlignmentTables.EvilAlignments.Contains(alignmentAlignment.ToFlag()) && (troop.GetRace() == -1 || alignmentAlignment is Alignment.Evil or Alignment.PureEvil);
                good = good && AlignmentTables.GoodAlignments.Contains(alignmentAlignment.ToFlag()) && (troop.GetRace() == -1 || alignmentAlignment is Alignment.PureGood or Alignment.Good);
                neutral = neutral && AlignmentTables.NeutralAlignments.Contains(alignmentAlignment.ToFlag());
            }

            return good && !evil ? (sbyte)-2 : evil && !good ? (sbyte)-4 : (sbyte)-3;
        }
        else
        {
            return relation;
        }
    }
}
