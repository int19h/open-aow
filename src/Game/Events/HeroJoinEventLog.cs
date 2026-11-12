// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// { Interface declaration run owned by P2. }
public partial class HeroJoinEventLog : JoinEventLog
{
    public HeroJoinEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131828;

    // Hero.PAS:116
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        Hero hero;
        bool executeNow;
        var unitControl = AoWHSMap.Current.UnitControl;
        hero = unitControl.FindUnit<Hero>(FirstUnitId);
        if (hero != null && hero.Player == 0 && hero.OfferToJoin(_player))
        {
            executeNow = trigger == ExecuteTrigger.User;
            executeNow = executeNow || AoWHSMap.Current.PlayMode == PlayMode.Turn && GameTables.GameEngine.Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin);
            executeNow = executeNow || AoWHSMap.Current.PlayMode == PlayMode.Simultaneous && GameTables.GameEngine.Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin);
            if (executeNow)
            {
                result = base.Execute(mode, trigger, done);
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:148
    public override bool Accept(ref string errorText)
    {
        NegotiateTokenEvent tokenEvent;
        Troop U;
        MapCoordinate location = default;
        bool result = base.Accept(ref errorText);
        if (result && !AoWHSMap.Current.Players.GetPlayers(_player).GetBusy())
        {
            U = AoWHSMap.Current.UnitControl.FindUnit(FirstUnitId);
            U.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
            tokenEvent = new NegotiateTokenEvent(null);
            tokenEvent.Player = _player;
            tokenEvent.Object = NegotiateObject.Army;
            tokenEvent.Gold = Gold;
            tokenEvent.Offer = NegotiateTokenEvent.NegotiateOffer.OfferBribe;
            tokenEvent.X = (byte)location.X;
            tokenEvent.Y = (byte)location.Y;
            tokenEvent.MapLevel = (byte)location.MapLevel;
            tokenEvent.Mode = Set<NegotiateModeElement>.Of();
            tokenEvent.Result = NegotiateTokenEvent.NegotiateResult.Accept;
            tokenEvent.EventLogId = JoinIndex;
            AoWHSMap.Current.TokenManager.GetTokenControl(_player).AddEvent(tokenEvent);
            tokenEvent.Release();
        }

        return result;
    }

    // Hero.PAS:132
    // The AI reaches this through THeroEmergeEventLog.ExecuteAI(), not through the virtual
    // ExecuteAI(sbyte) of 9.6-3, which this class leaves as the no-op (AI.pas:1626 never
    // dispatched on it).
    public bool ExecuteAI()
    {
        Hero hero;
        string errorText = "";
        bool result = false;
        hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(FirstUnitId);
        if (hero != null && hero.Player == 0 && hero.OfferToJoin(_player))
        {
            var players = AoWHSMap.Current.Players.GetPlayers(_player);
            if (AoWHSMap.Current.Players.GetPlayers(_player).AIBudget.RequestBudgetExpense(Gold, 0, players.AIBudget.DefaultBudget, true))
            {
                if (Accept(ref errorText))
                {
                    var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(_player);
                    AoWHSMap.Current.Players.GetPlayers(_player).AIBudget.BudgetExpenseMade(Gold, 0, fPlayersPlayers.AIBudget.DefaultBudget, AIBudgetManager.ExpenseMode.BudgetThenPool);
                }

                result = true;
            }
        }

        return result;
    }

}
