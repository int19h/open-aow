// Translated from E3EventDialogs.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.FastCombatWin;
using static Aow.Units.FastCombatWin.FastCombatWin;
using Aow.Units.AoWRelations;
using static Aow.Units.AoWRelations.AoWRelations;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using Aow.Units.AoWSMTPDlg;
using static Aow.Units.AoWSMTPDlg.AoWSMTPDlg;
using Aow.Units.TCFCDlg;
using static Aow.Units.TCFCDlg.TCFCDlg;
using Aow.Units.AoWNewTurnDialog;
using static Aow.Units.AoWNewTurnDialog.AoWNewTurnDialog;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.AoWSendEmailGameDlg;
using static Aow.Units.AoWSendEmailGameDlg.AoWSendEmailGameDlg;
using Aow.Units.AoWPasswordDlg;
using static Aow.Units.AoWPasswordDlg.AoWPasswordDlg;
using Aow.Units.AoWArmyEventDlg;
using static Aow.Units.AoWArmyEventDlg.AoWArmyEventDlg;
using Aow.Units.AoWCityRebelDlg;
using static Aow.Units.AoWCityRebelDlg.AoWCityRebelDlg;
using Aow.Units.AoWCityJoinDlg;
using static Aow.Units.AoWCityJoinDlg.AoWCityJoinDlg;
using Aow.Units.AoWPartyJoin;
using static Aow.Units.AoWPartyJoin.AoWPartyJoin;
using Aow.Units.AoWHeroJoinDlg;
using static Aow.Units.AoWHeroJoinDlg.AoWHeroJoinDlg;
using Aow.Units.AoWMsgReceiveDlg;
using static Aow.Units.AoWMsgReceiveDlg.AoWMsgReceiveDlg;
using Aow.Units.AoWTutorialWin;
using static Aow.Units.AoWTutorialWin.AoWTutorialWin;
using Aow.Units.AoWHeroUpgradeDlg;
using static Aow.Units.AoWHeroUpgradeDlg.AoWHeroUpgradeDlg;
using Aow.Units.AoWHeroInfoDlg;
using static Aow.Units.AoWHeroInfoDlg.AoWHeroInfoDlg;
using Aow.Units.AoWWizardsTowerDlg;
using static Aow.Units.AoWWizardsTowerDlg.AoWWizardsTowerDlg;
using Aow.Units.AoWArenaDlg;
using static Aow.Units.AoWArenaDlg.AoWArenaDlg;
using Aow.Units.AoWItemsFoundDlg;
using static Aow.Units.AoWItemsFoundDlg.AoWItemsFoundDlg;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;

namespace Aow.Units.E3EventDialogs
{
    public static partial class E3EventDialogs
    {
        // E3EventDialogs.pas:194
        // { Event-log dialog routines called by TMapEvents in AoW.exe. }
        public static void ShowDiplomaticEvent(Aow.Units.AoWRelations.TRelWindow Dialog, Aow.Units.AoWE.TDiplomaticActionEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.ClearActions();
            Dialog.ResetChecks();
            // { Original reports its source filename, independent of this checkout. }
            if (Dialog.FEvent != null)
            {
                Aow.Units.E3EventDialogs.E3EventDialogs.OriginalAssert((AnsiString)"Assertion failure", (AnsiString)"D:\\AoWDev\\AoWRelations.pas", 663);
            }

            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            Event.AddRef();

            var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__3.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying)
            {
                Dialog.ShowRelations(Event.FAction.FTarget);
                Dialog.SetAction(Event.FAction);
                switch (Dialog.FAction.FActionType)
                {
                    case Aow.Units.AoWE.TDiplomaticActionType.daSendMessage:
                        Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FAction.FSource));
                        Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FAction.FSource));
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.RMsgRecPnl.Visible = true;
                        var call__4 = Dialog.FAction.FMessage.Text;
                        Dialog.RMMemo.Items.Text = call__4;
                        var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Dialog.FAction.FSource);
                        var call__6 = call__5.GetName();
                        var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MessageFromXRStr);
                        var call__8 = Aow.Units.AoWE.AoWE.Format(call__7, [call__6]);
                        Dialog.RMHeaderLbl.Text = call__8;
                        break;
                    case Aow.Units.AoWE.TDiplomaticActionType.daSendMessageAsY:
                        Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FAction.FOtherPlayer));
                        Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FAction.FOtherPlayer));
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.RMsgRecPnl.Visible = true;
                        var call__9 = Dialog.FAction.FMessage.Text;
                        Dialog.RMMemo.Items.Text = call__9;
                        var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Dialog.FAction.FOtherPlayer);
                        var call__11 = call__10.GetName();
                        var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MessageFromXRStr);
                        var call__13 = Aow.Units.AoWE.AoWE.Format(call__12, [call__11]);
                        Dialog.RMHeaderLbl.Text = call__13;
                        break;
                    case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                    case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                    case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                    case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                    case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                        Dialog.RNewActionPnl.Visible = false;
                        if (Dialog.FAction.FReaction == Aow.Units.AoWE.TDiplomaticActionReaction.darNone)
                        {
                            Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FAction.FSource));
                            Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FAction.FSource));
                            Dialog.RIncomingPnl.Visible = true;
                            Dialog.MBGoldBribeV.Text = Aow.Rtl.Rtl.IntToStr(Dialog.FAction.FGold);
                            Dialog.MBManaBribeV.Text = Aow.Rtl.Rtl.IntToStr(Dialog.FAction.FMana);
                            var call__14 = Dialog.FAction.FMessage.Text;
                            Dialog.MMemo.Items.Text = call__14;
                            switch (Dialog.FAction.FActionType)
                            {
                                case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                                    Dialog.MIcon.CurrentImage = 8;
                                    break;
                                case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                                    Dialog.MIcon.CurrentImage = 10;
                                    break;
                                case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                                    Dialog.MIcon.CurrentImage = 47;
                                    break;
                                case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                                    Dialog.MIcon.CurrentImage = 47;
                                    break;
                                case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                                    Dialog.MIcon.CurrentImage = 48;
                                    break;
                            }
                        }
                        else
                        {
                            if ((Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Dialog.FAction.FSource) && (Dialog.FAction.FReaction != ((Aow.Units.AoWE.TDiplomaticActionReaction)3)))
                            {
                                Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FAction.FTarget));
                                Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FAction.FTarget));
                                Dialog.RNewActionPnl.Visible = false;
                                Dialog.IncommingInfoPnl.Visible = true;
                                var call__15 = Dialog.FAction.FMessage.Text;
                                Dialog.IIMemo.Items.Text = call__15;
                                switch (Dialog.FAction.FActionType)
                                {
                                    case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                                        Dialog.IIIcon.CurrentImage = 8;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                                        Dialog.IIIcon.CurrentImage = 10;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                                        Dialog.IIIcon.CurrentImage = 47;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                                        Dialog.IIIcon.CurrentImage = 47;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                                        Dialog.IIIcon.CurrentImage = 48;
                                        break;
                                }
                            }
                            else
                            {
                                Dialog.FinishEvent();
                                if (Dialog.FAction.FReaction == ((Aow.Units.AoWE.TDiplomaticActionReaction)3))
                                {
                                    Dialog.ShowPlayerActions(unchecked((sbyte)Dialog.FAction.FTarget));
                                }
                                else
                                {
                                    Dialog.ShowPlayerActions(unchecked((sbyte)Dialog.FAction.FSource));
                                }
                            }
                        }

                        break;
                    case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWar:
                    case Aow.Units.AoWE.TDiplomaticActionType.daBreakAlliance:
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Dialog.FAction.FSource)
                        {
                            Dialog.FinishEvent();
                            Dialog.ShowPlayerActions(unchecked((sbyte)Dialog.FAction.FTarget));
                        }
                        else
                        {
                            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Dialog.FAction.FTarget)
                            {
                                Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FAction.FSource));
                                Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FAction.FSource));
                                Dialog.RNewActionPnl.Visible = false;
                                Dialog.IncommingInfoPnl.Visible = true;
                                var call__16 = Dialog.FAction.FMessage.Text;
                                Dialog.IIMemo.Items.Text = call__16;
                                if (Dialog.FAction.FActionType == Aow.Units.AoWE.TDiplomaticActionType.daDeclareWar)
                                {
                                    Dialog.IIIcon.CurrentImage = 9;
                                }
                                else
                                {
                                    if (Dialog.FAction.FActionType == Aow.Units.AoWE.TDiplomaticActionType.daBreakAlliance)
                                    {
                                        Dialog.IIIcon.CurrentImage = 8;
                                    }
                                }
                            }
                            else
                            {
                                Dialog.FinishEvent();
                                Dialog.ShowPlayerActions(unchecked((sbyte)Dialog.FAction.FSource));
                            }
                        }

                        break;
                    case Aow.Units.AoWE.TDiplomaticActionType.daThreatenToAttack:
                        Dialog.HighlightPlayer(unchecked((sbyte)Dialog.FAction.FSource));
                        Dialog.ShowPlayerInformation(unchecked((sbyte)Dialog.FAction.FSource));
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
        public static void ShowInformationBook(Aow.Units.AoWWorldMap.TWorldMap Dialog, Aow.Units.EventLog.TShowBookEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.FOnBookClosed = Aow.Rtl.Rtl.RebindDelegate<Aow.Rtl.TNotifyEvent>(Done);
            Dialog.FBookContext = Event;
            if (Dialog.FBookContext != null)
            {
                Dialog.FBookContext.AddRef();
            }

            Dialog.LabelL.Text = Aow.Units.AoWE.AoWE.AoWHSMap.FMapName;
            Dialog.MemoL.Items.Clear();
            Dialog.MemoL.Items.AddStrings(Aow.Units.AoWE.AoWE.AoWHSMap.FDescription);
            var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.MapSettingsRStr);
            Dialog.LabelR.Text = call__19;
            Dialog.MemoR.Items.Clear();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FVictoryCondition == ((Aow.Units.AoWE.TVictoryCondition)0))
            {
                var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConquerTheMapRStr);
                var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ObjectiveXRStr);
                var call__22 = Aow.Units.AoWE.AoWE.Format(call__21, [call__20]);
                Dialog.MemoR.Items.Add(call__22);
            }
            else
            {
                var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TVictoryConditionRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FVictoryCondition]);
                var call__24 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ObjectiveXRStr);
                var call__25 = Aow.Units.AoWE.AoWE.Format(call__24, [call__23]);
                Dialog.MemoR.Items.Add(call__25);
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDayLimit > 0)
            {
                var call__26 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.DayLimitXRStr);
                var call__27 = Aow.Units.AoWE.AoWE.Format(call__26, [Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FDayLimit)]);
                Dialog.MemoR.Items.Add(call__27);
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FAlliedVictory)
            {
                var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnabledRStr);
                var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlliedVictoryXRStr);
                var call__30 = Aow.Units.AoWE.AoWE.Format(call__29, [call__28]);
                Dialog.MemoR.Items.Add(call__30);
            }
            else
            {
                var call__31 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DisabledRStr);
                var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlliedVictoryXRStr);
                var call__33 = Aow.Units.AoWE.AoWE.Format(call__32, [call__31]);
                Dialog.MemoR.Items.Add(call__33);
            }

            var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount();
            var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PlayersOnMapXRStr);
            var call__36 = Aow.Units.AoWE.AoWE.Format(call__35, [Aow.Rtl.Rtl.IntToStr(call__34 - 1)]);
            Dialog.MemoR.Items.Add(call__36);
            var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MaximumSphereLevelXRStr);
            var call__38 = Aow.Units.AoWE.AoWE.Format(call__37, [Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FSphereCount)]);
            Dialog.MemoR.Items.Add(call__38);
            var call__39 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MaximumPlayerHeroesXRStr);
            var call__40 = Aow.Units.AoWE.AoWE.Format(call__39, [Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FHeroCountMax)]);
            Dialog.MemoR.Items.Add(call__40);
            if ((Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType == ((Aow.Units.AoWE.TPlayerConnectionType)0)) && Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType)
            {
                var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerConnectionTypeRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType]);
                var call__42 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HotSeatRStr);
                var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GametypeXRStr);
                var call__44 = Aow.Units.AoWE.AoWE.Format(call__43, [(call__41 + AnsiString.FromBytes(47)) + call__42]);
                Dialog.MemoR.Items.Add(call__44);
            }
            else
            {
                var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerConnectionTypeRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType]);
                var call__46 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GametypeXRStr);
                var call__47 = Aow.Units.AoWE.AoWE.Format(call__46, [call__45]);
                Dialog.MemoR.Items.Add(call__47);
            }

            var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayModeRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode]);
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnModeXRStr);
            var call__50 = Aow.Units.AoWE.AoWE.Format(call__49, [call__48]);
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

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
        }

        // E3EventDialogs.pas:170
        public static void ShowCombatType(Aow.Units.TCFCDlg.TTCFCDialog Dialog, Aow.Units.AoWE.TCombatTypeRequestEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            if (Aow.Units.E2Globals.E2Globals.DialogWin.FSkipCombatChoice)
            {
                if (Dialog.TCchk.Checked)
                {
                    Event.Select((Aow.Units.AoWE.TCombatType)2);
                }
                else
                {
                    Event.Select((Aow.Units.AoWE.TCombatType)1);
                }

                Done(Event);
            }
            else
            {
                Dialog.FRequest = Event;
                Dialog.FRequest.AddRef();
                Dialog.FOnDone = Aow.Rtl.Rtl.RebindDelegate<Aow.Rtl.TNotifyEvent>(Done);
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
                if (Dialog.TCchk.Checked)
                {
                    Event.Select((Aow.Units.AoWE.TCombatType)2);
                }
                else
                {
                    Event.Select((Aow.Units.AoWE.TCombatType)1);
                }

                var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Dialog.FRequest.FPlayer);
                if (call__52.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                {
                    Dialog.CancelBtn.Visible = true;
                    Dialog.FightBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                }
                else
                {
                    Dialog.CancelBtn.Visible = false;
                    Dialog.FightBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                }

                Dialog.Dlg.ReAlign();
                Dialog.Dlg.Visible = true;
                Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(61, true, true);
            }
        }

        // E3EventDialogs.pas:786
        public static void ShowSMTPEvent(Aow.Units.AoWSMTPDlg.TSMTPDlg Dialog, Aow.Units.AoWE.TSMTPEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Dialog.ResetProgress();
            Dialog.FDone = Done;
            Dialog.FEvent = Event;
            var smtpEventLog = Dialog.FEvent;
            smtpEventLog.AddRef();

            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RecipientXRStr);
            var call__56 = Aow.Units.AoWE.AoWE.Format(call__55, [Event.FToAddress]);
            Dialog.RecipientLbl.Text = call__56;
            try
            {
                Dialog.IMSSMTP.FMessage = Dialog.IMSMessage;
                var call__57 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSMTPServer();
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

        public static Aow.Units.AoWSendEmailGameDlg.TSendEmailGameDlg SendEmailGameDlg;
        public static Aow.Units.AoWPasswordDlg.TPasswordDlg PasswordDlg;
        public static Aow.Units.AoWArmyEventDlg.TArmyEventDlg ArmyEventDlg;
        public static Aow.Units.AoWCityRebelDlg.TCityRebelDlg CityRebelDlg;
        public static Aow.Units.AoWCityJoinDlg.TCityJoinDlg CityJoinDlg;
        public static Aow.Units.AoWPartyJoin.TPartyJoinDlg PartyJoinDlg;
        public static Aow.Units.AoWHeroJoinDlg.THeroJoinDlg HeroJoinDlg;
        public static Aow.Units.AoWMsgReceiveDlg.TMsgReceiveDlg MsgReceiveDlg;
        public static Aow.Units.AoWHeroUpgradeDlg.THeroUpgradeDlg HeroUpgradeDlg;
        public static Aow.Units.AoWHeroInfoDlg.THeroInfoDlg HeroInfoDlg;
        public static Aow.Units.AoWWizardsTowerDlg.TWizardsTowerDlg WizardsTowerDlg;
        public static Aow.Units.AoWArenaDlg.TArenaDlg ArenaDlg;
        public static Aow.Units.AoWItemsFoundDlg.TItemsFoundDlg ItemsFoundDlg;
        public static Aow.Units.FastCombatWin.TFastCombatWindow FastCombatWindow;
        public static Aow.Units.AoWNewTurnDialog.TNewTurnDlg NewTurnDlg;
        public static Aow.Units.TCFCDlg.TTCFCDialog TCFCDialog;
        // E3EventDialogs.pas:33
        public static void OriginalAssert(Aow.Rtl.AnsiString Message, Aow.Rtl.AnsiString Filename, int LineNumber)
        {
            Aow.Rtl.Rtl.Assert(false, Message, Filename.ToString(), LineNumber);
        }
    }
}
