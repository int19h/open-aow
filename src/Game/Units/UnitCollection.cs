using System.Collections;

using Aow.Engine.Nodes;

using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

/// <summary>
/// The troops of an army or a unit list, in the holder's child slots and their stored order. The
/// holder owns the slots: it checks each troop that enters, notifies its changes, routes messages
/// to its troops, frees them and stores them as its children. This collection reads them typed
/// and sorts them. A default value has no holder and throws on use, as a missing list does.
/// </summary>
public readonly struct UnitCollection : IReadOnlyList<Troop>
{
    private readonly EChangeNotifyNode _holder;
    private readonly IUnitHolder _hooks;

    // An army and a unit list admit only troops (AcceptsChild), so every slot holds a troop or
    // is empty.
    internal UnitCollection(Army holder)
    {
        _holder = holder;
        _hooks = holder;
    }

    internal UnitCollection(UnitList holder)
    {
        _holder = holder;
        _hooks = holder;
    }

    /// <summary>The army or unit list that holds the troops.</summary>
    public EChangeNotifyNode Holder => _holder;

    // The holder's CanAddUnit, which an army and its kinds override.
    public bool CanAddUnit(Troop troop) => _hooks.CanAddUnit(troop);

    // The holder's AddUnit: the troop takes an army or a unit list as owner, a unit user list
    // holds a reference. The name is not Add, so a collection expression does not build this
    // view without a holder.
    public bool AddUnit(Troop troop) => _hooks.AddUnit(troop);

    public int Count => _holder.GetCount();

    // The troop in slot index, read as the original reads the list, without a range check. The
    // holder admits only troops, so the cast never fails.
    public Troop this[int index] => (Troop)_holder.GetChild(index);

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
        for (int I = Count - 1; I >= 0; I--)
        {
            result += this[I].GetStrength(damageType);
        }

        return result;
    }

    // Unit.PAS:2373
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            result += this[I].GetStrengthMax(damageTypes);
        }

        return result;
    }

    // Unit.PAS:2338
    public void Sort()
    {
        _holder.BeginUpdate();
        _holder.PackChildren();
        _holder.SortChildren<Troop>(CompareUnits);
        _holder.EndUpdate();
    }

    // Unit.PAS:2349
    public void SortOnMoves()
    {
        _holder.BeginUpdate();
        _holder.PackChildren();
        _holder.SortChildren<Troop>(CompareUnitMoves);
        _holder.EndUpdate();
    }

    // Unit.PAS:2217
    public int IndexOf(Troop troop) => _holder.IndexOfChild(troop);

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
}
