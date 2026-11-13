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
        bool result;
        EndTurnTokenEvent eventValue;
        if (!Control.Player.SingleProcessCycle)
        {
            eventValue = new EndTurnTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Player = Control.Player.PlayerIndex;
            eventValue.Day = StrategicMap.Current.Day;
            eventValue.Mode = EndTurnTokenEventMode.EndTurn;
            StrategicMap.Current.TokenManager.GetTokenControl(Control.Player.PlayerIndex).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:1029
    public static new int Priority => 10;

}
