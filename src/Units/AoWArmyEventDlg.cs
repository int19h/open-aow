// Translated from AoWArmyEventDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWArmyEventDlg
{
    // completion callback. army event. frame. visible unit indices
    public partial class TArmyEventDlg : TDataModule
    {
        public TArmyEventDlg(TComponent owner) : base(owner)
        { }

        public TAOWWindow ArmyEventWin;
        public TAoWFrame BGoldSq;
        public TAOWPanel UnitPnl;
        public TAOWLabel EventText;
        public TAOWImage DamagePic;
        public TAOWButton CloseBut;
        public TAOWImage TitleIcon;
        public TAOWButton Close;
        public TAOWLabel Title;
        public TAoWFrame BCeltL;
        public TAOWImage DecoL;
        public TAOWImage DecoR;
        public TIvTranslator Translator;
        public TExecuteDoneEvent FOnClose;
        public TArmyEventLog FEvent;
        public int FFrame;
        public TByteList FUnits;
        // AoWArmyEventDlg.pas:113
        public void DrawUnits(int X, int Y, in TSurfaceDesc SD)
        {
            int Offset = default;
            int I = default;
            var byteList = FUnits;
            Offset = X - byteList.Count * 16;
            {
                I = 0;
                int end__2 = byteList.Count - 1;
                for (; I <= end__2; I += 1)
                {
                    DrawUnit(I, I * 32 + Offset, Y, SD);
                }
            }
        }

        // AoWArmyEventDlg.pas:121
        public void DrawUnit(int Index, int X, int Y, TSurfaceDesc SD)
        {
            TArmyEventLog Event = default;
            int Frame = default;
            TP2UnitShowInfo Info = default;
            Event = FEvent;
            var call__3 = FUnits[Index];
            var call__4 = Event.FDamageValues[call__3];
            if (call__4 != 0)
            {
                // { The original staggers animation by bits of the unit object's address. }
                var call__5 = FUnits[Index];
                var call__6 = Event.FArmyView.GetUnit(call__5);
                Frame = FFrame - (((int)(((uint)ObjectToInt(call__6)) >> 3)) & 7);
                Info = default;
                Info.Frame = (int)(((uint)Frame) >> 1) - 1;
                Info.Direction = 3;
                Info.Animation = TUnitAnimation.uaHit;
                var call__7 = FUnits[Index];
                var call__8 = Event.FArmyView.GetUnit(call__7);
                call__8.ShowEx(X, Y, SD, Info);
                if (Frame == 5)
                {
                    var call__9 = FUnits[Index];
                    var call__10 = Event.FArmyView.GetUnit(call__9);
                    var call__11 = call__10.GetHitPoints();
                    if (call__11 != 0)
                    {
                        var call__12 = FUnits[Index];
                        var call__13 = Event.FArmyView.GetUnit(call__12);
                        var call__14 = call__13.GetUnitGFXResourceIndex();
                        var call__15 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__14);
                        call__15.FSFXLibraries[3].Play(0);
                    }
                    else
                    {
                        var call__16 = FUnits[Index];
                        var call__17 = Event.FArmyView.GetUnit(call__16);
                        var call__18 = call__17.GetUnitGFXResourceIndex();
                        var call__19 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__18);
                        call__19.FSFXLibraries[4].Play(0);
                    }
                }

                if (Frame > 5)
                {
                    var call__20 = FUnits[Index];
                    var call__21 = Event.FDamageValues[call__20];
                    var call__22 = FUnits[Index];
                    var call__23 = Event.FDamageTypes[call__22];
                    ShowDamageIcon(X, Y + 8, SD, call__21, (TDamageType)call__23);
                    var call__24 = FUnits[Index];
                    var call__25 = Event.FArmyView.GetUnit(call__24);
                    var call__26 = call__25.GetHitPoints();
                    var call__27 = FUnits[Index];
                    var call__28 = Event.FDamageValues[call__27];
                    var call__29 = FUnits[Index];
                    var call__30 = Event.FArmyView.GetUnit(call__29);
                    var call__31 = call__30.GetHits();
                    var call__32 = HitPointsToHitPcnt((sbyte)(byte)(call__26 - call__28), call__31);
                    ShowHitPcnt(X, Y, SD, call__32);
                }
                else
                {
                    var call__33 = FUnits[Index];
                    var call__34 = Event.FArmyView.GetUnit(call__33);
                    var call__35 = call__34.GetHits();
                    var call__36 = FUnits[Index];
                    var call__37 = Event.FArmyView.GetUnit(call__36);
                    var call__38 = call__37.GetHitPoints();
                    var call__39 = HitPointsToHitPcnt(call__38, call__35);
                    ShowHitPcnt(X, Y, SD, call__39);
                }
            }
            else
            {
                var call__40 = FUnits[Index];
                var call__41 = Event.FArmyView.GetUnit(call__40);
                call__41.Show(X, Y, SD);
            }
        }

        // AoWArmyEventDlg.pas:153
        public void ClearEvent()
        {
            TArmyEventLog Event = default;
            TExecuteDoneEvent Notify = default;
            int I = default;
            int Resource = default;
            General.Manager.OverlayPriority = 0;
            {
                I = 0;
                int end__42 = FUnits.Count - 1;
                for (; I <= end__42; I += 1)
                {
                    var call__43 = FUnits[I];
                    var call__44 = FEvent.FArmyView.GetUnit(call__43);
                    var call__45 = call__44.GetUnitGFXResourceIndex();
                    Resource = call__45;
                    if (Resource != -1)
                    {
                        var call__46 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Resource);
                        call__46.Release();
                    }
                }
            }

            Notify = FOnClose;
            Event = FEvent;
            FOnClose = null;
            FEvent = null;
            if (Notify != null)
            {
                Notify(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWArmyEventDlg.pas:171
        public void ArmyEventDlgCreate()
        {
            var call__47 = new TByteList();
            FUnits = call__47;
        }

        // AoWArmyEventDlg.pas:176
        public void ArmyEventDlgDestroy()
        {
            FUnits.Free();
        }

        // AoWArmyEventDlg.pas:181
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            ArmyEventWin.Visible = false;
        }

        // AoWArmyEventDlg.pas:187
        public void UnitPnlDrawBack(TAoWComponent Sender, TSurfaceDesc SD)
        {
            if (FFrame < 1000)
            {
                FFrame += 1;
            }

            DrawUnits(ArmyEventWin.WinLeft + 203, ArmyEventWin.WinTop + 100, SD);
        }

        // AoWArmyEventDlg.pas:193
        public void UnitPnlUpdate()
        {
            UnitPnl.FDrawn = false;
            ArmyEventWin.FDrawn = false;
        }

        // AoWArmyEventDlg.pas:198
        public void ArmyEventWinHide()
        {
            ClearEvent();
        }

        // AoWArmyEventDlg.pas:203
        public void ArmyEventWinKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27 || Key == 32)
            {
                ArmyEventWin.Visible = false;
            }
        }
    }

    public static partial class AoWArmyEventDlg
    {
        // AoWArmyEventDlg.pas:84
        public static void ShowArmyEvent(TArmyEventDlg Dialog, TArmyEventLog Event, TExecuteDoneEvent Done)
        {
            TArmyEventLog Log = default;
            TArmyView Army = default;
            int I = default;
            int Image = default;
            Assert(Dialog.FEvent == null, (AnsiString)"", "AoWArmyEventDlg.pas", 92);
            Dialog.FOnClose = Done;
            Dialog.FEvent = Event;
            Dialog.FEvent.AddRef();
            Log = Dialog.FEvent;
            Dialog.DamagePic.CurrentImage = Log.FArmyIcon;
            var call__49 = Log.FArmyMessages.Text;
            Dialog.EventText.Text = call__49;
            Army = Log.FArmyView;
            Dialog.FUnits.Clear();
            {
                I = 0;
                int end__50 = Army.GetCount() - 1;
                for (; I <= end__50; I += 1)
                {
                    var call__51 = Army.GetUnitConcealedForPlayer(I, AoWHSMap.FSeatedPlayer);
                    bool bool__52 = !call__51;
                    if (!bool__52)
                    {
                        var call__53 = Log.FDamageValues[I];
                        bool__52 = call__53 != 0;
                    }

                    if (bool__52)
                    {
                        Dialog.FUnits.Add((byte)I);
                        var call__54 = Log.FArmyView.GetUnit(I);
                        var call__55 = call__54.GetUnitGFXResourceIndex();
                        Image = call__55;
                        if (Image != -1)
                        {
                            var call__56 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Image);
                            call__56.AddRef();
                        }
                    }
                }
            }

            Dialog.ArmyEventWin.Visible = true;
            Dialog.FFrame = 0;
        }
    }
}
