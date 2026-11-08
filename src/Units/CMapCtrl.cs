// Translated from CMapCtrl.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWTC;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWTC.AoWTCUnit;

namespace Aow.Units.CMapCtrl;

// event methods occupy
public partial class TAoWCombatMapControl : TComponent
{
    public TSelectHSEvent FOnSelected;
    public TSelectHSEvent FOnDeselected;
    public TNotifyEvent<TObject> FOnActivateHSM;
    public TNotifyEvent<TObject> FOnDeactivateHSM;
    public TPlayerChangedEvent FOnIncomeChanged;
    public TPlayerChangedEvent FOnUpkeepChanged;
    public TPlayerChangedEvent FOnGemsChanged;
    public TShowMessageEvent FOnShowMessage;
    public TTurnChangedEvent FOnTurnChanged;
    public TPlayerChangedEvent FOnPlayerChanged;
    public TPlayerChangedEvent FOnSeatedPlayerChanged;
    public TOnExecuteEventLogEvent FOnExecuteEventLog;
    public TMapChangedEvent FOnMapChanged;
    public TPlayerChangedEvent FOnLogbookChanged;
    public TNotifyEvent<TObject> FOnActivateMap;
    public TNotifyEvent<TObject> FOnDeActivateMap;
    public TSelectHSEvent FOnMouseOverTarget;
    public TSelectCombatUnitEvent FOnUnitChanged;
    public TCastCombatSpellEvent FOnCastSpell;
    public TActivateCombatAbilityEvent FOnActivateAbility;

    // CMapCtrl.pas:117
    public override void Destroy()
    {
        if (AoWEngine != null)
        {
            AoWEngine.OnHSMLoaded -= ActivateHSM;
            AoWEngine.OnHSMClose -= DeactivateHSM;
            if (AoWCombatMap != null)
            {
                ReleaseHSM();
            }
        }

        base.Destroy();
    }

    // CMapCtrl.pas:107
    public TAoWCombatMapControl(TComponent owner) : base(owner)
    {
        if (ValidateAoWEngine())
        {
            AoWEngine.OnHSMLoaded += ActivateHSM;
            AoWEngine.OnHSMClose += DeactivateHSM;
        }
    }

    // CMapCtrl.pas:128
    public void LinkToHSM()
    {
        AoWCombatMap.FChangeTurnEvents += ChangeTurn;
        AoWCombatMap.FPlayerChangedEvents += PlayerChanged;
        AoWCombatMap.FSeatedPlayerChangedEvents += SeatedPlayerChanged;
        AoWCombatMap.Selection.OnSelect += SelectHS;
        AoWCombatMap.Selection.OnDeselect += DeselectHS;
        AoWCombatMap.FShowMessageEvents += ShowMessage;
        AoWCombatMap.FMapChangedEvents += MapChanged;
        AoWCombatMap.FDeselectEvents += ActivateMap;
        AoWCombatMap.FSelectEvents += DeactivateMap;
        AoWCombatMap.FMouseOverTargetEvents += MouseOverTarget;
        AoWCombatMap.FUnitChangedEvents += UnitChanged;
        AoWCombatMap.FAbilityActivatedEvents += ActivateAbility;
        AoWCombatMap.FCombatStartedEvents += CastSpell;
        AoWHSMap.FExecuteEventLogEvents += ExecuteEventLog;
        for (int I = 0, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            AoWHSMap.FPlayers.GetPlayers(I).FGemsChangedEvents += GemsChanged;
            AoWHSMap.FPlayers.GetPlayers(I).FIncomeChangedEvents += IncomeChanged;
            AoWHSMap.FPlayers.GetPlayers(I).FUpkeepChangedEvents += UpkeepChanged;
            AoWHSMap.FPlayers.GetPlayers(I).FLogbookChangedEvents += LogbookChanged;
        }
    }

    // CMapCtrl.pas:154
    public void ReleaseHSM()
    {
        AoWCombatMap.FChangeTurnEvents -= ChangeTurn;
        AoWCombatMap.FPlayerChangedEvents -= PlayerChanged;
        AoWCombatMap.FSeatedPlayerChangedEvents -= SeatedPlayerChanged;
        AoWCombatMap.Selection.OnSelect -= SelectHS;
        AoWCombatMap.Selection.OnDeselect -= DeselectHS;
        AoWCombatMap.FShowMessageEvents -= ShowMessage;
        AoWCombatMap.FMapChangedEvents -= MapChanged;
        AoWHSMap.FExecuteEventLogEvents -= ExecuteEventLog;
        AoWCombatMap.FDeselectEvents -= ActivateMap;
        AoWCombatMap.FSelectEvents -= DeactivateMap;
        AoWCombatMap.FMouseOverTargetEvents -= MouseOverTarget;
        AoWCombatMap.FUnitChangedEvents -= UnitChanged;
        AoWCombatMap.FAbilityActivatedEvents -= ActivateAbility;
        AoWCombatMap.FCombatStartedEvents -= CastSpell;
        for (int I = 0, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            AoWHSMap.FPlayers.GetPlayers(I).FGemsChangedEvents -= GemsChanged;
            AoWHSMap.FPlayers.GetPlayers(I).FIncomeChangedEvents -= IncomeChanged;
            AoWHSMap.FPlayers.GetPlayers(I).FUpkeepChangedEvents -= UpkeepChanged;
            AoWHSMap.FPlayers.GetPlayers(I).FLogbookChangedEvents -= LogbookChanged;
        }
    }

    // CMapCtrl.pas:180
    public void ActivateHSM(THSMap Sender)
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
    public void DeactivateHSM(THSMap Sender)
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
    public void SelectHS(TObject Sender)
    {
        try
        {
            if (FOnSelected != null)
            {
                FOnSelected(AoWCombatMap.Selection.EventHS);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnSelect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:216
    public void DeselectHS(TObject Sender)
    {
        try
        {
            if (FOnDeselected != null)
            {
                FOnDeselected(AoWCombatMap.Selection.EventHS);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeselect event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:225
    public void MouseOverTarget(TObject Sender)
    {
        try
        {
            if (FOnMouseOverTarget != null)
            {
                FOnMouseOverTarget(AoWCombatMap.FUnitSelection.FMouseOverTarget);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnMouseOverTarget event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:234
    public void UnitChanged(TObject Sender)
    {
        try
        {
            if (FOnUnitChanged != null)
            {
                var tcUnitHS = AoWCombatMap.FUnitSelection.GetTCUnitHS();
                FOnUnitChanged(tcUnitHS);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during UnitChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:243
    public void PlayerChanged(TObject Sender)
    {
        try
        {
            if (FOnPlayerChanged != null)
            {
                FOnPlayerChanged(AoWCombatMap.FCurrentPlayer);
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
                FOnSeatedPlayerChanged(AoWCombatMap.FSeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:261
    public void GemsChanged(TPlayer Sender)
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
    public void IncomeChanged(TPlayer Sender)
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
    public void UpkeepChanged(TPlayer Sender)
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
                FOnTurnChanged(AoWCombatMap.FSeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ChangeTurn event (TAoWCombatMapControl)");
        }
    }

    // CMapCtrl.pas:307
    public void LogbookChanged(TPlayer Sender)
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
    public void ActivateMap(TObject Sender)
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
    public void DeactivateMap(TObject Sender)
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
    public void CastSpell(TCombatSpellCaster Sender)
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
    public void ActivateAbility(TActivateAbilityInfo Sender)
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
    public TSelectHSEvent OnSelected { get => FOnSelected; set => FOnSelected = value; }

    [Published]
    public TSelectHSEvent OnDeselected { get => FOnDeselected; set => FOnDeselected = value; }

    [Published]
    public TNotifyEvent<TObject> OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

    [Published]
    public TNotifyEvent<TObject> OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

    [Published]
    public TPlayerChangedEvent OnIncomeChanged { get => FOnIncomeChanged; set => FOnIncomeChanged = value; }

    [Published]
    public TPlayerChangedEvent OnUpkeepChanged { get => FOnUpkeepChanged; set => FOnUpkeepChanged = value; }

    [Published]
    public TPlayerChangedEvent OnGemsChanged { get => FOnGemsChanged; set => FOnGemsChanged = value; }

    [Published]
    public TShowMessageEvent OnShowMessage { get => FOnShowMessage; set => FOnShowMessage = value; }

    [Published]
    public TTurnChangedEvent OnTurnChanged { get => FOnTurnChanged; set => FOnTurnChanged = value; }

    [Published]
    public TPlayerChangedEvent OnPlayerChanged { get => FOnPlayerChanged; set => FOnPlayerChanged = value; }

    [Published]
    public TPlayerChangedEvent OnSeatedPlayerChanged { get => FOnSeatedPlayerChanged; set => FOnSeatedPlayerChanged = value; }

    [Published]
    public TOnExecuteEventLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

    [Published]
    public TMapChangedEvent OnMapChanged { get => FOnMapChanged; set => FOnMapChanged = value; }

    [Published]
    public TPlayerChangedEvent OnLogbookChanged { get => FOnLogbookChanged; set => FOnLogbookChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnActivateMap { get => FOnActivateMap; set => FOnActivateMap = value; }

    [Published]
    public TNotifyEvent<TObject> OnDeActivateMap { get => FOnDeActivateMap; set => FOnDeActivateMap = value; }

    [Published]
    public TSelectHSEvent OnMouseOverTarget { get => FOnMouseOverTarget; set => FOnMouseOverTarget = value; }

    [Published]
    public TSelectCombatUnitEvent OnUnitChanged { get => FOnUnitChanged; set => FOnUnitChanged = value; }

    [Published]
    public TCastCombatSpellEvent OnCastSpell { get => FOnCastSpell; set => FOnCastSpell = value; }

    [Published]
    public TActivateCombatAbilityEvent OnActivateAbility { get => FOnActivateAbility; set => FOnActivateAbility = value; }

    public void ExecuteEventLog(TObject sender)
    {
        try
        {
            if (FOnExecuteEventLog is null) return;
            var info = sender is ExecuteEventLogNotification notification
                ? notification.Info : ReadLegacyRecord<TExecuteEventLogInfo>(sender);
            FOnExecuteEventLog(info.Log, info.Done);
        }
        catch { RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)"); }
    }

    // CMapCtrl.pas:198. The notification carries the changed map area.
    public void MapChanged(MapChangedNotification sender)
    {
        try
        {
            if (FOnMapChanged is not null && sender is not null) FOnMapChanged(sender.Info);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }

    public void ShowMessage(ShowMessageNotification sender)
    {
        try { if (sender is not null) FOnShowMessage?.Invoke(sender.Info); }
        catch { RtlUnit.ShowMessage("Exception occurred during ShowMessage event(TAoWCombatMapControl)"); }
    }
}

public delegate void TOnMessageEvent(ref TEMsg EMsg);
public delegate void TSelectHSEvent(THexagonSprite HS);
public delegate void TSelectCombatUnitEvent(TTacticalCombatUnitHS HS);
public delegate void TDeselectHSEvent(THexagonSprite HS);
public delegate void TPlayerChangedEvent(sbyte Player);
public delegate void TShowMessageEvent(in TShowMessageInfo Info);
public delegate void TTurnChangedEvent(int Turn);
public delegate void TMapChangedEvent(in TMapChangedInfo ChangedArea);
public delegate void TOnGameOverEvent(sbyte Player);
public delegate void TOnExecuteEventLogEvent(TEventLog EventLog, TExecuteDoneEvent ExecuteDoneEvent);
public delegate void TCastCombatSpellEvent(TCombatSpellCaster SC);
public delegate void TActivateCombatAbilityEvent(TActivateAbilityInfo AAI);

public static partial class CMapCtrlUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
