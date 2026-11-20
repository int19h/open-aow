#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// { Interface declaration run owned by P2. }
public partial class HeroJoinEventLog(CustomNode owner) : JoinEventLog(owner)
{
    public new const int ClassId = 131828;

    // Hero.PAS:116
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        UnitControl unitControl = StrategicMap.Current.UnitControl;
        Hero hero = unitControl.FindUnit<Hero>(FirstUnitId);
        if (hero is not null && hero.Player == 0 && hero.OfferToJoin(Player))
        {
            bool executeNow = trigger == ExecuteTrigger.User;
            executeNow = executeNow || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin));
            executeNow = executeNow || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin));
            return executeNow && base.Execute(mode, trigger, done);
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:148
    public override bool Accept(ref string errorText)
    {
        bool result = base.Accept(ref errorText);
        if (result && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            Troop u = StrategicMap.Current.UnitControl.FindUnit(FirstUnitId);
            MapCoordinate coordinate18 = u.GetLocation();
            sbyte locationX = coordinate18.X;
            sbyte locationY = coordinate18.Y;
            sbyte locationMapLevel = coordinate18.MapLevel;
            MapCoordinate location = new(locationX, locationY, locationMapLevel);
            NegotiateTokenEvent tokenEvent = new(null)
            {
                Holder = CreationHolder(),
                Player = Player,
                Object = NegotiateObject.Army,
                Gold = Gold,
                Offer = NegotiateTokenEvent.NegotiateOffer.OfferBribe,
                Location = new MapCoordinate(unchecked((sbyte)(byte)location.X), unchecked((sbyte)(byte)location.Y), unchecked((sbyte)(byte)location.MapLevel)),
                Mode = Set<NegotiateModeElement>.Of(),
                Result = NegotiateTokenEvent.NegotiateResult.Accept,
                EventLogId = JoinIndex
            };
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
        }

        return result;
    }

    // Hero.PAS:132
    // The AI reaches this through THeroEmergeEventLog.ExecuteAI(), not through the virtual
    // ExecuteAI(sbyte) of 9.6-3, which this class leaves as the no-op (AI.pas:1626 never
    // dispatched on it).
    public bool ExecuteAI()
    {
        Hero hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(FirstUnitId);
        if (hero is not null && hero.Player == 0 && hero.OfferToJoin(Player))
        {
            Player players = StrategicMap.Current.Players.GetPlayers(Player);
            if (StrategicMap.Current.Players.GetPlayers(Player).AIBudget.RequestBudgetExpense(Gold, mana: 0, budget: players.AIBudget.DefaultBudget, usePool: true))
            {
                string errorText = "";
                if (Accept(ref errorText))
                {
                    Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player);
                    _ = StrategicMap.Current.Players.GetPlayers(Player).AIBudget.BudgetExpenseMade(Gold, mana: 0, budget: fPlayersPlayers.AIBudget.DefaultBudget, mode: AIBudgetManager.ExpenseMode.BudgetThenPool);
                }

                return true;
            }
        }

        return false;
    }
}
