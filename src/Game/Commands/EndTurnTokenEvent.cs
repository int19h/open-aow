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

    public new const int ClassId = 135425;

    public sbyte Player;
    public int Day;
    public EndTurnTokenEventMode Mode;

    // MoveT.PAS:1907
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Day, 20);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 21);
        stream.rwData(Bytes.Of(ref Mode, 1), 1, 22);
    }

    // MoveT.PAS:1915
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        base.Execute(tokenEvent);
        if (AoWHSMap.Current.Day == Day)
        {
            if (Mode == EndTurnTokenEventMode.EndTurn && !AoWHSMap.Current.Players.GetPlayers(Player).SingleProcessCycle)
            {
                AoWHSMap.Current.PlayerControl.EndTurn(Player);
                if (AoWHSMap.Current.PlayMode == PlayMode.Simultaneous && AoWHSMap.Current.Players.GetPlayers(Player).PlayerRace != -1)
                {
                    var players = AoWHSMap.Current.Players.GetPlayers(Player);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                    var translatedText = AbilityRules.TranslateResourceString(s_finishedTheirTurnText);
                    AoWHSMap.Current.ShowMessage(raceNamesPlural + " " + translatedText);
                }
            }

            if (Mode == EndTurnTokenEventMode.CancelEndTurn)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).SetEndTurn(false);
            }

            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    private static readonly string s_finishedTheirTurnText = "finished their turn";

    // NetworkE.pas:230

    // NetworkE.pas:230

}

public enum EndTurnTokenEventMode : byte
{
    EndTurn = 0, CancelEndTurn = 1
}
