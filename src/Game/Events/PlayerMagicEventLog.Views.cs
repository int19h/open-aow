
namespace Aow.Game.Events;

public partial class PlayerMagicEventLog
{
    public override EventView View() => Mode switch
    {
        EventMode.SpellResearched => EventView.SpellResearched,
        EventMode.OutOfMana => EventView.OutOfMana,
        EventMode.StartResearch => EventView.StartResearch,
        _ => EventView.None,
    };
}
