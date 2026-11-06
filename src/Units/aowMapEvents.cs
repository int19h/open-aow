// Translated from aowMapEvents.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.HSEngine;
using Aow.Units.MapCtrl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.EventLog;
using Aow.Units.City;
using Aow.Units.WizardsTower;
using Aow.Units.Arena;
using Aow.Units.FlagEvents;
using Aow.Units.Dungeon;
using Aow.Units.ItemExpS;
using static Aow.Units.AoWArmyEventDlg.AoWArmyEventDlgUnit;
using Aow.Units.AoWCityRebelDlg;
using Aow.Units.AoWMsgReceiveDlg;
using Aow.Units.AoWTutorialWin;
using Aow.Units.AoWArmyInfoDlg;
using Aow.Units.AoWWizardsTowerDlg;
using static Aow.Units.AoWArenaDlg.AoWArenaDlgUnit;
using static Aow.Units.FastCombatWin.FastCombatWinUnit;
using Aow.Units.AoWPartyJoin;
using Aow.Units.AoWHeroJoinDlg;
using static Aow.Units.AoWHeroInfoDlg.AoWHeroInfoDlgUnit;
using Aow.Units.AoWHeroUpgradeDlg;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlgUnit;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialogUnit;
using Aow.Units.AoWPasswordDlg;
using Aow.Units.AoWMagic;
using Aow.Units.AoWRelations;
using Aow.Units.AoWCastUnitSpellDlg;
using Aow.Units.AoWTargetSelection;
using Aow.Units.AoWCityJoinDlg;
using Aow.Units.AoWItemsFoundDlg;
using static Aow.Units.AoWRealm.AoWRealmUnit;
using static Aow.Units.AoWInfoWin.AoWInfoWinUnit;
using static Aow.Units.E3Globals.E3GlobalsUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.AoWUnitBanner.AoWUnitBannerUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.E1Globals.E1GlobalsUnit;
using Aow.Units.PrdPlace;
using static Aow.Units.AoWCityScreen.AoWCityScreenUnit;
using static Aow.Units.AoWProdScreen.AoWProdScreenUnit;
using static Aow.Units.E2CampaignSupport.E2CampaignSupportUnit;
using Aow.Units.GlobalSpells;
using Aow.Units.UnitSpells;
using static Aow.Units.E3EventDialogs.E3EventDialogsUnit;
using static Aow.Units.AoWSMTPDlg.AoWSMTPDlgUnit;
using static Aow.Units.AoWPInfoWin.AoWPInfoWinUnit;
using static Aow.Units.aowHelpWin.aowHelpWinUnit;
using static Aow.Units.AoWUnitWindow.AoWUnitWindowUnit;
using Aow.Platform;
using Aow.Units.AOWControlWin;
using Aow.Units.AOWPyreScrn;
using Aow.Units.AoWWorldMap;
using Aow.Units.E2Globals;
using Aow.Units.AoWSWindow;
using Aow.Units.AoWExeForms;
using Aow.Units.AOWMWindow;
using Aow.Units.aowSetupEvents;
using Aow.Units.E3Globals;

namespace Aow.Units.aowMapEvents;

// map control
public partial class TMapEvents : TDataModule
{
    public TMapEvents(TComponent owner) : base(owner)
    { }

    public TAoWMapControl TheMap;
    // Renderers of the DoubleClick keys of the structures (Structure.Views.cs). ClassRegistry adds
    // the original renderers.
    public static readonly Catalog<StructureView, Action<TMapEvents, TStructure>> StructureScreens = new("map structure screen");
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
        AOWControlWinUnit.ControlWin.MovesLbl.Text = "";
        AOWControlWinUnit.ControlWin.MoveIcon.Visible = false;
        AOWControlWinUnit.ControlWin.Emp1Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp1Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp2Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp2Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp3Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp3Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp4Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp4Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp5Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp5Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp6Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp6Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp7Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp7Chk.Visible = true;
        AOWControlWinUnit.ControlWin.Emp8Fr.Visible = true;
        AOWControlWinUnit.ControlWin.Emp8Chk.Visible = true;
        AOWControlWinUnit.ControlWin.ControlWindow.Visible = false;
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
        int Count;
        Count = 0;
        for (int I = 0; I <= 7; I++)
        {
            General.FSelectedUnits[I] = -1;
        }

        for (int I = 0, last = Army.GetCount() - 1; I <= last; I++)
        {
            if (!Army.GetUnitConcealedForPlayer(I, AoWHSMap.FSeatedPlayer))
            {
                General.FSelectedUnits[Count] = I;
                Count += 1;
            }
        }

        General.FUnitList = Army;
        AOWControlWinUnit.ControlWin.UpdateArmy();
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

        var format = AoWEUnit.Format(TranslateRStr(DayXRStr), [Day]);
        PInfoW.Turn.Text = format;
        aowMapEventsUnit.MapEvents.TheMapGemsChanged(AoWHSMap.FSeatedPlayer);
        aowMapEventsUnit.MapEvents.TheMapIncomeChanged(AoWHSMap.FSeatedPlayer);
        if (RealmWin.Dlg.Visible)
        {
            RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:187
    public void TheMapEndTurn(sbyte Player, bool Cancelled)
    {
        if (AoWHSMap.FSeatedPlayer == Player && AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FGameOverStatus == TGameOverStatus.goNone)
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
                    aowMapEventsUnit.ShowRebellion(CityRebelDlg, (TCityRebellionEventLog)Event, Done);
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
                                aowMapEventsUnit.ShowCityNegotiation(CityJoinDlg, (TNegotiateRequestEventLog)Event, Done);
                            }
                            else
                            {
                                if (negotiateRequestEventLog.FObject == TNegotiateObject.noArmy)
                                {
                                    ArmyHS = (TArmyHS)AoWHSMap.MapContainer.GetField(negotiateRequestEventLog.FX, negotiateRequestEventLog.FY, negotiateRequestEventLog.FL).FindOwnedChild(131607);
                                    if (ArmyHS != null)
                                    {
                                        var joinSelection = ArmyHS.FArmy.GetJoinSelection(negotiateRequestEventLog.FPlayer);
                                        if (ArmyHS.FArmy.Hero(joinSelection, AllHeroTypes) == null)
                                        {
                                            aowMapEventsUnit.ShowPartyJoin(PartyJoinDlg, (TNegotiateRequestEventLog)Event, Done);
                                        }
                                        else
                                        {
                                            aowMapEventsUnit.ShowHeroNegotiation(HeroJoinDlg, (TNegotiateRequestEventLog)Event, Done);
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
                                    AOWPyreScrnUnit.PyreScreen.ShowGameOver((TGameOverEventLog)Event, Done);
                                }
                                else
                                {
                                    aowMapEventsUnit.ShowReceivedMessage(MsgReceiveDlg, Event, Done);
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
                                            ShowInformationBook(AoWWorldMapUnit.WorldMap, (TShowBookEventLog)Event, Done);
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
                                                        aowMapEventsUnit.ShowMagicEvent(MagicWin, (TPlayerMagicEventLog)Event, Done);
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
                                                    aowMapEventsUnit.ShowHeroUpgrade(HeroUpgradeDlg, (THeroUpgradeEventLog)Event, Done);
                                                }
                                                else
                                                {
                                                    if (Event is THeroJoinEventLog)
                                                    {
                                                        aowMapEventsUnit.ShowHeroJoin(HeroJoinDlg, (THeroJoinEventLog)Event, Done);
                                                    }
                                                    else
                                                    {
                                                        if (Event is TArmyInfoEventLog)
                                                        {
                                                            var armyInfoEventLog = (TArmyInfoEventLog)Event;
                                                            if (armyInfoEventLog.FArmyView.Hero(armyInfoEventLog.FSelection, AllHeroTypes) == null)
                                                            {
                                                                aowMapEventsUnit.ShowArmyInfo(ArmyInfoDlg, (TArmyInfoEventLog)Event, Done);
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
                                                                UnitObject = AoWHSMap.FUnitControl.FindUnit(((TPrisonersJoinEventLog)Event).FFirstUnitID);
                                                                if (UnitObject != null && UnitObject is THero)
                                                                {
                                                                    aowMapEventsUnit.ShowPrisonerHero(HeroJoinDlg, (TPrisonersJoinEventLog)Event, Done);
                                                                }
                                                                else
                                                                {
                                                                    aowMapEventsUnit.ShowPrisonersJoin(PartyJoinDlg, (TPrisonersJoinEventLog)Event, Done);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Event is TEnterWizardsTowerEventLog)
                                                                {
                                                                    aowMapEventsUnit.ShowTower(WizardsTowerDlg, (TEnterWizardsTowerEventLog)Event, Done);
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
                                                                            aowMapEventsUnit.ShowMessageEvent(MessageDlg, (TMessageEventLog)Event, Done);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Event is TTutorialMessageEventLog)
                                                                            {
                                                                                aowMapEventsUnit.ShowTutorialEvent(E2GlobalsUnit.TutorialWindow, (TTutorialMessageEventLog)Event, Mode, Done);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Event is TFlagMessageEventLog)
                                                                                {
                                                                                    aowMapEventsUnit.ShowFlagMessage(E2GlobalsUnit.TutorialWindow, (TFlagMessageEventLog)Event, Mode, Done);
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
                                                                                            aowMapEventsUnit.ShowSaveGameEvent(FileDlgWindow, (TSaveGameEventLog)Event, Done);
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
                                                                                                        aowMapEventsUnit.ShowPasswordEvent(PasswordDlg, (TPasswordEventLog)Event, Done);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (Event is TItemExplorationSiteEventLog)
                                                                                                        {
                                                                                                            aowMapEventsUnit.ShowItemsFound(ItemsFoundDlg, (TItemExplorationSiteEventLog)Event, Done);
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
        bool Update;
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
                if (AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType != TPlayerType.ptHuman)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                // { The original clears both income signs when the gold display is cleared. }
                TBWindow.WGTLbl.Text = "";
                TBWindow.WGSign.Text = "";
                TBWindow.WMTSign.Text = "";
            }
            else
            {
                var player = AoWHSMap.FPlayers.GetPlayers(Player);
                TBWindow.WGTLbl.Text = IntToStr(player.FGems);
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
        bool Update;
        int NetIncome;
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
                if (AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType != TPlayerType.ptHuman)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                TBWindow.WGILbl.Text = "";
            }
            else
            {
                var upkeep = AoWHSMap.FPlayers.GetPlayers(Player).GetUpkeep();
                NetIncome = AoWHSMap.FPlayers.GetPlayers(Player).FIncome - upkeep;
                if (NetIncome >= 0)
                {
                    TBWindow.WGSign.Text = "+";
                    TBWindow.WGSign.TextColor = 7864183;
                    TBWindow.WGILbl.TextColor = 7864183;
                }
                else
                {
                    TBWindow.WGSign.Text = "-";
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
        if (Info.MapLevel == AoWSWindowUnit.SWindow.Scanner.FLevel)
        {
            AoWSWindowUnit.SWindow.Scanner.DrawMap(Info.Bounds.Left - 6, Info.Bounds.Top - 6, Info.Bounds.Right - Info.Bounds.Left, Info.Bounds.Bottom - Info.Bounds.Top);
        }
    }

    // aowMapEvents.pas:370
    public void TheMapScannerIconsChanged(ref TMapChangedInfo Info)
    {
        if (Info.MapLevel == AoWSWindowUnit.SWindow.Scanner.FLevel)
        {
            AoWSWindowUnit.SWindow.Scanner.SetObDrawn(false);
        }
    }

    // aowMapEvents.pas:375
    public void TheMapPlayerMagicChanged(sbyte Player)
    {
        bool Update;
        int NetIncome;
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
                if (AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType != TPlayerType.ptHuman)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                TBWindow.WMTLbl.Text = "";
                TBWindow.WMILbl.Text = "";
            }
            else
            {
                var player = AoWHSMap.FPlayers.GetPlayers(Player);
                TBWindow.WMTLbl.Text = IntToStr(player.FMagicControl.FMana);
                NetIncome = AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.GetManaIncome() - AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.GetManaUpkeep();
                if (NetIncome >= 0)
                {
                    TBWindow.WMTSign.Text = "+";
                    TBWindow.WMTSign.TextColor = 7864183;
                    TBWindow.WMILbl.TextColor = 7864183;
                }
                else
                {
                    TBWindow.WMTSign.Text = "-";
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
                    aowTurnButton.FCurPlayer = AoWHSMap.FPlayers.GetPlayers(Player).FPlayerRace;
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
        IWindow.StructNameLbl.Text = "";
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
        aowMapEventsUnit.ShowTargetSelection(AoWWorldMapUnit.TargetSelectionDlg, Control, Change);
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
            aowMapEventsUnit.ShowUnitSpellCaster(CastUnitSpellDlg, (TUnitSpellCaster)Caster);
        }
        else
        {
            if (Caster is TDisjunctionSpellCaster)
            {
                AoWExeFormsUnit.DisjunctionDlg.Execute((TDisjunctionSpellCaster)Caster);
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
        aowMapEventsUnit.RefreshRelations(RelWindow);
    }

    // aowMapEvents.pas:515
    public void TheMapDiplomaticChange()
    {
        aowMapEventsUnit.RefreshRelations(RelWindow);
    }

    // aowMapEvents.pas:518
    public void TheMapStructureDblClick(TStructure Structure)
    {
        if (Structure == General.FStructure && Structure != null)
        {
            if (StructureScreens.TryGet(Structure.DoubleClick(AoWHSMap.FSeatedPlayer), out var open))
            {
                open(this, Structure);
            }
        }
    }

    // aowMapEvents.pas:176. The offline transport has no connection to release.
    public void TheMapDeactivateHSM()
    {
        try
        {
            General.SaveLayout();
            General.HideWindows();
            AOWMWindowUnit.MWindow.MapViewer.HSMap = null;
            if (aowSetupEventsUnit.SetupEvents.FLobbyGame)
                GameHost.MainForm.CloseRequested();
        }
        catch
        {
            ShowMessage("Error during close");
        }
    }
}

public static partial class aowMapEventsUnit
{
    // aowMapEvents.pas:998
    public static void ShowItemsFound(TItemsFoundDlg Dialog, TItemExplorationSiteEventLog Event, TExecuteDoneEvent Done)
    {
        TItemExplorationSiteEventLog Log;
        Dialog.Reset();
        Dialog.FOnDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        var text = Log.FMessages.Text;
        Dialog.MessageMemo.Items.Text = text;
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
        TNegotiateRequestEventLog Log;
        TCity Town;
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        var text = Log.FJoinMessage.Text;
        Dialog.Memo.Items.Text = text;
        Dialog.ApproachLbl.Text = Log.FText;
        Town = (TCity)AoWHSMap.MapContainer.GetField(Log.FX, Log.FY, Log.FL).FindNoneTransparentHS(typeof(TCity));
        Dialog.FRace = Town.FRace;
        var topCityActionImage = Town.GetTopCityActionImage(TCityAction.caNone);
        Dialog.CityPicLine.ImageTop = topCityActionImage;
        var middleCityActionImage = Town.GetMiddleCityActionImage(TCityAction.caNone);
        Dialog.CityPicLine.ImageMiddle = middleCityActionImage;
        var bottomCityActionImage = Town.GetBottomCityActionImage(TCityAction.caNone);
        Dialog.CityPicLine.ImageBottom = bottomCityActionImage;
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
                var unlimited = TranslateRStr(UnlimitedRStr);
                Dialog.TargetLbl.Text = LowerCase(unlimited);
            }
            else
            {
                Dialog.TargetLbl.Text = IntToStr(Control.FRange);
            }

            Dialog.BorderImage.ImageTop = Control.FSourceImage;
            Dialog.BorderImage.ImageMiddle = Control.FValidTargetImage;
            Dialog.BorderImage.ImageBottom = Control.FInvalidTargetImage;
            General.Manager.OverlayPriority = 49;
            var window = AOWMWindowUnit.MWindow.MapWindow;
            window.FModal = true;
            window.Priority = 500;

            AoWSWindowUnit.SWindow.ScannerWindow.Priority = Dialog.Dlg.Priority;
        }
        else
        {
            if (Change == TTargetSelectionChange.tscDeactivated)
            {
                Dialog.Dlg.Visible = false;
                AOWMWindowUnit.MWindow.MapWindow.Priority = 0;
                AOWMWindowUnit.MWindow.MapWindow.FModal = false;
                AoWSWindowUnit.SWindow.ScannerWindow.Priority = 10;
                General.Manager.OverlayPriority = 0;
            }
        }
    }

    // aowMapEvents.pas:909
    public static void ShowUnitSpellCaster(TCastUnitSpellDlg Dialog, TUnitSpellCaster Caster)
    {
        THero Hero;
        TSpell Spell;
        if (Dialog.FCaster != null)
        {
            Dialog.FCaster.Release();
        }

        Dialog.FCaster = Caster;
        Dialog.FCaster.AddRef();
        Hero = (THero)AoWHSMap.FUnitControl.FindUnit(Dialog.FCaster.FCasterSourceID);
        if (Hero != null)
        {
            var name = Hero.GetName();
            Dialog.CasterNameLbl.Text = name;
            var castingPoints = Hero.GetCastingPoints();
            var castingPointsText = TranslateRStr(CastingPointsRStr);
            var castingPointsMax = Hero.GetCastingPointsMax();
            Dialog.CastingPointsLbl.Text = castingPointsText + ": " + IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
        }

        Spell = AoWHSSet.FSpellControl.GetSpell(Dialog.FCaster.FCasterSpellID);
        Dialog.SpellNameLbl.Text = Spell.FSpellName;
        // { Original still uses Hero below if the source-unit lookup failed. }
        var castingMana = Hero.CastingMana(Spell);
        var castingCost = TranslateRStr(CastingCostRStr);
        var mana = TranslateRStr(ManaRStr);
        Dialog.SpellManaLbl.Text = castingCost + ": " + IntToStr(castingMana) + " " + mana;
        var upkeep = TranslateRStr(UpkeepRStr);
        var manaText = TranslateRStr(ManaRStr);
        Dialog.SpellUpkeepLbl.Text = upkeep + ": " + IntToStr(Spell.FManaUpkeep) + " " + manaText;
        var player = AoWHSMap.FPlayers.GetPlayers(Hero.FPlayer);
        var mana3 = TranslateRStr(ManaRStr);
        Dialog.CastingManaLbl.Text = mana3 + ": " + IntToStr(player.FMagicControl.FMana);
        Dialog.U1.Checked = false;
        Dialog.U2.Checked = false;
        Dialog.U3.Checked = false;
        Dialog.U4.Checked = false;
        Dialog.U5.Checked = false;
        Dialog.U6.Checked = false;
        Dialog.U7.Checked = false;
        Dialog.U8.Checked = false;
        Dialog.SelectionInfoLbl.Text = "";
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
        var text = Event.GetText();
        Dialog.HeaderMemo.Items.Text = text;
        var fMessageText = ((TGameOverEventLog)Event).FMessage.Text;
        Dialog.MessageMemo.Items.Text = fMessageText;
        var player = AoWHSMap.FPlayers.GetPlayers(Event.FPlayer);
        Dialog.LShield.CurrentImage = player.FPlayerRace + 13;
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
        if (Event.FMessage.Count > 0)
        {
            var message = Event.FMessage[0];
            Dialog.HeaderMemo.Items.Add(message);
        }

        Dialog.MessageMemo.Items.Clear();
        Dialog.MessageMemo.Items.AddStrings(Event.FMessage);
        if (Dialog.MessageMemo.Items.Count > 0)
        {
            Dialog.MessageMemo.Items.Delete(0);
        }

        if (Dialog.MessageMemo.Items.Count > 0 && IsEmpty(Dialog.MessageMemo.Items[0]))
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
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        var text = Event.FMessages.Text;
        Dialog.Message.Items.Text = text;
        Dialog.Title.Text = Event.FTitle;
        Dialog.HeaderLbl.Text = Event.FDescription;
        Dialog.Dlg.Visible = true;
        Dialog.FUnitInfoShown = false;
    }

    // aowMapEvents.pas:611
    public static void ShowTower(TWizardsTowerDlg Dialog, TEnterWizardsTowerEventLog Event, TExecuteDoneEvent Done)
    {
        TWizardsTower Tower;
        TSpell Spell;
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Tower = Dialog.FEvent.FTower;
        if (Tower.FSpellID != 0)
        {
            var buyCost = Dialog.FEvent.FTower.BuyCost();
            var format = AoWEUnit.Format(TranslateRStr(ForXGoldRStr), [IntToStr(buyCost)]);
            Dialog.PriceLbl.Text = format + ".";
            Spell = AoWHSSet.FSpellControl.GetSpell(Tower.FSpellID);
            Dialog.SpellName.Text = Spell.FSpellName;
            var magicSphere = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
            var level = TranslateRStr(AoWEUnit.LevelRStr);
            Dialog.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
            var castingCost = TranslateRStr(CastingCostRStr);
            var mana = TranslateRStr(ManaRStr);
            Dialog.SpellCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
            var text = Spell.FSpellText.Text;
            Dialog.ResearchMemo.Items.Text = text;
        }
        else
        {
            Dialog.PriceLbl.Text = "";
            var none = TranslateRStr(AoWEUnit.NoneRStr);
            Dialog.SpellName.Text = none;
            Dialog.SpellClass.Text = "";
            Dialog.SpellCost.Text = "";
            Dialog.ResearchMemo.Items.Text = "";
        }

        Dialog.Dlg.Visible = true;
    }

    // aowMapEvents.pas:647
    public static void ResetPartyJoin(TPartyJoinDlg Dialog)
    {
        Dialog.LeftBtn.Visible = true;
        Dialog.MiddleBtn.Visible = true;
        Dialog.RightBtn.Visible = true;
        var partyJoin = TranslateRStr(PartyJoinRStr);
        Dialog.Title.Text = partyJoin;
    }

    // aowMapEvents.pas:656
    public static void UpdatePartyJoin(TPartyJoinDlg Dialog)
    {
        if (Dialog.ValidateJoin())
        {
            Dialog.FUnits.Clear();
            Dialog.FFaceUnit = AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
            Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
            var text = Dialog.FJoinEvent.FJoinMessage.Text;
            Dialog.JoinMemo.Items.Text = text;
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
            Dialog.FFaceUnit = AoWHSMap.FUnitControl.FindUnit(Dialog.FJoinEvent.FFirstUnitID);
            Dialog.FJoinEvent.ListJoinUnits(Dialog.FUnits, ref Dialog.FSelection);
            var level = ((THero)Dialog.FFaceUnit).GetLevel();
            var format = AoWEUnit.Format(TranslateRStr(LevelXRStr), [IntToStr(level)]);
            Dialog.HeroLevel.Text = format;
            Dialog.HeroName.Text = ((THero)Dialog.FFaceUnit).FHeroName;
            var text = Dialog.FJoinEvent.FJoinMessage.Text;
            Dialog.JoinMemo.Items.Text = text;
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
        var accept = TranslateRStr(AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        var attack = TranslateRStr(AttackRStr);
        Dialog.MiddleBtn.Text = attack;
        var cancel = TranslateRStr(CancelRStr);
        Dialog.RightBtn.Text = cancel;
        UpdatePartyJoin(Dialog);
        AoWHSMap.FTutorialControl.ShowMessage(70, true, true);
    }

    // aowMapEvents.pas:706
    public static void ShowPrisonersJoin(TPartyJoinDlg Dialog, TPrisonersJoinEventLog Event, TExecuteDoneEvent Done)
    {
        ResetPartyJoin(Dialog);
        Dialog.Dlg.Visible = true;
        var prisonerLiberation = TranslateRStr(PrisonerLiberationRStr);
        Dialog.Title.Text = prisonerLiberation;
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = TranslateRStr(AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var reject = TranslateRStr(RejectRStr);
        Dialog.RightBtn.Text = reject;
        UpdatePartyJoin(Dialog);
    }

    // aowMapEvents.pas:722
    public static void ShowHeroJoin(THeroJoinDlg Dialog, THeroJoinEventLog Event, TExecuteDoneEvent Done)
    {
        ResetHeroJoin(Dialog);
        Dialog.FJoinEvent = Event;
        Dialog.FJoinEvent.AddRef();
        Dialog.FDone = Done;
        var accept = TranslateRStr(AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var cancel = TranslateRStr(CancelRStr);
        Dialog.RightBtn.Text = cancel;
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
        var accept = TranslateRStr(AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        var attack = TranslateRStr(AttackRStr);
        Dialog.MiddleBtn.Text = attack;
        var cancel = TranslateRStr(CancelRStr);
        Dialog.RightBtn.Text = cancel;
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
        var accept = TranslateRStr(AcceptRStr);
        Dialog.LeftBtn.Text = accept;
        Dialog.MiddleBtn.Visible = false;
        var reject = TranslateRStr(RejectRStr);
        Dialog.RightBtn.Text = reject;
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
            Dialog.FEditedHero = (THero)Dialog.FHero.Copy(AoWEngine);
            Dialog.UpdateStatistics();
            Dialog.RefreshAbilities();
            Dialog.UnitName.Text = Dialog.FHero.FHeroName;
            var level = Dialog.FHero.GetLevel();
            var levelText = TranslateRStr(AoWEUnit.LevelRStr);
            Dialog.LevelLbl.Text = levelText + " " + IntToStr(level);
            if (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel > 1)
            {
                var format = AoWEUnit.Format(TranslateRStr(E3GlobalsUnit.XMadeYLevelsRStr), [Dialog.FHero.FHeroName, (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel)]);
                Dialog.HeaderLbl.Text = format + "!";
            }
            else
            {
                var format = AoWEUnit.Format(TranslateRStr(E3GlobalsUnit.XMadeALevelRStr), [Dialog.FHero.FHeroName]);
                Dialog.HeaderLbl.Text = format + "!";
            }

            Dialog.Dlg.Visible = true;
        }
    }

    // aowMapEvents.pas:811
    public static void ShowMessageEvent(TMessageDlg Dialog, TMessageEventLog Event, TExecuteDoneEvent Done)
    {
        TMessageEventLog Log;
        ResetMessageDialog(Dialog);
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        switch (Log.FLayout)
        {
            case TMessageEventLogLayout.melNormal:
                var text = Log.FMessage.Text;
                Dialog.MessageMemo.Items.Text = text;
                if (Dialog.MessageMemo.Items.Text.Length < 80)
                {
                    Dialog.MessageMemo.Items.Insert(0, "");
                }

                break;
            case TMessageEventLogLayout.melHeader:
                Dialog.MemoPnl.Visible = false;
                Dialog.HeaderMemoPnl.Visible = true;
                Dialog.HeaderLbl.Text = Log.FHeader;
                var fMessageText = Log.FMessage.Text;
                Dialog.HeaderMemo.Items.Text = fMessageText;
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
        if (!IsEmpty(Event.FDirectory))
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
        TPasswordEventLog Log;
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
