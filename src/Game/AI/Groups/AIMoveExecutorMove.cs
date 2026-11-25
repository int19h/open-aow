#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.AI.Groups;

// units, path, points, moves, source, destination
public sealed class AIMoveExecutorMove(CustomNode? owner) : AIMoveExecutorUnitList(owner)
{

    public MovePath? Path { get; private set; } = [];
    public int MovePoints { get; set; }
    public int Moves { get; set; }
    public MapCoordinate Source { get; set; }
    public MapCoordinate Destination { get; set; }

    // AIGroup.pas:4446
    public override void Dispose()
    {
        // { Original retains this empty exception frame when the path is already nil. }
        if (Path is null)
        {
            try
            {
            }
            catch
            {
                HostLog.CatchDialog(RtlHost.Logger, location: "AIMoveExecutorMove.Dispose", text: "Path already destroyed!?!");
                ShowMessage("Path already destroyed!?!");
            }
        }

        // { Original handler consumes the exception after showing the message. }
        Path?.Dispose();
        Path = null;
        base.Dispose();
    }

    // AIGroup.pas:4460
    public void AddUnits(AIMoveExecutorUnitList units, int selection)
    {
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                _ = AddChild(units.GetUnits(i));
            }

            selection >>>= 1;
        }
    }

    // AIGroup.pas:4471
    public void AddUnit(AIMoveExecutorUnit unitEntry) => AddChild(unitEntry);

    // AIGroup.pas:4476
    public void SetUnitStatus(AIMoveExecutorUnitStatus status)
    {
        foreach (AIMoveExecutorUnit unit in this.Reverse())
        {
            unit.Status = status;
        }
    }

    // AIGroup.pas:4484
    public bool RetrieveArmy(out ArmyHexSprite? army, out int selection)
    {
        army = GetEngine().StrategicMap.MapContainer.GetField(Source.X, Source.Y, Source.MapLevel).GetArmyHexSprite();
        if (army is not null)
        {
            selection = 0;
            foreach (AIMoveExecutorUnit units in this.Reverse())
            {
                int index = army.Army.FindChildIndex(units.Troop);
                selection = index != -1 ? (selection | (1 << index)).TruncateToByte() : throw new KeyNotFoundException("Unit missing!");
            }

            return true;
        }
        else
        {
            selection = 0;
            army = null;
            return false;
        }
    }
}
