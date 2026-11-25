using Aow.Engine;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
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
}
