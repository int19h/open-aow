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
public partial class HeroJoinEventLog : JoinEventLog
{
    public HeroJoinEventLog(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131828;

    // Hero.PAS:116
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        Hero hero;
        bool executeNow;
        UnitControl unitControl = StrategicMap.Current.UnitControl;
        hero = unitControl.FindUnit<Hero>(FirstUnitId);
        if (hero != null && hero.Player == 0 && hero.OfferToJoin(Player))
        {
            executeNow = trigger == ExecuteTrigger.User;
            executeNow = executeNow || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin));
            executeNow = executeNow || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin));
            result = executeNow && base.Execute(mode, trigger, done);
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
        Troop u;
        MapCoordinate location = default;
        bool result = base.Accept(ref errorText);
        if (result && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            u = StrategicMap.Current.UnitControl.FindUnit(FirstUnitId);
            MapCoordinate coordinate18 = u.GetLocation();
            sbyte locationX = coordinate18.X;
            sbyte locationY = coordinate18.Y;
            sbyte locationMapLevel = coordinate18.MapLevel;
            location = new MapCoordinate(locationX, locationY, locationMapLevel);
            tokenEvent = new NegotiateTokenEvent(null) { Holder = CreationHolder() };
            tokenEvent.Player = Player;
            tokenEvent.Object = NegotiateObject.Army;
            tokenEvent.Gold = Gold;
            tokenEvent.Offer = NegotiateTokenEvent.NegotiateOffer.OfferBribe;
            tokenEvent.Location = new MapCoordinate(unchecked((sbyte)(byte)location.X), unchecked((sbyte)(byte)location.Y), unchecked((sbyte)(byte)location.MapLevel));
            tokenEvent.Mode = Set<NegotiateModeElement>.Of();
            tokenEvent.Result = NegotiateTokenEvent.NegotiateResult.Accept;
            tokenEvent.EventLogId = JoinIndex;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
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
        hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(FirstUnitId);
        if (hero != null && hero.Player == 0 && hero.OfferToJoin(Player))
        {
            Player players = StrategicMap.Current.Players.GetPlayers(Player);
            if (StrategicMap.Current.Players.GetPlayers(Player).AIBudget.RequestBudgetExpense(Gold, 0, players.AIBudget.DefaultBudget, true))
            {
                if (Accept(ref errorText))
                {
                    Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player);
                    StrategicMap.Current.Players.GetPlayers(Player).AIBudget.BudgetExpenseMade(Gold, 0, fPlayersPlayers.AIBudget.DefaultBudget, AIBudgetManager.ExpenseMode.BudgetThenPool);
                }

                result = true;
            }
        }

        return result;
    }

}
