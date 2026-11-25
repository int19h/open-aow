#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

public class EndTurnAIPlayerAction(CustomNode? owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    // AI.pas:1032
    public override bool Process()
    {
        if (!Control.Player.SingleProcessCycle)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            EndTurnTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                Player = Control.Player.PlayerIndex,
                Day = strategicMap.Day,
                Mode = EndTurnTokenEventMode.EndTurn
            };
            strategicMap.TokenManager.GetTokenControl(Control.Player.PlayerIndex).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // AI.pas:1029
    public static new int Priority => 10;

}
