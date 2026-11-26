#nullable disable warnings
using System.Collections;

using Aow.Engine.Nodes;

using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Rtl;

namespace Aow.Game.Units;

/// <summary>
/// The troops of an army, a unit list, a unit user list or an AI group, in the holder's child
/// slots and their stored order. The holder owns the slots. An army and a unit list own the troops
/// they add. A unit user list and an AI group hold references and release them. The holder checks
/// each troop that enters,
/// notifies its changes, routes messages to its troops and stores the slots. This collection reads
/// them typed and sorts them. A default value has no holder and throws on use, as a missing list
/// does.
/// </summary>
public readonly struct UnitCollection : IReadOnlyList<Troop>
{
    private readonly IUnitHolder _hooks;

    // An army, a unit list, a unit user list and an AI group admit only troops (AcceptsChild), so
    // every slot holds a troop or is empty.
    internal UnitCollection(Army holder)
    {
        Holder = holder;
        _hooks = holder;
    }

    internal UnitCollection(UnitList holder)
    {
        Holder = holder;
        _hooks = holder;
    }

    internal UnitCollection(AIGroup holder)
    {
        Holder = holder;
        _hooks = holder;
    }

    internal UnitCollection(UnitUserList holder)
    {
        Holder = holder;
        _hooks = holder;
    }

    /// <summary>The army, unit list, unit user list or AI group that holds the troops.</summary>
    public ObservableNode Holder { get; }

    // The holder's CanAddUnit, which an army and its kinds override.
    public bool CanAddUnit(Troop troop) => _hooks.CanAddUnit(troop);

    // The holder's AddUnit: the troop takes an army or a unit list as owner, and a unit user list
    // or an AI group holds a reference. The name is not Add, so a collection expression does not
    // build this view without a holder.
    public bool AddUnit(Troop troop) => _hooks.AddUnit(troop);

    public int Count => Holder.GetCount();

    // The troop in slot index, read as the original reads the list, without a range check. The
    // holder admits only troops, so the cast never fails.
    public Troop this[int index] => Holder.GetChild<Troop>(index);

    public IEnumerator<Troop> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Unit.PAS:2366
    public int GetStrength(DamageType damageType)
    {
        int result = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            result += this[i].GetStrength(damageType);
        }

        return result;
    }

    // Unit.PAS:2373
    public int GetStrengthMax(DamageTypes damageTypes)
    {
        int result = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            result += this[i].GetStrengthMax(damageTypes);
        }

        return result;
    }

    // Unit.PAS:2338
    public void Sort()
    {
        using (Holder.BeginUpdate())
        {
            Holder.PackChildren();
            Holder.SortChildren<Troop>(CompareUnits);
        }
    }

    // Unit.PAS:2349
    public void SortOnMoves()
    {
        using (Holder.BeginUpdate())
        {
            Holder.PackChildren();
            Holder.SortChildren<Troop>(CompareUnitMoves);
        }
    }

    // Unit.PAS:2217
    public int IndexOf(Troop? troop) => Holder.IndexOfChild(troop);

    // Unit.PAS:2221
    public int IndexOfId(int id) => this.FindLastIndex(item => item.UnitId == id);

    // Unit.PAS:2231
    public int IndexOfBehavior(GroupBehavior behavior)
        => this.FindLastIndex(behavior, match: static (sought, troop) =>
            troop.Group is AIGroup group && group.GroupClass.Behavior == sought);

    // Unit.PAS:2243
    public int IndexOfGroup(AIGroup group) => this.FindLastIndex(item => item.Group == group);

    // Unit.PAS:2253
    public int IndexOfGroupId(int id)
        => this.FindLastIndex(id, match: static (sought, troop) =>
            troop.Group is AIGroup group && group.GroupId == sought);

    // Unit.PAS:2265
    public Troop? FindId(int id) => this.FindLast(item => item.UnitId == id);

    // Unit.PAS:2277
    public Troop? GetUnit(int index)
        // { Original accepts negative indexes and reads the backing array directly. }
        => index < Count ? this[index] : null;

    private static int CompareUnits(Troop item1, Troop item2)
    {
        Troop troop = item1;
        int s1 = troop.UnitLevel;
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

        Troop troop2 = item2;
        int s2 = troop2.UnitLevel;
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
            s1 = item1.UnitStatus.Contains(UnitStatuses.Enchanted) ? 1 : 0;

            s2 = item2.UnitStatus.Contains(UnitStatuses.Enchanted) ? 1 : 0;

            result = s2 - s1;
        }

        if (result == 0)
        {
            result = item2.UnitGraphicsResourceIndex - item1.UnitGraphicsResourceIndex;
            if (result == 0)
            {
                return item2.UnitId - item1.UnitId;
            }
        }

        return result;
    }

    private static int CompareUnitMoves(Troop item1, Troop item2) => item2.Moves - item1.Moves;
}
