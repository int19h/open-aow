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
        dungeon = (Dungeon)AoWHSMap.Current.MapContainer.GetField(X, Y, MapLevel).FindOwnedChild(131882);
        if (dungeon != null && !AoWHSMap.Current.Players.GetPlayers(_player).GetBusy())
        {
            tokenEvent = dungeon.CreateTokenEvent();
            dungeon.SetupTokenEvent(tokenEvent);
            tokenEvent.Param = _player;
            tokenEvent.Action = 1;
            AoWHSMap.Current.TokenManager.GetTokenControl(_player).AddEvent(tokenEvent);
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
        dungeon = (Dungeon)AoWHSMap.Current.MapContainer.GetField(X, Y, MapLevel).FindOwnedChild(131882);
        if (dungeon != null)
        {
            for (int I = 0, last = dungeon.Prisoners.Count - 1; I <= last; I++)
            {
                var unitType = dungeon.Prisoners.GetUnit(I).GetUnitType();
                if (Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType))
                {
                    index = dungeon.Prisoners.GetUnit(I).GetUnitGraphicsResourceIndex();
                    if (AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(index) != null)
                    {
                        var random = Random(15);
                        var unitGraphicsResource = AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(index);
                        unitGraphicsResource.SfxLibraries[3].Play(0, 100, false, random, SamplePanning.Center, null);
                    }
                }
            }
        }

        return true;
    }

}
