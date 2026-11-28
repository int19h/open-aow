using System.Globalization;
using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Host;
using Aow.Game.Net;
using Aow.Game.Recording;
using Aow.Game.Session;
using Aow.Game.World.Map;
using FormatJson = Aow.Formats.Json;

namespace Aow.Game.Tests;

/// <summary>
/// The token recorder of issue 343, step 1: the shape of its two files and its split between
/// input events and the events an execution derives.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class TokenRecorderTests
{
    private const string ProbeClass = "TokenRecorderTests.ProbeTokenEvent";

    static TokenRecorderTests() => _ = EngineClass.Register(TokenEvent.ClassId, static owner => new ProbeTokenEvent(owner));

    [Fact]
    public void AnInputLineCarriesTheSeatOriginCountersAndDocumentAsMinimalJson()
    {
        using RecordedMap recorded = new();
        recorded.Map.Day = 4;
        recorded.AddToPlayer(PlayerId.Player2, probe: recorded.Probe("order"));

        recorded.Drain();

        string line = Assert.Single(recorded.EventLines());
        Assert.Equal("""{"seat":2,"queue":"player","origin":"human","counter":0,"round":0,"addRound":0,"day":4,"class":"TokenRecorderTests.ProbeTokenEvent","event":{"name":"order"}}""", line);
        using JsonDocument trace = JsonDocument.Parse(Assert.Single(recorded.TraceLines()));
        JsonElement root = trace.RootElement;
        Assert.Equal(["counter", "round", "day", "seat", "class", "input", "result", "attempts", "randSeed", "defendersSeed", "derived"],
            root.EnumerateObject().Select(static member => member.Name));
        Assert.Equal(2, root.GetProperty("seat").GetInt32());
        Assert.True(root.GetProperty("input").GetBoolean());
        Assert.Equal("done", root.GetProperty("result").GetString());
        Assert.Equal(DelphiRandom.RandSeed, root.GetProperty("randSeed").GetInt32());
        Assert.Equal(recorded.Map.DefendersSeed, root.GetProperty("defendersSeed").GetInt32());
        Assert.Equal(0, root.GetProperty("derived").GetArrayLength());
        Assert.DoesNotContain(": ", recorded.TraceText);
        Assert.DoesNotContain(", ", recorded.TraceText);
    }

    [Fact]
    public void EventsAddedDuringAnExecutionAreDerivedAndOnlyInTheTrace()
    {
        using RecordedMap recorded = new();
        ProbeTokenEvent order = recorded.Probe("order", run: (_, executor) =>
        {
            executor.TokenManager!.GetTokenControl(PlayerId.Player2)!.AddEvent(recorded.Probe("answer"));
            executor.AddSystemEvent(recorded.Probe("system"));
            Assert.True(executor.InsertExecuteEvent(recorded.Probe("inserted")));
            return ExecuteResult.Done;
        });
        recorded.AddToPlayer(PlayerId.Player1, probe: order);
        ProbeTokenEvent outside = recorded.Probe("outside");
        recorded.Map.TokenManager!.TokenExecutor.AddSystemEvent(outside);
        _ = outside.Release();

        recorded.Drain();

        // A system event added outside an execution is an input. It runs before the token controls.
        Assert.Equal(["outside", "order"], recorded.EventLines().Select(static line => Name(line)));
        using JsonDocument system = JsonDocument.Parse(recorded.EventLines()[0]);
        Assert.Equal(JsonValueKind.Null, system.RootElement.GetProperty("seat").ValueKind);
        Assert.Equal("system", system.RootElement.GetProperty("queue").GetString());

        List<JsonDocument> trace = [.. recorded.TraceLines().Select(static line => JsonDocument.Parse(line))];
        try
        {
            Assert.Equal([true, true, false, false, false], trace.Select(static line => line.RootElement.GetProperty("input").GetBoolean()));
            JsonElement derived = trace[1].RootElement.GetProperty("derived");
            Assert.Equal(["player:2", "system:", "executor:"], derived.EnumerateArray().Select(static item =>
                item.GetProperty("queue").GetString() + ":" + (item.GetProperty("seat").ValueKind == JsonValueKind.Null ? "" : item.GetProperty("seat").GetInt32().ToString(CultureInfo.InvariantCulture))));
            Assert.All(derived.EnumerateArray(), static item => Assert.Equal(ProbeClass, item.GetProperty("class").GetString()));
            Assert.Equal(3, recorded.Recorder.DerivedCount);
            Assert.Equal(2, recorded.Recorder.InputCount);
            Assert.Equal(5, recorded.Recorder.ExecutedCount);
            Assert.Equal(0, recorded.Recorder.NestedCount);
        }
        finally
        {
            trace.ForEach(static line => line.Dispose());
        }
    }

    [Fact]
    public void ARetriedEventIsCapturedBeforeItsFirstExecutionAndTracedWhenItCompletes()
    {
        using RecordedMap recorded = new();
        ProbeTokenEvent move = recorded.Probe("move", run: (probe, executor) =>
        {
            switch (executor.ExecuteCount)
            {
                case 0:
                    executor.ReportValidated(probe, isValid: false);
                    return ExecuteResult.Retry;
                case 1:
                    executor.AddSystemEvent(recorded.Probe("effect"));
                    return ExecuteResult.Retry;
                default:
                    return ExecuteResult.DoneHalt;
            }
        });
        recorded.AddToPlayer(PlayerId.Player1, probe: move);

        recorded.Drain();

        using JsonDocument trace = JsonDocument.Parse(recorded.TraceLines()[0]);
        JsonElement root = trace.RootElement;
        Assert.Equal("halt", root.GetProperty("result").GetString());
        Assert.Equal(3, root.GetProperty("attempts").GetInt32());
        Assert.False(root.GetProperty("valid").GetBoolean());
        Assert.Equal(1, root.GetProperty("derived").GetArrayLength());
        // The capture saw the event before its first execution.
        Assert.Equal("move", Name(Assert.Single(recorded.EventLines())));
        Assert.Equal(3, move.Executions);
    }

    [Fact]
    public void AnEventStillExecutingIsTracedAsUnfinishedWhenRecordingStops()
    {
        using RecordedMap recorded = new();
        recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("endless", run: static (_, _) => ExecuteResult.Retry));
        recorded.Drain();
        Assert.Empty(recorded.TraceLines());

        recorded.Recorder.Dispose();

        using JsonDocument trace = JsonDocument.Parse(Assert.Single(recorded.TraceLines()));
        Assert.Equal("unfinished", trace.RootElement.GetProperty("result").GetString());
        Assert.Equal(recorded.Rounds, trace.RootElement.GetProperty("attempts").GetInt32());
    }

    [Fact]
    public void TheEndOfALocalBatchIsTracedButIsNoInput()
    {
        using RecordedMap recorded = new(localSeat: PlayerId.Player1);
        // A local batch goes to the other players as packets, so its event is one the schema knows.
        SyncTokenEvent sync = new(null!) { Holder = EngineFixture.Engine };
        recorded.Map.TokenManager!.GetTokenControl(PlayerId.Player1)!.AddEvent(sync);
        _ = sync.Release();

        recorded.Drain();

        // A local token control ends each of its batches with a pass, one in every round here.
        Assert.Equal("SyncTokenEvent", Name(Assert.Single(recorded.EventLines())));
        List<string> trace = [.. recorded.TraceLines().Select(static line =>
        {
            using JsonDocument executed = JsonDocument.Parse(line);
            return executed.RootElement.GetProperty("class").GetString() + ":" + executed.RootElement.GetProperty("input").GetBoolean().ToString();
        })];
        Assert.Equal("SyncTokenEvent:True", trace[0]);
        Assert.Equal(recorded.Rounds, trace.Count - 1);
        Assert.All(trace.Skip(1), static line => Assert.Equal("PassTokenTokenEvent:False", line));
    }

    [Fact]
    public void AnAddInsideTheAIIsTheAIsAndAHostActionThereIsTheHosts()
    {
        using RecordedMap recorded = new();
        recorded.Map.SetupPlayerControl(PlayMode.Turn, connection: PlayerConnectionType.Local);
        recorded.Map.CurrentPlayer = PlayerId.Player1;
        _ = recorded.Map.AIExecutor.ActivatePlayer(PlayerId.Player1);
        Action? previous = HeadlessHooks.BeforeAIPlayerProcess;
        int calls = 0;
        HeadlessHooks.BeforeAIPlayerProcess = () =>
        {
            calls += 1;
            if (calls == 1)
            {
                recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("decided"));
                using (recorded.Recorder.HostInput("directed", changesState: false))
                {
                    recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("directed"));
                }
            }
        };
        try
        {
            recorded.Map.AIExecutor.Process();
        }
        finally
        {
            HeadlessHooks.BeforeAIPlayerProcess = previous;
            recorded.Map.AIExecutor.DeactivatePlayer(PlayerId.Player1);
        }

        // The turn control holds the executor on the current seat. The recorder reads the adds above.
        recorded.Map.TokenManager!.TokenExecutor.HoldTacticalBattle = false;
        recorded.Drain();

        Assert.True(calls >= 1);
        List<string> lines = recorded.EventLines();
        Assert.Equal("""{"host":"directed","counter":0,"round":0,"day":0,"adds":1}""", lines[0]);
        Assert.Equal(["decided:ai", "directed:host"], lines.Skip(1).Select(static line =>
        {
            using JsonDocument input = JsonDocument.Parse(line);
            return Name(line) + ":" + input.RootElement.GetProperty("origin").GetString();
        }));
    }

    [Fact]
    public void TheDocumentOfAnEventIsItsOwnFormatNode()
    {
        using EndTurnTokenEvent endTurn = new(null!)
        {
            Holder = EngineFixture.Engine,
            Day = 7,
            Player = PlayerId.Player2,
            Mode = EndTurnTokenEventMode.CancelEndTurn,
        };

        string document = FormatJson.JsonWriter.ToMinimalText(TokenEventDocument.Write(endTurn, engine: EngineFixture.Engine));

        Assert.Equal("EndTurnTokenEvent", TokenEventDocument.ClassName(endTurn));
        Assert.Equal("""{"TokenCounter":-1,"Day":7,"Player":2,"Mode":"CancelEndTurn"}""", document);
    }

    [Fact]
    public void AFaultInTheCaptureStopsTheRecordingAndNotTheGame()
    {
        using RecordedMap recorded = new(capture: static _ => throw new InvalidOperationException("capture refused"));
        ProbeTokenEvent first = recorded.Probe("first");
        ProbeTokenEvent second = recorded.Probe("second");
        recorded.AddToPlayer(PlayerId.Player1, probe: first);
        recorded.AddToPlayer(PlayerId.Player1, probe: second);

        recorded.Drain();

        Assert.Equal(1, first.Executions);
        Assert.Equal(1, second.Executions);
        Assert.Equal(2, recorded.Map.TokenManager!.TokenExecutor.TokenCounter);
        Assert.Equal(1, recorded.Recorder.FaultCount);
        Assert.Equal("capture: InvalidOperationException: capture refused", recorded.Recorder.Fault);
        Assert.Empty(recorded.TraceLines());
        Assert.Empty(recorded.EventLines());
    }

    [Fact]
    public void AFaultInTheTraceWriterStopsTheRecordingAndNotTheGame()
    {
        using RecordedMap recorded = new(trace: new RefusingWriter());
        ProbeTokenEvent first = recorded.Probe("first");
        ProbeTokenEvent second = recorded.Probe("second");
        recorded.AddToPlayer(PlayerId.Player1, probe: first);
        recorded.AddToPlayer(PlayerId.Player1, probe: second);

        recorded.Drain();

        // The first event executed once and the executor moved on, the second was not recorded.
        Assert.Equal(1, first.Executions);
        Assert.Equal(1, second.Executions);
        Assert.Equal(2, recorded.Map.TokenManager!.TokenExecutor.TokenCounter);
        Assert.Equal(1, recorded.Recorder.FaultCount);
        Assert.Equal("execution end: IOException: disk full", recorded.Recorder.Fault);
        Assert.Equal(["first"], recorded.EventLines().Select(static line => Name(line)));
    }

    [Fact]
    public void AnInputWhoseDocumentFailsKeepsAMarkedLineAndTheOthersStay()
    {
        using RecordedMap recorded = new(capture: static tokenEvent =>
        {
            string name = ((ProbeTokenEvent)tokenEvent).Name;
            return name == "broken" ? () => throw new InvalidOperationException("no schema") : () => new FormatJson.JsonObject().Add("name", new FormatJson.JsonString(name));
        });
        recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("broken"));
        recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("whole"));
        recorded.Drain();

        List<string> lines = recorded.EventLines();

        Assert.Equal(2, lines.Count);
        using JsonDocument broken = JsonDocument.Parse(lines[0]);
        Assert.Equal(JsonValueKind.Null, broken.RootElement.GetProperty("event").ValueKind);
        Assert.Equal("InvalidOperationException: no schema", broken.RootElement.GetProperty("documentError").GetString());
        Assert.Equal("whole", Name(lines[1]));
        Assert.Equal(1, recorded.Recorder.FaultCount);
    }

    [Fact]
    public void EachRecorderRecordsOnlyItsOwnExecutor()
    {
        using RecordedMap left = new();
        using RecordedMap right = new();
        left.AddToPlayer(PlayerId.Player1, probe: left.Probe("left one"));
        right.AddToPlayer(PlayerId.Player1, probe: right.Probe("right one"));
        left.AddToPlayer(PlayerId.Player2, probe: left.Probe("left two"));
        right.Drain();
        left.Drain();
        right.AddToPlayer(PlayerId.Player2, probe: right.Probe("right two"));
        right.Drain();

        Assert.Equal(["left one", "left two"], left.EventLines().Select(static line => Name(line)));
        Assert.Equal(["right one", "right two"], right.EventLines().Select(static line => Name(line)));
        Assert.Equal(2, left.TraceLines().Count);
        Assert.Equal(2, right.TraceLines().Count);
    }

    [Fact]
    public void AStoppedRecorderIsSilent()
    {
        using RecordedMap recorded = new();
        recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("before"));
        recorded.Drain();
        List<string> events = recorded.EventLines();
        string trace = recorded.TraceText;

        ProbeTokenEvent after = recorded.Probe("after", run: static (_, executor) =>
        {
            executor.AddSystemEvent(new ProbeTokenEvent(null!) { Holder = EngineFixture.Engine, Name = "derived" });
            return ExecuteResult.Done;
        });
        recorded.AddToPlayer(PlayerId.Player1, probe: after);
        recorded.Drain();
        recorded.Recorder.Dispose();

        Assert.Equal(1, after.Executions);
        Assert.Equal(["before"], events.Select(static line => Name(line)));
        Assert.Equal(trace, recorded.TraceText);
        Assert.Equal((1, 1, 0), (recorded.Recorder.InputCount, recorded.Recorder.ExecutedCount, recorded.Recorder.DerivedCount));
    }

    [Fact]
    public void TheBattleAIHoldsOnlyTheTurnOfItsSeat()
    {
        using RecordedMap recorded = new();
        using IDisposable previousCombat = EngineFixture.UseCombatMap(CombatMap.Current);
        using CombatMap combat = new(null, holder: EngineFixture.Engine) { CurrentPlayer = PlayerId.Player1 };
        combat.AI.Status = BattleAI.AIStatus.Done;
        recorded.Map.CombatMap = combat;
        try
        {
            recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("battle seat"));
            recorded.AddToPlayer(PlayerId.Player2, probe: recorded.Probe("other seat"));
        }
        finally
        {
            recorded.Map.CombatMap = null;
        }

        recorded.Drain();

        Assert.Equal(["battle seat:ai", "other seat:human"], recorded.EventLines().Select(static line =>
        {
            using JsonDocument input = JsonDocument.Parse(line);
            return Name(line) + ":" + input.RootElement.GetProperty("origin").GetString();
        }));
    }

    [Fact]
    public void RefusedStructureAndNegotiationEventsTraceTheirValidation()
    {
        using RecordedMap recorded = new();
        recorded.InstallField();
        // No structure stands on the one field at 0,0, and the negotiation names no location.
        recorded.Add(PlayerId.Player1, tokenEvent: new StructureTokenEvent(null!) { Holder = EngineFixture.Engine });
        recorded.Add(PlayerId.Player1, tokenEvent: new NegotiateTokenEvent(null!) { Holder = EngineFixture.Engine, Location = new MapCoordinate(X: -1, Y: 0, MapLevel: 0) });

        recorded.Drain();

        Assert.Equal(["StructureTokenEvent:halt:False", "NegotiateTokenEvent:done:False"], recorded.TraceLines().Select(static line => Executed(line)));
    }

    [Fact]
    public void ARealMoveAndMultipleMoveReportTheValidationTheyActOn()
    {
        using RecordedMap recorded = new(isDefaultCapture: true);
        // A move with no path refuses itself. The multiple move keeps the result of its move.
        recorded.Add(PlayerId.Player1, tokenEvent: new ArmyDefaultMoveTokenEvent(null!) { Holder = EngineFixture.Engine });
        MultiMoveTokenEvent multiMove = new(null!) { Holder = EngineFixture.Engine };
        ArmyDefaultMoveTokenEvent inner = new(null!) { Holder = EngineFixture.Engine };
        multiMove.Events.Add(inner);
        _ = inner.Release();
        recorded.Add(PlayerId.Player1, tokenEvent: multiMove);

        recorded.Drain();

        Assert.Equal(["ArmyDefaultMoveTokenEvent:halt:False", "MultiMoveTokenEvent:done:False"], recorded.TraceLines().Select(static line => Executed(line)));
        // The default capture writes each event's own-format node through the document backend.
        List<string> events = recorded.EventLines();
        Assert.Equal(2, events.Count);
        using JsonDocument move = JsonDocument.Parse(events[0]);
        Assert.Equal("ArmyDefaultMoveTokenEvent", move.RootElement.GetProperty("class").GetString());
        Assert.Equal(JsonValueKind.Object, move.RootElement.GetProperty("event").ValueKind);
        // The document is the event as it was added, with the token counter back at -1.
        using ArmyDefaultMoveTokenEvent same = new(null!) { Holder = EngineFixture.Engine };
        Assert.Equal(FormatJson.JsonWriter.ToMinimalText(TokenEventDocument.Write(same, engine: EngineFixture.Engine)), move.RootElement.GetProperty("event").GetRawText());
        Assert.Equal(-1, move.RootElement.GetProperty("event").GetProperty("TokenCounter").GetInt32());
        using JsonDocument multi = JsonDocument.Parse(events[1]);
        Assert.Contains("ArmyDefaultMoveTokenEvent", multi.RootElement.GetProperty("event").GetProperty("Events").GetRawText(), comparisonType: StringComparison.Ordinal);
        Assert.Equal(0, recorded.Recorder.FaultCount);
    }

    [Fact]
    public void AHostActionLeavesAMarkerInOrderWithTheInputs()
    {
        using RecordedMap recorded = new();
        recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("before"));
        recorded.Drain();
        TokenExecutor executor = recorded.Map.TokenManager!.TokenExecutor;
        string at = $"\"counter\":{executor.TokenCounter},\"round\":{executor.Round},\"day\":0";
        using (recorded.Recorder.HostInput("refused-request", changesState: false))
        {
            // Adds nothing, so it leaves no marker.
        }

        using (recorded.Recorder.HostInput("capture", changesState: true))
        {
            // Changes the state directly and adds nothing: the marker still shows it.
        }

        using (recorded.Recorder.HostInput("order", changesState: false))
        {
            recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("ordered"));
            recorded.AddToPlayer(PlayerId.Player2, probe: recorded.Probe("ordered too"));
        }

        recorded.Drain();

        List<string> lines = recorded.EventLines();
        Assert.Equal(5, lines.Count);
        Assert.Equal("before", Name(lines[0]));
        Assert.Equal($$"""{"host":"capture",{{at}},"adds":0}""", lines[1]);
        Assert.Equal($$"""{"host":"order",{{at}},"adds":2}""", lines[2]);
        Assert.Equal(["ordered:host", "ordered too:host"], lines.Skip(3).Select(static line =>
        {
            using JsonDocument input = JsonDocument.Parse(line);
            return Name(line) + ":" + input.RootElement.GetProperty("origin").GetString();
        }));
        Assert.Equal(2, recorded.Recorder.HostActionCount);
    }

    [Fact]
    public void AnInputKeepsTheRoundOfItsAdd()
    {
        using RecordedMap recorded = new();
        recorded.Drain();
        int addRound = recorded.Map.TokenManager!.TokenExecutor.Round;
        recorded.AddToPlayer(PlayerId.Player1, probe: recorded.Probe("late"));
        recorded.Drain();

        using JsonDocument input = JsonDocument.Parse(Assert.Single(recorded.EventLines()));
        Assert.Equal(addRound, input.RootElement.GetProperty("addRound").GetInt32());
        Assert.True(input.RootElement.GetProperty("round").GetInt32() >= addRound);
    }

    private static string Executed(string line)
    {
        using JsonDocument trace = JsonDocument.Parse(line);
        JsonElement root = trace.RootElement;
        return root.GetProperty("class").GetString() + ":" + root.GetProperty("result").GetString() + ":" + root.GetProperty("valid").GetBoolean().ToString();
    }

    private sealed class RefusingWriter : StringWriter
    {
        public override void WriteLine(string? value) => throw new IOException("disk full");
    }

    private static string Name(string line)
    {
        using JsonDocument input = JsonDocument.Parse(line);
        return input.RootElement.GetProperty("event").GetProperty("name").GetString()!;
    }

    private sealed class ProbeTokenEvent(CustomNode owner) : TokenEvent(owner)
    {
        public string Name { get; init; } = "";

        public Func<ProbeTokenEvent, TokenExecutor, ExecuteResult>? Run { get; init; }

        public int Executions { get; private set; }

        public override ExecuteResult Execute(TokenExecutor tokenEvent)
        {
            Executions += 1;
            return Run?.Invoke(this, tokenEvent) ?? ExecuteResult.Done;
        }
    }

    private sealed class RecordedMap : IDisposable
    {
        private readonly IDisposable _restore;
        private readonly StringWriter _events = new() { NewLine = "\n" };
        private readonly StringWriter _trace;

        internal StrategicMap Map { get; }

        internal TokenRecorder Recorder { get; }

        internal int Rounds { get; private set; }

        internal string TraceText => _trace.ToString();

        // A null capture names the probe. The default capture is the recorder's own, through the
        // persistence tree and the document backend.
        public RecordedMap(PlayerId? localSeat = null, bool isDefaultCapture = false, Func<TokenEvent, Func<FormatJson.JsonObject>>? capture = null, StringWriter? trace = null)
        {
            _trace = trace ?? new StringWriter { NewLine = "\n" };
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            _restore = EngineFixture.UseMap(Map);
            LocalPlayerCommunicationControl communication = new(EngineFixture.Engine);
            communication.Setup(Map.Players.Count);
            for (int i = 0; i < Map.Players.Count; i++)
            {
                communication.SetConnectionMode(mode: PlayerId.FromInt32(i) == localSeat ? ConnectionMode.Local : ConnectionMode.Global, index: PlayerId.FromInt32(i));
            }

            _ = Map.PlayerCommunicator.ActivatePlayerCommunicationControl(communication);
            communication.Release();
            Map.TokenManager!.Initialize(Map.PlayerCommunicator);
            Recorder = new TokenRecorder(Map, events: _events, trace: _trace, capture: isDefaultCapture ? null : capture ?? NameCapture);
        }

        // The probe's name and the executions it had when the recorder captured it.
        private static Func<FormatJson.JsonObject> NameCapture(TokenEvent tokenEvent)
        {
            string name = tokenEvent is ProbeTokenEvent probe ? probe.Name + (probe.Executions == 0 ? "" : "@" + probe.Executions.ToString(CultureInfo.InvariantCulture)) : tokenEvent.GetType().Name;
            return () => new FormatJson.JsonObject().Add("name", new FormatJson.JsonString(name));
        }

        internal void Add(PlayerId player, TokenEvent tokenEvent)
        {
            Map.TokenManager!.GetTokenControl(player)!.AddEvent(tokenEvent);
            _ = tokenEvent.Release();
        }

        // A one-field map, so an event that validates against a field finds one.
        internal void InstallField()
        {
            MapContainer container = new(null) { Holder = EngineFixture.Engine, Map = Map };
            MapLevel level = new(container);
            level.CreateMapFieldList(1, 1);
            container.Width = 1;
            container.Height = 1;
            container.LevelCount = 1;
            Map.MapContainer = container;
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 0x2000_0008, sender: Map);
            message.Payload = EngineFixture.Engine;
            level.MessageProc(ref message, sender: Map);
        }

        internal ProbeTokenEvent Probe(string name, Func<ProbeTokenEvent, TokenExecutor, ExecuteResult>? run = null)
            => new(null!) { Holder = EngineFixture.Engine, Name = name, Run = run };

        internal void AddToPlayer(PlayerId player, ProbeTokenEvent probe)
        {
            Map.TokenManager!.GetTokenControl(player)!.AddEvent(probe);
            _ = probe.Release();
        }

        // Each Process runs the system events or one round of token controls.
        internal void Drain()
        {
            for (int i = 0; i < 8; i++)
            {
                Map.TokenManager!.Process();
                Rounds += 1;
            }
        }

        // The recorder writes the input events when it stops.
        internal List<string> EventLines()
        {
            Recorder.Dispose();
            return Lines(_events);
        }

        internal List<string> TraceLines() => Lines(_trace);

        private static List<string> Lines(StringWriter writer) => [.. writer.ToString().Split('\n', options: StringSplitOptions.RemoveEmptyEntries)];

        public void Dispose()
        {
            Recorder.Dispose();
            _restore.Dispose();
            Map.Dispose();
        }
    }
}
