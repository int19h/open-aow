// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

public partial class EndTurnAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    public EndTurnAIPlayerAction(ECustomNode owner) : base(owner)
    { }

    // AI.pas:1032
    public override bool Process()
    {
        bool Result;
        EndTurnTokenEvent Event;
        if (!FControl.FPlayer.FSingleProcessCycle)
        {
            Event = new EndTurnTokenEvent(null);
            Event.FPlayer = FControl.FPlayer.FPlayerIndex;
            Event.FDay = AoWHSMap.Current.FDay;
            Event.FMode = EndTurnTokenEvent.Mode.EndTurn;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FControl.FPlayer.FPlayerIndex).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:1029
    public static new int Priority => 10;

}
