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
    public TNotifyEvent<object> FOnDiplomaticChange;
    public TNotifyEvent<object> FOnActivateHSM;
    public TNotifyEvent<object> FOnDeactivateHSM;
    public TMapPlayerEvent FOnIncomeChanged;
    public TMapPlayerEvent FOnUpkeepChanged;
    public TMapPlayerEvent FOnGemsChanged;
    public TMapPlayerEvent FOnPlayerMagicChanged;
    public TNotifyEvent<object> FOnGlobalMagicChanged;
    public TMapRefEvent<TShowMessageInfo> FOnShowMessage;
    public TMapDayEvent FOnChangeDay;
    public TNotifyEvent<sbyte> FOnNewTurn;
    public TMapPlayerEvent FOnPlayerChanged;
    public TMapPlayerEvent FOnSeatedPlayerChanged;
    public TNotifyEvent<object> FOnCombatEvent;
    public TMapExecuteLogEvent FOnExecuteEventLog;
    public TMapRefEvent<TMapChangedInfo> FOnMapChanged;
    public TMapRefEvent<TMapChangedInfo> FOnScannerIconsChanged;
    public TNotifyEvent<object> FOnMapSettingsChanged;
    public TMapPlayerEvent FOnLogbookChanged;
    public TNotifyEvent<object> FOnGameOver;
    public TMapPlayerEvent FOnGameTerminated;
    public TMapPlayerEvent FOnPlayerTerminated;
    public TNotifyEvent<object> FOnActivateCombatMap;
    public TNotifyEvent<object> FOnDeactivateCombatMap;
    public TMapTargetEvent FOnTargetSelectionChange;
    public TNotifyEvent<object> FOnViewPortChange;
    public TMapObjectEvent<TSpellCaster> FOnCastSpell;
    public TMapEndTurnEvent FOnEndTurn;
    public TNotifyEvent<object> FOnRelationsChanged;
    public TNotifyEvent<object> FOnExitGameEvent;
    public TMapObjectEvent<THexagonSprite> FOnHSChanged;

    // MapCtrl.pas:186
    public override void Dispose()
    {
        if (AoWEngine != null)
        {
            AoWEngine.FOnHSMLoaded -= ActivateHSM;
            AoWEngine.FOnHSMClose -= DeactivateHSM;
            if (AoWHSMap != null)
            {
                ReleaseHSM();
            }
        }

        base.Dispose();
    }

    // MapCtrl.pas:174
    public TAoWMapControl(TComponent owner) : base(owner)
    {
        if (ValidateAoWEngine())
        {
            AoWEngine.FOnHSMLoaded += ActivateHSM;
            AoWEngine.FOnHSMClose += DeactivateHSM;
            if (AoWHSMap != null)
            {
                LinkToHSM();
            }
        }
    }

    // MapCtrl.pas:197
    public void LinkToHSM()
    {
        AoWHSMap.FMouse.FOnDblClick += DblClick;
        AoWHSMap.FNewDayEvents += ChangeDay;
        AoWHSMap.FCurrentPlayerChangedEvents += PlayerChanged;
        AoWHSMap.FNewTurnEvents += NewTurn;
        AoWHSMap.FEndTurnEvents += EndTurn;
        AoWHSMap.FSeatedPlayerChangedEvents += SeatedPlayerChanged;
        AoWHSMap.FSelection.FOnSelect += SelectHS;
        AoWHSMap.FSelection.FOnDeselect += DeselectHS;
        AoWHSMap.FShowMessageEvents += ShowMessage;
        AoWHSMap.FMapChangedEvents += MapChanged;
        AoWHSMap.FScannerIconsChangedEvents += ScannerIconsChanged;
        AoWHSMap.FUpdateVisibilityEvents += MapSettingsChanged;
        AoWHSMap.FCombatEvents += CombatEvent;
        AoWHSMap.FStructureChangedEvents += StructureChanged;
        AoWHSMap.FStructureDestroyedEvents += StructureDestroyed;
        AoWHSMap.FArmyChangedEvents += ArmyChanged;
        AoWHSMap.FArmyDestroyedEvents += ArmyDestroyed;
        AoWHSMap.FUnitChangedEvents += UnitChanged;
        AoWHSMap.FUnitDestroyedEvents += UnitDestroyed;
        AoWHSMap.FGameTerminatedEvents += GameOver;
        AoWHSMap.FGameOverEvents += GameTerminated;
        AoWHSMap.FHSChangedEvents += HSChanged;
        AoWHSMap.FPlayerTerminatedEvents += PlayerTerminated;
        AoWHSMap.FExecuteEventLogEvents += ExecuteEventLog;
        AoWHSMap.FTargetSelection.FOnChange += TargetSelectionChange;
        AoWHSMap.FViewPortChangedEvents += ViewPortChange;
        AoWHSMap.FCosmeticSurgeryEvents += CastSpell;
        AoWHSMap.FOnActivateAbility += ActivateAbility;
        AoWHSMap.FGlobalMagicControl.FChangedEvents += GlobalMagicChanged;
        AoWHSMap.FPlayerChangedEvents += RelationsChanged;
        AoWHSMap.FDiplomaticRelationsChangedEvents += DiplomaticChange;
        AoWHSMap.FExitEvents += ExitGameEvent;
        AoWHSMap.FCombatMapCreatedEvents += ActivateCombatMap;
        AoWHSMap.FCombatMapDestroyedEvents += DeactivateCombatMap;
        for (int I = 0, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FGemsChangedEvents += GemsChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FIncomeChangedEvents += IncomeChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FUpkeepChangedEvents += UpkeepChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FLogbookChangedEvents += LogbookChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FMagicControl.FChangedEvents += PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHSM()
    {
        AoWHSMap.FMouse.FOnDblClick -= DblClick;
        AoWHSMap.FNewDayEvents -= ChangeDay;
        AoWHSMap.FCurrentPlayerChangedEvents -= PlayerChanged;
        AoWHSMap.FNewTurnEvents -= NewTurn;
        AoWHSMap.FEndTurnEvents -= EndTurn;
        AoWHSMap.FSeatedPlayerChangedEvents -= SeatedPlayerChanged;
        AoWHSMap.FSelection.FOnSelect -= SelectHS;
        AoWHSMap.FSelection.FOnDeselect -= DeselectHS;
        AoWHSMap.FShowMessageEvents -= ShowMessage;
        AoWHSMap.FMapChangedEvents -= MapChanged;
        AoWHSMap.FScannerIconsChangedEvents -= ScannerIconsChanged;
        AoWHSMap.FUpdateVisibilityEvents -= MapSettingsChanged;
        AoWHSMap.FCombatEvents -= CombatEvent;
        AoWHSMap.FArmyChangedEvents -= ArmyChanged;
        AoWHSMap.FArmyDestroyedEvents -= ArmyDestroyed;
        AoWHSMap.FUnitChangedEvents -= UnitChanged;
        AoWHSMap.FUnitDestroyedEvents -= UnitDestroyed;
        AoWHSMap.FStructureChangedEvents -= StructureChanged;
        AoWHSMap.FStructureDestroyedEvents -= StructureDestroyed;
        AoWHSMap.FHSChangedEvents -= HSChanged;
        AoWHSMap.FGameTerminatedEvents -= GameOver;
        AoWHSMap.FGameOverEvents -= GameTerminated;
        AoWHSMap.FPlayerTerminatedEvents -= PlayerTerminated;
        AoWHSMap.FExecuteEventLogEvents -= ExecuteEventLog;
        AoWHSMap.FTargetSelection.FOnChange -= TargetSelectionChange;
        AoWHSMap.FViewPortChangedEvents -= ViewPortChange;
        AoWHSMap.FCosmeticSurgeryEvents -= CastSpell;
        AoWHSMap.FOnActivateAbility -= ActivateAbility;
        AoWHSMap.FGlobalMagicControl.FChangedEvents -= GlobalMagicChanged;
        AoWHSMap.FPlayerChangedEvents -= RelationsChanged;
        AoWHSMap.FDiplomaticRelationsChangedEvents -= DiplomaticChange;
        AoWHSMap.FExitEvents -= ExitGameEvent;
        AoWHSMap.FCombatMapCreatedEvents -= ActivateCombatMap;
        AoWHSMap.FCombatMapDestroyedEvents -= DeactivateCombatMap;
        for (int I = 0, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FGemsChangedEvents -= GemsChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FIncomeChangedEvents -= IncomeChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FUpkeepChangedEvents -= UpkeepChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FLogbookChangedEvents -= LogbookChanged;
            ((TPlayer)(object)(object)AoWHSMap.FPlayers.GetPlayers(I)).FMagicControl.FChangedEvents -= PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:291
    public void ActivateHSM(THSMap Map)
    {
        if (Map.EClass.ClassID == 131089)
        {
            LinkToHSM();
            if (FOnActivateHSM != null)
            {
                FOnActivateHSM(this);
            }
        }
    }

    // MapCtrl.pas:300
    public void DeactivateHSM(THSMap Map)
    {
        if (Map.EClass.ClassID == 131089)
        {
            if (FOnDeactivateHSM != null)
            {
                FOnDeactivateHSM(this);
            }

            ReleaseHSM();
        }
    }

    // MapCtrl.pas:309
    public void DblClick(object Sender)
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
    public void MapChanged(MapChangedNotification Sender)
    {
        try
        {
            if (FOnMapChanged != null)
            {
                FOnMapChanged(ref Sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:359
    public void ScannerIconsChanged(MapChangedNotification Sender)
    {
        try
        {
            if (FOnScannerIconsChanged != null)
            {
                FOnScannerIconsChanged(ref Sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void MapSettingsChanged(object Sender)
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
    public void SelectHS(object Sender)
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
    public void DeselectHS(object Sender)
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
    public void PlayerChanged(object Sender)
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
    public void ExitGameEvent(object Sender)
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
    public void NewTurn(sbyte player)
    {
        try
        {
            if (FOnNewTurn != null)
            {
                FOnNewTurn(player);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during NewTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:404
    public void EndTurn(sbyte player)
    {
        try
        {
            if (FOnEndTurn != null)
            {
                var listed = (object)AoWHSMap.FPlayers.GetPlayers(player);
                FOnEndTurn(player, !((TPlayer)(object)listed).FSingleProcessCycle);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void SeatedPlayerChanged(object Sender)
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
    public void GemsChanged(TPlayer Player)
    {
        try
        {
            if (FOnGemsChanged != null)
            {
                FOnGemsChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void IncomeChanged(TPlayer Player)
    {
        try
        {
            if (FOnIncomeChanged != null)
            {
                FOnIncomeChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void UpkeepChanged(TPlayer Player)
    {
        try
        {
            if (FOnUpkeepChanged != null)
            {
                FOnUpkeepChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void PlayerMagicChanged(TPlayerMagicControl Control)
    {
        try
        {
            if (FOnPlayerMagicChanged != null)
            {
                FOnPlayerMagicChanged(Control.FPlayer.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Player Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:458
    public void GlobalMagicChanged(object Sender)
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
    public void ShowMessage(ShowMessageNotification Sender)
    {
        try
        {
            if (FOnShowMessage != null)
            {
                FOnShowMessage(ref Sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ShowMessage event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:476
    public void ChangeDay(object Sender)
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
    public void CombatEvent(object Sender)
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
    public void LogbookChanged(TPlayer Player)
    {
        try
        {
            if (FOnLogbookChanged != null)
            {
                FOnLogbookChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during LogbookChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:503
    public void StructureChanged(TStructure Obj)
    {
        try
        {
            if (FOnStructureChanged != null)
            {
                FOnStructureChanged(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:512
    public void StructureDestroyed(TStructure Obj)
    {
        try
        {
            if (FOnStructureDestroyed != null)
            {
                FOnStructureDestroyed(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:521
    public void ArmyChanged(TArmy Obj)
    {
        try
        {
            if (FOnArmyChanged != null)
            {
                FOnArmyChanged(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:530
    public void ArmyDestroyed(TArmy Obj)
    {
        try
        {
            if (FOnArmyDestroyed != null)
            {
                FOnArmyDestroyed(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:539
    public void HSChanged(THexagonSprite Sender)
    {
        try
        {
            if (FOnHSChanged != null)
            {
                FOnHSChanged(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnHSChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:548
    public void UnitChanged(TAbstractUnit Obj)
    {
        try
        {
            if (FOnUnitChanged != null)
            {
                FOnUnitChanged(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void UnitDestroyed(TAbstractUnit Obj)
    {
        try
        {
            if (FOnUnitDestroyed != null)
            {
                FOnUnitDestroyed(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void GameOver(object Sender)
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
    public void GameTerminated(object Sender)
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
    public void PlayerTerminated(sbyte player)
    {
        try
        {
            if (FOnPlayerTerminated != null)
            {
                FOnPlayerTerminated(player);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void ExecuteEventLog(ExecuteEventLogNotification Info)
    {
        try
        {
            if (FOnExecuteEventLog != null)
            {
                FOnExecuteEventLog(Info.Info.Log, Info.Info.Immediate, Info.Info.Done);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void ActivateCombatMap(object Sender)
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
    public void DeactivateCombatMap(object Sender)
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
    public void TargetSelectionChange(TTargetSelectionControl Control)
    {
        try
        {
            if (FOnTargetSelectionChange != null && Control != null && (((int)AoWHSMap.FTargetSelection.FTargetMode.Image) & 8) != 0)
            {
                FOnTargetSelectionChange(Control, Control.FChange);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:630
    public void CastSpell(TSpellCaster Sender)
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
            RtlUnit.ShowMessage("Exception occurred during OnCastSpell event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:639
    public void ViewPortChange(object Sender)
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
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void DiplomaticChange(object Sender)
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
    public void RelationsChanged(object Sender)
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
    public TNotifyEvent<object> OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

    [Published]
    public TNotifyEvent<object> OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

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
    public TNotifyEvent<sbyte> OnNewTurn { get => FOnNewTurn; set => FOnNewTurn = value; }

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
    public TNotifyEvent<object> OnMapSettingsChanged { get => FOnMapSettingsChanged; set => FOnMapSettingsChanged = value; }

    [Published]
    public TNotifyEvent<object> OnCombat { get => FOnCombatEvent; set => FOnCombatEvent = value; }

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
    public TNotifyEvent<object> OnGlobalMagicChanged { get => FOnGlobalMagicChanged; set => FOnGlobalMagicChanged = value; }

    [Published]
    public TNotifyEvent<object> OnRelationsChanged { get => FOnRelationsChanged; set => FOnRelationsChanged = value; }

    [Published]
    public TNotifyEvent<object> OnActivateCombatMap { get => FOnActivateCombatMap; set => FOnActivateCombatMap = value; }

    [Published]
    public TNotifyEvent<object> OnDeactivateCombatMap { get => FOnDeactivateCombatMap; set => FOnDeactivateCombatMap = value; }

    [Published]
    public TMapObjectEvent<TItemHS> OnItemsSelected { get => FOnSelectItem; set => FOnSelectItem = value; }

    [Published]
    public TMapObjectEvent<TItemHS> OnItemsDeselected { get => FOnDeselectItem; set => FOnDeselectItem = value; }

    [Published]
    public TNotifyEvent<object> OnGameOver { get => FOnGameOver; set => FOnGameOver = value; }

    [Published]
    public TMapPlayerEvent OnGameTerminated { get => FOnGameTerminated; set => FOnGameTerminated = value; }

    [Published]
    public TMapPlayerEvent OnPlayerTerminated { get => FOnPlayerTerminated; set => FOnPlayerTerminated = value; }

    [Published]
    public TMapExecuteLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

    [Published]
    public TNotifyEvent<object> OnViewPortChange { get => FOnViewPortChange; set => FOnViewPortChange = value; }

    [Published]
    public TNotifyEvent<object> OnDiplomaticChange { get => FOnDiplomaticChange; set => FOnDiplomaticChange = value; }

    [Published]
    public TNotifyEvent<object> OnExitGame { get => FOnExitGameEvent; set => FOnExitGameEvent = value; }
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

public delegate object TGetPlayers(TPlayerList List, int Index);
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
