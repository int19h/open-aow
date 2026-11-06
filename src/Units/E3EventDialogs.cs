// Translated from E3EventDialogs.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.EventLog;
using Aow.Units.FastCombatWin;
using Aow.Units.AoWRelations;
using static Aow.Units.E2Globals.E2GlobalsUnit;
using Aow.Units.AoWWorldMap;
using Aow.Units.AoWSMTPDlg;
using Aow.Units.TCFCDlg;
using Aow.Units.AoWNewTurnDialog;
using Aow.Units.AoWSendEmailGameDlg;
using Aow.Units.AoWPasswordDlg;
using Aow.Units.AoWArmyEventDlg;
using Aow.Units.AoWCityRebelDlg;
using Aow.Units.AoWCityJoinDlg;
using Aow.Units.AoWPartyJoin;
using Aow.Units.AoWHeroJoinDlg;
using Aow.Units.AoWMsgReceiveDlg;
using Aow.Units.AoWHeroUpgradeDlg;
using Aow.Units.AoWHeroInfoDlg;
using Aow.Units.AoWWizardsTowerDlg;
using Aow.Units.AoWArenaDlg;
using Aow.Units.AoWItemsFoundDlg;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E3Globals.E3GlobalsUnit;
using Aow.Units.AoWComp;

namespace Aow.Units.E3EventDialogs;

public static partial class E3EventDialogsUnit
{
    // E3EventDialogs.pas:194
    // { Event-log dialog routines called by TMapEvents in AoW.exe. }
    public static void ShowDiplomaticEvent(TRelWindow Dialog, TDiplomaticActionEventLog Event, TExecuteDoneEvent Done)
    {
        Dialog.ClearActions();
        Dialog.ResetChecks();
        // { Original reports its source filename, independent of this checkout. }
        if (Dialog.FEvent != null)
        {
            OriginalAssert("Assertion failure", "D:\\AoWDev\\AoWRelations.pas", 663);
        }

        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Event.AddRef();

        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStatus == TPlayerStatus.psPlaying)
        {
            Dialog.ShowRelations(Event.FAction.FTarget);
            Dialog.SetAction(Event.FAction);
            var proposal = Dialog.FAction.Proposal;
            var view = proposal.View;
            if (view == ProposalView.Message)
            {
                var from = proposal.SignedAsOtherPlayer ? Dialog.FAction.FOtherPlayer : Dialog.FAction.FSource;
                Dialog.HighlightPlayer(from);
                Dialog.ShowPlayerInformation(from);
                Dialog.RNewActionPnl.Visible = false;
                Dialog.RMsgRecPnl.Visible = true;
                var text = proposal.MessageText;
                Dialog.RMMemo.Items.Text = text;
                var name = AoWHSMap.FPlayers.GetPlayers(from).GetName();
                var format = AoWEUnit.Format(TranslateRStr(MessageFromXRStr), [name]);
                Dialog.RMHeaderLbl.Text = format;
            }
            else if (view == ProposalView.Offer)
            {
                Dialog.RNewActionPnl.Visible = false;
                if (Dialog.FAction.FReaction == TDiplomaticActionReaction.darNone)
                {
                    Dialog.HighlightPlayer(Dialog.FAction.FSource);
                    Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                    Dialog.RIncomingPnl.Visible = true;
                    var (_, gold, mana) = proposal.ToFields();
                    Dialog.MBGoldBribeV.Text = IntToStr(gold);
                    Dialog.MBManaBribeV.Text = IntToStr(mana);
                    var text2 = proposal.MessageText;
                    Dialog.MMemo.Items.Text = text2;
                    if (proposal.Icon >= 0)
                    {
                        Dialog.MIcon.CurrentImage = proposal.Icon;
                    }
                }
                else
                {
                    if (AoWHSMap.FSeatedPlayer == Dialog.FAction.FSource && Dialog.FAction.FReaction != (TDiplomaticActionReaction)3)
                    {
                        Dialog.HighlightPlayer(Dialog.FAction.FTarget);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FTarget);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        Dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            Dialog.IIIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        Dialog.FinishEvent();
                        if (Dialog.FAction.FReaction == (TDiplomaticActionReaction)3)
                        {
                            Dialog.ShowPlayerActions(Dialog.FAction.FTarget);
                        }
                        else
                        {
                            Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                        }
                    }
                }
            }
            else if (view == ProposalView.Notice)
            {
                if (AoWHSMap.FSeatedPlayer == Dialog.FAction.FSource)
                {
                    Dialog.FinishEvent();
                    Dialog.ShowPlayerActions(Dialog.FAction.FTarget);
                }
                else
                {
                    if (AoWHSMap.FSeatedPlayer == Dialog.FAction.FTarget)
                    {
                        Dialog.HighlightPlayer(Dialog.FAction.FSource);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        Dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            Dialog.IIIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        Dialog.FinishEvent();
                        Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                    }
                }
            }
            else if (view == ProposalView.Threat)
            {
                Dialog.HighlightPlayer(Dialog.FAction.FSource);
                Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                Dialog.RNewActionPnl.Visible = false;
                Dialog.IncommingInfoPnl.Visible = true;
                var text3 = proposal.MessageText;
                Dialog.IIMemo.Items.Text = text3;
                if (proposal.Icon >= 0)
                {
                    Dialog.IIIcon.CurrentImage = proposal.Icon;
                }
            }
        }
        else
        {
            Dialog.FinishDialog();
        }
    }

    // E3EventDialogs.pas:751
    public static void ShowInformationBook(TWorldMap Dialog, TShowBookEventLog Event, TExecuteDoneEvent Done)
    {
        Dialog.FOnBookClosed = RebindDelegate<TNotifyEvent>(Done);
        Dialog.FBookContext = Event;
        if (Dialog.FBookContext != null)
        {
            Dialog.FBookContext.AddRef();
        }

        Dialog.LabelL.Text = AoWHSMap.FMapName;
        Dialog.MemoL.Items.Clear();
        Dialog.MemoL.Items.AddStrings(AoWHSMap.FDescription);
        var mapSettings = TranslateRStr(MapSettingsRStr);
        Dialog.LabelR.Text = mapSettings;
        Dialog.MemoR.Items.Clear();
        if (AoWHSMap.FVictoryCondition == 0)
        {
            var conquerTheMap = TranslateRStr(ConquerTheMapRStr);
            var format = AoWEUnit.Format(TranslateRStr(ObjectiveXRStr), [conquerTheMap]);
            Dialog.MemoR.Items.Add(format);
        }
        else
        {
            var victoryCondition = TranslateRStr(AoWEUnit.TVictoryConditionRStr[(int)AoWHSMap.FVictoryCondition]);
            var format = AoWEUnit.Format(TranslateRStr(ObjectiveXRStr), [victoryCondition]);
            Dialog.MemoR.Items.Add(format);
        }

        if (AoWHSMap.FDayLimit > 0)
        {
            var format = AoWEUnit.Format(TranslateRStr(DayLimitXRStr), [IntToStr(AoWHSMap.FDayLimit)]);
            Dialog.MemoR.Items.Add(format);
        }

        if (AoWHSMap.FAlliedVictory)
        {
            var enabled = TranslateRStr(EnabledRStr);
            var format = AoWEUnit.Format(TranslateRStr(AlliedVictoryXRStr), [enabled]);
            Dialog.MemoR.Items.Add(format);
        }
        else
        {
            var disabled = TranslateRStr(DisabledRStr);
            var format = AoWEUnit.Format(TranslateRStr(AlliedVictoryXRStr), [disabled]);
            Dialog.MemoR.Items.Add(format);
        }

        var count = AoWHSMap.FPlayers.GetCount();
        var aoWEFormat = AoWEUnit.Format(TranslateRStr(PlayersOnMapXRStr), [IntToStr(count - 1)]);
        Dialog.MemoR.Items.Add(aoWEFormat);
        var format2 = AoWEUnit.Format(TranslateRStr(MaximumSphereLevelXRStr), [IntToStr(AoWHSMap.FSphereCount)]);
        Dialog.MemoR.Items.Add(format2);
        var format3 = AoWEUnit.Format(TranslateRStr(MaximumPlayerHeroesXRStr), [IntToStr(AoWHSMap.FHeroCountMax)]);
        Dialog.MemoR.Items.Add(format3);
        if (AoWHSMap.FConnectionType == 0 && AoWHSMap.FRequestCombatType)
        {
            var playerConnectionType = TranslateRStr(AoWEUnit.TPlayerConnectionTypeRStr[(int)AoWHSMap.FConnectionType]);
            var hotSeat = TranslateRStr(HotSeatRStr);
            var format = AoWEUnit.Format(TranslateRStr(GametypeXRStr), [playerConnectionType + "/" + hotSeat]);
            Dialog.MemoR.Items.Add(format);
        }
        else
        {
            var playerConnectionType = TranslateRStr(AoWEUnit.TPlayerConnectionTypeRStr[(int)AoWHSMap.FConnectionType]);
            var format = AoWEUnit.Format(TranslateRStr(GametypeXRStr), [playerConnectionType]);
            Dialog.MemoR.Items.Add(format);
        }

        var playMode = TranslateRStr(TPlayModeRStr[(int)AoWHSMap.FPlayMode]);
        var format4 = AoWEUnit.Format(TranslateRStr(TurnModeXRStr), [playMode]);
        Dialog.MemoR.Items.Add(format4);
        Dialog.PicL.Visible = false;
        Dialog.PicR.Visible = false;
        Dialog.MemoL.Visible = true;
        Dialog.MemoR.Visible = true;
        Dialog.Back.Visible = false;
        Dialog.Cancel.Visible = false;
        Dialog.Next.Visible = false;
        Dialog.BookWin.Visible = true;
        if (Dialog.BookWin.FBackSurface != null)
        {
            Dialog.BookWin.FBackSurface.Cls(Dialog.BookWin.FMaskColor);
        }

        General.Manager.OverlayPriority = 49;
    }

    // E3EventDialogs.pas:170
    public static void ShowCombatType(TTCFCDialog Dialog, TCombatTypeRequestEventLog Event, TExecuteDoneEvent Done)
    {
        if (DialogWin.FSkipCombatChoice)
        {
            if (Dialog.TCchk.Checked)
            {
                Event.Select((TCombatType)2);
            }
            else
            {
                Event.Select((TCombatType)1);
            }

            Done(Event);
        }
        else
        {
            Dialog.FRequest = Event;
            Dialog.FRequest.AddRef();
            Dialog.FOnDone = RebindDelegate<TNotifyEvent>(Done);
            General.Manager.OverlayPriority = 49;
            if (Dialog.TCchk.Checked)
            {
                Event.Select((TCombatType)2);
            }
            else
            {
                Event.Select((TCombatType)1);
            }

            if (AoWHSMap.FPlayers.GetPlayers(Dialog.FRequest.FPlayer).FPlayerType == TPlayerType.ptHuman)
            {
                Dialog.CancelBtn.Visible = true;
                Dialog.FightBtn.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            }
            else
            {
                Dialog.CancelBtn.Visible = false;
                Dialog.FightBtn.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            }

            Dialog.Dlg.ReAlign();
            Dialog.Dlg.Visible = true;
            AoWHSMap.FTutorialControl.ShowMessage(61, true, true);
        }
    }

    // E3EventDialogs.pas:786
    public static void ShowSMTPEvent(TSMTPDlg Dialog, TSMTPEventLog Event, TExecuteDoneEvent Done)
    {
        Dialog.ResetProgress();
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        var smtpEventLog = Dialog.FEvent;
        smtpEventLog.AddRef();

        var format = AoWEUnit.Format(TranslateRStr(RecipientXRStr), [Event.FToAddress]);
        Dialog.RecipientLbl.Text = format;
        try
        {
            Dialog.IMSSMTP.FMessage = Dialog.IMSMessage;
            var smtpServer = AoWEngine.FRegistry.GetSMTPServer();
            Dialog.IMSSMTP.FHost = smtpServer;
            Dialog.IMSSMTP.WinsockLoaded = true;
            var tmsMessage = Dialog.IMSMessage;
            tmsMessage.FEncode = false;
            tmsMessage.FIncludeHeader = false;
            tmsMessage.FHeader.FFrom = Event.FFromAddress;
            tmsMessage.FRecipients.Clear();
            Dialog.IMSMessage.FRecipients.AddAddress(Event.FToAddress, "");

            Dialog.IMSMessage.FSubject = Event.FSubject;
            Dialog.IMSMessage.SetBody(Event.FMessage);
            Dialog.IMSMessage.FAttachments.Clear();
            Dialog.IMSMessage.FAttachments.AddFile(Event.FFileName);
            Dialog.StartSending();
            Dialog.Dlg.Visible = true;
        }
        catch
        {
            Dialog.FEvent.FSuccess = false;
            Dialog.FFinished = true;
            Dialog.Dlg.Visible = true;
        }
    }

    public static TSendEmailGameDlg SendEmailGameDlg;
    public static TPasswordDlg PasswordDlg;
    public static TArmyEventDlg ArmyEventDlg;
    public static TCityRebelDlg CityRebelDlg;
    public static TCityJoinDlg CityJoinDlg;
    public static TPartyJoinDlg PartyJoinDlg;
    public static THeroJoinDlg HeroJoinDlg;
    public static TMsgReceiveDlg MsgReceiveDlg;
    public static THeroUpgradeDlg HeroUpgradeDlg;
    public static THeroInfoDlg HeroInfoDlg;
    public static TWizardsTowerDlg WizardsTowerDlg;
    public static TArenaDlg ArenaDlg;
    public static TItemsFoundDlg ItemsFoundDlg;
    public static TFastCombatWindow FastCombatWindow;
    public static TNewTurnDlg NewTurnDlg;
    public static TTCFCDialog TCFCDialog;
    // E3EventDialogs.pas:33
    public static void OriginalAssert(string Message, string Filename, int LineNumber)
    {
        Assert(false, Message, Filename, LineNumber);
    }
}
