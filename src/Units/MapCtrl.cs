// Translated from MapCtrl.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.MapCtrl
{
    public partial class TAoWMapControl : Aow.Rtl.TComponent
    {
        public Aow.Units.MapCtrl.TMapObjectEvent FOnSelectArmy;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnDeselectArmy;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnDblClickArmy;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnArmyChanged;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnArmyDestroyed;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnSelectStructure;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnDeselectStructure;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnDblClickStructure;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnStructureChanged;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnStructureDestroyed;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnSelectItem;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnDeselectItem;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnUnitChanged;
        public Aow.Units.MapCtrl.TMapObjectEvent FOnUnitDestroyed;
        public Aow.Rtl.TNotifyEvent FOnActivateAbility;
        public Aow.Rtl.TNotifyEvent FOnDiplomaticChange;
        public Aow.Rtl.TNotifyEvent FOnActivateHSM;
        public Aow.Rtl.TNotifyEvent FOnDeactivateHSM;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnIncomeChanged;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnUpkeepChanged;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnGemsChanged;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnPlayerMagicChanged;
        public Aow.Rtl.TNotifyEvent FOnGlobalMagicChanged;
        public Aow.Rtl.TNotifyEvent FOnShowMessage;
        public Aow.Units.MapCtrl.TMapDayEvent FOnChangeDay;
        public Aow.Rtl.TNotifyEvent FOnNewTurn;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnPlayerChanged;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnSeatedPlayerChanged;
        public Aow.Rtl.TNotifyEvent FOnCombatEvent;
        public Aow.Units.MapCtrl.TMapExecuteLogEvent FOnExecuteEventLog;
        public Aow.Rtl.TNotifyEvent FOnMapChanged;
        public Aow.Rtl.TNotifyEvent FOnScannerIconsChanged;
        public Aow.Rtl.TNotifyEvent FOnMapSettingsChanged;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnLogbookChanged;
        public Aow.Rtl.TNotifyEvent FOnGameOver;
        public Aow.Units.MapCtrl.TMapPlayerEvent FOnGameTerminated;
        public Aow.Rtl.TNotifyEvent FOnPlayerTerminated;
        public Aow.Rtl.TNotifyEvent FOnActivateCombatMap;
        public Aow.Rtl.TNotifyEvent FOnDeactivateCombatMap;
        public Aow.Units.MapCtrl.TMapTargetEvent FOnTargetSelectionChange;
        public Aow.Rtl.TNotifyEvent FOnViewPortChange;
        public Aow.Rtl.TNotifyEvent FOnCastSpell;
        public Aow.Units.MapCtrl.TMapEndTurnEvent FOnEndTurn;
        public Aow.Rtl.TNotifyEvent FOnRelationsChanged;
        public Aow.Rtl.TNotifyEvent FOnExitGameEvent;
        public Aow.Rtl.TNotifyEvent FOnHSChanged;
        // MapCtrl.pas:186
        public override void Destroy()
        {
            if (Aow.Units.AoWE.AoWE.AoWEngine != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.FOnHSMLoaded.Remove(ActivateHSM);
                Aow.Units.AoWE.AoWE.AoWEngine.FOnHSMClose.Remove(DeactivateHSM);
                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                {
                    ReleaseHSM();
                }
            }

            base.Destroy();
        }

        // MapCtrl.pas:174
        public TAoWMapControl(TComponent owner) : base(owner)
        {
            if (Aow.Units.AoWE.AoWE.ValidateAoWEngine())
            {
                Aow.Units.AoWE.AoWE.AoWEngine.FOnHSMLoaded.Add(ActivateHSM);
                Aow.Units.AoWE.AoWE.AoWEngine.FOnHSMClose.Add(DeactivateHSM);
                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                {
                    LinkToHSM();
                }
            }
        }

        // MapCtrl.pas:197
        public void LinkToHSM()
        {
            int I = default;
            Aow.Units.AoWE.AoWE.AoWHSMap.FMouse.FOnDblClick.Add(DblClick);
            Aow.Units.AoWE.AoWE.AoWHSMap.FNewDayEvents.Add(ChangeDay);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayerChangedEvents.Add(PlayerChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FNewTurnEvents.Add(NewTurn);
            Aow.Units.AoWE.AoWE.AoWHSMap.FEndTurnEvents.Add(EndTurn);
            Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayerChangedEvents.Add(SeatedPlayerChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelection.FOnSelect.Add(SelectHS);
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelection.FOnDeselect.Add(DeselectHS);
            Aow.Units.AoWE.AoWE.AoWHSMap.FShowMessageEvents.Add(ShowMessage);
            Aow.Units.AoWE.AoWE.AoWHSMap.FMapChangedEvents.Add(MapChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FScannerIconsChangedEvents.Add(ScannerIconsChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FUpdateVisibilityEvents.Add(MapSettingsChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCombatEvents.Add(CombatEvent);
            Aow.Units.AoWE.AoWE.AoWHSMap.FStructureChangedEvents.Add(StructureChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FStructureDestroyedEvents.Add(StructureDestroyed);
            Aow.Units.AoWE.AoWE.AoWHSMap.FArmyChangedEvents.Add(ArmyChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FArmyDestroyedEvents.Add(ArmyDestroyed);
            Aow.Units.AoWE.AoWE.AoWHSMap.FUnitChangedEvents.Add(UnitChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FUnitDestroyedEvents.Add(UnitDestroyed);
            Aow.Units.AoWE.AoWE.AoWHSMap.FGameTerminatedEvents.Add(GameOver);
            Aow.Units.AoWE.AoWE.AoWHSMap.FGameOverEvents.Add(GameTerminated);
            Aow.Units.AoWE.AoWE.AoWHSMap.FHSChangedEvents.Add(HSChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerTerminatedEvents.Add(PlayerTerminated);
            Aow.Units.AoWE.AoWE.AoWHSMap.FExecuteEventLogEvents.Add(ExecuteEventLog);
            Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FOnChange.Add(TargetSelectionChange);
            Aow.Units.AoWE.AoWE.AoWHSMap.FViewPortChangedEvents.Add(ViewPortChange);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCosmeticSurgeryEvents.Add(CastSpell);
            Aow.Units.AoWE.AoWE.AoWHSMap.FOnActivateAbility.Add(ActivateAbility);
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FChangedEvents.Add(GlobalMagicChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerChangedEvents.Add(RelationsChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FDiplomaticRelationsChangedEvents.Add(DiplomaticChange);
            Aow.Units.AoWE.AoWE.AoWHSMap.FExitEvents.Add(ExitGameEvent);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMapCreatedEvents.Add(ActivateCombatMap);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMapDestroyedEvents.Add(DeactivateCombatMap);
            {
                I = 0;
                int end__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__2)).FGemsChangedEvents.Add(GemsChanged);
                    var call__3 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__3)).FIncomeChangedEvents.Add(IncomeChanged);
                    var call__4 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__4)).FUpkeepChangedEvents.Add(UpkeepChanged);
                    var call__5 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__5)).FLogbookChangedEvents.Add(LogbookChanged);
                    var call__6 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__6)).FMagicControl.FChangedEvents.Add(PlayerMagicChanged);
                }
            }
        }

        // MapCtrl.pas:244
        public void ReleaseHSM()
        {
            int I = default;
            Aow.Units.AoWE.AoWE.AoWHSMap.FMouse.FOnDblClick.Remove(DblClick);
            Aow.Units.AoWE.AoWE.AoWHSMap.FNewDayEvents.Remove(ChangeDay);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayerChangedEvents.Remove(PlayerChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FNewTurnEvents.Remove(NewTurn);
            Aow.Units.AoWE.AoWE.AoWHSMap.FEndTurnEvents.Remove(EndTurn);
            Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayerChangedEvents.Remove(SeatedPlayerChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelection.FOnSelect.Remove(SelectHS);
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelection.FOnDeselect.Remove(DeselectHS);
            Aow.Units.AoWE.AoWE.AoWHSMap.FShowMessageEvents.Remove(ShowMessage);
            Aow.Units.AoWE.AoWE.AoWHSMap.FMapChangedEvents.Remove(MapChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FScannerIconsChangedEvents.Remove(ScannerIconsChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FUpdateVisibilityEvents.Remove(MapSettingsChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCombatEvents.Remove(CombatEvent);
            Aow.Units.AoWE.AoWE.AoWHSMap.FArmyChangedEvents.Remove(ArmyChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FArmyDestroyedEvents.Remove(ArmyDestroyed);
            Aow.Units.AoWE.AoWE.AoWHSMap.FUnitChangedEvents.Remove(UnitChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FUnitDestroyedEvents.Remove(UnitDestroyed);
            Aow.Units.AoWE.AoWE.AoWHSMap.FStructureChangedEvents.Remove(StructureChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FStructureDestroyedEvents.Remove(StructureDestroyed);
            Aow.Units.AoWE.AoWE.AoWHSMap.FHSChangedEvents.Remove(HSChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FGameTerminatedEvents.Remove(GameOver);
            Aow.Units.AoWE.AoWE.AoWHSMap.FGameOverEvents.Remove(GameTerminated);
            Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerTerminatedEvents.Remove(PlayerTerminated);
            Aow.Units.AoWE.AoWE.AoWHSMap.FExecuteEventLogEvents.Remove(ExecuteEventLog);
            Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FOnChange.Remove(TargetSelectionChange);
            Aow.Units.AoWE.AoWE.AoWHSMap.FViewPortChangedEvents.Remove(ViewPortChange);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCosmeticSurgeryEvents.Remove(CastSpell);
            Aow.Units.AoWE.AoWE.AoWHSMap.FOnActivateAbility.Remove(ActivateAbility);
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FChangedEvents.Remove(GlobalMagicChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerChangedEvents.Remove(RelationsChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FDiplomaticRelationsChangedEvents.Remove(DiplomaticChange);
            Aow.Units.AoWE.AoWE.AoWHSMap.FExitEvents.Remove(ExitGameEvent);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMapCreatedEvents.Remove(ActivateCombatMap);
            Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMapDestroyedEvents.Remove(DeactivateCombatMap);
            {
                I = 0;
                int end__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; I <= end__7; I += 1)
                {
                    var call__8 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__8)).FGemsChangedEvents.Remove(GemsChanged);
                    var call__9 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__9)).FIncomeChangedEvents.Remove(IncomeChanged);
                    var call__10 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__10)).FUpkeepChangedEvents.Remove(UpkeepChanged);
                    var call__11 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__11)).FLogbookChangedEvents.Remove(LogbookChanged);
                    var call__12 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I));
                    ((Aow.Units.AoWE.TPlayer)((System.Object)call__12)).FMagicControl.FChangedEvents.Remove(PlayerMagicChanged);
                }
            }
        }

        // MapCtrl.pas:291
        public void ActivateHSM(Aow.Rtl.TObject Map)
        {
            if (((Aow.Units.Engine.TEObject)Map).EClass.ClassID == 131089)
            {
                LinkToHSM();
                if (FOnActivateHSM != null)
                {
                    FOnActivateHSM(this);
                }
            }
        }

        // MapCtrl.pas:300
        public void DeactivateHSM(Aow.Rtl.TObject Map)
        {
            if (((Aow.Units.Engine.TEObject)Map).EClass.ClassID == 131089)
            {
                if (FOnDeactivateHSM != null)
                {
                    FOnDeactivateHSM(this);
                }

                ReleaseHSM();
            }
        }

        // MapCtrl.pas:309
        public void DblClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.HSEngine.THexagonSprite HS = default;
            if (FOnDblClickArmy != null)
            {
                var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.Selection.FindClass(typeof(Aow.Units.AoWE.TArmyHS));
                HS = call__13;
                if (HS != null)
                {
                    FOnDblClickArmy((Aow.Rtl.TObject)((Aow.Units.AoWE.TArmyHS)((System.Object)HS)).FArmy);
                }
            }

            if (FOnDblClickStructure != null)
            {
                var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.Selection.FindClass(typeof(Aow.Units.AoWE.TStructure));
                HS = call__14;
                if (HS != null)
                {
                    FOnDblClickStructure(HS);
                }
            }
        }

        // MapCtrl.pas:350
        public void MapChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnMapChanged != null)
                {
                    FOnMapChanged(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during MapChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:359
        public void ScannerIconsChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnScannerIconsChanged != null)
                {
                    FOnScannerIconsChanged(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:368
        public void MapSettingsChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnMapSettingsChanged != null)
                {
                    FOnMapSettingsChanged(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during MapSettingsChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:324
        public void SelectHS(Aow.Rtl.TObject Sender)
        {
            Aow.Units.HSEngine.THexagonSpriteSelection Selection = default;
            try
            {
                Selection = Aow.Units.AoWE.AoWE.AoWHSMap.Selection;
                bool bool__15 = (FOnSelectArmy != null) && (Selection.EventHS is Aow.Units.AoWE.TArmyHS);
                if (bool__15)
                {
                    var call__16 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetArmyHS();
                    bool__15 = (call__16 != null);
                }

                if (bool__15)
                {
                    FOnSelectArmy((Aow.Rtl.TObject)((Aow.Units.AoWE.TArmyHS)((System.Object)Selection.EventHS)).FArmy);
                }

                if ((FOnSelectStructure != null) && (Selection.EventHS is Aow.Units.AoWE.TStructure))
                {
                    FOnSelectStructure(Selection.EventHS);
                }

                if ((FOnSelectItem != null) && (Selection.EventHS is Aow.Units.AoWE.TItemHS))
                {
                    FOnSelectItem(Selection.EventHS);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during SelectHS event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:337
        public void DeselectHS(Aow.Rtl.TObject Sender)
        {
            Aow.Units.HSEngine.THexagonSpriteSelection Selection = default;
            try
            {
                Selection = Aow.Units.AoWE.AoWE.AoWHSMap.Selection;
                if ((FOnDeselectArmy != null) && (Selection.EventHS is Aow.Units.AoWE.TArmyHS))
                {
                    FOnDeselectArmy((Aow.Rtl.TObject)((Aow.Units.AoWE.TArmyHS)((System.Object)Selection.EventHS)).FArmy);
                }

                if ((FOnDeselectStructure != null) && (Selection.EventHS is Aow.Units.AoWE.TStructure))
                {
                    FOnDeselectStructure(Selection.EventHS);
                }

                if ((FOnDeselectItem != null) && (Selection.EventHS is Aow.Units.AoWE.TItemHS))
                {
                    FOnDeselectItem(Selection.EventHS);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during DeselectHS event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:377
        public void PlayerChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnPlayerChanged != null)
                {
                    FOnPlayerChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during PlayerChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:386
        public void ExitGameEvent(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnExitGameEvent != null)
                {
                    FOnExitGameEvent(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during ExitGame event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:395
        public void NewTurn(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnNewTurn != null)
                {
                    FOnNewTurn(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during NewTurn event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:404
        public void EndTurn(Aow.Rtl.TObject Player)
        {
            try
            {
                if (FOnEndTurn != null)
                {
                    var call__17 = (Aow.Rtl.TObject)(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Rtl.Rtl.ObjectToInt(Player)));
                    FOnEndTurn(unchecked((sbyte)Aow.Rtl.Rtl.ObjectToInt(Player)), !((Aow.Units.AoWE.TPlayer)((System.Object)call__17)).FSingleProcessCycle);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during EndTurn event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:413
        public void SeatedPlayerChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnSeatedPlayerChanged != null)
                {
                    FOnSeatedPlayerChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during PlayerChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:422
        public void GemsChanged(Aow.Rtl.TObject Player)
        {
            try
            {
                if (FOnGemsChanged != null)
                {
                    FOnGemsChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)((System.Object)Player)).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Gems Changed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:431
        public void IncomeChanged(Aow.Rtl.TObject Player)
        {
            try
            {
                if (FOnIncomeChanged != null)
                {
                    FOnIncomeChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)((System.Object)Player)).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Income Changed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:440
        public void UpkeepChanged(Aow.Rtl.TObject Player)
        {
            try
            {
                if (FOnUpkeepChanged != null)
                {
                    FOnUpkeepChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)((System.Object)Player)).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Upkeep Changed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:449
        public void PlayerMagicChanged(Aow.Rtl.TObject Control)
        {
            try
            {
                if (FOnPlayerMagicChanged != null)
                {
                    FOnPlayerMagicChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayerMagicControl)((System.Object)Control)).FPlayer.FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Player Magic Changed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:458
        public void GlobalMagicChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnGlobalMagicChanged != null)
                {
                    FOnGlobalMagicChanged(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Magic Changed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:467
        public void ShowMessage(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnShowMessage != null)
                {
                    FOnShowMessage(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during ShowMessage event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:476
        public void ChangeDay(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnChangeDay != null)
                {
                    FOnChangeDay(Aow.Units.AoWE.AoWE.AoWHSMap.FDay);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during ChangeDay event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:485
        public void CombatEvent(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnCombatEvent != null)
                {
                    FOnCombatEvent(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during on combat event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:494
        public void LogbookChanged(Aow.Rtl.TObject Player)
        {
            try
            {
                if (FOnLogbookChanged != null)
                {
                    FOnLogbookChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)((System.Object)Player)).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during LogbookChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:503
        public void StructureChanged(Aow.Rtl.TObject Obj)
        {
            try
            {
                if (FOnStructureChanged != null)
                {
                    FOnStructureChanged(Obj);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnStructureChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:512
        public void StructureDestroyed(Aow.Rtl.TObject Obj)
        {
            try
            {
                if (FOnStructureDestroyed != null)
                {
                    FOnStructureDestroyed(Obj);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:521
        public void ArmyChanged(Aow.Rtl.TObject Obj)
        {
            try
            {
                if (FOnArmyChanged != null)
                {
                    FOnArmyChanged(Obj);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnArmyChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:530
        public void ArmyDestroyed(Aow.Rtl.TObject Obj)
        {
            try
            {
                if (FOnArmyDestroyed != null)
                {
                    FOnArmyDestroyed(Obj);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:539
        public void HSChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnHSChanged != null)
                {
                    FOnHSChanged(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnHSChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:548
        public void UnitChanged(Aow.Rtl.TObject Obj)
        {
            try
            {
                if (FOnUnitChanged != null)
                {
                    FOnUnitChanged(Obj);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnUnitChanged event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:557
        public void UnitDestroyed(Aow.Rtl.TObject Obj)
        {
            try
            {
                if (FOnUnitDestroyed != null)
                {
                    FOnUnitDestroyed(Obj);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:566
        public void GameOver(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnGameOver != null)
                {
                    FOnGameOver(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnGameOver event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:575
        public void GameTerminated(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnGameTerminated != null)
                {
                    FOnGameTerminated(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FGameResult));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnGameTerminated event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:584
        public void PlayerTerminated(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnPlayerTerminated != null)
                {
                    FOnPlayerTerminated(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:593
        public void ExecuteEventLog(Aow.Rtl.TObject Info)
        {
            try
            {
                if (FOnExecuteEventLog != null)
                {
                    FOnExecuteEventLog(((Aow.Units.AoWE.ExecuteEventLogNotification)Info).Info.Log, ((Aow.Units.AoWE.ExecuteEventLogNotification)Info).Info.Immediate, ((Aow.Units.AoWE.ExecuteEventLogNotification)Info).Info.Done);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:603
        public void ActivateCombatMap(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnActivateCombatMap != null)
                {
                    FOnActivateCombatMap(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:612
        public void DeactivateCombatMap(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnDeactivateCombatMap != null)
                {
                    FOnDeactivateCombatMap(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:621
        public void TargetSelectionChange(Aow.Rtl.TObject Control)
        {
            try
            {
                if ((FOnTargetSelectionChange != null) && ((((int)Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FTargetMode.Image) & 8) != 0))
                {
                    FOnTargetSelectionChange((Aow.Units.AoWE.TTargetSelectionControl)Control, ((Aow.Units.AoWE.TTargetSelectionControl)Control).FChange);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:630
        public void CastSpell(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnCastSpell != null)
                {
                    FOnCastSpell(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnCastSpell event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:639
        public void ViewPortChange(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnViewPortChange != null)
                {
                    FOnViewPortChange(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnViewPortChange event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:648
        public void ActivateAbility(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnActivateAbility != null)
                {
                    FOnActivateAbility(Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnActivateAbility event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:657
        public void DiplomaticChange(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnDiplomaticChange != null)
                {
                    FOnDiplomaticChange(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
            }
        }

        // MapCtrl.pas:666
        public void RelationsChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnRelationsChanged != null)
                {
                    FOnRelationsChanged(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnRelationsChanged event (TAoWMapControl)");
            }
        }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnArmySelected { get => FOnSelectArmy; set => FOnSelectArmy = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnArmyDeselected { get => FOnDeselectArmy; set => FOnDeselectArmy = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnArmyDblClick { get => FOnDblClickArmy; set => FOnDblClickArmy = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnArmyChanged { get => FOnArmyChanged; set => FOnArmyChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnArmyDestroyed { get => FOnArmyDestroyed; set => FOnArmyDestroyed = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnStructureSelected { get => FOnSelectStructure; set => FOnSelectStructure = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnStructureDeselected { get => FOnDeselectStructure; set => FOnDeselectStructure = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnStructureDblClick { get => FOnDblClickStructure; set => FOnDblClickStructure = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnStructureChanged { get => FOnStructureChanged; set => FOnStructureChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnStructureDestroyed { get => FOnStructureDestroyed; set => FOnStructureDestroyed = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnUnitChanged { get => FOnUnitChanged; set => FOnUnitChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnUnitDestroyed { get => FOnUnitDestroyed; set => FOnUnitDestroyed = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnGemsChanged { get => FOnGemsChanged; set => FOnGemsChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnIncomeChanged { get => FOnIncomeChanged; set => FOnIncomeChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnUpkeepChanged { get => FOnUpkeepChanged; set => FOnUpkeepChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnShowMessage { get => FOnShowMessage; set => FOnShowMessage = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapDayEvent OnDayChanged { get => FOnChangeDay; set => FOnChangeDay = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnPlayerChanged { get => FOnPlayerChanged; set => FOnPlayerChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnNewTurn { get => FOnNewTurn; set => FOnNewTurn = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapEndTurnEvent OnEndTurn { get => FOnEndTurn; set => FOnEndTurn = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnHSChanged { get => FOnHSChanged; set => FOnHSChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnSeatedPlayerChanged { get => FOnSeatedPlayerChanged; set => FOnSeatedPlayerChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnMapChanged { get => FOnMapChanged; set => FOnMapChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnScannerIconsChanged { get => FOnScannerIconsChanged; set => FOnScannerIconsChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnMapSettingsChanged { get => FOnMapSettingsChanged; set => FOnMapSettingsChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnCombat { get => FOnCombatEvent; set => FOnCombatEvent = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnLogbookChanged { get => FOnLogbookChanged; set => FOnLogbookChanged = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapTargetEvent OnTargetSelectionChange { get => FOnTargetSelectionChange; set => FOnTargetSelectionChange = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnCastSpell { get => FOnCastSpell; set => FOnCastSpell = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnActivateAbility { get => FOnActivateAbility; set => FOnActivateAbility = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnPlayerMagicChanged { get => FOnPlayerMagicChanged; set => FOnPlayerMagicChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnGlobalMagicChanged { get => FOnGlobalMagicChanged; set => FOnGlobalMagicChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnRelationsChanged { get => FOnRelationsChanged; set => FOnRelationsChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnActivateCombatMap { get => FOnActivateCombatMap; set => FOnActivateCombatMap = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDeactivateCombatMap { get => FOnDeactivateCombatMap; set => FOnDeactivateCombatMap = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnItemsSelected { get => FOnSelectItem; set => FOnSelectItem = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapObjectEvent OnItemsDeselected { get => FOnDeselectItem; set => FOnDeselectItem = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnGameOver { get => FOnGameOver; set => FOnGameOver = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapPlayerEvent OnGameTerminated { get => FOnGameTerminated; set => FOnGameTerminated = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnPlayerTerminated { get => FOnPlayerTerminated; set => FOnPlayerTerminated = value; }

        [Published]
        public Aow.Units.MapCtrl.TMapExecuteLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnViewPortChange { get => FOnViewPortChange; set => FOnViewPortChange = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDiplomaticChange { get => FOnDiplomaticChange; set => FOnDiplomaticChange = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnExitGame { get => FOnExitGameEvent; set => FOnExitGameEvent = value; }
    }

    public delegate void TMapEvent();
    public delegate void TMapPlayerEvent(sbyte Player);
    public delegate void TMapDayEvent(int Day);
    public delegate void TMapObjectEvent(Aow.Rtl.TObject Obj);
    public delegate void TMapEndTurnEvent(sbyte Player, bool Automatic);
    public delegate void TMapExecuteLogEvent(Aow.Units.EventLog.TEventLog Log, bool Immediate, Aow.Units.EventLog.TExecuteDoneEvent Done);
    public delegate void TMapTargetEvent(Aow.Units.AoWE.TTargetSelectionControl Control, Aow.Units.AoWE.TTargetSelectionChange Change);
    public struct TExecuteLogInfo
    {
        public Aow.Units.EventLog.TEventLog Log;
        public object Data2;
        public object Data1;
        public byte Trigger;
    }

    public struct TRawObject
    {
        public Aow.Units.FixedArray_object_512 Fields;
    }

    [StructLayout(LayoutKind.Explicit, Size = 376)]
    public struct TMapState
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_164 Reserved00;
        [FieldOffset(164)]
        public sbyte CurrentPlayer;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
        [FieldOffset(166)]
        public Aow.Units.FixedArray_byte_115 ReservedA6;
        [FieldOffset(281)]
        public sbyte Result;
        [FieldOffset(282)]
        public Aow.Units.FixedArray_byte_90 Reserved11A;
        [FieldOffset(372)]
        public int Day;
    }

    [StructLayout(LayoutKind.Explicit, Size = 167)]
    public struct TPlayerState
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_44 Reserved00;
        [FieldOffset(44)]
        public bool Human;
        [FieldOffset(45)]
        public Aow.Units.FixedArray_byte_121 Reserved2D;
        [FieldOffset(166)]
        public sbyte PlayerID;
    }

    [StructLayout(LayoutKind.Explicit, Size = 57)]
    public struct TTargetState
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_13 Reserved00;
        [FieldOffset(13)]
        public byte Status;
        [FieldOffset(14)]
        public Aow.Units.FixedArray_byte_42 Reserved0E;
        [FieldOffset(56)]
        public byte Mode;
    }

    public delegate Aow.Rtl.TObject TGetPlayers(Aow.Units.AoWE.TPlayerList List, int Index);
    public delegate Aow.Units.AoWE.TArmyHS TGetArmyHS(Aow.Units.AoWE.TSelectedArmy Selection);
    public delegate bool TValidateEngine();
    public static partial class MapCtrl
    {
        // MapCtrl.pas:675
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.MapCtrl.TAoWMapControl)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
