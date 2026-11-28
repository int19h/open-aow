#nullable disable warnings
// Translated from Dungeon.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Events;

// FX, FY, FL
public partial class PrisonersJoinEventLog(CustomNode? owner, EngineObject holder) : JoinEventLog(owner, holder: holder)
{
    public MapCoordinate Location { get; init; }

    // Dungeon.pas:292
    public override bool Accept(ref string errorText)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Dungeon dungeon = strategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Dungeon>(Dungeon.ClassId);
        if (dungeon is not null && !strategicMap.Players.GetPlayers(Player).GetBusy())
        {
            StructureTokenEvent tokenEvent = dungeon.CreateTokenEvent();
            dungeon.SetupTokenEvent(tokenEvent);
            tokenEvent.Param = PlayerIdExtensions.ToOriginal(Player);
            tokenEvent.Action = 1;
            GetEngine().StrategicMap.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Dungeon.pas:311
    public override bool Cancel()
    {
        Dungeon dungeon = GetEngine().StrategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Dungeon>(Dungeon.ClassId);
        if (dungeon is not null)
        {
            for (int i = 0, last = dungeon.Prisoners.Units.Count - 1; i <= last; i++)
            {
                UnitType unitType = dungeon.Prisoners.Units.GetUnit(i).GetUnitType();
                if (unitType is UnitType.Humanoid or UnitType.Creature)
                {
                    int index = dungeon.Prisoners.Units.GetUnit(i).UnitGraphicsResourceIndex;
                    if (GetEngine().GamePack.UnitGraphicsResources.GetUnitGraphicsResource(index) is not null)
                    {
                        int random = Random(15);
                        UnitGraphicsResource? unitGraphicsResource = GetEngine().GamePack.UnitGraphicsResources.GetUnitGraphicsResource(index);
                        _ = unitGraphicsResource.SfxLibraries[3].Play(0, volume: 100, looped: false, delay: random, panning: SamplePanning.Center, callback: null);
                    }
                }
            }
        }

        return true;
    }
}
