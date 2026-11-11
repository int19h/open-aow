// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// player, day, end-turn mode
public partial class EndTurnTokenEvent : TokenEvent
{
    public EndTurnTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135425;

    public sbyte FPlayer;
    public int FDay;
    public Mode FMode;

    // MoveT.PAS:1907
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FDay, 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 21);
        Stream.rwData(Bytes.Of(ref FMode, 1), 1, 22);
    }

    // MoveT.PAS:1915
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result;
        base.Execute(TE);
        if (AoWHSMap.Current.FDay == FDay)
        {
            if (FMode == Mode.EndTurn && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FSingleProcessCycle)
            {
                AoWHSMap.Current.FPlayerControl.EndTurn(FPlayer);
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace != -1)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                    var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players.FPlayerRace]);
                    var aoWETranslateRStr = AbilityRules.TranslateRStr(P5FinishedTheirTurnRStr);
                    AoWHSMap.Current.ShowMessage(raceNamesPlural + " " + aoWETranslateRStr);
                }
            }

            if (FMode == Mode.CancelEndTurn)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetEndTurn(false);
            }

            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    private static readonly string P5FinishedTheirTurnRStr = "finished their turn";

    public enum Mode : byte
    {
        EndTurn = 0, CancelEndTurn = 1
    }

    // NetworkE.pas:230

    // NetworkE.pas:230

}
