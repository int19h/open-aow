using Aow.Formats.Json;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Recording;

/// <summary>
/// Records the token event stream of one map (recordings.md 4, step 1 of issue 343). It writes
/// one line of minimal JSON to <c>trace.jsonl</c> after each executed event completes, and one
/// line to <c>events.jsonl</c> for each input event, in the order the inputs first executed, with
/// a marker line for each host action in that order.
/// </summary>
/// <remarks>
/// <para>
/// An input is an event added while the executor executes nothing. An event added while
/// <see cref="TokenExecutor.Executing"/> is true comes from the execution of the event that runs,
/// and is listed in that event's trace line as derived. The executor executes one event at a time
/// and never inside another's execution: only <see cref="TokenExecutor.Process"/> and the system
/// events execute, and both run from the frame. The recorder counts any nested execution in
/// <see cref="NestedCount"/> and marks its trace line.
/// </para>
/// <para>
/// The executor numbers events and adds the end of each local batch itself
/// (<see cref="TokenControl.PrefetchTokenEvents"/>), so those events have no add and are no input.
/// They are in the trace. The recorder reads the event, the executor's counters, the map's day and
/// both seeds, and changes none of them.
/// </para>
/// <para>
/// An input's document node comes from the tree its persistence writes when it first executes,
/// with its token counter back at -1, the value of an added event. The recorder maps the trees to
/// document nodes and writes <c>events.jsonl</c> when it stops, after the game, because the
/// document backend fills caches the first time it maps a class (<see cref="TokenEventDocument"/>).
/// Reflection and the tree writer run on an <see cref="IsolatedThread"/> for the same reason.
/// </para>
/// <para>
/// A fault of the recorder leaves the game state alone. Every handler catches it, the recorder
/// counts it in <see cref="FaultCount"/>, names the first in <see cref="Fault"/> and records
/// nothing more. A fault of the isolated work or of the recorder's own bookkeeping throws nothing
/// on the game thread. A fault of a writer does throw there, and the game thread's later identity
/// hashes, and with them later random draws, are then no longer those of a run without the
/// recorder. An input whose document node fails to map gets a line with a null <c>event</c> and
/// its <c>documentError</c>.
/// </para>
/// </remarks>
public sealed class TokenRecorder : IDisposable
{
    private readonly StrategicMap _map;
    private readonly TokenExecutor _executor;
    private readonly TextWriter _events;
    private readonly TextWriter _trace;
    private readonly Func<TokenEvent, Func<JsonObject>> _capture;
    private readonly List<PendingLine> _pending = [];
    private readonly Func<string?>? _site;
    private readonly IsolatedThread _isolated;
    // Lists found by reference, not tables: a table hashes the event, and the first identity hash
    // of an object takes the next value of the thread's hash sequence. The game reads identity
    // hashes as addresses (FrameFixtureInputs.AnimationOffset), so one more moves an
    // animation phase, a move sound and the global RandSeed it draws from. An input the game drops
    // without executing it stays in the list. The desktop recorder of step 5 bounds it.
    private readonly List<InputEvent> _inputs = [];
    private readonly List<Execution> _running = [];
    private readonly Stack<Execution> _executing = [];
    private readonly Stack<HostAction> _hostActions = [];
    private long _executionsBegun;
    private long _adds;
    private bool _isSubscribed;
    private bool _disposed;

    /// <param name="map">The map whose executor the recorder subscribes to.</param>
    /// <param name="events">Receives the input events, one line each.</param>
    /// <param name="trace">Receives the executed events, one line each.</param>
    /// <param name="capture">
    /// Takes an input event's state when it first executes and returns the writer of its document
    /// node, which runs when the recorder stops. Null captures the tree and maps it through the
    /// document backend (<see cref="TokenEventDocument"/>).
    /// </param>
    /// <param name="site">
    /// Names the code that adds an event, written as <c>site</c> on its line. Null writes no site.
    /// </param>
    public TokenRecorder(StrategicMap map, TextWriter events, TextWriter trace, Func<TokenEvent, Func<JsonObject>>? capture = null, Func<string?>? site = null)
    {
        _map = map;
        _executor = (map.TokenManager ?? throw new InvalidOperationException("The map has no token manager.")).TokenExecutor;
        _events = events;
        _trace = trace;
        GameEngine engine = map.GetEngine() ?? throw new InvalidOperationException("The map has no engine.");
        _capture = capture ?? (tokenEvent =>
        {
            ObjectNode tree = TokenEventDocument.Capture(tokenEvent, engine: engine);
            string className = TokenEventDocument.ClassName(tokenEvent);
            return () => TokenEventDocument.AsAdded(TokenEventDocument.Write(tree, className: className));
        });
        _site = site;
        _isolated = new IsolatedThread("Token recorder");
        TokenExecutor.TokenEventAdded += OnAdded;
        TokenExecutor.TokenEventExecuting += OnExecuting;
        TokenExecutor.TokenEventExecuted += OnExecuted;
        TokenExecutor.TokenEventValidated += OnValidated;
        _isSubscribed = true;
    }

    /// <summary>The input events recorded for <c>events.jsonl</c>.</summary>
    public int InputCount { get; private set; }

    /// <summary>The executed events written to <c>trace.jsonl</c>.</summary>
    public int ExecutedCount { get; private set; }

    /// <summary>The events added during an execution.</summary>
    public int DerivedCount { get; private set; }

    /// <summary>The executions that began inside another execution. The executor makes none.</summary>
    public int NestedCount { get; private set; }

    /// <summary>The host actions with a marker line in <c>events.jsonl</c>.</summary>
    public int HostActionCount { get; private set; }

    /// <summary>The faults of the recorder. After the first one it records nothing more.</summary>
    public int FaultCount { get; private set; }

    /// <summary>The first fault: where it happened, the exception type and its message.</summary>
    public string? Fault { get; private set; }

    /// <summary>
    /// Marks a host action until the scope ends: a directed input of the headless host that stands
    /// in for a player. The events added in the scope have the origin
    /// <see cref="TokenEventOrigin.Host"/>, even when the action runs inside the AI's processing.
    /// When the scope ends, a marker line goes into <c>events.jsonl</c> in order with the inputs,
    /// with the action, the executor's counter, round and the day at its start and the number of
    /// adds in the scope. An action that changes the game state outside the token stream passes
    /// <paramref name="changesState"/>, and its marker is written even with no add. Without it, an
    /// action with no add leaves no marker.
    /// </summary>
    public HostInputScope HostInput(string action, bool changesState)
    {
        _hostActions.Push(new HostAction(action, ChangesState: changesState, Counter: _executor.TokenCounter, Round: _executor.Round, Day: _map.Day, AddsAtStart: _adds));
        return new HostInputScope(this);
    }

    private void EndHostInput()
    {
        if (!_hostActions.TryPop(out HostAction? action) || !_isSubscribed)
        {
            return;
        }

        long adds = _adds - action.AddsAtStart;
        if (adds == 0 && !action.ChangesState)
        {
            return;
        }

        JsonObject line = new JsonObject()
            .Add("host", new JsonString(action.Name))
            .Add("counter", new JsonInteger(action.Counter))
            .Add("round", new JsonInteger(action.Round))
            .Add("day", new JsonInteger(action.Day))
            .Add("adds", new JsonInteger(adds));
        _pending.Add(new PendingLine(line, Document: null, Site: null));
        HostActionCount += 1;
    }

    // Provisional until replay (step 2) checks it per seat: an add is the AI's while the strategic
    // AI executor processes, or while the battle AI holds the turn of the add's seat. The battle AI
    // stays Done until its end of turn succeeds, so another seat's add in that window is human.
    private TokenEventOrigin Origin(PlayerId? player)
        => _hostActions.Count != 0 ? TokenEventOrigin.Host
            : _map.AIExecutor.Status.Contains(AIExecutorStatuses.Executing)
                || (_map.CombatMap is { AI.Status: BattleAI.AIStatus.Busy or BattleAI.AIStatus.Done } combat && (player is null || player == combat.CurrentPlayer))
            ? TokenEventOrigin.AI
            : TokenEventOrigin.Human;

    private static string OriginName(TokenEventOrigin origin) => origin switch
    {
        TokenEventOrigin.Human => "human",
        TokenEventOrigin.AI => "ai",
        TokenEventOrigin.Host => "host",
        _ => throw new ArgumentOutOfRangeException(nameof(origin)),
    };

    // A fault the recorder finds itself, or one from the isolated work: no exception on the game thread.
    private void Stop(string where, string error)
    {
        FaultCount += 1;
        Fault ??= $"{where}: {error}";
        Unsubscribe();
    }

    // A fault that throws on the game thread, a writer's.
    private void Record(string where, Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Stop(where, error: $"{exception.GetType().Name}: {exception.Message}");
        }
    }

    private T? Isolated<T>(string where, Func<T> work)
    {
        (T? result, string? error) = _isolated.Run(work);
        if (error is not null)
        {
            Stop(where, error: error);
        }

        return result;
    }

    private void Unsubscribe()
    {
        if (!_isSubscribed)
        {
            return;
        }

        _isSubscribed = false;
        TokenExecutor.TokenEventAdded -= OnAdded;
        TokenExecutor.TokenEventExecuting -= OnExecuting;
        TokenExecutor.TokenEventExecuted -= OnExecuted;
        TokenExecutor.TokenEventValidated -= OnValidated;
    }

    private void OnAdded(object? sender, TokenEventAddedEventArgs e)
    {
        if (ReferenceEquals(sender, _executor))
        {
            Record(where: "add", action: () => Added(e));
        }
    }

    private void OnExecuting(object? sender, TokenEventExecutingEventArgs e)
    {
        if (ReferenceEquals(sender, _executor))
        {
            Record(where: "execution start", action: () => Executing(e));
        }
    }

    private void OnValidated(object? sender, TokenEventValidatedEventArgs e)
    {
        if (ReferenceEquals(sender, _executor))
        {
            Record(where: "validation", action: () => Validated(e));
        }
    }

    private void OnExecuted(object? sender, TokenEventExecutedEventArgs e)
    {
        if (ReferenceEquals(sender, _executor))
        {
            Record(where: "execution end", action: () => Executed(e));
        }
    }

    private void Added(TokenEventAddedEventArgs e)
    {
        _adds += 1;
        string? site = _site?.Invoke();
        if (_executor.Executing)
        {
            DerivedCount += 1;
            if (_executing.TryPeek(out Execution? execution))
            {
                execution.Derived.Add(new DerivedEvent(EventType: e.TokenEvent.GetType(), Queue: e.Queue, Player: e.Player, Site: site));
            }
        }
        else
        {
            _ = _inputs.RemoveAll(input => ReferenceEquals(input.TokenEvent, e.TokenEvent));
            _inputs.Add(new InputEvent(TokenEvent: e.TokenEvent, Queue: e.Queue, Player: e.Player, Origin: Origin(e.Player), AddRound: _executor.Round, Site: site));
        }
    }

    private void Executing(TokenEventExecutingEventArgs e)
    {
        bool nested = _executor.Executing;
        if (nested)
        {
            NestedCount += 1;
        }

        Execution? execution = _running.Find(running => ReferenceEquals(running.TokenEvent, e.TokenEvent));
        if (execution is null)
        {
            int index = _inputs.FindIndex(input => ReferenceEquals(input.TokenEvent, e.TokenEvent));
            InputEvent? added = index < 0 ? null : _inputs[index];
            _executionsBegun += 1;
            execution = new Execution
            {
                TokenEvent = e.TokenEvent,
                EventType = e.TokenEvent.GetType(),
                Counter = e.TokenEvent.TokenCounter,
                Round = _executor.Round,
                Day = _map.Day,
                // The batch of a token control executes in the executing state. A system event does not.
                Seat = _executor.State.Contains(TokenExecutorStates.Executing) ? _executor.PrefetchTacticalBattle?.Player : null,
                IsInput = added is not null,
                Order = _executionsBegun,
            };
            _running.Add(execution);
            if (added is not null)
            {
                _inputs.RemoveAt(index);
                CaptureInput(execution, added);
                if (!_isSubscribed)
                {
                    return;
                }
            }
        }

        execution.Attempts += 1;
        execution.IsNested |= nested;
        _executing.Push(execution);
    }

    // A multiple move validates its moves and then executes them, and each move validates itself
    // again. The result of the event the executor runs is the one its trace line keeps.
    private void Validated(TokenEventValidatedEventArgs e)
    {
        if (_executing.TryPeek(out Execution? execution) && ReferenceEquals(execution.TokenEvent, e.TokenEvent))
        {
            execution.Validation = e.IsValid;
        }
    }

    private void Executed(TokenEventExecutedEventArgs e)
    {
        if (!_executing.TryPop(out Execution? execution) || !ReferenceEquals(execution.TokenEvent, e.TokenEvent))
        {
            Stop("execution end", error: "a token event ended an execution it did not begin");
            return;
        }

        if (e.Result is ExecuteResult.Done or ExecuteResult.DoneHalt)
        {
            int index = _running.FindIndex(running => ReferenceEquals(running, execution));
            _running.RemoveAt(index);
            WriteTrace(execution, result: e.Result == ExecuteResult.Done ? "done" : "halt");
        }
    }

    private void CaptureInput(Execution execution, InputEvent added)
    {
        (string ClassName, Func<JsonObject> Document)? captured = Isolated<(string, Func<JsonObject>)?>(where: "capture", work: () => (NameTable.Stored(execution.EventType), _capture(execution.TokenEvent)));
        if (captured is not { } input)
        {
            return;
        }

        JsonObject line = new JsonObject()
            .Add("seat", Seat(added.Player, queue: added.Queue))
            .Add("queue", new JsonString(QueueName(added.Queue)))
            .Add("origin", new JsonString(OriginName(added.Origin)))
            .Add("counter", new JsonInteger(execution.Counter))
            .Add("round", new JsonInteger(execution.Round))
            .Add("addRound", new JsonInteger(added.AddRound))
            .Add("day", new JsonInteger(execution.Day))
            .Add("class", new JsonString(input.ClassName));
        _pending.Add(new PendingLine(line, Document: input.Document, Site: added.Site));
        InputCount += 1;
    }

    // After the game, one line at a time: the document backend maps an input's tree and the line
    // is written. A tree that fails to map leaves a line with a null event and the error.
    private void WriteLines()
    {
        foreach (PendingLine pending in _pending)
        {
            if (pending.Document is { } document)
            {
                (JsonObject? mapped, string? error) = _isolated.Run(document);
                _ = pending.Line.Add("event", mapped ?? (JsonValue)new JsonNull());
                if (error is not null)
                {
                    FaultCount += 1;
                    Fault ??= $"event document: {error}";
                    _ = pending.Line.Add("documentError", new JsonString(error));
                }
            }

            if (pending.Site is { } site)
            {
                _ = pending.Line.Add("site", new JsonString(site));
            }

            Record(where: "events write", action: () => _events.WriteLine(JsonWriter.ToMinimalText(pending.Line)));
        }

        _pending.Clear();
    }

    private void WriteTrace(Execution execution, string result)
    {
        (string ClassName, string[] DerivedNames)? names = Isolated<(string, string[])?>(where: "trace names", work: () => (NameTable.Stored(execution.EventType), execution.Derived.Select(static item => NameTable.Stored(item.EventType)).ToArray()));
        if (names is not { } named)
        {
            return;
        }

        JsonArray derived = new();
        foreach ((DerivedEvent item, string derivedName) in execution.Derived.Zip(named.DerivedNames))
        {
            JsonObject entry = new JsonObject()
                .Add("class", new JsonString(derivedName))
                .Add("queue", new JsonString(QueueName(item.Queue)))
                .Add("seat", Seat(item.Player, queue: item.Queue));
            if (item.Site is { } site)
            {
                _ = entry.Add("site", new JsonString(site));
            }

            _ = derived.Add(entry);
        }

        JsonObject line = new JsonObject()
            .Add("counter", new JsonInteger(execution.Counter))
            .Add("round", new JsonInteger(execution.Round))
            .Add("day", new JsonInteger(execution.Day))
            .Add("seat", execution.Seat is { } seat ? new JsonInteger(seat.Value) : new JsonNull())
            .Add("class", new JsonString(named.ClassName))
            .Add("input", new JsonBoolean(execution.IsInput))
            .Add("result", new JsonString(result))
            .Add("attempts", new JsonInteger(execution.Attempts));
        if (execution.Validation is bool valid)
        {
            _ = line.Add("valid", new JsonBoolean(valid));
        }

        _ = line
            .Add("randSeed", new JsonInteger(DelphiRandom.RandSeed))
            .Add("defendersSeed", new JsonInteger(_map.DefendersSeed))
            .Add("derived", derived);
        if (execution.IsNested)
        {
            _ = line.Add("nested", new JsonBoolean(true));
        }

        _trace.WriteLine(JsonWriter.ToMinimalText(line));
        ExecutedCount += 1;
    }

    private static JsonValue Seat(PlayerId? player, TokenEventQueue queue)
        => queue == TokenEventQueue.Player && player is { } seat ? new JsonInteger(seat.Value) : new JsonNull();

    private static string QueueName(TokenEventQueue queue) => queue switch
    {
        TokenEventQueue.Player => "player",
        TokenEventQueue.System => "system",
        TokenEventQueue.Executor => "executor",
        _ => throw new ArgumentOutOfRangeException(nameof(queue)),
    };

    /// <summary>
    /// Stops recording and writes <c>events.jsonl</c>. An event still executing over several frames
    /// gets its trace line with the result <c>unfinished</c>, so a run that ends inside a move still
    /// shows it. A fault here is counted as any other and never thrown.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        bool isFaulted = FaultCount != 0;
        Unsubscribe();
        if (!isFaulted)
        {
            foreach (Execution execution in _running.OrderBy(static execution => execution.Order))
            {
                Record(where: "unfinished trace", action: () => WriteTrace(execution, result: "unfinished"));
            }
        }

        _running.Clear();
        _inputs.Clear();
        WriteLines();
        Record(where: "isolated thread", action: _isolated.Dispose);
        Record(where: "events flush", action: _events.Flush);
        Record(where: "trace flush", action: _trace.Flush);
    }

    /// <summary>The scope of <see cref="HostInput"/>.</summary>
    public readonly struct HostInputScope : IDisposable
    {
        private readonly TokenRecorder? _recorder;

        internal HostInputScope(TokenRecorder recorder) => _recorder = recorder;

        public void Dispose() => _recorder?.EndHostInput();
    }

    private sealed record HostAction(string Name, bool ChangesState, int Counter, int Round, int Day, long AddsAtStart);

    private sealed record InputEvent(TokenEvent TokenEvent, TokenEventQueue Queue, PlayerId? Player, TokenEventOrigin Origin, int AddRound, string? Site);

    // An input with the writer of its document node, or a host action's marker with none.
    private sealed record PendingLine(JsonObject Line, Func<JsonObject>? Document, string? Site);

    private sealed record DerivedEvent(Type EventType, TokenEventQueue Queue, PlayerId? Player, string? Site);

    // Mutable: an event that returns Retry executes again on later frames, and its attempts and
    // the events it derives add up until it completes.
    private sealed class Execution
    {
        public required TokenEvent TokenEvent { get; init; }

        public required Type EventType { get; init; }

        public required int Counter { get; init; }

        public required int Round { get; init; }

        public required int Day { get; init; }

        public required PlayerId? Seat { get; init; }

        public required bool IsInput { get; init; }

        public required long Order { get; init; }

        public List<DerivedEvent> Derived { get; } = [];

        public int Attempts { get; set; }

        public bool IsNested { get; set; }

        public bool? Validation { get; set; }
    }
}
