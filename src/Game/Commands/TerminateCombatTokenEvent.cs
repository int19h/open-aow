// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public class TerminateCombatTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // CombatTE.pas:149
    public new const int ClassId = 2_236_422;

    // CombatTE.pas:152
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        StrategicMap.Current.DestroyCombat();
        return ExecuteResult.Done;
    }
}
