
namespace Aow.Game.Events;

// A view key. Instances are the keys, the name serves the reports.
public sealed class EventView(string name)
{
    public string Name => name;

    public override string ToString() => name;

    // Nothing to show, and the screen does not report the event done.
    public static EventView None { get; } = new("None");
    // Nothing to show, and the screen reports the event done at once.
    public static EventView Acknowledge { get; } = new("Acknowledge");
    // Dialogs and screens (aowMapEvents.pas:198).
    public static EventView Combat { get; } = new("Combat");
    public static EventView ArmyEvent { get; } = new("ArmyEvent");
    public static EventView CityRebellion { get; } = new("CityRebellion");
    public static EventView DiplomaticAction { get; } = new("DiplomaticAction");
    public static EventView CityNegotiation { get; } = new("CityNegotiation");
    public static EventView PartyJoin { get; } = new("PartyJoin");
    public static EventView HeroNegotiation { get; } = new("HeroNegotiation");
    public static EventView GameOver { get; } = new("GameOver");
    public static EventView ReceivedMessage { get; } = new("ReceivedMessage");
    public static EventView NewTurn { get; } = new("NewTurn");
    public static EventView CombatType { get; } = new("CombatType");
    public static EventView InformationBook { get; } = new("InformationBook");
    public static EventView SpellResearched { get; } = new("SpellResearched");
    public static EventView OutOfMana { get; } = new("OutOfMana");
    public static EventView StartResearch { get; } = new("StartResearch");
    public static EventView HeroUpgrade { get; } = new("HeroUpgrade");
    public static EventView HeroJoin { get; } = new("HeroJoin");
    public static EventView ArmyInfo { get; } = new("ArmyInfo");
    public static EventView HeroInfo { get; } = new("HeroInfo");
    public static EventView PrisonerHero { get; } = new("PrisonerHero");
    public static EventView PrisonersJoin { get; } = new("PrisonersJoin");
    public static EventView WizardsTower { get; } = new("WizardsTower");
    public static EventView Arena { get; } = new("Arena");
    public static EventView Message { get; } = new("Message");
    public static EventView Tutorial { get; } = new("Tutorial");
    public static EventView FlagMessage { get; } = new("FlagMessage");
    public static EventView Help { get; } = new("Help");
    public static EventView SaveGame { get; } = new("SaveGame");
    public static EventView SendEmailGame { get; } = new("SendEmailGame");
    public static EventView Smtp { get; } = new("SMTP");
    public static EventView Password { get; } = new("Password");
    public static EventView ItemsFound { get; } = new("ItemsFound");
    public static EventView CityProduction { get; } = new("CityProduction");
    public static EventView ProductionPlace { get; } = new("ProductionPlace");
}
