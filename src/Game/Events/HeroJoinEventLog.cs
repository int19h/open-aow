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

    public new const int ClassID = 131828;

    // Hero.PAS:116
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        Hero Hero;
        bool ExecuteNow;
        var unitControl = AoWHSMap.Current.FUnitControl;
        Hero = unitControl.FindUnit<Hero>(FFirstUnitID);
        if (Hero != null && Hero.FPlayer == 0 && Hero.OfferToJoin(FPlayer))
        {
            ExecuteNow = Trigger == ExecuteTrigger.User;
            ExecuteNow = ExecuteNow || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin);
            ExecuteNow = ExecuteNow || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroJoin);
            if (ExecuteNow)
            {
                Result = base.Execute(Mode, Trigger, Done);
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:148
    public override bool Accept(ref string ErrorText)
    {
        NegotiateTokenEvent TE;
        Troop U;
        MapCoordinate Location = default;
        bool Result = base.Accept(ref ErrorText);
        if (Result && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            U = AoWHSMap.Current.FUnitControl.FindUnit(FFirstUnitID);
            U.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
            TE = new NegotiateTokenEvent(null);
            TE.FPlayer = FPlayer;
            TE.FObject = NegotiateObject.Army;
            TE.FGold = FGold;
            TE.FOffer = NegotiateTokenEvent.NegotiateOffer.OfferBribe;
            TE.FX = (byte)Location.X;
            TE.FY = (byte)Location.Y;
            TE.FL = (byte)Location.L;
            TE.FMode = Set<NegotiateModeElement>.Of();
            TE.FResult = NegotiateTokenEvent.NegotiateResult.Accept;
            TE.FEventLogID = FJoinIndex;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
            TE.Release();
        }

        return Result;
    }

    // Hero.PAS:132
    // The AI reaches this through THeroEmergeEventLog.ExecuteAI(), not through the virtual
    // ExecuteAI(sbyte) of 9.6-3, which this class leaves as the no-op (AI.pas:1626 never
    // dispatched on it).
    public bool ExecuteAI()
    {
        Hero Hero;
        string ErrorText = "";
        bool Result = false;
        Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FFirstUnitID);
        if (Hero != null && Hero.FPlayer == 0 && Hero.OfferToJoin(FPlayer))
        {
            var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIBudget.RequestBudgetExpense(FGold, 0, players.FAIBudget.FDefaultBudget, true))
            {
                if (Accept(ref ErrorText))
                {
                    var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIBudget.BudgetExpenseMade(FGold, 0, fPlayersPlayers.FAIBudget.FDefaultBudget, AIBudgetManager.ExpenseMode.BudgetThenPool);
                }

                Result = true;
            }
        }

        return Result;
    }

}
