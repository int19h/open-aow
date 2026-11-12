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
        Path = new MovePath();
    }

    public MovePath Path { get; private set; }
    public int MovePoints { get; set; }
    public int Moves { get; set; }
    public sbyte SourceX { get; set; }
    public sbyte SourceY { get; set; }
    public sbyte SourceL { get; set; }
    public sbyte DestinationX { get; set; }
    public sbyte DestinationY { get; set; }
    public sbyte DestinationL { get; set; }

    // AIGroup.pas:4446
    public override void Dispose()
    {
        // { Original retains this empty exception frame when the path is already nil. }
        if (Path == null)
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
        Path?.Dispose();
        Path = null;
        base.Dispose();
    }

    // AIGroup.pas:4460
    public void AddUnits(AIMoveExecutorUnitList units, byte selection)
    {
        for (int I = 0, last = units.Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                AddChild(units.GetUnits(I));
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // AIGroup.pas:4471
    public void AddUnit(AIMoveExecutorUnit unitEntry)
    {
        AddChild(unitEntry);
    }

    // AIGroup.pas:4476
    public void SetUnitStatus(AIMoveExecutorUnitStatus status)
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            this[I].Status = status;
        }
    }

    // AIGroup.pas:4484
    public bool RetrieveArmy(ref ArmyHexSprite army, ref byte selection)
    {
        bool result;
        int index;
        army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(SourceX, SourceY, SourceL)).GetArmyHexSprite();
        if (army != null)
        {
            selection = 0;
            for (int I = Count - 1; I >= 0; I--)
            {
                var units = GetUnits(I);
                index = army.Army.FindChildIndex(units.Troop);
                if (index != -1)
                {
                    selection = (byte)(selection | (1 << index));
                }
                else
                {
                    throw new KeyNotFoundException("Unit missing!");
                }
            }

            result = true;
        }
        else
        {
            selection = 0;
            army = null;
            result = false;
        }

        return result;
    }

    // intf_P5_06.inc:347

}
