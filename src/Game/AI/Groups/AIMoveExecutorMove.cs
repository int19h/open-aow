#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.AI.Groups;

// units, path, points, moves, source, destination
public class AIMoveExecutorMove(CustomNode owner) : AIMoveExecutorUnitList(owner)
{

    public MovePath Path { get; private set; } = new MovePath();
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
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                _ = AddChild(units.GetUnits(i));
            }

            selection = (byte)(selection >>> 1);
        }
    }

    // AIGroup.pas:4471
    public void AddUnit(AIMoveExecutorUnit unitEntry) => AddChild(unitEntry);

    // AIGroup.pas:4476
    public void SetUnitStatus(AIMoveExecutorUnitStatus status)
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            this[i].Status = status;
        }
    }

    // AIGroup.pas:4484
    public bool RetrieveArmy(out ArmyHexSprite army, out byte selection)
    {
        army = StrategicMap.Current.MapContainer.GetField(Source.X, Source.Y, Source.MapLevel).GetArmyHexSprite();
        if (army is not null)
        {
            selection = 0;
            for (int i = Count - 1; i >= 0; i--)
            {
                AIMoveExecutorUnit units = GetUnits(i);
                int index = army.Army.FindChildIndex(units.Troop);
                selection = index != -1 ? (byte)(selection | (1 << index)) : throw new KeyNotFoundException("Unit missing!");
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
