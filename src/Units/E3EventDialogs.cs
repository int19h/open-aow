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

            var call__3 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            if (call__3.FStatus == TPlayerStatus.psPlaying)
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
                        var call__4 = Dialog.FAction.FMessage.Text;
                        Dialog.RMMemo.Items.Text = call__4;
                        var call__5 = AoWHSMap.FPlayers.GetPlayers(Dialog.FAction.FSource);
                        var call__6 = call__5.GetName();
                        var call__7 = TranslateRStr(MessageFromXRStr);
                        var call__8 = AoWE.AoWE.Format(call__7, [call__6]);
                        Dialog.RMHeaderLbl.Text = call__8;
                        break;
                    case TDiplomaticActionType.daSendMessageAsY:
                        Dialog.HighlightPlayer(Dialog.FAction.FOtherPlayer);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FOtherPlayer);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.RMsgRecPnl.Visible = true;
                        var call__9 = Dialog.FAction.FMessage.Text;
                        Dialog.RMMemo.Items.Text = call__9;
                        var call__10 = AoWHSMap.FPlayers.GetPlayers(Dialog.FAction.FOtherPlayer);
                        var call__11 = call__10.GetName();
                        var call__12 = TranslateRStr(MessageFromXRStr);
                        var call__13 = AoWE.AoWE.Format(call__12, [call__11]);
                        Dialog.RMHeaderLbl.Text = call__13;
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
                            var call__14 = Dialog.FAction.FMessage.Text;
                            Dialog.MMemo.Items.Text = call__14;
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
                                var call__15 = Dialog.FAction.FMessage.Text;
                                Dialog.IIMemo.Items.Text = call__15;
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
                                var call__16 = Dialog.FAction.FMessage.Text;
                                Dialog.IIMemo.Items.Text = call__16;
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
                        var call__17 = Dialog.FAction.FMessage.Text;
                        Dialog.IIMemo.Items.Text = call__17;
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
            var call__19 = TranslateRStr(MapSettingsRStr);
            Dialog.LabelR.Text = call__19;
            Dialog.MemoR.Items.Clear();
            if (AoWHSMap.FVictoryCondition == 0)
            {
                var call__20 = TranslateRStr(ConquerTheMapRStr);
                var call__21 = TranslateRStr(ObjectiveXRStr);
                var call__22 = AoWE.AoWE.Format(call__21, [call__20]);
                Dialog.MemoR.Items.Add(call__22);
            }
            else
            {
                var call__23 = TranslateRStr(AoWE.AoWE.TVictoryConditionRStr[(int)AoWHSMap.FVictoryCondition]);
                var call__24 = TranslateRStr(ObjectiveXRStr);
                var call__25 = AoWE.AoWE.Format(call__24, [call__23]);
                Dialog.MemoR.Items.Add(call__25);
            }

            if (AoWHSMap.FDayLimit > 0)
            {
                var call__26 = TranslateRStr(DayLimitXRStr);
                var call__27 = AoWE.AoWE.Format(call__26, [IntToStr(AoWHSMap.FDayLimit)]);
                Dialog.MemoR.Items.Add(call__27);
            }

            if (AoWHSMap.FAlliedVictory)
            {
                var call__28 = TranslateRStr(EnabledRStr);
                var call__29 = TranslateRStr(AlliedVictoryXRStr);
                var call__30 = AoWE.AoWE.Format(call__29, [call__28]);
                Dialog.MemoR.Items.Add(call__30);
            }
            else
            {
                var call__31 = TranslateRStr(DisabledRStr);
                var call__32 = TranslateRStr(AlliedVictoryXRStr);
                var call__33 = AoWE.AoWE.Format(call__32, [call__31]);
                Dialog.MemoR.Items.Add(call__33);
            }

            var call__34 = AoWHSMap.FPlayers.GetCount();
            var call__35 = TranslateRStr(PlayersOnMapXRStr);
            var call__36 = AoWE.AoWE.Format(call__35, [IntToStr(call__34 - 1)]);
            Dialog.MemoR.Items.Add(call__36);
            var call__37 = TranslateRStr(MaximumSphereLevelXRStr);
            var call__38 = AoWE.AoWE.Format(call__37, [IntToStr(AoWHSMap.FSphereCount)]);
            Dialog.MemoR.Items.Add(call__38);
            var call__39 = TranslateRStr(MaximumPlayerHeroesXRStr);
            var call__40 = AoWE.AoWE.Format(call__39, [IntToStr(AoWHSMap.FHeroCountMax)]);
            Dialog.MemoR.Items.Add(call__40);
            if (AoWHSMap.FConnectionType == 0 && AoWHSMap.FRequestCombatType)
            {
                var call__41 = TranslateRStr(AoWE.AoWE.TPlayerConnectionTypeRStr[(int)AoWHSMap.FConnectionType]);
                var call__42 = TranslateRStr(HotSeatRStr);
                var call__43 = TranslateRStr(GametypeXRStr);
                var call__44 = AoWE.AoWE.Format(call__43, [call__41 + AnsiString.FromBytes(47) + call__42]);
                Dialog.MemoR.Items.Add(call__44);
            }
            else
            {
                var call__45 = TranslateRStr(AoWE.AoWE.TPlayerConnectionTypeRStr[(int)AoWHSMap.FConnectionType]);
                var call__46 = TranslateRStr(GametypeXRStr);
                var call__47 = AoWE.AoWE.Format(call__46, [call__45]);
                Dialog.MemoR.Items.Add(call__47);
            }

            var call__48 = TranslateRStr(TPlayModeRStr[(int)AoWHSMap.FPlayMode]);
            var call__49 = TranslateRStr(TurnModeXRStr);
            var call__50 = AoWE.AoWE.Format(call__49, [call__48]);
            Dialog.MemoR.Items.Add(call__50);
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

                var call__52 = AoWHSMap.FPlayers.GetPlayers(Dialog.FRequest.FPlayer);
                if (call__52.FPlayerType == TPlayerType.ptHuman)
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

            var call__55 = TranslateRStr(RecipientXRStr);
            var call__56 = AoWE.AoWE.Format(call__55, [Event.FToAddress]);
            Dialog.RecipientLbl.Text = call__56;
            try
            {
                Dialog.IMSSMTP.FMessage = Dialog.IMSMessage;
                var call__57 = AoWEngine.FRegistry.GetSMTPServer();
                Dialog.IMSSMTP.FHost = call__57;
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
