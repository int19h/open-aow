using Aow.Game;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Headless;

/// <summary>
/// The oracle's campaign victory run (issue 344, decision 4) on a started campaign map. Before the
/// player control seats the first local human (PlayerC.PAS:779), the campaign seat becomes the
/// given CPU type. The player lists keep the seat from activation, so the seat is seated and its
/// view reset as the human it was, and the game hands its open turn to the AI. When the game
/// makes that seat the winner, the seat turns human before <see cref="Player.SetGameOverStatus"/>
/// tests its type, and PlayerList.Update rebuilds the local lists (Player.PAS:1498). The game's
/// own campaign branch (Player.PAS:425) then runs FinishScenario, the transfer Retrieve takes the
/// first local human, and the campaign is saved under the finished name (Diplomacy.pas:2416). The
/// native harness does the same at breakpoints on the entries of TAoWHSMap.StartLoadedMap and
/// TPlayer.SetGameOverStatus (playtest.py --campaign-victory). <see cref="Finish"/> reads what
/// FinishScenario and Retrieve left behind, after they ran.
/// </summary>
internal sealed class CampaignVictory(GameEngine engine, PlayerType seatType)
{
    private bool _finishedSaveExisted;

    /// <summary>The campaign seat, found at the start as FinishScenario finds it. -1 until the map starts.</summary>
    public int Seat { get; private set; } = -1;

    /// <summary>The seat's CPU type during play.</summary>
    public PlayerType SeatType => seatType;

    /// <summary>The day the seat turned human as the winner, null until then.</summary>
    public int? Day { get; private set; }

    /// <summary>The file name FinishScenario saved the campaign under, null until <see cref="Finish"/> finds the save.</summary>
    public string? FinishedName { get; private set; }

    /// <summary>The campaign state after FinishScenario and Retrieve, null until <see cref="Finish"/>.</summary>
    public object? Finished { get; private set; }

    public void BeforeStartLoadedMap(StrategicMap map)
    {
        if (Seat >= 0)
        {
            return;
        }

        int seat = CampaignPlay.CampaignSeat(map);
        Player player = map.Players.GetPlayers(seat) ?? throw new InvalidDataException("The map has no campaign seat.");
        if (player.PlayerType != PlayerType.Human)
        {
            throw new InvalidDataException($"Player {seat} is {player.PlayerType}, not the human campaign seat.");
        }

        Seat = seat;
        player.PlayerType = seatType;
    }

    public void BeforeSetGameOverStatus(Player player, GameOverStatus status)
    {
        if (status != GameOverStatus.Won || player.GameOverStatus != GameOverStatus.None || Day is not null
            || Seat < 0 || StrategicMap.Current is not { } map || !ReferenceEquals(player, map.Players.GetPlayers(Seat))
            || player.PlayerType == PlayerType.Human)
        {
            return;
        }

        player.PlayerType = PlayerType.Human;
        map.Players.Update();
        Day = map.Day;
        _finishedSaveExisted = FinishedPath() is { } path && RtlHost.Files.FileExists(path);
    }

    /// <summary>
    /// After the run: the effects FinishScenario and Retrieve leave (Diplomacy.pas:2416 and :1842).
    /// The scenario is no longer started, the completed list holds it, the transfer settings hold
    /// the seat's leader, and the save under the finished name is new since the victory.
    /// </summary>
    public void Finish()
    {
        if (Day is null || engine.Campaign is not { } campaign || FinishedPath() is not { } path)
        {
            return;
        }

        CampaignTransferSettings transfer = campaign.TransferSettings;
        bool written = !_finishedSaveExisted && RtlHost.Files.FileExists(path);
        Finished = new
        {
            scenario_started = campaign.ScenarioStarted,
            current_scenario = campaign.CurrentScenarioId,
            completed_scenarios = campaign.CompletedScenarios.ToArray(),
            transfer_leader = transfer.Leader?.UnitId,
            transfer_units = transfer.Units.Count,
            transfer_items = transfer.Items.Count,
            transfer_heroes = transfer.Heroes.Count,
            finished_save_written = written,
        };
        if (written && !campaign.ScenarioStarted && campaign.CompletedScenarios.IndexOf(campaign.CurrentScenarioId) >= 0)
        {
            FinishedName = DelphiPaths.ExtractFileName(path).ToString();
        }
    }

    // The name FinishScenario saves under (Diplomacy.pas:2427), in the game's Save folder.
    private string? FinishedPath() => engine.Campaign?.GetAutoSaveFileName(CampaignAutoSaveType.ScenarioFinished) is { } name
        ? $"{engine.StartupDirectory}Save\\{name}"
        : null;
}
