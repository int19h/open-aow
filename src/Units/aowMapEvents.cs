// Translated from aowMapEvents.pas.
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
using Aow.Units.MapCtrl;
using static Aow.Units.MapCtrl.MapCtrl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.WizardsTower;
using static Aow.Units.WizardsTower.WizardsTower;
using Aow.Units.Arena;
using static Aow.Units.Arena.Arena;
using Aow.Units.FlagEvents;
using static Aow.Units.FlagEvents.FlagEvents;
using Aow.Units.Dungeon;
using static Aow.Units.Dungeon.Dungeon;
using Aow.Units.ItemExpS;
using static Aow.Units.ItemExpS.ItemExpS;
using Aow.Units.AoWArmyEventDlg;
using static Aow.Units.AoWArmyEventDlg.AoWArmyEventDlg;
using Aow.Units.AoWCityRebelDlg;
using static Aow.Units.AoWCityRebelDlg.AoWCityRebelDlg;
using Aow.Units.AoWMsgReceiveDlg;
using static Aow.Units.AoWMsgReceiveDlg.AoWMsgReceiveDlg;
using Aow.Units.AoWTutorialWin;
using static Aow.Units.AoWTutorialWin.AoWTutorialWin;
using Aow.Units.AoWArmyInfoDlg;
using static Aow.Units.AoWArmyInfoDlg.AoWArmyInfoDlg;
using Aow.Units.AoWWizardsTowerDlg;
using static Aow.Units.AoWWizardsTowerDlg.AoWWizardsTowerDlg;
using Aow.Units.AoWArenaDlg;
using static Aow.Units.AoWArenaDlg.AoWArenaDlg;
using Aow.Units.FastCombatWin;
using static Aow.Units.FastCombatWin.FastCombatWin;
using Aow.Units.AoWPartyJoin;
using static Aow.Units.AoWPartyJoin.AoWPartyJoin;
using Aow.Units.AoWHeroJoinDlg;
using static Aow.Units.AoWHeroJoinDlg.AoWHeroJoinDlg;
using Aow.Units.AoWHeroInfoDlg;
using static Aow.Units.AoWHeroInfoDlg.AoWHeroInfoDlg;
using Aow.Units.AoWHeroUpgradeDlg;
using static Aow.Units.AoWHeroUpgradeDlg.AoWHeroUpgradeDlg;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWPasswordDlg;
using static Aow.Units.AoWPasswordDlg.AoWPasswordDlg;
using Aow.Units.AoWMagic;
using static Aow.Units.AoWMagic.AoWMagic;
using Aow.Units.AoWRelations;
using static Aow.Units.AoWRelations.AoWRelations;
using Aow.Units.AoWCastUnitSpellDlg;
using static Aow.Units.AoWCastUnitSpellDlg.AoWCastUnitSpellDlg;
using Aow.Units.AoWTargetSelection;
using static Aow.Units.AoWTargetSelection.AoWTargetSelection;
using Aow.Units.AoWCityJoinDlg;
using static Aow.Units.AoWCityJoinDlg.AoWCityJoinDlg;
using Aow.Units.AoWItemsFoundDlg;
using static Aow.Units.AoWItemsFoundDlg.AoWItemsFoundDlg;
using Aow.Units.AoWRealm;
using static Aow.Units.AoWRealm.AoWRealm;
using Aow.Units.AoWInfoWin;
using static Aow.Units.AoWInfoWin.AoWInfoWin;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWUnitBanner;
using static Aow.Units.AoWUnitBanner.AoWUnitBanner;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.AoWCityScreen;
using static Aow.Units.AoWCityScreen.AoWCityScreen;
using Aow.Units.AoWProdScreen;
using static Aow.Units.AoWProdScreen.AoWProdScreen;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using Aow.Units.GlobalSpells;
using static Aow.Units.GlobalSpells.GlobalSpells;
using Aow.Units.UnitSpells;
using static Aow.Units.UnitSpells.UnitSpells;
using Aow.Units.E3EventDialogs;
using static Aow.Units.E3EventDialogs.E3EventDialogs;
using Aow.Units.E3StructureInfo;
using static Aow.Units.E3StructureInfo.E3StructureInfo;
using Aow.Units.AoWNewTurnDialog;
using static Aow.Units.AoWNewTurnDialog.AoWNewTurnDialog;
using Aow.Units.AoWSendEmailGameDlg;
using static Aow.Units.AoWSendEmailGameDlg.AoWSendEmailGameDlg;
using Aow.Units.AoWSMTPDlg;
using static Aow.Units.AoWSMTPDlg.AoWSMTPDlg;
using Aow.Units.TCFCDlg;
using static Aow.Units.TCFCDlg.TCFCDlg;
using Aow.Units.AoWPInfoWin;
using static Aow.Units.AoWPInfoWin.AoWPInfoWin;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.AoWUnitWindow;
using static Aow.Units.AoWUnitWindow.AoWUnitWindow;

namespace Aow.Units.aowMapEvents
{
    // map control
    public partial class TMapEvents : Aow.Rtl.TDataModule
    {
        public TMapEvents(TComponent owner) : base(owner)
        { }

        public Aow.Units.MapCtrl.TAoWMapControl TheMap;
        // aowMapEvents.pas:93
        public void TheMapActivateAbility(Aow.Units.AoWE.TActivateAbilityInfo Request)
        {
            if (Request.FAbilityID == 52)
            {
                Aow.Units.AoWExeForms.AoWExeForms.SpellBook.ShowCasting(Request);
            }
            else
            {
                if (Request.FAbilityID == 125)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.ConstructDlg.Execute(Request);
                }
                else
                {
                    if (Request is Aow.Units.AoWE.TUnitSelectorAAI)
                    {
                        Aow.Units.E3Globals.E3Globals.UnitSelectionDlg.Execute((Aow.Units.AoWE.TUnitSelectorAAI)Request);
                    }
                }
            }
        }

        // aowMapEvents.pas:100
        public void TheMapArmyChanged(Aow.Units.AoWE.TArmy Army)
        {
            if (Army != null)
            {
                if (Army == Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList)
                {
                    TheMapArmySelected(Army);
                    if (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    }

                    if (Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopup.Visible)
                    {
                        Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopupShow(this);
                    }
                }

                if (((Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Army.FPlayer) && Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible) && (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 3))
                {
                    Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
                }
            }
            else
            {
                if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible && (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 3))
                {
                    Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
                }
            }
        }

        // aowMapEvents.pas:117
        public void TheMapArmyDeselected(Aow.Units.AoWE.TArmy Army)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.FAbilities.Clear();
            Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList = null;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.MovesLbl.Text = (AnsiString)"";
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.MoveIcon.Visible = false;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp1Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp1Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp2Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp2Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp3Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp3Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp4Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp4Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp5Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp5Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp6Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp6Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp7Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp7Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp8Fr.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.Emp8Chk.Visible = true;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible = false;
            if (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible)
            {
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
            }
        }

        // aowMapEvents.pas:144
        public void TheMapArmyDestroyed(Aow.Units.AoWE.TArmy Army)
        {
            if ((Army != null) && (Army == Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList))
            {
                TheMapArmyDeselected(Army);
            }

            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible && (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 3))
            {
                Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            }
        }

        // aowMapEvents.pas:150
        public void TheMapArmySelected(Aow.Units.AoWE.TArmy Army)
        {
            int I = default;
            int Count = default;
            Count = 0;
            {
                I = 0;
                byte end__1 = 7;
                for (; I <= end__1; I += 1)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I] = -1;
                }
            }

            {
                I = 0;
                int end__2 = Army.GetCount() - 1;
                for (; I <= end__2; I += 1)
                {
                    var call__3 = Army.GetUnitConcealedForPlayer(I, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    if (!call__3)
                    {
                        Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Count] = I;
                        Count += 1;
                    }
                }
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList = Army;
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.UpdateArmy();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Army.FPlayer)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(63, true, true);
            }
        }

        // aowMapEvents.pas:167
        public void TheMapDayChanged(int Day)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msActivated))
            {
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FLastPlayer = -1;
            }

            var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DayXRStr);
            var call__5 = Aow.Units.AoWE.AoWE.Format(call__4, [(int)Day]);
            Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.Turn.Text = call__5;
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapGemsChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapIncomeChanged(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible)
            {
                Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            }
        }

        // aowMapEvents.pas:187
        public void TheMapEndTurn(sbyte Player, bool Cancelled)
        {
            bool bool__6 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player);
            if (bool__6)
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                bool__6 = (call__7.FGameOverStatus == Aow.Units.AoWE.TGameOverStatus.goNone);
            }

            if (bool__6)
            {
                if (Cancelled)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FDrawn = false;
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FButtonChecked = false;
                }
                else
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.HideTransientWindows();
                }
            }

            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.PInd.FDrawn = false;
        }

        // aowMapEvents.pas:198
        public void TheMapExecuteEventLog(Aow.Units.EventLog.TEventLog Event, bool Mode, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TAbstractUnit UnitObject = default;
            if (!Mode)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.HideTransientWindows();
            }

            if (Event is Aow.Units.AoWE.TCombatEventLog)
            {
                Aow.Units.FastCombatWin.FastCombatWin.ShowCombatEvent(Aow.Units.E3EventDialogs.E3EventDialogs.FastCombatWindow, (Aow.Units.AoWE.TCombatEventLog)Event, Done);
            }
            else
            {
                if (Event is Aow.Units.AoWE.TArmyEventLog)
                {
                    Aow.Units.AoWArmyEventDlg.AoWArmyEventDlg.ShowArmyEvent(Aow.Units.E3EventDialogs.E3EventDialogs.ArmyEventDlg, (Aow.Units.AoWE.TArmyEventLog)Event, Done);
                }
                else
                {
                    if (Event is Aow.Units.City.TCityRebellionEventLog)
                    {
                        Aow.Units.aowMapEvents.aowMapEvents.ShowRebellion(Aow.Units.E3EventDialogs.E3EventDialogs.CityRebelDlg, (Aow.Units.City.TCityRebellionEventLog)Event, Done);
                    }
                    else
                    {
                        if (Event is Aow.Units.AoWE.TDiplomaticActionEventLog)
                        {
                            Aow.Units.E3EventDialogs.E3EventDialogs.ShowDiplomaticEvent(Aow.Units.E1Globals.E1Globals.RelWindow, (Aow.Units.AoWE.TDiplomaticActionEventLog)Event, Done);
                        }
                        else
                        {
                            if (Event is Aow.Units.AoWE.TNegotiateRequestEventLog)
                            {
                                var negotiateRequestEventLog = (Aow.Units.AoWE.TNegotiateRequestEventLog)Event;
                                if (negotiateRequestEventLog.FObject == Aow.Units.AoWE.TNegotiateObject.noCity)
                                {
                                    Aow.Units.aowMapEvents.aowMapEvents.ShowCityNegotiation(Aow.Units.E3EventDialogs.E3EventDialogs.CityJoinDlg, (Aow.Units.AoWE.TNegotiateRequestEventLog)Event, Done);
                                }
                                else
                                {
                                    if (negotiateRequestEventLog.FObject == Aow.Units.AoWE.TNegotiateObject.noArmy)
                                    {
                                        var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(negotiateRequestEventLog.FX, negotiateRequestEventLog.FY, negotiateRequestEventLog.FL);
                                        var call__10 = call__9.FindOwnedChild(131607);
                                        ArmyHS = (Aow.Units.AoWE.TArmyHS)call__10;
                                        if (ArmyHS != null)
                                        {
                                            var call__11 = ArmyHS.FArmy.GetJoinSelection(unchecked((sbyte)negotiateRequestEventLog.FPlayer));
                                            var call__12 = ArmyHS.FArmy.Hero(unchecked((byte)call__11), Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
                                            if (call__12 == null)
                                            {
                                                Aow.Units.aowMapEvents.aowMapEvents.ShowPartyJoin(Aow.Units.E3EventDialogs.E3EventDialogs.PartyJoinDlg, (Aow.Units.AoWE.TNegotiateRequestEventLog)Event, Done);
                                            }
                                            else
                                            {
                                                Aow.Units.aowMapEvents.aowMapEvents.ShowHeroNegotiation(Aow.Units.E3EventDialogs.E3EventDialogs.HeroJoinDlg, (Aow.Units.AoWE.TNegotiateRequestEventLog)Event, Done);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Event is Aow.Units.EventLog.TGameOverEventLog)
                                {
                                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Event.FPlayer)
                                    {
                                        Aow.Units.AOWPyreScrn.AOWPyreScrn.PyreScreen.ShowGameOver((Aow.Units.EventLog.TGameOverEventLog)Event, Done);
                                    }
                                    else
                                    {
                                        Aow.Units.aowMapEvents.aowMapEvents.ShowReceivedMessage(Aow.Units.E3EventDialogs.E3EventDialogs.MsgReceiveDlg, Event, Done);
                                    }
                                }
                                else
                                {
                                    if (Event is Aow.Units.AoWE.TNewTurnEventLog)
                                    {
                                        Aow.Units.E3EventDialogs.E3EventDialogs.NewTurnDlg.ShowNewTurn((Aow.Units.AoWE.TNewTurnEventLog)Event, Done);
                                    }
                                    else
                                    {
                                        if (Event is Aow.Units.AoWE.TCombatTypeRequestEventLog)
                                        {
                                            Aow.Units.E3EventDialogs.E3EventDialogs.ShowCombatType(Aow.Units.E3EventDialogs.E3EventDialogs.TCFCDialog, (Aow.Units.AoWE.TCombatTypeRequestEventLog)Event, Done);
                                        }
                                        else
                                        {
                                            if (Event is Aow.Units.EventLog.TShowBookEventLog)
                                            {
                                                Aow.Units.E3EventDialogs.E3EventDialogs.ShowInformationBook(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap, (Aow.Units.EventLog.TShowBookEventLog)Event, Done);
                                            }
                                            else
                                            {
                                                if (Event is Aow.Units.AoWE.TPlayerMagicEventLog)
                                                {
                                                    var playerMagicEventLog = (Aow.Units.AoWE.TPlayerMagicEventLog)Event;
                                                    switch (playerMagicEventLog.FMode)
                                                    {
                                                        case Aow.Units.AoWE.TPlayerMagicEventMode.pmemSpellResearched:
                                                            Aow.Units.AoWExeForms.AoWExeForms.SpellBook.ShowResearchedEvent((Aow.Units.AoWE.TPlayerMagicEventLog)Event, Done);
                                                            break;
                                                        case Aow.Units.AoWE.TPlayerMagicEventMode.pmemOutOfMana:
                                                            Aow.Units.aowMapEvents.aowMapEvents.ShowMagicEvent(Aow.Units.AoWExeForms.AoWExeForms.MagicWin, (Aow.Units.AoWE.TPlayerMagicEventLog)Event, Done);
                                                            break;
                                                        case Aow.Units.AoWE.TPlayerMagicEventMode.pmemStartResearch:
                                                            Aow.Units.AoWExeForms.AoWExeForms.SpellBook.ShowResearchEvent((Aow.Units.AoWE.TPlayerMagicEventLog)Event, Done);
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Event is Aow.Units.AoWE.THeroUpgradeEventLog)
                                                    {
                                                        Aow.Units.aowMapEvents.aowMapEvents.ShowHeroUpgrade(Aow.Units.E3EventDialogs.E3EventDialogs.HeroUpgradeDlg, (Aow.Units.AoWE.THeroUpgradeEventLog)Event, Done);
                                                    }
                                                    else
                                                    {
                                                        if (Event is Aow.Units.AoWE.THeroJoinEventLog)
                                                        {
                                                            Aow.Units.aowMapEvents.aowMapEvents.ShowHeroJoin(Aow.Units.E3EventDialogs.E3EventDialogs.HeroJoinDlg, (Aow.Units.AoWE.THeroJoinEventLog)Event, Done);
                                                        }
                                                        else
                                                        {
                                                            if (Event is Aow.Units.AoWE.TArmyInfoEventLog)
                                                            {
                                                                var armyInfoEventLog = (Aow.Units.AoWE.TArmyInfoEventLog)Event;
                                                                var call__15 = armyInfoEventLog.FArmyView.Hero(unchecked((byte)armyInfoEventLog.FSelection), Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
                                                                if (call__15 == null)
                                                                {
                                                                    Aow.Units.aowMapEvents.aowMapEvents.ShowArmyInfo(Aow.Units.E3Globals.E3Globals.ArmyInfoDlg, (Aow.Units.AoWE.TArmyInfoEventLog)Event, Done);
                                                                }
                                                                else
                                                                {
                                                                    Aow.Units.AoWHeroInfoDlg.AoWHeroInfoDlg.ShowHeroInfo(Aow.Units.E3EventDialogs.E3EventDialogs.HeroInfoDlg, (Aow.Units.AoWE.TArmyInfoEventLog)Event, Done);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Event is Aow.Units.Dungeon.TPrisonersJoinEventLog)
                                                                {
                                                                    var call__16 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(((Aow.Units.Dungeon.TPrisonersJoinEventLog)Event).FFirstUnitID);
                                                                    UnitObject = call__16;
                                                                    if ((UnitObject != null) && (UnitObject is Aow.Units.AoWE.THero))
                                                                    {
                                                                        Aow.Units.aowMapEvents.aowMapEvents.ShowPrisonerHero(Aow.Units.E3EventDialogs.E3EventDialogs.HeroJoinDlg, (Aow.Units.Dungeon.TPrisonersJoinEventLog)Event, Done);
                                                                    }
                                                                    else
                                                                    {
                                                                        Aow.Units.aowMapEvents.aowMapEvents.ShowPrisonersJoin(Aow.Units.E3EventDialogs.E3EventDialogs.PartyJoinDlg, (Aow.Units.Dungeon.TPrisonersJoinEventLog)Event, Done);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Event is Aow.Units.WizardsTower.TEnterWizardsTowerEventLog)
                                                                    {
                                                                        Aow.Units.aowMapEvents.aowMapEvents.ShowTower(Aow.Units.E3EventDialogs.E3EventDialogs.WizardsTowerDlg, (Aow.Units.WizardsTower.TEnterWizardsTowerEventLog)Event, Done);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Event is Aow.Units.Arena.TEnterArenaEventLog)
                                                                        {
                                                                            Aow.Units.AoWArenaDlg.AoWArenaDlg.ShowArena(Aow.Units.E3EventDialogs.E3EventDialogs.ArenaDlg, (Aow.Units.Arena.TEnterArenaEventLog)Event, Done);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Event is Aow.Units.EventLog.TMessageEventLog)
                                                                            {
                                                                                Aow.Units.aowMapEvents.aowMapEvents.ShowMessageEvent(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg, (Aow.Units.EventLog.TMessageEventLog)Event, Done);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Event is Aow.Units.EventLog.TTutorialMessageEventLog)
                                                                                {
                                                                                    Aow.Units.aowMapEvents.aowMapEvents.ShowTutorialEvent(Aow.Units.E2Globals.E2Globals.TutorialWindow, (Aow.Units.EventLog.TTutorialMessageEventLog)Event, Mode, Done);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Event is Aow.Units.FlagEvents.TFlagMessageEventLog)
                                                                                    {
                                                                                        Aow.Units.aowMapEvents.aowMapEvents.ShowFlagMessage(Aow.Units.E2Globals.E2Globals.TutorialWindow, (Aow.Units.FlagEvents.TFlagMessageEventLog)Event, Mode, Done);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (Event is Aow.Units.EventLog.THelpMessageEventLog)
                                                                                        {
                                                                                            Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.ShowHelpEvent((Aow.Units.EventLog.THelpMessageEventLog)Event, Mode, Done);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (Event is Aow.Units.EventLog.TSaveGameEventLog)
                                                                                            {
                                                                                                Aow.Units.aowMapEvents.aowMapEvents.ShowSaveGameEvent(Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow, (Aow.Units.EventLog.TSaveGameEventLog)Event, Done);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (Event is Aow.Units.AoWE.TSendEmailGameEventLog)
                                                                                                {
                                                                                                    Aow.Units.E3EventDialogs.E3EventDialogs.SendEmailGameDlg.ShowSendDialog((Aow.Units.AoWE.TSendEmailGameEventLog)Event, Done);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (Event is Aow.Units.AoWE.TSMTPEventLog)
                                                                                                    {
                                                                                                        Aow.Units.E3EventDialogs.E3EventDialogs.ShowSMTPEvent(Aow.Units.AoWSMTPDlg.AoWSMTPDlg.SMTPDlg, (Aow.Units.AoWE.TSMTPEventLog)Event, Done);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (Event is Aow.Units.EventLog.TPasswordEventLog)
                                                                                                        {
                                                                                                            Aow.Units.aowMapEvents.aowMapEvents.ShowPasswordEvent(Aow.Units.E3EventDialogs.E3EventDialogs.PasswordDlg, (Aow.Units.EventLog.TPasswordEventLog)Event, Done);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (Event is Aow.Units.ItemExpS.TItemExplorationSiteEventLog)
                                                                                                            {
                                                                                                                Aow.Units.aowMapEvents.aowMapEvents.ShowItemsFound(Aow.Units.E3EventDialogs.E3EventDialogs.ItemsFoundDlg, (Aow.Units.ItemExpS.TItemExplorationSiteEventLog)Event, Done);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (Event is Aow.Units.City.TCityProductionEventLog)
                                                                                                                {
                                                                                                                    Aow.Units.AoWCityScreen.AoWCityScreen.CityScreen.ProductionEvent((Aow.Units.City.TCityProductionEventLog)Event);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (Event is Aow.Units.PrdPlace.TProductionPlaceEventLog)
                                                                                                                    {
                                                                                                                        Aow.Units.AoWProdScreen.AoWProdScreen.ProductionScreen.ProductionEvent((Aow.Units.PrdPlace.TProductionPlaceEventLog)Event);
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        if (Event is Aow.Units.EventLog.TUserEventLog)
                                                                                                                        {
                                                                                                                            Done(Event);
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // aowMapEvents.pas:287
        public void TheMapGameOver()
        {
        }

        // aowMapEvents.pas:290
        public void TheMapGemsChanged(sbyte Player)
        {
            bool Update = default;
            Update = false;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
            {
                Update = true;
            }
            else
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap.FSeatedPlayer == Player)
                    {
                        Update = true;
                    }
                }
            }

            if (Update)
            {
                if (Player == -1)
                {
                    Update = false;
                }
                else
                {
                    var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__17.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        Update = false;
                    }
                }

                if (!Update)
                {
                    // { The original clears both income signs when the gold display is cleared. }
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGTLbl.Text = (AnsiString)"";
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.Text = (AnsiString)"";
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.Text = (AnsiString)"";
                }
                else
                {
                    var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGTLbl.Text = Aow.Rtl.Rtl.IntToStr(call__18.FGems);
                    if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible && (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 0))
                    {
                        Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
                    }
                }

                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TGTLbl.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGTLbl.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TGSign.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TMTSign.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.Text;
            }
        }

        // aowMapEvents.pas:320
        public void TheMapGlobalMagicChanged()
        {
            Aow.Units.E3Globals.E3Globals.GlobalMagicChanged(Aow.Units.AoWExeForms.AoWExeForms.MagicWin);
        }

        // aowMapEvents.pas:323
        public void TheMapIncomeChanged(sbyte Player)
        {
            bool Update = default;
            int NetIncome = default;
            Update = false;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
            {
                Update = true;
            }
            else
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap.FSeatedPlayer == Player)
                    {
                        Update = true;
                    }
                }
            }

            if (Update)
            {
                if (Player == -1)
                {
                    Update = false;
                }
                else
                {
                    var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__19.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        Update = false;
                    }
                }

                if (!Update)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGILbl.Text = (AnsiString)"";
                }
                else
                {
                    var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__21 = call__20.GetUpkeep();
                    var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    NetIncome = call__22.FIncome - call__21;
                    if (NetIncome >= 0)
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.Text = AnsiString.FromBytes(43);
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.TextColor = 7864183;
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGILbl.TextColor = 7864183;
                    }
                    else
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.Text = AnsiString.FromBytes(45);
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.TextColor = 119;
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGILbl.TextColor = 119;
                    }

                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGILbl.Text = Aow.Rtl.Rtl.IntToStr(System.Math.Abs(NetIncome));
                    if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible && (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 0))
                    {
                        Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
                    }
                }

                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TGILbl.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGILbl.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TGSign.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TGSign.TextColor = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGSign.TextColor;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TGILbl.TextColor = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WGILbl.TextColor;
            }
        }

        // aowMapEvents.pas:362
        public void TheMapMapChanged(ref Aow.Units.AoWE.TMapChangedInfo Info)
        {
            if (Info.MapLevel == Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FLevel)
            {
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.DrawMap(Info.Bounds.Left - 6, Info.Bounds.Top - 6, Info.Bounds.Right - Info.Bounds.Left, Info.Bounds.Bottom - Info.Bounds.Top);
            }
        }

        // aowMapEvents.pas:370
        public void TheMapScannerIconsChanged(ref Aow.Units.AoWE.TMapChangedInfo Info)
        {
            if (Info.MapLevel == Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FLevel)
            {
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetObDrawn(false);
            }
        }

        // aowMapEvents.pas:375
        public void TheMapPlayerMagicChanged(sbyte Player)
        {
            bool Update = default;
            int NetIncome = default;
            Aow.Units.E3Globals.E3Globals.MagicPlayerChanged(Aow.Units.AoWExeForms.AoWExeForms.MagicWin, unchecked((sbyte)Player));
            Aow.Units.AoWExeForms.AoWExeForms.SpellBook.PlayerChanged(unchecked((sbyte)Player));
            Update = false;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
            {
                Update = true;
            }
            else
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap.FSeatedPlayer == Player)
                    {
                        Update = true;
                    }
                }
            }

            if (Update)
            {
                if (Player == -1)
                {
                    Update = false;
                }
                else
                {
                    var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__23.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        Update = false;
                    }
                }

                if (!Update)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTLbl.Text = (AnsiString)"";
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMILbl.Text = (AnsiString)"";
                }
                else
                {
                    var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTLbl.Text = Aow.Rtl.Rtl.IntToStr(call__24.FMagicControl.FMana);
                    var call__25 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__26 = call__25.FMagicControl.GetManaIncome();
                    var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__28 = call__27.FMagicControl.GetManaUpkeep();
                    NetIncome = call__26 - call__28;
                    if (NetIncome >= 0)
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.Text = AnsiString.FromBytes(43);
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.TextColor = 7864183;
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMILbl.TextColor = 7864183;
                    }
                    else
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.Text = AnsiString.FromBytes(45);
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.TextColor = 119;
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMILbl.TextColor = 119;
                    }

                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMILbl.Text = Aow.Rtl.Rtl.IntToStr(System.Math.Abs(NetIncome));
                }

                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TMTLbl.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTLbl.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TMILbl.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMILbl.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TMTSign.Text = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.Text;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TMTSign.TextColor = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMTSign.TextColor;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TMILbl.TextColor = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.WMILbl.TextColor;
            }
        }

        // aowMapEvents.pas:1021
        public void TheMapLogbookChanged(sbyte Player)
        {
            if (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
            {
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.UpdateEvents();
            }
        }

        // aowMapEvents.pas:1026
        public void TheMapPlayerTerminated(sbyte Player)
        {
            /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowMapEvents.pas:409
        public void TheMapSeatedPlayerChanged(sbyte Player)
        {
            if (Player == -1)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.HideWindows();
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.CurP.Visible = false;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.FWorldClockDelay = 0;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FCurPlayer = 12;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FLastPlayer = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FCurPlayer;
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FAnimationFrame = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FCurPlayer * 7;
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.ShowWindows();
                var aowTurnButton = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msActivated) && !aowTurnButton.FAnimating)
                {
                    if (Player > -1)
                    {
                        var call__30 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        aowTurnButton.FCurPlayer = call__30.FPlayerRace;
                    }
                    else
                    {
                        aowTurnButton.FCurPlayer = -2;
                    }

                    if (aowTurnButton.FCurPlayer == -1)
                    {
                        aowTurnButton.FCurPlayer = 12;
                    }

                    aowTurnButton.FLastPlayer = -1;
                }

                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.CurP.Visible = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmTurn;
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.UpdatePlayer();
                if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible)
                {
                    Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
                }
            }

            TheMapGemsChanged(unchecked((sbyte)Player));
            TheMapIncomeChanged(unchecked((sbyte)Player));
            TheMapPlayerMagicChanged(unchecked((sbyte)Player));
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.PInd.FDrawn = false;
            Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.UpdateEvents();
        }

        // aowMapEvents.pas:440
        public void TheMapShowMessage(ref Aow.Units.AoWE.TShowMessageInfo Info)
        {
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBMessEdit.TextColor = Info.Color;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBMessEdit.Text = Info.Text;
        }

        // aowMapEvents.pas:446
        public void TheMapStructureChanged(Aow.Units.AoWE.TStructure Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.UpdateStructureInfo(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow, Structure);
            if (Structure != null)
            {
                Aow.Units.AoWCityScreen.AoWCityScreen.CityScreen.CityChanged(Structure);
                Aow.Units.AoWProdScreen.AoWProdScreen.ProductionScreen.PlaceChanged(Structure);
            }

            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible && (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected < 3))
            {
                Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            }
        }

        // aowMapEvents.pas:457
        public void TheMapStructureDeselected(Aow.Units.AoWE.TStructure Structure)
        {
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSign != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.FSign.ChangeText(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.SignText.Items);
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.FStructure = null;
            Aow.Units.AoWWinMan.AoWWinMan.General.FCity = null;
            Aow.Units.AoWWinMan.AoWWinMan.General.FProductionPlace = null;
            Aow.Units.AoWWinMan.AoWWinMan.General.FProductionControl = null;
            Aow.Units.AoWWinMan.AoWWinMan.General.FSign = null;
            Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.StructNameLbl.Text = (AnsiString)"";
            Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.StructLine.Visible = false;
            Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StructButton.Visible = false;
        }

        // aowMapEvents.pas:467
        public void TheMapStructureDestroyed(Aow.Units.AoWE.TStructure Structure)
        {
            if ((Structure != null) && (Structure == Aow.Units.AoWWinMan.AoWWinMan.General.FStructure))
            {
                TheMapStructureDeselected(Structure);
            }

            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible)
            {
                if ((Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 2) || (Aow.Units.AoWRealm.AoWRealm.RealmWin.HeaderTab.Selected == 1))
                {
                    Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
                }
            }
        }

        // aowMapEvents.pas:473
        public void TheMapStructureSelected(Aow.Units.AoWE.TStructure Structure)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.FPreviousStructure = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure;
            Aow.Units.AoWWinMan.AoWWinMan.General.FStructure = Structure;
            Aow.Units.AoWWinMan.AoWWinMan.General.FProductionControl = null;
            Aow.Units.AoWInfoWin.AoWInfoWin.ShowStructureInfo(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow, Structure);
        }

        // aowMapEvents.pas:481
        public void TheMapItemsDeselected()
        {
            Aow.Units.E3Globals.E3Globals.HideItemSelection(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow);
        }

        // aowMapEvents.pas:484
        public void TheMapItemsSelected(Aow.Units.AoWE.TItemHS Items)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ShowItemSelection(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow, Items);
        }

        // aowMapEvents.pas:487
        public void TheMapTargetSelectionChange(Aow.Units.AoWE.TTargetSelectionControl Control, Aow.Units.AoWE.TTargetSelectionChange Change)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ShowTargetSelection(Aow.Units.AoWWorldMap.AoWWorldMap.TargetSelectionDlg, Control, Change);
        }

        // aowMapEvents.pas:490
        public void TheMapUnitChanged(Aow.Units.AoWE.TAbstractUnit U)
        {
            if (U == Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit)
            {
                if (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible)
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                }

                if (Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopup.Visible)
                {
                    Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopupShow(this);
                }
            }

            Aow.Units.AoWExeForms.AoWExeForms.SpellBook.UnitChanged(U);
        }

        // aowMapEvents.pas:500
        public void TheMapCastSpell(Aow.Units.AoWE.TSpellCaster Caster)
        {
            if (Caster is Aow.Units.AoWE.TUnitSpellCaster)
            {
                Aow.Units.aowMapEvents.aowMapEvents.ShowUnitSpellCaster(Aow.Units.E3Globals.E3Globals.CastUnitSpellDlg, (Aow.Units.AoWE.TUnitSpellCaster)Caster);
            }
            else
            {
                if (Caster is Aow.Units.GlobalSpells.TDisjunctionSpellCaster)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.DisjunctionDlg.Execute((Aow.Units.GlobalSpells.TDisjunctionSpellCaster)Caster);
                }
                else
                {
                    if (Caster is Aow.Units.UnitSpells.TCosmeticSurgerySpellCaster)
                    {
                        Aow.Units.E3Globals.E3Globals.DeformFaceDlg.Execute((Aow.Units.UnitSpells.TCosmeticSurgerySpellCaster)Caster);
                    }
                }
            }
        }

        // aowMapEvents.pas:507
        public void TheMapHSChanged(Aow.Units.HSEngine.THexagonSprite HS)
        {
            if (HS is Aow.Units.AoWE.TItemHS)
            {
                Aow.Units.AoWInfoWin.AoWInfoWin.UpdateItemSelection(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow, (Aow.Units.AoWE.TItemHS)HS);
            }
        }

        // aowMapEvents.pas:512
        public void TheMapRelationsChanged()
        {
            Aow.Units.aowMapEvents.aowMapEvents.RefreshRelations(Aow.Units.E1Globals.E1Globals.RelWindow);
        }

        // aowMapEvents.pas:515
        public void TheMapDiplomaticChange()
        {
            Aow.Units.aowMapEvents.aowMapEvents.RefreshRelations(Aow.Units.E1Globals.E1Globals.RelWindow);
        }

        // aowMapEvents.pas:518
        public void TheMapStructureDblClick(Aow.Units.AoWE.TStructure Structure)
        {
            if (Structure == Aow.Units.AoWWinMan.AoWWinMan.General.FStructure)
            {
                if ((Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.City.TCity) && (((Aow.Units.City.TCity)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
                {
                    Aow.Units.AoWCityScreen.AoWCityScreen.CityScreen.ShowCity((Aow.Units.City.TCity)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure);
                }
                else
                {
                    bool bool__31 = (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.PrdPlace.TProductionPlace);
                    if (bool__31)
                    {
                        var call__32 = ((Aow.Units.PrdPlace.TProductionPlace)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).CanDblClick();
                        bool__31 = call__32;
                    }

                    if (bool__31 && (((Aow.Units.AoWE.TPlayerStructure)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
                    {
                        Aow.Units.AoWProdScreen.AoWProdScreen.ProductionScreen.ShowPlace((Aow.Units.PrdPlace.TProductionPlace)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure);
                    }
                    else
                    {
                        if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.WizardsTower.TWizardsTower)
                        {
                            ((Aow.Units.WizardsTower.TWizardsTower)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Enter();
                        }
                        else
                        {
                            if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.Arena.TArena)
                            {
                                ((Aow.Units.Arena.TArena)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Enter();
                            }
                        }
                    }
                }
            }
        }
    }

    public static partial class aowMapEvents
    {
        // aowMapEvents.pas:998
        public static void ShowItemsFound(Aow.Units.AoWItemsFoundDlg.TItemsFoundDlg Dialog, Aow.Units.ItemExpS.TItemExplorationSiteEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.ItemExpS.TItemExplorationSiteEventLog Log = default;
            Dialog.Reset();
            Dialog.FOnDone = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Log = Dialog.FEvent;
            Dialog.TitleLbl.Text = Log.FTitle;
            var call__34 = Log.FMessages.Text;
            Dialog.MessageMemo.Items.Text = call__34;
            Dialog.LeftBtn.Visible = Log.FItemIDs.Count > 5;
            Dialog.RightBtn.Visible = Log.FItemIDs.Count > 5;
            if (Dialog.GetHero() == null)
            {
                Dialog.OKPnl.Visible = true;
            }
            else
            {
                Dialog.PickupPnl.Visible = true;
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(54, true, true);
            Dialog.Dlg.FDrawn = false;
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:975
        public static void ShowCityNegotiation(Aow.Units.AoWCityJoinDlg.TCityJoinDlg Dialog, Aow.Units.AoWE.TNegotiateRequestEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.AoWE.TNegotiateRequestEventLog Log = default;
            Aow.Units.City.TCity Town = default;
            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Log = Dialog.FEvent;
            var call__36 = Log.FJoinMessage.Text;
            Dialog.Memo.Items.Text = call__36;
            Dialog.ApproachLbl.Text = Log.FText;
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Log.FX, Log.FY, Log.FL);
            var call__38 = call__37.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
            Town = (Aow.Units.City.TCity)call__38;
            Dialog.FRace = unchecked((sbyte)Town.FRace);
            var call__39 = Town.GetTopCityActionImage(Aow.Units.City.TCityAction.caNone);
            Dialog.CityPicLine.ImageTop = call__39;
            var call__40 = Town.GetMiddleCityActionImage(Aow.Units.City.TCityAction.caNone);
            Dialog.CityPicLine.ImageMiddle = call__40;
            var call__41 = Town.GetBottomCityActionImage(Aow.Units.City.TCityAction.caNone);
            Dialog.CityPicLine.ImageBottom = call__41;
            Dialog.Dlg.Visible = true;
            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(62, true, true);
        }

        // aowMapEvents.pas:942
        public static void ShowTargetSelection(Aow.Units.AoWTargetSelection.TTargetSelectionDlg Dialog, Aow.Units.AoWE.TTargetSelectionControl Control, Aow.Units.AoWE.TTargetSelectionChange Change)
        {
            if (Change == Aow.Units.AoWE.TTargetSelectionChange.tscActivated)
            {
                Dialog.HeaderLbl.Text = Control.FTitle;
                Dialog.MainBtn.Text = Control.FCancelText;
                Dialog.DefaultMemo.Items.Assign(Control.FText);
                Dialog.Dlg.Visible = true;
                if (!Control.FTargetMode.Contains(Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance))
                {
                    var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnlimitedRStr);
                    Dialog.TargetLbl.Text = Aow.Rtl.Rtl.LowerCase(call__43);
                }
                else
                {
                    Dialog.TargetLbl.Text = Aow.Rtl.Rtl.IntToStr(Control.FRange);
                }

                Dialog.BorderImage.ImageTop = Control.FSourceImage;
                Dialog.BorderImage.ImageMiddle = Control.FValidTargetImage;
                Dialog.BorderImage.ImageBottom = Control.FInvalidTargetImage;
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
                var window = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                window.FModal = true;
                window.Priority = 500;

                Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Priority = Dialog.Dlg.Priority;
            }
            else
            {
                if (Change == Aow.Units.AoWE.TTargetSelectionChange.tscDeactivated)
                {
                    Dialog.Dlg.Visible = false;
                    Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Priority = 0;
                    Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.FModal = false;
                    Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Priority = 10;
                    Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
                }
            }
        }

        // aowMapEvents.pas:909
        public static void ShowUnitSpellCaster(Aow.Units.AoWCastUnitSpellDlg.TCastUnitSpellDlg Dialog, Aow.Units.AoWE.TUnitSpellCaster Caster)
        {
            Aow.Units.AoWE.THero Hero = default;
            Aow.Units.AoWE.TSpell Spell = default;
            if (Dialog.FCaster != null)
            {
                Dialog.FCaster.Release();
            }

            Dialog.FCaster = Caster;
            Dialog.FCaster.AddRef();
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(Dialog.FCaster.FCasterSourceID);
            Hero = (Aow.Units.AoWE.THero)call__46;
            if (Hero != null)
            {
                var call__47 = Hero.GetName();
                Dialog.CasterNameLbl.Text = call__47;
                var call__48 = Hero.GetCastingPoints();
                var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingPointsRStr);
                var call__50 = Hero.GetCastingPointsMax();
                Dialog.CastingPointsLbl.Text = (((call__49 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__48)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__50);
            }

            var call__51 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Dialog.FCaster.FCasterSpellID);
            Spell = call__51;
            Dialog.SpellNameLbl.Text = Spell.FSpellName;
            // { Original still uses Hero below if the source-unit lookup failed. }
            var call__52 = Hero.CastingMana(Spell);
            var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingCostRStr);
            var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
            Dialog.SpellManaLbl.Text = (((call__53 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__52)) + AnsiString.FromBytes(32)) + call__54;
            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpkeepRStr);
            var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
            Dialog.SpellUpkeepLbl.Text = (((call__55 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(Spell.FManaUpkeep)) + AnsiString.FromBytes(32)) + call__56;
            var call__57 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Hero.FPlayer);
            var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
            Dialog.CastingManaLbl.Text = (call__58 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__57.FMagicControl.FMana);
            Dialog.U1.Checked = false;
            Dialog.U2.Checked = false;
            Dialog.U3.Checked = false;
            Dialog.U4.Checked = false;
            Dialog.U5.Checked = false;
            Dialog.U6.Checked = false;
            Dialog.U7.Checked = false;
            Dialog.U8.Checked = false;
            Dialog.SelectionInfoLbl.Text = (AnsiString)"";
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:891
        public static void RefreshRelations(Aow.Units.AoWRelations.TRelWindow Dialog)
        {
            if (Dialog.RelWin.Visible)
            {
                Dialog.FillPlayers();
                Dialog.RelPnl.FAligned = false;
                Dialog.RelWin.FDrawn = false;
                if (Dialog.FSelectedPlayer != -1)
                {
                    Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FSelectedPlayer));
                }

                if (Dialog.FInfoPlayer != -1)
                {
                    if (Dialog.RNewActionPnl.Visible)
                    {
                        Dialog.ShowPlayerActions(unchecked((sbyte)Dialog.FInfoPlayer));
                    }
                    else
                    {
                        Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FInfoPlayer));
                    }
                }
                else
                {
                    if (Dialog.FSelectedRace != -1)
                    {
                        Dialog.HighlightRace(unchecked((sbyte)Dialog.FSelectedRace));
                    }
                }

                if (Dialog.FInfoRace != -1)
                {
                    Dialog.ShowRaceInformation(unchecked((byte)Dialog.FInfoRace));
                }
            }
        }

        // aowMapEvents.pas:531
        public static void ShowRebellion(Aow.Units.AoWCityRebelDlg.TCityRebelDlg Dialog, Aow.Units.City.TCityRebellionEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.FOnClose = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.CityNameLabel.Text = Dialog.FEvent.FCityName;
            Dialog.RebelWin.Visible = true;
        }

        // aowMapEvents.pas:543
        public static void ShowReceivedMessage(Aow.Units.AoWMsgReceiveDlg.TMsgReceiveDlg Dialog, Aow.Units.EventLog.TEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.FDone = Done;
            var call__62 = Event.GetText();
            Dialog.HeaderMemo.Items.Text = call__62;
            var call__63 = ((Aow.Units.EventLog.TGameOverEventLog)Event).FMessage.Text;
            Dialog.MessageMemo.Items.Text = call__63;
            var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Event.FPlayer);
            Dialog.LShield.CurrentImage = call__64.FPlayerRace + 13;
            Dialog.RShield.CurrentImage = Dialog.LShield.CurrentImage;
            Dialog.MsgReceiveWin.Visible = true;
        }

        // aowMapEvents.pas:558
        public static void ShowTutorialEvent(Aow.Units.AoWTutorialWin.TTutorialWindow Dialog, Aow.Units.EventLog.TTutorialMessageEventLog Event, bool Unused, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.AbVSB.Position = 0;
            Dialog.FinishDialog();
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.FDone = Done;
            Dialog.HeaderMemo.Items.Clear();
            var call__66 = Event.FMessage.Count;
            if (call__66 > 0)
            {
                var call__67 = Event.FMessage[0];
                Dialog.HeaderMemo.Items.Add(call__67);
            }

            Dialog.MessageMemo.Items.Clear();
            Dialog.MessageMemo.Items.AddStrings(Event.FMessage);
            var call__68 = Dialog.MessageMemo.Items.Count;
            if (call__68 > 0)
            {
                Dialog.MessageMemo.Items.Delete(0);
            }

            var call__69 = Dialog.MessageMemo.Items.Count;
            bool bool__70 = (call__69 > 0);
            if (bool__70)
            {
                var call__71 = Dialog.MessageMemo.Items[0];
                bool__70 = (call__71 == ((AnsiString)""));
            }

            if (bool__70)
            {
                Dialog.MessageMemo.Items.Delete(0);
            }

            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:577
        public static void ShowFlagMessage(Aow.Units.AoWTutorialWin.TTutorialWindow Dialog, Aow.Units.FlagEvents.TFlagMessageEventLog Event, bool Unused, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.AbVSB.Position = 0;
            Dialog.FinishDialog();
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.FDone = Done;
            Dialog.HeaderMemo.Items.Clear();
            Dialog.HeaderMemo.Items.Add(Event.FHelpFileName);
            Dialog.MessageMemo.Items.Clear();
            Dialog.MessageMemo.Items.AddStrings(Event.FMessage);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:594
        public static void ShowArmyInfo(Aow.Units.AoWArmyInfoDlg.TArmyInfoDlg Dialog, Aow.Units.AoWE.TArmyInfoEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.AoWE.TArmyInfoEventLog Log = default;
            Dialog.FOnClose = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            var call__74 = Event.FMessages.Text;
            Dialog.Message.Items.Text = call__74;
            Dialog.Title.Text = Event.FTitle;
            Dialog.HeaderLbl.Text = Event.FDescription;
            Dialog.Dlg.Visible = true;
            Dialog.FUnitInfoShown = false;
        }

        // aowMapEvents.pas:611
        public static void ShowTower(Aow.Units.AoWWizardsTowerDlg.TWizardsTowerDlg Dialog, Aow.Units.WizardsTower.TEnterWizardsTowerEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.WizardsTower.TWizardsTower Tower = default;
            Aow.Units.AoWE.TSpell Spell = default;
            Dialog.FOnClose = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Tower = Dialog.FEvent.FTower;
            if (Tower.FSpellID != 0)
            {
                var call__76 = Dialog.FEvent.FTower.BuyCost();
                var call__77 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ForXGoldRStr);
                var call__78 = Aow.Units.AoWE.AoWE.Format(call__77, [Aow.Rtl.Rtl.IntToStr(call__76)]);
                Dialog.PriceLbl.Text = call__78 + AnsiString.FromBytes(46);
                var call__79 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Tower.FSpellID);
                Spell = call__79;
                Dialog.SpellName.Text = Spell.FSpellName;
                var call__80 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var call__81 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelRStr);
                Dialog.SpellClass.Text = (((call__80 + ((AnsiString)", ")) + call__81) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell.FSpellLevel);
                var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingCostRStr);
                var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
                Dialog.SpellCost.Text = (((call__82 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(Spell.FBaseManaCost)) + AnsiString.FromBytes(32)) + call__83;
                var call__84 = Spell.FSpellText.Text;
                Dialog.ResearchMemo.Items.Text = call__84;
            }
            else
            {
                Dialog.PriceLbl.Text = (AnsiString)"";
                var call__85 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                Dialog.SpellName.Text = call__85;
                Dialog.SpellClass.Text = (AnsiString)"";
                Dialog.SpellCost.Text = (AnsiString)"";
                Dialog.ResearchMemo.Items.Text = (AnsiString)"";
            }

            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:647
        public static void ResetPartyJoin(Aow.Units.AoWPartyJoin.TPartyJoinDlg Dialog)
        {
            Dialog.LeftBtn.Visible = true;
            Dialog.MiddleBtn.Visible = true;
            Dialog.RightBtn.Visible = true;
            var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.PartyJoinRStr);
            Dialog.Title.Text = call__87;
        }

        // aowMapEvents.pas:656
        public static void UpdatePartyJoin(Aow.Units.AoWPartyJoin.TPartyJoinDlg Dialog)
        {
            if (Dialog.ValidateJoin())
            {
                Dialog.FUnits.Clear();
                var call__89 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
                Dialog.FFaceUnit = call__89;
                Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
                var call__90 = Dialog.FJoinEvent.FJoinMessage.Text;
                Dialog.JoinMemo.Items.Text = call__90;
            }
        }

        // aowMapEvents.pas:668
        public static void ResetHeroJoin(Aow.Units.AoWHeroJoinDlg.THeroJoinDlg Dialog)
        {
            Dialog.LeftBtn.Visible = true;
            Dialog.MiddleBtn.Visible = true;
            Dialog.RightBtn.Visible = true;
        }

        // aowMapEvents.pas:676
        public static void UpdateHeroJoin(Aow.Units.AoWHeroJoinDlg.THeroJoinDlg Dialog)
        {
            if (Dialog.ValidateJoin())
            {
                Dialog.FUnits.Clear();
                var call__93 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
                Dialog.FFaceUnit = call__93;
                Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
                var call__94 = ((Aow.Units.AoWE.THero)Dialog.FFaceUnit).GetLevel();
                var call__95 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelXRStr);
                var call__96 = Aow.Units.AoWE.AoWE.Format(call__95, [Aow.Rtl.Rtl.IntToStr(call__94)]);
                Dialog.HeroLevel.Text = call__96;
                Dialog.HeroName.Text = ((Aow.Units.AoWE.THero)Dialog.FFaceUnit).FHeroName;
                var call__97 = Dialog.FJoinEvent.FJoinMessage.Text;
                Dialog.JoinMemo.Items.Text = call__97;
            }
        }

        // aowMapEvents.pas:690
        public static void ShowPartyJoin(Aow.Units.AoWPartyJoin.TPartyJoinDlg Dialog, Aow.Units.AoWE.TNegotiateRequestEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ResetPartyJoin(Dialog);
            Dialog.Dlg.Visible = true;
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__99 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AcceptRStr);
            Dialog.LeftBtn.Text = call__99;
            var call__100 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AttackRStr);
            Dialog.MiddleBtn.Text = call__100;
            var call__101 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
            Dialog.RightBtn.Text = call__101;
            Aow.Units.aowMapEvents.aowMapEvents.UpdatePartyJoin(Dialog);
            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(70, true, true);
        }

        // aowMapEvents.pas:706
        public static void ShowPrisonersJoin(Aow.Units.AoWPartyJoin.TPartyJoinDlg Dialog, Aow.Units.Dungeon.TPrisonersJoinEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ResetPartyJoin(Dialog);
            Dialog.Dlg.Visible = true;
            var call__103 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PrisonerLiberationRStr);
            Dialog.Title.Text = call__103;
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__104 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AcceptRStr);
            Dialog.LeftBtn.Text = call__104;
            Dialog.MiddleBtn.Visible = false;
            var call__105 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RejectRStr);
            Dialog.RightBtn.Text = call__105;
            Aow.Units.aowMapEvents.aowMapEvents.UpdatePartyJoin(Dialog);
        }

        // aowMapEvents.pas:722
        public static void ShowHeroJoin(Aow.Units.AoWHeroJoinDlg.THeroJoinDlg Dialog, Aow.Units.AoWE.THeroJoinEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ResetHeroJoin(Dialog);
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__107 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AcceptRStr);
            Dialog.LeftBtn.Text = call__107;
            Dialog.MiddleBtn.Visible = false;
            var call__108 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
            Dialog.RightBtn.Text = call__108;
            Aow.Units.aowMapEvents.aowMapEvents.UpdateHeroJoin(Dialog);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:736
        public static void ShowHeroNegotiation(Aow.Units.AoWHeroJoinDlg.THeroJoinDlg Dialog, Aow.Units.AoWE.TNegotiateRequestEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ResetHeroJoin(Dialog);
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__110 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AcceptRStr);
            Dialog.LeftBtn.Text = call__110;
            var call__111 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AttackRStr);
            Dialog.MiddleBtn.Text = call__111;
            var call__112 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
            Dialog.RightBtn.Text = call__112;
            Aow.Units.aowMapEvents.aowMapEvents.UpdateHeroJoin(Dialog);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:750
        public static void ShowPrisonerHero(Aow.Units.AoWHeroJoinDlg.THeroJoinDlg Dialog, Aow.Units.Dungeon.TPrisonersJoinEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ResetHeroJoin(Dialog);
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__114 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AcceptRStr);
            Dialog.LeftBtn.Text = call__114;
            Dialog.MiddleBtn.Visible = false;
            var call__115 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RejectRStr);
            Dialog.RightBtn.Text = call__115;
            Aow.Units.aowMapEvents.aowMapEvents.UpdateHeroJoin(Dialog);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:789
        public static void ShowHeroUpgrade(Aow.Units.AoWHeroUpgradeDlg.THeroUpgradeDlg Dialog, Aow.Units.AoWE.THeroUpgradeEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Completed)
        {
            Dialog.FReferencedHero = Event;
            Dialog.FReferencedHero.AddRef();
            Dialog.FOnClose = Completed;
            if (Dialog.ValidateHero())
            {
                var call__117 = Dialog.FHero.Copy(Aow.Units.AoWE.AoWE.AoWEngine);
                Dialog.FEditedHero = (Aow.Units.AoWE.THero)call__117;
                Dialog.UpdateStatistics();
                Dialog.RefreshAbilities();
                Dialog.UnitName.Text = Dialog.FHero.FHeroName;
                var call__118 = Dialog.FHero.GetLevel();
                var call__119 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelRStr);
                Dialog.LevelLbl.Text = (call__119 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__118);
                if ((Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel) > 1)
                {
                    var call__120 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.XMadeYLevelsRStr);
                    var call__121 = Aow.Units.AoWE.AoWE.Format(call__120, [Dialog.FHero.FHeroName, (int)(Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel)]);
                    Dialog.HeaderLbl.Text = call__121 + AnsiString.FromBytes(33);
                }
                else
                {
                    var call__122 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.XMadeALevelRStr);
                    var call__123 = Aow.Units.AoWE.AoWE.Format(call__122, [Dialog.FHero.FHeroName]);
                    Dialog.HeaderLbl.Text = call__123 + AnsiString.FromBytes(33);
                }

                Dialog.Dlg.Visible = true;
            }
        }

        // aowMapEvents.pas:811
        public static void ShowMessageEvent(Aow.Units.AoWMessageDlg.TMessageDlg Dialog, Aow.Units.EventLog.TMessageEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.EventLog.TMessageEventLog Log = default;
            Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Dialog);
            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Log = Dialog.FEvent;
            Dialog.TitleLbl.Text = Log.FTitle;
            switch (Log.FLayout)
            {
                case Aow.Units.EventLog.TMessageEventLogLayout.melNormal:
                    var call__125 = Log.FMessage.Text;
                    Dialog.MessageMemo.Items.Text = call__125;
                    var call__126 = Dialog.MessageMemo.Items.Text;
                    if (call__126.Length < 80)
                    {
                        Dialog.MessageMemo.Items.Insert(0, (AnsiString)"");
                    }

                    break;
                case Aow.Units.EventLog.TMessageEventLogLayout.melHeader:
                    Dialog.MemoPnl.Visible = false;
                    Dialog.HeaderMemoPnl.Visible = true;
                    Dialog.HeaderLbl.Text = Log.FHeader;
                    var call__127 = Log.FMessage.Text;
                    Dialog.HeaderMemo.Items.Text = call__127;
                    break;
            }

            switch (Log.FButtons)
            {
                case Aow.Units.EventLog.TMessageEventLogButtons.mebOk:
                    Dialog.OKPnl.Visible = true;
                    break;
                case Aow.Units.EventLog.TMessageEventLogButtons.mebOkCancel:
                    Dialog.OKCancelPnl.Visible = true;
                    break;
                case Aow.Units.EventLog.TMessageEventLogButtons.mebYesNo:
                    Dialog.YesNoPnl.Visible = true;
                    break;
                case Aow.Units.EventLog.TMessageEventLogButtons.mebYesNoCancel:
                    Dialog.YesNoCancelPnl.Visible = true;
                    break;
            }

            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:844
        public static void ShowSaveGameEvent(Aow.Units.AoWFileDialog.TFileDlgWindow Dialog, Aow.Units.EventLog.TSaveGameEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.FDone = Done;
            Dialog.FSaveEvent = Event;
            Dialog.FSaveEvent.AddRef();
            Dialog.FMode = 1;
            Dialog.FileDialog.Visible = true;
            if (Event.FDirectory != ((AnsiString)""))
            {
                Aow.Rtl.Rtl.SetCurrentDir(Event.FDirectory);
            }

            Dialog.SFileName.Text = Event.FDefaultName;
            Dialog.Title.Text = Event.FTitle;
            Dialog.FindFiles();
        }

        // aowMapEvents.pas:858
        public static void ShowPasswordEvent(Aow.Units.AoWPasswordDlg.TPasswordDlg Dialog, Aow.Units.EventLog.TPasswordEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.EventLog.TPasswordEventLog Log = default;
            Dialog.Reset();
            Dialog.FDone = Done;
            Dialog.FPasswordEvent = Event;
            Dialog.FPasswordEvent.AddRef();
            Log = Dialog.FPasswordEvent;
            Dialog.TitleLbl.Text = Log.FTitle;
            Dialog.PasswordLbl.Text = Log.FPrompt;
            Dialog.PasswordEdit.Text = Log.FPassword;
            Dialog.PasswordEdit.FRedraw = true;
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:875
        public static void ShowMagicWindow(Aow.Units.AoWMagic.TMagicWin Window)
        {
            if (!Window.Dlg.Visible)
            {
                Aow.Units.E3Globals.E3Globals.RefreshMagicWindow(Window);
                Aow.Units.E3Globals.E3Globals.RefreshGlobalMagic(Window);
            }

            Window.Dlg.Visible = true;
        }

        // aowMapEvents.pas:885
        public static void ShowMagicEvent(Aow.Units.AoWMagic.TMagicWin Window, Aow.Units.AoWE.TPlayerMagicEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.aowMapEvents.aowMapEvents.ShowMagicWindow(Window);
            if (Done != null)
            {
                Done(Event);
            }
        }

        public static Aow.Units.aowMapEvents.TMapEvents MapEvents;
    }
}
