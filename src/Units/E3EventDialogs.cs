// Translated from E3EventDialogs.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.FastCombatWin;
using Aow.Units.AoWRelations;
using static Aow.Units.E2Globals.E2Globals;
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
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWComp;

namespace Aow.Units.E3EventDialogs
{
    public static partial class E3EventDialogs
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
                OriginalAssert((AnsiString)"Assertion failure", (AnsiString)"D:\\AoWDev\\AoWRelations.pas", 663);
            }

            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            Event.AddRef();

            if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStatus == TPlayerStatus.psPlaying)
            {
                Dialog.ShowRelations(Event.FAction.FTarget);
                Dialog.SetAction(Event.FAction);
                switch (Dialog.FAction.FActionType)
                {
                    case TDiplomaticActionType.daSendMessage:
                        Dialog.HighlightPlayer(Dialog.FAction.FSource);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.RMsgRecPnl.Visible = true;
                        var text = Dialog.FAction.FMessage.Text;
                        Dialog.RMMemo.Items.Text = text;
                        var name = AoWHSMap.FPlayers.GetPlayers(Dialog.FAction.FSource).GetName();
                        var format = AoWE.AoWE.Format(TranslateRStr(MessageFromXRStr), [name]);
                        Dialog.RMHeaderLbl.Text = format;
                        break;
                    case TDiplomaticActionType.daSendMessageAsY:
                        Dialog.HighlightPlayer(Dialog.FAction.FOtherPlayer);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FOtherPlayer);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.RMsgRecPnl.Visible = true;
                        var fMessageText = Dialog.FAction.FMessage.Text;
                        Dialog.RMMemo.Items.Text = fMessageText;
                        var name2 = AoWHSMap.FPlayers.GetPlayers(Dialog.FAction.FOtherPlayer).GetName();
                        var aoWEFormat = AoWE.AoWE.Format(TranslateRStr(MessageFromXRStr), [name2]);
                        Dialog.RMHeaderLbl.Text = aoWEFormat;
                        break;
                    case TDiplomaticActionType.daOfferPeace:
                    case TDiplomaticActionType.daProposeAlliance:
                    case TDiplomaticActionType.daMakeTribute:
                    case TDiplomaticActionType.daBreakAllianceWithY:
                    case TDiplomaticActionType.daDeclareWarToY:
                        Dialog.RNewActionPnl.Visible = false;
                        if (Dialog.FAction.FReaction == TDiplomaticActionReaction.darNone)
                        {
                            Dialog.HighlightPlayer(Dialog.FAction.FSource);
                            Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                            Dialog.RIncomingPnl.Visible = true;
                            Dialog.MBGoldBribeV.Text = IntToStr(Dialog.FAction.FGold);
                            Dialog.MBManaBribeV.Text = IntToStr(Dialog.FAction.FMana);
                            var text2 = Dialog.FAction.FMessage.Text;
                            Dialog.MMemo.Items.Text = text2;
                            switch (Dialog.FAction.FActionType)
                            {
                                case TDiplomaticActionType.daOfferPeace:
                                    Dialog.MIcon.CurrentImage = 8;
                                    break;
                                case TDiplomaticActionType.daProposeAlliance:
                                    Dialog.MIcon.CurrentImage = 10;
                                    break;
                                case TDiplomaticActionType.daDeclareWarToY:
                                    Dialog.MIcon.CurrentImage = 47;
                                    break;
                                case TDiplomaticActionType.daBreakAllianceWithY:
                                    Dialog.MIcon.CurrentImage = 47;
                                    break;
                                case TDiplomaticActionType.daMakeTribute:
                                    Dialog.MIcon.CurrentImage = 48;
                                    break;
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
                                var text2 = Dialog.FAction.FMessage.Text;
                                Dialog.IIMemo.Items.Text = text2;
                                switch (Dialog.FAction.FActionType)
                                {
                                    case TDiplomaticActionType.daOfferPeace:
                                        Dialog.IIIcon.CurrentImage = 8;
                                        break;
                                    case TDiplomaticActionType.daProposeAlliance:
                                        Dialog.IIIcon.CurrentImage = 10;
                                        break;
                                    case TDiplomaticActionType.daDeclareWarToY:
                                        Dialog.IIIcon.CurrentImage = 47;
                                        break;
                                    case TDiplomaticActionType.daBreakAllianceWithY:
                                        Dialog.IIIcon.CurrentImage = 47;
                                        break;
                                    case TDiplomaticActionType.daMakeTribute:
                                        Dialog.IIIcon.CurrentImage = 48;
                                        break;
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

                        break;
                    case TDiplomaticActionType.daDeclareWar:
                    case TDiplomaticActionType.daBreakAlliance:
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
                                var text2 = Dialog.FAction.FMessage.Text;
                                Dialog.IIMemo.Items.Text = text2;
                                if (Dialog.FAction.FActionType == TDiplomaticActionType.daDeclareWar)
                                {
                                    Dialog.IIIcon.CurrentImage = 9;
                                }
                                else
                                {
                                    if (Dialog.FAction.FActionType == TDiplomaticActionType.daBreakAlliance)
                                    {
                                        Dialog.IIIcon.CurrentImage = 8;
                                    }
                                }
                            }
                            else
                            {
                                Dialog.FinishEvent();
                                Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                            }
                        }

                        break;
                    case TDiplomaticActionType.daThreatenToAttack:
                        Dialog.HighlightPlayer(Dialog.FAction.FSource);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text3 = Dialog.FAction.FMessage.Text;
                        Dialog.IIMemo.Items.Text = text3;
                        Dialog.IIIcon.CurrentImage = 46;
                        break;
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
                var format = AoWE.AoWE.Format(TranslateRStr(ObjectiveXRStr), [conquerTheMap]);
                Dialog.MemoR.Items.Add(format);
            }
            else
            {
                var victoryCondition = TranslateRStr(AoWE.AoWE.TVictoryConditionRStr[(int)AoWHSMap.FVictoryCondition]);
                var format = AoWE.AoWE.Format(TranslateRStr(ObjectiveXRStr), [victoryCondition]);
                Dialog.MemoR.Items.Add(format);
            }

            if (AoWHSMap.FDayLimit > 0)
            {
                var format = AoWE.AoWE.Format(TranslateRStr(DayLimitXRStr), [IntToStr(AoWHSMap.FDayLimit)]);
                Dialog.MemoR.Items.Add(format);
            }

            if (AoWHSMap.FAlliedVictory)
            {
                var enabled = TranslateRStr(EnabledRStr);
                var format = AoWE.AoWE.Format(TranslateRStr(AlliedVictoryXRStr), [enabled]);
                Dialog.MemoR.Items.Add(format);
            }
            else
            {
                var disabled = TranslateRStr(DisabledRStr);
                var format = AoWE.AoWE.Format(TranslateRStr(AlliedVictoryXRStr), [disabled]);
                Dialog.MemoR.Items.Add(format);
            }

            var count = AoWHSMap.FPlayers.GetCount();
            var aoWEFormat = AoWE.AoWE.Format(TranslateRStr(PlayersOnMapXRStr), [IntToStr(count - 1)]);
            Dialog.MemoR.Items.Add(aoWEFormat);
            var format2 = AoWE.AoWE.Format(TranslateRStr(MaximumSphereLevelXRStr), [IntToStr(AoWHSMap.FSphereCount)]);
            Dialog.MemoR.Items.Add(format2);
            var format3 = AoWE.AoWE.Format(TranslateRStr(MaximumPlayerHeroesXRStr), [IntToStr(AoWHSMap.FHeroCountMax)]);
            Dialog.MemoR.Items.Add(format3);
            if (AoWHSMap.FConnectionType == 0 && AoWHSMap.FRequestCombatType)
            {
                var playerConnectionType = TranslateRStr(AoWE.AoWE.TPlayerConnectionTypeRStr[(int)AoWHSMap.FConnectionType]);
                var hotSeat = TranslateRStr(HotSeatRStr);
                var format = AoWE.AoWE.Format(TranslateRStr(GametypeXRStr), [playerConnectionType + AnsiString.FromBytes(47) + hotSeat]);
                Dialog.MemoR.Items.Add(format);
            }
            else
            {
                var playerConnectionType = TranslateRStr(AoWE.AoWE.TPlayerConnectionTypeRStr[(int)AoWHSMap.FConnectionType]);
                var format = AoWE.AoWE.Format(TranslateRStr(GametypeXRStr), [playerConnectionType]);
                Dialog.MemoR.Items.Add(format);
            }

            var playMode = TranslateRStr(TPlayModeRStr[(int)AoWHSMap.FPlayMode]);
            var format4 = AoWE.AoWE.Format(TranslateRStr(TurnModeXRStr), [playMode]);
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

            var format = AoWE.AoWE.Format(TranslateRStr(RecipientXRStr), [Event.FToAddress]);
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
                Dialog.IMSMessage.FRecipients.AddAddress(Event.FToAddress, (AnsiString)"");

                Dialog.IMSMessage.FSubject = Event.FSubject;
                Dialog.IMSMessage.SetBody(Event.FMessage);
                Dialog.IMSMessage.FAttachments.Clear();
                Dialog.IMSMessage.FAttachments.AddFile(Event.FFileName);
                Dialog.StartSending();
                Dialog.Dlg.Visible = true;
            }
            catch (System.Exception)
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
        public static void OriginalAssert(AnsiString Message, AnsiString Filename, int LineNumber)
        {
            Assert(false, Message, Filename.ToString(), LineNumber);
        }
    }
}
