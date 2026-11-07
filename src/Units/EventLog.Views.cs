using Aow.Units.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.E2CampaignSupport.E2CampaignSupportUnit;

// What the map events screen shows for each event log kind, and what an AI player does with it.
// The keys name views, and the map events screen holds the renderer of each key, so this file
// names no screen. A mod event kind overrides these and registers renderers for keys of its own.
// The overrides read the state the screen cascade read (aowMapEvents.pas:198), with the same
// queries, when the screen asks for the view.

namespace Aow.Units.EventLog
{
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
        public static readonly EventView SMTP = new("SMTP");
        public static readonly EventView Password = new("Password");
        public static readonly EventView ItemsFound = new("ItemsFound");
        public static readonly EventView CityProduction = new("CityProduction");
        public static readonly EventView ProductionPlace = new("ProductionPlace");
    }

    public partial class TEventLog
    {
        // What the map events screen shows (aowMapEvents.pas:198).
        public virtual EventView View() => EventView.None;

        // What the AI player Player does with the event when it goes through its logbook
        // (AI.pas:1626).
        public virtual void ExecuteAI(sbyte Player) { }
    }

    public partial class TMessageEventLog
    {
        public override EventView View() => EventView.Message;
    }

    public partial class THelpMessageEventLog
    {
        public override EventView View() => EventView.Help;
    }

    public partial class TTutorialMessageEventLog
    {
        public override EventView View() => EventView.Tutorial;
    }

    public partial class TGameOverEventLog
    {
        // The seated player sees the game over screen, every other player the message.
        public override EventView View() => AoWHSMap.FSeatedPlayer == FPlayer ? EventView.GameOver : EventView.ReceivedMessage;
    }

    public partial class TShowBookEventLog
    {
        public override EventView View() => EventView.InformationBook;
    }

    public partial class TSaveGameEventLog
    {
        public override EventView View() => EventView.SaveGame;
    }

    public partial class TPasswordEventLog
    {
        public override EventView View() => EventView.Password;
    }

    public partial class TUserEventLog
    {
        public override EventView View() => EventView.Acknowledge;
    }
}

namespace Aow.Units.AoWE
{
    public partial class TCombatEventLog
    {
        public override EventView View() => EventView.Combat;
    }

    public partial class TArmyEventLog
    {
        public override EventView View() => EventView.ArmyEvent;
    }

    public partial class TDiplomaticActionEventLog
    {
        public override EventView View() => EventView.DiplomaticAction;

        // The AI answers the proposal.
        public override void ExecuteAI(sbyte Player) => FAction.ExecuteAI(Player);
    }

    public partial class TNegotiateRequestEventLog
    {
        // A city negotiation, or for an army the hero negotiation when a hero of the army offers
        // to join, the party join otherwise. Without an army on the field nothing shows.
        public override EventView View()
        {
            if (FObject == TNegotiateObject.noCity)
            {
                return EventView.CityNegotiation;
            }

            if (FObject == TNegotiateObject.noArmy)
            {
                var ArmyHS = (TArmyHS)AoWHSMap.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    var joinSelection = ArmyHS.FArmy.GetJoinSelection(FPlayer);
                    return ArmyHS.FArmy.Hero(joinSelection, AllHeroTypes) == null ? EventView.PartyJoin : EventView.HeroNegotiation;
                }
            }

            return EventView.None;
        }
    }

    public partial class TNewTurnEventLog
    {
        public override EventView View() => EventView.NewTurn;
    }

    public partial class TCombatTypeRequestEventLog
    {
        public override EventView View() => EventView.CombatType;
    }

    public partial class TPlayerMagicEventLog
    {
        public override EventView View() => FMode switch
        {
            TPlayerMagicEventMode.pmemSpellResearched => EventView.SpellResearched,
            TPlayerMagicEventMode.pmemOutOfMana => EventView.OutOfMana,
            TPlayerMagicEventMode.pmemStartResearch => EventView.StartResearch,
            _ => EventView.None,
        };
    }

    public partial class THeroUpgradeEventLog
    {
        public override EventView View() => EventView.HeroUpgrade;
    }

    public partial class THeroJoinEventLog
    {
        public override EventView View() => EventView.HeroJoin;
    }

    public partial class THeroEmergeEventLog
    {
        // The AI takes the hero when it can pay.
        public override void ExecuteAI(sbyte Player) => _ = ExecuteAI();
    }

    public partial class TArmyInfoEventLog
    {
        // A selection with a hero shows the hero, any other the army.
        public override EventView View() => FArmyView.Hero(FSelection, AllHeroTypes) == null ? EventView.ArmyInfo : EventView.HeroInfo;
    }

    public partial class TSendEmailGameEventLog
    {
        public override EventView View() => EventView.SendEmailGame;
    }

    public partial class TSMTPEventLog
    {
        public override EventView View() => EventView.SMTP;
    }
}

namespace Aow.Units.City
{
    public partial class TCityRebellionEventLog
    {
        public override EventView View() => EventView.CityRebellion;
    }

    public partial class TCityProductionEventLog
    {
        public override EventView View() => EventView.CityProduction;
    }
}

namespace Aow.Units.PrdPlace
{
    public partial class TProductionPlaceEventLog
    {
        public override EventView View() => EventView.ProductionPlace;
    }
}

namespace Aow.Units.Dungeon
{
    public partial class TPrisonersJoinEventLog
    {
        // Prisoners whose first unit is a hero show the hero, any others the party.
        public override EventView View()
        {
            var UnitObject = AoWHSMap.FUnitControl.FindUnit(FFirstUnitID);
            return UnitObject != null && UnitObject is THero ? EventView.PrisonerHero : EventView.PrisonersJoin;
        }
    }
}

namespace Aow.Units.WizardsTower
{
    public partial class TEnterWizardsTowerEventLog
    {
        public override EventView View() => EventView.WizardsTower;
    }
}

namespace Aow.Units.Arena
{
    public partial class TEnterArenaEventLog
    {
        public override EventView View() => EventView.Arena;
    }
}

namespace Aow.Units.FlagEvents
{
    public partial class TFlagMessageEventLog
    {
        public override EventView View() => EventView.FlagMessage;
    }
}

namespace Aow.Units.ItemExpS
{
    public partial class TItemExplorationSiteEventLog
    {
        public override EventView View() => EventView.ItemsFound;
    }
}
