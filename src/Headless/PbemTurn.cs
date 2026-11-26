using System.Globalization;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Headless;

/// <summary>
/// One human turn of a play by email game (issue 344, package 4). The loaded turn file names the
/// local seat: its current player, as TPBEMPlayerControl.ValidateLoadedMap takes it
/// (PlayerC.PAS:1136). On the first frame the seat is seated the host saves the loaded state,
/// gives the seat's directed order if the run has one, and then asks before each frame to end
/// the seat's turn the way the end turn button does (TAoWHSMap.EndTurn). The send dialog for the
/// next seat belongs to the host. It replaces the dialog, writes the game state with AutoSave at
/// the send event and ends the run. pbem_chain.py states what the dialog's modes do instead. The
/// native harness stops at the same points (playtest.py run-pbem).
/// </summary>
internal sealed class PbemTurn(StrategicMap map, PbemOrder? order)
{
    private readonly PlayerId? _seat = map.CurrentPlayer;

    private readonly EmailGameFacts _loadedEmailGame = EmailGameFacts.Of(map, seat: map.CurrentPlayer);

    private EmailGameFacts? _seatedEmailGame;

    private int? _seatedPlayer;

    private readonly List<string> _eventLogs = [];

    public SideWindowsLayout Layout { get; } = new();

    /// <summary>What the directed order found and asked for, or null without an order.</summary>
    public object? OrderRecord { get; private set; }

    public long? LoadedFrame { get; private set; }

    public long? EndTurnFrame { get; private set; }

    public int EndTurnRequests { get; private set; }

    public bool IsHandedOff { get; private set; }

    public int? HandoffPlayer { get; private set; }

    public int? HandoffMode { get; private set; }

    public int? HandoffDay { get; private set; }

    /// <summary>
    /// Why the host refuses --pbem for a loaded game, or null. Activation sets up the PBEM control
    /// only for an email game played in turns (TAoWHSMap.SetupPlayerControl, AoWHSMap.PAS:2621),
    /// and only a human current player raises the send dialog that ends the run. A control the
    /// game already holds is that control. Any other game never reaches the send dialog, and the
    /// run has no end.
    /// </summary>
    public static string? Refusal(PlayerConnectionType connection, PlayMode mode, Type? control, PlayerType? current)
    {
        return connection != PlayerConnectionType.Email ? "--pbem needs an email game"
            : mode is not PlayMode.Turn and not PlayMode.None ? "--pbem needs a game played in turns"
            : control is not null && control != typeof(PbemPlayerControl) ? "--pbem needs the play by email player control"
            : current != PlayerType.Human ? "--pbem needs a human current player"
            : null;
    }

    /// <summary>
    /// Before a frame: while the local seat is current and seated and its turn has not ended,
    /// save the loaded state once, give the directed order once, and request the end of the turn.
    /// The request is refused while event logs wait or the seat is busy, a move or a combat
    /// included, and the host asks again on the next frame.
    /// </summary>
    public void BeforeFrame(long frame, Action saveLoaded)
    {
        if (EndTurnFrame is not null || map.CurrentPlayer != _seat || map.SeatedPlayer != _seat)
        {
            return;
        }

        if (LoadedFrame is null)
        {
            LoadedFrame = frame;
            _seatedPlayer = PlayerIdExtensions.ToOriginal(map.SeatedPlayer);
            _seatedEmailGame = EmailGameFacts.Of(map, _seat);
            saveLoaded();
            if (order is { } given)
            {
                OrderRecord = Attack(given);
            }
        }

        EndTurnRequests++;
        if (map.EndTurn())
        {
            EndTurnFrame = frame;
        }
    }

    /// <summary>
    /// A queued event log the host closes, by class, as the native harness records it. The
    /// combat type dialog of the seat's combat closes with fast combat, the dialog's FC button
    /// (TTCFCDialog.FCchkDoubleClick, Select(1)). Closed with no choice, the move leaves the
    /// combat out.
    /// </summary>
    public void EventLogShown(EventLog log)
    {
        if (log is CombatTypeRequestEventLog request)
        {
            request.Select(CombatType.Fast);
        }

        _eventLogs.Add(log.GetType().Name);
    }

    /// <summary>
    /// The send dialog of TPBEMPlayerControl.NewTurn (PlayerC.PAS:984) for the next seat: save
    /// the turn file and end the run. Any other event log returns false.
    /// </summary>
    public bool TryHandOff(EventLog log, Action saveHandoff)
    {
        if (log is not SendEmailGameEventLog send)
        {
            return false;
        }

        HandoffPlayer = PlayerIdExtensions.ToOriginal(send.SendPlayer);
        HandoffMode = send.Mode;
        HandoffDay = map.Day;
        saveHandoff();
        IsHandedOff = true;
        return true;
    }

    // MoveT.PAS:3181 MoveArmyXYL, the map UI's move order, with the selection bits of every slot
    // of the ordering army. It plans the path and queues the move token, an attack when the
    // target is in reach. The combat then runs with FRequestCombatType set, as every combat of an
    // email game does (TTurnPlayerControl.DefaultStartMap, PlayerC.PAS:761).
    private object Attack(PbemOrder given)
    {
        if (map.UnitControl?.FindUnit(given.UnitId) is not Troop unit
            || map.UnitControl.FindUnit(given.TargetUnitId) is not Troop target
            || ArmyOf(unit) is not Army army
            || ArmyOf(target) is not Army targetArmy)
        {
            return new { unit = given.UnitId, target_unit = given.TargetUnitId, found = false, moved = false };
        }

        ArmyHexSprite? sprite = army.GetArmyHexSprite();
        MapCoordinate here = army.Location;
        MapCoordinate there = targetArmy.Location;
        int count = army.Units.Count;
        bool moved = sprite is not null && count <= 8
            && ArmyHexSprite.MoveArmyMapCoordinate(sprite, selection: (1 << count) - 1, there.X, there.Y, L: there.MapLevel, force: false);
        return new
        {
            unit = given.UnitId,
            target_unit = given.TargetUnitId,
            found = true,
            player = PlayerIdExtensions.ToOriginal(army.Player),
            army_units = count,
            from = new[] { here.X, here.Y, here.MapLevel },
            to = new[] { there.X, there.Y, there.MapLevel },
            target_player = PlayerIdExtensions.ToOriginal(targetArmy.Player),
            target_units = targetArmy.Units.Count,
            request_combat_type = map.RequestCombatType,
            moved
        };
    }

    // The army in the owner's army list whose units hold the unit, as the native driver finds it.
    private Army? ArmyOf(Troop troop)
    {
        if (map.Players.GetPlayers(troop.Player)?.Armies is not ArmyCollection armies)
        {
            return null;
        }

        for (int i = 0; i < armies.Count; i++)
        {
            Army army = armies.GetArmy(i);
            if (army.Units.IndexOf(troop) >= 0)
            {
                return army;
            }
        }

        return null;
    }

    public object Record => new
    {
        seat = PlayerIdExtensions.ToOriginal(_seat),
        control = map.PlayerControl.GetType().Name,
        connection = map.ConnectionType.ToString(),
        seated_player = _seatedPlayer,
        email_game_loaded = _loadedEmailGame,
        email_game_seated = _seatedEmailGame,
        order = OrderRecord,
        event_logs = _eventLogs,
        loaded_frame = LoadedFrame,
        end_turn_frame = EndTurnFrame,
        end_turn_requests = EndTurnRequests,
        handed_off = IsHandedOff,
        handoff_player = HandoffPlayer,
        handoff_mode = HandoffMode,
        handoff_day = HandoffDay,
    };
}

/// <summary>
/// The seat's email game as the game holds it: the ids ValidateLoadedMap and CreateEmailGame set
/// and the last day only StartLoadedEmailGame sets (PlayerC.PAS:1094). The turn file does not
/// store the last day (Diplomacy.pas:2465).
/// </summary>
internal sealed record EmailGameFacts(int UniqueLocalId, int UniqueId, int SecurityId, int LastDay, string Status)
{
    public static EmailGameFacts Of(StrategicMap map, PlayerId? seat)
    {
        PbemPlayerSettings settings = map.Players.GetPlayers(seat)?.PbemSettings ?? throw new InvalidOperationException("The seat has no email settings.");
        EmailGame game = settings.EmailGame;
        return new(game.UniqueLocalId, UniqueId: game.UniqueId, SecurityId: game.SecurityId, LastDay: game.LastDay, Status: settings.Status.ToString());
    }
}

/// <summary>
/// The native map window of a loaded game shows its side windows when a human is seated
/// (General.ShowWindows in TMapEvents.TheMapSeatedPlayerChanged), and its layout takes them on
/// the second frame after, if the seat is still seated then. Once taken they stay, also after
/// the seat leaves. Until then the map view is wider by the side windows. The view size decides
/// the seat's saved view centre and where the view clamps when it centres. The timing is the
/// native window's, measured by playtest.py run-pbem at each save and each seated frame, and
/// holds for the headless host only.
/// </summary>
internal sealed class SideWindowsLayout
{
    public const int SideWindowsWidth = 96;

    private const int LayoutFrames = 2;

    private long? _firstSeatedFrame;

    private bool _isNarrow;

    /// <summary>Notes, before a frame, whether a human is seated, and takes the side windows when due.</summary>
    public void Observe(long frame, bool isHumanSeated)
    {
        if (_isNarrow)
        {
            return;
        }

        if (!isHumanSeated)
        {
            _firstSeatedFrame = null;
            return;
        }

        _firstSeatedFrame ??= frame;
        _isNarrow = frame - _firstSeatedFrame >= LayoutFrames;
    }

    /// <summary>The map view width, from the width with the side windows shown.</summary>
    public int Width(int width) => _isNarrow ? width : width + SideWindowsWidth;
}

/// <summary>A human seat's directed order: the army of a unit moves against the army of another unit.</summary>
internal readonly record struct PbemOrder(int UnitId, int TargetUnitId)
{
    public static PbemOrder Parse(string text)
    {
        string[] parts = text.Split(':');
        return parts.Length == 2
            && int.TryParse(parts[0], style: NumberStyles.None, provider: CultureInfo.InvariantCulture, result: out int unitId)
            && int.TryParse(parts[1], style: NumberStyles.None, provider: CultureInfo.InvariantCulture, result: out int targetUnitId)
            && unitId > 0
            && targetUnitId > 0
            && unitId != targetUnitId
            ? new PbemOrder(unitId, TargetUnitId: targetUnitId)
            : throw new ArgumentException("--pbem-attack takes UNIT:TARGET");
    }
}
