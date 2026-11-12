// Translated from CMapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Grids;

// event methods occupy
public partial class CombatMapControl : Component
{
    private SelectHSEvent _onSelected;
    private SelectHSEvent _onDeselected;
    private NotifyEvent<object> _onActivateHsm;
    private NotifyEvent<object> _onDeactivateHsm;
    private PlayerChangedEvent _onIncomeChanged;
    private PlayerChangedEvent _onUpkeepChanged;
    private PlayerChangedEvent _onGemsChanged;
    private ShowMessageEvent _onShowMessage;
    private TurnChangedEvent _onTurnChanged;
    private PlayerChangedEvent _onPlayerChanged;
    private PlayerChangedEvent _onSeatedPlayerChanged;
    private OnExecuteEventLogEvent _onExecuteEventLog;
    private MapChangedEvent _onMapChanged;
    private PlayerChangedEvent _onLogbookChanged;
    private NotifyEvent<object> _onActivateMap;
    private NotifyEvent<object> _onDeActivateMap;
    private SelectHSEvent _onMouseOverTarget;
    private SelectCombatUnitEvent _onUnitChanged;
    private CastCombatSpellEvent _onCastSpell;
    private ActivateCombatAbilityEvent _onActivateAbility;

    // CMapCtrl.pas:117
    public override void Dispose()
    {
        if (GameTables.GameEngine != null)
        {
            GameTables.GameEngine.OnHsmLoaded -= ActivateHsm;
            GameTables.GameEngine.OnHsmClose -= DeactivateHsm;
            if (CombatMap.Current != null)
            {
                ReleaseHsm();
            }
        }

        base.Dispose();
    }

    // CMapCtrl.pas:107
    public CombatMapControl(Component owner) : base(owner)
    {
        if (GameTables.ValidateGameEngine())
        {
            GameTables.GameEngine.OnHsmLoaded += ActivateHsm;
            GameTables.GameEngine.OnHsmClose += DeactivateHsm;
        }
    }

    // CMapCtrl.pas:128
    public void LinkToHsm()
    {
        CombatMap.Current.FChangeTurnEvents += ChangeTurn;
        CombatMap.Current.FPlayerChangedEvents += PlayerChanged;
        CombatMap.Current.FSeatedPlayerChangedEvents += SeatedPlayerChanged;
        CombatMap.Current.Selection.OnSelect += SelectHexSprite;
        CombatMap.Current.Selection.OnDeselect += DeselectHexSprite;
        CombatMap.Current.FShowMessageEvents += ShowMessage;
        CombatMap.Current.FMapChangedEvents += MapChanged;
        CombatMap.Current.FDeselectEvents += ActivateMap;
        CombatMap.Current.FSelectEvents += DeactivateMap;
        CombatMap.Current.FMouseOverTargetEvents += MouseOverTarget;
        CombatMap.Current.FUnitChangedEvents += UnitChanged;
        CombatMap.Current.FAbilityActivatedEvents += ActivateAbility;
        CombatMap.Current.FCombatStartedEvents += CastSpell;
        StrategicMap.Current.FExecuteEventLogEvents += ExecuteEventLog;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            StrategicMap.Current.Players.GetPlayers(I).FGemsChangedEvents += GemsChanged;
            StrategicMap.Current.Players.GetPlayers(I).FIncomeChangedEvents += IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(I).FUpkeepChangedEvents += UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(I).FLogbookChangedEvents += LogbookChanged;
        }
    }

    // CMapCtrl.pas:154
    public void ReleaseHsm()
    {
        CombatMap.Current.FChangeTurnEvents -= ChangeTurn;
        CombatMap.Current.FPlayerChangedEvents -= PlayerChanged;
        CombatMap.Current.FSeatedPlayerChangedEvents -= SeatedPlayerChanged;
        CombatMap.Current.Selection.OnSelect -= SelectHexSprite;
        CombatMap.Current.Selection.OnDeselect -= DeselectHexSprite;
        CombatMap.Current.FShowMessageEvents -= ShowMessage;
        CombatMap.Current.FMapChangedEvents -= MapChanged;
        StrategicMap.Current.FExecuteEventLogEvents -= ExecuteEventLog;
        CombatMap.Current.FDeselectEvents -= ActivateMap;
        CombatMap.Current.FSelectEvents -= DeactivateMap;
        CombatMap.Current.FMouseOverTargetEvents -= MouseOverTarget;
        CombatMap.Current.FUnitChangedEvents -= UnitChanged;
        CombatMap.Current.FAbilityActivatedEvents -= ActivateAbility;
        CombatMap.Current.FCombatStartedEvents -= CastSpell;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            StrategicMap.Current.Players.GetPlayers(I).FGemsChangedEvents -= GemsChanged;
            StrategicMap.Current.Players.GetPlayers(I).FIncomeChangedEvents -= IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(I).FUpkeepChangedEvents -= UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(I).FLogbookChangedEvents -= LogbookChanged;
        }
    }

    // CMapCtrl.pas:180
    public void ActivateHsm(GameMap sender)
    {
        if (sender.EngineClass.ClassId == 2228480)
        {
            LinkToHsm();
            if (_onActivateHsm != null)
            {
                _onActivateHsm(this);
            }
        }
    }

    // CMapCtrl.pas:189
    public void DeactivateHsm(GameMap sender)
    {
        if (sender.EngineClass.ClassId == 2228480)
        {
            if (_onDeactivateHsm != null)
            {
                _onDeactivateHsm(this);
            }

            ReleaseHsm();
        }
    }

    // CMapCtrl.pas:207
    public void SelectHexSprite(object sender)
    {
        try
        {
            if (_onSelected != null)
            {
                _onSelected(CombatMap.Current.Selection.EventHexSprite);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnSelect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:216
    public void DeselectHexSprite(object sender)
    {
        try
        {
            if (_onDeselected != null)
            {
                _onDeselected(CombatMap.Current.Selection.EventHexSprite);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeselect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:225
    public void MouseOverTarget(object sender)
    {
        try
        {
            if (_onMouseOverTarget != null)
            {
                _onMouseOverTarget(CombatMap.Current.UnitSelection.MouseOverTarget);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnMouseOverTarget event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:234
    public void UnitChanged(object sender)
    {
        try
        {
            if (_onUnitChanged != null)
            {
                var tacticalBattleUnitHexSprite = CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite();
                _onUnitChanged(tacticalBattleUnitHexSprite);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during UnitChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:243
    public void PlayerChanged(object sender)
    {
        try
        {
            if (_onPlayerChanged != null)
            {
                _onPlayerChanged(CombatMap.Current.CurrentPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:252
    public void SeatedPlayerChanged(sbyte player)
    {
        try
        {
            if (_onSeatedPlayerChanged != null)
            {
                _onSeatedPlayerChanged(CombatMap.Current.SeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:261
    public void GemsChanged(Player sender)
    {
        try
        {
            if (_onGemsChanged != null)
            {
                _onGemsChanged(sender.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Gems Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:270
    public void IncomeChanged(Player sender)
    {
        try
        {
            if (_onIncomeChanged != null)
            {
                _onIncomeChanged(sender.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Income Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:279
    public void UpkeepChanged(Player sender)
    {
        try
        {
            if (_onUpkeepChanged != null)
            {
                _onUpkeepChanged(sender.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Upkeep Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:297
    public void ChangeTurn(sbyte player)
    {
        try
        {
            // { The original passes the seated player as the turn number. }
            if (_onTurnChanged != null)
            {
                _onTurnChanged(CombatMap.Current.SeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ChangeTurn event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:307
    public void LogbookChanged(Player sender)
    {
        try
        {
            if (_onLogbookChanged != null)
            {
                _onLogbookChanged(sender.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during LogbookChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:325
    public void ActivateMap(object sender)
    {
        try
        {
            if (_onActivateMap != null)
            {
                _onActivateMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:334
    public void DeactivateMap(object sender)
    {
        try
        {
            if (_onDeActivateMap != null)
            {
                _onDeActivateMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeactivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:343
    public void CastSpell(CombatSpellCaster sender)
    {
        try
        {
            if (_onCastSpell != null)
            {
                _onCastSpell(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnCastSpell event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:352
    public void ActivateAbility(AbilityActivation sender)
    {
        try
        {
            if (_onActivateAbility != null)
            {
                _onActivateAbility(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWCombatMapControl)");
        }
    }

    [Published]
    public SelectHSEvent OnSelected { get => _onSelected; set => _onSelected = value; }

    [Published]
    public SelectHSEvent OnDeselected { get => _onDeselected; set => _onDeselected = value; }

    [Published]
    public NotifyEvent<object> OnActivateHsm { get => _onActivateHsm; set => _onActivateHsm = value; }

    [Published]
    public NotifyEvent<object> OnDeactivateHsm { get => _onDeactivateHsm; set => _onDeactivateHsm = value; }

    [Published]
    public PlayerChangedEvent OnIncomeChanged { get => _onIncomeChanged; set => _onIncomeChanged = value; }

    [Published]
    public PlayerChangedEvent OnUpkeepChanged { get => _onUpkeepChanged; set => _onUpkeepChanged = value; }

    [Published]
    public PlayerChangedEvent OnGemsChanged { get => _onGemsChanged; set => _onGemsChanged = value; }

    [Published]
    public ShowMessageEvent OnShowMessage { get => _onShowMessage; set => _onShowMessage = value; }

    [Published]
    public TurnChangedEvent OnTurnChanged { get => _onTurnChanged; set => _onTurnChanged = value; }

    [Published]
    public PlayerChangedEvent OnPlayerChanged { get => _onPlayerChanged; set => _onPlayerChanged = value; }

    [Published]
    public PlayerChangedEvent OnSeatedPlayerChanged { get => _onSeatedPlayerChanged; set => _onSeatedPlayerChanged = value; }

    [Published]
    public OnExecuteEventLogEvent OnExecuteEventLog { get => _onExecuteEventLog; set => _onExecuteEventLog = value; }

    [Published]
    public MapChangedEvent OnMapChanged { get => _onMapChanged; set => _onMapChanged = value; }

    [Published]
    public PlayerChangedEvent OnLogbookChanged { get => _onLogbookChanged; set => _onLogbookChanged = value; }

    [Published]
    public NotifyEvent<object> OnActivateMap { get => _onActivateMap; set => _onActivateMap = value; }

    [Published]
    public NotifyEvent<object> OnDeActivateMap { get => _onDeActivateMap; set => _onDeActivateMap = value; }

    [Published]
    public SelectHSEvent OnMouseOverTarget { get => _onMouseOverTarget; set => _onMouseOverTarget = value; }

    [Published]
    public SelectCombatUnitEvent OnUnitChanged { get => _onUnitChanged; set => _onUnitChanged = value; }

    [Published]
    public CastCombatSpellEvent OnCastSpell { get => _onCastSpell; set => _onCastSpell = value; }

    [Published]
    public ActivateCombatAbilityEvent OnActivateAbility { get => _onActivateAbility; set => _onActivateAbility = value; }

    public void ExecuteEventLog(object sender)
    {
        try
        {
            if (_onExecuteEventLog is null) return;
            var info = sender is ExecuteEventLogNotification notification
                ? notification.Info : ReadLegacyRecord<ExecuteEventLogInfo>(sender);
            _onExecuteEventLog(info.Log, info.Done);
        }
        catch { RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)"); }
    }

    // CMapCtrl.pas:198. The notification carries the changed map area.
    public void MapChanged(object sender)
    {
        try
        {
            if (_onMapChanged is not null) _onMapChanged(((MapChangedNotification)sender).Info);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }

    public void ShowMessage(object sender)
    {
        try { _onShowMessage?.Invoke(((ShowMessageNotification)sender).Info); }
        catch { RtlUnit.ShowMessage("Exception occurred during ShowMessage event(TAoWCombatMapControl)"); }
    }

    public delegate void SelectHSEvent(HexSprite HS);
    public delegate void SelectCombatUnitEvent(TacticalBattleUnitHexSprite HS);
    public delegate void PlayerChangedEvent(sbyte Player);
    public delegate void ShowMessageEvent(in ShowMessageInfo Info);
    public delegate void TurnChangedEvent(int Turn);
    public delegate void MapChangedEvent(in MapChangedInfo ChangedArea);
    public delegate void OnExecuteEventLogEvent(EventLog EventLog, ExecuteDoneEvent ExecuteDoneEvent);
    public delegate void CastCombatSpellEvent(CombatSpellCaster SC);
    public delegate void ActivateCombatAbilityEvent(AbilityActivation AAI);
}
