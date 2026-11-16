#nullable disable
// Translated from CMapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Grids;

// event methods occupy
public partial class CombatMapControl : Component
{
    private readonly GameEngine _engine;

    [Published]
    public event SelectHSEvent OnSelected;
    [Published]
    public event SelectHSEvent OnDeselected;
    [Published]
    public event NotifyEvent<object> OnActivateHsm;
    [Published]
    public event NotifyEvent<object> OnDeactivateHsm;
    [Published]
    public event PlayerChangedEvent OnIncomeChanged;
    [Published]
    public event PlayerChangedEvent OnUpkeepChanged;
    [Published]
    public event PlayerChangedEvent OnGemsChanged;
    [Published]
    public event ShowMessageEvent OnShowMessage;
    [Published]
    public event TurnChangedEvent OnTurnChanged;
    [Published]
    public event PlayerChangedEvent OnPlayerChanged;
    [Published]
    public event PlayerChangedEvent OnSeatedPlayerChanged;
    [Published]
    public event OnExecuteEventLogEvent OnExecuteEventLog;

    internal void ClearExecuteEventLogHandler() => OnExecuteEventLog = null;

    [Published]
    public event MapChangedEvent OnMapChanged;
    [Published]
    public event PlayerChangedEvent OnLogbookChanged;
    [Published]
    public event NotifyEvent<object> OnActivateMap;
    [Published]
    public event NotifyEvent<object> OnDeActivateMap;
    [Published]
    public event SelectHSEvent OnMouseOverTarget;
    [Published]
    public event SelectCombatUnitEvent OnUnitChanged;
    [Published]
    public event CastCombatSpellEvent OnCastSpell;
    [Published]
    public event ActivateCombatAbilityEvent OnActivateAbility;

    // CMapCtrl.pas:117
    public override void Dispose()
    {
        if (_engine != null)
        {
            _engine.OnHsmLoaded -= ActivateHsm;
            _engine.OnHsmClose -= DeactivateHsm;
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
        // The engine of the dialog module that owns the control, as the original read the
        // engine global.
        _engine = (owner as UIModule)?.UI.Engine;
        if (_engine != null)
        {
            _engine.OnHsmLoaded += ActivateHsm;
            _engine.OnHsmClose += DeactivateHsm;
        }
    }

    // CMapCtrl.pas:128
    public void LinkToHsm()
    {
        CombatMap.Current.TurnChanged += ChangeTurn;
        CombatMap.Current.PlayerChanged += PlayerChanged;
        CombatMap.Current.SeatedPlayerChanged += SeatedPlayerChanged;
        CombatMap.Current.Selection.OnSelect += SelectHexSprite;
        CombatMap.Current.Selection.OnDeselect += DeselectHexSprite;
        CombatMap.Current.ShowingMessage += ShowMessage;
        CombatMap.Current.MapChanged += MapChanged;
        CombatMap.Current.Deselected += ActivateMap;
        CombatMap.Current.Selected += DeactivateMap;
        CombatMap.Current.MouseOverTarget += MouseOverTarget;
        CombatMap.Current.UnitChanged += UnitChanged;
        CombatMap.Current.AbilityActivated += ActivateAbility;
        CombatMap.Current.CombatStarted += CastSpell;
        StrategicMap.Current.ExecutingEventLog += ExecuteEventLog;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged += GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged += IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged += UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged += LogbookChanged;
        }
    }

    // CMapCtrl.pas:154
    public void ReleaseHsm()
    {
        CombatMap.Current.TurnChanged -= ChangeTurn;
        CombatMap.Current.PlayerChanged -= PlayerChanged;
        CombatMap.Current.SeatedPlayerChanged -= SeatedPlayerChanged;
        CombatMap.Current.Selection.OnSelect -= SelectHexSprite;
        CombatMap.Current.Selection.OnDeselect -= DeselectHexSprite;
        CombatMap.Current.ShowingMessage -= ShowMessage;
        CombatMap.Current.MapChanged -= MapChanged;
        StrategicMap.Current.ExecutingEventLog -= ExecuteEventLog;
        CombatMap.Current.Deselected -= ActivateMap;
        CombatMap.Current.Selected -= DeactivateMap;
        CombatMap.Current.MouseOverTarget -= MouseOverTarget;
        CombatMap.Current.UnitChanged -= UnitChanged;
        CombatMap.Current.AbilityActivated -= ActivateAbility;
        CombatMap.Current.CombatStarted -= CastSpell;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged -= GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged -= IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged -= UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged -= LogbookChanged;
        }
    }

    // CMapCtrl.pas:180
    public void ActivateHsm(GameMap sender)
    {
        if (sender.EngineClass.ClassId == 2228480)
        {
            LinkToHsm();
            OnActivateHsm?.Invoke(this);
        }
    }

    // CMapCtrl.pas:189
    public void DeactivateHsm(GameMap sender)
    {
        if (sender.EngineClass.ClassId == 2228480)
        {
            OnDeactivateHsm?.Invoke(this);

            ReleaseHsm();
        }
    }

    // CMapCtrl.pas:207
    public void SelectHexSprite(object sender)
    {
        try
        {
            OnSelected?.Invoke(CombatMap.Current.Selection.EventHexSprite);
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
            OnDeselected?.Invoke(CombatMap.Current.Selection.EventHexSprite);
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
            OnMouseOverTarget?.Invoke(CombatMap.Current.UnitSelection.MouseOverTarget);
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
            if (OnUnitChanged != null)
            {
                TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite();
                OnUnitChanged(tacticalBattleUnitHexSprite);
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
            OnPlayerChanged?.Invoke(CombatMap.Current.CurrentPlayer);
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
            OnSeatedPlayerChanged?.Invoke(CombatMap.Current.SeatedPlayer);
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
            OnGemsChanged?.Invoke(sender.PlayerIndex);
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
            OnIncomeChanged?.Invoke(sender.PlayerIndex);
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
            OnUpkeepChanged?.Invoke(sender.PlayerIndex);
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
            OnTurnChanged?.Invoke(CombatMap.Current.SeatedPlayer);
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
            OnLogbookChanged?.Invoke(sender.PlayerIndex);
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
            OnActivateMap?.Invoke(this);
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
            OnDeActivateMap?.Invoke(this);
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
            OnCastSpell?.Invoke(sender);
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
            OnActivateAbility?.Invoke(sender);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWCombatMapControl)");
        }
    }

    public void ExecuteEventLog(object sender)
    {
        try
        {
            if (OnExecuteEventLog is null)
            {
                return;
            }

            ExecuteEventLogInfo info = sender is ExecuteEventLogNotification notification
                ? notification.Info : ReadLegacyRecord<ExecuteEventLogInfo>(sender);
            OnExecuteEventLog(info.Log, info.Done);
        }
        catch { RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)"); }
    }

    // CMapCtrl.pas:198. The notification carries the changed map area.
    public void MapChanged(object sender)
    {
        try
        {
            if (OnMapChanged is not null)
            {
                OnMapChanged(((MapChangedNotification)sender).Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }

    public void ShowMessage(object sender)
    {
        try { OnShowMessage?.Invoke(((ShowMessageNotification)sender).Info); }
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
