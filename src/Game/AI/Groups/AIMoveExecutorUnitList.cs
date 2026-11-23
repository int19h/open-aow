#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.AI.Groups;

// children
public class AIMoveExecutorUnitList(CustomNode? owner) : OwningNode(owner), IChildList<AIMoveExecutorUnit>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIMoveExecutorUnit.
    public AIMoveExecutorUnit this[int index] => GetChild<AIMoveExecutorUnit>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIMoveExecutorUnit;

    int IChildSink<AIMoveExecutorUnit>.AddChild(AIMoveExecutorUnit child) => AddChild(child);

    // AIGroup.pas:4328
    public AIMoveExecutorUnit GetUnits(int index) => this[index];

    // AIGroup.pas:4333
    public int IndexOfUnitId(int id)
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

    // AIGroup.pas:4344
    public int IndexOfUnit(Troop? troop)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].Troop == troop)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4355
    public int FindStatusUnit(AIMoveExecutorUnitStatus status)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].Status == status)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4366
    public int ValidateSelection(int selection)
    {
        int n = Count;
        if (n > 8)
        {
            n = 8;
        }

        return selection & AbilityRules.ArmySelectionMasks[n];
    }

    // AIGroup.pas:4375
    public int SelectionCount(int selection)
    {
        selection = ValidateSelection(selection);
        int result = 0;
        while (selection != 0)
        {
            if ((selection & 1) != 0)
            {
                result += 1;
            }

            selection >>>= 1;
        }

        return result;
    }

    // AIGroup.pas:4386
    public AIMoveExecutorUnit? Transporter(int selection)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                if (this[i].Troop.GetTransporter())
                {
                    return this[i];
                }
            }

            selection >>>= 1;
        }

        return null;
    }

    // AIGroup.pas:4403
    public void SetMoveResult(int selection, int moveResult)
    {
        foreach (AIMoveExecutorUnit unit in this)
        {
            if ((selection & 1) != 0)
            {
                unit.MoveResult = moveResult;
            }

            selection >>>= 1;
        }
    }

    // AIGroup.pas:4415
    public void SetTransportStatus(int selection, AIMoveExecutorTransportStatus status)
    {
        foreach (AIMoveExecutorUnit unit in this)
        {
            if ((selection & 1) != 0)
            {
                unit.TransportStatus = status;
            }

            selection >>>= 1;
        }
    }

    // AIGroup.pas:4428
    public void SetStatus(int selection, AIMoveExecutorUnitStatus status)
    {
        foreach (AIMoveExecutorUnit unit in this)
        {
            if ((selection & 1) != 0)
            {
                unit.Status = status;
            }

            selection >>>= 1;
        }
    }
}
