#nullable disable
using System.Collections;
using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

/// <summary>
/// The production locations of a production location list, in the list's child slots and their
/// stored order. The list holds references and does not dispose the locations. This collection
/// reads them typed and sorts them. A default value has no list and throws on use, as a missing
/// list does.
/// </summary>
public readonly struct AIGroupProductionLocationCollection : IReadOnlyList<AIGroupProductionLocation>
{
    private readonly AIGroupProductionLocationList _list;

    internal AIGroupProductionLocationCollection(AIGroupProductionLocationList list) => _list = list;

    /// <summary>The list that holds the locations.</summary>
    public AIGroupProductionLocationList Holder => _list;

    public int Count => _list.GetCount();

    // The location in slot index, read as the original reads the list, without a range check.
    // The list admits only production locations, so the cast never fails.
    public AIGroupProductionLocation this[int index] => (AIGroupProductionLocation)_list.GetChild(index);

    public IEnumerator<AIGroupProductionLocation> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // AIGroup.pas:907
    public AIGroupProductionLocation GetProductionLocation(int index)
    {
        Rtl.RtlUnit.Assert(index >= 0 && index < Count, message: "", file: "AIGroup.pas", line: 909);
        return this[index];
    }

    // AIGroup.pas:919
    public void SortOnMovePoints()
    {
        _list.SortChildren<AIGroupProductionLocation>(CompareMovePoints);
    }

    // AIGroup.pas:934
    public int IndexOfPC(ProductionControl control)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].ProductionControl == control)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:945
    public AIGroupProductionLocation FindPC(ProductionControl control)
    {
        AIGroupProductionLocation result;
        int index;
        index = IndexOfPC(control);
        if (index != -1)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    private static int CompareMovePoints(AIGroupProductionLocation item1, AIGroupProductionLocation item2)
    {
        return item2.MovePoints - item1.MovePoints;
    }
}
