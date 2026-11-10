using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

// What the map events screen shows for each event log kind, and what an AI player does with it.
// The keys name views, and the map events screen holds the renderer of each key, so this file
// names no screen. A mod event kind overrides these and registers renderers for keys of its own.
// The overrides read the state the screen cascade read (aowMapEvents.pas:198), with the same
// queries, when the screen asks for the view.

namespace Aow.Game.Events
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

    public partial class EventLog
    {
        // What the map events screen shows (aowMapEvents.pas:198).
        public virtual EventView View() => EventView.None;

        // What the AI player Player does with the event when it goes through its logbook
        // (AI.pas:1626).
        public virtual void ExecuteAI(sbyte Player) { }
    }

    public partial class MessageEventLog
    {
        public override EventView View() => EventView.Message;
    }

    public partial class HelpMessageEventLog
    {
        public override EventView View() => EventView.Help;
    }

    public partial class TutorialMessageEventLog
    {
        public override EventView View() => EventView.Tutorial;
    }

    public partial class GameOverEventLog
    {
        // The seated player sees the game over screen, every other player the message.
        public override EventView View() => World.Map.AoWHSMap.Current.FSeatedPlayer == FPlayer ? EventView.GameOver : EventView.ReceivedMessage;
    }

    public partial class ShowBookEventLog
    {
        public override EventView View() => EventView.InformationBook;
    }

    public partial class SaveGameEventLog
    {
        public override EventView View() => EventView.SaveGame;
    }

    public partial class PasswordEventLog
    {
        public override EventView View() => EventView.Password;
    }

    public partial class UserEventLog
    {
        public override EventView View() => EventView.Acknowledge;
    }
}

namespace Aow.Game.Events
{
    public partial class CombatEventLog
    {
        public override EventView View() => EventView.Combat;
    }

    public partial class ArmyEventLog
    {
        public override EventView View() => EventView.ArmyEvent;
    }

    public partial class DiplomaticActionEventLog
    {
        public override EventView View() => EventView.DiplomaticAction;

        // The AI answers the proposal.
        public override void ExecuteAI(sbyte Player) => FAction.ExecuteAI(Player);
    }

    public partial class NegotiateRequestEventLog
    {
        // A city negotiation, or for an army the hero negotiation when a hero of the army offers
        // to join, the party join otherwise. Without an army on the field nothing shows.
        public override EventView View()
        {
            if (FObject == NegotiateObject.City)
            {
                return EventView.CityNegotiation;
            }

            if (FObject == NegotiateObject.Army)
            {
                var ArmyHS = (ArmyHS)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    var joinSelection = ArmyHS.FArmy.GetJoinSelection(FPlayer);
                    return ArmyHS.FArmy.Hero(joinSelection, RosterTables.AllHeroTypes) == null ? EventView.PartyJoin : EventView.HeroNegotiation;
                }
            }

            return EventView.None;
        }
    }

    public partial class NewTurnEventLog
    {
        public override EventView View() => EventView.NewTurn;
    }

    public partial class CombatTypeRequestEventLog
    {
        public override EventView View() => EventView.CombatType;
    }

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

    public partial class HeroUpgradeEventLog
    {
        public override EventView View() => EventView.HeroUpgrade;
    }

    public partial class HeroJoinEventLog
    {
        public override EventView View() => EventView.HeroJoin;
    }

    public partial class HeroEmergeEventLog
    {
        // The AI takes the hero when it can pay.
        public override void ExecuteAI(sbyte Player) => _ = ExecuteAI();
    }

    public partial class ArmyInfoEventLog
    {
        // A selection with a hero shows the hero, any other the army.
        public override EventView View() => FArmyView.Hero(FSelection, RosterTables.AllHeroTypes) == null ? EventView.ArmyInfo : EventView.HeroInfo;
    }

    public partial class SendEmailGameEventLog
    {
        public override EventView View() => EventView.SendEmailGame;
    }

    public partial class SMTPEventLog
    {
        public override EventView View() => EventView.SMTP;
    }
}

namespace Aow.Game.Events
{
    public partial class CityRebellionEventLog
    {
        public override EventView View() => EventView.CityRebellion;
    }

    public partial class CityProductionEventLog
    {
        public override EventView View() => EventView.CityProduction;
    }
}

namespace Aow.Game.Events
{
    public partial class ProductionPlaceEventLog
    {
        public override EventView View() => EventView.ProductionPlace;
    }
}

namespace Aow.Game.Events
{
    public partial class PrisonersJoinEventLog
    {
        // Prisoners whose first unit is a hero show the hero, any others the party.
        public override EventView View()
        {
            var UnitObject = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FFirstUnitID);
            return UnitObject != null && UnitObject is Hero ? EventView.PrisonerHero : EventView.PrisonersJoin;
        }
    }
}

namespace Aow.Game.Events
{
    public partial class EnterWizardsTowerEventLog
    {
        public override EventView View() => EventView.WizardsTower;
    }
}

namespace Aow.Game.Events
{
    public partial class EnterArenaEventLog
    {
        public override EventView View() => EventView.Arena;
    }
}

namespace Aow.Game.Events
{
    public partial class FlagMessageEventLog
    {
        public override EventView View() => EventView.FlagMessage;
    }
}

namespace Aow.Game.Events
{
    public partial class ItemExplorationSiteEventLog
    {
        public override EventView View() => EventView.ItemsFound;
    }
}
