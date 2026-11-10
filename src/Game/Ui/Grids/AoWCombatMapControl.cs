// Translated from CMapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

// event methods occupy
public partial class AoWCombatMapControl : Component
{
    public SelectHSEvent FOnSelected;
    public SelectHSEvent FOnDeselected;
    public NotifyEvent<object> FOnActivateHSM;
    public NotifyEvent<object> FOnDeactivateHSM;
    public PlayerChangedEvent FOnIncomeChanged;
    public PlayerChangedEvent FOnUpkeepChanged;
    public PlayerChangedEvent FOnGemsChanged;
    public ShowMessageEvent FOnShowMessage;
    public TurnChangedEvent FOnTurnChanged;
    public PlayerChangedEvent FOnPlayerChanged;
    public PlayerChangedEvent FOnSeatedPlayerChanged;
    public OnExecuteEventLogEvent FOnExecuteEventLog;
    public MapChangedEvent FOnMapChanged;
    public PlayerChangedEvent FOnLogbookChanged;
    public NotifyEvent<object> FOnActivateMap;
    public NotifyEvent<object> FOnDeActivateMap;
    public SelectHSEvent FOnMouseOverTarget;
    public SelectCombatUnitEvent FOnUnitChanged;
    public CastCombatSpellEvent FOnCastSpell;
    public ActivateCombatAbilityEvent FOnActivateAbility;

    // CMapCtrl.pas:117
    public override void Dispose()
    {
        if (GameTables.AoWEngine != null)
        {
            GameTables.AoWEngine.OnHSMLoaded -= ActivateHSM;
            GameTables.AoWEngine.OnHSMClose -= DeactivateHSM;
            if (AoWCombatMap.Current != null)
            {
                ReleaseHSM();
            }
        }

        base.Dispose();
    }

    // CMapCtrl.pas:107
    public AoWCombatMapControl(Component owner) : base(owner)
    {
        if (GameTables.ValidateAoWEngine())
        {
            GameTables.AoWEngine.OnHSMLoaded += ActivateHSM;
            GameTables.AoWEngine.OnHSMClose += DeactivateHSM;
        }
    }

    // CMapCtrl.pas:128
    public void LinkToHSM()
    {
        AoWCombatMap.Current.FChangeTurnEvents += ChangeTurn;
        AoWCombatMap.Current.FPlayerChangedEvents += PlayerChanged;
        AoWCombatMap.Current.FSeatedPlayerChangedEvents += SeatedPlayerChanged;
        AoWCombatMap.Current.Selection.OnSelect += SelectHS;
        AoWCombatMap.Current.Selection.OnDeselect += DeselectHS;
        AoWCombatMap.Current.FShowMessageEvents += ShowMessage;
        AoWCombatMap.Current.FMapChangedEvents += MapChanged;
        AoWCombatMap.Current.FDeselectEvents += ActivateMap;
        AoWCombatMap.Current.FSelectEvents += DeactivateMap;
        AoWCombatMap.Current.FMouseOverTargetEvents += MouseOverTarget;
        AoWCombatMap.Current.FUnitChangedEvents += UnitChanged;
        AoWCombatMap.Current.FAbilityActivatedEvents += ActivateAbility;
        AoWCombatMap.Current.FCombatStartedEvents += CastSpell;
        AoWHSMap.Current.FExecuteEventLogEvents += ExecuteEventLog;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(I).FGemsChangedEvents += GemsChanged;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FIncomeChangedEvents += IncomeChanged;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FUpkeepChangedEvents += UpkeepChanged;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FLogbookChangedEvents += LogbookChanged;
        }
    }

    // CMapCtrl.pas:154
    public void ReleaseHSM()
    {
        AoWCombatMap.Current.FChangeTurnEvents -= ChangeTurn;
        AoWCombatMap.Current.FPlayerChangedEvents -= PlayerChanged;
        AoWCombatMap.Current.FSeatedPlayerChangedEvents -= SeatedPlayerChanged;
        AoWCombatMap.Current.Selection.OnSelect -= SelectHS;
        AoWCombatMap.Current.Selection.OnDeselect -= DeselectHS;
        AoWCombatMap.Current.FShowMessageEvents -= ShowMessage;
        AoWCombatMap.Current.FMapChangedEvents -= MapChanged;
        AoWHSMap.Current.FExecuteEventLogEvents -= ExecuteEventLog;
        AoWCombatMap.Current.FDeselectEvents -= ActivateMap;
        AoWCombatMap.Current.FSelectEvents -= DeactivateMap;
        AoWCombatMap.Current.FMouseOverTargetEvents -= MouseOverTarget;
        AoWCombatMap.Current.FUnitChangedEvents -= UnitChanged;
        AoWCombatMap.Current.FAbilityActivatedEvents -= ActivateAbility;
        AoWCombatMap.Current.FCombatStartedEvents -= CastSpell;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(I).FGemsChangedEvents -= GemsChanged;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FIncomeChangedEvents -= IncomeChanged;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FUpkeepChangedEvents -= UpkeepChanged;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FLogbookChangedEvents -= LogbookChanged;
        }
    }

    // CMapCtrl.pas:180
    public void ActivateHSM(HSMap Sender)
    {
        if (Sender.EClass.ClassID == 2228480)
        {
            LinkToHSM();
            if (FOnActivateHSM != null)
            {
                FOnActivateHSM(this);
            }
        }
    }

    // CMapCtrl.pas:189
    public void DeactivateHSM(HSMap Sender)
    {
        if (Sender.EClass.ClassID == 2228480)
        {
            if (FOnDeactivateHSM != null)
            {
                FOnDeactivateHSM(this);
            }

            ReleaseHSM();
        }
    }

    // CMapCtrl.pas:207
    public void SelectHS(object Sender)
    {
        try
        {
            if (FOnSelected != null)
            {
                FOnSelected(AoWCombatMap.Current.Selection.EventHS);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnSelect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:216
    public void DeselectHS(object Sender)
    {
        try
        {
            if (FOnDeselected != null)
            {
                FOnDeselected(AoWCombatMap.Current.Selection.EventHS);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeselect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:225
    public void MouseOverTarget(object Sender)
    {
        try
        {
            if (FOnMouseOverTarget != null)
            {
                FOnMouseOverTarget(AoWCombatMap.Current.FUnitSelection.FMouseOverTarget);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnMouseOverTarget event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:234
    public void UnitChanged(object Sender)
    {
        try
        {
            if (FOnUnitChanged != null)
            {
                var tcUnitHS = AoWCombatMap.Current.FUnitSelection.GetTCUnitHS();
                FOnUnitChanged(tcUnitHS);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during UnitChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:243
    public void PlayerChanged(object Sender)
    {
        try
        {
            if (FOnPlayerChanged != null)
            {
                FOnPlayerChanged(AoWCombatMap.Current.FCurrentPlayer);
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
            if (FOnSeatedPlayerChanged != null)
            {
                FOnSeatedPlayerChanged(AoWCombatMap.Current.FSeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:261
    public void GemsChanged(Player Sender)
    {
        try
        {
            if (FOnGemsChanged != null)
            {
                FOnGemsChanged(Sender.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Gems Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:270
    public void IncomeChanged(Player Sender)
    {
        try
        {
            if (FOnIncomeChanged != null)
            {
                FOnIncomeChanged(Sender.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Income Changed event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:279
    public void UpkeepChanged(Player Sender)
    {
        try
        {
            if (FOnUpkeepChanged != null)
            {
                FOnUpkeepChanged(Sender.FPlayerIndex);
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
            if (FOnTurnChanged != null)
            {
                FOnTurnChanged(AoWCombatMap.Current.FSeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ChangeTurn event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:307
    public void LogbookChanged(Player Sender)
    {
        try
        {
            if (FOnLogbookChanged != null)
            {
                FOnLogbookChanged(Sender.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during LogbookChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:325
    public void ActivateMap(object Sender)
    {
        try
        {
            if (FOnActivateMap != null)
            {
                FOnActivateMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:334
    public void DeactivateMap(object Sender)
    {
        try
        {
            if (FOnDeActivateMap != null)
            {
                FOnDeActivateMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeactivateMap event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:343
    public void CastSpell(CombatSpellCaster Sender)
    {
        try
        {
            if (FOnCastSpell != null)
            {
                FOnCastSpell(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnCastSpell event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:352
    public void ActivateAbility(ActivateAbilityInfo Sender)
    {
        try
        {
            if (FOnActivateAbility != null)
            {
                FOnActivateAbility(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWCombatMapControl)");
        }
    }

    [Published]
    public SelectHSEvent OnSelected { get => FOnSelected; set => FOnSelected = value; }

    [Published]
    public SelectHSEvent OnDeselected { get => FOnDeselected; set => FOnDeselected = value; }

    [Published]
    public NotifyEvent<object> OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

    [Published]
    public NotifyEvent<object> OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

    [Published]
    public PlayerChangedEvent OnIncomeChanged { get => FOnIncomeChanged; set => FOnIncomeChanged = value; }

    [Published]
    public PlayerChangedEvent OnUpkeepChanged { get => FOnUpkeepChanged; set => FOnUpkeepChanged = value; }

    [Published]
    public PlayerChangedEvent OnGemsChanged { get => FOnGemsChanged; set => FOnGemsChanged = value; }

    [Published]
    public ShowMessageEvent OnShowMessage { get => FOnShowMessage; set => FOnShowMessage = value; }

    [Published]
    public TurnChangedEvent OnTurnChanged { get => FOnTurnChanged; set => FOnTurnChanged = value; }

    [Published]
    public PlayerChangedEvent OnPlayerChanged { get => FOnPlayerChanged; set => FOnPlayerChanged = value; }

    [Published]
    public PlayerChangedEvent OnSeatedPlayerChanged { get => FOnSeatedPlayerChanged; set => FOnSeatedPlayerChanged = value; }

    [Published]
    public OnExecuteEventLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

    [Published]
    public MapChangedEvent OnMapChanged { get => FOnMapChanged; set => FOnMapChanged = value; }

    [Published]
    public PlayerChangedEvent OnLogbookChanged { get => FOnLogbookChanged; set => FOnLogbookChanged = value; }

    [Published]
    public NotifyEvent<object> OnActivateMap { get => FOnActivateMap; set => FOnActivateMap = value; }

    [Published]
    public NotifyEvent<object> OnDeActivateMap { get => FOnDeActivateMap; set => FOnDeActivateMap = value; }

    [Published]
    public SelectHSEvent OnMouseOverTarget { get => FOnMouseOverTarget; set => FOnMouseOverTarget = value; }

    [Published]
    public SelectCombatUnitEvent OnUnitChanged { get => FOnUnitChanged; set => FOnUnitChanged = value; }

    [Published]
    public CastCombatSpellEvent OnCastSpell { get => FOnCastSpell; set => FOnCastSpell = value; }

    [Published]
    public ActivateCombatAbilityEvent OnActivateAbility { get => FOnActivateAbility; set => FOnActivateAbility = value; }

    public void ExecuteEventLog(object sender)
    {
        try
        {
            if (FOnExecuteEventLog is null) return;
            var info = sender is ExecuteEventLogNotification notification
                ? notification.Info : ReadLegacyRecord<ExecuteEventLogInfo>(sender);
            FOnExecuteEventLog(info.Log, info.Done);
        }
        catch { RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)"); }
    }

    // CMapCtrl.pas:198. The notification carries the changed map area.
    public void MapChanged(object sender)
    {
        try
        {
            if (FOnMapChanged is not null) FOnMapChanged(((MapChangedNotification)sender).Info);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }

    public void ShowMessage(object sender)
    {
        try { FOnShowMessage?.Invoke(((ShowMessageNotification)sender).Info); }
        catch { RtlUnit.ShowMessage("Exception occurred during ShowMessage event(TAoWCombatMapControl)"); }
    }

    public delegate void SelectHSEvent(HexagonSprite HS);
    public delegate void SelectCombatUnitEvent(TacticalCombatUnitHS HS);
    public delegate void PlayerChangedEvent(sbyte Player);
    public delegate void ShowMessageEvent(in ShowMessageInfo Info);
    public delegate void TurnChangedEvent(int Turn);
    public delegate void MapChangedEvent(in MapChangedInfo ChangedArea);
    public delegate void OnExecuteEventLogEvent(EventLog EventLog, ExecuteDoneEvent ExecuteDoneEvent);
    public delegate void CastCombatSpellEvent(CombatSpellCaster SC);
    public delegate void ActivateCombatAbilityEvent(ActivateAbilityInfo AAI);
}
