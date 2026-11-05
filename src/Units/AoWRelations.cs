// Translated from AoWRelations.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;

namespace Aow.Units.AoWRelations
{
    // diplomatic actions, player indices, race indices, controls
    public partial class TRelWindow : TDataModule
    {
        public TRelWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow RelWin;
        public TAOWImage RShield;
        public TAOWImage LShield;
        public TAOWButton MExitBtn;
        public TAOWPanel RelPnl;
        public TAOWPanel RIncomingPnl;
        public TAOWButton MAcceptBtn;
        public TAOWButton MRejectBtn;
        public TAOWVScrollBar MScroll;
        public TAOWImage P1I;
        public TAOWImage P2I;
        public TAOWImage P3I;
        public TAOWImage P4I;
        public TAOWImage P5I;
        public TAOWImage P6I;
        public TAOWImage P7I;
        public TAOWImage P8I;
        public TAOWImage P9I;
        public TAOWImage P10I;
        public TAOWImage P11I;
        public TAOWImage P12I;
        public TAOWImage R1I;
        public TAOWImage R2I;
        public TAOWImage R3I;
        public TAOWImage R4I;
        public TAOWImage R5I;
        public TAOWImage R6I;
        public TAOWImage R7I;
        public TAOWImage R8I;
        public TAOWImage R9I;
        public TAOWImage R10I;
        public TAOWImage R11I;
        public TAOWImage R12I;
        public TAOWImage FaceShadow;
        public TAOWMemo MMemo;
        public TAOWLabel MBGoldLbl;
        public TAOWImage MIcon;
        public TAOWPanel RMsgEditPnl;
        public TAOWPanel RSBribePnl;
        public TAOWVScrollBar SScroll;
        public TAOWMemo SMemo;
        public TAOWHScrollBar SBGoldSlid;
        public TAOWPanel ActionPnl;
        public TAOWPanel LInfoPlayerPnl;
        public TAOWPanel RNewActionPnl;
        public TAOWButton AExit;
        public TAOWLabel APlayer;
        public TAOWPanel LInfoRacePnl;
        public TAOWLabel ARace2;
        public TAOWImage AAlignment;
        public TAOWLabel AAlignmentLbl;
        public TAOWCheckBox P1CB;
        public TAOWCheckBox P2CB;
        public TAOWCheckBox P3CB;
        public TAOWCheckBox P4CB;
        public TAOWCheckBox P5CB;
        public TAOWCheckBox P6CB;
        public TAOWCheckBox P7CB;
        public TAOWCheckBox P8CB;
        public TAOWCheckBox P9CB;
        public TAOWCheckBox P10CB;
        public TAOWCheckBox P11CB;
        public TAOWCheckBox P12CB;
        public TAOWImage FaceFr;
        public TAOWCheckBox R1CB;
        public TAOWCheckBox R2CB;
        public TAOWCheckBox R3CB;
        public TAOWCheckBox R4CB;
        public TAOWCheckBox R5CB;
        public TAOWCheckBox R6CB;
        public TAOWCheckBox R7CB;
        public TAOWCheckBox R8CB;
        public TAOWCheckBox R9CB;
        public TAOWCheckBox R10CB;
        public TAOWCheckBox R11CB;
        public TAOWCheckBox R12CB;
        public TAOWImage PlyrGlow;
        public TAOWLabel NoContact;
        public TAOWButton ASelect;
        public TAOWListBox ActionsLB;
        public TAOWVScrollBar ActionSB;
        public TAOWPanel LBActionsPnl;
        public TAOWLabel ActionLbl;
        public TAOWImage PIRelIcn;
        public TAOWLabel PIRelLbl;
        public TAOWPanel PIRelPnl;
        public TAOWLabel PIRelV;
        public TAOWImage APlyrIcon;
        public TAOWLabel APlyrType;
        public TAOWImage PlStatIcn;
        public TAOWLabel PlStatLbl;
        public TAOWPanel PIStatPnl;
        public TAOWLabel PlStatV;
        public TAOWPanel PTypePnl;
        public TAOWPanel PIFrame;
        public TAOWPanel RIFrame;
        public TAOWPanel RIRelPnl;
        public TAOWPanel RAlignPnl;
        public TAOWImage RlRelIcn;
        public TAOWLabel RIRelLbl;
        public TAOWLabel RlRelV;
        public TAOWLabel MBGoldBribeV;
        public TAOWImage MBGoldIcn;
        public TAOWPanel MBGoldPnl;
        public TAOWPanel MBManaPnl;
        public TAOWLabel MBManaLbl;
        public TAOWLabel MBManaBribeV;
        public TAOWImage MBManaIcn;
        public TAOWLabel MsgLbl;
        public TAOWButton MsgSendBtn;
        public TAOWButton MsgCancelBtn;
        public TAoWFrame RSBribe2;
        public TAOWLabel SBGoldV;
        public TAOWImage SBGoldIcn;
        public TAOWPanel SBGold;
        public TAOWPanel SBMana;
        public TAOWLabel SBManaV;
        public TAOWImage SBManaIcn;
        public TAoWFrame RSBSep1;
        public TAOWLabel SBribeBtn;
        public TAOWHScrollBar SBManaSlid;
        public TAOWImage SBIcon;
        public TAOWButton RSBSendBtn;
        public TAOWButton RBSCanlBtn;
        public TAOWImage StatusGlow;
        public TAOWLabel BribeHeaderLbl;
        public TAOWPanel RMsgRecPnl;
        public TAOWMemo RMMemo;
        public TAOWLabel RMHeaderLbl;
        public TAOWButton RMReplyBtn;
        public TAOWButton RMCancelBtn;
        public TAOWButton RMExitBtn;
        public TAOWPanel IncommingInfoPnl;
        public TAOWButton IIOkBtn;
        public TAOWButton IIExitBtn;
        public TAOWVScrollBar IRScroll;
        public TAOWMemo IIMemo;
        public TAOWImage IIIcon;
        public TIvTranslator Translator;
        public TAOWLabel OfTheLbl;
        public TAOWLabel PRaceLbl;
        public int FSelectedIndex;
        public TDiplomaticAction FAction;
        public TDiplomaticActionList FActions;
        public sbyte FInfoPlayer;
        public sbyte FInfoRace;
        public sbyte FSelectedPlayer;
        public sbyte FSelectedRace;
        public TIntegerList FPlayers;
        public TIntegerList FRaces;
        public TList FControls;
        public TExecuteDoneEvent FDone;
        public TDiplomaticActionEventLog FEvent;
        public int FGlowIntensity;
        public bool FGlowIncreasing;
        // AoWRelations.pas:221
        public void FillPlayers()
        {
            int I = default;
            FPlayers.Clear();
            {
                I = 1;
                int end__1 = AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__3 = call__2.GetFirstDaySeen((sbyte)I);
                    bool bool__4 = call__3 != -1;
                    if (bool__4)
                    {
                        var call__5 = AoWHSMap.FPlayers.GetPlayers(I);
                        bool__4 = call__5.FStatus == TPlayerStatus.psPlaying;
                    }

                    if (bool__4)
                    {
                        FPlayers.Add(I);
                    }
                }
            }

            {
                I = 1;
                byte end__6 = 12;
                for (; I <= end__6; I += 1)
                {
                    if (I <= FPlayers.Count)
                    {
                        var call__7 = FControls[I + 23];
                        ((TAOWCheckBox)call__7).Visible = true;
                        var call__8 = FControls[I + 23];
                        ((TAOWCheckBox)call__8).ToolTipText.Clear();
                        var call__9 = FPlayers[I - 1];
                        var call__10 = AoWHSMap.FPlayers.GetPlayers(call__9);
                        var call__11 = call__10.GetName();
                        var call__12 = FControls[I + 23];
                        ((TAOWCheckBox)call__12).ToolTipText.Add(call__11);
                    }
                    else
                    {
                        var call__13 = FControls[I + 23];
                        ((TAOWCheckBox)call__13).Visible = false;
                    }
                }
            }

            var call__14 = FPlayers.IndexOf(AoWHSMap.FSeatedPlayer);
            PlyrGlow.Alignment.LeftOffset = call__14 * 51 - 37 + 51;
            PlyrGlow.FAligned = false;
        }

        // AoWRelations.pas:241
        public void ShowRelations(int Player)
        {
            int I = default;
            int Index = default;
            ClearActions();
            ResetChecks();
            General.Manager.OverlayPriority = 49;
            FRaces.Clear();
            {
                I = 0;
                int end__15 = AoWHSMap.FRaces.GetCount() - 1;
                for (; I <= end__15; I += 1)
                {
                    var call__16 = AoWHSMap.FRaces.GetRace(I);
                    if (call__16.FEnabled)
                    {
                        FRaces.Add(I);
                    }
                }
            }

            {
                I = 0;
                byte end__17 = 11;
                for (; I <= end__17; I += 1)
                {
                    if (I < FRaces.Count)
                    {
                        var call__18 = FControls[I + 36];
                        ((TAOWCheckBox)call__18).Visible = true;
                        var call__19 = FControls[I + 36];
                        ((TAOWCheckBox)call__19).ToolTipText.Clear();
                        var call__20 = FRaces[I];
                        var call__21 = AoWHSMap.FRaces.GetRace(call__20);
                        var call__22 = TranslateRStr(RaceNamesPluralRStr[call__21.FRace]);
                        var call__23 = FControls[I + 36];
                        ((TAOWCheckBox)call__23).ToolTipText.Add(call__22);
                    }
                    else
                    {
                        var call__24 = FControls[I + 36];
                        ((TAOWCheckBox)call__24).Visible = false;
                    }
                }
            }

            FillPlayers();
            FGlowIntensity = 50;
            FGlowIncreasing = false;
            RelWin.Visible = true;
            var call__25 = FControls[Player + 23];
            ((TAOWCheckBox)call__25).Checked = true;
            var call__26 = FPlayers.IndexOf(AoWHSMap.FSeatedPlayer);
            Index = call__26;
            var call__27 = FControls[Index + 24];
            ((TAOWCheckBox)call__27).Checked = true;
            var call__28 = FControls[Index + 24];
            P1CBChange((TAOWCheckBox)call__28);
            AoWHSMap.FTutorialControl.ShowMessage(65, true, true);
        }

        // AoWRelations.pas:270
        public void ResetChecks()
        {
            int I = default;
            TAOWCheckBox Check = default;
            FSelectedIndex = -1;
            RelPnl.FAligned = false;
            RelWin.FDrawn = false;
            PlyrGlow.Alignment.TopOffset = 10;
            {
                I = 24;
                byte end__29 = 35;
                for (; I <= end__29; I += 1)
                {
                    var call__30 = FControls[I];
                    Check = (TAOWCheckBox)call__30;
                    Check.Checked = false;
                    Check.Alignment.TopOffset = 10;
                }
            }

            {
                I = 36;
                byte end__31 = 47;
                for (; I <= end__31; I += 1)
                {
                    var call__32 = FControls[I];
                    Check = (TAOWCheckBox)call__32;
                    Check.Checked = false;
                    Check.Alignment.TopOffset = 185;
                }
            }
        }

        // AoWRelations.pas:293
        public void ShowPlayerActions(sbyte Player)
        {
            int Selected = default;
            int I = default;
            ClearActions();
            HighlightPlayer(Player);
            ShowPlayerInformation(Player);
            Selected = ActionsLB.Selected;
            var call__33 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            call__33.ListDiplomaticActions(Player, ref FActions);
            {
                I = 0;
                int end__34 = FActions.GetCount() - 1;
                for (; I <= end__34; I += 1)
                {
                    var call__35 = FActions.GetDiplomaticActions(I);
                    var call__36 = call__35.GetName(AoWHSMap.FSeatedPlayer);
                    var call__37 = FActions.GetDiplomaticActions(I);
                    ActionsLB.Items.AddObject(call__36, call__37);
                }
            }

            ActionsLB.Selected = Selected;
            ActionsLBChange(this);
            ActionPnl.Visible = true;
        }

        // AoWRelations.pas:309
        public void ShowPlayerInformation(sbyte Player)
        {
            TDiplomaticRelation Relation = default;
            FInfoPlayer = Player;
            FInfoRace = -1;
            FaceShadow.Visible = true;
            NoContact.Visible = false;
            PlStatV.Visible = true;
            PlStatIcn.Visible = true;
            PlStatLbl.Visible = true;
            LInfoPlayerPnl.Visible = true;
            LInfoRacePnl.Visible = false;
            var call__38 = AoWHSMap.FPlayers.GetPlayers(Player);
            LShield.CurrentImage = call__38.FPlayerRace + 26;
            LShield.Visible = true;
            RShield.CurrentImage = LShield.CurrentImage;
            RShield.Visible = true;
            var call__39 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__40 = call__39.GetFirstDaySeen(Player);
            NoContact.Visible = call__40 == -1;
            APlayer.Visible = !NoContact.Visible;
            OfTheLbl.Visible = APlayer.Visible;
            PRaceLbl.Visible = APlayer.Visible;
            var call__41 = AoWHSMap.FPlayers.GetPlayers(Player);
            var call__42 = call__41.GetFirstDaySeen(AoWHSMap.FSeatedPlayer);
            if (call__42 != -1)
            {
                if (Player != AoWHSMap.FSeatedPlayer)
                {
                    var call__43 = AoWHSMap.FPlayers.GetDiplomaticRelation(Player, AoWHSMap.FSeatedPlayer);
                    Relation = call__43;
                    var call__44 = TranslateRStr(TDiplomaticRelationRStr[(int)Relation]);
                    PlStatV.Text = call__44;
                    if (Relation != TDiplomaticRelation.drUnknown)
                    {
                        PlStatIcn.CurrentImage = (int)Relation + 8;
                    }
                    else
                    {
                        PlStatIcn.CurrentImage = 50;
                    }
                }
                else
                {
                    PlStatIcn.Visible = false;
                    PlStatLbl.Visible = false;
                    PlStatV.Visible = false;
                }
            }
            else
            {
                PlStatIcn.Visible = false;
                var call__45 = TranslateRStr(NoContactRStr);
                PlStatV.Text = call__45;
            }

            var call__46 = AoWHSMap.FPlayers.GetPlayers(Player);
            var call__47 = call__46.GetName();
            APlayer.Text = call__47;
            OfTheLbl.Text = AoWPInfoWin.AoWPInfoWin.PInfoW.LeadL.Text;
            var call__48 = AoWHSMap.FPlayers.GetPlayers(Player);
            var call__49 = TranslateRStr(RaceNamesPluralRStr[call__48.FPlayerRace]);
            PRaceLbl.Text = call__49;
            var call__50 = AoWHSMap.FPlayers.GetPlayers(Player);
            if (TSet<TPlayerType>.Of(TPlayerType.ptHuman, TPlayerType.ptCPU1, TPlayerType.ptCPU2, TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__50.FPlayerType))
            {
                var call__51 = AoWHSMap.FPlayers.GetPlayers(Player);
                switch (call__51.FPlayerType)
                {
                    case TPlayerType.ptCPU1:
                        APlyrIcon.CurrentImage = 1;
                        break;
                    case TPlayerType.ptCPU2:
                        APlyrIcon.CurrentImage = 2;
                        break;
                    case TPlayerType.ptCPU3:
                        APlyrIcon.CurrentImage = 3;
                        break;
                    case TPlayerType.ptCPU4:
                        APlyrIcon.CurrentImage = 4;
                        break;
                    case TPlayerType.ptCPU5:
                        APlyrIcon.CurrentImage = 5;
                        break;
                    default:
                        APlyrIcon.CurrentImage = 0;
                        break;
                }

                APlyrIcon.Visible = true;
            }
            else
            {
                APlyrIcon.Visible = false;
            }

            var call__52 = AoWHSMap.FPlayers.GetPlayers(Player);
            var call__53 = TranslateRStr(TPlayerTypeRStr[(int)call__52.FPlayerType]);
            APlyrType.Text = call__53;
        }

        // AoWRelations.pas:370
        public void HighlightPlayer(sbyte Player)
        {
            int I = default;
            TAOWCheckBox Check = default;
            ResetChecks();
            FSelectedPlayer = Player;
            FSelectedRace = -1;
            var call__54 = FPlayers.IndexOf(Player);
            FSelectedIndex = call__54;
            if (FSelectedIndex != -1)
            {
                var call__55 = FControls[FSelectedIndex + 24];
                Check = (TAOWCheckBox)call__55;
                Check.Alignment.TopOffset = 20;
                Check.Checked = true;
                if (AoWHSMap.FSeatedPlayer == Player)
                {
                    PlyrGlow.Alignment.TopOffset = Check.Alignment.TopOffset;
                }
            }

            {
                I = 0;
                byte end__56 = 11;
                for (; I <= end__56; I += 1)
                {
                    if (I < FPlayers.Count)
                    {
                        var call__57 = FControls[I];
                        ((TAOWImage)call__57).Visible = true;
                        var call__58 = FPlayers[I];
                        if (call__58 == Player)
                        {
                            var call__59 = FControls[I];
                            ((TAOWImage)call__59).CurrentImage = 2;
                        }
                        else
                        {
                            if (I > FRaces.Count)
                            {
                                var call__60 = FControls[I];
                                ((TAOWImage)call__60).CurrentImage = 0;
                            }
                            else
                            {
                                var call__61 = FControls[I];
                                ((TAOWImage)call__61).CurrentImage = 1;
                            }
                        }
                    }
                    else
                    {
                        var call__62 = FControls[I];
                        ((TAOWImage)call__62).CurrentImage = 2;
                    }

                    var call__63 = FControls[I];
                    switch (((TAOWImage)call__63).CurrentImage)
                    {
                        case 1:
                            var call__64 = FControls[I];
                            ((TAOWImage)call__64).ToolTipText.Clear();
                            if (FSelectedPlayer != -1)
                            {
                                var call__65 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                                var call__66 = call__65.GetFirstDaySeen(FSelectedPlayer);
                                if (call__66 != -1)
                                {
                                    var call__67 = FPlayers[I];
                                    var call__68 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                                    var call__69 = call__68.GetFirstDaySeen((sbyte)call__67);
                                    if (call__69 != -1)
                                    {
                                        var call__70 = FPlayers[I];
                                        var call__71 = AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer);
                                        var call__72 = call__71.GetFirstDaySeen((sbyte)call__70);
                                        if (call__72 != -1)
                                        {
                                            var call__73 = FPlayers[I];
                                            var call__74 = AoWHSMap.FPlayers.GetDiplomaticRelation(FSelectedPlayer, (sbyte)call__73);
                                            var call__75 = FControls[I];
                                            ((TAOWImage)call__75).ToolTipText.Add(TDiplomaticRelationRStr[(int)call__74]);
                                        }
                                    }
                                }
                            }

                            break;
                        default:
                            var call__76 = FControls[I];
                            ((TAOWImage)call__76).ToolTipText.Clear();
                            break;
                    }
                }
            }

            {
                I = 0;
                byte end__77 = 11;
                for (; I <= end__77; I += 1)
                {
                    if (I < FRaces.Count)
                    {
                        var call__78 = FControls[I + 12];
                        ((TAOWImage)call__78).CurrentImage = 4;
                    }
                    else
                    {
                        var call__79 = FControls[I + 12];
                        ((TAOWImage)call__79).CurrentImage = 5;
                    }

                    var call__80 = FControls[I + 12];
                    switch (((TAOWImage)call__80).CurrentImage)
                    {
                        case 4:
                            var call__81 = FControls[I + 12];
                            ((TAOWImage)call__81).ToolTipText.Clear();
                            if (FSelectedPlayer != -1)
                            {
                                var call__82 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                                var call__83 = call__82.GetFirstDaySeen(FSelectedPlayer);
                                if (call__83 != -1)
                                {
                                    var call__84 = FRaces[I];
                                    var call__85 = AoWHSMap.FRaces.GetRace(call__84);
                                    var call__86 = call__85.GetPlayerRelation(FSelectedPlayer);
                                    var call__87 = TranslateRStr(TStatusRStr[(int)call__86]);
                                    var call__88 = FRaces[I];
                                    var call__89 = AoWHSMap.FRaces.GetRace(call__88);
                                    var call__90 = call__89.GetPlayerRelationValue(FSelectedPlayer);
                                    var call__91 = FControls[12 + I];
                                    ((TAOWImage)call__91).ToolTipText.Add(call__87 + (AnsiString)" (" + IntToStr(call__90) + AnsiString.FromBytes(41));
                                }
                            }

                            break;
                        default:
                            var call__92 = FControls[I + 12];
                            ((TAOWImage)call__92).ToolTipText.Clear();
                            break;
                    }
                }
            }
        }

        // AoWRelations.pas:427
        public void ShowRaceInformation(byte Race)
        {
            FInfoPlayer = -1;
            FInfoRace = (sbyte)Race;
            LShield.Visible = false;
            RShield.Visible = false;
            FaceShadow.Visible = true;
            ActionPnl.Visible = true;
            LInfoPlayerPnl.Visible = false;
            LInfoRacePnl.Visible = true;
            var call__93 = AoWHSMap.FRaces.GetRace((sbyte)Race);
            var call__94 = call__93.GetPlayerRelation(AoWHSMap.FSeatedPlayer);
            RlRelIcn.CurrentImage = (int)call__94 + 11;
            var call__95 = AoWHSMap.FRaces.GetRace((sbyte)Race);
            var call__96 = call__95.GetPlayerRelation(AoWHSMap.FSeatedPlayer);
            var call__97 = TranslateRStr(TStatusRStr[(int)call__96]);
            RlRelV.Text = call__97;
            var call__98 = TranslateRStr(RaceNamesPluralRStr[(sbyte)Race]);
            ARace2.Text = call__98;
            switch (Race)
            {
                case >= 0 and <= 3:
                    var call__99 = TranslateRStr(NeutralRStr);
                    var call__100 = TranslateRStr(AlignmentXRStr);
                    var call__101 = AoWE.AoWE.Format(call__100, [call__99]);
                    AAlignmentLbl.Text = call__101;
                    AAlignment.CurrentImage = 33;
                    break;
                case >= 4 and <= 7:
                    var call__102 = TranslateRStr(GoodRStr);
                    var call__103 = TranslateRStr(AlignmentXRStr);
                    var call__104 = AoWE.AoWE.Format(call__103, [call__102]);
                    AAlignmentLbl.Text = call__104;
                    AAlignment.CurrentImage = 31;
                    break;
                case >= 8 and <= 11:
                    var call__105 = TranslateRStr(EvilRStr);
                    var call__106 = TranslateRStr(AlignmentXRStr);
                    var call__107 = AoWE.AoWE.Format(call__106, [call__105]);
                    AAlignmentLbl.Text = call__107;
                    AAlignment.CurrentImage = 35;
                    break;
            }
        }

        // AoWRelations.pas:456
        public void HighlightRace(sbyte Race)
        {
            int I = default;
            int Index = default;
            TAOWCheckBox Check = default;
            ResetChecks();
            FSelectedPlayer = -1;
            FSelectedRace = Race;
            var call__108 = FRaces.IndexOf(Race);
            Index = call__108;
            var call__109 = FControls[Index + 36];
            Check = (TAOWCheckBox)call__109;
            Check.Alignment.TopOffset = 175;
            Check.Checked = true;
            FSelectedIndex = Check.Tag;
            {
                I = 0;
                byte end__110 = 11;
                for (; I <= end__110; I += 1)
                {
                    if (I < FPlayers.Count)
                    {
                        var call__111 = FControls[I];
                        ((TAOWImage)call__111).CurrentImage = 1;
                    }
                    else
                    {
                        var call__112 = FControls[I];
                        ((TAOWImage)call__112).CurrentImage = 2;
                    }

                    var call__113 = FControls[I];
                    switch (((TAOWImage)call__113).CurrentImage)
                    {
                        case 1:
                            var call__114 = FControls[I];
                            ((TAOWImage)call__114).ToolTipText.Clear();
                            var call__115 = FPlayers[I];
                            var call__116 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                            var call__117 = call__116.GetFirstDaySeen((sbyte)call__115);
                            if (call__117 != -1)
                            {
                                var call__118 = FPlayers[I];
                                var call__119 = AoWHSMap.FRaces.GetRace(FSelectedRace);
                                var call__120 = call__119.GetPlayerRelation((sbyte)call__118);
                                var call__121 = TranslateRStr(TStatusRStr[(int)call__120]);
                                var call__122 = FPlayers[I];
                                var call__123 = AoWHSMap.FRaces.GetRace(FSelectedRace);
                                var call__124 = call__123.GetPlayerRelationValue((sbyte)call__122);
                                var call__125 = FControls[I];
                                ((TAOWImage)call__125).ToolTipText.Add(call__121 + (AnsiString)" (" + IntToStr(call__124) + AnsiString.FromBytes(41));
                            }

                            break;
                        default:
                            var call__126 = FControls[I];
                            ((TAOWImage)call__126).ToolTipText.Clear();
                            break;
                    }
                }
            }

            {
                I = 0;
                byte end__127 = 11;
                for (; I <= end__127; I += 1)
                {
                    var call__128 = FControls[I + 12];
                    ((TAOWImage)call__128).CurrentImage = 5;
                    var call__129 = FControls[I + 12];
                    ((TAOWImage)call__129).ToolTipText.Clear();
                }
            }
        }

        // AoWRelations.pas:490
        public void SetAction(TDiplomaticAction Action)
        {
            if (Action != FAction)
            {
                if (FAction != null)
                {
                    FAction.Release();
                }

                FAction = Action;
                if (FAction != null)
                {
                    FAction.AddRef();
                }
            }
        }

        // AoWRelations.pas:500
        public void FinishEvent()
        {
            TExecuteDoneEvent Done = default;
            TDiplomaticActionEventLog Pending = default;
            TDiplomaticActionEventLog Event = default;
            Pending = FEvent;
            if (Pending != null)
            {
                Done = FDone;
                Event = Pending;
                FDone = null;
                FEvent = null;
                if (Done != null)
                {
                    Done(Event);
                }

                if (Event != null)
                {
                    Event.Release();
                }
            }
        }

        // AoWRelations.pas:516
        public void FinishDialog()
        {
            SetAction(null);
            General.Manager.OverlayPriority = 0;
            FinishEvent();
            AoWHSMap.FTutorialControl.ShowMessage(71, true, true);
        }

        // AoWRelations.pas:524
        public void ClearActions()
        {
            SetAction(null);
            RNewActionPnl.Visible = true;
            RIncomingPnl.Visible = false;
            RMsgEditPnl.Visible = false;
            RSBribePnl.Visible = false;
            RMsgRecPnl.Visible = false;
            IncommingInfoPnl.Visible = false;
            FActions.Clear();
            ActionsLB.Items.Clear();
            SMemo.Items.Clear();
            MMemo.Items.Clear();
            ActionsLBChange(this);
        }

        // AoWRelations.pas:540
        public void RelWindowCreate()
        {
            var call__130 = new TDiplomaticActionList(null);
            FActions = call__130;
            var call__131 = new TIntegerList();
            FRaces = call__131;
            var call__132 = new TIntegerList();
            FPlayers = call__132;
            var call__133 = new TList();
            FControls = call__133;
            FControls.Add(P1I);
            FControls.Add(P2I);
            FControls.Add(P3I);
            FControls.Add(P4I);
            FControls.Add(P5I);
            FControls.Add(P6I);
            FControls.Add(P7I);
            FControls.Add(P8I);
            FControls.Add(P9I);
            FControls.Add(P10I);
            FControls.Add(P11I);
            FControls.Add(P12I);
            FControls.Add(R1I);
            FControls.Add(R2I);
            FControls.Add(R3I);
            FControls.Add(R4I);
            FControls.Add(R5I);
            FControls.Add(R6I);
            FControls.Add(R7I);
            FControls.Add(R8I);
            FControls.Add(R9I);
            FControls.Add(R10I);
            FControls.Add(R11I);
            FControls.Add(R12I);
            FControls.Add(P1CB);
            FControls.Add(P2CB);
            FControls.Add(P3CB);
            FControls.Add(P4CB);
            FControls.Add(P5CB);
            FControls.Add(P6CB);
            FControls.Add(P7CB);
            FControls.Add(P8CB);
            FControls.Add(P9CB);
            FControls.Add(P10CB);
            FControls.Add(P11CB);
            FControls.Add(P12CB);
            FControls.Add(R1CB);
            FControls.Add(R2CB);
            FControls.Add(R3CB);
            FControls.Add(R4CB);
            FControls.Add(R5CB);
            FControls.Add(R6CB);
            FControls.Add(R7CB);
            FControls.Add(R8CB);
            FControls.Add(R9CB);
            FControls.Add(R10CB);
            FControls.Add(R11CB);
            FControls.Add(R12CB);
        }

        // AoWRelations.pas:596
        public void RelWindowDestroy()
        {
            FActions.Free();
            FControls.Free();
            FPlayers.Free();
            FRaces.Free();
        }

        // AoWRelations.pas:604
        public void ExitClick()
        {
            General.PlayButtonSound(0);
            RelWin.Visible = false;
        }

        // AoWRelations.pas:610
        public void P1CBChange(TObject Sender)
        {
            TAOWCheckBox Check = default;
            Check = (TAOWCheckBox)Sender;
            if (Check.Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }

            RelPnl.FAligned = false;
            RelWin.FDrawn = false;
            if (Check.Checked)
            {
                if (FAction == null)
                {
                    var call__134 = FPlayers[Check.Tag];
                    ShowPlayerActions((sbyte)call__134);
                }
                else
                {
                    var call__135 = FPlayers[Check.Tag];
                    HighlightPlayer((sbyte)call__135);
                }
            }
        }

        // AoWRelations.pas:623
        public void R1CBChange(TObject Sender)
        {
            TAOWCheckBox Check = default;
            Check = (TAOWCheckBox)Sender;
            if (Check.Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }

            RelPnl.FAligned = false;
            if (FAction == null)
            {
                ClearActions();
                var call__136 = FRaces[Check.Tag - 20];
                HighlightRace((sbyte)call__136);
                var call__137 = FRaces[Check.Tag - 20];
                ShowRaceInformation((byte)call__137);
            }
            else
            {
                var call__138 = FRaces[Check.Tag - 20];
                HighlightRace((sbyte)call__138);
            }
        }

        // AoWRelations.pas:639
        public void P1CBPreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            sbyte Player = default;
            TRect R = default;
            TAoWComponent Component = default;
            Component = Sender;
            if (Component.Tag < FPlayers.Count)
            {
                var call__140 = FPlayers[Component.Tag];
                Player = (sbyte)call__140;
                var call__141 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__142 = call__141.GetFirstDaySeen(Player);
                if (call__142 == -1)
                {
                    var call__143 = IntGfxMod.RelWin[28];
                    call__143.ShowClipped(Component.WinLeft, Component.WinTop, Surface);
                }
                else
                {
                    var call__144 = Rect(Component.WinLeft + 1, Component.WinTop + 1, Component.WinLeft + Component.WinWidth - 1, Component.WinTop + Component.WinHeight - 1);
                    R = call__144;
                    var call__145 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__146 = call__145.GetFace();
                    if (call__146 != null)
                    {
                        var call__147 = AoWHSMap.FPlayers.GetPlayers(Player);
                        var call__148 = call__147.GetFace();
                        call__148.ShowStretched(R, Surface);
                    }
                }

                var call__149 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__150 = IntGfxMod.RelWin[call__149.FPlayerRace + 16];
                call__150.ShowClipped(Component.WinLeft, Component.WinTop, Surface);
            }
        }

        // AoWRelations.pas:659
        public void R1CBPreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TAoWComponent Control = default;
            sbyte Race = default;
            TRect R = default;
            TDiplomaticAction Action = default;
            int Modifier = default;
            Control = (TAoWComponent)Sender;
            if (Control.Tag - 20 < FRaces.Count)
            {
                var call__152 = FRaces[Control.Tag - 20];
                Race = (sbyte)call__152;
                var call__153 = Rect(Control.WinLeft + 1, Control.WinTop + 1, Control.WinLeft + Control.WinWidth - 1, Control.WinTop + Control.WinHeight - 1);
                R = call__153;
                var call__154 = AoWHSSet.FRaceResources.GetRaceResource(Race);
                var call__155 = call__154.GetFace();
                call__155.ShowStretched(R, Surface);
                Action = FAction;
                if (Action == null && RNewActionPnl.Visible && ActionsLB.Selected != -1)
                {
                    var call__156 = ActionsLB.Items.Objects[ActionsLB.Selected];
                    Action = (TDiplomaticAction)call__156;
                }

                if (Action != null)
                {
                    var call__157 = Action.GetRelationModifier(AoWHSMap.FSeatedPlayer, Race);
                    Modifier = call__157;
                    if (Modifier != 0)
                    {
                        AoWExeForms.AoWExeForms.FontModule.Age10.Color = 16777215;
                        AoWExeForms.AoWExeForms.FontModule.Age10.SetTextCentered(R.Left + (R.Right - R.Left) / 2, R.Top + 50, IntToStr(Modifier), Surface);
                    }
                }
            }
        }

        // AoWRelations.pas:689
        public void P1IDraw(TObject Sender, TSurfaceDesc Surface)
        {
            int Player = default;
            TDiplomaticRelation Relation = default;
            int Image = default;
            var component = (TAoWComponent)Sender;
            if (FPlayers.Count > component.Tag && component.Tag != FSelectedIndex)
            {
                var call__159 = FPlayers[component.Tag];
                Player = call__159;
                if (FSelectedPlayer != -1)
                {
                    var call__160 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__161 = call__160.GetFirstDaySeen(FSelectedPlayer);
                    if (call__161 != -1)
                    {
                        var call__162 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        var call__163 = call__162.GetFirstDaySeen((sbyte)Player);
                        if (call__163 != -1)
                        {
                            var call__164 = AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer);
                            var call__165 = call__164.GetFirstDaySeen((sbyte)Player);
                            if (call__165 != -1)
                            {
                                var call__166 = AoWHSMap.FPlayers.GetDiplomaticRelation(FSelectedPlayer, (sbyte)Player);
                                Relation = call__166;
                                if (Relation != TDiplomaticRelation.drUnknown)
                                {
                                    Image = (int)Relation + 8;
                                }
                                else
                                {
                                    Image = 50;
                                }

                                var call__167 = IntGfxMod.RelWin[Image];
                                call__167.Show(component.WinLeft, component.WinTop + 8, Surface);
                            }
                        }
                    }
                }
                else
                {
                    var call__168 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__169 = call__168.GetFirstDaySeen((sbyte)Player);
                    if (call__169 != -1)
                    {
                        var call__170 = AoWHSMap.FRaces.GetRace(FSelectedRace);
                        var call__171 = call__170.GetPlayerRelation((sbyte)Player);
                        var call__172 = IntGfxMod.RelWin[(int)call__171 + 11];
                        call__172.ShowClipped(component.WinLeft, component.WinTop + 8, Surface);
                    }
                }
            }
        }

        // AoWRelations.pas:715
        public void R1IDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            int RaceIndex = default;
            TAoWComponent S = default;
            S = Sender;
            if (FSelectedPlayer != -1 && S.Tag - 20 < FRaces.Count)
            {
                var call__174 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__175 = call__174.GetFirstDaySeen(FSelectedPlayer);
                if (call__175 != -1)
                {
                    var call__176 = FRaces[S.Tag - 20];
                    RaceIndex = call__176;
                    var call__177 = AoWHSMap.FRaces.GetRace(RaceIndex);
                    var call__178 = call__177.GetPlayerRelation(FSelectedPlayer);
                    var call__179 = IntGfxMod.RelWin[(int)call__178 + 11];
                    call__179.ShowClipped(S.WinLeft, S.WinTop + 15, Surface);
                }
            }
        }

        // AoWRelations.pas:723
        public void MAcceptBtnClick()
        {
            AnsiString Reason = default;
            TDiplomaticAction Action = default;
            General.PlayButtonSound(0);
            Action = FAction;
            if (Action != null)
            {
                if (Action.Accept(ref Reason))
                {
                    FinishEvent();
                    ShowPlayerActions(FAction.FSource);
                }
                else
                {
                    AoWHSMap.ShowMessage(Reason);
                }
            }
        }

        // AoWRelations.pas:739
        public void MRejectBtnClick()
        {
            AnsiString Reason = default;
            TDiplomaticAction Action = default;
            General.PlayButtonSound(0);
            Action = FAction;
            if (Action != null)
            {
                if (Action.Reject(ref Reason))
                {
                    FinishEvent();
                    ShowPlayerActions(FAction.FSource);
                }
                else
                {
                    AoWHSMap.ShowMessage(Reason);
                }
            }
        }

        // AoWRelations.pas:755
        public void PlyrGlowUpdate()
        {
            if (FGlowIncreasing)
            {
                FGlowIntensity += 1;
            }
            else
            {
                FGlowIntensity -= 1;
            }

            if (FGlowIntensity == 20)
            {
                FGlowIncreasing = true;
            }
            else
            {
                if (FGlowIntensity == 50)
                {
                    FGlowIncreasing = false;
                }
            }

            PlyrGlow.FDrawn = false;
        }

        // AoWRelations.pas:763
        public void PlyrGlowDraw(TObject Sender, TSurfaceDesc Surface)
        {
            var call__180 = IntGfxMod.RelWin[44];
            call__180.ShowBlended(PlyrGlow.WinLeft, PlyrGlow.WinTop, Surface, IntensityBlendTable.LookupTables[FGlowIntensity]);
        }

        // AoWRelations.pas:768
        public void FaceFrPreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            var call__181 = Rect(FaceFr.WinLeft + 1, FaceFr.WinTop, FaceFr.WinLeft + FaceFr.WinWidth, FaceFr.WinTop + FaceFr.WinHeight);
            R = call__181;
            if (FAction == null)
            {
                if (FSelectedPlayer != -1)
                {
                    var call__182 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__183 = call__182.GetFirstDaySeen(FSelectedPlayer);
                    if (call__183 != -1)
                    {
                        var call__184 = AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer);
                        call__184.FLeader.ShowFace(R, Surface);
                    }
                }
                else
                {
                    var call__185 = AoWHSSet.FRaceResources.GetRaceResource(FSelectedRace);
                    var call__186 = call__185.GetFace();
                    call__186.Show(FaceFr.WinLeft + 1, FaceFr.WinTop + 3, Surface);
                }
            }
            else
            {
                if (FEvent != null && FEvent.FAction.FActionType == TDiplomaticActionType.daSendMessageAsY)
                {
                    var call__187 = AoWHSMap.FPlayers.GetPlayers(FEvent.FAction.FOtherPlayer);
                    call__187.FLeader.ShowFace(R, Surface);
                }
                else
                {
                    if (FAction.FSource == AoWHSMap.FSeatedPlayer)
                    {
                        var call__188 = AoWHSMap.FPlayers.GetPlayers(FAction.FTarget);
                        call__188.FLeader.ShowFace(R, Surface);
                    }
                    else
                    {
                        if (FAction.FReaction == TDiplomaticActionReaction.darNone)
                        {
                            var call__189 = AoWHSMap.FPlayers.GetPlayers(FAction.FSource);
                            call__189.FLeader.ShowFace(R, Surface);
                        }
                        else
                        {
                            var call__190 = AoWHSMap.FPlayers.GetPlayers(FAction.FTarget);
                            call__190.FLeader.ShowFace(R, Surface);
                        }
                    }
                }
            }
        }

        // AoWRelations.pas:794
        public void SBGoldSlidChange(TObject Sender)
        {
            if (FAction != null)
            {
                FAction.FGold = SBGoldSlid.Position;
            }

            SBGoldV.Text = IntToStr(SBGoldSlid.Position);
        }

        // AoWRelations.pas:800
        public void SBManaSlidChange(TObject Sender)
        {
            if (FAction != null)
            {
                FAction.FMana = SBManaSlid.Position;
            }

            SBManaV.Text = IntToStr(SBManaSlid.Position);
        }

        // AoWRelations.pas:806
        public void RelWinHide()
        {
            FinishDialog();
        }

        // AoWRelations.pas:811
        public void ASelectClick()
        {
            AnsiString Reason = default;
            if (ActionsLB.Selected != -1)
            {
                var call__191 = ActionsLB.Items.Objects[ActionsLB.Selected];
                SetAction((TDiplomaticAction)call__191);
                switch (FAction.FActionType)
                {
                    case TDiplomaticActionType.daDeclareWar:
                    case TDiplomaticActionType.daThreatenToAttack:
                    case TDiplomaticActionType.daBreakAlliance:
                        var call__192 = FAction.Send(ref Reason);
                        if (!call__192)
                        {
                            AoWHSMap.ShowMessage(Reason);
                        }

                        break;
                    case TDiplomaticActionType.daOfferPeace:
                    case TDiplomaticActionType.daProposeAlliance:
                    case TDiplomaticActionType.daMakeTribute:
                    case TDiplomaticActionType.daBreakAllianceWithY:
                    case TDiplomaticActionType.daDeclareWarToY:
                        RNewActionPnl.Visible = false;
                        SBIcon.Visible = true;
                        RSBribePnl.Visible = true;
                        SBGoldSlid.Position = 0;
                        SBManaSlid.Position = 0;
                        var call__193 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        SBGoldSlid.Max = call__193.FGems;
                        var call__194 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        SBManaSlid.Max = call__194.FMagicControl.FMana;
                        SBManaSlidChange(null);
                        SBGoldSlidChange(null);
                        var call__195 = FAction.GetName(AoWHSMap.FSeatedPlayer);
                        BribeHeaderLbl.Text = call__195;
                        switch (FAction.FActionType)
                        {
                            case TDiplomaticActionType.daOfferPeace:
                                SBIcon.CurrentImage = 8;
                                break;
                            case TDiplomaticActionType.daProposeAlliance:
                                SBIcon.CurrentImage = 10;
                                break;
                            case TDiplomaticActionType.daMakeTribute:
                                SBIcon.CurrentImage = 48;
                                break;
                            case TDiplomaticActionType.daDeclareWarToY:
                                SBIcon.CurrentImage = 47;
                                break;
                            case TDiplomaticActionType.daBreakAllianceWithY:
                                SBIcon.CurrentImage = 47;
                                break;
                            default:
                                SBIcon.Visible = false;
                                break;
                        }

                        break;
                    case TDiplomaticActionType.daSendMessage:
                    case TDiplomaticActionType.daSendMessageAsY:
                        RNewActionPnl.Visible = false;
                        RMsgEditPnl.Visible = true;
                        break;
                }
            }
        }

        // AoWRelations.pas:851
        public void ActionsLBChange(TObject Sender)
        {
            RelPnl.FDrawn = false;
            if (ActionsLB.Selected != -1)
            {
                ASelect.ILIndexDown = 37;
                ASelect.ILIndexLit = 40;
                ASelect.ILIndexUp = 34;
            }
            else
            {
                ASelect.ILIndexDown = 46;
                ASelect.ILIndexLit = 49;
                ASelect.ILIndexUp = 43;
            }
        }

        // AoWRelations.pas:868
        public void CancelSelectedAction()
        {
            if (FEvent != null)
            {
                FinishEvent();
            }

            ShowPlayerActions(FAction.FTarget);
            RNewActionPnl.Visible = true;
            RSBribePnl.Visible = false;
            RMsgEditPnl.Visible = false;
        }

        // AoWRelations.pas:877
        public void ChangeActionMessage(TAOWMemo Sender)
        {
            if (FAction != null)
            {
                var call__196 = Sender.Items.Text;
                FAction.FMessage.Text = call__196;
            }
        }

        // AoWRelations.pas:882
        public void MsgSendActionBtn()
        {
            AnsiString Reason = default;
            sbyte Player = default;
            TDiplomaticAction Action = default;
            Action = FAction;
            if (Action != null)
            {
                if (Action.Send(ref Reason))
                {
                    Player = FAction.FTarget;
                    if (FEvent != null && FEvent.FAction.FActionType == TDiplomaticActionType.daSendMessageAsY)
                    {
                        Player = FEvent.FAction.FOtherPlayer;
                    }

                    FinishEvent();
                    ShowPlayerActions(Player);
                }
                else
                {
                    AoWHSMap.ShowMessage(Reason);
                }
            }
        }

        // AoWRelations.pas:901
        public void ReplyBtnClick()
        {
            TDiplomaticAction Action = default;
            var call__197 = AoWHSMap.FPlayers.GetPlayers(FAction.FTarget);
            var call__198 = call__197.CreateDiplomaticAction(TDiplomaticActionType.daSendMessage, FAction.FSource, -1);
            Action = call__198;
            ClearActions();
            SetAction(Action);
            RNewActionPnl.Visible = false;
            RMsgEditPnl.Visible = true;
        }

        // AoWRelations.pas:912
        public void RMCancelBtnClick()
        {
            if (FEvent != null)
            {
                FinishEvent();
            }

            ShowPlayerActions(FAction.FSource);
        }

        // AoWRelations.pas:918
        public void IIOkBtnClick()
        {
            FinishEvent();
            if (FAction.FSource == AoWHSMap.FSeatedPlayer)
            {
                ShowPlayerActions(FAction.FTarget);
            }
            else
            {
                ShowPlayerActions(FAction.FSource);
            }
        }

        // AoWRelations.pas:925
        public void RelWinKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                RelWin.Visible = false;
            }
        }
    }

    public static partial class AoWRelations
    {
    }
}
