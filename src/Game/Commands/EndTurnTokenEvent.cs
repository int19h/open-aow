// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
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

    public sbyte Player { get; set; }
    public int Day { get; set; }
    public EndTurnTokenEventMode Mode { get; set; }

    // MoveT.PAS:1907
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Day = persister.Int32("Day", value: Day);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Mode = persister.Enum("Mode", value: Mode);
    }

    // MoveT.PAS:1915
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        base.Execute(tokenEvent);
        if (StrategicMap.Current.Day == Day)
        {
            if (Mode == EndTurnTokenEventMode.EndTurn && !StrategicMap.Current.Players.GetPlayers(Player).SingleProcessCycle)
            {
                StrategicMap.Current.PlayerControl.EndTurn(Player);
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && StrategicMap.Current.Players.GetPlayers(Player).PlayerRace != -1)
                {
                    var players = StrategicMap.Current.Players.GetPlayers(Player);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                    var translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_finishedTheirTurnText);
                    StrategicMap.Current.ShowMessage(raceNamesPlural + " " + translatedText);
                }
            }

            if (Mode == EndTurnTokenEventMode.CancelEndTurn)
            {
                StrategicMap.Current.Players.GetPlayers(Player).SetEndTurn(false);
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
