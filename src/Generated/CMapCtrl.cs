// Generated from CMapCtrl.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CMapCtrl
{
    // { TAoWCombatMapControl: size $C4; event methods occupy $24..$C3. VMT 00435F38. }
    public partial class TAoWCombatMapControl : Aow.Rtl.TComponent
    {
        public Aow.Units.CMapCtrl.TSelectHSEvent FOnSelected;
        // { $24 }
        public Aow.Units.CMapCtrl.TSelectHSEvent FOnDeselected;
        // { $2C }
        public Aow.Rtl.TNotifyEvent FOnActivateHSM;
        // { $34 }
        public Aow.Rtl.TNotifyEvent FOnDeactivateHSM;
        // { $3C }
        public Aow.Units.CMapCtrl.TPlayerChangedEvent FOnIncomeChanged;
        // { $44 }
        public Aow.Units.CMapCtrl.TPlayerChangedEvent FOnUpkeepChanged;
        // { $4C }
        public Aow.Units.CMapCtrl.TPlayerChangedEvent FOnGemsChanged;
        // { $54 }
        public Aow.Units.CMapCtrl.TShowMessageEvent FOnShowMessage;
        // { $5C }
        public Aow.Units.CMapCtrl.TTurnChangedEvent FOnTurnChanged;
        // { $64 }
        public Aow.Units.CMapCtrl.TPlayerChangedEvent FOnPlayerChanged;
        // { $6C }
        public Aow.Units.CMapCtrl.TPlayerChangedEvent FOnSeatedPlayerChanged;
        // { $74 }
        public Aow.Units.CMapCtrl.TOnExecuteEventLogEvent FOnExecuteEventLog;
        // { $7C }
        public Aow.Units.CMapCtrl.TMapChangedEvent FOnMapChanged;
        // { $84 }
        public Aow.Units.CMapCtrl.TPlayerChangedEvent FOnLogbookChanged;
        // { $8C }
        public Aow.Rtl.TNotifyEvent FOnActivateMap;
        // { $94 }
        public Aow.Rtl.TNotifyEvent FOnDeActivateMap;
        // { $9C }
        public Aow.Units.CMapCtrl.TSelectHSEvent FOnMouseOverTarget;
        // { $A4 }
        public Aow.Units.CMapCtrl.TSelectHSEvent FOnUnitChanged;
        // { $AC }
        public Aow.Units.CMapCtrl.TCastCombatSpellEvent FOnCastSpell;
        // { $B4 }
        public Aow.Units.CMapCtrl.TActivateCombatAbilityEvent FOnActivateAbility;
        // CMapCtrl.pas:117
        // { $BC }
        public override void Destroy()
        {
            if (Aow.Units.AoWE.AoWE.AoWEngine != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Remove(ActivateHSM);
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMClose.Remove(DeactivateHSM);
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                {
                    ReleaseHSM();
                }
            }

            base.Destroy();
        }

        // CMapCtrl.pas:107
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            if (Aow.Units.AoWE.AoWE.ValidateAoWEngine())
            {
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Add(ActivateHSM);
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMClose.Add(DeactivateHSM);
            }
        }

        // CMapCtrl.pas:128
        public void LinkToHSM()
        {
            int I = default;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FChangeTurnEvents.Add(ChangeTurn);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayerChangedEvents.Add(PlayerChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayerChangedEvents.Add(SeatedPlayerChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.OnSelect.Add(SelectHS);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.OnDeselect.Add(DeselectHS);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FShowMessageEvents.Add(ShowMessage);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMapChangedEvents.Add(MapChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FDeselectEvents.Add(ActivateMap);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSelectEvents.Add(DeactivateMap);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMouseOverTargetEvents.Add(MouseOverTarget);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitChangedEvents.Add(UnitChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FAbilityActivatedEvents.Add(ActivateAbility);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombatStartedEvents.Add(CastSpell);
            Aow.Units.AoWE.AoWE.AoWHSMap.FExecuteEventLogEvents.Add(ExecuteEventLog);
            {
                I = 0;
                int end__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__2.FGemsChangedEvents.Add(GemsChanged);
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__3.FIncomeChangedEvents.Add(IncomeChanged);
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__4.FUpkeepChangedEvents.Add(UpkeepChanged);
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__5.FLogbookChangedEvents.Add(LogbookChanged);
                }
            }
        }

        // CMapCtrl.pas:154
        public void ReleaseHSM()
        {
            int I = default;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FChangeTurnEvents.Remove(ChangeTurn);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayerChangedEvents.Remove(PlayerChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayerChangedEvents.Remove(SeatedPlayerChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.OnSelect.Remove(SelectHS);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.OnDeselect.Remove(DeselectHS);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FShowMessageEvents.Remove(ShowMessage);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMapChangedEvents.Remove(MapChanged);
            Aow.Units.AoWE.AoWE.AoWHSMap.FExecuteEventLogEvents.Remove(ExecuteEventLog);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FDeselectEvents.Remove(ActivateMap);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSelectEvents.Remove(DeactivateMap);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMouseOverTargetEvents.Remove(MouseOverTarget);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitChangedEvents.Remove(UnitChanged);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FAbilityActivatedEvents.Remove(ActivateAbility);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombatStartedEvents.Remove(CastSpell);
            {
                I = 0;
                int end__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; I <= end__6; I += 1)
                {
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__7.FGemsChangedEvents.Remove(GemsChanged);
                    var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__8.FIncomeChangedEvents.Remove(IncomeChanged);
                    var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__9.FUpkeepChangedEvents.Remove(UpkeepChanged);
                    var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__10.FLogbookChangedEvents.Remove(LogbookChanged);
                }
            }
        }

        // CMapCtrl.pas:180
        public void ActivateHSM(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.Engine.TEObject)Sender).ClassType.ClassID() == 2228480)
            {
                LinkToHSM();
                if (FOnActivateHSM != null)
                {
                    FOnActivateHSM(this);
                }
            }
        }

        // CMapCtrl.pas:189
        public void DeactivateHSM(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.Engine.TEObject)Sender).ClassType.ClassID() == 2228480)
            {
                if (FOnDeactivateHSM != null)
                {
                    FOnDeactivateHSM(this);
                }

                ReleaseHSM();
            }
        }

        // CMapCtrl.pas:207
        public void SelectHS(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnSelected != null)
                {
                    FOnSelected(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.EventHS);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnSelect event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:216
        public void DeselectHS(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnDeselected != null)
                {
                    FOnDeselected(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.EventHS);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnDeselect event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:225
        public void MouseOverTarget(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnMouseOverTarget != null)
                {
                    FOnMouseOverTarget(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMouseOverTarget);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnMouseOverTarget event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:234
        public void UnitChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnUnitChanged != null)
                {
                    var call__11 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                    FOnUnitChanged(call__11);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during UnitChanged event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:243
        public void PlayerChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnPlayerChanged != null)
                {
                    FOnPlayerChanged(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:252
        public void SeatedPlayerChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnSeatedPlayerChanged != null)
                {
                    FOnSeatedPlayerChanged(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during PlayerChanged event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:261
        public void GemsChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnGemsChanged != null)
                {
                    FOnGemsChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)Sender).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Gems Changed event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:270
        public void IncomeChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnIncomeChanged != null)
                {
                    FOnIncomeChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)Sender).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Income Changed event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:279
        public void UpkeepChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnUpkeepChanged != null)
                {
                    FOnUpkeepChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)Sender).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during Upkeep Changed event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:297
        public void ChangeTurn(Aow.Rtl.TObject Sender)
        {
            try
            {
                // { The original passes the seated player as the turn number. }
                if (FOnTurnChanged != null)
                {
                    FOnTurnChanged(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during ChangeTurn event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:307
        public void LogbookChanged(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnLogbookChanged != null)
                {
                    FOnLogbookChanged(unchecked((sbyte)((Aow.Units.AoWE.TPlayer)Sender).FPlayerIndex));
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during LogbookChanged event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:316
        public void ExecuteEventLog(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnExecuteEventLog != null)
                {
                    /* TODO(T-N3) Untyped pointer dereference requires a typed storage override */
                    throw new System.NotImplementedException();
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:325
        public void ActivateMap(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnActivateMap != null)
                {
                    FOnActivateMap(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnActivateMap event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:334
        public void DeactivateMap(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnDeActivateMap != null)
                {
                    FOnDeActivateMap(this);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnDeactivateMap event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:343
        public void CastSpell(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnCastSpell != null)
                {
                    FOnCastSpell((Aow.Units.AoWE.TCombatSpellCaster)Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnCastSpell event (TAoWCombatMapControl)");
            }
        }

        // CMapCtrl.pas:352
        public void ActivateAbility(Aow.Rtl.TObject Sender)
        {
            try
            {
                if (FOnActivateAbility != null)
                {
                    FOnActivateAbility((Aow.Units.AoWE.TActivateAbilityInfo)Sender);
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Exception occurred during OnActivateAbility event (TAoWCombatMapControl)");
            }
        }

        [Published]
        public Aow.Units.CMapCtrl.TSelectHSEvent OnSelected { get => FOnSelected; set => FOnSelected = value; }

        [Published]
        public Aow.Units.CMapCtrl.TSelectHSEvent OnDeselected { get => FOnDeselected; set => FOnDeselected = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

        [Published]
        public Aow.Units.CMapCtrl.TPlayerChangedEvent OnIncomeChanged { get => FOnIncomeChanged; set => FOnIncomeChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TPlayerChangedEvent OnUpkeepChanged { get => FOnUpkeepChanged; set => FOnUpkeepChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TPlayerChangedEvent OnGemsChanged { get => FOnGemsChanged; set => FOnGemsChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TShowMessageEvent OnShowMessage { get => FOnShowMessage; set => FOnShowMessage = value; }

        [Published]
        public Aow.Units.CMapCtrl.TTurnChangedEvent OnTurnChanged { get => FOnTurnChanged; set => FOnTurnChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TPlayerChangedEvent OnPlayerChanged { get => FOnPlayerChanged; set => FOnPlayerChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TPlayerChangedEvent OnSeatedPlayerChanged { get => FOnSeatedPlayerChanged; set => FOnSeatedPlayerChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TOnExecuteEventLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

        [Published]
        public Aow.Units.CMapCtrl.TMapChangedEvent OnMapChanged { get => FOnMapChanged; set => FOnMapChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TPlayerChangedEvent OnLogbookChanged { get => FOnLogbookChanged; set => FOnLogbookChanged = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnActivateMap { get => FOnActivateMap; set => FOnActivateMap = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDeActivateMap { get => FOnDeActivateMap; set => FOnDeActivateMap = value; }

        [Published]
        public Aow.Units.CMapCtrl.TSelectHSEvent OnMouseOverTarget { get => FOnMouseOverTarget; set => FOnMouseOverTarget = value; }

        [Published]
        public Aow.Units.CMapCtrl.TSelectHSEvent OnUnitChanged { get => FOnUnitChanged; set => FOnUnitChanged = value; }

        [Published]
        public Aow.Units.CMapCtrl.TCastCombatSpellEvent OnCastSpell { get => FOnCastSpell; set => FOnCastSpell = value; }

        [Published]
        public Aow.Units.CMapCtrl.TActivateCombatAbilityEvent OnActivateAbility { get => FOnActivateAbility; set => FOnActivateAbility = value; }

        // CMapCtrl.pas:54
        public static Aow.Units.CMapCtrl.TAoWCombatMapControl Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.CMapCtrl.TAoWCombatMapControl o = new Aow.Units.CMapCtrl.TAoWCombatMapControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public delegate void TOnMessageEvent(ref Aow.Units.Engine.TEMsg EMsg);
    public delegate void TSelectHSEvent(Aow.Units.HSEngine.THexagonSprite HS);
    public delegate void TDeselectHSEvent(Aow.Units.HSEngine.THexagonSprite HS);
    public delegate void TPlayerChangedEvent(sbyte Player);
    public delegate void TShowMessageEvent(in Aow.Units.AoWE.TShowMessageInfo Info);
    public delegate void TTurnChangedEvent(int Turn);
    public delegate void TMapChangedEvent(in Aow.Units.AoWE.TMapChangedInfo ChangedArea);
    public delegate void TOnGameOverEvent(sbyte Player);
    public delegate void TOnExecuteEventLogEvent(Aow.Units.EventLog.TEventLog EventLog, Aow.Units.EventLog.TExecuteDoneEvent ExecuteDoneEvent);
    public delegate void TCastCombatSpellEvent(Aow.Units.AoWE.TCombatSpellCaster SC);
    public delegate void TActivateCombatAbilityEvent(Aow.Units.AoWE.TActivateAbilityInfo AAI);
    public static partial class CMapCtrl
    {
        // CMapCtrl.pas:361
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoWEngine", [typeof(Aow.Units.CMapCtrl.TAoWCombatMapControl)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
