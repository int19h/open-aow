// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Ai;

public partial class EndTurnAIPA : AIPlayerAction, IAIPlayerActionClass
{
    public EndTurnAIPA(ECustomNode owner) : base(owner)
    { }

    // AI.pas:1032
    public override bool Process()
    {
        bool Result;
        EndTurnTE Event;
        if (!FControl.FPlayer.FSingleProcessCycle)
        {
            Event = new EndTurnTE(null);
            Event.FPlayer = FControl.FPlayer.FPlayerIndex;
            Event.FDay = AoWHSMap.Current.FDay;
            Event.FMode = EndTurnTE.EndTurnMode.EndTurn;
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
