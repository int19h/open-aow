using System.Reflection;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Headless;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 344, the campaign victory run. The host hook runs first in Player.SetGameOverStatus, before
/// the game tests the status or the player's type (Player.PAS:425), so a campaign seat that plays as
/// a CPU player can turn human before the campaign branch reads its type. HeadlessSeatTests pins the
/// second hook, BeforeStartLoadedMap, and the seat conversion and its turn back to human. The run of
/// FinishScenario and Retrieve needs a loaded campaign map, so the oracle's campaign_victory scenario
/// covers it, and so do the Program rules on the options, which need a full host run. The capture
/// scan and the terminated stop in Process are tested here on a bare map.
/// </summary>
[Collection("Engine")]
public sealed class CampaignVictoryHookTests
{
    [Fact]
    public void TheHookSeesEveryCallFirstEvenWithoutAChange()
    {
        Player player = new(null) { Holder = EngineFixture.Engine };
        List<(Player Player, GameOverStatus Status)> calls = [];
        HeadlessHooks.BeforeSetGameOverStatus = (target, status) => calls.Add((target, status));
        try
        {
            // The status stays None, so the game changes nothing after the hook.
            player.SetGameOverStatus(GameOverStatus.None);
        }
        finally
        {
            HeadlessHooks.BeforeSetGameOverStatus = null;
        }

        (Player called, GameOverStatus requested) = Assert.Single(calls);
        Assert.Same(player, called);
        Assert.Equal(GameOverStatus.None, requested);
        Assert.Equal(GameOverStatus.None, player.GameOverStatus);
    }

    [Fact]
    public void TheVictoryRunIsOffUnlessAsked()
    {
        Assert.False(HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out"]).CampaignVictory);
        Assert.True(HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out", "--campaign-victory"]).CampaignVictory);
    }

    [Fact]
    public void TheLeaderAttackCanMoveTheWholeArmy()
    {
        Assert.Equal(new LeaderAttackTarget(Player: 1, UnitId: 64, WholeArmy: true), LeaderAttackTarget.Parse("1:64:army"));
        Assert.False(LeaderAttackTarget.Parse("1:64").WholeArmy);
        ArgumentException error = Assert.Throws<ArgumentException>(() => LeaderAttackTarget.Parse("1:64:armies"));
        Assert.Contains("PLAYER:UNIT:army", error.Message);
    }

    [Fact]
    public void TheCaptureInputNamesAPlayer()
    {
        Assert.Equal(0, HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out"]).CaptureLocations);
        Assert.Equal(1, HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out", "--capture-locations", "1"]).CaptureLocations);
        ArgumentException error = Assert.Throws<ArgumentException>(() => HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out", "--capture-locations", "0"]));
        Assert.Contains("--capture-locations takes a player", error.Message);
    }

    [Fact]
    public void ATerminatedMapStopsProcessOnlyWhileTheHookIsSet()
    {
        // The probe action is the phase switch. The hook path returns before it. The clear path runs it.
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        using IDisposable restore = EngineFixture.UseMap(map);
        AIPlayerControl? control = null;
        HeadlessHooks.BeforeAIPlayerProcess = null;
        try
        {
            map.TriggerGameTerminatedEvent();
            Player player = map.Players.GetPlayers(1) ?? throw new InvalidOperationException("the seat is missing");
            player.PlayerType = PlayerType.Cpu1;
            (map.TokenManager ?? throw new InvalidOperationException("the token manager is missing")).CreatePlayer(player.PlayerIndex);
            control = new AIPlayerControl(null) { Holder = EngineFixture.Engine, Player = player };
            ProbeAction probe = new(null) { Holder = EngineFixture.Engine };
            probe.SetOwner(control.Actions ?? throw new InvalidOperationException("the action list is missing"));
            _ = typeof(AIPlayerControl).GetProperty(nameof(AIPlayerControl.Phase))!
                .GetSetMethod(nonPublic: true)!
                .Invoke(control, parameters: [AIPlayerProcessPhase.Main]);

            bool open = control.Process(map.AIExecutor, tokenControlDone: false);
            Assert.True(open);
            Assert.Equal(1, probe.Calls);
            Assert.Equal(AIPlayerProcessPhase.Main, control.Phase);

            int seen = 0;
            HeadlessHooks.BeforeAIPlayerProcess = () => seen++;
            bool stopped = control.Process(map.AIExecutor, tokenControlDone: false);
            Assert.False(stopped);
            Assert.Equal(1, seen);
            Assert.Equal(1, probe.Calls);
        }
        finally
        {
            HeadlessHooks.BeforeAIPlayerProcess = null;
            control?.Dispose();
            map.Dispose();
            restore.Dispose();
        }
    }

    [Fact]
    public void TheCaptureScanFollowsStructureControlOrder()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        using IDisposable restore = EngineFixture.UseMap(map);
        try
        {
            OwningNode list = StructureList(map);
            BareLocation first = Place(list, id: 11, label: "First", victory: true);
            BareLocation middle = Place(list, id: 12, label: "Middle", victory: false);
            BareLocation second = Place(list, id: 13, label: "Second", victory: true);

            DirectedInputs inputs = RunCapture(map);

            Assert.Equal(2, inputs.CaptureRequests.Count);
            Assert.Equal(11, Read<int>(inputs.CaptureRequests[0], name: "structure_id"));
            Assert.Equal("First", Read<string>(inputs.CaptureRequests[0], name: "name"));
            Assert.Equal(0, Read<int>(inputs.CaptureRequests[0], name: "previous_player"));
            Assert.True(Read<bool>(inputs.CaptureRequests[0], name: "captured"));
            Assert.Equal(13, Read<int>(inputs.CaptureRequests[1], name: "structure_id"));
            Assert.Equal("Second", Read<string>(inputs.CaptureRequests[1], name: "name"));
            Assert.Equal([1], first.Captors);
            Assert.Empty(middle.Captors);
            Assert.Equal([1], second.Captors);
            Assert.True(Read<bool>(inputs.CaptureRecord, name: "done"));
        }
        finally
        {
            map.Dispose();
            restore.Dispose();
        }
    }

    [Fact]
    public void TheCaptureScanStopsWhenTheFirstCaptureEndsTheGame()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        using IDisposable restore = EngineFixture.UseMap(map);
        try
        {
            OwningNode list = StructureList(map);
            BareLocation first = Place(list, id: 21, label: "Ends", victory: true, endGame: true);
            BareLocation second = Place(list, id: 22, label: "Later", victory: true);

            DirectedInputs inputs = RunCapture(map);

            Assert.Equal([1], first.Captors);
            Assert.Empty(second.Captors);
            Assert.Equal(21, Read<int>(Assert.Single(inputs.CaptureRequests), name: "structure_id"));
            Assert.Equal(StrategicMapGameStatus.Terminated, map.GameStatus);
            Assert.True(Read<bool>(inputs.CaptureRecord, name: "done"));
        }
        finally
        {
            map.Dispose();
            restore.Dispose();
        }
    }

    [Fact]
    public void TheCaptureScanThrowsWhenNoLocationRemains()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        using IDisposable restore = EngineFixture.UseMap(map);
        try
        {
            _ = Place(StructureList(map), id: 31, label: "Held", victory: false);
            InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => RunCapture(map));
            Assert.Contains("player 1 has no victory location left to capture", error.Message);
        }
        finally
        {
            map.Dispose();
            restore.Dispose();
        }
    }

    private static DirectedInputs RunCapture(StrategicMap map)
    {
        map.CurrentPlayer = PlayerId.FromOriginal(1);
        DirectedInputs inputs = new(map, items: false, spell: 0, dispel: 0, endEnchantment: null!, leaderAttack: null, captureLocations: 1);
        inputs.DaySaved();
        inputs.BeforeFrame();
        return inputs;
    }

    private static OwningNode StructureList(StrategicMap map)
    {
        FieldInfo field = typeof(StructureControl).GetField("_structures", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("the structure list is missing");
        return field.GetValue(map.StructureControl ?? throw new InvalidOperationException("the map has no structure control")) as OwningNode
            ?? throw new InvalidOperationException("the structure list is missing");
    }

    private static void MarkVictory(PlayerStructure structure, int player)
    {
        FieldInfo field = typeof(PlayerStructure).GetField("_victoryLocations", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("the victory bits are missing");
        BitList bits = field.GetValue(structure) as BitList
            ?? throw new InvalidOperationException("the victory bits are missing");
        PlayerId id = PlayerId.FromOriginal(player) ?? throw new InvalidOperationException("the player is missing");
        bits[id.Value] = true;
    }

    private static BareLocation Place(OwningNode list, int id, string label, bool victory, bool endGame = false)
    {
        BareLocation structure = new(null)
        {
            Holder = EngineFixture.Engine,
            StructureId = id,
            Label = label,
            EndGame = endGame,
        };
        if (victory)
        {
            MarkVictory(structure, player: 1);
        }

        _ = list.AddChild(structure);
        return structure;
    }

    private static T Read<T>(object record, string name)
    {
        PropertyInfo property = record.GetType().GetProperty(name)
            ?? throw new InvalidOperationException($"the record has no {name}");
        return (T)(property.GetValue(record) ?? throw new InvalidOperationException($"the record has no {name}"));
    }

    private sealed class BareLocation(CustomNode? owner) : PlayerStructure(owner)
    {
        public string Label { get; init; } = "";

        public bool EndGame { get; init; }

        public List<int> Captors { get; } = [];

        public override string GetName() => Label;

        public override void SetPlayer(PlayerId? player)
        {
            Captors.Add(PlayerIdExtensions.ToOriginal(player));
            Player = player;
            if (EndGame)
            {
                (StrategicMap.Current ?? throw new InvalidOperationException("the map is missing")).TriggerGameTerminatedEvent();
            }
        }
    }

    private sealed class ProbeAction(CustomNode? owner) : AIPlayerAction(owner)
    {
        public int Calls { get; private set; }

        public override bool Process()
        {
            Calls++;
            return true;
        }
    }
}
