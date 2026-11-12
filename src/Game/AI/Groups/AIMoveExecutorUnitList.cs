// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.AI.Groups;

// children
public partial class AIMoveExecutorUnitList : ENode, IChildList<AIMoveExecutorUnit>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIMoveExecutorUnit, so the cast never fails.
    public AIMoveExecutorUnit this[int index] => (AIMoveExecutorUnit)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIMoveExecutorUnit;

    int IChildSink<AIMoveExecutorUnit>.AddChild(AIMoveExecutorUnit child) => AddChild(child);

    public AIMoveExecutorUnitList(ECustomNode owner) : base(owner)
    { }

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
    public int IndexOfUnit(Troop troop)
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
    public byte ValidateSelection(byte selection)
    {
        int N;
        N = _children.Count;
        if (N > 8)
        {
            N = 8;
        }

        return (byte)(selection & AbilityRules.ArmySelectionMasks[N]);
    }

    // AIGroup.pas:4375
    public int SelectionCount(byte selection)
    {
        selection = ValidateSelection(selection);
        int result = 0;
        while (selection != 0)
        {
            if ((selection & 1) != 0)
            {
                result += 1;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // AIGroup.pas:4386
    public AIMoveExecutorUnit Transporter(byte selection)
    {
        AIMoveExecutorUnit result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                if (this[I].Troop.GetTransporter())
                {
                    result = this[I];
                    break;
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // AIGroup.pas:4403
    public void SetMoveResult(byte selection, byte moveResult)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                this[I].MoveResult = moveResult;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // AIGroup.pas:4415
    public void SetTransportStatus(byte selection, AIMoveExecutorTransportStatus status)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                this[I].TransportStatus = status;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // AIGroup.pas:4428
    public void SetStatus(byte selection, AIMoveExecutorUnitStatus status)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                this[I].Status = status;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // Engine.pas:631

}
