// Generated from E3EventDialogs.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
            // { Tried typed event view and explicit action case; no exact match. }
            {
                var w__1 = Dialog;
                w__1.ClearActions();
                w__1.ResetChecks();
                // { Original reports its source filename, independent of this checkout. }
                if (w__1.FEvent != null)
                {
                    Aow.Units.E3EventDialogs.E3EventDialogs.OriginalAssert((AnsiString)"Assertion failure", (AnsiString)"D:\\AoWDev\\AoWRelations.pas", 663);
                }

                w__1.FDone = Done;
                {
                    var w__2 = Event;
                    w__1.FEvent = Event;
                    w__2.AddRef();
                }

                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (call__3.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying)
                {
                    w__1.ShowRelations(Event.FAction.FTarget);
                    w__1.SetAction(Event.FAction);
                    switch (w__1.FAction.FActionType)
                    {
                        case Aow.Units.AoWE.TDiplomaticActionType.daSendMessage:
                            w__1.HighlightPlayer(unchecked((sbyte)w__1.FAction.FSource));
                            w__1.ShowPlayerInformation(unchecked((sbyte)w__1.FAction.FSource));
                            w__1.RNewActionPnl.Visible = false;
                            w__1.RMsgRecPnl.Visible = true;
                            var call__4 = w__1.FAction.FMessage.Text;
                            w__1.RMMemo.Items.Text = call__4;
                            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(w__1.FAction.FSource);
                            var call__6 = call__5.GetName();
                            var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MessageFromXRStr);
                            var call__8 = Aow.Units.AoWE.AoWE.Format(call__7, [call__6]);
                            w__1.RMHeaderLbl.Text = call__8;
                            break;
                        case Aow.Units.AoWE.TDiplomaticActionType.daSendMessageAsY:
                            w__1.HighlightPlayer(unchecked((sbyte)w__1.FAction.FOtherPlayer));
                            w__1.ShowPlayerInformation(unchecked((sbyte)w__1.FAction.FOtherPlayer));
                            w__1.RNewActionPnl.Visible = false;
                            w__1.RMsgRecPnl.Visible = true;
                            var call__9 = w__1.FAction.FMessage.Text;
                            w__1.RMMemo.Items.Text = call__9;
                            var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(w__1.FAction.FOtherPlayer);
                            var call__11 = call__10.GetName();
                            var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MessageFromXRStr);
                            var call__13 = Aow.Units.AoWE.AoWE.Format(call__12, [call__11]);
                            w__1.RMHeaderLbl.Text = call__13;
                            break;
                        case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                        case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                        case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                        case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                        case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                            w__1.RNewActionPnl.Visible = false;
                            if (w__1.FAction.FReaction == Aow.Units.AoWE.TDiplomaticActionReaction.darNone)
                            {
                                w__1.HighlightPlayer(unchecked((sbyte)w__1.FAction.FSource));
                                w__1.ShowPlayerInformation(unchecked((sbyte)w__1.FAction.FSource));
                                w__1.RIncomingPnl.Visible = true;
                                w__1.MBGoldBribeV.Text = Aow.Rtl.Rtl.IntToStr(w__1.FAction.FGold);
                                w__1.MBManaBribeV.Text = Aow.Rtl.Rtl.IntToStr(w__1.FAction.FMana);
                                var call__14 = w__1.FAction.FMessage.Text;
                                w__1.MMemo.Items.Text = call__14;
                                switch (w__1.FAction.FActionType)
                                {
                                    case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                                        w__1.MIcon.CurrentImage = 8;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                                        w__1.MIcon.CurrentImage = 10;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                                        w__1.MIcon.CurrentImage = 47;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                                        w__1.MIcon.CurrentImage = 47;
                                        break;
                                    case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                                        w__1.MIcon.CurrentImage = 48;
                                        break;
                                }
                            }
                            else
                            {
                                if ((Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == w__1.FAction.FSource) && (w__1.FAction.FReaction != ((Aow.Units.AoWE.TDiplomaticActionReaction)3)))
                                {
                                    w__1.HighlightPlayer(unchecked((sbyte)w__1.FAction.FTarget));
                                    w__1.ShowPlayerInformation(unchecked((sbyte)w__1.FAction.FTarget));
                                    w__1.RNewActionPnl.Visible = false;
                                    w__1.IncommingInfoPnl.Visible = true;
                                    var call__15 = w__1.FAction.FMessage.Text;
                                    w__1.IIMemo.Items.Text = call__15;
                                    switch (w__1.FAction.FActionType)
                                    {
                                        case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                                            w__1.IIIcon.CurrentImage = 8;
                                            break;
                                        case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                                            w__1.IIIcon.CurrentImage = 10;
                                            break;
                                        case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                                            w__1.IIIcon.CurrentImage = 47;
                                            break;
                                        case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                                            w__1.IIIcon.CurrentImage = 47;
                                            break;
                                        case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                                            w__1.IIIcon.CurrentImage = 48;
                                            break;
                                    }
                                }
                                else
                                {
                                    w__1.FinishEvent();
                                    if (w__1.FAction.FReaction == ((Aow.Units.AoWE.TDiplomaticActionReaction)3))
                                    {
                                        w__1.ShowPlayerActions(unchecked((sbyte)w__1.FAction.FTarget));
                                    }
                                    else
                                    {
                                        w__1.ShowPlayerActions(unchecked((sbyte)w__1.FAction.FSource));
                                    }
                                }
                            }

                            break;
                        case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWar:
                        case Aow.Units.AoWE.TDiplomaticActionType.daBreakAlliance:
                            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == w__1.FAction.FSource)
                            {
                                w__1.FinishEvent();
                                w__1.ShowPlayerActions(unchecked((sbyte)w__1.FAction.FTarget));
                            }
                            else
                            {
                                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == w__1.FAction.FTarget)
                                {
                                    w__1.HighlightPlayer(unchecked((sbyte)w__1.FAction.FSource));
                                    w__1.ShowPlayerInformation(unchecked((sbyte)w__1.FAction.FSource));
                                    w__1.RNewActionPnl.Visible = false;
                                    w__1.IncommingInfoPnl.Visible = true;
                                    var call__16 = w__1.FAction.FMessage.Text;
                                    w__1.IIMemo.Items.Text = call__16;
                                    if (w__1.FAction.FActionType == Aow.Units.AoWE.TDiplomaticActionType.daDeclareWar)
                                    {
                                        w__1.IIIcon.CurrentImage = 9;
                                    }
                                    else
                                    {
                                        if (w__1.FAction.FActionType == Aow.Units.AoWE.TDiplomaticActionType.daBreakAlliance)
                                        {
                                            w__1.IIIcon.CurrentImage = 8;
                                        }
                                    }
                                }
                                else
                                {
                                    w__1.FinishEvent();
                                    w__1.ShowPlayerActions(unchecked((sbyte)w__1.FAction.FSource));
                                }
                            }

                            break;
                        case Aow.Units.AoWE.TDiplomaticActionType.daThreatenToAttack:
                            w__1.HighlightPlayer(unchecked((sbyte)w__1.FAction.FSource));
                            w__1.ShowPlayerInformation(unchecked((sbyte)w__1.FAction.FSource));
                            w__1.RNewActionPnl.Visible = false;
                            w__1.IncommingInfoPnl.Visible = true;
                            var call__17 = w__1.FAction.FMessage.Text;
                            w__1.IIMemo.Items.Text = call__17;
                            w__1.IIIcon.CurrentImage = 46;
                            break;
                    }
                }
                else
                {
                    w__1.FinishDialog();
                }
            }
        }

        // E3EventDialogs.pas:751
        public static void ShowInformationBook(Aow.Units.AoWWorldMap.TWorldMap Dialog, Aow.Units.EventLog.TShowBookEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            {
                var w__18 = Dialog;
                w__18.FOnBookClosed = Aow.Rtl.Rtl.RebindDelegate<Aow.Rtl.TNotifyEvent>(Done);
                w__18.FBookContext = Event;
                if (w__18.FBookContext != null)
                {
                    w__18.FBookContext.AddRef();
                }

                w__18.LabelL.Text = Aow.Units.AoWE.AoWE.AoWHSMap.FMapName;
                w__18.MemoL.Items.Clear();
                w__18.MemoL.Items.AddStrings(Aow.Units.AoWE.AoWE.AoWHSMap.FDescription);
                var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.MapSettingsRStr);
                w__18.LabelR.Text = call__19;
                w__18.MemoR.Items.Clear();
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FVictoryCondition == ((Aow.Units.AoWE.TVictoryCondition)0))
                {
                    var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConquerTheMapRStr);
                    var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ObjectiveXRStr);
                    var call__22 = Aow.Units.AoWE.AoWE.Format(call__21, [call__20]);
                    w__18.MemoR.Items.Add(call__22);
                }
                else
                {
                    var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TVictoryConditionRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FVictoryCondition]);
                    var call__24 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ObjectiveXRStr);
                    var call__25 = Aow.Units.AoWE.AoWE.Format(call__24, [call__23]);
                    w__18.MemoR.Items.Add(call__25);
                }

                if (Aow.Units.AoWE.AoWE.AoWHSMap.FDayLimit > 0)
                {
                    var call__26 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.DayLimitXRStr);
                    var call__27 = Aow.Units.AoWE.AoWE.Format(call__26, [Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FDayLimit)]);
                    w__18.MemoR.Items.Add(call__27);
                }

                if (Aow.Units.AoWE.AoWE.AoWHSMap.FAlliedVictory)
                {
                    var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnabledRStr);
                    var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlliedVictoryXRStr);
                    var call__30 = Aow.Units.AoWE.AoWE.Format(call__29, [call__28]);
                    w__18.MemoR.Items.Add(call__30);
                }
                else
                {
                    var call__31 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DisabledRStr);
                    var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlliedVictoryXRStr);
                    var call__33 = Aow.Units.AoWE.AoWE.Format(call__32, [call__31]);
                    w__18.MemoR.Items.Add(call__33);
                }

                var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount();
                var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PlayersOnMapXRStr);
                var call__36 = Aow.Units.AoWE.AoWE.Format(call__35, [Aow.Rtl.Rtl.IntToStr(call__34 - 1)]);
                w__18.MemoR.Items.Add(call__36);
                var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MaximumSphereLevelXRStr);
                var call__38 = Aow.Units.AoWE.AoWE.Format(call__37, [Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FSphereCount)]);
                w__18.MemoR.Items.Add(call__38);
                var call__39 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MaximumPlayerHeroesXRStr);
                var call__40 = Aow.Units.AoWE.AoWE.Format(call__39, [Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWE.AoWE.AoWHSMap.FHeroCountMax)]);
                w__18.MemoR.Items.Add(call__40);
                if ((Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType == ((Aow.Units.AoWE.TPlayerConnectionType)0)) && Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType)
                {
                    var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerConnectionTypeRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType]);
                    var call__42 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HotSeatRStr);
                    var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GametypeXRStr);
                    var call__44 = Aow.Units.AoWE.AoWE.Format(call__43, [(call__41 + AnsiString.FromBytes(47)) + call__42]);
                    w__18.MemoR.Items.Add(call__44);
                }
                else
                {
                    var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerConnectionTypeRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType]);
                    var call__46 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GametypeXRStr);
                    var call__47 = Aow.Units.AoWE.AoWE.Format(call__46, [call__45]);
                    w__18.MemoR.Items.Add(call__47);
                }

                var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayModeRStr[(int)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode]);
                var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnModeXRStr);
                var call__50 = Aow.Units.AoWE.AoWE.Format(call__49, [call__48]);
                w__18.MemoR.Items.Add(call__50);
                w__18.PicL.Visible = false;
                w__18.PicR.Visible = false;
                w__18.MemoL.Visible = true;
                w__18.MemoR.Visible = true;
                w__18.Back.Visible = false;
                w__18.Cancel.Visible = false;
                w__18.Next.Visible = false;
                w__18.BookWin.Visible = true;
                if (w__18.BookWin.FBackSurface != null)
                {
                    w__18.BookWin.FBackSurface.Cls(w__18.BookWin.FMaskColor);
                }

                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            }
        }

        // E3EventDialogs.pas:170
        public static void ShowCombatType(Aow.Units.TCFCDlg.TTCFCDialog Dialog, Aow.Units.AoWE.TCombatTypeRequestEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            {
                var w__51 = Dialog;
                if (Aow.Units.E2Globals.E2Globals.DialogWin.FSkipCombatChoice)
                {
                    if (w__51.TCchk.Checked)
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
                    w__51.FRequest = Event;
                    w__51.FRequest.AddRef();
                    w__51.FOnDone = Aow.Rtl.Rtl.RebindDelegate<Aow.Rtl.TNotifyEvent>(Done);
                    Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
                    if (w__51.TCchk.Checked)
                    {
                        Event.Select((Aow.Units.AoWE.TCombatType)2);
                    }
                    else
                    {
                        Event.Select((Aow.Units.AoWE.TCombatType)1);
                    }

                    var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(w__51.FRequest.FPlayer);
                    if (call__52.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        w__51.CancelBtn.Visible = true;
                        w__51.FightBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                    }
                    else
                    {
                        w__51.CancelBtn.Visible = false;
                        w__51.FightBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                    }

                    w__51.Dlg.ReAlign();
                    w__51.Dlg.Visible = true;
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(61, true, true);
                }
            }
        }

        // E3EventDialogs.pas:786
        public static void ShowSMTPEvent(Aow.Units.AoWSMTPDlg.TSMTPDlg Dialog, Aow.Units.AoWE.TSMTPEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            {
                var w__53 = Dialog;
                w__53.ResetProgress();
                w__53.FDone = Done;
                w__53.FEvent = Event;
                {
                    var w__54 = w__53.FEvent;
                    w__54.AddRef();
                }

                var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RecipientXRStr);
                var call__56 = Aow.Units.AoWE.AoWE.Format(call__55, [Event.FToAddress]);
                w__53.RecipientLbl.Text = call__56;
                try
                {
                    w__53.IMSSMTP.FMessage = w__53.IMSMessage;
                    var call__57 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSMTPServer();
                    w__53.IMSSMTP.FHost = call__57;
                    w__53.IMSSMTP.WinsockLoaded = true;
                    {
                        var w__58 = w__53.IMSMessage;
                        w__58.FEncode = false;
                        w__58.FIncludeHeader = false;
                        w__58.FHeader.FFrom = Event.FFromAddress;
                        w__58.FRecipients.Clear();
                        Dialog.IMSMessage.FRecipients.AddAddress(Event.FToAddress, (AnsiString)"");
                    }

                    w__53.IMSMessage.FSubject = Event.FSubject;
                    w__53.IMSMessage.SetBody(Event.FMessage);
                    w__53.IMSMessage.FAttachments.Clear();
                    w__53.IMSMessage.FAttachments.AddFile(Event.FFileName);
                    w__53.StartSending();
                    w__53.Dlg.Visible = true;
                }
                catch (System.Exception)
                {
                    w__53.FEvent.FSuccess = false;
                    w__53.FFinished = true;
                    w__53.Dlg.Visible = true;
                }
            }
        }

        // { $45B04C }
        public static Aow.Units.AoWSendEmailGameDlg.TSendEmailGameDlg SendEmailGameDlg;
        public static Aow.Units.AoWPasswordDlg.TPasswordDlg PasswordDlg;
        public static Aow.Units.AoWArmyEventDlg.TArmyEventDlg ArmyEventDlg;
        public static Aow.Units.AoWCityRebelDlg.TCityRebelDlg CityRebelDlg;
        public static Aow.Units.AoWCityJoinDlg.TCityJoinDlg CityJoinDlg;
        public static Aow.Units.AoWPartyJoin.TPartyJoinDlg PartyJoinDlg;
        public static Aow.Units.AoWHeroJoinDlg.THeroJoinDlg HeroJoinDlg;
        public static Aow.Units.AoWMsgReceiveDlg.TMsgReceiveDlg MsgReceiveDlg;
        // { $45B278 }
        public static Aow.Units.AoWHeroUpgradeDlg.THeroUpgradeDlg HeroUpgradeDlg;
        public static Aow.Units.AoWHeroInfoDlg.THeroInfoDlg HeroInfoDlg;
        public static Aow.Units.AoWWizardsTowerDlg.TWizardsTowerDlg WizardsTowerDlg;
        public static Aow.Units.AoWArenaDlg.TArenaDlg ArenaDlg;
        public static Aow.Units.AoWItemsFoundDlg.TItemsFoundDlg ItemsFoundDlg;
        public static Aow.Units.FastCombatWin.TFastCombatWindow FastCombatWindow;
        // { $45B1B0 }
        public static Aow.Units.AoWNewTurnDialog.TNewTurnDlg NewTurnDlg;
        // { $45B1E8 }
        public static Aow.Units.TCFCDlg.TTCFCDialog TCFCDialog;
        // E3EventDialogs.pas:33
        // { $45B1F0 }
        public static void OriginalAssert(Aow.Rtl.AnsiString Message, Aow.Rtl.AnsiString Filename, int LineNumber)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }
}
