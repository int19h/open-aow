using Aow.Engine;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Headless;

/// <summary>What a campaign document does once it is loaded. The game shows the world map when the scenario has not started.</summary>
internal enum CampaignStep
{
    PlayEmbeddedMap,
    BetweenMaps,
    StartNext,
}

/// <summary>
/// Campaign load and the world map's move to the next scenario. The transfer is the screen's own
/// calls: the path selection, the transfer setup, the Auto selection, StartScenario, the scenario
/// load and SetupScenario. The host does not keep a second copy of that transfer.
/// </summary>
internal static class CampaignPlay
{
    public static CampaignStep Choose(bool scenarioStarted, string? nextScenario) => scenarioStarted
        ? nextScenario is not null
            ? throw new ArgumentException("--next-scenario applies to a campaign that is between maps.")
            : CampaignStep.PlayEmbeddedMap
        : nextScenario is null ? CampaignStep.BetweenMaps : CampaignStep.StartNext;

    public static void Register(GameEngine engine) => engine.RegisterEngineClasses([
        EngineClass.Of<CampaignScenarioPage>(),
        EngineClass.Of<WorldMapCampaignScenario>(),
        EngineClass.Of<WorldMapCampaign>(),
    ]);

    /// <summary>Loads the named next node and applies the transfer. Returns the scenario file name.</summary>
    public static string StartNext(GameEngine engine, string name, ICollection<DroppedObject>? dropped = null)
    {
        Campaign campaign = engine.Campaign ?? throw new InvalidDataException("The campaign did not load.");
        ScenarioOffer offer = ScenarioOffers.Select(ScenarioOffers.From(campaign), name);
        campaign.SelectNextScenario(offer.Index);
        // The screen previews the scenario from the campaign directory, then loads that file name
        // after changing to the same directory. SetupScenario applies the transfer onto the loaded map.
        string directory = $"{engine.StartupDirectory}Scenario\\Campaign\\";
        campaign.TransferSettings.SetupSelection(FileBoundary.ReadName($"{directory}{offer.FileName}"));
        campaign.TransferSettings.AutoSelect();
        campaign.StartScenario(offer.Index, progress: null);
        _ = RtlHost.Files.SetCurrentDirectory(directory);
        engine.LoadHsm(FileBoundary.ReadName(offer.FileName), edit: false, progress: null, profile: GameProfile.WorldMap, dropped: dropped);
        campaign.SetupScenario();
        return offer.FileName;
    }

    /// <summary>
    /// The campaign seat, as SetupScenario and FinishScenario find it (Diplomacy.pas:2343 and
    /// :2416): from the last player down to player 1, stopping at the first human or at player 1.
    /// </summary>
    public static int CampaignSeat(StrategicMap map)
    {
        int seat = map.Players.Count - 1;
        while (seat > 1 && map.Players.GetPlayers(seat)?.PlayerType != PlayerType.Human)
        {
            seat--;
        }

        return seat;
    }

    /// <summary>
    /// The oracle's all-CPU transfer run (issue 344): after SetupScenario the campaign seat plays as
    /// the given CPU type. The seat is <see cref="CampaignSeat"/>, and it has to be human, else the
    /// call throws. PlayerList.Update then rebuilds the local and active lists (Player.PAS:1498).
    /// Returns the seat index.
    /// </summary>
    public static int SeatAsCpu(GameEngine engine, PlayerType seatType)
    {
        if (engine.HexSpriteMap is not StrategicMap map)
        {
            throw new InvalidDataException("The next scenario did not load.");
        }

        int seat = CampaignSeat(map);
        Player player = map.Players.GetPlayers(seat) ?? throw new InvalidDataException("The scenario has no campaign seat.");
        if (player.PlayerType != PlayerType.Human)
        {
            throw new InvalidDataException($"Player {seat} is {player.PlayerType}, not the human campaign seat.");
        }

        player.PlayerType = seatType;
        map.Players.Update();
        return seat;
    }
}
