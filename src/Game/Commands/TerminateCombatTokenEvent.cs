// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public partial class TerminateCombatTokenEvent : TokenEvent
{

    public TerminateCombatTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:149
    public new const int ClassID = 2236422;

    // CombatTE.pas:152
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        AoWHSMap.Current.DestroyCombat();
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
