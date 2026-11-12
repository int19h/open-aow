
namespace Aow.Game.Events;

// A view key. Instances are the keys, the name serves the reports.
public sealed class EventView(string name)
{
    public string Name => name;

    public override string ToString() => name;

    // Nothing to show, and the screen does not report the event done.
    public static readonly EventView None = new("None");
    // Nothing to show, and the screen reports the event done at once.
    public static readonly EventView Acknowledge = new("Acknowledge");
    // Dialogs and screens (aowMapEvents.pas:198).
    public static readonly EventView Combat = new("Combat");
    public static readonly EventView ArmyEvent = new("ArmyEvent");
    public static readonly EventView CityRebellion = new("CityRebellion");
    public static readonly EventView DiplomaticAction = new("DiplomaticAction");
    public static readonly EventView CityNegotiation = new("CityNegotiation");
    public static readonly EventView PartyJoin = new("PartyJoin");
    public static readonly EventView HeroNegotiation = new("HeroNegotiation");
    public static readonly EventView GameOver = new("GameOver");
    public static readonly EventView ReceivedMessage = new("ReceivedMessage");
    public static readonly EventView NewTurn = new("NewTurn");
    public static readonly EventView CombatType = new("CombatType");
    public static readonly EventView InformationBook = new("InformationBook");
    public static readonly EventView SpellResearched = new("SpellResearched");
    public static readonly EventView OutOfMana = new("OutOfMana");
    public static readonly EventView StartResearch = new("StartResearch");
    public static readonly EventView HeroUpgrade = new("HeroUpgrade");
    public static readonly EventView HeroJoin = new("HeroJoin");
    public static readonly EventView ArmyInfo = new("ArmyInfo");
    public static readonly EventView HeroInfo = new("HeroInfo");
    public static readonly EventView PrisonerHero = new("PrisonerHero");
    public static readonly EventView PrisonersJoin = new("PrisonersJoin");
    public static readonly EventView WizardsTower = new("WizardsTower");
    public static readonly EventView Arena = new("Arena");
    public static readonly EventView Message = new("Message");
    public static readonly EventView Tutorial = new("Tutorial");
    public static readonly EventView FlagMessage = new("FlagMessage");
    public static readonly EventView Help = new("Help");
    public static readonly EventView SaveGame = new("SaveGame");
    public static readonly EventView SendEmailGame = new("SendEmailGame");
    public static readonly EventView Smtp = new("SMTP");
    public static readonly EventView Password = new("Password");
    public static readonly EventView ItemsFound = new("ItemsFound");
    public static readonly EventView CityProduction = new("CityProduction");
    public static readonly EventView ProductionPlace = new("ProductionPlace");
}
