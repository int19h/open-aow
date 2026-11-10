
namespace Aow.Game.Events;

public partial class PlayerMagicEventLog
{
    public override EventView View() => FMode switch
    {
        PlayerMagicEventMode.SpellResearched => EventView.SpellResearched,
        PlayerMagicEventMode.OutOfMana => EventView.OutOfMana,
        PlayerMagicEventMode.StartResearch => EventView.StartResearch,
        _ => EventView.None,
    };
}
