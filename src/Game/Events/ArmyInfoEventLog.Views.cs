
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public partial class ArmyInfoEventLog
{
    // A selection with a hero shows the hero, any other the army.
    public override EventView View() => ArmyView.Hero(Selection, types: RosterTables.AllHeroTypes) is null ? EventView.ArmyInfo : EventView.HeroInfo;
}
