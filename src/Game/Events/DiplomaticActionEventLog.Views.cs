
namespace Aow.Game.Events;

public partial class DiplomaticActionEventLog
{
    public override EventView View() => EventView.DiplomaticAction;

    // The AI answers the proposal.
    public override void ExecuteAI(sbyte player) => Action.ExecuteAI(player);
}
