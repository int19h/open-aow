// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Ai.Groups;

// children
public partial class AIMoveExecuterUnitList : ENode
{
    public AIMoveExecuterUnitList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4328
    public AIMoveExecuterUnit GetUnits(int Index) => (AIMoveExecuterUnit)FChildren.List[Index];

    // AIGroup.pas:4333
    public int IndexOfUnitID(int ID)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecuterUnit)FChildren.List[Result]).FUnitID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4344
    public int IndexOfUnit(AbstractUnit AUnit)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecuterUnit)FChildren.List[Result]).FUnit == AUnit)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4355
    public int FindStatusUnit(AIMoveExecuterUnitStatus Status)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecuterUnit)FChildren.List[Result]).FStatus == Status)
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
    public AIMoveExecuterUnit Transporter(byte Selection)
    {
        AIMoveExecuterUnit Result = null;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                if (((AIMoveExecuterUnit)FChildren.List[I]).FUnit.GetTransporter())
                {
                    Result = (AIMoveExecuterUnit)FChildren.List[I];
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
                ((AIMoveExecuterUnit)FChildren.List[I]).FMoveResult = MoveResult;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // AIGroup.pas:4415
    public void SetTransportStatus(byte Selection, AIMoveExecuterTransportStatus Status)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                ((AIMoveExecuterUnit)FChildren.List[I]).FTransportStatus = Status;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // AIGroup.pas:4428
    public void SetStatus(byte Selection, AIMoveExecuterUnitStatus Status)
    {
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                ((AIMoveExecuterUnit)FChildren.List[I]).FStatus = Status;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // Engine.pas:631

}
