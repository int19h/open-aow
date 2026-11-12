
namespace Aow.Game.Events;

public partial class EventLog
{
    // What the map events screen shows (aowMapEvents.pas:198).
    public virtual EventView View() => EventView.None;

    // What the AI player Player does with the event when it goes through its logbook
    // (AI.pas:1626).
    public virtual void ExecuteAI(sbyte player) { }
}
