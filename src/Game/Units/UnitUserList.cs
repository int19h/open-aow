// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// The original's TUnitUserList derives from TUnitList. The port's list holds its troops as
// references in its node children (it routes messages to them and notifies their changes) and
// reads them through Units. AddUnit does not set the owner. A troop's owner stays where the
// caller left it, including this list when the caller sets that owner. The members below that
// carry a Unit.PAS line are TUnitList's part, with the reference writes TUnitUserList overrides.
public partial class UnitUserList : ObservableNode, IChildList<Troop>, IUnitHolder
{
    // The troop in slot index, without a range check.
    public Troop this[int index] => Units[index];

    // A unit user list takes troops only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop;

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    public UnitUserList(CustomNode owner) : base(owner)
    { }

    /// <summary>The troops of this list.</summary>
    public UnitCollection Units => new(this);

    // Unit.PAS:2294. The load releases the reference each loaded child took when it was created.
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int index = 0, last = Units.Count - 1; index <= last; index++)
            {
                Units[index].Release();
            }
        }
    }

    // Unit.PAS:2380
    public override void Clear()
    {
        for (int index = Count - 1; index >= 0; index--)
        {
            this[index].Release();
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
        CheckChild(child);
        if (CanAddUnit((Troop)child))
        {
            child.AddRef();
            return base.AddChild(child);
        }

        return -1;
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

    // Unit.PAS:2386
    public virtual bool AddUnit(Troop troop)
    {
        if (CanAddUnit(troop))
        {
            AddChild(troop);
            return true;
        }

        return false;
    }

    // Unit.PAS:2396
    public virtual bool DeleteUnit(Troop troop)
    {
        if (CanDeleteUnit(troop))
        {
            RemoveChild(troop);
            return true;
        }

        return false;
    }

    // Unit.PAS:2201
    public virtual bool AddUnits(UnitCollection units)
    {
        for (int index = units.Count - 1; index >= 0; index--)
        {
            AddUnit(units.GetUnit(index));
        }

        return true;
    }

    // Unit.PAS:2406
    public void AddUnits(UnitCollection units, byte selection)
    {
        for (int index = 0, last = units.Count - 1; index <= last; index++)
        {
            if (((1 << index) & selection) != 0)
            {
                AddUnit(units.GetUnit(index));
            }
        }
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
        EngineObject previous = index < Count ? GetChild(index) : null;
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
            RaiseChange(ObservableChange.Replaced(index, item: troop, previous: previous));
        }
    }
}
