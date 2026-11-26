#nullable disable warnings
// Translated from CMapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.PointerMemory;

namespace Aow.Game.UI.Grids;

// event methods occupy
public sealed class CombatMapControl : Component
{
    private readonly GameEngine? _engine;

    // The maps the control's relays are linked to, and the players at link time (issue 326).
    private CombatMap? _linkedCombatMap;
    private StrategicMap? _linkedWorld;
    private List<Player> _linkedPlayers = [];

    private static void ShowMessageLogged(string text)
    {
        HostLog.CatchDialogAtCallSite(RtlHost.Logger, text: text);
        LegacyApplication.ShowMessage(text);
    }

    [Published]
    public event SelectHSEvent? Selected;
    [Published]
    public event SelectHSEvent? Deselected;
    [Published]
    public event EventHandler? HexSpriteMapLoaded;
    [Published]
    public event EventHandler? HexSpriteMapClosing;
    [Published]
    public event EventHandler<PlayerEventArgs>? IncomeChanged;
    [Published]
    public event EventHandler<PlayerEventArgs>? UpkeepChanged;
    [Published]
    public event EventHandler<PlayerEventArgs>? GemsChanged;
    [Published]
    public event ShowMessageEvent? ShowingMessage;
    [Published]
    public event TurnChangedEvent? TurnChanged;
    [Published]
    public event EventHandler<PlayerEventArgs>? PlayerChanged;
    [Published]
    public event PlayerChangedEvent? SeatedPlayerChanged;
    [Published]
    public event EventHandler<ExecutingEventLogEventArgs>? ExecutingEventLog;

    internal void ClearExecuteEventLogHandler() => ExecutingEventLog = null;

    [Published]
    public event MapChangedEvent? MapChanged;
    [Published]
    public event EventHandler<PlayerEventArgs>? LogbookChanged;
    [Published]
    public event NotifyEvent<object>? BattleStarted;
    [Published]
    public event NotifyEvent<object>? BattleEnding;
    [Published]
    public event SelectHSEvent? MouseOverTargetChanged;
    [Published]
    public event SelectCombatUnitEvent? UnitChanged;
    [Published]
    public event ActivateCombatAbilityEvent? AbilityActivated;

    // CMapCtrl.pas:117
    public override void Dispose()
    {
        if (_engine is not null)
        {
            _engine.HexSpriteMapLoaded -= engine_HexSpriteMapLoaded;
            _engine.HexSpriteMapClosing -= engine_HexSpriteMapClosing;
            ReleaseHsm();
        }

        base.Dispose();
    }

    // CMapCtrl.pas:107
    public CombatMapControl(Component owner) : base(owner)
    {
        // The engine of the dialog module that owns the control, as the original read the
        // engine global.
        _engine = (owner as UIModule)?.UI.Engine;
        if (_engine is not null)
        {
            _engine.HexSpriteMapLoaded += engine_HexSpriteMapLoaded;
            _engine.HexSpriteMapClosing += engine_HexSpriteMapClosing;
        }
    }

    // CMapCtrl.pas:128. The original links on each loaded raise of a combat map and releases only
    // on a closing raise, which the end of a battle never sends: ReleaseCombatMap raises
    // CombatMapDestroyed and frees the map (AoWHSMap.PAS:2891), so each battle left one more copy
    // of the relays on the event log and the player events. The port remembers what it linked,
    // linking it again changes nothing, and the end of the battle releases the links (issue 326,
    // the at-most-one-handler check).
    public void LinkToHsm(CombatMap combat, StrategicMap world)
    {
        if (_linkedCombatMap == combat && _linkedWorld == world)
        {
            return;
        }

        ReleaseHsm();
        _linkedCombatMap = combat;
        _linkedWorld = world;
        _linkedPlayers = [.. Enumerable.Range(0, world.Players.Count).Select(world.Players.GetPlayers)];
        world.CombatMapDestroyed += world_CombatMapDestroyed;
        combat.TurnChanged += ChangeTurn;
        combat.PlayerChanged += CombatMap_PlayerChanged;
        combat.SeatedPlayerChanged += CombatMap_SeatedPlayerChanged;
        combat.Selection.Selected += SelectHexSprite;
        combat.Selection.Deselected += DeselectHexSprite;
        combat.ShowingMessage += CombatMap_ShowingMessage;
        combat.MapChanged += CombatMap_MapChanged;
        combat.BattleStarted += CombatMap_Deselected;
        combat.BattleEnding += DeactivateMap;
        combat.MouseOverTargetChanged += CombatMap_MouseOverTarget;
        combat.UnitChanged += CombatMap_UnitChanged;
        combat.AbilityActivated += CombatMap_AbilityActivated;
        world.ExecutingEventLog += StrategicMap_ExecutingEventLog;
        for (int i = 0, last = _linkedPlayers.Count - 1; i <= last; i++)
        {
            _linkedPlayers[i].GemsChanged += Player_GemsChanged;
            _linkedPlayers[i].IncomeChanged += Player_IncomeChanged;
            _linkedPlayers[i].UpkeepChanged += Player_UpkeepChanged;
            _linkedPlayers[i].LogbookChanged += Player_LogbookChanged;
        }
    }

    // CMapCtrl.pas:154
    public void ReleaseHsm()
    {
        if (_linkedCombatMap is not CombatMap combat || _linkedWorld is not StrategicMap world)
        {
            return;
        }

        _linkedCombatMap = null;
        _linkedWorld = null;
        world.CombatMapDestroyed -= world_CombatMapDestroyed;
        combat.TurnChanged -= ChangeTurn;
        combat.PlayerChanged -= CombatMap_PlayerChanged;
        combat.SeatedPlayerChanged -= CombatMap_SeatedPlayerChanged;
        combat.Selection.Selected -= SelectHexSprite;
        combat.Selection.Deselected -= DeselectHexSprite;
        combat.ShowingMessage -= CombatMap_ShowingMessage;
        combat.MapChanged -= CombatMap_MapChanged;
        world.ExecutingEventLog -= StrategicMap_ExecutingEventLog;
        combat.BattleStarted -= CombatMap_Deselected;
        combat.BattleEnding -= DeactivateMap;
        combat.MouseOverTargetChanged -= CombatMap_MouseOverTarget;
        combat.UnitChanged -= CombatMap_UnitChanged;
        combat.AbilityActivated -= CombatMap_AbilityActivated;
        for (int i = 0, last = _linkedPlayers.Count - 1; i <= last; i++)
        {
            _linkedPlayers[i].GemsChanged -= Player_GemsChanged;
            _linkedPlayers[i].IncomeChanged -= Player_IncomeChanged;
            _linkedPlayers[i].UpkeepChanged -= Player_UpkeepChanged;
            _linkedPlayers[i].LogbookChanged -= Player_LogbookChanged;
        }

        _linkedPlayers = [];
    }

    // AoWHSMap.PAS:2891. The end of a battle raises CombatMapDestroyed before the combat map goes,
    // and the control releases its links there.
    private void world_CombatMapDestroyed(object sender) => ReleaseHsm();

    // CMapCtrl.pas:180
    public void engine_HexSpriteMapLoaded(GameMap sender)
    {
        if (sender.EngineClass.ClassId == CombatMap.ClassId)
        {
            LinkToHsm((CombatMap)sender, world: sender.GetEngine().StrategicMap);
            HexSpriteMapLoaded?.Invoke(this, e: EventArgs.Empty);
        }
    }

    // CMapCtrl.pas:189
    public void engine_HexSpriteMapClosing(GameMap sender)
    {
        if (sender.EngineClass.ClassId == CombatMap.ClassId)
        {
            HexSpriteMapClosing?.Invoke(this, e: EventArgs.Empty);

            ReleaseHsm();
        }
    }

    // CMapCtrl.pas:207
    public void SelectHexSprite(object sender)
    {
        try
        {
            Selected?.Invoke(CombatMap.Current.Selection.EventHexSprite);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnSelect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:216
    public void DeselectHexSprite(object sender)
    {
        try
        {
            Deselected?.Invoke(CombatMap.Current.Selection.EventHexSprite);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnDeselect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:225
    public void CombatMap_MouseOverTarget(object sender)
    {
        try
        {
            MouseOverTargetChanged?.Invoke(CombatMap.Current.UnitSelection.MouseOverTarget);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnMouseOverTarget event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:234
    public void CombatMap_UnitChanged(object sender)
    {
        try
        {
            if (UnitChanged is not null)
            {
                TacticalBattleUnitHexSprite? tacticalBattleUnitHexSprite = CombatMap.Current.UnitSelection.TacticalBattleUnitHexSprite;
                UnitChanged(tacticalBattleUnitHexSprite);
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during UnitChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:243
    public void CombatMap_PlayerChanged(object sender)
    {
        try
        {
            PlayerChanged?.Invoke(this, e: new PlayerEventArgs((sbyte)PlayerIdExtensions.ToOriginal(CombatMap.Current.CurrentPlayer)));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:252
    public void CombatMap_SeatedPlayerChanged(sbyte player)
    {
        try
        {
            SeatedPlayerChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(CombatMap.Current.SeatedPlayer));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:261
    public void Player_GemsChanged(Player sender)
    {
        try
        {
            GemsChanged?.Invoke(this, e: new PlayerEventArgs((sbyte)PlayerIdExtensions.ToOriginal(sender.PlayerIndex)));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Gems Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:270
    public void Player_IncomeChanged(Player sender)
    {
        try
        {
            IncomeChanged?.Invoke(this, e: new PlayerEventArgs((sbyte)PlayerIdExtensions.ToOriginal(sender.PlayerIndex)));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Income Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:279
    public void Player_UpkeepChanged(Player sender)
    {
        try
        {
            UpkeepChanged?.Invoke(this, e: new PlayerEventArgs((sbyte)PlayerIdExtensions.ToOriginal(sender.PlayerIndex)));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Upkeep Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:297
    public void ChangeTurn(sbyte player)
    {
        try
        {
            // { The original passes the seated player as the turn number. }
            TurnChanged?.Invoke(PlayerIdExtensions.ToOriginal(CombatMap.Current.SeatedPlayer));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during ChangeTurn event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:307
    public void Player_LogbookChanged(Player sender)
    {
        try
        {
            LogbookChanged?.Invoke(this, e: new PlayerEventArgs((sbyte)PlayerIdExtensions.ToOriginal(sender.PlayerIndex)));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during LogbookChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:325
    public void CombatMap_Deselected(object sender)
    {
        try
        {
            BattleStarted?.Invoke(this);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnActivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:334
    public void DeactivateMap(object sender)
    {
        try
        {
            BattleEnding?.Invoke(this);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnDeactivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:352
    public void CombatMap_AbilityActivated(AbilityActivation sender)
    {
        try
        {
            AbilityActivated?.Invoke(sender);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnActivateAbility event (TAoWCombatMapControl)");
        }
    }

    public void StrategicMap_ExecutingEventLog(object sender)
    {
        try
        {
            if (ExecutingEventLog is null)
            {
                return;
            }

            ExecuteEventLogInfo info = sender is ExecuteEventLogNotification notification
                ? notification.Info : ReadLegacyRecord<ExecuteEventLogInfo>(sender);
            ExecutingEventLog(this, e: new ExecutingEventLogEventArgs(info.Log, done: info.Done));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:198. The notification carries the changed map area.
    public void CombatMap_MapChanged(object sender)
    {
        try
        {
            if (MapChanged is not null)
            {
                MapChanged(((MapChangedNotification)sender).Info);
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }

    public void CombatMap_ShowingMessage(object sender)
    {
        try
        {
            ShowingMessage?.Invoke(((ShowMessageNotification)sender).Info);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during ShowMessage event(TAoWCombatMapControl)");
        }
    }

    public delegate void SelectHSEvent(HexSprite? HS);
    public delegate void SelectCombatUnitEvent(TacticalBattleUnitHexSprite? HS);
    public delegate void PlayerChangedEvent(sbyte Player);
    public delegate void ShowMessageEvent(in ShowMessageInfo Info);
    public delegate void TurnChangedEvent(int Turn);
    public delegate void MapChangedEvent(in MapChangedInfo ChangedArea);
    public delegate void ActivateCombatAbilityEvent(AbilityActivation AAI);
}
