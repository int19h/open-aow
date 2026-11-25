using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Headless;

namespace Aow.Game.Tests;

/// <summary>
/// The headless load paths. A human seat stays human and is handed to the AI only when asked.
/// A campaign is recognized from its profile, and a finished campaign stays between maps until
/// a next node is named. A dropped load child is a partial load.
/// </summary>
public sealed class HeadlessLoadTests
{
    [Fact]
    public void AHumanSeatIsHandedToTheAIOnlyWhenTheHostAsks()
    {
        Assert.False(HeadlessHooks.HandSeatToAI(PlayerType.Human));
        Assert.True(HeadlessHooks.HandSeatToAI(PlayerType.Cpu1));
        Assert.True(HeadlessHooks.HandSeatToAI(PlayerType.Independent));
        Assert.True(HeadlessHooks.HandSeatToAI(PlayerType.Free));
        HeadlessHooks.DriveHumanSeats = true;
        try
        {
            Assert.True(HeadlessHooks.HandSeatToAI(PlayerType.Human));
            Assert.True(HeadlessHooks.HandSeatToAI(PlayerType.Cpu3));
        }
        finally
        {
            HeadlessHooks.DriveHumanSeats = false;
        }
    }

    [Fact]
    public void ADroppedChildIsAPartialLoadAndATransferThenFails()
    {
        Assert.Equal(RunOutcome.Played, RunOutcome.Name(droppedChildren: false, betweenMaps: false));
        Assert.Equal(RunOutcome.BetweenMaps, RunOutcome.Name(droppedChildren: false, betweenMaps: true));
        Assert.Equal(RunOutcome.PartialLoad, RunOutcome.Name(droppedChildren: true, betweenMaps: false));
        Assert.Equal(RunOutcome.PartialLoad, RunOutcome.Name(droppedChildren: true, betweenMaps: true));
        Assert.True(RunOutcome.Succeeds(complete: true, failed: false, transfer: false, droppedChildren: true));
        Assert.False(RunOutcome.Succeeds(complete: true, failed: false, transfer: true, droppedChildren: true));
        Assert.True(RunOutcome.Succeeds(complete: true, failed: false, transfer: true, droppedChildren: false));
    }

    [Fact]
    public void AnEarlyEndNamesTheResult()
    {
        Assert.Null(RunEnd.Describe(ended: false, result: StrategicMapGameResult.None, day: 9, dayLimit: 20, won: 0, lost: 1));
        Assert.Equal("turn-limit", RunEnd.Describe(ended: true, result: StrategicMapGameResult.Failed, day: 20, dayLimit: 20, won: 0, lost: 2));
        Assert.Equal("allied-victory", RunEnd.Describe(ended: true, result: StrategicMapGameResult.Succeeded, day: 59, dayLimit: 0, won: 2, lost: 3));
        Assert.Equal("win", RunEnd.Describe(ended: true, result: StrategicMapGameResult.Succeeded, day: 12, dayLimit: 0, won: 1, lost: 1));
        Assert.Equal("win", RunEnd.Describe(ended: true, result: StrategicMapGameResult.None, day: 12, dayLimit: 0, won: 1, lost: 1));
        Assert.Equal("loss", RunEnd.Describe(ended: true, result: StrategicMapGameResult.Failed, day: 9, dayLimit: 0, won: 0, lost: 1));
        Assert.Equal("loss", RunEnd.Describe(ended: true, result: StrategicMapGameResult.None, day: 9, dayLimit: 0, won: 0, lost: 1));
    }

    [Fact]
    public void CpuHumansIsOffUnlessAsked()
    {
        HostRequest request = HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out"]);

        Assert.False(request.CpuHumans);
        Assert.Null(request.NextScenario);

        HostRequest asked = HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out", "--cpu-humans", "--next-scenario", "Subterranean Path"]);

        Assert.True(asked.CpuHumans);
        Assert.Equal("Subterranean Path", asked.NextScenario);
    }

    [Fact]
    public void AnUnknownOptionIsRefused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out", "--humans"]));

        Assert.Contains("--humans", error.Message);
    }

    [Fact]
    public void TheProfileDecidesAndTheExtensionDoesNot()
    {
        Assert.Equal(SaveKind.Campaign, SaveDocument.Kind(Document(kind: "campaign-save", profile: "campaign")));
        Assert.Equal(SaveKind.WorldMap, SaveDocument.Kind(Document(kind: "save", profile: "world-hsm")));
        Assert.Equal(SaveKind.WorldMap, SaveDocument.Kind("CFS\0not-json"u8));
    }

    [Fact]
    public void APlayingCampaignIgnoresTheExtensionAndAFinishedOneWaits()
    {
        Assert.Equal(CampaignStep.PlayEmbeddedMap, CampaignPlay.Choose(scenarioStarted: true, nextScenario: null));
        Assert.Equal(CampaignStep.BetweenMaps, CampaignPlay.Choose(scenarioStarted: false, nextScenario: null));
        Assert.Equal(CampaignStep.StartNext, CampaignPlay.Choose(scenarioStarted: false, nextScenario: "5"));
        _ = Assert.Throws<ArgumentException>(() => CampaignPlay.Choose(scenarioStarted: true, nextScenario: "5"));
    }

    [Fact]
    public void TheWorldMapOffersTheFirstThreePaths()
    {
        Assert.Equal([5, 7], ScenarioOffers.OfferedIds(first: 5, second: 7, third: -1));
        Assert.Equal([9], ScenarioOffers.OfferedIds(first: 9, second: -1, third: -1));
        Assert.Empty(ScenarioOffers.OfferedIds(first: -1, second: -1, third: -1));
    }

    [Fact]
    public void ACampaignAutosaveIsADayFile()
    {
        Assert.True(RunFiles.IsAutosaveFile("autosave.oasg"));
        Assert.True(RunFiles.IsAutosaveFile("autosave.asg"));
        Assert.True(RunFiles.IsAutosaveFile(@"Save\autosave.oacg"));
        Assert.True(RunFiles.IsAutosaveFile("autosave.acg"));
        Assert.True(RunFiles.IsAutosaveFile(@"Save\autosave.oacg.openaow-tmp"));
        Assert.False(RunFiles.IsAutosaveFile("day-02.asg"));
        Assert.False(RunFiles.IsAutosaveFile("autosave.cam"));
    }

    [Fact]
    public void ANextNodeMatchesThePathTheFileOrTheId()
    {
        ScenarioOffer[] offers =
        [
            new(Id: 5, Index: 2, FileName: "Halfling2a.csm", PathDescription: "Subterranean Path"),
            new(Id: 7, Index: 3, FileName: "Halfling2b.csm", PathDescription: "The Northern Trade Route"),
        ];

        Assert.Equal(2, ScenarioOffers.Select(offers, name: "Subterranean Path").Index);
        Assert.Equal(3, ScenarioOffers.Select(offers, name: "halfling2b.csm").Index);
        Assert.Equal(5, ScenarioOffers.Select(offers, name: "5").Id);
        ArgumentException error = Assert.Throws<ArgumentException>(() => ScenarioOffers.Select(offers, name: "Goblin Rush"));
        Assert.Contains("Subterranean Path", error.Message);
    }

    private static byte[] Document(string kind, string profile)
    {
        return System.Text.Encoding.UTF8.GetBytes($$"""
            {
              "$format": "OpenAoW",
              "$version": 9,
              "$kind": "{{kind}}",
              "$container": { "profile": "{{profile}}" },
              "root": {}
            }
            """);
    }
}
