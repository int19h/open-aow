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

    [Published]
    public event SelectHSEvent? Selected;
    [Published]
    public event SelectHSEvent? Deselected;
    [Published]
    public event NotifyEvent<object>? ActivateHsm;
    [Published]
    public event NotifyEvent<object>? DeactivateHsm;
    [Published]
    public event PlayerChangedEvent? IncomeChanged;
    [Published]
    public event PlayerChangedEvent? UpkeepChanged;
    [Published]
    public event PlayerChangedEvent? GemsChanged;
    [Published]
    public event ShowMessageEvent? ShowMessage;
    [Published]
    public event TurnChangedEvent? TurnChanged;
    [Published]
    public event PlayerChangedEvent? PlayerChanged;
    [Published]
    public event PlayerChangedEvent? SeatedPlayerChanged;
    [Published]
    public event Action<EventLog, ExecuteDoneEvent>? ExecuteEventLog;

    internal void ClearExecuteEventLogHandler() => ExecuteEventLog = null;

    [Published]
    public event MapChangedEvent? MapChanged;
    [Published]
    public event PlayerChangedEvent? LogbookChanged;
    [Published]
    public event NotifyEvent<object>? ActivateMap;
    [Published]
    public event NotifyEvent<object>? DeActivateMap;
    [Published]
    public event SelectHSEvent? MouseOverTarget;
    [Published]
    public event SelectCombatUnitEvent? UnitChanged;
    [Published]
    public event CastCombatSpellEvent? OnCastSpell;
    [Published]
    public event ActivateCombatAbilityEvent? ActivateAbility;

    // CMapCtrl.pas:117
    public override void Dispose()
    {
        if (_engine is not null)
        {
            _engine.OnHsmLoaded -= engine_HexSpriteMapLoaded;
            _engine.OnHsmClose -= engine_HexSpriteMapClosing;
            if (CombatMap.Current is not null)
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
        if (_engine is not null)
        {
            _engine.OnHsmLoaded += engine_HexSpriteMapLoaded;
            _engine.OnHsmClose += engine_HexSpriteMapClosing;
        }
    }

    // CMapCtrl.pas:128
    public void LinkToHsm()
    {
        CombatMap.Current.TurnChanged += ChangeTurn;
        CombatMap.Current.PlayerChanged += CombatMap_PlayerChanged;
        CombatMap.Current.SeatedPlayerChanged += CombatMap_SeatedPlayerChanged;
        CombatMap.Current.Selection.OnSelect += SelectHexSprite;
        CombatMap.Current.Selection.OnDeselect += DeselectHexSprite;
        CombatMap.Current.ShowingMessage += CombatMap_ShowingMessage;
        CombatMap.Current.MapChanged += CombatMap_MapChanged;
        CombatMap.Current.Deselected += CombatMap_Deselected;
        CombatMap.Current.Selected += DeactivateMap;
        CombatMap.Current.MouseOverTarget += CombatMap_MouseOverTarget;
        CombatMap.Current.UnitChanged += CombatMap_UnitChanged;
        CombatMap.Current.AbilityActivated += CombatMap_AbilityActivated;
        CombatMap.Current.CombatStarted += CastSpell;
        StrategicMap.Current.ExecutingEventLog += StrategicMap_ExecutingEventLog;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged += Player_GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged += Player_IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged += Player_UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged += Player_LogbookChanged;
        }
    }

    // CMapCtrl.pas:154
    public void ReleaseHsm()
    {
        CombatMap.Current.TurnChanged -= ChangeTurn;
        CombatMap.Current.PlayerChanged -= CombatMap_PlayerChanged;
        CombatMap.Current.SeatedPlayerChanged -= CombatMap_SeatedPlayerChanged;
        CombatMap.Current.Selection.OnSelect -= SelectHexSprite;
        CombatMap.Current.Selection.OnDeselect -= DeselectHexSprite;
        CombatMap.Current.ShowingMessage -= CombatMap_ShowingMessage;
        CombatMap.Current.MapChanged -= CombatMap_MapChanged;
        StrategicMap.Current.ExecutingEventLog -= StrategicMap_ExecutingEventLog;
        CombatMap.Current.Deselected -= CombatMap_Deselected;
        CombatMap.Current.Selected -= DeactivateMap;
        CombatMap.Current.MouseOverTarget -= CombatMap_MouseOverTarget;
        CombatMap.Current.UnitChanged -= CombatMap_UnitChanged;
        CombatMap.Current.AbilityActivated -= CombatMap_AbilityActivated;
        CombatMap.Current.CombatStarted -= CastSpell;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged -= Player_GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged -= Player_IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged -= Player_UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged -= Player_LogbookChanged;
        }
    }

    // CMapCtrl.pas:180
    public void engine_HexSpriteMapLoaded(GameMap sender)
    {
        if (sender.EngineClass.ClassId == CombatMap.ClassId)
        {
            LinkToHsm();
            ActivateHsm?.Invoke(this);
        }
    }

    // CMapCtrl.pas:189
    public void engine_HexSpriteMapClosing(GameMap sender)
    {
        if (sender.EngineClass.ClassId == CombatMap.ClassId)
        {
            DeactivateHsm?.Invoke(this);

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
            LegacyApplication.ShowMessage("Exception occurred during OnSelect event (TAoWCombatMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnDeselect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:225
    public void CombatMap_MouseOverTarget(object sender)
    {
        try
        {
            MouseOverTarget?.Invoke(CombatMap.Current.UnitSelection.MouseOverTarget);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnMouseOverTarget event (TAoWCombatMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during UnitChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:243
    public void CombatMap_PlayerChanged(object sender)
    {
        try
        {
            PlayerChanged?.Invoke(CombatMap.Current.CurrentPlayer);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:252
    public void CombatMap_SeatedPlayerChanged(sbyte player)
    {
        try
        {
            SeatedPlayerChanged?.Invoke(CombatMap.Current.SeatedPlayer);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:261
    public void Player_GemsChanged(Player sender)
    {
        try
        {
            GemsChanged?.Invoke(sender.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Gems Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:270
    public void Player_IncomeChanged(Player sender)
    {
        try
        {
            IncomeChanged?.Invoke(sender.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Income Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:279
    public void Player_UpkeepChanged(Player sender)
    {
        try
        {
            UpkeepChanged?.Invoke(sender.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Upkeep Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:297
    public void ChangeTurn(sbyte player)
    {
        try
        {
            // { The original passes the seated player as the turn number. }
            TurnChanged?.Invoke(CombatMap.Current.SeatedPlayer);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ChangeTurn event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:307
    public void Player_LogbookChanged(Player sender)
    {
        try
        {
            LogbookChanged?.Invoke(sender.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during LogbookChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:325
    public void CombatMap_Deselected(object sender)
    {
        try
        {
            ActivateMap?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnActivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:334
    public void DeactivateMap(object sender)
    {
        try
        {
            DeActivateMap?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnDeactivateMap event (TAoWCombatMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnCastSpell event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:352
    public void CombatMap_AbilityActivated(AbilityActivation sender)
    {
        try
        {
            ActivateAbility?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnActivateAbility event (TAoWCombatMapControl)");
        }
    }

    public void StrategicMap_ExecutingEventLog(object sender)
    {
        try
        {
            if (ExecuteEventLog is null)
            {
                return;
            }

            ExecuteEventLogInfo info = sender is ExecuteEventLogNotification notification
                ? notification.Info : ReadLegacyRecord<ExecuteEventLogInfo>(sender);
            ExecuteEventLog(info.Log, info.Done);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }

    public void CombatMap_ShowingMessage(object sender)
    {
        try
        {
            ShowMessage?.Invoke(((ShowMessageNotification)sender).Info);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ShowMessage event(TAoWCombatMapControl)");
        }
    }

    public delegate void SelectHSEvent(HexSprite? HS);
    public delegate void SelectCombatUnitEvent(TacticalBattleUnitHexSprite? HS);
    public delegate void PlayerChangedEvent(sbyte Player);
    public delegate void ShowMessageEvent(in ShowMessageInfo Info);
    public delegate void TurnChangedEvent(int Turn);
    public delegate void MapChangedEvent(in MapChangedInfo ChangedArea);
    public delegate void CastCombatSpellEvent(CombatSpellCaster SC);
    public delegate void ActivateCombatAbilityEvent(AbilityActivation AAI);
}
