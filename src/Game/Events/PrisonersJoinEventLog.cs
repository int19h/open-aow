#nullable disable
// Translated from Dungeon.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// FX, FY, FL
public partial class PrisonersJoinEventLog(CustomNode owner) : JoinEventLog(owner)
{
    public MapCoordinate Location { get; set; }

    // Dungeon.pas:292
    public override bool Accept(ref string errorText)
    {
        bool result;
        Dungeon dungeon;
        StructureTokenEvent tokenEvent;
        dungeon = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Dungeon>(131882);
        if (dungeon != null && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            tokenEvent = dungeon.CreateTokenEvent();
            dungeon.SetupTokenEvent(tokenEvent);
            tokenEvent.Param = Player;
            tokenEvent.Action = 1;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Dungeon.pas:311
    public override bool Cancel()
    {
        Dungeon dungeon;
        int index;
        dungeon = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Dungeon>(131882);
        if (dungeon != null)
        {
            for (int i = 0, last = dungeon.Prisoners.Units.Count - 1; i <= last; i++)
            {
                UnitType unitType = dungeon.Prisoners.Units.GetUnit(i).GetUnitType();
                if (Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType))
                {
                    index = dungeon.Prisoners.Units.GetUnit(i).GetUnitGraphicsResourceIndex();
                    if (GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index) != null)
                    {
                        int random = Random(15);
                        UnitGraphicsResource unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index);
                        _ = unitGraphicsResource.SfxLibraries[3].Play(0, 100, false, random, SamplePanning.Center, null);
                    }
                }
            }
        }

        return true;
    }

}
