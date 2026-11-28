#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Events;

// { Interface declaration run owned by P2. }
public partial class HeroJoinEventLog(CustomNode? owner, EngineObject holder) : JoinEventLog(owner, holder: holder)
{
    public new const int ClassId = 131_828;

    // Hero.PAS:116
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent? done)
    {
        UnitControl? unitControl = GetEngine().StrategicMap.UnitControl;
        Hero hero = unitControl.FindUnit<Hero>(FirstUnitId);
        if (hero is not null && hero.Player == PlayerId.Independent && hero.OfferToJoin(Player))
        {
            bool executeNow = trigger == ExecuteTrigger.User;
            executeNow = executeNow || (GetEngine().StrategicMap.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.HeroJoin));
            executeNow = executeNow || (GetEngine().StrategicMap.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.HeroJoin));
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
        if (result && !GetEngine().StrategicMap.Players.GetPlayers(Player).GetBusy())
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            Troop? u = strategicMap.UnitControl.FindUnit(FirstUnitId);
            MapCoordinate coordinate18 = u.GetLocation();
            int locationX = coordinate18.X;
            int locationY = coordinate18.Y;
            int locationMapLevel = coordinate18.MapLevel;
            MapCoordinate location = new((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
            NegotiateTokenEvent tokenEvent = new(null)
            {
                Holder = CreationHolder(),
                Player = Player,
                Object = NegotiateObject.Army,
                Gold = Gold,
                Offer = NegotiateTokenEvent.NegotiateOffer.OfferBribe,
                Location = new MapCoordinate(unchecked((sbyte)(byte)location.X), unchecked((sbyte)(byte)location.Y), unchecked((sbyte)(byte)location.MapLevel)),
                Mode = NegotiateModes.None,
                Result = NegotiateTokenEvent.NegotiateResult.Accept,
                EventLogId = JoinIndex
            };
            strategicMap.TokenManager.GetTokenControl(Player).AddEvent(tokenEvent);
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
        Hero hero = GetEngine().StrategicMap.UnitControl.FindUnit<Hero>(FirstUnitId);
        if (hero is not null && hero.Player == PlayerId.Independent && hero.OfferToJoin(Player))
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            Player? players = strategicMap.Players.GetPlayers(Player);
            if (strategicMap.Players.GetPlayers(Player).AIBudget.RequestBudgetExpense(Gold, mana: 0, budget: players.AIBudget.DefaultBudget, usePool: true))
            {
                string errorText = "";
                if (Accept(ref errorText))
                {
                    StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                    Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(Player);
                    _ = engineStrategicMap.Players.GetPlayers(Player).AIBudget.BudgetExpenseMade(Gold, mana: 0, budget: fPlayersPlayers.AIBudget.DefaultBudget, mode: AIBudgetManager.ExpenseMode.BudgetThenPool);
                }

                return true;
            }
        }

        return false;
    }
}
