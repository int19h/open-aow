
namespace Aow.Game.Events;

public partial class UserEventLog
{
    public override EventView View() => EventView.Acknowledge;
}
