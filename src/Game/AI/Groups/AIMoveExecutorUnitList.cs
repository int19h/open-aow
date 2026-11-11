// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.AI.Groups;

// children
public partial class AIMoveExecutorUnitList : ENode
{
    public AIMoveExecutorUnitList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4328
    public AIMoveExecutorUnit GetUnits(int Index) => (AIMoveExecutorUnit)FChildren.List[Index];

    // AIGroup.pas:4333
    public int IndexOfUnitID(int ID)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecutorUnit)FChildren.List[Result]).FUnitID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4344
    public int IndexOfUnit(Troop AUnit)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecutorUnit)FChildren.List[Result]).FUnit == AUnit)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4355
    public int FindStatusUnit(AIMoveExecutorUnitStatus Status)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecutorUnit)FChildren.List[Result]).FStatus == Status)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4366
    public byte ValidateSelection(byte Selection)
    {
        int N;
        N = FChildren.Count;
        if (N > 8)
        {
            N = 8;
        }

        return (byte)(Selection & AbilityRules.ArmySelectionMasks[N]);
    }

    // AIGroup.pas:4375
    public int SelectionCount(byte Selection)
    {
        Selection = ValidateSelection(Selection);
        int Result = 0;
        while (Selection != 0)
        {
            if ((Selection & 1) != 0)
            {
                Result += 1;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // AIGroup.pas:4386
    public AIMoveExecutorUnit Transporter(byte Selection)
    {
        AIMoveExecutorUnit Result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                if (((AIMoveExecutorUnit)FChildren.List[I]).FUnit.GetTransporter())
                {
                    Result = (AIMoveExecutorUnit)FChildren.List[I];
                    break;
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // AIGroup.pas:4403
    public void SetMoveResult(byte Selection, byte MoveResult)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                ((AIMoveExecutorUnit)FChildren.List[I]).FMoveResult = MoveResult;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // AIGroup.pas:4415
    public void SetTransportStatus(byte Selection, AIMoveExecutorTransportStatus Status)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                ((AIMoveExecutorUnit)FChildren.List[I]).FTransportStatus = Status;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // AIGroup.pas:4428
    public void SetStatus(byte Selection, AIMoveExecutorUnitStatus Status)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                ((AIMoveExecutorUnit)FChildren.List[I]).FStatus = Status;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // Engine.pas:631

}
