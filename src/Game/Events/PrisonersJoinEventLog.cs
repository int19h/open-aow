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
public partial class PrisonersJoinEventLog : JoinEventLog
{
    public PrisonersJoinEventLog(ECustomNode owner) : base(owner)
    { }

    public sbyte X { get; set; }
    public sbyte Y { get; set; }
    public sbyte MapLevel { get; set; }

    // Dungeon.pas:292
    public override bool Accept(ref string errorText)
    {
        bool result;
        Dungeon dungeon;
        StructureTokenEvent tokenEvent;
        dungeon = (Dungeon)StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).FindOwnedChild(131882);
        if (dungeon != null && !StrategicMap.Current.Players.GetPlayers(_player).GetBusy())
        {
            tokenEvent = dungeon.CreateTokenEvent();
            dungeon.SetupTokenEvent(tokenEvent);
            tokenEvent.Param = _player;
            tokenEvent.Action = 1;
            StrategicMap.Current.TokenManager.GetTokenControl(_player).AddEvent(tokenEvent);
            tokenEvent.Release();
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
        dungeon = (Dungeon)StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).FindOwnedChild(131882);
        if (dungeon != null)
        {
            for (int I = 0, last = dungeon.Prisoners.Units.Count - 1; I <= last; I++)
            {
                var unitType = dungeon.Prisoners.Units.GetUnit(I).GetUnitType();
                if (Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType))
                {
                    index = dungeon.Prisoners.Units.GetUnit(I).GetUnitGraphicsResourceIndex();
                    if (GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index) != null)
                    {
                        var random = Random(15);
                        var unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index);
                        unitGraphicsResource.SfxLibraries[3].Play(0, 100, false, random, SamplePanning.Center, null);
                    }
                }
            }
        }

        return true;
    }

}
