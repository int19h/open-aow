#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// player, day, end-turn mode
public class EndTurnTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_425;

    public sbyte Player { get; set; }
    public int Day { get; set; }
    public EndTurnTokenEventMode Mode { get; set; }

    // MoveT.PAS:1907
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Day = persister.Int32("Day", value: Day);
        Player = persister.UnsignedByte(name: "Player", value: Player);
        Mode = persister.Enum("Mode", value: Mode);
    }

    // MoveT.PAS:1915
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        if (StrategicMap.Current.Day == Day)
        {
            if (Mode == EndTurnTokenEventMode.EndTurn && !StrategicMap.Current.Players.GetPlayers(Player).SingleProcessCycle)
            {
                StrategicMap.Current.PlayerControl.EndTurn(Player);
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && StrategicMap.Current.Players.GetPlayers(Player).PlayerRace != -1)
                {
                    Player players = StrategicMap.Current.Players.GetPlayers(Player);
                    string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                    string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_finishedTheirTurnText);
                    StrategicMap.Current.ShowMessage($"{raceNamesPlural} {translatedText}");
                }
            }

            if (Mode == EndTurnTokenEventMode.CancelEndTurn)
            {
                StrategicMap.Current.Players.GetPlayers(Player).SetEndTurn(false);
            }

            return ExecuteResult.Done;
        }
        else
        {
            return ExecuteResult.DoneHalt;
        }
    }

    private static readonly string s_finishedTheirTurnText = "finished their turn";
}

public enum EndTurnTokenEventMode : byte
{
    EndTurn = 0, CancelEndTurn = 1
}
