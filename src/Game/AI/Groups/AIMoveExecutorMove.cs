// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// units, path, points, moves, source, destination
public partial class AIMoveExecutorMove : AIMoveExecutorUnitList
{
    // AIGroup.pas:4440
    public AIMoveExecutorMove(ECustomNode owner) : base(owner)
    {
        FPath = new MovePath();
    }

    public MovePath FPath;
    public int FMovePoints;
    public int FMoves;
    public sbyte FSourceX;
    public sbyte FSourceY;
    public sbyte FSourceL;
    public sbyte FDestinationX;
    public sbyte FDestinationY;
    public sbyte FDestinationL;

    // AIGroup.pas:4446
    public override void Dispose()
    {
        // { Original retains this empty exception frame when the path is already nil. }
        if (FPath == null)
        {
            try
            {
            }
            catch
            {
                ShowMessage("Path already destroyed!?!");
            }
        }

        // { Original handler consumes the exception after showing the message. }
        FPath?.Dispose();
        FPath = null;
        base.Dispose();
    }

    // AIGroup.pas:4460
    public void AddUnits(AIMoveExecutorUnitList Units, byte Selection)
    {
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                AddChild(Units.GetUnits(I));
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // AIGroup.pas:4471
    public void AddUnit(AIMoveExecutorUnit UnitEntry)
    {
        AddChild(UnitEntry);
    }

    // AIGroup.pas:4476
    public void SetUnitStatus(AIMoveExecutorUnitStatus Status)
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((AIMoveExecutorUnit)FChildren.List[I]).FStatus = Status;
        }
    }

    // AIGroup.pas:4484
    public bool RetrieveArmy(ref ArmyHexSprite Army, ref byte Selection)
    {
        bool Result;
        int Index;
        Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FSourceX, FSourceY, FSourceL)).GetArmyHS();
        if (Army != null)
        {
            Selection = 0;
            for (int I = Count - 1; I >= 0; I--)
            {
                var units = GetUnits(I);
                Index = Army.FArmy.FindChildIndex(units.FUnit);
                if (Index != -1)
                {
                    Selection = (byte)(Selection | (1 << Index));
                }
                else
                {
                    throw new KeyNotFoundException("Unit missing!");
                }
            }

            Result = true;
        }
        else
        {
            Selection = 0;
            Army = null;
            Result = false;
        }

        return Result;
    }

    // intf_P5_06.inc:347

}
