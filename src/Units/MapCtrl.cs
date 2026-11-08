// Translated from MapCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.MapCtrl;

public partial class TAoWMapControl : TComponent
{
    public TMapObjectEvent<TArmy> FOnSelectArmy;
    public TMapObjectEvent<TArmy> FOnDeselectArmy;
    public TMapObjectEvent<TArmy> FOnDblClickArmy;
    public TMapObjectEvent<TArmy> FOnArmyChanged;
    public TMapObjectEvent<TArmy> FOnArmyDestroyed;
    public TMapObjectEvent<TStructure> FOnSelectStructure;
    public TMapObjectEvent<TStructure> FOnDeselectStructure;
    public TMapObjectEvent<TStructure> FOnDblClickStructure;
    public TMapObjectEvent<TStructure> FOnStructureChanged;
    public TMapObjectEvent<TStructure> FOnStructureDestroyed;
    public TMapObjectEvent<TItemHS> FOnSelectItem;
    public TMapObjectEvent<TItemHS> FOnDeselectItem;
    public TMapObjectEvent<TAbstractUnit> FOnUnitChanged;
    public TMapObjectEvent<TAbstractUnit> FOnUnitDestroyed;
    public TMapObjectEvent<TActivateAbilityInfo> FOnActivateAbility;
    public TNotifyEvent<TObject> FOnDiplomaticChange;
    public TNotifyEvent<TObject> FOnActivateHSM;
    public TNotifyEvent<TObject> FOnDeactivateHSM;
    public TMapPlayerEvent FOnIncomeChanged;
    public TMapPlayerEvent FOnUpkeepChanged;
    public TMapPlayerEvent FOnGemsChanged;
    public TMapPlayerEvent FOnPlayerMagicChanged;
    public TNotifyEvent<TObject> FOnGlobalMagicChanged;
    public TMapRefEvent<TShowMessageInfo> FOnShowMessage;
    public TMapDayEvent FOnChangeDay;
    public TNotifyEvent<TObject> FOnNewTurn;
    public TMapPlayerEvent FOnPlayerChanged;
    public TMapPlayerEvent FOnSeatedPlayerChanged;
    public TNotifyEvent<TObject> FOnCombatEvent;
    public TMapExecuteLogEvent FOnExecuteEventLog;
    public TMapRefEvent<TMapChangedInfo> FOnMapChanged;
    public TMapRefEvent<TMapChangedInfo> FOnScannerIconsChanged;
    public TNotifyEvent<TObject> FOnMapSettingsChanged;
    public TMapPlayerEvent FOnLogbookChanged;
    public TNotifyEvent<TObject> FOnGameOver;
    public TMapPlayerEvent FOnGameTerminated;
    public TMapPlayerEvent FOnPlayerTerminated;
    public TNotifyEvent<TObject> FOnActivateCombatMap;
    public TNotifyEvent<TObject> FOnDeactivateCombatMap;
    public TMapTargetEvent FOnTargetSelectionChange;
    public TNotifyEvent<TObject> FOnViewPortChange;
    public TMapObjectEvent<TSpellCaster> FOnCastSpell;
    public TMapEndTurnEvent FOnEndTurn;
    public TNotifyEvent<TObject> FOnRelationsChanged;
    public TNotifyEvent<TObject> FOnExitGameEvent;
    public TMapObjectEvent<THexagonSprite> FOnHSChanged;

    // MapCtrl.pas:186
    public override void Destroy()
    {
        if (AoWEngine != null)
        {
            AoWEngine.FOnHSMLoaded.Remove(ActivateHSM);
            AoWEngine.FOnHSMClose.Remove(DeactivateHSM);
            if (AoWHSMap != null)
            {
                ReleaseHSM();
            }
        }

        base.Destroy();
    }

    // MapCtrl.pas:174
    public TAoWMapControl(TComponent owner) : base(owner)
    {
        if (ValidateAoWEngine())
        {
            AoWEngine.FOnHSMLoaded.Add(ActivateHSM);
            AoWEngine.FOnHSMClose.Add(DeactivateHSM);
            if (AoWHSMap != null)
            {
                LinkToHSM();
            }
        }
    }

    // MapCtrl.pas:197
    public void LinkToHSM()
    {
        AoWHSMap.FMouse.FOnDblClick.Add(DblClick);
        AoWHSMap.FNewDayEvents.Add(ChangeDay);
        AoWHSMap.FCurrentPlayerChangedEvents.Add(PlayerChanged);
        AoWHSMap.FNewTurnEvents.Add(NewTurn);
        AoWHSMap.FEndTurnEvents.Add(EndTurn);
        AoWHSMap.FSeatedPlayerChangedEvents.Add(SeatedPlayerChanged);
        AoWHSMap.FSelection.FOnSelect.Add(SelectHS);
        AoWHSMap.FSelection.FOnDeselect.Add(DeselectHS);
        AoWHSMap.FShowMessageEvents.Add(ShowMessage);
        AoWHSMap.FMapChangedEvents.Add(MapChanged);
        AoWHSMap.FScannerIconsChangedEvents.Add(ScannerIconsChanged);
        AoWHSMap.FUpdateVisibilityEvents.Add(MapSettingsChanged);
        AoWHSMap.FCombatEvents.Add(CombatEvent);
        AoWHSMap.FStructureChangedEvents.Add(StructureChanged);
        AoWHSMap.FStructureDestroyedEvents.Add(StructureDestroyed);
        AoWHSMap.FArmyChangedEvents.Add(ArmyChanged);
        AoWHSMap.FArmyDestroyedEvents.Add(ArmyDestroyed);
        AoWHSMap.FUnitChangedEvents.Add(UnitChanged);
        AoWHSMap.FUnitDestroyedEvents.Add(UnitDestroyed);
        AoWHSMap.FGameTerminatedEvents.Add(GameOver);
        AoWHSMap.FGameOverEvents.Add(GameTerminated);
        AoWHSMap.FHSChangedEvents.Add(HSChanged);
        AoWHSMap.FPlayerTerminatedEvents.Add(PlayerTerminated);
        AoWHSMap.FExecuteEventLogEvents.Add(ExecuteEventLog);
        AoWHSMap.FTargetSelection.FOnChange.Add(TargetSelectionChange);
        AoWHSMap.FViewPortChangedEvents.Add(ViewPortChange);
        AoWHSMap.FCosmeticSurgeryEvents.Add(CastSpell);
        AoWHSMap.FOnActivateAbility.Add(ActivateAbility);
        AoWHSMap.FGlobalMagicControl.FChangedEvents.Add(GlobalMagicChanged);
        AoWHSMap.FPlayerChangedEvents.Add(RelationsChanged);
        AoWHSMap.FDiplomaticRelationsChangedEvents.Add(DiplomaticChange);
        AoWHSMap.FExitEvents.Add(ExitGameEvent);
        AoWHSMap.FCombatMapCreatedEvents.Add(ActivateCombatMap);
        AoWHSMap.FCombatMapDestroyedEvents.Add(DeactivateCombatMap);
        for (int I = 0, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FGemsChangedEvents.Add(GemsChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FIncomeChangedEvents.Add(IncomeChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FUpkeepChangedEvents.Add(UpkeepChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FLogbookChangedEvents.Add(LogbookChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FMagicControl.FChangedEvents.Add(PlayerMagicChanged);
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHSM()
    {
        AoWHSMap.FMouse.FOnDblClick.Remove(DblClick);
        AoWHSMap.FNewDayEvents.Remove(ChangeDay);
        AoWHSMap.FCurrentPlayerChangedEvents.Remove(PlayerChanged);
        AoWHSMap.FNewTurnEvents.Remove(NewTurn);
        AoWHSMap.FEndTurnEvents.Remove(EndTurn);
        AoWHSMap.FSeatedPlayerChangedEvents.Remove(SeatedPlayerChanged);
        AoWHSMap.FSelection.FOnSelect.Remove(SelectHS);
        AoWHSMap.FSelection.FOnDeselect.Remove(DeselectHS);
        AoWHSMap.FShowMessageEvents.Remove(ShowMessage);
        AoWHSMap.FMapChangedEvents.Remove(MapChanged);
        AoWHSMap.FScannerIconsChangedEvents.Remove(ScannerIconsChanged);
        AoWHSMap.FUpdateVisibilityEvents.Remove(MapSettingsChanged);
        AoWHSMap.FCombatEvents.Remove(CombatEvent);
        AoWHSMap.FArmyChangedEvents.Remove(ArmyChanged);
        AoWHSMap.FArmyDestroyedEvents.Remove(ArmyDestroyed);
        AoWHSMap.FUnitChangedEvents.Remove(UnitChanged);
        AoWHSMap.FUnitDestroyedEvents.Remove(UnitDestroyed);
        AoWHSMap.FStructureChangedEvents.Remove(StructureChanged);
        AoWHSMap.FStructureDestroyedEvents.Remove(StructureDestroyed);
        AoWHSMap.FHSChangedEvents.Remove(HSChanged);
        AoWHSMap.FGameTerminatedEvents.Remove(GameOver);
        AoWHSMap.FGameOverEvents.Remove(GameTerminated);
        AoWHSMap.FPlayerTerminatedEvents.Remove(PlayerTerminated);
        AoWHSMap.FExecuteEventLogEvents.Remove(ExecuteEventLog);
        AoWHSMap.FTargetSelection.FOnChange.Remove(TargetSelectionChange);
        AoWHSMap.FViewPortChangedEvents.Remove(ViewPortChange);
        AoWHSMap.FCosmeticSurgeryEvents.Remove(CastSpell);
        AoWHSMap.FOnActivateAbility.Remove(ActivateAbility);
        AoWHSMap.FGlobalMagicControl.FChangedEvents.Remove(GlobalMagicChanged);
        AoWHSMap.FPlayerChangedEvents.Remove(RelationsChanged);
        AoWHSMap.FDiplomaticRelationsChangedEvents.Remove(DiplomaticChange);
        AoWHSMap.FExitEvents.Remove(ExitGameEvent);
        AoWHSMap.FCombatMapCreatedEvents.Remove(ActivateCombatMap);
        AoWHSMap.FCombatMapDestroyedEvents.Remove(DeactivateCombatMap);
        for (int I = 0, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FGemsChangedEvents.Remove(GemsChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FIncomeChangedEvents.Remove(IncomeChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FUpkeepChangedEvents.Remove(UpkeepChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FLogbookChangedEvents.Remove(LogbookChanged);
            ((TPlayer)(object)(TObject)AoWHSMap.FPlayers.GetPlayers(I)).FMagicControl.FChangedEvents.Remove(PlayerMagicChanged);
        }
    }

    // MapCtrl.pas:291
    public void ActivateHSM(TObject Map)
    {
        if (((TEObject)Map).EClass.ClassID == 131089)
        {
            LinkToHSM();
            if (FOnActivateHSM != null)
            {
                FOnActivateHSM(this);
            }
        }
    }

    // MapCtrl.pas:300
    public void DeactivateHSM(TObject Map)
    {
        if (((TEObject)Map).EClass.ClassID == 131089)
        {
            if (FOnDeactivateHSM != null)
            {
                FOnDeactivateHSM(this);
            }

            ReleaseHSM();
        }
    }

    // MapCtrl.pas:309
    public void DblClick(TObject Sender)
    {
        THexagonSprite HS;
        if (FOnDblClickArmy != null)
        {
            HS = AoWHSMap.Selection.FindClass(typeof(TArmyHS));
            if (HS is TArmyHS armyHS)
            {
                FOnDblClickArmy(armyHS.FArmy);
            }
        }

        if (FOnDblClickStructure != null)
        {
            HS = AoWHSMap.Selection.FindClass(typeof(TStructure));
            if (HS is TStructure structure)
            {
                FOnDblClickStructure(structure);
            }
        }
    }

    // MapCtrl.pas:350
    public void MapChanged(TObject Sender)
    {
        try
        {
            if (FOnMapChanged != null && Sender is MapChangedNotification mapChanged)
            {
                FOnMapChanged(ref mapChanged.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:359
    public void ScannerIconsChanged(TObject Sender)
    {
        try
        {
            if (FOnScannerIconsChanged != null && Sender is MapChangedNotification scannerChanged)
            {
                FOnScannerIconsChanged(ref scannerChanged.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void MapSettingsChanged(TObject Sender)
    {
        try
        {
            if (FOnMapSettingsChanged != null)
            {
                FOnMapSettingsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapSettingsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:324
    public void SelectHS(TObject Sender)
    {
        THexagonSpriteSelection Selection;
        try
        {
            Selection = AoWHSMap.Selection;
            if (FOnSelectArmy != null && Selection.EventHS is TArmyHS armyHS && AoWHSMap.FSelectedArmy.GetArmyHS() != null)
            {
                FOnSelectArmy(armyHS.FArmy);
            }

            if (FOnSelectStructure != null && Selection.EventHS is TStructure structure)
            {
                FOnSelectStructure(structure);
            }

            if (FOnSelectItem != null && Selection.EventHS is TItemHS item)
            {
                FOnSelectItem(item);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during SelectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:337
    public void DeselectHS(TObject Sender)
    {
        THexagonSpriteSelection Selection;
        try
        {
            Selection = AoWHSMap.Selection;
            if (FOnDeselectArmy != null && Selection.EventHS is TArmyHS armyHS)
            {
                FOnDeselectArmy(armyHS.FArmy);
            }

            if (FOnDeselectStructure != null && Selection.EventHS is TStructure structure)
            {
                FOnDeselectStructure(structure);
            }

            if (FOnDeselectItem != null && Selection.EventHS is TItemHS item)
            {
                FOnDeselectItem(item);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during DeselectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:377
    public void PlayerChanged(TObject Sender)
    {
        try
        {
            if (FOnPlayerChanged != null)
            {
                FOnPlayerChanged(AoWHSMap.FCurrentPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:386
    public void ExitGameEvent(TObject Sender)
    {
        try
        {
            if (FOnExitGameEvent != null)
            {
                FOnExitGameEvent(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ExitGame event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:395
    public void NewTurn(TObject Sender)
    {
        try
        {
            if (FOnNewTurn != null)
            {
                FOnNewTurn(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during NewTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:404
    public void EndTurn(TObject Player)
    {
        try
        {
            if (FOnEndTurn != null)
            {
                var player = (TObject)AoWHSMap.FPlayers.GetPlayers(ObjectToInt(Player));
                FOnEndTurn((sbyte)ObjectToInt(Player), !((TPlayer)(object)player).FSingleProcessCycle);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void SeatedPlayerChanged(TObject Sender)
    {
        try
        {
            if (FOnSeatedPlayerChanged != null)
            {
                FOnSeatedPlayerChanged(AoWHSMap.FSeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:422
    public void GemsChanged(TObject Player)
    {
        try
        {
            if (FOnGemsChanged != null)
            {
                FOnGemsChanged(((TPlayer)(object)Player).FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void IncomeChanged(TObject Player)
    {
        try
        {
            if (FOnIncomeChanged != null)
            {
                FOnIncomeChanged(((TPlayer)(object)Player).FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void UpkeepChanged(TObject Player)
    {
        try
        {
            if (FOnUpkeepChanged != null)
            {
                FOnUpkeepChanged(((TPlayer)(object)Player).FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void PlayerMagicChanged(TObject Control)
    {
        try
        {
            if (FOnPlayerMagicChanged != null)
            {
                FOnPlayerMagicChanged(((TPlayerMagicControl)(object)Control).FPlayer.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Player Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:458
    public void GlobalMagicChanged(TObject Sender)
    {
        try
        {
            if (FOnGlobalMagicChanged != null)
            {
                FOnGlobalMagicChanged(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:467
    public void ShowMessage(TObject Sender)
    {
        try
        {
            if (FOnShowMessage != null && Sender is ShowMessageNotification message)
            {
                FOnShowMessage(ref message.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ShowMessage event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:476
    public void ChangeDay(TObject Sender)
    {
        try
        {
            if (FOnChangeDay != null)
            {
                FOnChangeDay(AoWHSMap.FDay);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ChangeDay event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:485
    public void CombatEvent(TObject Sender)
    {
        try
        {
            if (FOnCombatEvent != null)
            {
                FOnCombatEvent(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during on combat event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:494
    public void LogbookChanged(TObject Player)
    {
        try
        {
            if (FOnLogbookChanged != null)
            {
                FOnLogbookChanged(((TPlayer)(object)Player).FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during LogbookChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:503
    public void StructureChanged(TObject Obj)
    {
        try
        {
            Raise(FOnStructureChanged, Obj);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:512
    public void StructureDestroyed(TObject Obj)
    {
        try
        {
            Raise(FOnStructureDestroyed, Obj);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:521
    public void ArmyChanged(TObject Obj)
    {
        try
        {
            Raise(FOnArmyChanged, Obj);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:530
    public void ArmyDestroyed(TObject Obj)
    {
        try
        {
            Raise(FOnArmyDestroyed, Obj);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:539
    public void HSChanged(TObject Sender)
    {
        try
        {
            Raise(FOnHSChanged, Sender);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnHSChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:548
    public void UnitChanged(TObject Obj)
    {
        try
        {
            Raise(FOnUnitChanged, Obj);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void UnitDestroyed(TObject Obj)
    {
        try
        {
            Raise(FOnUnitDestroyed, Obj);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void GameOver(TObject Sender)
    {
        try
        {
            if (FOnGameOver != null)
            {
                FOnGameOver(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnGameOver event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:575
    public void GameTerminated(TObject Sender)
    {
        try
        {
            if (FOnGameTerminated != null)
            {
                FOnGameTerminated((sbyte)AoWHSMap.FGameResult);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnGameTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:584
    public void PlayerTerminated(TObject Sender)
    {
        try
        {
            if (FOnPlayerTerminated != null)
            {
                FOnPlayerTerminated((sbyte)ObjectToInt(Sender));
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void ExecuteEventLog(TObject Info)
    {
        try
        {
            if (FOnExecuteEventLog != null)
            {
                FOnExecuteEventLog(((ExecuteEventLogNotification)Info).Info.Log, ((ExecuteEventLogNotification)Info).Info.Immediate, ((ExecuteEventLogNotification)Info).Info.Done);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void ActivateCombatMap(TObject Sender)
    {
        try
        {
            if (FOnActivateCombatMap != null)
            {
                FOnActivateCombatMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:612
    public void DeactivateCombatMap(TObject Sender)
    {
        try
        {
            if (FOnDeactivateCombatMap != null)
            {
                FOnDeactivateCombatMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:621
    public void TargetSelectionChange(TObject Control)
    {
        try
        {
            if (FOnTargetSelectionChange != null && Control is TTargetSelectionControl control && (((int)AoWHSMap.FTargetSelection.FTargetMode.Image) & 8) != 0)
            {
                FOnTargetSelectionChange(control, control.FChange);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:630
    public void CastSpell(TObject Sender)
    {
        try
        {
            Raise(FOnCastSpell, Sender);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnCastSpell event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:639
    public void ViewPortChange(TObject Sender)
    {
        try
        {
            if (FOnViewPortChange != null)
            {
                FOnViewPortChange(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnViewPortChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:648
    public void ActivateAbility(TObject Sender)
    {
        try
        {
            Raise(FOnActivateAbility, Sender);
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void DiplomaticChange(TObject Sender)
    {
        try
        {
            if (FOnDiplomaticChange != null)
            {
                FOnDiplomaticChange(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:666
    public void RelationsChanged(TObject Sender)
    {
        try
        {
            if (FOnRelationsChanged != null)
            {
                FOnRelationsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnRelationsChanged event (TAoWMapControl)");
        }
    }

    // A null payload stays null. Any other object is delivered when it is the event's class.
    static void Raise<T>(TMapObjectEvent<T> handler, TObject value) where T : class
    {
        if (handler == null)
        {
            return;
        }

        if (value == null)
        {
            handler(null);
            return;
        }

        if (value is T typed)
        {
            handler(typed);
        }
    }

    [Published]
    public TMapObjectEvent<TArmy> OnArmySelected { get => FOnSelectArmy; set => FOnSelectArmy = value; }

    [Published]
    public TMapObjectEvent<TArmy> OnArmyDeselected { get => FOnDeselectArmy; set => FOnDeselectArmy = value; }

    [Published]
    public TMapObjectEvent<TArmy> OnArmyDblClick { get => FOnDblClickArmy; set => FOnDblClickArmy = value; }

    [Published]
    public TMapObjectEvent<TArmy> OnArmyChanged { get => FOnArmyChanged; set => FOnArmyChanged = value; }

    [Published]
    public TMapObjectEvent<TArmy> OnArmyDestroyed { get => FOnArmyDestroyed; set => FOnArmyDestroyed = value; }

    [Published]
    public TMapObjectEvent<TStructure> OnStructureSelected { get => FOnSelectStructure; set => FOnSelectStructure = value; }

    [Published]
    public TMapObjectEvent<TStructure> OnStructureDeselected { get => FOnDeselectStructure; set => FOnDeselectStructure = value; }

    [Published]
    public TMapObjectEvent<TStructure> OnStructureDblClick { get => FOnDblClickStructure; set => FOnDblClickStructure = value; }

    [Published]
    public TMapObjectEvent<TStructure> OnStructureChanged { get => FOnStructureChanged; set => FOnStructureChanged = value; }

    [Published]
    public TMapObjectEvent<TStructure> OnStructureDestroyed { get => FOnStructureDestroyed; set => FOnStructureDestroyed = value; }

    [Published]
    public TMapObjectEvent<TAbstractUnit> OnUnitChanged { get => FOnUnitChanged; set => FOnUnitChanged = value; }

    [Published]
    public TMapObjectEvent<TAbstractUnit> OnUnitDestroyed { get => FOnUnitDestroyed; set => FOnUnitDestroyed = value; }

    [Published]
    public TNotifyEvent<TObject> OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

    [Published]
    public TNotifyEvent<TObject> OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

    [Published]
    public TMapPlayerEvent OnGemsChanged { get => FOnGemsChanged; set => FOnGemsChanged = value; }

    [Published]
    public TMapPlayerEvent OnIncomeChanged { get => FOnIncomeChanged; set => FOnIncomeChanged = value; }

    [Published]
    public TMapPlayerEvent OnUpkeepChanged { get => FOnUpkeepChanged; set => FOnUpkeepChanged = value; }

    [Published]
    public TMapRefEvent<TShowMessageInfo> OnShowMessage { get => FOnShowMessage; set => FOnShowMessage = value; }

    [Published]
    public TMapDayEvent OnDayChanged { get => FOnChangeDay; set => FOnChangeDay = value; }

    [Published]
    public TMapPlayerEvent OnPlayerChanged { get => FOnPlayerChanged; set => FOnPlayerChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnNewTurn { get => FOnNewTurn; set => FOnNewTurn = value; }

    [Published]
    public TMapEndTurnEvent OnEndTurn { get => FOnEndTurn; set => FOnEndTurn = value; }

    [Published]
    public TMapObjectEvent<THexagonSprite> OnHSChanged { get => FOnHSChanged; set => FOnHSChanged = value; }

    [Published]
    public TMapPlayerEvent OnSeatedPlayerChanged { get => FOnSeatedPlayerChanged; set => FOnSeatedPlayerChanged = value; }

    [Published]
    public TMapRefEvent<TMapChangedInfo> OnMapChanged { get => FOnMapChanged; set => FOnMapChanged = value; }

    [Published]
    public TMapRefEvent<TMapChangedInfo> OnScannerIconsChanged { get => FOnScannerIconsChanged; set => FOnScannerIconsChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnMapSettingsChanged { get => FOnMapSettingsChanged; set => FOnMapSettingsChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnCombat { get => FOnCombatEvent; set => FOnCombatEvent = value; }

    [Published]
    public TMapPlayerEvent OnLogbookChanged { get => FOnLogbookChanged; set => FOnLogbookChanged = value; }

    [Published]
    public TMapTargetEvent OnTargetSelectionChange { get => FOnTargetSelectionChange; set => FOnTargetSelectionChange = value; }

    [Published]
    public TMapObjectEvent<TSpellCaster> OnCastSpell { get => FOnCastSpell; set => FOnCastSpell = value; }

    [Published]
    public TMapObjectEvent<TActivateAbilityInfo> OnActivateAbility { get => FOnActivateAbility; set => FOnActivateAbility = value; }

    [Published]
    public TMapPlayerEvent OnPlayerMagicChanged { get => FOnPlayerMagicChanged; set => FOnPlayerMagicChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnGlobalMagicChanged { get => FOnGlobalMagicChanged; set => FOnGlobalMagicChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnRelationsChanged { get => FOnRelationsChanged; set => FOnRelationsChanged = value; }

    [Published]
    public TNotifyEvent<TObject> OnActivateCombatMap { get => FOnActivateCombatMap; set => FOnActivateCombatMap = value; }

    [Published]
    public TNotifyEvent<TObject> OnDeactivateCombatMap { get => FOnDeactivateCombatMap; set => FOnDeactivateCombatMap = value; }

    [Published]
    public TMapObjectEvent<TItemHS> OnItemsSelected { get => FOnSelectItem; set => FOnSelectItem = value; }

    [Published]
    public TMapObjectEvent<TItemHS> OnItemsDeselected { get => FOnDeselectItem; set => FOnDeselectItem = value; }

    [Published]
    public TNotifyEvent<TObject> OnGameOver { get => FOnGameOver; set => FOnGameOver = value; }

    [Published]
    public TMapPlayerEvent OnGameTerminated { get => FOnGameTerminated; set => FOnGameTerminated = value; }

    [Published]
    public TMapPlayerEvent OnPlayerTerminated { get => FOnPlayerTerminated; set => FOnPlayerTerminated = value; }

    [Published]
    public TMapExecuteLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

    [Published]
    public TNotifyEvent<TObject> OnViewPortChange { get => FOnViewPortChange; set => FOnViewPortChange = value; }

    [Published]
    public TNotifyEvent<TObject> OnDiplomaticChange { get => FOnDiplomaticChange; set => FOnDiplomaticChange = value; }

    [Published]
    public TNotifyEvent<TObject> OnExitGame { get => FOnExitGameEvent; set => FOnExitGameEvent = value; }
}

public delegate void TMapEvent();
public delegate void TMapPlayerEvent(sbyte Player);
public delegate void TMapDayEvent(int Day);
public delegate void TMapObjectEvent<in TSender>(TSender Obj);
public delegate void TMapRefEvent<TInfo>(ref TInfo Info);
public delegate void TMapEndTurnEvent(sbyte Player, bool Automatic);
public delegate void TMapExecuteLogEvent(TEventLog Log, bool Immediate, TExecuteDoneEvent Done);
public delegate void TMapTargetEvent(TTargetSelectionControl Control, TTargetSelectionChange Change);

public struct TExecuteLogInfo
{
    public TEventLog Log;
    public object Data2;
    public object Data1;
    public byte Trigger;
}

public struct TRawObject
{
    public FixedArray_object_512 Fields;
}

[StructLayout(LayoutKind.Explicit, Size = 376)]
public struct TMapState
{
    [FieldOffset(0)]
    public FixedArray_byte_164 Reserved00;
    [FieldOffset(164)]
    public sbyte CurrentPlayer;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
    [FieldOffset(166)]
    public FixedArray_byte_115 ReservedA6;
    [FieldOffset(281)]
    public sbyte Result;
    [FieldOffset(282)]
    public FixedArray_byte_90 Reserved11A;
    [FieldOffset(372)]
    public int Day;
}

[StructLayout(LayoutKind.Explicit, Size = 167)]
public struct TPlayerState
{
    [FieldOffset(0)]
    public FixedArray_byte_44 Reserved00;
    [FieldOffset(44)]
    public bool Human;
    [FieldOffset(45)]
    public FixedArray_byte_121 Reserved2D;
    [FieldOffset(166)]
    public sbyte PlayerID;
}

[StructLayout(LayoutKind.Explicit, Size = 57)]
public struct TTargetState
{
    [FieldOffset(0)]
    public FixedArray_byte_13 Reserved00;
    [FieldOffset(13)]
    public byte Status;
    [FieldOffset(14)]
    public FixedArray_byte_42 Reserved0E;
    [FieldOffset(56)]
    public byte Mode;
}

public delegate TObject TGetPlayers(TPlayerList List, int Index);
public delegate TArmyHS TGetArmyHS(TSelectedArmy Selection);
public delegate bool TValidateEngine();

public static partial class MapCtrlUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
