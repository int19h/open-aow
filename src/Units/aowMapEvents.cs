// Translated from aowMapEvents.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.HSEngine;
using Aow.Units.MapCtrl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.City;
using Aow.Units.WizardsTower;
using Aow.Units.Arena;
using Aow.Units.FlagEvents;
using Aow.Units.Dungeon;
using Aow.Units.ItemExpS;
using static Aow.Units.AoWArmyEventDlg.AoWArmyEventDlg;
using Aow.Units.AoWCityRebelDlg;
using Aow.Units.AoWMsgReceiveDlg;
using Aow.Units.AoWTutorialWin;
using Aow.Units.AoWArmyInfoDlg;
using Aow.Units.AoWWizardsTowerDlg;
using static Aow.Units.AoWArenaDlg.AoWArenaDlg;
using static Aow.Units.FastCombatWin.FastCombatWin;
using Aow.Units.AoWPartyJoin;
using Aow.Units.AoWHeroJoinDlg;
using static Aow.Units.AoWHeroInfoDlg.AoWHeroInfoDlg;
using Aow.Units.AoWHeroUpgradeDlg;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWPasswordDlg;
using Aow.Units.AoWMagic;
using Aow.Units.AoWRelations;
using Aow.Units.AoWCastUnitSpellDlg;
using Aow.Units.AoWTargetSelection;
using Aow.Units.AoWCityJoinDlg;
using Aow.Units.AoWItemsFoundDlg;
using static Aow.Units.AoWRealm.AoWRealm;
using static Aow.Units.AoWInfoWin.AoWInfoWin;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWUnitBanner.AoWUnitBanner;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.PrdPlace;
using static Aow.Units.AoWCityScreen.AoWCityScreen;
using static Aow.Units.AoWProdScreen.AoWProdScreen;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using Aow.Units.GlobalSpells;
using Aow.Units.UnitSpells;
using static Aow.Units.E3EventDialogs.E3EventDialogs;
using static Aow.Units.AoWSMTPDlg.AoWSMTPDlg;
using static Aow.Units.AoWPInfoWin.AoWPInfoWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using static Aow.Units.AoWUnitWindow.AoWUnitWindow;

namespace Aow.Units.aowMapEvents
{
    // map control
    public partial class TMapEvents : TDataModule
    {
        public TMapEvents(TComponent owner) : base(owner)
        { }

        public TAoWMapControl TheMap;
        // aowMapEvents.pas:93
        public void TheMapActivateAbility(TActivateAbilityInfo Request)
        {
            if (Request.FAbilityID == 52)
            {
                SpellBook.ShowCasting(Request);
            }
            else
            {
                if (Request.FAbilityID == 125)
                {
                    ConstructDlg.Execute(Request);
                }
                else
                {
                    if (Request is TUnitSelectorAAI)
                    {
                        UnitSelectionDlg.Execute((TUnitSelectorAAI)Request);
                    }
                }
            }
        }

        // aowMapEvents.pas:100
        public void TheMapArmyChanged(TArmy Army)
        {
            if (Army != null)
            {
                if (Army == General.FUnitList)
                {
                    TheMapArmySelected(Army);
                    if (UnitWindow.UnitWin.Visible)
                    {
                        UnitWindow.ShowUnit();
                    }

                    if (UnitBanner.BannerPopup.Visible)
                    {
                        UnitBanner.BannerPopupShow(this);
                    }
                }

                if (AoWHSMap.FSeatedPlayer == Army.FPlayer && RealmWin.Dlg.Visible && RealmWin.HeaderTab.Selected == 3)
                {
                    RealmWin.UpdateOverview();
                }
            }
            else
            {
                if (RealmWin.Dlg.Visible && RealmWin.HeaderTab.Selected == 3)
                {
                    RealmWin.UpdateOverview();
                }
            }
        }

        // aowMapEvents.pas:117
        public void TheMapArmyDeselected(TArmy Army)
        {
            General.FAbilities.Clear();
            General.FUnitList = null;
            AOWControlWin.AOWControlWin.ControlWin.MovesLbl.Text = (AnsiString)"";
            AOWControlWin.AOWControlWin.ControlWin.MoveIcon.Visible = false;
            AOWControlWin.AOWControlWin.ControlWin.Emp1Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp1Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp2Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp2Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp3Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp3Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp4Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp4Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp5Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp5Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp6Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp6Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp7Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp7Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp8Fr.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.Emp8Chk.Visible = true;
            AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = false;
            TBWindow.ConButton.Visible = false;
            if (UnitWindow.UnitWin.Visible)
            {
                UnitWindow.UnitWin.Visible = false;
            }
        }

        // aowMapEvents.pas:144
        public void TheMapArmyDestroyed(TArmy Army)
        {
            if (Army != null && Army == General.FUnitList)
            {
                TheMapArmyDeselected(Army);
            }

            if (RealmWin.Dlg.Visible && RealmWin.HeaderTab.Selected == 3)
            {
                RealmWin.UpdateOverview();
            }
        }

        // aowMapEvents.pas:150
        public void TheMapArmySelected(TArmy Army)
        {
            int I = default;
            int Count = default;
            Count = 0;
            {
                I = 0;
                byte end__1 = 7;
                for (; I <= end__1; I += 1)
                {
                    General.FSelectedUnits[I] = -1;
                }
            }

            {
                I = 0;
                int end__2 = Army.GetCount() - 1;
                for (; I <= end__2; I += 1)
                {
                    var call__3 = Army.GetUnitConcealedForPlayer(I, AoWHSMap.FSeatedPlayer);
                    if (!call__3)
                    {
                        General.FSelectedUnits[Count] = I;
                        Count += 1;
                    }
                }
            }

            General.FUnitList = Army;
            AOWControlWin.AOWControlWin.ControlWin.UpdateArmy();
            if (AoWHSMap.FSeatedPlayer == Army.FPlayer)
            {
                AoWHSMap.FTutorialControl.ShowMessage(63, true, true);
            }
        }

        // aowMapEvents.pas:167
        public void TheMapDayChanged(int Day)
        {
            if (AoWHSMap.Status.Contains(THSMapStatusElement.msActivated))
            {
                TBWindow.TurnButton.FLastPlayer = -1;
            }

            var call__4 = TranslateRStr(DayXRStr);
            var call__5 = AoWE.AoWE.Format(call__4, [Day]);
            PInfoW.Turn.Text = call__5;
            aowMapEvents.MapEvents.TheMapGemsChanged(AoWHSMap.FSeatedPlayer);
            aowMapEvents.MapEvents.TheMapIncomeChanged(AoWHSMap.FSeatedPlayer);
            if (RealmWin.Dlg.Visible)
            {
                RealmWin.UpdateOverview();
            }
        }

        // aowMapEvents.pas:187
        public void TheMapEndTurn(sbyte Player, bool Cancelled)
        {
            bool bool__6 = AoWHSMap.FSeatedPlayer == Player;
            if (bool__6)
            {
                var call__7 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                bool__6 = call__7.FGameOverStatus == TGameOverStatus.goNone;
            }

            if (bool__6)
            {
                if (Cancelled)
                {
                    TBWindow.TurnButton.FDrawn = false;
                    TBWindow.TurnButton.FButtonChecked = false;
                }
                else
                {
                    General.HideTransientWindows();
                }
            }

            TBWindow.PInd.FDrawn = false;
        }

        // aowMapEvents.pas:198
        public void TheMapExecuteEventLog(TEventLog Event, bool Mode, TExecuteDoneEvent Done)
        {
            TArmyHS ArmyHS = default;
            TAbstractUnit UnitObject = default;
            if (!Mode)
            {
                General.HideTransientWindows();
            }

            if (Event is TCombatEventLog)
            {
                ShowCombatEvent(FastCombatWindow, (TCombatEventLog)Event, Done);
            }
            else
            {
                if (Event is TArmyEventLog)
                {
                    ShowArmyEvent(ArmyEventDlg, (TArmyEventLog)Event, Done);
                }
                else
                {
                    if (Event is TCityRebellionEventLog)
                    {
                        aowMapEvents.ShowRebellion(CityRebelDlg, (TCityRebellionEventLog)Event, Done);
                    }
                    else
                    {
                        if (Event is TDiplomaticActionEventLog)
                        {
                            ShowDiplomaticEvent(RelWindow, (TDiplomaticActionEventLog)Event, Done);
                        }
                        else
                        {
                            if (Event is TNegotiateRequestEventLog)
                            {
                                var negotiateRequestEventLog = (TNegotiateRequestEventLog)Event;
                                if (negotiateRequestEventLog.FObject == TNegotiateObject.noCity)
                                {
                                    aowMapEvents.ShowCityNegotiation(CityJoinDlg, (TNegotiateRequestEventLog)Event, Done);
                                }
                                else
                                {
                                    if (negotiateRequestEventLog.FObject == TNegotiateObject.noArmy)
                                    {
                                        var call__9 = AoWHSMap.MapContainer.GetField(negotiateRequestEventLog.FX, negotiateRequestEventLog.FY, negotiateRequestEventLog.FL);
                                        var call__10 = call__9.FindOwnedChild(131607);
                                        ArmyHS = (TArmyHS)call__10;
                                        if (ArmyHS != null)
                                        {
                                            var call__11 = ArmyHS.FArmy.GetJoinSelection(negotiateRequestEventLog.FPlayer);
                                            var call__12 = ArmyHS.FArmy.Hero(call__11, AllHeroTypes);
                                            if (call__12 == null)
                                            {
                                                aowMapEvents.ShowPartyJoin(PartyJoinDlg, (TNegotiateRequestEventLog)Event, Done);
                                            }
                                            else
                                            {
                                                aowMapEvents.ShowHeroNegotiation(HeroJoinDlg, (TNegotiateRequestEventLog)Event, Done);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Event is TGameOverEventLog)
                                {
                                    if (AoWHSMap.FSeatedPlayer == Event.FPlayer)
                                    {
                                        AOWPyreScrn.AOWPyreScrn.PyreScreen.ShowGameOver((TGameOverEventLog)Event, Done);
                                    }
                                    else
                                    {
                                        aowMapEvents.ShowReceivedMessage(MsgReceiveDlg, Event, Done);
                                    }
                                }
                                else
                                {
                                    if (Event is TNewTurnEventLog)
                                    {
                                        NewTurnDlg.ShowNewTurn((TNewTurnEventLog)Event, Done);
                                    }
                                    else
                                    {
                                        if (Event is TCombatTypeRequestEventLog)
                                        {
                                            ShowCombatType(TCFCDialog, (TCombatTypeRequestEventLog)Event, Done);
                                        }
                                        else
                                        {
                                            if (Event is TShowBookEventLog)
                                            {
                                                ShowInformationBook(AoWWorldMap.AoWWorldMap.WorldMap, (TShowBookEventLog)Event, Done);
                                            }
                                            else
                                            {
                                                if (Event is TPlayerMagicEventLog)
                                                {
                                                    var playerMagicEventLog = (TPlayerMagicEventLog)Event;
                                                    switch (playerMagicEventLog.FMode)
                                                    {
                                                        case TPlayerMagicEventMode.pmemSpellResearched:
                                                            SpellBook.ShowResearchedEvent((TPlayerMagicEventLog)Event, Done);
                                                            break;
                                                        case TPlayerMagicEventMode.pmemOutOfMana:
                                                            aowMapEvents.ShowMagicEvent(MagicWin, (TPlayerMagicEventLog)Event, Done);
                                                            break;
                                                        case TPlayerMagicEventMode.pmemStartResearch:
                                                            SpellBook.ShowResearchEvent((TPlayerMagicEventLog)Event, Done);
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Event is THeroUpgradeEventLog)
                                                    {
                                                        aowMapEvents.ShowHeroUpgrade(HeroUpgradeDlg, (THeroUpgradeEventLog)Event, Done);
                                                    }
                                                    else
                                                    {
                                                        if (Event is THeroJoinEventLog)
                                                        {
                                                            aowMapEvents.ShowHeroJoin(HeroJoinDlg, (THeroJoinEventLog)Event, Done);
                                                        }
                                                        else
                                                        {
                                                            if (Event is TArmyInfoEventLog)
                                                            {
                                                                var armyInfoEventLog = (TArmyInfoEventLog)Event;
                                                                var call__15 = armyInfoEventLog.FArmyView.Hero(armyInfoEventLog.FSelection, AllHeroTypes);
                                                                if (call__15 == null)
                                                                {
                                                                    aowMapEvents.ShowArmyInfo(ArmyInfoDlg, (TArmyInfoEventLog)Event, Done);
                                                                }
                                                                else
                                                                {
                                                                    ShowHeroInfo(HeroInfoDlg, (TArmyInfoEventLog)Event, Done);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Event is TPrisonersJoinEventLog)
                                                                {
                                                                    var call__16 = AoWHSMap.FUnitControl.FindUnit(((TPrisonersJoinEventLog)Event).FFirstUnitID);
                                                                    UnitObject = call__16;
                                                                    if (UnitObject != null && UnitObject is THero)
                                                                    {
                                                                        aowMapEvents.ShowPrisonerHero(HeroJoinDlg, (TPrisonersJoinEventLog)Event, Done);
                                                                    }
                                                                    else
                                                                    {
                                                                        aowMapEvents.ShowPrisonersJoin(PartyJoinDlg, (TPrisonersJoinEventLog)Event, Done);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Event is TEnterWizardsTowerEventLog)
                                                                    {
                                                                        aowMapEvents.ShowTower(WizardsTowerDlg, (TEnterWizardsTowerEventLog)Event, Done);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Event is TEnterArenaEventLog)
                                                                        {
                                                                            ShowArena(ArenaDlg, (TEnterArenaEventLog)Event, Done);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Event is TMessageEventLog)
                                                                            {
                                                                                aowMapEvents.ShowMessageEvent(MessageDlg, (TMessageEventLog)Event, Done);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Event is TTutorialMessageEventLog)
                                                                                {
                                                                                    aowMapEvents.ShowTutorialEvent(E2Globals.E2Globals.TutorialWindow, (TTutorialMessageEventLog)Event, Mode, Done);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Event is TFlagMessageEventLog)
                                                                                    {
                                                                                        aowMapEvents.ShowFlagMessage(E2Globals.E2Globals.TutorialWindow, (TFlagMessageEventLog)Event, Mode, Done);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (Event is THelpMessageEventLog)
                                                                                        {
                                                                                            HelpWindow.ShowHelpEvent((THelpMessageEventLog)Event, Mode, Done);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (Event is TSaveGameEventLog)
                                                                                            {
                                                                                                aowMapEvents.ShowSaveGameEvent(FileDlgWindow, (TSaveGameEventLog)Event, Done);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (Event is TSendEmailGameEventLog)
                                                                                                {
                                                                                                    SendEmailGameDlg.ShowSendDialog((TSendEmailGameEventLog)Event, Done);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (Event is TSMTPEventLog)
                                                                                                    {
                                                                                                        ShowSMTPEvent(SMTPDlg, (TSMTPEventLog)Event, Done);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (Event is TPasswordEventLog)
                                                                                                        {
                                                                                                            aowMapEvents.ShowPasswordEvent(PasswordDlg, (TPasswordEventLog)Event, Done);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (Event is TItemExplorationSiteEventLog)
                                                                                                            {
                                                                                                                aowMapEvents.ShowItemsFound(ItemsFoundDlg, (TItemExplorationSiteEventLog)Event, Done);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (Event is TCityProductionEventLog)
                                                                                                                {
                                                                                                                    CityScreen.ProductionEvent((TCityProductionEventLog)Event);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (Event is TProductionPlaceEventLog)
                                                                                                                    {
                                                                                                                        ProductionScreen.ProductionEvent((TProductionPlaceEventLog)Event);
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        if (Event is TUserEventLog)
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
            if (AoWHSMap.FSeatedPlayer == Player)
            {
                Update = true;
            }
            else
            {
                if (AoWHSMap.FCombatMap != null)
                {
                    if (AoWHSMap.FCombatMap.FSeatedPlayer == Player)
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
                    var call__17 = AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__17.FPlayerType != TPlayerType.ptHuman)
                    {
                        Update = false;
                    }
                }

                if (!Update)
                {
                    // { The original clears both income signs when the gold display is cleared. }
                    TBWindow.WGTLbl.Text = (AnsiString)"";
                    TBWindow.WGSign.Text = (AnsiString)"";
                    TBWindow.WMTSign.Text = (AnsiString)"";
                }
                else
                {
                    var call__18 = AoWHSMap.FPlayers.GetPlayers(Player);
                    TBWindow.WGTLbl.Text = IntToStr(call__18.FGems);
                    if (RealmWin.Dlg.Visible && RealmWin.HeaderTab.Selected == 0)
                    {
                        RealmWin.UpdateOverview();
                    }
                }

                TBWindow.TGTLbl.Text = TBWindow.WGTLbl.Text;
                TBWindow.TGSign.Text = TBWindow.WGSign.Text;
                TBWindow.TMTSign.Text = TBWindow.WMTSign.Text;
            }
        }

        // aowMapEvents.pas:320
        public void TheMapGlobalMagicChanged()
        {
            GlobalMagicChanged(MagicWin);
        }

        // aowMapEvents.pas:323
        public void TheMapIncomeChanged(sbyte Player)
        {
            bool Update = default;
            int NetIncome = default;
            Update = false;
            if (AoWHSMap.FSeatedPlayer == Player)
            {
                Update = true;
            }
            else
            {
                if (AoWHSMap.FCombatMap != null)
                {
                    if (AoWHSMap.FCombatMap.FSeatedPlayer == Player)
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
                    var call__19 = AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__19.FPlayerType != TPlayerType.ptHuman)
                    {
                        Update = false;
                    }
                }

                if (!Update)
                {
                    TBWindow.WGILbl.Text = (AnsiString)"";
                }
                else
                {
                    var call__20 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__21 = call__20.GetUpkeep();
                    var call__22 = AoWHSMap.FPlayers.GetPlayers(Player);
                    NetIncome = call__22.FIncome - call__21;
                    if (NetIncome >= 0)
                    {
                        TBWindow.WGSign.Text = AnsiString.FromBytes(43);
                        TBWindow.WGSign.TextColor = 7864183;
                        TBWindow.WGILbl.TextColor = 7864183;
                    }
                    else
                    {
                        TBWindow.WGSign.Text = AnsiString.FromBytes(45);
                        TBWindow.WGSign.TextColor = 119;
                        TBWindow.WGILbl.TextColor = 119;
                    }

                    TBWindow.WGILbl.Text = IntToStr(Math.Abs(NetIncome));
                    if (RealmWin.Dlg.Visible && RealmWin.HeaderTab.Selected == 0)
                    {
                        RealmWin.UpdateOverview();
                    }
                }

                TBWindow.TGILbl.Text = TBWindow.WGILbl.Text;
                TBWindow.TGSign.Text = TBWindow.WGSign.Text;
                TBWindow.TGSign.TextColor = TBWindow.WGSign.TextColor;
                TBWindow.TGILbl.TextColor = TBWindow.WGILbl.TextColor;
            }
        }

        // aowMapEvents.pas:362
        public void TheMapMapChanged(ref TMapChangedInfo Info)
        {
            if (Info.MapLevel == AoWSWindow.AoWSWindow.SWindow.Scanner.FLevel)
            {
                AoWSWindow.AoWSWindow.SWindow.Scanner.DrawMap(Info.Bounds.Left - 6, Info.Bounds.Top - 6, Info.Bounds.Right - Info.Bounds.Left, Info.Bounds.Bottom - Info.Bounds.Top);
            }
        }

        // aowMapEvents.pas:370
        public void TheMapScannerIconsChanged(ref TMapChangedInfo Info)
        {
            if (Info.MapLevel == AoWSWindow.AoWSWindow.SWindow.Scanner.FLevel)
            {
                AoWSWindow.AoWSWindow.SWindow.Scanner.SetObDrawn(false);
            }
        }

        // aowMapEvents.pas:375
        public void TheMapPlayerMagicChanged(sbyte Player)
        {
            bool Update = default;
            int NetIncome = default;
            MagicPlayerChanged(MagicWin, Player);
            SpellBook.PlayerChanged(Player);
            Update = false;
            if (AoWHSMap.FSeatedPlayer == Player)
            {
                Update = true;
            }
            else
            {
                if (AoWHSMap.FCombatMap != null)
                {
                    if (AoWHSMap.FCombatMap.FSeatedPlayer == Player)
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
                    var call__23 = AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__23.FPlayerType != TPlayerType.ptHuman)
                    {
                        Update = false;
                    }
                }

                if (!Update)
                {
                    TBWindow.WMTLbl.Text = (AnsiString)"";
                    TBWindow.WMILbl.Text = (AnsiString)"";
                }
                else
                {
                    var call__24 = AoWHSMap.FPlayers.GetPlayers(Player);
                    TBWindow.WMTLbl.Text = IntToStr(call__24.FMagicControl.FMana);
                    var call__25 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__26 = call__25.FMagicControl.GetManaIncome();
                    var call__27 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__28 = call__27.FMagicControl.GetManaUpkeep();
                    NetIncome = call__26 - call__28;
                    if (NetIncome >= 0)
                    {
                        TBWindow.WMTSign.Text = AnsiString.FromBytes(43);
                        TBWindow.WMTSign.TextColor = 7864183;
                        TBWindow.WMILbl.TextColor = 7864183;
                    }
                    else
                    {
                        TBWindow.WMTSign.Text = AnsiString.FromBytes(45);
                        TBWindow.WMTSign.TextColor = 119;
                        TBWindow.WMILbl.TextColor = 119;
                    }

                    TBWindow.WMILbl.Text = IntToStr(Math.Abs(NetIncome));
                }

                TBWindow.TMTLbl.Text = TBWindow.WMTLbl.Text;
                TBWindow.TMILbl.Text = TBWindow.WMILbl.Text;
                TBWindow.TMTSign.Text = TBWindow.WMTSign.Text;
                TBWindow.TMTSign.TextColor = TBWindow.WMTSign.TextColor;
                TBWindow.TMILbl.TextColor = TBWindow.WMILbl.TextColor;
            }
        }

        // aowMapEvents.pas:1021
        public void TheMapLogbookChanged(sbyte Player)
        {
            if (Player == AoWHSMap.FSeatedPlayer)
            {
                PInfoW.UpdateEvents();
            }
        }

        // aowMapEvents.pas:1026
        public void TheMapPlayerTerminated(sbyte Player)
        {
            /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowMapEvents.pas:409
        public void TheMapSeatedPlayerChanged(sbyte Player)
        {
            if (Player == -1)
            {
                General.HideWindows();
                General.Manager.OverlayPriority = 0;
                TBWindow.CurP.Visible = false;
                TBWindow.FWorldClockDelay = 0;
                TBWindow.TurnButton.FCurPlayer = 12;
                TBWindow.TurnButton.FLastPlayer = TBWindow.TurnButton.FCurPlayer;
                TBWindow.TurnButton.FAnimationFrame = TBWindow.TurnButton.FCurPlayer * 7;
            }
            else
            {
                General.ShowWindows();
                var aowTurnButton = TBWindow.TurnButton;
                if (AoWHSMap.Status.Contains(THSMapStatusElement.msActivated) && !aowTurnButton.FAnimating)
                {
                    if (Player > -1)
                    {
                        var call__30 = AoWHSMap.FPlayers.GetPlayers(Player);
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

                TBWindow.CurP.Visible = AoWHSMap.FPlayMode == TPlayMode.pmTurn;
                PInfoW.UpdatePlayer();
                if (RealmWin.Dlg.Visible)
                {
                    RealmWin.UpdateOverview();
                }
            }

            TheMapGemsChanged(Player);
            TheMapIncomeChanged(Player);
            TheMapPlayerMagicChanged(Player);
            TBWindow.PInd.FDrawn = false;
            PInfoW.UpdateEvents();
        }

        // aowMapEvents.pas:440
        public void TheMapShowMessage(ref TShowMessageInfo Info)
        {
            TBWindow.TBMessEdit.TextColor = Info.Color;
            TBWindow.TBMessEdit.Text = Info.Text;
        }

        // aowMapEvents.pas:446
        public void TheMapStructureChanged(TStructure Structure)
        {
            UpdateStructureInfo(IWindow, Structure);
            if (Structure != null)
            {
                CityScreen.CityChanged(Structure);
                ProductionScreen.PlaceChanged(Structure);
            }

            if (RealmWin.Dlg.Visible && RealmWin.HeaderTab.Selected < 3)
            {
                RealmWin.UpdateOverview();
            }
        }

        // aowMapEvents.pas:457
        public void TheMapStructureDeselected(TStructure Structure)
        {
            if (General.FSign != null)
            {
                General.FSign.ChangeText(AoWHSMap.FSeatedPlayer, IWindow.SignText.Items);
            }

            General.FStructure = null;
            General.FCity = null;
            General.FProductionPlace = null;
            General.FProductionControl = null;
            General.FSign = null;
            IWindow.StructNameLbl.Text = (AnsiString)"";
            IWindow.StructLine.Visible = false;
            IWindow.InfoWin.Visible = false;
            TBWindow.StructButton.Visible = false;
        }

        // aowMapEvents.pas:467
        public void TheMapStructureDestroyed(TStructure Structure)
        {
            if (Structure != null && Structure == General.FStructure)
            {
                TheMapStructureDeselected(Structure);
            }

            if (RealmWin.Dlg.Visible)
            {
                if (RealmWin.HeaderTab.Selected == 2 || RealmWin.HeaderTab.Selected == 1)
                {
                    RealmWin.UpdateOverview();
                }
            }
        }

        // aowMapEvents.pas:473
        public void TheMapStructureSelected(TStructure Structure)
        {
            General.FPreviousStructure = General.FStructure;
            General.FStructure = Structure;
            General.FProductionControl = null;
            ShowStructureInfo(IWindow, Structure);
        }

        // aowMapEvents.pas:481
        public void TheMapItemsDeselected()
        {
            HideItemSelection(IWindow);
        }

        // aowMapEvents.pas:484
        public void TheMapItemsSelected(TItemHS Items)
        {
            ShowItemSelection(IWindow, Items);
        }

        // aowMapEvents.pas:487
        public void TheMapTargetSelectionChange(TTargetSelectionControl Control, TTargetSelectionChange Change)
        {
            aowMapEvents.ShowTargetSelection(AoWWorldMap.AoWWorldMap.TargetSelectionDlg, Control, Change);
        }

        // aowMapEvents.pas:490
        public void TheMapUnitChanged(TAbstractUnit U)
        {
            if (U == General.FSelectedUnit)
            {
                if (UnitWindow.UnitWin.Visible)
                {
                    UnitWindow.ShowUnit();
                }

                if (UnitBanner.BannerPopup.Visible)
                {
                    UnitBanner.BannerPopupShow(this);
                }
            }

            SpellBook.UnitChanged(U);
        }

        // aowMapEvents.pas:500
        public void TheMapCastSpell(TSpellCaster Caster)
        {
            if (Caster is TUnitSpellCaster)
            {
                aowMapEvents.ShowUnitSpellCaster(CastUnitSpellDlg, (TUnitSpellCaster)Caster);
            }
            else
            {
                if (Caster is TDisjunctionSpellCaster)
                {
                    AoWExeForms.AoWExeForms.DisjunctionDlg.Execute((TDisjunctionSpellCaster)Caster);
                }
                else
                {
                    if (Caster is TCosmeticSurgerySpellCaster)
                    {
                        DeformFaceDlg.Execute((TCosmeticSurgerySpellCaster)Caster);
                    }
                }
            }
        }

        // aowMapEvents.pas:507
        public void TheMapHSChanged(THexagonSprite HS)
        {
            if (HS is TItemHS)
            {
                UpdateItemSelection(IWindow, (TItemHS)HS);
            }
        }

        // aowMapEvents.pas:512
        public void TheMapRelationsChanged()
        {
            aowMapEvents.RefreshRelations(RelWindow);
        }

        // aowMapEvents.pas:515
        public void TheMapDiplomaticChange()
        {
            aowMapEvents.RefreshRelations(RelWindow);
        }

        // aowMapEvents.pas:518
        public void TheMapStructureDblClick(TStructure Structure)
        {
            if (Structure == General.FStructure)
            {
                if (General.FStructure is TCity && ((TCity)General.FStructure).FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    CityScreen.ShowCity((TCity)General.FStructure);
                }
                else
                {
                    bool bool__31 = General.FStructure is TProductionPlace;
                    if (bool__31)
                    {
                        var call__32 = ((TProductionPlace)General.FStructure).CanDblClick();
                        bool__31 = call__32;
                    }

                    if (bool__31 && ((TPlayerStructure)General.FStructure).FPlayer == AoWHSMap.FSeatedPlayer)
                    {
                        ProductionScreen.ShowPlace((TProductionPlace)General.FStructure);
                    }
                    else
                    {
                        if (General.FStructure is TWizardsTower)
                        {
                            ((TWizardsTower)General.FStructure).Enter();
                        }
                        else
                        {
                            if (General.FStructure is TArena)
                            {
                                ((TArena)General.FStructure).Enter();
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
        public static void ShowItemsFound(TItemsFoundDlg Dialog, TItemExplorationSiteEventLog Event, TExecuteDoneEvent Done)
        {
            TItemExplorationSiteEventLog Log = default;
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

            AoWHSMap.FTutorialControl.ShowMessage(54, true, true);
            Dialog.Dlg.FDrawn = false;
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:975
        public static void ShowCityNegotiation(TCityJoinDlg Dialog, TNegotiateRequestEventLog Event, TExecuteDoneEvent Done)
        {
            TNegotiateRequestEventLog Log = default;
            TCity Town = default;
            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Log = Dialog.FEvent;
            var call__36 = Log.FJoinMessage.Text;
            Dialog.Memo.Items.Text = call__36;
            Dialog.ApproachLbl.Text = Log.FText;
            var call__37 = AoWHSMap.MapContainer.GetField(Log.FX, Log.FY, Log.FL);
            var call__38 = call__37.FindNoneTransparentHS(typeof(TCity));
            Town = (TCity)call__38;
            Dialog.FRace = Town.FRace;
            var call__39 = Town.GetTopCityActionImage(TCityAction.caNone);
            Dialog.CityPicLine.ImageTop = call__39;
            var call__40 = Town.GetMiddleCityActionImage(TCityAction.caNone);
            Dialog.CityPicLine.ImageMiddle = call__40;
            var call__41 = Town.GetBottomCityActionImage(TCityAction.caNone);
            Dialog.CityPicLine.ImageBottom = call__41;
            Dialog.Dlg.Visible = true;
            AoWHSMap.FTutorialControl.ShowMessage(62, true, true);
        }

        // aowMapEvents.pas:942
        public static void ShowTargetSelection(TTargetSelectionDlg Dialog, TTargetSelectionControl Control, TTargetSelectionChange Change)
        {
            if (Change == TTargetSelectionChange.tscActivated)
            {
                Dialog.HeaderLbl.Text = Control.FTitle;
                Dialog.MainBtn.Text = Control.FCancelText;
                Dialog.DefaultMemo.Items.Assign(Control.FText);
                Dialog.Dlg.Visible = true;
                if (!Control.FTargetMode.Contains(TTargetSelectionModeElement.tmValidateDistance))
                {
                    var call__43 = TranslateRStr(UnlimitedRStr);
                    Dialog.TargetLbl.Text = LowerCase(call__43);
                }
                else
                {
                    Dialog.TargetLbl.Text = IntToStr(Control.FRange);
                }

                Dialog.BorderImage.ImageTop = Control.FSourceImage;
                Dialog.BorderImage.ImageMiddle = Control.FValidTargetImage;
                Dialog.BorderImage.ImageBottom = Control.FInvalidTargetImage;
                General.Manager.OverlayPriority = 49;
                var window = AOWMWindow.AOWMWindow.MWindow.MapWindow;
                window.FModal = true;
                window.Priority = 500;

                AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Priority = Dialog.Dlg.Priority;
            }
            else
            {
                if (Change == TTargetSelectionChange.tscDeactivated)
                {
                    Dialog.Dlg.Visible = false;
                    AOWMWindow.AOWMWindow.MWindow.MapWindow.Priority = 0;
                    AOWMWindow.AOWMWindow.MWindow.MapWindow.FModal = false;
                    AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Priority = 10;
                    General.Manager.OverlayPriority = 0;
                }
            }
        }

        // aowMapEvents.pas:909
        public static void ShowUnitSpellCaster(TCastUnitSpellDlg Dialog, TUnitSpellCaster Caster)
        {
            THero Hero = default;
            TSpell Spell = default;
            if (Dialog.FCaster != null)
            {
                Dialog.FCaster.Release();
            }

            Dialog.FCaster = Caster;
            Dialog.FCaster.AddRef();
            var call__46 = AoWHSMap.FUnitControl.FindUnit(Dialog.FCaster.FCasterSourceID);
            Hero = (THero)call__46;
            if (Hero != null)
            {
                var call__47 = Hero.GetName();
                Dialog.CasterNameLbl.Text = call__47;
                var call__48 = Hero.GetCastingPoints();
                var call__49 = TranslateRStr(CastingPointsRStr);
                var call__50 = Hero.GetCastingPointsMax();
                Dialog.CastingPointsLbl.Text = call__49 + (AnsiString)": " + IntToStr(call__48) + AnsiString.FromBytes(47) + IntToStr(call__50);
            }

            var call__51 = AoWHSSet.FSpellControl.GetSpell(Dialog.FCaster.FCasterSpellID);
            Spell = call__51;
            Dialog.SpellNameLbl.Text = Spell.FSpellName;
            // { Original still uses Hero below if the source-unit lookup failed. }
            var call__52 = Hero.CastingMana(Spell);
            var call__53 = TranslateRStr(CastingCostRStr);
            var call__54 = TranslateRStr(ManaRStr);
            Dialog.SpellManaLbl.Text = call__53 + (AnsiString)": " + IntToStr(call__52) + AnsiString.FromBytes(32) + call__54;
            var call__55 = TranslateRStr(UpkeepRStr);
            var call__56 = TranslateRStr(ManaRStr);
            Dialog.SpellUpkeepLbl.Text = call__55 + (AnsiString)": " + IntToStr(Spell.FManaUpkeep) + AnsiString.FromBytes(32) + call__56;
            var call__57 = AoWHSMap.FPlayers.GetPlayers(Hero.FPlayer);
            var call__58 = TranslateRStr(ManaRStr);
            Dialog.CastingManaLbl.Text = call__58 + (AnsiString)": " + IntToStr(call__57.FMagicControl.FMana);
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
        public static void RefreshRelations(TRelWindow Dialog)
        {
            if (Dialog.RelWin.Visible)
            {
                Dialog.FillPlayers();
                Dialog.RelPnl.FAligned = false;
                Dialog.RelWin.FDrawn = false;
                if (Dialog.FSelectedPlayer != -1)
                {
                    Dialog.HighlightPlayer(Dialog.FSelectedPlayer);
                }

                if (Dialog.FInfoPlayer != -1)
                {
                    if (Dialog.RNewActionPnl.Visible)
                    {
                        Dialog.ShowPlayerActions(Dialog.FInfoPlayer);
                    }
                    else
                    {
                        Dialog.ShowPlayerInformation(Dialog.FInfoPlayer);
                    }
                }
                else
                {
                    if (Dialog.FSelectedRace != -1)
                    {
                        Dialog.HighlightRace(Dialog.FSelectedRace);
                    }
                }

                if (Dialog.FInfoRace != -1)
                {
                    Dialog.ShowRaceInformation((byte)Dialog.FInfoRace);
                }
            }
        }

        // aowMapEvents.pas:531
        public static void ShowRebellion(TCityRebelDlg Dialog, TCityRebellionEventLog Event, TExecuteDoneEvent Done)
        {
            Dialog.FOnClose = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.CityNameLabel.Text = Dialog.FEvent.FCityName;
            Dialog.RebelWin.Visible = true;
        }

        // aowMapEvents.pas:543
        public static void ShowReceivedMessage(TMsgReceiveDlg Dialog, TEventLog Event, TExecuteDoneEvent Done)
        {
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Dialog.FDone = Done;
            var call__62 = Event.GetText();
            Dialog.HeaderMemo.Items.Text = call__62;
            var call__63 = ((TGameOverEventLog)Event).FMessage.Text;
            Dialog.MessageMemo.Items.Text = call__63;
            var call__64 = AoWHSMap.FPlayers.GetPlayers(Event.FPlayer);
            Dialog.LShield.CurrentImage = call__64.FPlayerRace + 13;
            Dialog.RShield.CurrentImage = Dialog.LShield.CurrentImage;
            Dialog.MsgReceiveWin.Visible = true;
        }

        // aowMapEvents.pas:558
        public static void ShowTutorialEvent(TTutorialWindow Dialog, TTutorialMessageEventLog Event, bool Unused, TExecuteDoneEvent Done)
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
            bool bool__70 = call__69 > 0;
            if (bool__70)
            {
                var call__71 = Dialog.MessageMemo.Items[0];
                bool__70 = call__71 == (AnsiString)"";
            }

            if (bool__70)
            {
                Dialog.MessageMemo.Items.Delete(0);
            }

            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:577
        public static void ShowFlagMessage(TTutorialWindow Dialog, TFlagMessageEventLog Event, bool Unused, TExecuteDoneEvent Done)
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
        public static void ShowArmyInfo(TArmyInfoDlg Dialog, TArmyInfoEventLog Event, TExecuteDoneEvent Done)
        {
            TArmyInfoEventLog Log = default;
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
        public static void ShowTower(TWizardsTowerDlg Dialog, TEnterWizardsTowerEventLog Event, TExecuteDoneEvent Done)
        {
            TWizardsTower Tower = default;
            TSpell Spell = default;
            Dialog.FOnClose = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Tower = Dialog.FEvent.FTower;
            if (Tower.FSpellID != 0)
            {
                var call__76 = Dialog.FEvent.FTower.BuyCost();
                var call__77 = TranslateRStr(ForXGoldRStr);
                var call__78 = AoWE.AoWE.Format(call__77, [IntToStr(call__76)]);
                Dialog.PriceLbl.Text = call__78 + AnsiString.FromBytes(46);
                var call__79 = AoWHSSet.FSpellControl.GetSpell(Tower.FSpellID);
                Spell = call__79;
                Dialog.SpellName.Text = Spell.FSpellName;
                var call__80 = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var call__81 = TranslateRStr(AoWE.AoWE.LevelRStr);
                Dialog.SpellClass.Text = call__80 + (AnsiString)", " + call__81 + AnsiString.FromBytes(32) + IntToStr(Spell.FSpellLevel);
                var call__82 = TranslateRStr(CastingCostRStr);
                var call__83 = TranslateRStr(ManaRStr);
                Dialog.SpellCost.Text = call__82 + (AnsiString)": " + IntToStr(Spell.FBaseManaCost) + AnsiString.FromBytes(32) + call__83;
                var call__84 = Spell.FSpellText.Text;
                Dialog.ResearchMemo.Items.Text = call__84;
            }
            else
            {
                Dialog.PriceLbl.Text = (AnsiString)"";
                var call__85 = TranslateRStr(AoWE.AoWE.NoneRStr);
                Dialog.SpellName.Text = call__85;
                Dialog.SpellClass.Text = (AnsiString)"";
                Dialog.SpellCost.Text = (AnsiString)"";
                Dialog.ResearchMemo.Items.Text = (AnsiString)"";
            }

            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:647
        public static void ResetPartyJoin(TPartyJoinDlg Dialog)
        {
            Dialog.LeftBtn.Visible = true;
            Dialog.MiddleBtn.Visible = true;
            Dialog.RightBtn.Visible = true;
            var call__87 = TranslateRStr(PartyJoinRStr);
            Dialog.Title.Text = call__87;
        }

        // aowMapEvents.pas:656
        public static void UpdatePartyJoin(TPartyJoinDlg Dialog)
        {
            if (Dialog.ValidateJoin())
            {
                Dialog.FUnits.Clear();
                var call__89 = AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
                Dialog.FFaceUnit = call__89;
                Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
                var call__90 = Dialog.FJoinEvent.FJoinMessage.Text;
                Dialog.JoinMemo.Items.Text = call__90;
            }
        }

        // aowMapEvents.pas:668
        public static void ResetHeroJoin(THeroJoinDlg Dialog)
        {
            Dialog.LeftBtn.Visible = true;
            Dialog.MiddleBtn.Visible = true;
            Dialog.RightBtn.Visible = true;
        }

        // aowMapEvents.pas:676
        public static void UpdateHeroJoin(THeroJoinDlg Dialog)
        {
            if (Dialog.ValidateJoin())
            {
                Dialog.FUnits.Clear();
                var call__93 = AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
                Dialog.FFaceUnit = call__93;
                Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
                var call__94 = ((THero)Dialog.FFaceUnit).GetLevel();
                var call__95 = TranslateRStr(LevelXRStr);
                var call__96 = AoWE.AoWE.Format(call__95, [IntToStr(call__94)]);
                Dialog.HeroLevel.Text = call__96;
                Dialog.HeroName.Text = ((THero)Dialog.FFaceUnit).FHeroName;
                var call__97 = Dialog.FJoinEvent.FJoinMessage.Text;
                Dialog.JoinMemo.Items.Text = call__97;
            }
        }

        // aowMapEvents.pas:690
        public static void ShowPartyJoin(TPartyJoinDlg Dialog, TNegotiateRequestEventLog Event, TExecuteDoneEvent Done)
        {
            ResetPartyJoin(Dialog);
            Dialog.Dlg.Visible = true;
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__99 = TranslateRStr(AcceptRStr);
            Dialog.LeftBtn.Text = call__99;
            var call__100 = TranslateRStr(AttackRStr);
            Dialog.MiddleBtn.Text = call__100;
            var call__101 = TranslateRStr(CancelRStr);
            Dialog.RightBtn.Text = call__101;
            UpdatePartyJoin(Dialog);
            AoWHSMap.FTutorialControl.ShowMessage(70, true, true);
        }

        // aowMapEvents.pas:706
        public static void ShowPrisonersJoin(TPartyJoinDlg Dialog, TPrisonersJoinEventLog Event, TExecuteDoneEvent Done)
        {
            ResetPartyJoin(Dialog);
            Dialog.Dlg.Visible = true;
            var call__103 = TranslateRStr(PrisonerLiberationRStr);
            Dialog.Title.Text = call__103;
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__104 = TranslateRStr(AcceptRStr);
            Dialog.LeftBtn.Text = call__104;
            Dialog.MiddleBtn.Visible = false;
            var call__105 = TranslateRStr(RejectRStr);
            Dialog.RightBtn.Text = call__105;
            UpdatePartyJoin(Dialog);
        }

        // aowMapEvents.pas:722
        public static void ShowHeroJoin(THeroJoinDlg Dialog, THeroJoinEventLog Event, TExecuteDoneEvent Done)
        {
            ResetHeroJoin(Dialog);
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__107 = TranslateRStr(AcceptRStr);
            Dialog.LeftBtn.Text = call__107;
            Dialog.MiddleBtn.Visible = false;
            var call__108 = TranslateRStr(CancelRStr);
            Dialog.RightBtn.Text = call__108;
            UpdateHeroJoin(Dialog);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:736
        public static void ShowHeroNegotiation(THeroJoinDlg Dialog, TNegotiateRequestEventLog Event, TExecuteDoneEvent Done)
        {
            ResetHeroJoin(Dialog);
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__110 = TranslateRStr(AcceptRStr);
            Dialog.LeftBtn.Text = call__110;
            var call__111 = TranslateRStr(AttackRStr);
            Dialog.MiddleBtn.Text = call__111;
            var call__112 = TranslateRStr(CancelRStr);
            Dialog.RightBtn.Text = call__112;
            UpdateHeroJoin(Dialog);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:750
        public static void ShowPrisonerHero(THeroJoinDlg Dialog, TPrisonersJoinEventLog Event, TExecuteDoneEvent Done)
        {
            ResetHeroJoin(Dialog);
            Dialog.FJoinEvent = Event;
            Dialog.FJoinEvent.AddRef();
            Dialog.FDone = Done;
            var call__114 = TranslateRStr(AcceptRStr);
            Dialog.LeftBtn.Text = call__114;
            Dialog.MiddleBtn.Visible = false;
            var call__115 = TranslateRStr(RejectRStr);
            Dialog.RightBtn.Text = call__115;
            UpdateHeroJoin(Dialog);
            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:789
        public static void ShowHeroUpgrade(THeroUpgradeDlg Dialog, THeroUpgradeEventLog Event, TExecuteDoneEvent Completed)
        {
            Dialog.FReferencedHero = Event;
            Dialog.FReferencedHero.AddRef();
            Dialog.FOnClose = Completed;
            if (Dialog.ValidateHero())
            {
                var call__117 = Dialog.FHero.Copy(AoWEngine);
                Dialog.FEditedHero = (THero)call__117;
                Dialog.UpdateStatistics();
                Dialog.RefreshAbilities();
                Dialog.UnitName.Text = Dialog.FHero.FHeroName;
                var call__118 = Dialog.FHero.GetLevel();
                var call__119 = TranslateRStr(AoWE.AoWE.LevelRStr);
                Dialog.LevelLbl.Text = call__119 + AnsiString.FromBytes(32) + IntToStr(call__118);
                if (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel > 1)
                {
                    var call__120 = TranslateRStr(E3Globals.E3Globals.XMadeYLevelsRStr);
                    var call__121 = AoWE.AoWE.Format(call__120, [Dialog.FHero.FHeroName, (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel)]);
                    Dialog.HeaderLbl.Text = call__121 + AnsiString.FromBytes(33);
                }
                else
                {
                    var call__122 = TranslateRStr(E3Globals.E3Globals.XMadeALevelRStr);
                    var call__123 = AoWE.AoWE.Format(call__122, [Dialog.FHero.FHeroName]);
                    Dialog.HeaderLbl.Text = call__123 + AnsiString.FromBytes(33);
                }

                Dialog.Dlg.Visible = true;
            }
        }

        // aowMapEvents.pas:811
        public static void ShowMessageEvent(TMessageDlg Dialog, TMessageEventLog Event, TExecuteDoneEvent Done)
        {
            TMessageEventLog Log = default;
            ResetMessageDialog(Dialog);
            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Log = Dialog.FEvent;
            Dialog.TitleLbl.Text = Log.FTitle;
            switch (Log.FLayout)
            {
                case TMessageEventLogLayout.melNormal:
                    var call__125 = Log.FMessage.Text;
                    Dialog.MessageMemo.Items.Text = call__125;
                    var call__126 = Dialog.MessageMemo.Items.Text;
                    if (call__126.Length < 80)
                    {
                        Dialog.MessageMemo.Items.Insert(0, (AnsiString)"");
                    }

                    break;
                case TMessageEventLogLayout.melHeader:
                    Dialog.MemoPnl.Visible = false;
                    Dialog.HeaderMemoPnl.Visible = true;
                    Dialog.HeaderLbl.Text = Log.FHeader;
                    var call__127 = Log.FMessage.Text;
                    Dialog.HeaderMemo.Items.Text = call__127;
                    break;
            }

            switch (Log.FButtons)
            {
                case TMessageEventLogButtons.mebOk:
                    Dialog.OKPnl.Visible = true;
                    break;
                case TMessageEventLogButtons.mebOkCancel:
                    Dialog.OKCancelPnl.Visible = true;
                    break;
                case TMessageEventLogButtons.mebYesNo:
                    Dialog.YesNoPnl.Visible = true;
                    break;
                case TMessageEventLogButtons.mebYesNoCancel:
                    Dialog.YesNoCancelPnl.Visible = true;
                    break;
            }

            Dialog.Dlg.Visible = true;
        }

        // aowMapEvents.pas:844
        public static void ShowSaveGameEvent(TFileDlgWindow Dialog, TSaveGameEventLog Event, TExecuteDoneEvent Done)
        {
            Dialog.FDone = Done;
            Dialog.FSaveEvent = Event;
            Dialog.FSaveEvent.AddRef();
            Dialog.FMode = 1;
            Dialog.FileDialog.Visible = true;
            if (Event.FDirectory != (AnsiString)"")
            {
                SetCurrentDir(Event.FDirectory);
            }

            Dialog.SFileName.Text = Event.FDefaultName;
            Dialog.Title.Text = Event.FTitle;
            Dialog.FindFiles();
        }

        // aowMapEvents.pas:858
        public static void ShowPasswordEvent(TPasswordDlg Dialog, TPasswordEventLog Event, TExecuteDoneEvent Done)
        {
            TPasswordEventLog Log = default;
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
        public static void ShowMagicWindow(TMagicWin Window)
        {
            if (!Window.Dlg.Visible)
            {
                RefreshMagicWindow(Window);
                RefreshGlobalMagic(Window);
            }

            Window.Dlg.Visible = true;
        }

        // aowMapEvents.pas:885
        public static void ShowMagicEvent(TMagicWin Window, TPlayerMagicEventLog Event, TExecuteDoneEvent Done)
        {
            ShowMagicWindow(Window);
            if (Done != null)
            {
                Done(Event);
            }
        }

        public static TMapEvents MapEvents;
    }
}
