#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Session;

namespace Aow.Game.Events;

public partial class DiplomaticActionEventLog
{
    public override EventView View() => EventView.DiplomaticAction;

    // The AI answers the proposal.
    public override void ExecuteAI(PlayerId? player) => Action.ExecuteAI(player);
}
